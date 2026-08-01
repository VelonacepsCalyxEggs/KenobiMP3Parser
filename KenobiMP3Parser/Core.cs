using KenobiMp3Parser.Classes;
using KenobiMp3Parser.Constants;
using KenobiMp3Parser.Exceptions;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO.Hashing;
using System.Text;
using static KenobiMp3Parser.Constants.Spans;
using static KenobiMp3Parser.Constants.Uints;
using static KenobiMp3Parser.Constants.Tables;
namespace KenobiMp3Parser
{
    public static class Mp3MetadataParser
    {
        private const int MaxFailedFrames = 1024;
        private const int MaxConsecutiveFailedFrames = 6;
        private const int MaxStackAllocSize = 2048;

        private static int GetMp3Bitrate(int bitrateIndex, int version, int layer)
        {
            if (bitrateIndex <= 0 || bitrateIndex >= 15) return -1;

            int column = -1;
            if (version == 0b11) // MPEG 1
            {
                if (layer == 0b11) column = 0; // Layer I
                if (layer == 0b10) column = 1; // Layer II
                if (layer == 0b01) column = 2; // Layer III
            }
            else // MPEG 2 || 2.5
            {
                if (layer == 0b11) column = 3; // Layer I
                else column = 4;            // Layer II & III
            }

            return Mp3BitrateTable[bitrateIndex, column];
        }

        private static int GetMp3SamplesPerFrame(int version, int layer)
        {
            int column = -1;
            int row = -1;
            if (layer == 0b11) column = 0; // Layer I
            if (layer == 0b10) column = 1; // Layer II
            if (layer == 0b01) column = 2; // Layer III
            if (version == 0b11) // MPEG 1
            {
                row = 0;
            }
            else // MPEG 2 || 2.5
            {
                row = 1;
            }

            return Mp3SamplesPerFrameTable[row, column];
        }
        private static int GetMp3SampleRate(int sampleRateIndex, int version)
        {
            if (sampleRateIndex < 0 || sampleRateIndex >= 3) return -1;
            int column = -1;
            if (version == 0b11) // MPEG 1
                column = 0;
            else if (version == 0b10)
                column = 1;
            else if (version == 0b00)
                column = 2;
            else
                column = 3;
            return Mp3SampleRateTable[sampleRateIndex, column];
        }
        // TODO: Make this a TryReadMp3Frame so it doesn't throw exceptions, as they might be expensive.
        // Instead, I think adding a byte enum into Mp3Frame to indicate it's status (i.e. why it failed) is a good idea
        // and using BinaryPrimitives to convert the header into a uint is most likely faster than doing .toArray on a Span<Byte>
        private static Mp3Frame ReadMp3Frame(Stream stream)
        {
            Span<byte> header = stackalloc byte[4];
            stream.ReadExactly(header);
            //Console.WriteLine($"Full Header: {BitConverter.ToString(header)}");
            int vBits = header[1] >> 3 & 0b11;
            int lBits = header[1] >> 1 & 0b11;
            int crcProtect = header[1] & 0b01;
            int bitrateIndex = header[2] >> 4 & 0b1111;
            int sampleBits = header[2] >> 2 & 0b11;
            int pBit = header[2] >> 1 & 0b01;
            if (vBits == 0b01)
                throw new InvalidHeaderException(header.ToArray(), stream.Position, "Version bits invalid.");
            if (lBits == 0b00)
                throw new InvalidHeaderException(header.ToArray(), stream.Position, "Layer bits invalid.");
            if (bitrateIndex == 0b1111 || bitrateIndex == 0b0000)
                throw new InvalidHeaderException(header.ToArray(), stream.Position, "Bitrate index invalid.");
            if (sampleBits == 0b11)
                throw new InvalidHeaderException(header.ToArray(), stream.Position, "Sample bits invalid.");
            //if (crcProtect == 0b00)
            //Console.WriteLine("Has CRC");
            Mp3Version version = vBits switch
            {
                0b00 => Mp3Version.MPEGVersion25,
                0b01 => Mp3Version.reserved,
                0b10 => Mp3Version.MPEGVersion2,
                0b11 => Mp3Version.MPEGVersion1,
                _ => throw new NotSupportedException()
            };
            Mp3Layer layer = lBits switch
            {
                0b00 => Mp3Layer.reserved,
                0b01 => Mp3Layer.Layer3,
                0b10 => Mp3Layer.Layer2,
                0b11 => Mp3Layer.Layer1,
                _ => throw new NotSupportedException()
            };
            int bitrate = GetMp3Bitrate(bitrateIndex, vBits, lBits);
            int sampleRate = GetMp3SampleRate(sampleBits, vBits);
            int samplesPerFrame = GetMp3SamplesPerFrame(vBits, lBits);
            int frameSize;
            int padding = (pBit == 1) ? 1 : 0;

            if (lBits == 0b11) // Layer I
            {
                frameSize = (12 * bitrate / sampleRate + padding) * 4;
            }
            else // Layer II и Layer III
            {
                frameSize = ((samplesPerFrame / 8) * bitrate / sampleRate) + padding;
            }
            //stream.Position -= 4;
            return new Mp3Frame
            {
                Version = version,
                Layer = layer,
                HasCrc = crcProtect == 0,
                Padding = padding,
                SamplesPerFrame = samplesPerFrame,
                SampleRate = sampleRate,
                Bitrate = bitrate,
                FrameSize = frameSize,
            };
        }
        private static int ReadSynchsafeInt32(byte[] bytes)
        {
            if (bytes.Length < 4) throw new ArgumentException($"Needs four or more bytes, passed {bytes.Length}");

            return (bytes[0] << 21) | // 7 * 3
                    (bytes[1] << 14) | // 7 * 2
                    (bytes[2] << 7) | // 7 * 1
                    bytes[3];
        }
        private static int ReadSynchsafeInt32(Span<byte> bytes)
        {
            if (bytes.Length < 4) throw new ArgumentException($"Needs four or more bytes, passed {bytes.Length}");

            return (bytes[0] << 21) | // 7 * 3
                    (bytes[1] << 14) | // 7 * 2
                    (bytes[2] << 7) | // 7 * 1
                    bytes[3];
        }
        // Split this up into separate functions later, experimenting purposes only for now.
        private static Mp3Header ReadMp3Header(Stream stream, bool doMp3Check = true)
        {
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif

            if (doMp3Check && !CheckIfMp3(stream))
                throw new NotSupportedException("Cannot parse a non MP3 file with an MP3 parser.");
#if DEBUG
            sw.Stop();
            LogStopWatch(sw, "Mp3Check");
#endif
#if DEBUG
            sw.Restart();
#endif
            //Console.WriteLine("ID3 Header: " + Convert.ToHexString(id3Header));
            Span<byte> vrf = stackalloc byte[3]; // version, revision, flags.
            stream.ReadExactly(vrf);
            byte vByte = vrf[0];
            byte rByte = vrf[1];
            //Console.WriteLine($"ID3 Version: {vByte}");
            //Console.WriteLine($"ID3 Revision: {rByte}");
            byte flags = vrf[2];
            // Later act upon these flags accordingly.
            int isExperimental = flags >> 3 & 0b01;
            int hasExtendedHeader = flags >> 2 & 0b01;
            int unsynchronisationUsed = flags >> 1 & 0b01;
#if DEBUG
            sw.Stop();
            LogStopWatch(sw, "Flags");
#endif
#if DEBUG
            sw.Restart();
#endif
            //Console.WriteLine($"Is Experimental: {isExperimental}");
            //Console.WriteLine($"Has extended header: {hasExtendedHeader}");
            //Console.WriteLine($"Unsynchronised: {unsynchronisationUsed}"); // Later make this remove FF-00 everywhere.
            Span<byte> tagSize = stackalloc byte[4]; // synchsafe integer, which means the last bit (7) is always 0.
            stream.ReadExactly(tagSize);

            int framesSize = ReadSynchsafeInt32(tagSize);
            long endPosition = stream.Position + framesSize;
            // Read frames.
#if DEBUG
            sw.Stop();
            LogStopWatch(sw, "TagSize");
#endif
#if DEBUG
            sw.Restart();
#endif
            var metadata = new IDv2Metadata();
            //Console.WriteLine($"All size: {framesSize}");
            if (vByte == 0b100 || vByte == 0b011)
            {
                //ParseIDv234TagsUint(stream, metadata, endPosition, vByte);
                ParseIDv234Tags(stream, metadata, endPosition, vByte);
            }
            else
            {
                //ParseIDv22TagsUint(stream, metadata, endPosition, vByte);
                ParseIDv22Tags(stream, metadata, endPosition, vByte);
            }
#if DEBUG
            sw.Stop();
            LogStopWatch(sw, "TagParse");
#endif

            return new Mp3Header
            {
                Metadata = metadata,
                HeaderSize = framesSize + 10 // + 10 bytes in the beginning.
            };
        }
        private static void ParseIDv22Tags(Stream stream, IDv2Metadata metadata, long endPosition, int vByte)
        {
            while (stream.Position < endPosition)
            {
                Span<byte> tagId = stackalloc byte[3];
                stream.ReadExactly(tagId);
                if (tagId.SequenceEqual(ZERO_SPAN_3))
                {
                    int b = stream.ReadByte();
                    if (b == 0)
                    {
                        //Console.WriteLine("Padding, ending IDv2 parsing...");
                        break;
                    }
                    else stream.Position -= 4;
                }
                //string strTagId = Encoding.ASCII.GetString(tagId);
                int frameSize = GetHeaderFrameDataSize(stream, vByte);
                //Console.WriteLine($"Frame size: {frameSize}");

                // Compare the 3‑byte span against the new ReadOnlySpan<byte> constants
                if (tagId.SequenceEqual(TT2_MAGIC_SPAN))
                    metadata.Title = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TP1_MAGIC_SPAN))
                    metadata.Artists = GetDataFromTextHeaderFrame(stream, frameSize, tagId, false).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                else if (tagId.SequenceEqual(TP2_MAGIC_SPAN))
                    metadata.Artists2 = GetDataFromTextHeaderFrame(stream, frameSize, tagId, false).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                else if (tagId.SequenceEqual(TAL_MAGIC_SPAN))
                    metadata.Album = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TEN_MAGIC_SPAN))
                    metadata.EncodedBy = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TCR_MAGIC_SPAN))
                    metadata.Copyright = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TP3_MAGIC_SPAN))
                    metadata.Conductor = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TRK_MAGIC_SPAN))
                    metadata.TrackNumber = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TPA_MAGIC_SPAN))
                    metadata.DiscNumber = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TCO_MAGIC_SPAN))
                    metadata.Genre = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TRC_MAGIC_SPAN))
                    metadata.ISRC = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TCM_MAGIC_SPAN))
                    metadata.Composer = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TYE_MAGIC_SPAN))
                    metadata.Year = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TSS_MAGIC_SPAN))
                    metadata.EncoderSettings = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TLA_MAGIC_SPAN))
                    metadata.Language = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TPB_MAGIC_SPAN))
                    metadata.Publisher = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TBP_MAGIC_SPAN))
                    metadata.BPM = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TKE_MAGIC_SPAN))
                    metadata.Key = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else
                    stream.Position += frameSize;
            }
        }

        private static void ParseIDv234Tags(Stream stream, IDv2Metadata metadata, long endPosition, int vByte)
        {
            while (stream.Position < endPosition)
            {
                Span<byte> tagId = stackalloc byte[4];
                stream.ReadExactly(tagId);
                if (tagId.SequenceEqual(ZERO_SPAN_4))
                {
                    int b = stream.ReadByte();
                    if (b == 0)
                    {
                        //Console.WriteLine("Padding, ending IDv2 parsing...");
                        break;
                    }
                    else stream.Position -= 5;
                }
                //string strTagId = Encoding.ASCII.GetString(tagId);
                //Console.WriteLine(strTagId);
                int frameSize = GetHeaderFrameDataSize(stream, vByte);
                // Console.WriteLine($"Frame size: {frameSize}");
                stream.Position += 2; // Skip flags for now.

                // Compare the 4‑byte span against the new ReadOnlySpan<byte> constants
                if (tagId.SequenceEqual(TIT2_MAGIC_SPAN))
                    metadata.Title = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TPE1_MAGIC_SPAN))
                    metadata.Artists = GetDataFromTextHeaderFrame(stream, frameSize, tagId, false).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                else if (tagId.SequenceEqual(TPE2_MAGIC_SPAN))
                    metadata.Artists2 = GetDataFromTextHeaderFrame(stream, frameSize, tagId, false).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                else if (tagId.SequenceEqual(TRCK_MAGIC_SPAN))
                    metadata.TrackNumber = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TALB_MAGIC_SPAN))
                    metadata.Album = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TCON_MAGIC_SPAN))
                    metadata.Genre = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TYER_MAGIC_SPAN))
                    metadata.Year = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TDRC_MAGIC_SPAN))
                    metadata.Year = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TPUB_MAGIC_SPAN))
                    metadata.Publisher = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TSRC_MAGIC_SPAN))
                    metadata.ISRC = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TCOP_MAGIC_SPAN))
                    metadata.Copyright = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TPE3_MAGIC_SPAN))
                    metadata.Conductor = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TENC_MAGIC_SPAN))
                    metadata.EncodedBy = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TPOS_MAGIC_SPAN))
                    metadata.DiscNumber = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TCOM_MAGIC_SPAN))
                    metadata.Composer = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TSSE_MAGIC_SPAN))
                    metadata.EncoderSettings = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TLAN_MAGIC_SPAN))
                    metadata.Language = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TBPM_MAGIC_SPAN))
                    metadata.BPM = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else if (tagId.SequenceEqual(TKEY_MAGIC_SPAN))
                    metadata.Key = GetDataFromTextHeaderFrame(stream, frameSize, tagId);
                else
                    stream.Position += frameSize;
            }
        }
        private static void ParseIDv22TagsUint(Stream stream, IDv2Metadata metadata, long endPosition, int vByte)
        {
            while (stream.Position < endPosition)
            {
                Span<byte> tagIdBuffer = stackalloc byte[4];
                stream.ReadExactly(tagIdBuffer.Slice(1, 3));
                uint tagId = BinaryPrimitives.ReadUInt32BigEndian(tagIdBuffer);

                if (tagId == ZERO_UINT)
                    break;   // padding reached

                int frameSize = GetHeaderFrameDataSize(stream, vByte);

                switch (tagId)
                {
                    case TT2_MAGIC_UINT:
                        metadata.Title = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TP1_MAGIC_UINT:
                        metadata.Artists = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId, false)
                                            .Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case TP2_MAGIC_UINT:
                        metadata.Artists2 = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId, false)
                                             .Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case TAL_MAGIC_UINT:
                        metadata.Album = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TEN_MAGIC_UINT:
                        metadata.EncodedBy = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TCR_MAGIC_UINT:
                        metadata.Copyright = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TP3_MAGIC_UINT:
                        metadata.Conductor = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TRK_MAGIC_UINT:
                        metadata.TrackNumber = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TPA_MAGIC_UINT:
                        metadata.DiscNumber = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TCO_MAGIC_UINT:
                        metadata.Genre = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TRC_MAGIC_UINT:
                        metadata.ISRC = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TCM_MAGIC_UINT:
                        metadata.Composer = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TYE_MAGIC_UINT:
                        metadata.Year = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TSS_MAGIC_UINT:
                        metadata.EncoderSettings = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TLA_MAGIC_UINT:
                        metadata.Language = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TPB_MAGIC_UINT:
                        metadata.Publisher = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TBP_MAGIC_UINT:
                        metadata.BPM = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TKE_MAGIC_UINT:
                        metadata.Key = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    default:
                        stream.Position += frameSize;
                        break;
                }
            }
        }

        private static void ParseIDv234TagsUint(Stream stream, IDv2Metadata metadata, long endPosition, int vByte)
        {
            while (stream.Position < endPosition)
            {
                Span<byte> tagIdBuffer = stackalloc byte[4];
                stream.ReadExactly(tagIdBuffer);
                uint tagId = BinaryPrimitives.ReadUInt32BigEndian(tagIdBuffer);

                if (tagId == ZERO_UINT)
                    break;

                int frameSize = GetHeaderFrameDataSize(stream, vByte);
                stream.Position += 2; // Skip flags

                switch (tagId)
                {
                    case TIT2_MAGIC_UINT:
                        metadata.Title = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TPE1_MAGIC_UINT:
                        metadata.Artists = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId, false)
                                            .Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case TPE2_MAGIC_UINT:
                        metadata.Artists2 = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId, false)
                                             .Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case TRCK_MAGIC_UINT:
                        metadata.TrackNumber = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TALB_MAGIC_UINT:
                        metadata.Album = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TCON_MAGIC_UINT:
                        metadata.Genre = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TYER_MAGIC_UINT:
                        metadata.Year = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TDRC_MAGIC_UINT:
                        metadata.Year = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TPUB_MAGIC_UINT:
                        metadata.Publisher = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TSRC_MAGIC_UINT:
                        metadata.ISRC = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TCOP_MAGIC_UINT:
                        metadata.Copyright = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TPE3_MAGIC_UINT:
                        metadata.Conductor = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TENC_MAGIC_UINT:
                        metadata.EncodedBy = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TPOS_MAGIC_UINT:
                        metadata.DiscNumber = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TCOM_MAGIC_UINT:
                        metadata.Composer = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TSSE_MAGIC_UINT:
                        metadata.EncoderSettings = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TLAN_MAGIC_UINT:
                        metadata.Language = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TBPM_MAGIC_UINT:
                        metadata.BPM = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    case TKEY_MAGIC_UINT:
                        metadata.Key = GetDataFromTextHeaderFrameUint(stream, frameSize, tagId);
                        break;
                    default:
                        stream.Position += frameSize;
                        break;
                }
            }
        }
        private static int GetHeaderFrameDataSize(Stream stream, int id3Version)
        {
            if (id3Version == 0b100)
            {
                Span<byte> dataSize = stackalloc byte[4];
                stream.ReadExactly(dataSize);
                return ReadSynchsafeInt32(dataSize);
            }
            else if (id3Version == 0b011)
            {
                Span<byte> dataSize = stackalloc byte[4];
                stream.ReadExactly(dataSize);
                return BinaryPrimitives.ReadInt32BigEndian(dataSize);
            }
            else
            {
                Span<byte> dataSize = stackalloc byte[3];
                stream.ReadExactly(dataSize);
                return (dataSize[0] << 16) | (dataSize[1] << 8) | dataSize[2];
            }
        }
        private static string GetDataFromTextHeaderFrame(Stream stream, int frameSize, ReadOnlySpan<byte> tagId, bool removeNullTerminators = true)
        {
            if (frameSize <= 0)
                return string.Empty;

            if (frameSize > 2048)
            {
                var stringTag = Encoding.ASCII.GetString(tagId);
                throw new ArgumentOutOfRangeException($"Someone is trying to store War and Peace in the MP3 tags... One text frame was more than 2048 bytes. Tag in question: {stringTag}");
            }

            // If frame size is small, allocate on stack. If large, allocate on the heap.
            Span<byte> buffer = frameSize <= MaxStackAllocSize
                ? stackalloc byte[frameSize]
                : new byte[frameSize];

            stream.ReadExactly(buffer);

            byte encodingByte = buffer[0];
            ReadOnlySpan<byte> textData = buffer.Slice(1);

            string result = encodingByte switch
            {
                0x00 => // ISO-8859-1
                    Encoding.GetEncoding("ISO-8859-1").GetString(textData),

                0x01 => // UTF-16 with BOM (Handles both Big and Little Endian)
                    ParseUtf16WithBom(textData),

                0x02 => // UTF-16BE without BOM
                    Encoding.BigEndianUnicode.GetString(textData),

                0x03 => // UTF-8
                    Encoding.UTF8.GetString(textData),

                _ =>    // Fallback
                    Encoding.ASCII.GetString(textData)
            };

            // check if \0 exists and replace.
            if (removeNullTerminators && result.Contains('\0')) // TODO: This should be done on the Span instead of a string to optimize.
            {
                result = result.Replace("\0", string.Empty);
            }

            return result.Trim();
        }
        private static string GetDataFromTextHeaderFrameUint(Stream stream, int frameSize, uint tagId, bool removeNullTerminators = true)
        {
            if (frameSize <= 0)
                return string.Empty;

            if (frameSize > 2048)
            {
                throw new ArgumentOutOfRangeException($"Someone is trying to store War and Peace in the MP3 tags... One text frame was more than 2048 bytes.");
            }

            // If frame size is small, allocate on stack. If large, allocate on the heap.
            Span<byte> buffer = frameSize <= MaxStackAllocSize
                ? stackalloc byte[frameSize]
                : new byte[frameSize];

            stream.ReadExactly(buffer);

            byte encodingByte = buffer[0];
            ReadOnlySpan<byte> textData = buffer.Slice(1);

            string result = encodingByte switch
            {
                0x00 => // ISO-8859-1
                    Encoding.GetEncoding("ISO-8859-1").GetString(textData),

                0x01 => // UTF-16 with BOM (Handles both Big and Little Endian)
                    ParseUtf16WithBom(textData),

                0x02 => // UTF-16BE without BOM
                    Encoding.BigEndianUnicode.GetString(textData),

                0x03 => // UTF-8
                    Encoding.UTF8.GetString(textData),

                _ =>    // Fallback
                    Encoding.ASCII.GetString(textData)
            };

            // check if \0 exists and replace.
            if (removeNullTerminators && result.Contains('\0')) // TODO: This should be done on the Span instead of a string to optimize.
            {
                result = result.Replace("\0", string.Empty);
            }

            return result.Trim();
        }
        private static string ParseUtf16WithBom(ReadOnlySpan<byte> data)
        {
            if (data.Length < 2)
                return string.Empty;

            // Fast check for Big-Endian BOM (0xFE, 0xFF)
            if (data[0] == 0xFE && data[1] == 0xFF)
            {
                return Encoding.BigEndianUnicode.GetString(data.Slice(2));
            }

            // Little-Endian BOM (0xFF, 0xFE)
            return Encoding.Unicode.GetString(data.Slice(2));
        }
        /// <summary>
        /// Parses the MP3 file, requires a seekable file stream to be passed into it.
        /// Recommend to use a FileStream with sequential read + wrap it in a buffered stream for better performance.
        /// </summary>
        /// <param name="stream"></param>
        /// <exception cref="InvalidDataException"
        /// <exception cref="ArgumentException"
        /// <exception cref="EndOfStreamException"
        /// <exception cref="NotSupportedException"
        /// <returns></returns>
        public static Mp3FileInfo ParseMP3File(Stream stream, bool doMp3Check = true)
        {
            ParserState state = new ParserState();
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            if (!doMp3Check)
                stream.Position = 3;
            Mp3Header header = ReadMp3Header(stream, doMp3Check);
#if DEBUG
            sw.Stop();
            LogStopWatch(sw, "Header");
#endif
            stream.Position = header.HeaderSize; // all frames + 10 header bytes
            var xxHash = new XxHash64();
            int b;
            // To improve performance, it's probably better to read a bunch of bytes and then iterate over them.
#if DEBUG
            sw.Restart();
#endif
            while ((b = stream.ReadByte()) != -1)
            {
                if (b == 0xFF)
                {
                    int next = stream.ReadByte();
                    if (next == -1) break;
                    if ((next & 0xE0) == 0xE0)
                    {
                        long startPos = stream.Position - 2;
                        try
                        {
                            stream.Position = startPos;
                            // Check if we can read the header.
                            if (startPos + 4 > stream.Length)
                            {
                                break;
                            }
                            Mp3Frame mp3Frame = ReadMp3Frame(stream);
                            if (state.HasPrevFrame && mp3Frame.Version != state.PrevFrame.Version)
                            {
                                // Skip possibly garbage frame and keep scanning.
                                stream.Position = startPos + 1; // move one byte forward
                                state.RegisterFrameError(true);
                                if (state.FailedFramesConsecutive > MaxConsecutiveFailedFrames)
                                    throw new InvalidDataException($"Too many consecutive invalid frames. (>{MaxConsecutiveFailedFrames}");
                                state.PreviousFailed = true;
                                continue;
                            }

                            // Check if the frame size is actually available in the stream
                            // to prevent EndOfStreamException, kind of a sanity check.
                            if (startPos + mp3Frame.FrameSize > stream.Length)
                            {
                                break; // End of valid audio data
                            }
                            // I think this will support VBR files, but it needs to identify them for accurate bitrate (either calculate average or just set 0).
                            // Read the actual frame data for hashing
                            stream.Position = startPos;
                            if (mp3Frame.FrameSize > 1440)
                                throw new InvalidDataException("The entire discography of Mozart was stored in one MP3 frame.");
                            Span<byte> frameBuffer = new byte[mp3Frame.FrameSize];
                            stream.ReadExactly(frameBuffer);

                            xxHash.Append(frameBuffer);
                            state.RegisterValidFrame(mp3Frame);
                        }
                        // I realize that I might want to get rid of this looped catch block and make it state managed instead. Will see if it actually is better if it were the case.
                        catch (ArgumentException)
                        {
                            // If a false sync, just move one byte forward from the start
                            stream.Position = startPos + 1;
                            state.RegisterFrameError(false);
                            if (state.FailedFrames == MaxFailedFrames || state.FailedFramesConsecutive == MaxConsecutiveFailedFrames)
                            {
                                Console.WriteLine("All frames failed.");
                                throw;
                            }
                        }
                        catch (InvalidHeaderException)
                        {
                            stream.Position = startPos + 1;
                            state.RegisterFrameError(false);
                            if (state.FailedFrames == MaxFailedFrames || state.FailedFramesConsecutive == MaxConsecutiveFailedFrames)
                            {
                                Console.WriteLine("All frames failed.");
                                throw;
                            }
                        }
                        catch (EndOfStreamException ex)
                        {
                            Console.WriteLine($"Fake FF FE frame at the end of the file.: {ex}");
                            throw;
                        }
                        catch (InvalidDataException)
                        {
                            Console.WriteLine("Critical error.");
                            throw;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Unhandled exception occured while reading file: {ex}");
                            throw;
                        }
                    }
                }
            }
#if DEBUG
            sw.Stop();
            LogStopWatch(sw, "MainLoop");
#endif
            //byte[] hashBytes = md5.GetHashAndReset();
            //string hashString = Convert.ToHexString(hashBytes);
#if DEBUG
            sw.Restart();
#endif
            string hashString = xxHash.GetCurrentHashAsUInt64().ToString();
            xxHash.Reset();
            // Read IDv1 only if there is missing essential metadata (like Title, Artists or Album name)
            if (!header.Metadata.IsFilled())
            {
                ReadIDv1TagsAndReplaceMissingData(stream, header);
            }
#if DEBUG
            sw.Stop();
            LogStopWatch(sw, "Finalize");
#endif
            return new Mp3FileInfo()
            {
                Hash = hashString,
                Header = header,
                FrameData = state.PrevFrame,
                Duration = TimeSpan.FromSeconds(state.FramesRead * (double)state.PrevFrame.SamplesPerFrame / state.PrevFrame.SampleRate)
            };
        }
        /// <summary>
        /// Used to detect if the given stream is an MP3 file by reading the header (magic bytes).
        /// If you are going to pass it into ParseMP3File later, make sure to set jumpTo to 0, because it expects the stream to be at position 0.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="jumpTo"></param>
        /// <returns></returns>
        public static bool CheckIfMp3(Stream stream, int jumpTo)
        {
            Span<byte> id3Header = stackalloc byte[3];
            stream.ReadExactly(id3Header);

            stream.Position = jumpTo;

            return id3Header.SequenceEqual(ID3_MAGIC_SPAN);
        }
        /// <summary>
        /// Used to detect if the given stream is an MP3 file by reading the header (magic bytes).
        /// Does not return the stream to the start position.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static bool CheckIfMp3(Stream stream)
        {
            Span<byte> id3Header = stackalloc byte[3];
            stream.ReadExactly(id3Header);
            return id3Header.SequenceEqual(ID3_MAGIC_SPAN);
        }
        /// <summary>
        /// Modifies the header object with new values read from IDv1Tags (if they were previously not set)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="header"></param>
        private static void ReadIDv1TagsAndReplaceMissingData(Stream stream, Mp3Header header)
        {
            stream.Position = stream.Length - 128;
            Span<byte> tagBytes = stackalloc byte[3];
            stream.ReadExactly(tagBytes);
            if (tagBytes.SequenceEqual(TAG_MAGIC_SPAN))
            {
                Span<byte> titleBytes = new byte[30];
                stream.ReadExactly(titleBytes);
                string title = Encoding.ASCII.GetString(titleBytes).TrimEnd('\0', ' ');
                if (title.Length > 0)
                    header.Metadata.Title = title;
                byte[] artistBytes = new byte[30];
                stream.ReadExactly(artistBytes);
                string[] artists = Encoding.ASCII.GetString(artistBytes).TrimEnd('\0', ' ').Split('\0', StringSplitOptions.RemoveEmptyEntries);
                if (artists.Length > 0)
                    header.Metadata.Artists = artists;
                byte[] albumBytes = new byte[30];
                stream.ReadExactly(albumBytes);
                string album = Encoding.ASCII.GetString(albumBytes).TrimEnd('\0', ' ');
                if (album.Length > 0)
                    header.Metadata.Album = album;
                byte[] yearBytes = new byte[4];
                stream.ReadExactly(yearBytes);
                string year = Encoding.ASCII.GetString(yearBytes).TrimEnd('\0', ' ');
                if (year.Length > 0)
                    header.Metadata.Year = year;
                //byte[] commentBytes = new byte[30];
                //stream.ReadExactly(commentBytes);
                //Console.WriteLine(Encoding.ASCII.GetString(commentBytes).TrimEnd('\0', ' '));
                //byte[] genreByte = new byte[1];
                //stream.ReadExactly(genreByte);
                //Console.WriteLine(Encoding.ASCII.GetString(genreByte));
            }
        }

#if DEBUG
        private static void LogStopWatch(Stopwatch sw, string label)
        {
            Console.WriteLine($"{label}: {sw.ElapsedTicks} ticks");
        }
#endif
    }
    }

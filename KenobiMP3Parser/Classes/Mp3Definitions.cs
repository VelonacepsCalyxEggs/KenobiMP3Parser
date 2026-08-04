using System.Text;

namespace KenobiMp3Parser.Classes
{
    public readonly record struct Mp3Frame
    {
        public Mp3Layer Layer { get; init; }
        public Mp3Version Version { get; init; }
        public bool HasCrc { get; init; }
        public int Padding { get; init; }
        public int Bitrate { get; init; }
        public int SampleRate { get; init; }
        public int SamplesPerFrame { get; init; }
        public int FrameSize { get; init; }
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("=== Mp3FrameData ===");
            sb.AppendLine($"Bitrate: {Bitrate}");
            sb.AppendLine($"Sample Rate: {SampleRate}");
            sb.AppendLine($"Sample/Frame: {SamplesPerFrame}");
            sb.AppendLine($"Frame size: {FrameSize}");
            sb.AppendLine($"MP3 Frame Version: {Version}");
            sb.AppendLine($"MP3 Frame Layer: {Layer}");
            return sb.ToString();
        }
    }

    public enum FrameStatus : byte
    {
        SUCCESS, INVALID_VERSION, INVALID_LAYER, INVALID_BITRATE, INVALID_SAMPLE
    }

    public readonly record struct Mp3Header
    {
        public IDv2Metadata Metadata { get; init; }
        public int HeaderSize { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("=== Mp3Header ===");
            sb.AppendLine($"Header size: {HeaderSize} bytes");
            if (Metadata != null)
            {
                sb.AppendLine("Metadata:");
                // Show a compact summary of the most important fields
                var summary = Metadata.GetSummary();
                if (!string.IsNullOrEmpty(summary))
                    sb.AppendLine($"  {summary}");
                else
                    sb.AppendLine("  (empty)");
            }
            else
            {
                sb.AppendLine("Metadata: (null)");
            }
            return sb.ToString();
        }
    }

    public record class IDv2Metadata
    {
        public bool IsFilled() => Title != null && Artists != null && Album != null;
        public string? Title { get; set; }
        public string[]? Artists { get; set; }
        public string[]? Artists2 { get; set; }
        public string? Album { get; set; }
        public string? EncodedBy { get; set; }
        public string? Copyright { get; set; }
        public string? Conductor { get; set; }
        public string? TrackNumber { get; set; }
        public string? DiscNumber { get; set; }
        public string? Genre { get; set; }
        public string? ISRC { get; set; }
        public string? Composer { get; set; }
        public string? Year { get; set; }
        public string? Other { get; set; }
        public string? EncoderSettings { get; set; }
        public string? Language { get; set; }
        public string? Publisher { get; set; }
        public string? BPM { get; set; }
        public string? Key { get; set; }

        /// <summary>
        /// Returns a compact, multi-line string containing all non‑null fields.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== IDv2Metadata ===");

            // Use reflection or manual checks – manual is safer and faster.
            AppendIfNotNull(sb, nameof(Title), Title);
            AppendIfNotNull(sb, nameof(Album), Album);
            AppendIfNotNull(sb, nameof(Artists), Artists != null ? string.Join(", ", Artists) : null);
            AppendIfNotNull(sb, "Artists2", Artists2 != null ? string.Join(", ", Artists2) : null);
            AppendIfNotNull(sb, nameof(EncodedBy), EncodedBy);
            AppendIfNotNull(sb, nameof(Copyright), Copyright);
            AppendIfNotNull(sb, nameof(Conductor), Conductor);
            AppendIfNotNull(sb, nameof(TrackNumber), TrackNumber);
            AppendIfNotNull(sb, nameof(DiscNumber), DiscNumber);
            AppendIfNotNull(sb, nameof(Genre), Genre);
            AppendIfNotNull(sb, nameof(ISRC), ISRC);
            AppendIfNotNull(sb, nameof(Composer), Composer);
            AppendIfNotNull(sb, nameof(Year), Year);
            AppendIfNotNull(sb, nameof(Other), Other);
            AppendIfNotNull(sb, nameof(EncoderSettings), EncoderSettings);
            AppendIfNotNull(sb, nameof(Language), Language);
            AppendIfNotNull(sb, nameof(Publisher), Publisher);
            AppendIfNotNull(sb, nameof(BPM), BPM);
            AppendIfNotNull(sb, nameof(Key), Key);

            // Remove the trailing newline if nothing was added
            if (sb.Length == 0)
                sb.Append("(empty)");
            return sb.ToString();
        }

        private static void AppendIfNotNull(StringBuilder sb, string label, string? value)
        {
            if (!string.IsNullOrEmpty(value))
                sb.AppendLine($"{label}: {value}");
        }

        /// <summary>
        /// Returns a short one‑line summary of the most important fields.
        /// Used by Mp3Header.ToString().
        /// </summary>
        internal string GetSummary()
        {
            var parts = new List<string>();
            if (!string.IsNullOrEmpty(Title)) parts.Add($"Title: {Title}");
            if (!string.IsNullOrEmpty(Album)) parts.Add($"Album: {Album}");
            if (Artists != null && Artists.Length > 0) parts.Add($"Artists: {string.Join(", ", Artists)}");
            if (!string.IsNullOrEmpty(Year)) parts.Add($"Year: {Year}");
            if (!string.IsNullOrEmpty(Genre)) parts.Add($"Genre: {Genre}");
            return string.Join(" | ", parts);
        }
    }
    public readonly record struct Mp3FileInfo
    {
        public string Hash { get; init; }
        public Mp3Header Header { get; init; }
        public Mp3Frame FrameData { get; init; }
        public TimeSpan Duration { get; init; }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("=== Mp3FileInfo ===");
            sb.AppendLine($"Hash: {Hash}");
            sb.Append(Header.ToString());
            sb.Append(FrameData.ToString());
            sb.AppendLine($"Duration: {Duration:hh\\:mm\\:ss\\.ff}");
            return sb.ToString();
        }
    }

    public enum Mp3Layer : byte
    {
        reserved,
        Layer3,
        Layer2,
        Layer1
    }

    public enum Mp3Version : byte
    {
        MPEGVersion25,
        reserved,
        MPEGVersion2,
        MPEGVersion1
    }
}

namespace KenobiMp3Parser.Constants
{
    internal static class Spans
    {
        // Magic stuff, big endian
        public static ReadOnlySpan<byte> ID3_MAGIC_SPAN => "ID3"u8;
        public static ReadOnlySpan<byte> TAG_MAGIC_SPAN => "TAG"u8;
        public static ReadOnlySpan<byte> MP3_SYNC_SIG_SPAN => [0xFF, 0xFB];

        // Metadata Magics
        public static ReadOnlySpan<byte> TIT2_MAGIC_SPAN => "TIT2"u8; // title
        public static ReadOnlySpan<byte> TPE1_MAGIC_SPAN => "TPE1"u8; // artist 1: blue shift
        public static ReadOnlySpan<byte> TPE2_MAGIC_SPAN => "TPE2"u8; // artist 2: electric boogaloo
        public static ReadOnlySpan<byte> TPE3_MAGIC_SPAN => "TPE3"u8; // artists 3: A.K.A Conductor
        public static ReadOnlySpan<byte> TALB_MAGIC_SPAN => "TALB"u8; // album
        public static ReadOnlySpan<byte> TCOP_MAGIC_SPAN => "TCOP"u8; // copyrighted by
        public static ReadOnlySpan<byte> TENC_MAGIC_SPAN => "TENC"u8; // encoded by
        public static ReadOnlySpan<byte> TRCK_MAGIC_SPAN => "TRCK"u8; // track number
        public static ReadOnlySpan<byte> TPOS_MAGIC_SPAN => "TPOS"u8; // disc number
        public static ReadOnlySpan<byte> TCON_MAGIC_SPAN => "TCON"u8; // genre
        public static ReadOnlySpan<byte> TCOM_MAGIC_SPAN => "TCOM"u8; // composer
        public static ReadOnlySpan<byte> TYER_MAGIC_SPAN => "TYER"u8; // year
        public static ReadOnlySpan<byte> TDRC_MAGIC_SPAN => "TDRC"u8; // year but for 2.4
        public static ReadOnlySpan<byte> TXXX_MAGIC_SPAN => "TXXX"u8; // other things...?
        public static ReadOnlySpan<byte> TSSE_MAGIC_SPAN => "TSSE"u8; // encoder settings.
        public static ReadOnlySpan<byte> APIC_MAGIC_SPAN => "APIC"u8; // an image attachment...
        public static ReadOnlySpan<byte> TLAN_MAGIC_SPAN => "TLAN"u8; // language.
        public static ReadOnlySpan<byte> TPUB_MAGIC_SPAN => "TPUB"u8; // Publisher
        public static ReadOnlySpan<byte> TSRC_MAGIC_SPAN => "TSRC"u8; // ISRC
        public static ReadOnlySpan<byte> TBPM_MAGIC_SPAN => "TBPM"u8; // BPM
        public static ReadOnlySpan<byte> TKEY_MAGIC_SPAN => "TKEY"u8; // key
        public static ReadOnlySpan<byte> USLT_MAGIC_SPAN => "USLT"u8; // Unsynced Lyrics
        public static ReadOnlySpan<byte> COMM_MAGIC_SPAN => "COMM"u8; // Commentary...
        public static ReadOnlySpan<byte> POPM_MAGIC_SPAN => "POPM"u8; // Popularimeter
        public static ReadOnlySpan<byte> PRIV_MAGIC_SPAN => "PRIV"u8; // Private frame
        public static ReadOnlySpan<byte> TSOT_MAGIC_SPAN => "TSOT"u8; // Title sort...
        public static ReadOnlySpan<byte> UFID_MAGIC_SPAN => "UFID"u8; // Unique field
        public static ReadOnlySpan<byte> WXXX_MAGIC_SPAN => "WXXX"u8; // I don't even know.
        public static ReadOnlySpan<byte> GEOB_MAGIC_SPAN => "GEOB"u8; // General encapsulated object
        public static ReadOnlySpan<byte> TDAT_MAGIC_SPAN => "TDAT"u8; // Date, of my death presumably
        public static ReadOnlySpan<byte> RGAD_MAGIC_SPAN => "RGAD"u8; // Replay gain adjustment
        public static ReadOnlySpan<byte> TLEN_MAGIC_SPAN => "TLEN"u8; // Tag that contains length...
        public static ReadOnlySpan<byte> TDRL_MAGIC_SPAN => "TDRL"u8; // Release time
        public static ReadOnlySpan<byte> MCDI_MAGIC_SPAN => "MCDI"u8; // CD Table of Contents
        public static ReadOnlySpan<byte> TFLT_MAGIC_SPAN => "TFLT"u8; // File type...
        public static ReadOnlySpan<byte> USER_MAGIC_SPAN => "USER"u8; // Ownership or some shit
        public static ReadOnlySpan<byte> TOPE_MAGIC_SPAN => "TOPE"u8; // Original artists/performer
        public static ReadOnlySpan<byte> TCPM_MAGIC_SPAN => "TCPM"u8; // iTunes compilation marker
        public static ReadOnlySpan<byte> WCOP_MAGIC_SPAN => "WCOP"u8; // URL to copyright information.
        public static ReadOnlySpan<byte> WPUB_MAGIC_SPAN => "WPUB"u8; // url to publisher
        public static ReadOnlySpan<byte> WOAS_MAGIC_SPAN => "WOAS"u8; // url to song's webpage
        public static ReadOnlySpan<byte> TMOO_MAGIC_SPAN => "TMOO"u8;
        public static ReadOnlySpan<byte> TMED_MAGIC_SPAN => "TMED"u8;
        public static ReadOnlySpan<byte> IPLS_MAGIC_SPAN => "IPLS"u8;
        public static ReadOnlySpan<byte> SYLT_MAGIC_SPAN => "SYLT"u8;
        public static ReadOnlySpan<byte> TIT1_MAGIC_SPAN => "TIT1"u8;

        // Metadata Magics (3-letter ID, big-endian)
        public static ReadOnlySpan<byte> BUF_MAGIC_SPAN => "BUF"u8; // Recommended buffer size
        public static ReadOnlySpan<byte> CNT_MAGIC_SPAN => "CNT"u8; // Play counter
        public static ReadOnlySpan<byte> COM_MAGIC_SPAN => "COM"u8; // Comments
        public static ReadOnlySpan<byte> CRA_MAGIC_SPAN => "CRA"u8; // Audio encryption
        public static ReadOnlySpan<byte> CRM_MAGIC_SPAN => "CRM"u8; // Encrypted meta frame
        public static ReadOnlySpan<byte> ETC_MAGIC_SPAN => "ETC"u8; // Event timing codes
        public static ReadOnlySpan<byte> EQU_MAGIC_SPAN => "EQU"u8; // Equalization
        public static ReadOnlySpan<byte> GEO_MAGIC_SPAN => "GEO"u8; // General encapsulated object
        public static ReadOnlySpan<byte> IPL_MAGIC_SPAN => "IPL"u8; // Involved people list
        public static ReadOnlySpan<byte> LNK_MAGIC_SPAN => "LNK"u8; // Linked information
        public static ReadOnlySpan<byte> MCI_MAGIC_SPAN => "MCI"u8; // Music CD Identifier
        public static ReadOnlySpan<byte> MLL_MAGIC_SPAN => "MLL"u8; // MPEG location lookup table
        public static ReadOnlySpan<byte> PIC_MAGIC_SPAN => "PIC"u8; // Attached picture
        public static ReadOnlySpan<byte> POP_MAGIC_SPAN => "POP"u8; // Popularimeter
        public static ReadOnlySpan<byte> REV_MAGIC_SPAN => "REV"u8; // Reverb
        public static ReadOnlySpan<byte> RVA_MAGIC_SPAN => "RVA"u8; // Relative volume adjustment
        public static ReadOnlySpan<byte> SLT_MAGIC_SPAN => "SLT"u8; // Synchronized lyric/text
        public static ReadOnlySpan<byte> STC_MAGIC_SPAN => "STC"u8; // Synced tempo codes
        public static ReadOnlySpan<byte> TAL_MAGIC_SPAN => "TAL"u8; // Album/Movie/Show title
        public static ReadOnlySpan<byte> TBP_MAGIC_SPAN => "TBP"u8; // BPM (Beats Per Minute)
        public static ReadOnlySpan<byte> TCM_MAGIC_SPAN => "TCM"u8; // Composer
        public static ReadOnlySpan<byte> TCO_MAGIC_SPAN => "TCO"u8; // Content type
        public static ReadOnlySpan<byte> TCR_MAGIC_SPAN => "TCR"u8; // Copyright message
        public static ReadOnlySpan<byte> TDA_MAGIC_SPAN => "TDA"u8; // Date
        public static ReadOnlySpan<byte> TDY_MAGIC_SPAN => "TDY"u8; // Playlist delay
        public static ReadOnlySpan<byte> TEN_MAGIC_SPAN => "TEN"u8; // Encoded by
        public static ReadOnlySpan<byte> TFT_MAGIC_SPAN => "TFT"u8; // File type
        public static ReadOnlySpan<byte> TIM_MAGIC_SPAN => "TIM"u8; // Time
        public static ReadOnlySpan<byte> TKE_MAGIC_SPAN => "TKE"u8; // Initial key
        public static ReadOnlySpan<byte> TLA_MAGIC_SPAN => "TLA"u8; // Language(s)
        public static ReadOnlySpan<byte> TLE_MAGIC_SPAN => "TLE"u8; // Length
        public static ReadOnlySpan<byte> TMT_MAGIC_SPAN => "TMT"u8; // Media type
        public static ReadOnlySpan<byte> TOA_MAGIC_SPAN => "TOA"u8; // Original artist(s)/performer(s)
        public static ReadOnlySpan<byte> TOF_MAGIC_SPAN => "TOF"u8; // Original filename
        public static ReadOnlySpan<byte> TOL_MAGIC_SPAN => "TOL"u8; // Original Lyricist(s)/text writer(s)
        public static ReadOnlySpan<byte> TOR_MAGIC_SPAN => "TOR"u8; // Original release year
        public static ReadOnlySpan<byte> TOT_MAGIC_SPAN => "TOT"u8; // Original album/Movie/Show title
        public static ReadOnlySpan<byte> TP1_MAGIC_SPAN => "TP1"u8; // Lead artist(s)/Lead performer(s)...
        public static ReadOnlySpan<byte> TP2_MAGIC_SPAN => "TP2"u8; // Band/Orchestra/Accompaniment
        public static ReadOnlySpan<byte> TP3_MAGIC_SPAN => "TP3"u8; // Conductor/Performer refinement
        public static ReadOnlySpan<byte> TP4_MAGIC_SPAN => "TP4"u8; // Interpreted, remixed...
        public static ReadOnlySpan<byte> TPA_MAGIC_SPAN => "TPA"u8; // Part of a set
        public static ReadOnlySpan<byte> TPB_MAGIC_SPAN => "TPB"u8; // Publisher
        public static ReadOnlySpan<byte> TRC_MAGIC_SPAN => "TRC"u8; // ISRC
        public static ReadOnlySpan<byte> TRD_MAGIC_SPAN => "TRD"u8; // Recording dates
        public static ReadOnlySpan<byte> TRK_MAGIC_SPAN => "TRK"u8; // Track number/Position in set
        public static ReadOnlySpan<byte> TSI_MAGIC_SPAN => "TSI"u8; // Size
        public static ReadOnlySpan<byte> TSS_MAGIC_SPAN => "TSS"u8; // Software/hardware and settings...
        public static ReadOnlySpan<byte> TT1_MAGIC_SPAN => "TT1"u8; // Content group description
        public static ReadOnlySpan<byte> TT2_MAGIC_SPAN => "TT2"u8; // Title/Songname/Content description
        public static ReadOnlySpan<byte> TT3_MAGIC_SPAN => "TT3"u8; // Subtitle/Description refinement
        public static ReadOnlySpan<byte> TXT_MAGIC_SPAN => "TXT"u8; // Lyricist/text writer
        public static ReadOnlySpan<byte> TXX_MAGIC_SPAN => "TXX"u8; // User defined text information frame
        public static ReadOnlySpan<byte> TYE_MAGIC_SPAN => "TYE"u8; // Year
        public static ReadOnlySpan<byte> UFI_MAGIC_SPAN => "UFI"u8; // Unique file identifier
        public static ReadOnlySpan<byte> ULT_MAGIC_SPAN => "ULT"u8; // Unsynchronized lyric/text transcription
        public static ReadOnlySpan<byte> WAF_MAGIC_SPAN => "WAF"u8; // Official audio file webpage
        public static ReadOnlySpan<byte> WAR_MAGIC_SPAN => "WAR"u8; // Official artist/performer webpage
        public static ReadOnlySpan<byte> WAS_MAGIC_SPAN => "WAS"u8; // Official audio source webpage
        public static ReadOnlySpan<byte> WCM_MAGIC_SPAN => "WCM"u8; // Commercial information
        public static ReadOnlySpan<byte> WCP_MAGIC_SPAN => "WCP"u8; // Copyright/Legal information
        public static ReadOnlySpan<byte> WPB_MAGIC_SPAN => "WPB"u8; // Publishers official webpage
        public static ReadOnlySpan<byte> WXX_MAGIC_SPAN => "WXX"u8; // User defined URL link frame

        public static ReadOnlySpan<byte> ZERO_SPAN_4 => [0, 0, 0, 0];
        public static ReadOnlySpan<byte> ZERO_SPAN_3 => [0, 0, 0];

        // Probably more to come?...
    }
    internal static class Uints
    {
        // Magic stuff, big endian
        public const uint ID3_MAGIC_UINT = 0x49443300;      // "ID3"
        public const uint TAG_MAGIC_UINT = 0x54414700;      // "TAG"
        public const uint MP3_SYNC_SIG_UINT = 0xFFFB0000;   // 0xFF, 0xFB

        // Metadata Magics
        public const uint TIT2_MAGIC_UINT = 0x54495432;    // "TIT2" - title
        public const uint TPE1_MAGIC_UINT = 0x54504531;    // "TPE1" - artist 1: blue shift
        public const uint TPE2_MAGIC_UINT = 0x54504532;    // "TPE2" - artist 2: electric boogaloo
        public const uint TPE3_MAGIC_UINT = 0x54504533;    // "TPE3" - artists 3: A.K.A Conductor
        public const uint TALB_MAGIC_UINT = 0x54414C42;    // "TALB" - album
        public const uint TCOP_MAGIC_UINT = 0x54434F50;    // "TCOP" - copyrighted by
        public const uint TENC_MAGIC_UINT = 0x54454E43;    // "TENC" - encoded by
        public const uint TRCK_MAGIC_UINT = 0x5452434B;    // "TRCK" - track number
        public const uint TPOS_MAGIC_UINT = 0x54504F53;    // "TPOS" - disc number
        public const uint TCON_MAGIC_UINT = 0x54434F4E;    // "TCON" - genre
        public const uint TCOM_MAGIC_UINT = 0x54434F4D;    // "TCOM" - composer
        public const uint TYER_MAGIC_UINT = 0x54594552;    // "TYER" - year
        public const uint TDRC_MAGIC_UINT = 0x54445243;    // "TDRC" - year but for 2.4
        public const uint TXXX_MAGIC_UINT = 0x54585858;    // "TXXX" - other things...?
        public const uint TSSE_MAGIC_UINT = 0x54535345;    // "TSSE" - encoder settings.
        public const uint APIC_MAGIC_UINT = 0x41504943;    // "APIC" - an image attachment...
        public const uint TLAN_MAGIC_UINT = 0x544C414E;    // "TLAN" - language.
        public const uint TPUB_MAGIC_UINT = 0x54505542;    // "TPUB" - Publisher
        public const uint TSRC_MAGIC_UINT = 0x54535243;    // "TSRC" - ISRC
        public const uint TBPM_MAGIC_UINT = 0x5442504D;    // "TBPM" - BPM
        public const uint TKEY_MAGIC_UINT = 0x544B4559;    // "TKEY" - key
        public const uint USLT_MAGIC_UINT = 0x55534C54;    // "USLT" - Unsynced Lyrics
        public const uint COMM_MAGIC_UINT = 0x434F4D4D;    // "COMM" - Commentary...
        public const uint POPM_MAGIC_UINT = 0x504F504D;    // "POPM" - Popularimeter
        public const uint PRIV_MAGIC_UINT = 0x50524956;    // "PRIV" - Private frame
        public const uint TSOT_MAGIC_UINT = 0x54534F54;    // "TSOT" - Title sort...
        public const uint UFID_MAGIC_UINT = 0x55464944;    // "UFID" - Unique field
        public const uint WXXX_MAGIC_UINT = 0x57585858;    // "WXXX" - I don't even know.
        public const uint GEOB_MAGIC_UINT = 0x47454F42;    // "GEOB" - General encapsulated object
        public const uint TDAT_MAGIC_UINT = 0x54444154;    // "TDAT" - Date, of my death presumably
        public const uint RGAD_MAGIC_UINT = 0x52474144;    // "RGAD" - Replay gain adjustment
        public const uint TLEN_MAGIC_UINT = 0x544C454E;    // "TLEN" - Tag that contains length...
        public const uint TDRL_MAGIC_UINT = 0x5444524C;    // "TDRL" - Release time
        public const uint MCDI_MAGIC_UINT = 0x4D434449;    // "MCDI" - CD Table of Contents
        public const uint TFLT_MAGIC_UINT = 0x54464C54;    // "TFLT" - File type...
        public const uint USER_MAGIC_UINT = 0x55534552;    // "USER" - Ownership or some shit
        public const uint TOPE_MAGIC_UINT = 0x544F5045;    // "TOPE" - Original artists/performer
        public const uint TCPM_MAGIC_UINT = 0x5443504D;    // "TCPM" - iTunes compilation marker
        public const uint WCOP_MAGIC_UINT = 0x57434F50;    // "WCOP" - URL to copyright information.
        public const uint WPUB_MAGIC_UINT = 0x57505542;    // "WPUB" - url to publisher
        public const uint WOAS_MAGIC_UINT = 0x574F4153;    // "WOAS" - url to song's webpage
        public const uint TMOO_MAGIC_UINT = 0x544D4F4F;    // "TMOO"
        public const uint TMED_MAGIC_UINT = 0x544D4544;    // "TMED"
        public const uint IPLS_MAGIC_UINT = 0x49504C53;    // "IPLS"
        public const uint SYLT_MAGIC_UINT = 0x53594C54;    // "SYLT"
        public const uint TIT1_MAGIC_UINT = 0x54495431;    // "TIT1"

        // Metadata Magics (3-letter ID, big-endian)
        public const uint BUF_MAGIC_UINT = 0x42554600;    // "BUF" - Recommended buffer size
        public const uint CNT_MAGIC_UINT = 0x434E5400;    // "CNT" - Play counter
        public const uint COM_MAGIC_UINT = 0x434F4D00;    // "COM" - Comments
        public const uint CRA_MAGIC_UINT = 0x43524100;    // "CRA" - Audio encryption
        public const uint CRM_MAGIC_UINT = 0x43524D00;    // "CRM" - Encrypted meta frame
        public const uint ETC_MAGIC_UINT = 0x45544300;    // "ETC" - Event timing codes
        public const uint EQU_MAGIC_UINT = 0x45515500;    // "EQU" - Equalization
        public const uint GEO_MAGIC_UINT = 0x47454F00;    // "GEO" - General encapsulated object
        public const uint IPL_MAGIC_UINT = 0x49504C00;    // "IPL" - Involved people list
        public const uint LNK_MAGIC_UINT = 0x4C4E4B00;    // "LNK" - Linked information
        public const uint MCI_MAGIC_UINT = 0x4D434900;    // "MCI" - Music CD Identifier
        public const uint MLL_MAGIC_UINT = 0x4D4C4C00;    // "MLL" - MPEG location lookup table
        public const uint PIC_MAGIC_UINT = 0x50494300;    // "PIC" - Attached picture
        public const uint POP_MAGIC_UINT = 0x504F5000;    // "POP" - Popularimeter
        public const uint REV_MAGIC_UINT = 0x52455600;    // "REV" - Reverb
        public const uint RVA_MAGIC_UINT = 0x52564100;    // "RVA" - Relative volume adjustment
        public const uint SLT_MAGIC_UINT = 0x534C5400;    // "SLT" - Synchronized lyric/text
        public const uint STC_MAGIC_UINT = 0x53544300;    // "STC" - Synced tempo codes
        public const uint TAL_MAGIC_UINT = 0x54414C00;    // "TAL" - Album/Movie/Show title
        public const uint TBP_MAGIC_UINT = 0x54425000;    // "TBP" - BPM (Beats Per Minute)
        public const uint TCM_MAGIC_UINT = 0x54434D00;    // "TCM" - Composer
        public const uint TCO_MAGIC_UINT = 0x54434F00;    // "TCO" - Content type
        public const uint TCR_MAGIC_UINT = 0x54435200;    // "TCR" - Copyright message
        public const uint TDA_MAGIC_UINT = 0x54444100;    // "TDA" - Date
        public const uint TDY_MAGIC_UINT = 0x54445900;    // "TDY" - Playlist delay
        public const uint TEN_MAGIC_UINT = 0x54454E00;    // "TEN" - Encoded by
        public const uint TFT_MAGIC_UINT = 0x54465400;    // "TFT" - File type
        public const uint TIM_MAGIC_UINT = 0x54494D00;    // "TIM" - Time
        public const uint TKE_MAGIC_UINT = 0x544B4500;    // "TKE" - Initial key
        public const uint TLA_MAGIC_UINT = 0x544C4100;    // "TLA" - Language(s)
        public const uint TLE_MAGIC_UINT = 0x544C4500;    // "TLE" - Length
        public const uint TMT_MAGIC_UINT = 0x544D5400;    // "TMT" - Media type
        public const uint TOA_MAGIC_UINT = 0x544F4100;    // "TOA" - Original artist(s)/performer(s)
        public const uint TOF_MAGIC_UINT = 0x544F4600;    // "TOF" - Original filename
        public const uint TOL_MAGIC_UINT = 0x544F4C00;    // "TOL" - Original Lyricist(s)/text writer(s)
        public const uint TOR_MAGIC_UINT = 0x544F5200;    // "TOR" - Original release year
        public const uint TOT_MAGIC_UINT = 0x544F5400;    // "TOT" - Original album/Movie/Show title
        public const uint TP1_MAGIC_UINT = 0x54503100;    // "TP1" - Lead artist(s)/Lead performer(s)...
        public const uint TP2_MAGIC_UINT = 0x54503200;    // "TP2" - Band/Orchestra/Accompaniment
        public const uint TP3_MAGIC_UINT = 0x54503300;    // "TP3" - Conductor/Performer refinement
        public const uint TP4_MAGIC_UINT = 0x54503400;    // "TP4" - Interpreted, remixed...
        public const uint TPA_MAGIC_UINT = 0x54504100;    // "TPA" - Part of a set
        public const uint TPB_MAGIC_UINT = 0x54504200;    // "TPB" - Publisher
        public const uint TRC_MAGIC_UINT = 0x54524300;    // "TRC" - ISRC
        public const uint TRD_MAGIC_UINT = 0x54524400;    // "TRD" - Recording dates
        public const uint TRK_MAGIC_UINT = 0x54524B00;    // "TRK" - Track number/Position in set
        public const uint TSI_MAGIC_UINT = 0x54534900;    // "TSI" - Size
        public const uint TSS_MAGIC_UINT = 0x54535300;    // "TSS" - Software/hardware and settings...
        public const uint TT1_MAGIC_UINT = 0x54543100;    // "TT1" - Content group description
        public const uint TT2_MAGIC_UINT = 0x54543200;    // "TT2" - Title/Songname/Content description
        public const uint TT3_MAGIC_UINT = 0x54543300;    // "TT3" - Subtitle/Description refinement
        public const uint TXT_MAGIC_UINT = 0x54585400;    // "TXT" - Lyricist/text writer
        public const uint TXX_MAGIC_UINT = 0x54585800;    // "TXX" - User defined text information frame
        public const uint TYE_MAGIC_UINT = 0x54594500;    // "TYE" - Year
        public const uint UFI_MAGIC_UINT = 0x55464900;    // "UFI" - Unique file identifier
        public const uint ULT_MAGIC_UINT = 0x554C5400;    // "ULT" - Unsynchronized lyric/text transcription
        public const uint WAF_MAGIC_UINT = 0x57414600;    // "WAF" - Official audio file webpage
        public const uint WAR_MAGIC_UINT = 0x57415200;    // "WAR" - Official artist/performer webpage
        public const uint WAS_MAGIC_UINT = 0x57415300;    // "WAS" - Official audio source webpage
        public const uint WCM_MAGIC_UINT = 0x57434D00;    // "WCM" - Commercial information
        public const uint WCP_MAGIC_UINT = 0x57435000;    // "WCP" - Copyright/Legal information
        public const uint WPB_MAGIC_UINT = 0x57504200;    // "WPB" - Publishers official webpage
        public const uint WXX_MAGIC_UINT = 0x57585800;    // "WXX" - User defined URL link frame

        public const uint ZERO_UINT = 0u;


        // Probably more to come?...
    }
}

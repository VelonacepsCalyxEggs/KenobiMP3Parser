namespace KenobiMp3Parser.Constants
{
    internal static class Spans
    {
        // Magic stuff, big endian
        public static ReadOnlySpan<byte> ID3_MAGIC_SPAN => "ID3"u8;
        public static ReadOnlySpan<byte> TAG_MAGIC_SPAN => "TAG"u8;

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
}

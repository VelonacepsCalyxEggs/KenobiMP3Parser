using System.Collections.ObjectModel;

namespace KenobiMp3Parser.Constants
{
    internal static class Tables
    {
        public static readonly int[,] Mp3BitrateTable =
        {
            { 0, 0, 0, 0, 0 },
            { 32000, 32000, 32000, 32000, 8000},
            { 64000, 48000, 40000, 48000, 16000 },
            { 96000, 56000, 48000, 56000, 24000 },
            { 128000, 64000, 56000, 64000, 32000 },
            { 160000, 80000, 64000, 80000, 40000 },
            { 192000, 96000, 80000, 96000, 48000 },
            { 224000, 112000, 96000, 112000, 56000 },
            { 256000, 128000, 112000, 128000, 64000 },
            { 288000, 160000, 128000, 144000, 80000 },
            { 320000, 192000, 160000, 160000, 96000 },
            { 352000, 224000, 192000, 176000, 112000 },
            { 384000, 256000, 224000, 192000, 128000 },
            { 416000, 320000, 256000, 224000, 144000 },
            { 448000, 384000, 320000, 256000, 160000 }
        };
        public static readonly int[,] Mp3SampleRateTable =
        {
            { 44100, 22050, 11025 },
            { 48000, 24000, 12000 },
            { 32000, 16000, 8000 },
            { 0, 0, 0 }
        };
        public static readonly int[,] Mp3SamplesPerFrameTable =
        {
            { 384, 1152, 1152 },
            { 384, 1152, 576 }
        };
        public static readonly ReadOnlyCollection<string> Id3v1Genres = new(new[]
        {
            // 0 - 79 (ID3v1 standard)
            "Blues", "Classic Rock", "Country", "Dance", "Disco", "Funk", "Grunge", "Hip-Hop", "Jazz", "Metal",
            "New Age", "Oldies", "Other", "Pop", "R&B", "Rap", "Reggae", "Rock", "Techno", "Industrial",
            "Alternative", "Ska", "Death Metal", "Pranks", "Soundtrack", "Euro-Techno", "Ambient", "Trip-Hop", "Vocal", "Jazz+Funk",
            "Fusion", "Trance", "Classical", "Instrumental", "Acid", "House", "Game", "Sound Clip", "Gospel", "Noise",
            "Alternative Rock", "Bass", "Soul", "Punk", "Space", "Meditative", "Instrumental Pop", "Instrumental Rock", "Ethnic", "Gothic",
            "Darkwave", "Techno-Industrial", "Electronic", "Pop-Folk", "Eurodance", "Dream", "Southern Rock", "Comedy", "Cult", "Gangsta",
            "Top 40", "Christian Rap", "Pop/Funk", "Jungle", "Native American", "Cabaret", "New Wave", "Psychedelic", "Rave", "Showtunes",
            "Trailer", "Lo-Fi", "Tribal", "Acid Punk", "Acid Jazz", "Polka", "Retro", "Musical", "Rock & Roll", "Hard Rock",

            // 80 - 147 (Winamp extensions)
            "Folk", "Folk-Rock", "National Folk", "Swing", "Fast Fusion", "Bebop", "Latin", "Revival", "Celtic", "Bluegrass",
            "Avantgarde", "Gothic Rock", "Progressive Rock", "Psychedelic Rock", "Symphonic Rock", "Slow Rock", "Big Band", "Chorus", "Easy Listening", "Acoustic",
            "Humour", "Speech", "Chanson", "Opera", "Chamber Music", "Sonata", "Symphony", "Booty Bass", "Primus", "Porn Groove",
            "Satire", "Slow Jam", "Club", "Tango", "Samba", "Folklore", "Ballad", "Power Ballad", "Rhythmic Soul", "Freestyle",
            "Duet", "Punk Rock", "Drum Solo", "A cappella", "Euro-House", "Dance Hall", "Goa", "Drum & Bass", "Club-House", "Hardcore",
            "Terror", "Indie", "BritPop", "Negerpunk", "Polsk Punk", "Beat", "Christian Gangsta Rap", "Heavy Metal", "Black Metal", "Crossover",
            "Contemporary Christian", "Christian Rock", "Merengue", "Salsa", "Thrash Metal", "Anime", "JPop", "Synthpop",
            // Also are we for real right now WinAmp? Negerpunk?
            // 148 - 191 (Winamp 5.6 extensions)
            "Abstract", "Art Rock", "Baroque", "Bhangra", "Big Beat", "Breakbeat", "Chillout", "Downtempo", "Dub", "EBM",
            "Eclectic", "Electro", "Electroclash", "Emo", "Experimental", "Garage", "Global", "IDM", "Illbient", "Industro-Goth",
            "Jam Band", "Krautrock", "Leftfield", "Lounge", "Math Rock", "New Romantic", "Nu-Breakz", "Post-Punk", "Post-Rock", "Psytrance",
            "Shoegaze", "Space Rock", "Trop Rock", "World Music", "Neoclassical", "Audiobook", "Audio Theatre", "Neue Deutsche Welle", "Podcast", "Indie Rock",
            "G-Funk", "Dubstep", "Garage Rock", "Psybient"
        });

        /// <summary>
        /// Safely resolves an ID3v1 genre byte index to its string name.
        /// Handles out-of-bounds indices and the fallback 'Unknown' byte (255).
        /// </summary>
        public static string GetID3V1GenreName(byte genreByte)
        {
            if (genreByte == 255 || genreByte >= Id3v1Genres.Count)
            {
                return "Unknown";
            }

            return Id3v1Genres[genreByte];
        }

    }
}

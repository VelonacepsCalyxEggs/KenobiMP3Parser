using System.Diagnostics;
using KenobiMp3Parser;

namespace Mp3ParserCliExample
{
    class Program
    {
        static bool quiet = false;
        static bool showTime = false;
        static string? outputFilePath = null;
        static StreamWriter? outputWriter = null;

        static int Main(string[] args)
        {
            var paths = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg == "-h" || arg == "--help")
                {
                    PrintHelp();
                    return 0;
                }
                else if (arg == "-q" || arg == "--quiet")
                {
                    quiet = true;
                }
                else if (arg == "-t" || arg == "--time")
                {
                    showTime = true;
                }
                else if (arg == "-o" || arg == "--output")
                {
                    if (i + 1 < args.Length)
                    {
                        outputFilePath = args[++i];
                    }
                    else
                    {
                        Console.Error.WriteLine("Error: --output requires a file path.");
                        return 1;
                    }
                }
                else if (arg.StartsWith("-"))
                {
                    Console.Error.WriteLine($"Unknown option: {arg}");
                    return 1;
                }
                else
                {
                    paths.Add(arg);
                }
            }

            if (paths.Count == 0)
            {
                PrintHelp();
                return 1;
            }

            if (outputFilePath != null)
            {
                try
                {
                    outputWriter = new StreamWriter(outputFilePath, append: false);
                    outputWriter.AutoFlush = true;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Cannot open output file: {ex.Message}");
                    return 1;
                }
            }

            int totalFiles = 0, totalMp3 = 0, totalErrors = 0;
            var totalTimer = Stopwatch.StartNew();

            foreach (string path in paths)
            {
                if (File.Exists(path))
                {
                    ProcessSingleFile(path, ref totalFiles, ref totalMp3, ref totalErrors);
                }
                else if (Directory.Exists(path))
                {
                    ProcessDirectory(path, ref totalFiles, ref totalMp3, ref totalErrors);
                }
                else
                {
                    WriteError($"Path not found: {path}");
                    totalErrors++;
                }
            }

            totalTimer.Stop();

            string summary = $"Summary: {totalFiles} file(s) scanned, " +
                             $"{totalMp3} MP3 file(s) processed, " +
                             $"{totalErrors} error(s).";

            if (showTime)
                summary += $" Total time: {totalTimer.Elapsed.TotalSeconds:F2}s";

            WriteLine(summary);

            outputWriter?.Dispose();
            return totalErrors > 0 ? 1 : 0;
        }
        static void ProcessSingleFile(string filePath, ref int totalFiles, ref int totalMp3, ref int totalErrors)
        {
            totalFiles++;

            try
            {
                using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan);
                using var bs = new BufferedStream(fs, 1024 * 1024);

                bool isMp3 = Mp3MetadataParser.CheckIfMp3(bs);
                if (!isMp3)
                {
                    // non MP3 file
                    return;
                }

                totalMp3++;
                var sw = showTime ? Stopwatch.StartNew() : null;

                var data = Mp3MetadataParser.ParseMP3File(bs);
                sw?.Stop();

                string resultLine = $"File: {filePath}{Environment.NewLine}{data}";
                if (showTime && sw != null)
                    resultLine += $"{Environment.NewLine}Time: {sw.Elapsed.TotalMilliseconds:F2} ms";

                WriteLine(resultLine);
                WriteLine();   // blank line after each file
            }
            catch (Exception ex)
            {
                totalErrors++;
                WriteError($"Error processing {filePath}: {ex.Message}");
            }
        }

        static void ProcessDirectory(string directoryPath, ref int totalFiles, ref int totalMp3, ref int totalErrors)
        {
            WriteLine($"Scanning directory: {directoryPath}");
            WriteLine(new string('-', 60));

            var allFiles = Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                ProcessSingleFile(file, ref totalFiles, ref totalMp3, ref totalErrors);
            }

            WriteLine(new string('-', 60));
        }

        static void WriteLine(string message = "")
        {
            outputWriter?.WriteLine(message);

            if (!quiet)
                Console.WriteLine(message);
        }

        static void WriteError(string message)
        {
            Console.Error.WriteLine(message);

            outputWriter?.WriteLine($"[ERROR] {message}");
        }

        static void PrintHelp()
        {
            string help = @"
Mp3ParserCli – test tool for KenobiMp3Parser

Usage:
  Mp3ParserCli [options] <path1> [path2 ...]

Options:
  -q, --quiet        Suppress all normal output (only errors are printed).
  -t, --time         Show elapsed time for each file and total time.
  -o, --output FILE  Write all output (including errors) to FILE.
  -h, --help         Show this help message.

Examples:
  Mp3ParserCli song.mp3
  Mp3ParserCli -t -o results.txt music_folder/
  Mp3ParserCli -q -t file1.mp3 file2.mp3
";
            Console.WriteLine(help);
        }
    }
}
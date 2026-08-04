using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
namespace Tests;
public class BenchmarkAutomationTests
{
    [Fact]
    public void RunAllBenchmarks()
    {
        var args = new[] { "--filter", "*" };

        var config = ManualConfig.CreateMinimumViable()
                                 .AddLogger(ConsoleLogger.Default);

        var summaries = BenchmarkSwitcher.FromAssembly(typeof(BenchmarkAutomationTests).Assembly)
                                         .Run(args, config);

        Assert.NotEmpty(summaries);
    }
}
[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 3, printSource: true)]
public class FrameParseBenchmarks
{
    private readonly byte[] _validHeaderBytes = [0xFF, 0xFB, 0x90, 0x64];
    private MemoryStream _stream = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _stream = new MemoryStream(_validHeaderBytes);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _stream.Dispose();
    }

    [Benchmark(Baseline = true, Description = "TryReadMp3Frame")]
    public bool New()
    {
        _stream.Position = 0;
        return KenobiMp3Parser.Mp3MetadataParser.TryReadMp3Frame(_stream, out _, out _);
    }
}

[MemoryDiagnoser]
public class Mp3FullFileBenchmarks
{
    private readonly List<byte[]> _testData = [];

    [GlobalSetup]
    public void Setup()
    {
        string basePath = Path.Combine(AppContext.BaseDirectory, "Content", "MP3TestFiles");
        if (!Directory.Exists(basePath))
        {
            throw new DirectoryNotFoundException($"Base path not found: {basePath}. Ensure your .csproj copies the Content folder.");
        }

        var subDirs = Directory.GetDirectories(basePath).ToList();
        if (subDirs.Count == 0)
        {
            throw new InvalidOperationException($"No subdirectories found in {basePath}.");
        }

        foreach (var dir in subDirs)
        {
            var mp3Files = Directory.GetFiles(dir, "*.mp3").ToList();
            foreach (var file in mp3Files)
            {
                _testData.Add(File.ReadAllBytes(file));
            }
        }
    }

    [Benchmark(Baseline = true, Description = "ParseMP3File")]
    public void New()
    {
        foreach (var data in _testData)
        {
            using var stream = new MemoryStream(data, writable: false);
            KenobiMp3Parser.Mp3MetadataParser.ParseMP3File(stream);
        }
    }
}

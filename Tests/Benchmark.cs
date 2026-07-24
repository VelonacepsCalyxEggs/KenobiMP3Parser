using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using KenobiMp3Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class PerformanceTests
    {
        private readonly ITestOutputHelper _output;

        public PerformanceTests(ITestOutputHelper output) => _output = output;

        [Fact]
        public void RunMp3PerformanceBenchmarks()
        {
            _output.WriteLine($"=== Starting benchmark run at {DateTime.Now} ===");

            string basePath = Path.Combine(AppContext.BaseDirectory, "Content", "MP3TestFiles");
            _output.WriteLine($"Looking for test folders in: {basePath}");

            if (!Directory.Exists(basePath))
                Assert.Fail($"Base path not found: {basePath}. Ensure your .csproj copies the Content folder.");

            var subDirs = Directory.GetDirectories(basePath).ToList();
            _output.WriteLine($"Found {subDirs.Count} subdirectories.");
            if (!subDirs.Any())
                Assert.Fail($"No subdirectories found in {basePath}.");

            foreach (var dir in subDirs)
            {
                var mp3Files = Directory.GetFiles(dir, "*.mp3").ToList();
                _output.WriteLine($"  Folder '{Path.GetFileName(dir)}' contains {mp3Files.Count} MP3 file(s).");
                foreach (var file in mp3Files)
                    _output.WriteLine($"    - {Path.GetFileName(file)}");
            }

            Mp3FolderBenchmarks.SetLogger(_output);

            var config = ManualConfig.CreateEmpty()
                .AddJob(Job.Default
                    .WithToolchain(InProcessEmitToolchain.Instance)
                    .WithIterationCount(10)
                    .WithWarmupCount(3)
                    .WithLaunchCount(1))
                .WithOption(ConfigOptions.DisableOptimizationsValidator, true)
                .AddLogger(DefaultConfig.Instance.GetLoggers().ToArray())
                .AddExporter(DefaultConfig.Instance.GetExporters().ToArray())
                .AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray())
                .AddDiagnoser(DefaultConfig.Instance.GetDiagnosers().ToArray());

            _output.WriteLine("Starting BenchmarkDotNet run...");
            var summary = BenchmarkRunner.Run<Mp3FolderBenchmarks>(config);
            _output.WriteLine("Benchmark run completed.");

            Assert.NotNull(summary);
            Assert.False(summary.HasCriticalValidationErrors, "Benchmark validation failed.");
            Assert.NotEmpty(summary.Reports);

            foreach (var report in summary.Reports)
            {
                foreach (var exec in report.ExecuteResults)
                    if (exec.Errors.Any())
                        Assert.Fail($"Benchmark '{report.BenchmarkCase.Descriptor}' threw: {exec.Errors[0]}");


                if (report.ResultStatistics == null)
                    Assert.Fail($"Benchmark '{report.BenchmarkCase.Descriptor}' produced no statistics.");

                Assert.True(report.ResultStatistics.Mean > 0,
                    $"Benchmark '{report.BenchmarkCase.Descriptor}' has Mean = 0 or negative.");
            }

            DisplayBenchmarkResults(summary);

            _output.WriteLine("=== All benchmarks passed successfully ===");
        }

        private void DisplayBenchmarkResults(BenchmarkDotNet.Reports.Summary summary)
        {
            _output.WriteLine("");
            _output.WriteLine("BENCHMARK RESULTS");
            _output.WriteLine("====================");

            foreach (var report in summary.Reports)
            {
                var stats = report.ResultStatistics;
                if (stats == null) continue;

                double meanMs = stats.Mean / 1_000_000.0;
                double stdDevMs = stats.StandardDeviation / 1_000_000.0;

                var gcStats = report.GcStats;

                long allocatedBytes = gcStats.GetBytesAllocatedPerOperation(report.BenchmarkCase) ?? 0;

                long totalOperations = 0;
                foreach (var measurement in report.AllMeasurements)
                {
                    if (measurement.IterationMode == BenchmarkDotNet.Engines.IterationMode.Workload)
                    {
                        totalOperations += measurement.Operations;
                    }
                }
                if (totalOperations == 0) totalOperations = 1;

                double gen0 = (gcStats.Gen0Collections * 1000.0) / totalOperations;
                double gen1 = (gcStats.Gen1Collections * 1000.0) / totalOperations;

                _output.WriteLine($"  {report.BenchmarkCase.Descriptor}");
                _output.WriteLine($"    Mean:     {meanMs:F2} ms");
                _output.WriteLine($"    StdDev:   {stdDevMs:F2} ms");
                _output.WriteLine($"    Allocated: {(allocatedBytes / 1024.0 / 1024.0):F2} MB ({allocatedBytes:N0} bytes)");
                _output.WriteLine($"    GC Gen0:  {gen0:F2}  Gen1: {gen1:F2}");
                _output.WriteLine("");
            }

            var artifactsPath = Path.Combine(AppContext.BaseDirectory, "BenchmarkDotNet.Artifacts", "results");
            if (Directory.Exists(artifactsPath))
            {
                _output.WriteLine($"Results saved to: {artifactsPath}");
            }
        }

        [MemoryDiagnoser]
        [HideColumns("Error", "StdDev", "Ratio")]
        public class Mp3FolderBenchmarks
        {
            private static ITestOutputHelper _logger;

            public static void SetLogger(ITestOutputHelper logger) => _logger = logger;

            [ParamsSource(nameof(GetTargetFolders))]
            public string FolderPath { get; set; }

            private List<byte[]> _validFilesData = new();

            public static IEnumerable<string> GetTargetFolders()
            {
                string basePath = Path.Combine(AppContext.BaseDirectory, "Content", "MP3TestFiles");
                if (!Directory.Exists(basePath))
                    throw new DirectoryNotFoundException($"Base path not found: {basePath}");
                return Directory.GetDirectories(basePath);
            }

            [GlobalSetup]
            public void Setup()
            {
                _logger?.WriteLine($"--- GlobalSetup for folder: {FolderPath} ---");

                var mp3Files = Directory.GetFiles(FolderPath, "*.mp3");
                _logger?.WriteLine($"Found {mp3Files.Length} MP3 file(s) in {FolderPath}");

                var valid = new List<byte[]>();

                foreach (var filePath in mp3Files)
                {
                    try
                    {
                        byte[] data = File.ReadAllBytes(filePath);
                        using (var stream = new MemoryStream(data))
                        {
                            _ = Mp3MetadataParser.ParseMP3File(stream);
                            valid.Add(data);
                            _logger?.WriteLine($"Valid: {Path.GetFileName(filePath)}");
                        }
                    }
                    catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
                    {
                        _logger?.WriteLine($"Read error: {Path.GetFileName(filePath)} - {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        _logger?.WriteLine($"Skipped invalid: {Path.GetFileName(filePath)} - {ex.Message}");
                    }
                }

                _validFilesData = valid;
                _logger?.WriteLine($"Total valid files ready: {_validFilesData.Count}");

                if (_validFilesData.Count == 0)
                    throw new InvalidOperationException($"No valid MP3 files in {FolderPath}.");
            }

            [Benchmark]
            public void BenchmarkFolderParsing()
            {
                foreach (var fileData in _validFilesData)
                {
                    using var stream = new MemoryStream(fileData);
                    try
                    {
                        _ = Mp3MetadataParser.ParseMP3File(stream);
                    }
                    catch (Exception ex)
                    {
                        _logger?.WriteLine($"Unexpected exception in benchmark: {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
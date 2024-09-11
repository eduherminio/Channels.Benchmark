using BenchmarkDotNet.Attributes;

namespace Channels.Benchmark;

[MarkdownExporterAttribute.GitHub]
[HtmlExporter]
[MemoryDiagnoser]
//[BenchmarkDotNet.Diagnostics.Windows.Configs.NativeMemoryProfiler]
public class BaseBenchmark;

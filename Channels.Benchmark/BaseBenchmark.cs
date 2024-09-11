using BenchmarkDotNet.Attributes;

namespace Channels.Benchmark;

[MarkdownExporterAttribute.GitHub]
[HtmlExporter]
[MemoryDiagnoser]
//[NativeMemoryProfiler]
public class BaseBenchmark;

# Channels.Benchmark

[![Channels.Benchmark build][buildlogo]][buildlink]

Set of Benchmarks to measure the performance of different [`System.Threading.Channels`](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels) configurations.

- [`Channel<object>` vs `Channel<interface>` vs `Channel<concrete_class>`](Results/ChannelOfT_Benchmark.md)
- [Raw (class) payload vs wrappers: `class` vs `struct` vs `readonly struct` vs `record class` vs `record struct` vs `readonly record struct`](Results/PayloadType_Benchmark.md)
- [String serialization pre-write (`Channel<string>`) vs post-write (`Channel<object>`, `Channel<interface>`, including type detection vs not)](Results/StringSerialization_Benchmark.md)

[buildlink]: https://github.com/eduherminio/Channels.Benchmarks/actions/build/ci.yml
[buildlogo]: https://github.com/eduherminio/Channels.Benchmarks/actions/build/ci.yml/badge.svg

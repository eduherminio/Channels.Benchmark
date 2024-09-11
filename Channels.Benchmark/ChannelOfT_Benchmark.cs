using BenchmarkDotNet.Attributes;
using Channels.Benchmark.Model;
using System.Threading.Channels;

namespace Channels.Benchmark;

public class ChannelOfT_Benchmark : BaseBenchmark
{
    private readonly MyObject_Class_ImplementsInterface _input = new(new MyPayLoad(33592128, 27, 20, [352512, 684368, 345472, 691648, 344336, 675376, 353536, 699696, 345488, 701104, 344336, 709312, 345344, 706368, 346384, 699152, 345376, 689824, 344336, 688656, 345344, 697856, 344336, 705168, 345344, 714496, 338192, 671632, 345248, 678128, 353552, 714080, 346512, 693136, 345376, 676416, 352528, 717120, 345472], -500, +500));

#pragma warning disable S1104 // Fields should not have public accessibility
    [Params(1, 10, 100, 1_000, 10_000)]
    public int N;
#pragma warning restore S1104 // Fields should not have public accessibility

    private static readonly BoundedChannelOptions _boundedChannelOptions = new(512)
    {
        SingleReader = true,
        SingleWriter = false,
        FullMode = BoundedChannelFullMode.DropOldest
    };

    private readonly Channel<object> _objectChannel = Channel.CreateBounded<object>(_boundedChannelOptions);
    private readonly Channel<IMyObjectInterface> _interfaceChannel = Channel.CreateBounded<IMyObjectInterface>(_boundedChannelOptions);
    private readonly Channel<MyObject_Class_ImplementsInterface> _concreteClassChannel = Channel.CreateBounded<MyObject_Class_ImplementsInterface>(_boundedChannelOptions);

    [Benchmark(Baseline = true)]
    public async Task Object(MyPayLoad payload)
    {
        ChannelWriter<object> writer = _objectChannel.Writer;
        ChannelReader<object> reader = _objectChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_input);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    public async Task Interface(MyPayLoad payload)
    {
        ChannelWriter<IMyObjectInterface> writer = _interfaceChannel.Writer;
        ChannelReader<IMyObjectInterface> reader = _interfaceChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_input);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    public async Task ConcreteClass()
    {
        ChannelWriter<MyObject_Class_ImplementsInterface> writer = _concreteClassChannel.Writer;
        ChannelReader<MyObject_Class_ImplementsInterface> reader = _concreteClassChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_input);

            var output = reader.ReadAsync();
            await output;
        }
    }
}

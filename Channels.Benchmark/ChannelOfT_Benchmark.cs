using BenchmarkDotNet.Attributes;
using Channels.Benchmark.Model;
using System.Threading.Channels;

namespace Channels.Benchmark;

public class ChannelOfT_Benchmark : BaseBenchmark
{
    public static IEnumerable<MyPayLoad> Data =>
    [
        new MyPayLoad(33592128, 27, 20, [33592128], -500, +500 ),
        new MyPayLoad(33592128, 27, 20, [33591088, 33975472, 112608, 480352, 109456, 477200, 47968, 495952, 187344, 417008, 24220320, 18256304], -500, +500 ),
        new MyPayLoad(33592128, 27, 20, [33592128, 33974432, 112608, 477200, 109456, 33976512, 166864, 412848, 44848, 536656, 101055424, 480352, 93856, 18312528, 24276512, 101390400, 43808, 544800, 177056, 19426624], -500, +500 ),
        new MyPayLoad(33592128, 27, 20, [352512, 684368, 345472, 691648, 344336, 675376, 353536, 699696, 345488, 701104, 344336, 709312, 345344, 706368, 346384, 699152, 345376, 689824, 344336, 688656, 345344, 697856, 344336, 705168, 345344, 714496, 338192, 671632, 345248, 678128, 353552, 714080, 346512, 693136, 345376, 676416, 352528, 717120, 345472], -500, +500),
    ];

#pragma warning disable S1104 // Fields should not have public accessibility
    [Params(1, 10, 100, 1000)]
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
    [ArgumentsSource(nameof(Data))]
    public async Task Object(MyPayLoad payload)
    {
        ChannelWriter<object> writer = _objectChannel.Writer;
        ChannelReader<object> reader = _objectChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            MyObject_Class_ImplementsInterface input = new(payload);
            writer.TryWrite(input);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public async Task Interface(MyPayLoad payload)
    {
        ChannelWriter<IMyObjectInterface> writer = _interfaceChannel.Writer;
        ChannelReader<IMyObjectInterface> reader = _interfaceChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            MyObject_Class_ImplementsInterface input = new(payload);
            writer.TryWrite(input);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public async Task ConcreteClass(MyPayLoad payload)
    {
        ChannelWriter<MyObject_Class_ImplementsInterface> writer = _concreteClassChannel.Writer;
        ChannelReader<MyObject_Class_ImplementsInterface> reader = _concreteClassChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            MyObject_Class_ImplementsInterface input = new(payload);
            writer.TryWrite(input);

            var output = reader.ReadAsync();
            await output;
        }
    }
}

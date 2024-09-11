using BenchmarkDotNet.Attributes;
using Channels.Benchmark.Model;
using System.Threading.Channels;

namespace Channels.Benchmark;

public class StringSerialization_Benchmark : BaseBenchmark
{
    public static IEnumerable<MyObject_Class_ImplementsInterface> HeavyInput =>
[
        new (new(33592128, 27, 20, [33592128], -500, +500 )),
        new (new(33592128, 27, 20, [33591088, 33975472, 112608, 480352, 109456, 477200, 47968, 495952, 187344, 417008, 24220320, 18256304], -500, +500 )),
        new (new(33592128, 27, 20, [33592128, 33974432, 112608, 477200, 109456, 33976512, 166864, 412848, 44848, 536656, 101055424, 480352, 93856,
                    18312528, 24276512, 101390400, 43808, 544800, 177056, 19426624], -500, +500 )),
        new (new(33592128, 27, 20, [352512, 684368, 345472, 691648, 344336, 675376, 353536, 699696, 345488, 701104, 344336, 709312, 345344, 706368,
                    346384, 699152, 345376, 689824, 344336, 688656, 345344, 697856, 344336, 705168, 345344, 714496, 338192, 671632, 345248, 678128,
                    353552, 714080, 346512, 693136, 345376, 676416, 352528, 717120, 345472], -500, +500))
    ];

    private const string LightInput = "bestmove e7e8q ponder f8e8";

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

    private readonly Channel<string> _stringChannel = Channel.CreateBounded<string>(_boundedChannelOptions);
    private readonly Channel<IMyObjectInterface> _interfaceChannel = Channel.CreateBounded<IMyObjectInterface>(_boundedChannelOptions);
    private readonly Channel<object> _objectChannel = Channel.CreateBounded<object>(_boundedChannelOptions);

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(HeavyInput))]
    public async Task<long> PreWrite(MyObject_Class_ImplementsInterface heavyInput)
    {
        long result = 0;

        ChannelWriter<string> writer = _stringChannel.Writer;
        ChannelReader<string> reader = _stringChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(heavyInput.ToString());

            var output = reader.ReadAsync();
            result += (await output).Length;

            if (i % 15 == 0)
            {
                writer.TryWrite(LightInput);   // No need to wrap the string in MyLightObject_Class

                var anotherOutput = reader.ReadAsync();
                result += (await anotherOutput).Length;
            }
        }

        return result;
    }

    /// <summary>
    /// We call string.ToString(), should allocate more
    /// </summary>
    /// <param name="heavyInput"></param>
    /// <returns></returns>
    [Benchmark]
    [ArgumentsSource(nameof(HeavyInput))]
    public async Task<long> PostWrite_Interface_NoChecks(MyObject_Class_ImplementsInterface heavyInput)
    {
        long result = 0;

        ChannelWriter<IMyObjectInterface> writer = _interfaceChannel.Writer;
        ChannelReader<IMyObjectInterface> reader = _interfaceChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(heavyInput);

            var output = reader.ReadAsync();
            result += (await output).ToString()!.Length;

            if (i % 15 == 0)
            {
                writer.TryWrite(new MyLightObject_Class(LightInput));

                var anotherOutput = reader.ReadAsync();
                result += (await anotherOutput).ToString()!.Length;
            }
        }

        return result;
    }

    [Benchmark]
    [ArgumentsSource(nameof(HeavyInput))]
    public async Task<long> PostWrite_Interface_CheckType(MyObject_Class_ImplementsInterface heavyInput)
    {
        long result = 0;

        ChannelWriter<IMyObjectInterface> writer = _interfaceChannel.Writer;
        ChannelReader<IMyObjectInterface> reader = _interfaceChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(heavyInput);

            var output = await reader.ReadAsync();
            if (output is MyLightObject_Class light)
            {
                result += light.GetString().Length;
            }
            else
            {
                result += output.ToString()!.Length;
            }

            if (i % 15 == 0)
            {
                writer.TryWrite(new MyLightObject_Class(LightInput));

                var anotherOutput = await reader.ReadAsync();
                if (anotherOutput is MyLightObject_Class anotherLight)
                {
                    result += anotherLight.GetString().Length;
                }
                else
                {
                    result += anotherOutput.ToString()!.Length;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// We call string.ToString(), should allocate more
    /// </summary>
    /// <param name="heavyInput"></param>
    /// <returns></returns>
    [Benchmark]
    [ArgumentsSource(nameof(HeavyInput))]
    public async Task<long> PostWrite_object_NoChecks(MyObject_Class_ImplementsInterface heavyInput)
    {
        long result = 0;

        ChannelWriter<object> writer = _objectChannel.Writer;
        ChannelReader<object> reader = _objectChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(heavyInput);

            var output = reader.ReadAsync();
            result += (await output).ToString()!.Length;

            if (i % 15 == 0)
            {
                writer.TryWrite(LightInput);   // No need to wrap the string in MyLightObject_Class

                var anotherOutput = reader.ReadAsync();
                result += (await anotherOutput).ToString()!.Length;
            }
        }

        return result;
    }

    [Benchmark]
    [ArgumentsSource(nameof(HeavyInput))]
    public async Task<long> PostWrite_object_CheckType(MyObject_Class_ImplementsInterface heavyInput)
    {
        long result = 0;

        ChannelWriter<object> writer = _objectChannel.Writer;
        ChannelReader<object> reader = _objectChannel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(heavyInput);

            var output = await reader.ReadAsync();
            if (output is string light)
            {
                result += light.Length;
            }
            else
            {
                result += output.ToString()!.Length;
            }

            if (i % 15 == 0)
            {
                writer.TryWrite(LightInput);   // No need to wrap the string in MyLightObject_Class

                var anotherOutput = await reader.ReadAsync();
                if (anotherOutput is string anotherLight)
                {
                    result += anotherLight.Length;
                }
                else
                {
                    result += anotherOutput.ToString()!.Length;
                }
            }
        }

        return result;
    }
}

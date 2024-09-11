using BenchmarkDotNet.Attributes;
using Channels.Benchmark.Model;
using System.Threading.Channels;

namespace Channels.Benchmark;

public class PayloadType_Benchmark : BaseBenchmark
{
    private static readonly MyPayLoad _payload = new(33592128, 27, 20, [352512, 684368, 345472, 691648, 344336, 675376, 353536, 699696, 345488, 701104, 344336, 709312, 345344, 706368, 346384, 699152, 345376, 689824, 344336, 688656, 345344, 697856, 344336, 705168, 345344, 714496, 338192, 671632, 345248, 678128, 353552, 714080, 346512, 693136, 345376, 676416, 352528, 717120, 345472], -500, +500);

    private readonly MyPayLoad _rawPayload = _payload;
    private readonly MyObject_Class_ImplementsInterface _class = new(_payload);
    private readonly MyObject_Struct _struct = new(_payload);
    private readonly MyObject_ReadonlyStruct _readonlyStruct = new(_payload);
    private readonly MyObject_RecordClass _recordClass = new(_payload);
    private readonly MyObject_RecordStruct _recordStruct = new(_payload);
    private readonly MyObject_ReadonlyRecordStruct _readonlyRecordStruct = new(_payload);

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

    private readonly Channel<IMyObjectInterface> _channel = Channel.CreateBounded<IMyObjectInterface>(_boundedChannelOptions);

    [Benchmark(Baseline = true)]
    public async Task RawPayload()
    {
        ChannelWriter<IMyObjectInterface> writer = _channel.Writer;
        ChannelReader<IMyObjectInterface> reader = _channel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_rawPayload);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    public async Task Class()
    {
        ChannelWriter<IMyObjectInterface> writer = _channel.Writer;
        ChannelReader<IMyObjectInterface> reader = _channel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_class);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    public async Task Struct()
    {
        ChannelWriter<IMyObjectInterface> writer = _channel.Writer;
        ChannelReader<IMyObjectInterface> reader = _channel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_struct);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    public async Task ReadonlyStruct()
    {
        ChannelWriter<IMyObjectInterface> writer = _channel.Writer;
        ChannelReader<IMyObjectInterface> reader = _channel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_readonlyStruct);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    public async Task RecordClass()
    {
        ChannelWriter<IMyObjectInterface> writer = _channel.Writer;
        ChannelReader<IMyObjectInterface> reader = _channel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_recordClass);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    public async Task RecordStruct()
    {
        ChannelWriter<IMyObjectInterface> writer = _channel.Writer;
        ChannelReader<IMyObjectInterface> reader = _channel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_recordStruct);

            var output = reader.ReadAsync();
            await output;
        }
    }

    [Benchmark]
    public async Task ReadnlyRecordStruct()
    {
        ChannelWriter<IMyObjectInterface> writer = _channel.Writer;
        ChannelReader<IMyObjectInterface> reader = _channel.Reader;

        for (int i = 0; i < N; i++)
        {
            writer.TryWrite(_readonlyRecordStruct);

            var output = reader.ReadAsync();
            await output;
        }
    }
}

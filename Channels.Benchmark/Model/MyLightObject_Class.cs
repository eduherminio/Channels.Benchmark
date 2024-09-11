namespace Channels.Benchmark.Model;

public sealed class MyLightObject_Class
{
    private readonly string _message;

    public MyLightObject_Class(string message)
    {
        _message = message;
    }

    public override string ToString() => _message;
}

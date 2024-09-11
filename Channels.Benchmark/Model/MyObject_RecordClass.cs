using System.Text;

namespace Channels.Benchmark.Model;

public sealed record class MyObject_RecordClass : IMyObjectInterface
{
    private readonly MyPayLoad _payload;

    public MyObject_RecordClass(MyPayLoad payload)
    {
        _payload = payload;
    }

    public override string ToString()
    {
        var sb = new StringBuilder(256);

        sb.Append("info")
          .Append(" depth ").Append(_payload.Depth)
          .Append(" seldepth ").Append(_payload.DepthReached)
          .Append(" multipv 1")
          .Append(" score ").Append(_payload.Mate == default ? "cp WDL.NormalizeScore(_payload.Evaluation)" : "mate " + _payload.Mate)
          .Append(" nodes ").Append(_payload.Nodes)
          .Append(" nps ").Append(_payload.NodesPerSecond)
          .Append(" time ").Append(_payload.Time);

        if (_payload.HashfullPermill != -1)
        {
            sb.Append(" hashfull ").Append(_payload.HashfullPermill);
        }

        if (_payload.WDL is not null)
        {
            sb.Append(" wdl ")
              .Append(_payload.WDL.Value.WDLWin).Append(' ')
              .Append(_payload.WDL.Value.WDLDraw).Append(' ')
              .Append(_payload.WDL.Value.WDLLoss);
        }

        sb.Append(" pv ");
        foreach (var move in _payload.Moves)
        {
            sb.Append(move).Append(' ');
        }

        // Remove the trailing space
        if (_payload.Moves.Length > 0)
        {
            sb.Length--;
        }

        return sb.ToString();
    }
}

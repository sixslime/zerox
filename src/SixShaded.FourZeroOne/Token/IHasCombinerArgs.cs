namespace SixShaded.FourZeroOne.Token;

public interface IHasCombinerArgs<out RArgs, out ROut> : IToken<ROut>
    where RArgs : class, Res
    where ROut : class, Res
{
    public IEnumerable<IToken<RArgs>> Args { get; }
}
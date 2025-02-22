#nullable enable
namespace SixShaded.FourZeroOne.Token
{
    public interface IHasCombinerArgs<out RArgs, out ROut> : IToken<ROut>
    where RArgs : Res
    where ROut : Res
    { public IEnumerable<IToken<RArgs>> Args { get; } }
}
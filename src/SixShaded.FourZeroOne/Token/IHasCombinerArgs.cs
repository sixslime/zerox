#nullable enable
namespace SixShaded.FourZeroOne.Token
{
    public interface IHasCombinerArgs<out RArgs, out ROut> : IToken<ROut>
    where RArgs : class, ResObj
    where ROut : class, ResObj
    { public IEnumerable<IToken<RArgs>> Args { get; } }
}
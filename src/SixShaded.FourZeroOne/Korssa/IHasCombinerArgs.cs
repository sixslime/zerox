namespace SixShaded.FourZeroOne.Korssa;

public interface IHasCombinerArgs<out RArgs, out ROut> : IKorssa<ROut>
    where RArgs : class, Rog
    where ROut : class, Rog
{
    public IEnumerable<IKorssa<RArgs>> Args { get; }
}
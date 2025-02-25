namespace SixShaded.FourZeroOne.Korssa;

public interface IHasNoArgs<out RVal> : IKorssa<RVal>
    where RVal : class, Rog
{ }
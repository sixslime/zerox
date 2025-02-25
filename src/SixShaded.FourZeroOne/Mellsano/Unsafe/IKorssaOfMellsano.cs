namespace SixShaded.FourZeroOne.Mellsano.Unsafe;

public interface IKorssaOfMellsano<out R> : IKorssa<R>
    where R : class, Rog
{
    public IMellsano<R> AppliedMellsano { get; }
}
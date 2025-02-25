namespace SixShaded.FourZeroOne.Mellsano;

public interface IProxy<out R> : Rog
    where R : class, Rog
{
    public IKorssa<R> Korssa { get; }
    public MellsanoID FromMellsano { get; }
    public bool ReallowsMellsano { get; }
}
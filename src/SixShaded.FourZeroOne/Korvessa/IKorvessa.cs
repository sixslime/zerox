namespace SixShaded.FourZeroOne.Korvessa;

using Defined;

public interface IKorvessa<out R> : IKorssa<R> where R : class, Rog
{
    public Korvedu Du { get; }
    public object[] CustomData { get; }
}
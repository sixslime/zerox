namespace SixShaded.FourZeroOne.Korvessa.Unsafe;

using Defined;

public interface IKorvessa<out R> : IKorssa<R>
    where R : class, Rog
{
    public Roggi.Unsafe.IMetaFunction<R> DefinitionUnsafe { get; }
}
namespace SixShaded.DeTes.Declaration;

public interface IDeTesReference<out R>
    where R : class, Rog
{
    public IKorssa<R> Korssa { get; }
    public R Roggi { get; }
    public IOption<R> RoggiUnstable { get; }
    public IMemoryFZO Memory { get; }
}
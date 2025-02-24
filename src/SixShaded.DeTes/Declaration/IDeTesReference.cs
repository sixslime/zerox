namespace SixShaded.DeTes.Declaration;

public interface IDeTesReference<out R> where R : class, Res
{
    public IToken<R> Token { get; }
    public R Resolution { get; }
    public IOption<R> ResolutionUnstable { get; }
    public IMemoryFZO Memory { get; }
}
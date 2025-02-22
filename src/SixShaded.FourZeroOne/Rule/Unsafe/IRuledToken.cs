namespace SixShaded.FourZeroOne.Rule.Unsafe;

public interface IRuledToken<out R> : IToken<R>
    where R : class, Res
{
    public IRule<R> AppliedRule { get; }
}
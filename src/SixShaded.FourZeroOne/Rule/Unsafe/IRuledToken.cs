#nullable enable
namespace SixShaded.FourZeroOne.Rule.Unsafe
{
    public interface IRuledToken<out R> : IToken<R>
        where R : Res
    {
        public IRule<R> AppliedRule { get; }
    }
}

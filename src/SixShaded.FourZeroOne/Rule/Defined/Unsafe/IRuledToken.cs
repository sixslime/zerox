#nullable enable
using SixShaded;


#nullable enable
using SixShaded.FourZeroOne.Rule.Defined.Unsafe;

namespace SixShaded.FourZeroOne.Rule.Defined.Unsafe
{
    public interface IRuledToken<out R> : IToken<R>
        where R : class, Res
    {
        public IRule<R> AppliedRule { get; }
    }
}

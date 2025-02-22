#nullable enable
namespace SixShaded.FourZeroOne.Rule.Unsafe
{
    public interface IRule<out R>
        where R : class, Res
    {
        public RuleID ID { get; }
        public IRuleMatcher<IToken<R>> MatcherUnsafe { get; }
        public IBoxedMetaFunction<R> DefinitionUnsafe { get; }
        public IOption<IRuledToken<R>> TryApply(Token token);
    }
}

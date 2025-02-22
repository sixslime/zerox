#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record TokenBehavior<R> : IToken<R> where R : Res
    {
        public Tok[] ArgTokens { get; }
        public TokenBehavior(params Tok[] args)
        {
            ArgTokens = args;
        }
        public TokenBehavior(IEnumerable<Tok> args) : this(args.ToArray()) { }
        public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.ITokenContext tokenContext, IOption<Res>[] args) { return Resolve(tokenContext.ToHandle(), args); }
        protected abstract IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(ITokenContext runtime, IOption<Res>[] args);
        protected virtual IOption<string> CustomToString() => new None<string>();
        public sealed override string ToString()
        {
            return CustomToString()
                .OrElse(() => $"{GetType().Name}( {ArgTokens.AccumulateInto("", (msg, arg) => $"{msg}{arg} ")})");
        }
    }
}
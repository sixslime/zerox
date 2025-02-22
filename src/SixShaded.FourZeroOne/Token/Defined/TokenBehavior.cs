#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record TokenBehavior<R> : IToken<R> where R : class, ResObj
    {
        public any_token[] ArgTokens { get; }
        public TokenBehavior(params any_token[] args)
        {
            ArgTokens = args;
        }
        public TokenBehavior(IEnumerable<any_token> args) : this(args.ToArray()) { }
        public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.ITokenContext tokenContext, IOption<ResObj>[] args) { return Resolve(tokenContext.ToHandle(), args); }
        protected abstract IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(ITokenContext runtime, IOption<ResObj>[] args);
        protected virtual IOption<string> CustomToString() => new None<string>();
        public sealed override string ToString()
        {
            return CustomToString()
                .OrElse(() => $"{GetType().Name}( {ArgTokens.AccumulateInto("", (msg, arg) => $"{msg}{arg} ")})");
        }
    }
}
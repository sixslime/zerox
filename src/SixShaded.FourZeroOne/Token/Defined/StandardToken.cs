#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record StandardToken<R> : TokenBehavior<R> where R : class, Res
    {
        protected abstract ITask<IOption<R>> StandardResolve(ITokenContext runtime, IOption<Res>[] args);
        protected override IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(ITokenContext runtime, IOption<Res>[] args)
        {
            return new Ok<ITask<IOption<R>>, FZOSpec.EStateImplemented>(StandardResolve(runtime, args));
        }
        public StandardToken(params any_token[] args) : base(args) { }
        public StandardToken(IEnumerable<any_token> args) : base(args) { }
    }
}
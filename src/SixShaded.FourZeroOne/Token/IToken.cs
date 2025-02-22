#nullable enable
namespace SixShaded.FourZeroOne.Token
{
    public interface IToken<out R> where R : class, ResObj
    {
        public any_token[] ArgTokens { get; }
        public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.ITokenContext runtime, IOption<ResObj>[] args);
    }
}
namespace SixShaded.FourZeroOne.Token;

public interface IToken<out R> where R : class, Res
{
    public Tok[] ArgTokens { get; }
    public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.ITokenContext runtime, IOption<Res>[] args);
}
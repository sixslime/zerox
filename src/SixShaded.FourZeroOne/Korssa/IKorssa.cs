namespace SixShaded.FourZeroOne.Korssa;

public interface IKorssa<out R>
    where R : class, Rog
{
    public Kor[] ArgKorssas { get; }
    public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.IKorssaContext context, RogOpt[] args);
}
namespace SixShaded.VeiledOhOne;
using FourZeroOne.FZOSpec;
using FourZeroOne.Core.Roggis;
using ProcOutput = IResult<EProcessorStep, EProcessorHalt>;

public interface IVeil
{
     
}
public interface IVeiledState
{
    public IResult<VeiledState, EvalInputs> WithStep(EProcessorStep step);
}

public interface IVeiledProcessor
{
    public Task<ProcOutput> GetNextStep(IVeiledState state, IInputFZO input, IClientResolver evaluator);
}
public interface IClientOrigin where R : class, Rog
{
    public IKorssa<Number> Korssa { get; }
    public IMemoryFZO Memory { get; }
    public IStateFZO.IOrigin AsFZOOrigin();
}
public interface IClientResolver
{
    public IStateFZO UnintializedState { get; }
    public Task<IOption<Number>> Resolve(IVeiledState state);
}
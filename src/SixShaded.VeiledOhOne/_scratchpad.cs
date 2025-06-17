namespace SixShaded.VeiledOhOne;
using FourZeroOne.FZOSpec;
using FourZeroOne.Core.Roggis;
using ProcOutput = IResult<EProcessorStep, EProcessorHalt>;

public interface IVeil
{
     
}
public interface IVeiledState<out R> where R : class, Rog
{
    public IVeiledState<R> WithStep(EProcessorStep step);
}

public interface IVeiledProcessor
{
    public Task<ProcOutput> GetNextStep(IVeiledState state, IInputFZO input, IClientResolver resolver);
}

public interface IClientResolver
{
    public IStateFZO UnintializedState { get; }
    public Task<IOption<Number>> Resolve(IVeiledState<Number> state);
}
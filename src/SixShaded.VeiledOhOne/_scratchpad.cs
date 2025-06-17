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
    public Task<IResult<ProcOutput, EvalInputs>> GetNextStep(IVeiledState, );
}
public class EvalInputs
{
    public IStateFZO State { get; }
    public IMemoryFZO Memory { get; }
}

public interface IVeiledEvaluator
{
    public IOption<Number> Evaluate(IStateFZO.IOrigin origin)
}
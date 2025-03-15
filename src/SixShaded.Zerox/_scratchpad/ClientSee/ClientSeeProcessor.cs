namespace SixShaded.Zerox._scratchpad.ClientSee;
using FourZeroOne.FZOSpec;

public class ClientSeeProcessor(IProcessorFZO implementation)
{
    public IProcessorFZO Implementation { get; } = implementation;
    public Task<IResult<EProcessorStep, EProcessorHalt>> GetNextStep(IStateFZO state, Func<int, IInputFZO> clientMap)
    {

    }
}

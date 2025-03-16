namespace SixShaded.Zerox._scratchpad.ClientSee;
using FourZeroOne.FZOSpec;

public class ClientSeeProcessor
{
    public required IProcessorFZO Implementation { get; init; }

    public Task<IResult<EProcessorStep, EProcessorHalt>> GetNextStep(IStateFZO state, Func<int, IInputFZO> clientMap)
    {
        int currentClient =
            state
                .OperationStack
                .First()
                .MemoryStack
                .GetAt(0)
                .Unwrap()
                .GetObject(Rodais.CURRENT_CLIENT)
                .Unwrap()
                .GetComponent(Client.ID)
                .Unwrap()
                .Value;
        return Implementation.GetNextStep(state, clientMap(currentClient));
    }
}

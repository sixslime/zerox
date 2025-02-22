
namespace SixShaded.FourZeroOne.FZOSpec;

public interface IProcessorFZO
{
    public ITask<IResult<EProcessorStep, EProcessorHalt>> GetNextStep(IStateFZO state, IInputFZO input);

    public interface ITokenContext
    {
        public IMemoryFZO CurrentMemory { get; }
        public IInputFZO Input { get; }
    }
}
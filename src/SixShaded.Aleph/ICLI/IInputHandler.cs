namespace SixShaded.Aleph.ICLI;

internal interface IInputHandler
{
    public IOption<EInputProtocol> ShouldHandle(State.ProgramState state);
}
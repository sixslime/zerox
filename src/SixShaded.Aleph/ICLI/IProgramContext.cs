namespace SixShaded.Aleph.ICLI;

internal interface IProgramContext
{
    public State.ProgramState State { get; set; }
    public void SendEvent(IProgramEvent action);
    public void SendTerminationRequest();
}
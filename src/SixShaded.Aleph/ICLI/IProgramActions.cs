namespace SixShaded.Aleph.ICLI;
using State;
internal interface IProgramActions
{
    public ProgramState State { get; }
    public void DoInput(Config.AlephKeyPress key);
    public void SetState(Func<ProgramState, ProgramState> changeFunction);
    public void Quit();
}
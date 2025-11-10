namespace SixShaded.Aleph.ICLI.InputHandlers;
using Logical;
using State;

internal class ProgressorSelectInputHandler : IInputHandler
{
    private static Progressor[] _forwardProgressors =
    [
        new()
        {
            Name = "Next Operation",
            StopConditionDescription = "a new operation is pushed to the stack",
            Function =
                async context =>
                {
                    while ((await context.Next()).Check(out var step) && step.NextStep.CheckOk(out var pstep) && pstep is not EProcessorStep.PushOperation) { }
                    await context.Next();
                },
        }
    ];

    public void Tick(bool active, ProgramState state)
    {

    }
    public IOption<EInputProtocol> ShouldHandle(ProgramState state) => 
    (state.GetCurrentSession().UIContext is ESessionUIContext.SelectingProgressor s)
}
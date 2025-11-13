namespace SixShaded.Aleph.ICLI.InputHandlers;
using Logical;
using State;
using Formatting;

internal class ProgressorSelectInputHandler : IInputHandler
{
    private readonly Helpers.SelectionHelper _selectionHelper =
        new()
        {
            EntryGenerator =
                state =>
                    GetProgressors(state).Map(
                    progressor =>
                        ConsoleText.Text(progressor.Name)
                            .Format(TextFormat.Object)
                            .Build())
                    .ToArray(),
            SelectAction =
                (index, actions) =>
                {
                    actions.SendProgressor(GetProgressors(actions.State)[index]);
                    actions.BackToTopLevel();
                },
            CancelAction =
                actions =>
                {
                    actions.BackToTopLevel();
                },
        };
    private static readonly Progressor[] FORWARD_PROGRESSORS =
    [
        new()
        {
            Name = "Next Resolution",
            StopConditionDescription = "any operation is resolved",
            Backward = false,
            Function =
                async context =>
                {
                    while ((await context.Next()).Check(out var step) && step.NextStep.CheckOk(out var pstep) && pstep is not EProcessorStep.Resolve) { }
                    await context.Next();
                },
        },
        new()
        {
            Name = "Next Operation",
            StopConditionDescription = "a new operation is pushed to the stack",
            Backward = false,
            Function =
                async context =>
                {
                    while ((await context.Next()).Check(out var step) && step.NextStep.CheckOk(out var pstep) && pstep is not EProcessorStep.PushOperation) { }
                    await context.Next();
                },
        },
        new()
        {
            Name = "Next Identity Korssa",
            StopConditionDescription = "an identity (0 arguement) korssa is pushed to the stack",
            Backward = false,
            Function =
                async context =>
                {
                    while ((await context.Next()).Check(out var step) && step.NextStep.CheckOk(out var pstep) && (pstep is not EProcessorStep.PushOperation p || p.OperationKorssa.ArgKorssas.Length != 0)) { }
                    await context.Next();
                },
        },
    ];
    private static readonly Progressor[] BACKWARD_PROGRESSORS =
    [
        new()
        {
            Name = "Previous Operation",
            StopConditionDescription = "the previous operation was pushed to the stack",
            Backward = true,
            Function =
                async context =>
                {
                    while ((await context.Next()).Check(out var step) && step.NextStep.CheckOk(out var pstep) && pstep is not EProcessorStep.PushOperation) { }
                    await context.Next();
                },
        }
    ];

    private static Progressor[] GetProgressors(ProgramState state) =>
        (state.GetCurrentSession().UIContext.IsA<ESessionUIContext.SelectingProgressor>().Backward)
            ? BACKWARD_PROGRESSORS
            : FORWARD_PROGRESSORS;
    public void Tick(bool active, ProgramState state)
    {
        _selectionHelper.Tick(active, state);
    }

    public IOption<EInputProtocol> ShouldHandle(ProgramState state) => 
        (state.GetCurrentSession().UIContext is ESessionUIContext.SelectingProgressor && !state.GetCurrentSession().GetLogicalSession().InProgress).ToOption(_selectionHelper.Protocol);
}
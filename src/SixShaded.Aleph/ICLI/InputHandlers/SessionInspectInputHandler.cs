namespace SixShaded.Aleph.ICLI.InputHandlers;

using Config;
using State;

internal class SessionInspectInputHandler : IInputHandler
{
    private static readonly EInputProtocol.Keybind RETURN_VAL =
        new EInputProtocol.Keybind()
        {
            ContextDescription = "Inspecting session",
            ActionMap =
                new PMap<EKeyFunction, InputAction>(
                new()
                {
                    {
                        EKeyFunction.Exit, InputAction.Exit
                    },
                    {
                        EKeyFunction.SessionShowOperationExpansions, InputAction.ShowCurrentOperationExpansions
                    },
                    {
                        EKeyFunction.SessionShowOperationStack, InputAction.ShowCurrentOperationStack
                    },
                    {
                        EKeyFunction.QuickInfo, InputAction.QuickInfo
                    },
                    {
                        EKeyFunction.SessionProgressForward, new()
                        {
                            Name = "forward progress",
                            Description = "Progress the current session forward by a selected progresor.",
                            ActionFunction =
                                actions =>
                                {
                                    actions.SetState(
                                    state =>
                                        state.WithCurrentSession(
                                        session =>
                                            session with
                                            {
                                                UIContext =
                                                new ESessionUIContext.SelectingProgressor()
                                                {
                                                    Backward = false
                                                }
                                            }));
                                }
                        }
                    },
                    {
                        EKeyFunction.SessionProgressBackward, new()
                        {
                            Name = "backward progress",
                            Description = "Reverse progress in the current session by a selected progresor.",
                            ActionFunction =
                                actions =>
                                {
                                    actions.SetState(
                                    state =>
                                        state.WithCurrentSession(
                                        session =>
                                            session with
                                            {
                                                UIContext =
                                                new ESessionUIContext.SelectingProgressor()
                                                {
                                                    Backward = true
                                                }
                                            }));
                                }
                        }
                    },
                })
        };
    public IOption<EInputProtocol> ShouldHandle(ProgramState state) => (state.GetCurrentSession().UIContext is ESessionUIContext.TopLevel).ToOption(RETURN_VAL);

    public void Tick(bool isActiveHandler, ProgramState state)
    { }
}
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
                })
        };
    public IOption<EInputProtocol> ShouldHandle(ProgramState state) => RETURN_VAL.AsSome();
}
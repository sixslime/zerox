namespace SixShaded.Aleph.ICLI.InputHandlers;

using Config;
using State;
internal class LimboInputHandler : IInputHandler
{
    private static readonly EInputProtocol RETURN_VAL =
        new EInputProtocol.Keybind()
        {
            ContextDescription = "Waiting for first session",
            ActionMap =
                new PMap<EKeyFunction, InputAction>(
                new()
                {
                    {
                        EKeyFunction.Exit, InputAction.Exit
                    }
                })
        };

    public void Tick(bool _)
    { }

    public IOption<EInputProtocol> ShouldHandle(ProgramState state) => (state.Sessions.Count == 0 && state.HiddenSessions.Count == 0).ToOption(RETURN_VAL);
}
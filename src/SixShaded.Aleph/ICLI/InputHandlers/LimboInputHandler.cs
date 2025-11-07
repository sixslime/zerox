namespace SixShaded.Aleph.ICLI.InputHandlers;

using Config;
using State;

internal class LimboInputHandler : IInputHandler
{
    private static readonly EInputProtocol RETURN_VAL =
        new EInputProtocol.Keybind()
        {
            ActionMap =
                new PMap<EKeyFunction, InputAction>(
                new()
                {
                    {
                        EKeyFunction.Quit, InputAction.Quit
                    }
                })
        };
    public IOption<EInputProtocol> ShouldHandle(ProgramState state) => RETURN_VAL.AsSome();
}
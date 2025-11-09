namespace SixShaded.Aleph.ICLI.InputHandlers.Helpers;
using Formatting;
internal class SelectionHelper
{
    public EInputProtocol.Direct Protocol { get; } = new()
    {
        A
    }
    public required ConsoleText[] AvailableSelections { get; init; }
    public required Action<int, IProgramActions> 
    private void HandleKeypress(Key)
}
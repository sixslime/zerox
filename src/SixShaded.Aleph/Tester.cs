namespace SixShaded.Aleph;
using MinimaFZO;
using Logical;
using FourZeroOne.Core.Syntax;
using FourZeroOne.Core.Roggis;
using ICLI;
internal class Tester
{
    public static async Task Test(AlephArgs args)
    {

    }

    private static void HandleSelection(object? sender, SelectionPromptedEventArgs args)
    {
        Console.WriteLine("SELECT START");
        _ = Select(args);
    }

    private static async Task Select(SelectionPromptedEventArgs args)
    {
        await Task.Delay(10);
        Console.WriteLine("SELECT END");
        args.Callback.Select([0]);
    }
    private class Origin : IStateFZO.IOrigin
    {
        public required Kor Program { get; init; }
        public required IMemoryFZO InitialMemory { get; init; }
    }

    
}
namespace SixShaded.Aleph;
using MinimaFZO;
using Logical;
using FourZeroOne.Core.Syntax;
using FourZeroOne.Core.Roggis;
internal class Tester
{
    public static async Task Test(AlephConsole console)
    {
        IStateFZO state = new MinimaStateFZO().Initialize(
        new Origin()
        {
            InitialMemory = new MinimaMemoryFZO(),
            Program = 
                (1..5)
                .kFixed()
                .kMap(iX => iX.kRef().kYield().kIOSelectOne().kMultiply(10.kFixed()))
        });
        Master.Instance.AddSession(state);
        Master.Instance.SwitchSession(0);
        Master.Instance.CurrentSession.Unwrap().SelectionPromptedEvent += HandleSelection;
        Console.WriteLine(Master.Instance.CurrentSession);
        await Master.Instance.CurrentSession.Unwrap().Progress(new FullProgressor());

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

    private class FullProgressor : IProgressor
    {
        public string Identifier => "full";

        public async Task Consume(IProgressionContext context)
        {
            while ((await context.Next()).Check(out var step))
            {
                if (step.NextStep.Split(out var ok, out var err))
                {
                    Console.WriteLine(ok);
                }
                else
                {
                    Console.WriteLine(err);
                    Console.WriteLine(string.Join("\n", err.HaltingState.KorssaMutationStack));
                    Console.WriteLine(string.Join("\n", err.HaltingState.OperationStack));
                }
            }
        }
    }
}
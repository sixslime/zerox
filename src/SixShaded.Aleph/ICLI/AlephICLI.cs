namespace SixShaded.Aleph.ICLI;

using Logical;
using Language;
using MinimaFZO;
using System.Threading.Channels;

public static class AlephICLI
{
    internal static Channel<EProgramEvent> EventsChannel { get; } =
        Channel.CreateUnbounded<EProgramEvent>(
        new()
        {
            AllowSynchronousContinuations = true,
            SingleReader = true,
            SingleWriter = false,
        });

    public static IAlephICLIHandle Run(AlephArgs args)
    {
        if (Master.IsInitialized)
            throw new("Aleph is already initialized.");
        Master.Init(
        new()
        {
            LanguageKey = args.LanguageKey,
            Processor = args.Processor,
        });
        Task.Run(ProgramLoop);
        return new Handle();
    }

    private static async Task ProgramLoop()
    {

    }
    private static async Task Shutdown()
    {

    }

    private class Handle : IAlephICLIHandle
    {
        public async Task AddSession(IStateFZO rootState)
        {
            await EventsChannel.Writer.WriteAsync(
            new EProgramEvent.SessionAdd
            {
                RootState = rootState
            });
        }
        public async Task Stop()
        {
            await EventsChannel.Writer.WriteAsync(new EProgramEvent.StopProgram());
        }
    }
}

public interface IAlephICLIHandle
{
    public Task AddSession(IStateFZO rootState);
    public Task Stop();
}

internal abstract record EProgramEvent
{
    public sealed record KeyPress : EProgramEvent
    {
        public required ConsoleKeyInfo KeyInfo { get; init; }
    }

    public sealed record StopProgram : EProgramEvent;

    public sealed record SessionAdd : EProgramEvent
    {
        public required IStateFZO RootState { get; init; }
    }

    public sealed record Select : EProgramEvent
    {
        public required int[] Selection { get; init; }
    }
}
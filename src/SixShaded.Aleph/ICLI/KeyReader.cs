namespace SixShaded.Aleph.ICLI;
using System.Threading;
using ProgramEvents;

internal class KeyReader : IDisposable
{
    public static KeyReader Link(IProgramContext context, int pollInterval) => new(context, pollInterval);
    public IProgramContext LinkedProgram { get; }
    public int PollInterval { get; }

    private KeyReader(IProgramContext program, int pollInterval)
    {
        LinkedProgram = program;
        PollInterval = pollInterval;
        _isActive = true;
        _thread =
            new(Loop)
            {
                IsBackground = true,
                Name = "AlephConsole KeyReader Thread",
            };
        _thread.Start();
    }
    private bool _isActive;
    private readonly Thread _thread;
    private readonly object _activeLock = new();

    public void Dispose()
    {
        lock (_activeLock) _isActive = false;
    }
    public void Loop()
    {
        while (true)
        {
            lock (_activeLock)
            {
                if (!_isActive) return;
                while (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    LinkedProgram.SendEvent(
                    new KeyPressed()
                    {
                        KeyInfo = key,
                    });
                }
            }
            Thread.Sleep(PollInterval);
        }
    }
}
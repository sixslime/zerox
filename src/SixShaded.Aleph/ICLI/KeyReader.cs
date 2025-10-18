namespace SixShaded.Aleph.ICLI;
using System.Threading;
internal static class KeyReader
{

    private static bool _isActive = false;
    public static bool IsActive {
        get
        {
            lock (ACTIVE_LOCK) return _isActive;
        }
        private set
        {
            lock (ACTIVE_LOCK) _isActive = value;
        }
    }
    private static Thread? _thread;
    private static readonly object ACTIVE_LOCK = new();
    public static void Start()
    {
        if (IsActive) return;
        IsActive = true;
        _thread =
            new(Loop)
            {
                IsBackground = true,
                Name = "AlephConsole KeyReader Thread"
            };
        _thread.Start();
    }

    public static void Stop()
    {
        if (!IsActive) return;
        IsActive = false;
        _thread = null;
    }
    public static void Loop()
    {
        while (IsActive)
        {
            var key = Console.ReadKey(true);
            AlephICLI.FireEventAndForget(
            new EProgramEvent.KeyPressed()
            {
                KeyInfo = key
            });
        }
    }
}
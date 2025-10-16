namespace SixShaded.Aleph;
using Logical;
using Language;
using Views;

public class AlephConsole
{
    private static AlephConsole? _instance;
    public static void Run(RunArgs args, Action<AlephConsole> postInitializationAction)
    {
        if (_instance is not null) throw new Exception("Already Started");
        _instance = new();
        Master.Init(args.LanguageProvider);
        // yea idk
        Application.Init();
        Application.AddTimeout(
        TimeSpan.Zero, () =>
        {
            postInitializationAction(_instance);
            return false;
        });
        Application.Run<Main>().Dispose();
        Application.Shutdown();
    }
}
namespace SixShaded.Aleph;
using Logical;
using Language;
using Views;

public class AlephConsole
{
    private static AlephConsole? _instance;
    public static void Start(StartArgs args, Action<AlephConsole> postInitializationAction)
    {
        if (_instance is not null) throw new Exception("Already Started");
        _instance = new();
        Master.Init(args.LanguageProvider);
        // TODO
    }
}
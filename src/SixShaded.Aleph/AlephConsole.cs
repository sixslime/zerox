namespace SixShaded.Aleph;
using Logical;
using Language;
using MinimaFZO;
using Views;

public class AlephConsole
{
    private static AlephConsole? _instance;
    private RunArgs _args;
    private AlephConsole(RunArgs args)
    {
        _args = args;
    }
    public static void Run(RunArgs args, Action<AlephConsole> postInitializationAction)
    {
        if (_instance is not null) throw new Exception("Already Started");
        _instance = new(args);
        Master.Init(new()
        {
            LanguageProvider = args.LanguageProvider,
            Processor = args.Processor,
        });
        Application.Init();
        Application.AddTimeout(
        TimeSpan.Zero, () =>
        {
            Task.Run(() => postInitializationAction(_instance));
            return false;
        });
        Application.Run<Main>().Dispose();
        Application.Shutdown();
    }

    public static async Task Test()
    {
        _instance = new(new()
        {
            LanguageProvider = new(new Language.Builtin.Keys.StandardCoreKey()),
            Processor = new MinimaProcessorFZO(),
        });
        Master.Init(new()
        {
            LanguageProvider = _instance._args.LanguageProvider,
            Processor = _instance._args.Processor,
        });
        await Tester.Test(_instance);
    }
    public void AddSession(IStateFZO rootState)
    {
        Application.Invoke(
        () =>
            Master.Instance.AddSession(rootState));
    }
}
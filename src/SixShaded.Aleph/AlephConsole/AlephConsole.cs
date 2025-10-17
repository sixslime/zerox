namespace SixShaded.Aleph.AlephConsole;
using Logical;
using Language;
using MinimaFZO;

public class AlephConsole
{
    private static AlephConsole? _instance;
    private readonly RunArgs _args;
    private AlephConsole(RunArgs args)
    {
        _args = args;
    }

    public static AlephConsole Init(RunArgs args)
    {
        if (Master.IsInitialized)
            throw new Exception("Aleph is already initialized.");
        _instance = new(args);
        Master.Init(
        new()
        {
            LanguageKey = args.LanguageKey,
            Processor = args.Processor,
        });
        return _instance;
    }

    public void Run()
    {

    }

    public void AddSession(IStateFZO rootState)
    {
        Master.Instance.AddSession(rootState);
    }
}
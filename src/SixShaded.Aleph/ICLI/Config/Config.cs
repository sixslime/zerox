namespace SixShaded.Aleph.ICLI.Config;

using ConfigTable = Dictionary<string, object>;
using IConfigTable = IDictionary<string, object>;

internal static class Config
{
    private static readonly Lazy<EvaluatedConfig> CONFIG = new(GetConfig);
    private static readonly ConfigTable DEFAULT_CONFIG =
        new()
        {
            {
                "keybinds", new ConfigTable
                {
                    {
                        "back", "q"
                    },
                    {
                        "forward", "w"
                    },
                    {
                        "up", "k"
                    },
                    {
                        "down", "j"
                    },
                    {
                        "left", "h"
                    },
                    {
                        "right", "l"
                    },
                    {
                        "submit", "(enter)"
                    },
                    {
                        "help", "g"
                    },
                }
            },
        };
    private static readonly Dictionary<string, EKeyFunction> KEYFUNCTION_MAP =
        new()
        {
            {
                "back", EKeyFunction.Back
            },
            {
                "up", EKeyFunction.Up
            },
            {
                "down", EKeyFunction.Down
            },
            {
                "left", EKeyFunction.Left
            },
            {
                "right", EKeyFunction.Right
            },
            {
                "submit", EKeyFunction.Submit
            },
            {
                "forward", EKeyFunction.Forward
            },
            {
                "help", EKeyFunction.Help
            },
        };
    internal static IPMap<AlephKeyPress, EKeyFunction> Keybinds => CONFIG.Value.Keybinds;
    private static readonly Lazy<IPMap<EKeyFunction, IPSequence<AlephKeyPress>>> REVERSE_KEYBIND_LOOKUP =
        new(
        () =>
        {
            var dict = new Dictionary<EKeyFunction, IPSequence<AlephKeyPress>>();
            foreach (var pair in Keybinds.Elements)
            {
                if (dict.TryGetValue(pair.B, out var list))
                {
                    dict[pair.B] = list.WithEntries(pair.A);
                }
                else
                {
                    dict[pair.B] = new PSequence<AlephKeyPress>([pair.A]);
                }
            }
            return dict.ToPMap();
        });
    public static IPMap<EKeyFunction, IPSequence<AlephKeyPress>> ReverseKeybindLookup => REVERSE_KEYBIND_LOOKUP.Value;

    private static EvaluatedConfig GetConfig()
    {
        var rawConfig = new ConfigTable(DEFAULT_CONFIG);
        string configFilePath = Path.Join(Environment.CurrentDirectory, "config.toml");
        string[] args = Environment.GetCommandLineArgs().Skip(1).Map(x => x.Split('=')).Flatten().ToArray();
        for (int i = 0; i < args.Length - 1; i++)
        {
            if (args[i] != "--config") continue;
            configFilePath = args[i + 1];
            break;
        }
        if (File.Exists(configFilePath))
        {
            var customConfig = Toml.ToModel(File.ReadAllText(configFilePath));
            MergeConfig(rawConfig, customConfig);
        }
        var keybindMap = new Dictionary<AlephKeyPress, EKeyFunction>();
        foreach ((string actionString, object val) in (IConfigTable)rawConfig["keybinds"])
        {
            if (!KEYFUNCTION_MAP.TryGetValue(actionString, out var keyFunction)) continue;
            if (val is not string keystr) continue;
            string[] keyArr = keystr.Split(' ');
            bool shift = false;
            bool alt = false;
            bool ctrl = false;
            foreach (string mod in keyArr[..^1])
            {
                switch (mod.ToLower())
                {
                case "s":
                    shift = true;
                    continue;
                case "a":
                    alt = true;
                    continue;
                case "c":
                    ctrl = true;
                    continue;
                }
            }
            var alephKey =
                new AlephKeyPress
                {
                    Alt = alt,
                    Shift = shift,
                    Control = ctrl,
                    KeyString = keyArr[^1],
                };
            keybindMap[alephKey] = keyFunction;
        }
        return new()
        {
            Keybinds = new PMap<AlephKeyPress, EKeyFunction>(keybindMap),
        };
    }

    private static void MergeConfig(IConfigTable baseConfig, IConfigTable mergingConfig)
    {
        foreach ((string key, object value) in mergingConfig)
        {
            if (!baseConfig.TryGetValue(key, out object? baseVal)) continue;
            if (baseVal is IConfigTable baseNested)
            {
                if (value is not IConfigTable nested) continue;
                MergeConfig(baseNested, nested);
                continue;
            }
            baseConfig[key] = value;
        }
    }

    private class EvaluatedConfig
    {
        internal required IPMap<AlephKeyPress, EKeyFunction> Keybinds { get; init; }
    }
}
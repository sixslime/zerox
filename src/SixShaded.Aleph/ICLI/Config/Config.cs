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
                        "back", ";"
                    },
                    {
                        "submit", "(enter)"
                    },
                    {
                        "help", "q"
                    },
                    {
                        "exit", "(escape)"
                    },
                    {
                        "quickInfo", "u"
                    },
                    {
                        "session.showOperationStack", "e"
                    },
                    {
                        "session.showOperationExpansions", "o"
                    },
                    {
                        "session.showMemory", "a"
                    },
                    {
                        "session.progressForward", "n"
                    },
                    {
                        "session.progressBackward", "s n"
                    },
                    {
                        "session.makeBranch", "c"
                    }
                }
            },
            {
                "selection", new ConfigTable
                {
                    {
                        "indicators", "uehtoansfycr"
                    },
                    {
                        "submit", "(enter)"
                    },
                    {
                        "cancel", ";"
                    },
                    {
                        "show", "q"
                    },
                    {
                        "delete", "(backspace)"
                    },
                }
            }
        };
    private static readonly Dictionary<string, EKeyFunction> KEYFUNCTION_MAP =
        new()
        {
            {
                "back", EKeyFunction.Back
            },
            {
                "submit", EKeyFunction.Submit
            },
            {
                "help", EKeyFunction.Help
            },
            {
                "exit", EKeyFunction.Exit
            },
            {
                "quickInfo", EKeyFunction.QuickInfo
            },
            {
                "session.showOperationStack", EKeyFunction.SessionShowOperationStack
            },
            {
                "session.showOperationExpansions", EKeyFunction.SessionShowOperationExpansions
            },
            {
                "session.progressForward", EKeyFunction.SessionProgressForward
            },
            {
                "session.progressBackward", EKeyFunction.SessionProgressBackward
            },
            {
                "session.showMemory", EKeyFunction.SessionShowMemory
            },
            {
                "session.makeBranch", EKeyFunction.SessionBranch
            },
        };
    internal static IPMap<AlephKeyPress, EKeyFunction> Keybinds => CONFIG.Value.Keybinds;
    internal static SelectionKeys Selection => CONFIG.Value.Selection;
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
            if (val is not string keystr) throw new ConfigKeyException($"keybinds.{actionString}", "string expected");
            keybindMap[StringToAlephKey(keystr)] = keyFunction;
        }
        var selectionConfig = (IConfigTable)rawConfig["selection"];
        // a little atrocious but its whatever
        var selectionKeys =
            new SelectionKeys()
            {
                Cancel = selectionConfig["cancel"] is string cancelstr ? StringToAlephKey(cancelstr) : throw new ConfigKeyException("selection.cancel", "string expected"),
                Submit = selectionConfig["submit"] is string submitstr ? StringToAlephKey(submitstr) : throw new ConfigKeyException("selection.submit", "string expected"),
                Delete = selectionConfig["delete"] is string deletestr ? StringToAlephKey(deletestr) : throw new ConfigKeyException("selection.delete", "string expected"),
                ShowAvailable = selectionConfig["show"] is string showstr ? StringToAlephKey(showstr) : throw new ConfigKeyException("selection.show", "string expected"),
                Indicators = selectionConfig["indicators"] is string indicators ? indicators.ToLower() : throw new ConfigKeyException("selection.indicators", "string expected"),
            };
        return new()
        {
            Selection = selectionKeys,
            Keybinds = new PMap<AlephKeyPress, EKeyFunction>(keybindMap),
        };
    }

    private static AlephKeyPress StringToAlephKey(string keystr)
    {
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
        return
            new AlephKeyPress
            {
                Alt = alt,
                Shift = shift,
                Control = ctrl,
                KeyString = keyArr[^1],
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
        public required SelectionKeys Selection { get; init; }
        public required IPMap<AlephKeyPress, EKeyFunction> Keybinds { get; init; }
    }
}
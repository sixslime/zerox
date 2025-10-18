namespace SixShaded.Zerox;

using System.Reflection;
using SixShaded.FourZeroOne;
using SixShaded.FourZeroOne.Core.Syntax;
using FZOTypeMatch;
using FZOTypeMatch.Syntax;
using CoreTypeMatcher;
using Types = CoreTypeMatcher.Types;
using MinimaFZO;
using FourZeroOne.FZOSpec;
using Aleph;
using Aleph.Language.Builtin.Keys;
using SixShaded.SixLib.ICEE;

internal class Program
{
    public static async Task Main(string[] args)
    {
        Init();
        char[] buf = new char[5];

        Console.WriteLine("\x1b[1mThis text is bold.\x1b[0m");
    }

    private static void Loop(string msg)
    {
        while (true)
        {
            var e = Console.ReadKey();
            Console.WriteLine(msg);
        }
    }

    private static void Init()
    {
        Master.RegisterAxoi<FourZeroOne.Axois.Infinite.Axoi>();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            if (!assembly.FullName!.StartsWith("SixShaded.FourZeroOne.Axois")) continue;
            Console.WriteLine($"=== {assembly.FullName} ===");
            foreach (var type in assembly.GetExportedTypes())
            {
                Console.WriteLine(type.Name);
                foreach (var field in type.GetFields())
                {
                    Console.WriteLine($" - {field.Name}");
                }
            }
        }
    }
    private static void Log(object obj)
    {
        Console.WriteLine("LOG:");
        Console.WriteLine(obj);
    }
}

/* LANG (not up to date / recovered)
 * Token -> Korssa
 * Macro -> Korvessa
 * Resolution -> Roggi
 * Composition -> Roveggi
 * CompositionType -> Rovetu
 * ComponentIdentifier -> Rovu
 * Rule -> Mellsano
 * Matcher -> Ullasem
 * Package/Plugin -> Axoi
 *
 * 'v' ~ part
 * 'u' ~ identify
 * 've' ~ sum of parts
 * 'du' ~ data identifier
 * 'tu' ~ type identifier
 * 'sem' ~ comparison / match
 * 'no' ~ mutate
 * 'sa' ~ expression / literal relation to korssa
 * 'ro' ~ final / literal relation to roggi
 * '.n' ~ concept of
 *
 * SHAKY
 * 'a' ~ address?
 * 'da' ~ memory?
 *
 * perhaps rename
 * Korvessa -> Korssave
 * Roveggi -> Roggive
 *
 * NOLLA CHAR : \u2205
 */

/* DEV
 * mabye change "Axoi" to "Axxoc"
 */

/* TODO
 * - consider forcing all references to 'Multi<T>' to be 'IMulti<T>' (even in outputs), similar to how its always 'IRoveggi<C>', not 'Roveggi<C>'
 * - rework how axois/axovendus work.
 */
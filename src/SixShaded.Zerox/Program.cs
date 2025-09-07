namespace SixShaded.Zerox;

using System.Reflection;
using SixShaded.FourZeroOne;
using SixShaded.FourZeroOne.Core.Syntax;
using FZOTypeMatch;
using FZOTypeMatch.Syntax;
using CoreTypeMatcher;
using MinimaFZO;
using FourZeroOne.FZOSpec;
internal class Program
{
    public static async Task Main(string[] args)
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
        var typeMatch = new FZOTypeMatch([new CoreTypeMatcher()]);
        var testKorssa = (0..7).kFixed().kMap(iN => true.kFixed());
        Log(testKorssa);
        Log(testKorssa.FZOTypeInfo(typeMatch));
        Log(1.kFixed().FZOTypeInfo(typeMatch));
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
 * CompositionType -> Roveggitu
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
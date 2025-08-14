namespace SixShaded.Zerox;

using System.Reflection;
using SixShaded.FourZeroOne;
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
 * 
 */

/* TODO
 * assigning korssas should assign nolla (as opposed to just not assigning if data is nolla, how it is now).
 */
using MorseCode.ITask;
using FourZeroOne.FZOSpec;
using FourZeroOne.Token;
using Res = FourZeroOne.Resolution.IResolution;
using ResOpt = SixShaded.NotRust.IOption<FourZeroOne.Resolution.IResolution>;
using DeTes.Declaration;
using DeTes.Analysis;
using DeTes;
using SixShaded.NotRust;
#nullable enable
// does not check full coverage inside domains, but partial coverage may be intended in domains, so its tough.
namespace DeTesAssertIntegrity
{
    public static class DeTesAssertIntegritySyntax
    {
        public static IDeTesTest[] GenerateAssertIntegrityTests(this IDeTesTest forTest)
        {
            var contexts = new DeTesAssertIntegrityContextProvider().GetSanityContexts(forTest.Token);
            var o = new IDeTesTest[contexts.Length];
            foreach (var (i, sanityContext) in contexts.Enumerate())
            {
                o[i] = new DeTest
                {
                    InitialMemory = forTest.InitialMemory,
                    Token = C => forTest.Token(sanityContext.WithImplementingContext(C))
                };
            }
            return o;
        }
    }
    
    public class UnexpectedDeTesUseException() : Exception("DeTesSanity expects all DeTes TokenDeclaration objects (references, domains, etc.) to only be captured/used within non-immediately executing functions (such as assertion statements). This expectation was not upheld."), IKnownException { }
    public class InternalDeTesAssertIntegrityException(Exception inner) : Exception($"Internal DeTesSanity error.", inner) { }
    
}
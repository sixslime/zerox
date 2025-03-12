// does not check full coverage inside domains, but partial coverage may be intended in domains, so its tough.

namespace SixShaded.DeTesAssertIntegrity;

using DeTes;
using DeTes.Declaration;

public static class DeTesAssertIntegritySyntax
{
    public static IDeTesTest[] GenerateAssertIntegrityTests(this IDeTesTest forTest)
    {
        var contexts = new DeTesAssertIntegrityContextProvider().GetSanityContexts(forTest.Declaration);
        var o = new IDeTesTest[contexts.Length];
        foreach ((int i, var sanityContext) in contexts.Enumerate())
        {
            o[i] =
                new DeTest
                {
                    InitialMemory = forTest.InitialMemory,
                    Declaration = C => forTest.Declaration(sanityContext.WithImplementingContext(C)),
                };
        }
        return o;
    }
}

public class UnexpectedDeTesUseException() : Exception("DeTesSanity expects all DeTes KorssaDeclaration objects (references, domains, etc.) to only be captured/used within non-immediately executing functions (such as assertion statements). This expectation was not upheld."),
    IKnownException
{ }

public class InternalDeTesAssertIntegrityException(Exception inner) : Exception("Internal DeTesSanity error.", inner)
{ }
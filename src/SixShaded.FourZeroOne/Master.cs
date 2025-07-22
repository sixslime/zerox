namespace SixShaded.FourZeroOne;

// TODO
// temporary name, this is the equavalent of an "Assembly" of 401
public class Master
{
    /// <summary>
    /// type parameter must be the "Axoi" class of an axoi. <br></br>
    /// It's so stupid it just might work.
    /// </summary>
    /// <typeparam name="A"></typeparam>
    public static void RegisterAxoi<A>() where A : IsAxoi
    {
        // TODO
        // this will use reflection construct the mapping for abstract Rovetus of a given Axoi.
    }
    static Master()
    {

    }
}
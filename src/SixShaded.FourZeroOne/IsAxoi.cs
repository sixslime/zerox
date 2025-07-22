namespace SixShaded.FourZeroOne;


/// <summary>
/// Axoi classes must be functionally static and never be constructed (and have no public constructor). <br></br>
/// Ok, I think I've outdone myself; this is officially the most retarded system I've ever designed.
/// </summary>
public abstract class IsAxoi
{
    protected IsAxoi()
    {
        throw new InvalidOperationException("Instantiating an Axoi class is not allowed (Axoi classes must be functionally static).");
    }
}
namespace SixShaded.FourZeroOne;


public abstract class IsAxoi
{
    protected abstract string Name { get; }

    protected IsAxoi()
    {
        throw new Exception("Axois should not be directly instantiated; only by the FourZeroOne assembly process.");
    }

    // accessed via reflection in assembler.
    private IsAxoi(AxoiCreationKey assemblerKey)
    {

    }

    // accessed via reflection in assembler.
    internal class AxoiCreationKey
    {
        private AxoiCreationKey()
        { }
    }
}
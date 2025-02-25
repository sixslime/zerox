namespace SixShaded.FourZeroOne.Resolution.Unsafe;

public interface IComponentIdentifier<in C> : IComponentIdentifier where C : ICompositionType
{ }

public interface IComponentIdentifier
{
    public string Identity { get; }
    public string Package { get; }
}
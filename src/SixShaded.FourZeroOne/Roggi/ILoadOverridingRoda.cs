namespace SixShaded.FourZeroOne.Roggi;

/// <summary>
/// Used as a marker interface. <br></br>
/// Rodas that inherit will ignore LoadProgramState instructions (i.e. DynamicRodas).
/// </summary>
/// <typeparam name="R"></typeparam>
public interface ILoadOverridingRoda<out R> : IRoda<R>
    where R : class, Rog
{

}
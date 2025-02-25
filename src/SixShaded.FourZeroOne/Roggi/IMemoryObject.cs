namespace SixShaded.FourZeroOne.Roggi;

public interface IMemoryObject<out R> : IMemoryAddress<R>, Rog where R : class, Rog
{ }
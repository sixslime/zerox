namespace SixShaded.FourZeroOne.Roggi;

public interface IMemoryObject<out R> : IMemoryAddress<R>, Rog where R : class, Rog
{ }
// LEFTOFF: perhaps replace the concept of MemoryObject with MemoryRovetu so custom memory objects are fully data driven.
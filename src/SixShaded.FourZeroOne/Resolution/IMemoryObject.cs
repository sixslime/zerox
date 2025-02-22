namespace SixShaded.FourZeroOne.Resolution;

public interface IMemoryObject<out R> : IMemoryAddress<R>, Res where R : class, Res { }
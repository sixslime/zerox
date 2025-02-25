namespace SixShaded.DeTes.Analysis;

public class UnexpectedRoggiTypeException(Type got, Type expected) : Exception($"Got type ${got.Name}, expected ${expected.Name}") { }
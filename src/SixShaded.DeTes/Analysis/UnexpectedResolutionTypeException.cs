namespace SixShaded.DeTes.Analysis;

public class UnexpectedResolutionTypeException(Type got, Type expected) : Exception($"Got type ${got.Name}, expected ${expected.Name}") { }
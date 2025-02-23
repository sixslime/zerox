namespace SixShaded.DeTes.Realization;

public class DeTesInternalException(Exception inner) : Exception("Internal DeTes error.", inner);
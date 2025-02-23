namespace SixShaded.DeTes.Realization;

public class DeTesInvalidTestException : Exception
{
    public required EDeTesInvalidTest Value { get; init; }
}
namespace SixShaded.FourZeroOne.Mellsano;

public static class MellsanoIDGenerator
{
    private static int _currentID;
    public static MellsanoID Next() => new(_currentID++);
}
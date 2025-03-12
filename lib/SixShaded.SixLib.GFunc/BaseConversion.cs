namespace SixShaded.SixLib.GFunc;

public static class BaseConversionExt
{
    public const string HEX = "0123456789abcdef";
    public const string DECIMAL = "0123456789";
    public const string BINARY = "01";

    public static string ToBase(this int number, string baseRepresentation, string negativeSign = "-")
    {
        string neg = number < 0 ? negativeSign : "";
        int baseCount = baseRepresentation.Length;
        List<char> sb = new();
        do
        {
            sb.Add(baseRepresentation[Math.Abs(number % baseCount)]);
            number /= baseCount;
        } while (number != 0);
        sb.Reverse();
        return $"{neg}{new string(sb.ToArray())}";
    }

    public static string AddNegativeSign(string val) => $"-{val}";

    public static int FromBase(this string basedNumber, string baseRepresentation)
    {
        int o = 0;
        int baseCount = baseRepresentation.Length;
        for (int i = 0; i < basedNumber.Length; i++)
        {
            int worth = baseCount * (basedNumber.Length - i - 1);
            if (worth == 0) worth = 1;
            char c = basedNumber[i];
            int index = baseRepresentation.IndexOf(c);
            if (index == -1) throw new($"{basedNumber} contains characters not present in {baseRepresentation}.");
            o += index * worth;
        }
        return o;
    }

    public static string ConvertBase(this string basedNumber, string fromBase, string toBase) => basedNumber.FromBase(fromBase).ToBase(toBase);
}
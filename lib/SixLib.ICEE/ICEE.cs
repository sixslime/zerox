
namespace SixLib.ICEE
{
    public static class ICEEs
    {
        public static string ICEE<T>(this IEnumerable<T> array)
        {
            return $"[{string.Join(",", array)}]";
        }
    }
}

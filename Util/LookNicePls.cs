using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfection;
namespace LookNicePls
{
    public static class LookNicePlsExtensions
    {
        public static string LookNicePls<T>(this IEnumerable<T> array)
        {
            return $"[{string.Join(",", array)}]";
        }
    }
}

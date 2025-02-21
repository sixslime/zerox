using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace SixLib.ControlledFlows
{
    public static class Extensions
    {
        public static TransformedFlow<I, R> WithTransformedResult<I, R>(this ICeasableFlow<I> inputFlow, Func<I, R> transform)
        {
            return new(inputFlow, transform);
        }
    }
}

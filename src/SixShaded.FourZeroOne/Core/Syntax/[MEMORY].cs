using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Syntax
{
    using Resolutions;
    public static partial class TokenSyntax
    {
        public static Tokens.DynamicAssign<R> tAsVariable<R>(this IToken<R> token, out DynamicAddress<R> ident) where R : class, Res
        {
            ident = new();
            return new(ident, token);
        }
        public static Tokens.DynamicReference<R> tRef<R>(this IMemoryAddress<R> ident) where R : class, Res
        { return new(ident); }
        public static Tokens.Data.Insert<R> tDataWrite<R>(this IToken<IMemoryObject<R>> address, IToken<R> data)
            where R : class, Res
        { return new(address, data); }
        public static Tokens.Data.Get<R> tDataGet<R>(this IToken<IMemoryObject<R>> address)
            where R : class, Res
        { return new(address); }
        public static Tokens.Data.Remove tDataRedact(this IToken<IMemoryObject<Res>> address)
        { return new(address); }
        public static Macro<IMemoryObject<R>, MetaFunction<R, R>, Resolutions.Instructions.Assign<R>> tDataUpdate<R>(this IToken<IMemoryObject<R>> address, IToken<MetaFunction<R, R>> updateFunction)
            where R : class, Res
        { return Macros.UpdateMemoryObject<R>.Construct(address, updateFunction); }
        public static Macro<IMemoryObject<R>, MetaFunction<R, R>, Resolutions.Instructions.Assign<R>> tDataUpdate<R>(this IToken<IMemoryObject<R>> address, Func<DynamicAddress<R>, IToken<R>> updateFunction)
            where R : class, Res
        { return Macros.UpdateMemoryObject<R>.Construct(address, Core.tMetaFunction(updateFunction)); }
    }
}

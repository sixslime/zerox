using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Syntax
{
    using Resolutions;
    public static partial class Core
    {
        public static Tokens.SubEnvironment<R> tSubEnvironment<R>(Structure.Token.SubEnvironment<R> block) where R : class, Res
        { return new(block.Environment, block.Value); }
        public static Tokens.Multi.Union<Res> t_Env(params IToken<Res>[] environment)
        { return new(environment.Map(x => x.tYield())); }
    }
    public static partial class TokenSyntax
    {
        public static Tokens.IfElse<R> tIfTrueDirect<R>(this IToken<Bool> condition, Structure.Token.IfElse<MetaFunction<R>> block) where R : class, Res
        { return new(condition, block.Then, block.Else); }
        public static Tokens.Execute<R> t_IfTrue<R>(this IToken<Bool> condition, Structure.Token.IfElse<R> block) where R : class, Res
        {
            return condition.tIfTrueDirect<R>(new()
            {
                Then = block.Then.tMetaBoxed(),
                Else = block.Else.tMetaBoxed()
            }).tExecute();
        }
    }
}

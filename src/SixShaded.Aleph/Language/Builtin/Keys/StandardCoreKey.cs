namespace SixShaded.Aleph.Language.Builtin.Keys;
using FZOTypeMatch.Syntax;
using Contexts;
using FourZeroOne.Roveggi.Unsafe;
using FZOTypeMatch;
using CoreTypeMatcher;
using Ct = CoreTypeMatcher.Types;
using Logical;

public class StandardCoreKey : ILanguageKey
{
    internal static readonly FZOTypeMatch TYPEMATCH = new([new CoreTypeMatcher()]);
    public IOption<Translation> TranslateKorssa(KorssaTranslationContext context, TranslationBuilder builder)
    {
        IOption<Translation> __Translation(params object[] segs)
        {
            int argsLeft = context.Korssa.ArgKorssas.Length;
            foreach (var seg in segs)
            {
                switch (seg)
                {
                case string s:
                    builder.Text(s);
                    break;
                case int s:
                    while (s > 0)
                    {
                        builder.Marker(context.NextArg());
                        argsLeft--;
                        s--;
                        if (s > 0) builder.Text(", ");
                    }
                    break;
                case true:
                    while (argsLeft > 0)
                    {
                        builder.Marker(context.NextArg());
                        argsLeft--;
                        if (argsLeft > 0) builder.Text(", ");
                    }
                    break;
                case Rog s:
                    builder.InlineTranslation(s.AsSome());
                    break;
                case RogOpt s:
                    builder.InlineTranslation(s);
                    break;
                case IRovu s:
                    builder.InlineTranslation(s);
                    break;
                case IVarovu s:
                    builder.InlineTranslation(s);
                    break;
                case IAbstractRovu s:
                    builder.InlineTranslation(s);
                    break;
                case Addr s:
                    builder.InlineTranslation(s);
                    break;
                case Kor s:
                    builder.InlineTranslation(s);
                    break;
                case KorssaTypeInfo s:
                    builder.InlineTranslation(s);
                    break;
                case RovetuTypeInfo s:
                    builder.InlineTranslation(s);
                    break;
                case RoggiTypeInfo s:
                    builder.InlineTranslation(s);
                    break;
                default: throw new("invalid translation recipe");
                }
            }
            return builder.BuildAsSome();
        }

        if (!TYPEMATCH.GetKorssaTypeInfo(context.Korssa).Match.Check(out var typeMatch)) return new None<Translation>();
        return typeMatch switch
        {
            Ct.Korssa.Fixed v => __Translation("|", v.RoggiGetter(context.Korssa), "|"),
            Ct.Korssa.Nolla => __Translation(Master.Instance.LanguageProvider.TranslateNolla()),
            Ct.Korssa.SubEnvironment => __Translation("with environment ", 1, ", return ", 1),
            Ct.Korssa.Multi.Create => __Translation("[", true, "]"),
            Ct.Korssa.Number.Add => __Translation(1, " plus ", 1),
            Ct.Korssa.Number.Subtract => __Translation(1, " minus ", 1),
            Ct.Korssa.Number.Multiply => __Translation(1, " times ", 1),
            Ct.Korssa.Number.Divide => __Translation(1, " divided by ", 1),
            Ct.Korssa.Number.Modulo => __Translation(1, " modulo ", 1),
            Ct.Korssa.Number.GreaterThan => __Translation(1, " is greater than ", 1, "?"),
            Ct.Korssa.IfElse => __Translation("if ", 1, " then ", 1, " else ", 1),
            Ct.Korvessa.Number.Max => __Translation("max of ", 1, " and ", 1),
            Ct.Korssa.MetaExecuted => __Translation("(meta executed)"),
            Ct.Korssa.Memory.Reference v => __Translation("*", v.RodaInfoGetter(context.Korssa).Roda),
            Ct.Korssa.Memory.Assign v => __Translation("set ", v.RodaInfoGetter(context.Korssa).Roda, " to ", 1),
            Ct.Korssa.DefineMetaFunction v => __Translation(["#", v.SelfRodaGetter(context.Korssa),"<", ..v.MetaArgTypes.SeparateBy<object>(", "), ": ", v.MetaOutputType, ">"]),
            _ => new None<Translation>(),
        };
    }
    public IOption<Translation> TranslateRoggi(RoggiTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateRoda(RodaTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateRovu(RovuTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateVarovu(VarovuTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateAbstractRovu(AbstractRovuTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<string> TranslateKorssaTypeInfo(KorssaTypeInfo typeInfo) => new None<string>();
    public IOption<string> TranslateRoggiTypeInfo(RoggiTypeInfo typeInfo) => new None<string>();
    public IOption<string> TranslateRovetuTypeInfo(RovetuTypeInfo typeInfo) => new None<string>();
    public IOption<string> TranslateNolla() => new None<string>();
}
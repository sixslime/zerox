namespace SixShaded.Aleph.Language.Builtin.Keys;

using FourZeroOne.Roveggi.Unsafe;
using FZOTypeMatch;
using TranslationBuilders;

public class CoreKey : ILanguageKey
{
    public IOption<KorssaTranslation> TranslateKorssa(IKorssaType type, KorssaTranslationBuilder builder) => throw new NotImplementedException();

    public string TranslateRoggi(IRoggiType type) => throw new NotImplementedException();

    public string TranslateRovetu(IRovetuType type) => throw new NotImplementedException();

    public string TranslateRovu(IRovu rovu) => throw new NotImplementedException();

    public string TranslateRoda(Addr roda) => throw new NotImplementedException();
}
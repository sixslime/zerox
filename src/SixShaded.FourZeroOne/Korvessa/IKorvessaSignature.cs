namespace SixShaded.FourZeroOne.Korvessa;

using Unsafe;

public interface IKorvessaSignature<RVal> : IKorvessa<RVal>, IHasNoArgs<RVal>
    where RVal : class, Rog;

public interface IKorvessaSignature<RArg1, ROut> : IKorvessa<ROut>, IHasArgs<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog;

public interface IKorvessaSignature<RArg1, RArg2, ROut> : IKorvessa<ROut>, IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog;

public interface IKorvessaSignature<RArg1, RArg2, RArg3, ROut> : IKorvessa<ROut>, IHasArgs<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog;
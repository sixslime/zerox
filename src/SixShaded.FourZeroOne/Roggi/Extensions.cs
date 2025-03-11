namespace SixShaded.FourZeroOne.Roggi;

using Core.Korssas;

public static class Extensions
{
    public static FZOSpec.EStateImplemented.MetaExecute ConstructMetaExecute<R>(this Unsafe.IMetaFunction<R> metaFunction, params RogOpt[] argRoggis)
    where R : class, Rog
    {
        return new()
        {
            Korssa = new MetaExecuted<R>(metaFunction.Korssa),
            ObjectWrites =
                metaFunction.CapturedVariables
                    .Map(x => (x, metaFunction.CapturedMemory.GetObject(x)))
                    .Concat(metaFunction.ArgAddresses.ZipShort(argRoggis))
                    .Concat([(metaFunction.SelfAddress.IsA<Addr>(), metaFunction.AsSome().IsA<RogOpt>())])
                    .Tipled()
        };
    }
}

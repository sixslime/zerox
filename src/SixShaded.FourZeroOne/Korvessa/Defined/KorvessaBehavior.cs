namespace SixShaded.FourZeroOne.Korvessa.Defined;

using Core.Roggis;
using Core.Korssas;
using Core.Syntax;
using FZOSpec;
using Roggi.Unsafe;

public abstract record KorvessaBehavior<R>(params Kor[] args) : Korssa.Defined.Korssa<R>(args), Unsafe.IKorvessa<R>
    where R : class, Rog
{
    protected override IResult<ITask<IOption<R>>, EStateImplemented> Resolve(IKorssaContext runtime, RogOpt[] args) => DefinitionUnsafe.ConstructMetaExecute(args).AsErr(Hint<ITask<IOption<R>>>.HINT);
    public abstract IMetaFunction<R> DefinitionUnsafe { get; }
    public virtual bool Equals(KorvessaBehavior<R>? other) => other is not null && DefinitionUnsafe.Korssa.Equals(other.DefinitionUnsafe.Korssa);
    public override int GetHashCode() => DefinitionUnsafe.Korssa.GetHashCode();
}
namespace SixShaded.FourZeroOne.Core.Korssas.Memory.ProgramState;

using Roggis;
using Roggis.Instructions;

public sealed record Load(IKorssa<ProgramState> state) : Korssa.Defined.PureFunction<ProgramState, LoadProgramState>(state)
{
    protected override LoadProgramState EvaluatePure(ProgramState in1) =>
        new()
        {
            State = in1
        };
    protected override IOption<string> CustomToString() => $"LOAD({Arg1})".AsSome();
}
namespace SixShaded.FourZeroOne.Core.Korssas.Memory.ProgramState;

using Roggis;

public sealed record Get() : Korssa.Defined.Value<ProgramState>
{
    protected override ITask<IOption<ProgramState>> Evaluate(IKorssaContext runtime) =>
        new ProgramState()
            {
                Memory = runtime.CurrentMemory
            }.AsSome()
            .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"CURRENT_STATE".AsSome();
}
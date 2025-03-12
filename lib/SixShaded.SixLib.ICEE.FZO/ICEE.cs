namespace SixShaded.SixLib.ICEE.FZO;

using System.Text;

public static class ICEEs
{
    public static string ICEE(this IMemoryFZO memory) =>
        new StringBuilder()
            .AppendLine($"[FZO Memory] ({memory.GetType().Name})")
            .AppendLine("-----")
            .AppendLine("Objects:")
            .AppendJoin("\n", memory.Objects.Map(x => "- " + x))
            .AppendLine()
            .AppendLine()
            .AppendLine("Rules:")
            .AppendJoin("\n", memory.Mellsanos.Map(x => "- " + x))
            .AppendLine()
            .AppendLine("RuleMutes:")
            .AppendJoin("\n", memory.MellsanoMutes.Map(x => "- " + x))
            .AppendLine()
            .AppendLine("-----")
            .ToString();

    public static string ICEE(this IStateFZO state) =>
        new StringBuilder()
            .AppendLine($"[FZO State] ({state.GetType().Name})")
            .AppendLine("-----")
            .AppendLine("Initialized:")
            .AppendLine(state.Initialized.ToString())
            .AppendLine("Operation Stack:")
            .AppendJoin(
            "\n", state.OperationStack.Map(
            x =>
                $"- {x.Operation} <- {string.Join(" | ", x.ArgRoggiStack)}"))
            .AppendLine()
            .AppendLine("Last Memory:")
            .AppendLine(
            state.OperationStack.GetAt(0)
                .RemapAs(x => x.MemoryStack.GetAt(0))
                .Press()
                .RemapAs(x => x.ToString())
                .Or("N/A"))
            .AppendJoin("\n", state.KorssaMutationStack.Map(x => "- " + x))
            .AppendLine()
            .AppendLine("-----")
            .ToString();
}
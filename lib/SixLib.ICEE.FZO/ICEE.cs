
using System.Text;
using Perfection;
namespace SixLib.ICEE.FZO
{
    public static class ICEEs
    {
        public static string ICEE(this FourZeroOne.FZOSpec.IMemoryFZO memory)
        {
            return new StringBuilder()
                .AppendLine($"[FZO Memory] ({memory.GetType().Name})")
                .AppendLine("-----")
                .AppendLine("Objects:")
                .AppendJoin("\n", memory.Objects.Map(x => "- " + x))
                .AppendLine()
                .AppendLine()
                .AppendLine("Rules:")
                .AppendJoin("\n", memory.Rules.Map(x => "- " + x))
                .AppendLine()
                .AppendLine("RuleMutes:")
                .AppendJoin("\n", memory.RuleMutes.Map(x => "- " + x))
                .AppendLine()
                .AppendLine("-----")
                .ToString();
        }
        public static string ICEE(this FourZeroOne.FZOSpec.IStateFZO state)
        {
            return new StringBuilder()
                .AppendLine($"[FZO State] ({state.GetType().Name})")
                .AppendLine("-----")
                .AppendLine("Initialized:")
                .AppendLine(state.Initialized.ToString())
                .AppendLine("Operation Stack:")
                .AppendJoin("\n", state.OperationStack.Map(
                    x => 
                    $"- {x.Operation} <- {string.Join(" | ", x.ArgResolutionStack)}"))
                .AppendLine()
                .AppendLine("Last Memory:")
                .AppendLine(state.OperationStack.GetAt(0).RemapAs(x => x.MemoryStack.GetAt(0)).Press().RemapAs(x => x.ToString()).Or("N/A"))
                .AppendJoin("\n", state.TokenMutationStack.Map(x => "- " + x))
                .AppendLine()
                .AppendLine("-----")
                .ToString();
        }
    }
}

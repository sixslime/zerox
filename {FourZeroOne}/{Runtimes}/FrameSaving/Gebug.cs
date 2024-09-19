
using ControlledFlows;
using FourZeroOne.Resolution;
using FourZeroOne.Rule;
using FourZeroOne.Token.Unsafe;
using Perfection;
namespace FourZeroOne.Runtimes.FrameSaving
{
    public class Gebug : Runtime.FrameSaving
    {
        private int depth = 0;
        private string depthPad => "--".Yield(depth).AccumulateInto("", (msg, x) => msg + x);
        private HashSet<IToken> _seenTokens = [];
        public Gebug(State startingState, IToken program) : base(startingState, program)
        {
        }

        protected override void RecieveFrame(LinkedStack<Frame> frameNode)
        {
            //Debug.Log($"{depthPad}FRAME");
        }

        protected override void RecieveMacroExpansion(IToken macro, IToken expanded)
        {
            Console.WriteLine($"{depthPad}& {macro} -> {expanded}");
        }

        protected override void RecieveResolution(IOption<IResolution> resolution)
        {
            Console.WriteLine($"{depthPad}* {resolution}");
            depth--;
        }

        protected override void RecieveRuleSteps(IEnumerable<(IToken token, IRule appliedRule)> steps)
        {
            if (!steps.Any()) return;
            Console.WriteLine($"{depthPad}+ {steps.AccumulateInto("", (msg, x) => msg + x.token + $"\n")}");
        }

        protected override void RecieveToken(IToken token)
        {
            if (_seenTokens.Add(token)) {
                Console.WriteLine($"{depthPad}: {token}");
                depth++;
                return;
            }
            Console.WriteLine($"{depthPad}^ {token}");
        }

        protected override ICeasableFlow<IOption<IEnumerable<R>>> SelectionImplementation<R>(IEnumerable<R> from, int count)
        {
            throw new NotImplementedException();
        }

    }
}
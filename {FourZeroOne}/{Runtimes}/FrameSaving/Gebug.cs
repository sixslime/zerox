
using ControlledFlows;
using FourZeroOne.Resolution;
using FourZeroOne.Rule;
using FourZeroOne.Token.Unsafe;
using Perfection;
using MorseCode.ITask;
namespace FourZeroOne.Runtimes.FrameSaving
{
    public class Gebug : Runtime.FrameSaving
    {
        private int depth = 0;
        private IOption<LinkedStack<Frame>> _currentFrame;
        private string depthPad => "--".Yield(depth).AccumulateInto("", (msg, x) => msg + x);
        private HashSet<IToken> _seenTokens = [];
        public Gebug(State startingState, IToken program) : base(startingState, program)
        {
            _currentFrame = new None<LinkedStack<Frame>>();
        }

        protected override void RecieveFrame(LinkedStack<Frame> frameNode)
        {
            _currentFrame = frameNode.AsSome();
            Console.WriteLine($"=== {frameNode.Value.Token} ===");
        }

        protected override void RecieveMacroExpansion(IToken macro, IToken expanded)
        {
            Console.WriteLine($"{depthPad}& {macro} -> {expanded}");
        }

        protected override void RecieveResolution(IOption<IResolution> resolution)
        {
            Console.WriteLine($"{depthPad}: {resolution}");
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
                Console.WriteLine($"{depthPad}>> {token}");
                depth++;
                return;
            }
            Console.WriteLine($"{depthPad}> {token}");
        }

        protected override ITask<IOption<IEnumerable<R>>> SelectionImplementation<R>(IEnumerable<R> from, int count, out IOption<LinkedStack<Frame>> targetFrame)
        {
            targetFrame = new None<LinkedStack<Frame>>();
            R[] selectables = [.. from];
            if (selectables.Length < count) return Task.FromResult(new None<IEnumerable<R>>()).AsITask();
            string showString =
                selectables
                .Enumerate()
                .AccumulateInto($">> SELECT {count}", (msg, entry) => $"{msg}\n> [{entry.index}] - {entry.value}");
            Console.WriteLine(showString);

            while (true)
            {
                var inputString = Console.ReadLine();
                if (inputString is null) continue;
                if (inputString[0] == '<')
                {
                    if (!int.TryParse(inputString[1..], out var framesBack))
                        continue;
                    targetFrame = _currentFrame.Sequence(x => x.Unwrap().Link).ElementAt(framesBack);
                    return Task.FromResult(new None<IEnumerable<R>>()).AsITask();
                }
                int[] selectionIndicies = [.. inputString.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Map(x => int.TryParse(x, out var value) ? value : -1)
                    .Where(x => x >= 0)];
                if (selectionIndicies.Length != count)
                {
                    Console.WriteLine($"{count} inputs required, {selectionIndicies.Length} given.");
                    Console.WriteLine(showString);
                    continue;
                }
                if (selectionIndicies.HasMatch(x => x > selectables.Length - 1))
                {
                    Console.WriteLine($"One or more inputs is invalid.");
                    Console.WriteLine(showString);
                    continue;
                }
                return Task.FromResult(selectionIndicies.Map(x => selectables[x]).AsSome()).AsITask();
            }
            
        }

    }
}

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
        private IOption<LinkedStack<Frame>> _currentFrame;
        private int _depth = 0;
        private bool _macroExpanding = false;
        private string DepthPad(int depth) => "| ".Yield(depth).AccumulateInto("", (msg, x) => msg + x);
        public Gebug(IState startingState, IToken program) : base(startingState, program)
        {
            _currentFrame = new None<LinkedStack<Frame>>();
        }

        protected override void RecieveFrame(LinkedStack<Frame> frameNode)
        {
            _currentFrame = frameNode.AsSome();
            //Console.WriteLine($"=== {frameNode.Value.Token} ===");
        }

        protected override void RecieveMacroExpansion(IToken macro, IToken expanded, int depth)
        {
            _macroExpanding = true;
            Console.Write($"{DepthPad(depth)}|-┌");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($" {macro}");
            Console.ResetColor();
        }

        protected override void RecieveResolution(IOption<IResolution> resolution, int depth)
        {
            _depth = depth;
            Console.Write($"{DepthPad(depth)}├<┴");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" = {resolution}");
            Console.ResetColor();
        }

        protected override void RecieveRuleSteps(IEnumerable<(IToken token, IRule appliedRule)> steps)
        {
            if (!steps.Any()) return;
            Console.Write($"{DepthPad(_depth + 1)}? {steps.AccumulateInto("", (msg, x) => msg + x.token + $"\n")}");
        }

        protected override void RecieveToken(IToken token, int depth)
        {
            
            if (depth >= _depth)
            {
                if (_macroExpanding)
                {
                    Console.Write($"{DepthPad(depth + 1)}|");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($" ╚>");
                    _macroExpanding = false;
                }
                else
                {
                    Console.Write($"{DepthPad(depth)}|-┌ ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                Console.WriteLine($"{token}");
                Console.ResetColor();

            } else
            {
                Console.Write($"{DepthPad(_depth)}|");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" {token}");
                Console.ResetColor();
            }
            _depth = depth + 1;
        }

        protected override ITask<IOption<IEnumerable<R>>> SelectionImplementation<R>(IEnumerable<R> from, int count, out IOption<LinkedStack<Frame>> targetFrame)
        {
            Console.ForegroundColor = ConsoleColor.White;
            var o = SelectionPrompt(from, count, out targetFrame);
            Console.ResetColor();
            return o;
        }
        private ITask<IOption<IEnumerable<R>>> SelectionPrompt<R>(IEnumerable<R> from, int count, out IOption<LinkedStack<Frame>> targetFrame)
        {
            targetFrame = new None<LinkedStack<Frame>>();
            R[] selectables = [.. from];
            if (selectables.Length < count) return new None<IEnumerable<R>>().ToCompletedITask();
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
                    _currentFrame = targetFrame;
                    return new None<IEnumerable<R>>().ToCompletedITask();
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
                return selectionIndicies.Map(x => selectables[x]).AsSome().ToCompletedITask();
            }

        }
    }
}
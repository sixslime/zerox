
using ControlledFlows;
using FourZeroOne.Resolution;
using FourZeroOne.Rule;
using FourZeroOne.Token.Unsafe;
using Perfection;
using MorseCode.ITask;
namespace FourZeroOne.Runtimes.FrameSaving
{
    using LookNicePls;
    public class Gebug : Runtime.FrameSaving
    {
        private IOption<int[]>[] _autoSelections = [];
        private IOption<int>[] _autoRewinds = [];
        private IOption<LinkedStack<Frame>> _currentFrame = new None<LinkedStack<Frame>>();
        private int _depth = 0;
        private bool _macroExpanding = false;
        private int _promptIndex = -1;

        public void SetAutoSelections(params int[]?[] selections)
        {
            _autoSelections = selections.Map(x => x.NullToNone()).ToArray();
        }
        public void SetAutoRewinds(params int?[] rewinds)
        {
            _autoRewinds = rewinds.Map(x => x.NullToNone().RemapAs(x => (int)x! /*???*/)).ToArray(); ;
        } 
        protected override void OnRunCall(IState startingState, IToken program)
        {
            _currentFrame = new None<LinkedStack<Frame>>();
            _promptIndex = -1;
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
            Console.Write($"{DepthPad(_depth + 1)}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"? {steps.AccumulateInto("", (msg, x) => msg + x.token + $"\n")}");
            Console.ResetColor();
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

        
        private static string DepthPad(int depth) => "| ".Yield(depth).AccumulateInto("", (msg, x) => msg + x);

        private ITask<IOption<IEnumerable<R>>> SelectionPrompt<R>(IEnumerable<R> from, int count, out IOption<LinkedStack<Frame>> targetFrame)
        {
            _promptIndex++;

            targetFrame = new None<LinkedStack<Frame>>();
            R[] selectables = [.. from];
            if (selectables.Length < count) return new None<IEnumerable<R>>().ToCompletedITask();
            string showString =
                selectables
                .Enumerate()
                .AccumulateInto($">> SELECT {count}", (msg, entry) => $"{msg}\n> [{entry.index}] - {entry.value}");
            Console.WriteLine(showString);

            // AUTO SELECT/REWIND
            if (_autoRewinds.Length > _promptIndex && _autoRewinds[_promptIndex].Check(out var autoRewind))
                return __DoRewind(autoRewind, out targetFrame);
            if (_autoSelections.Length > _promptIndex && _autoSelections[_promptIndex].Check(out var selections))
            {
                if (!__ValidateSelections(selections)) throw new Exception($"[Gebug Runtime] Invalid auto selection: {_promptIndex}: {selections.LookNicePls()}");
                Console.WriteLine($"AUTO SELECT: {selections.LookNicePls()}");
                return __DoSelection(selections);
            }
                

            // MANUAL USER INPUT
            while (true)
            {
                var inputString = Console.ReadLine();
                if (inputString is null) continue;
                if (inputString[0] == '<')
                {
                    if (!int.TryParse(inputString[1..], out var framesBack))
                        continue;
                    return __DoRewind(framesBack, out targetFrame);
                }
                int[] selectionIndicies = [.. inputString.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Map(x => int.TryParse(x, out var value) ? value : -1)
                    .Where(x => x >= 0)];
                if (!__ValidateSelections(selectionIndicies)) continue;
                return __DoSelection(selectionIndicies);
            }
            bool __ValidateSelections(int[] selections)
            {
                if (selections.Length != count)
                {
                    Console.WriteLine($"{count} inputs required, {selections.Length} given.");
                    Console.WriteLine(showString);
                    return false;
                }
                if (selections.HasMatch(x => x > selectables.Length - 1))
                {
                    Console.WriteLine($"One or more inputs is invalid.");
                    Console.WriteLine(showString);
                    return false;
                }
                return true;
            }
            ITask<IOption<IEnumerable<R>>> __DoRewind(int framesBack, out IOption<LinkedStack<Frame>> targetFrame)
            {
                targetFrame = _currentFrame.Sequence(x => x.Unwrap().Link).ElementAt(framesBack);
                _currentFrame = targetFrame;
                return new None<IEnumerable<R>>().ToCompletedITask();
            }
            ITask<IOption<IEnumerable<R>>> __DoSelection(int[] selections)
            {
                return selections.Map(x => selectables[x]).AsSome().ToCompletedITask();
            }
        }
    }
}
namespace SixShaded.Aleph.Logical;

public class Session
{
    private static readonly None<ProgressionContext> INACTIVE_CONTEXT = new();
    private IOption<ProgressionContext> _activeProgressionContext = INACTIVE_CONTEXT;
    public required IStateFZO Root { get; init; }
    public required IInputFZO InputHandler { get; init; }
    public required IProcessorFZO Processor { get; init; }
    public bool InProgress => _activeProgressionContext.IsSome();
    public PSequence<Trackpoint> Trackpoints { get; private set; } = new();
    public int CurrentTrackpointIndex { get; private set; } = -1;
    public IOption<Trackpoint> CurrentTrackpoint => Trackpoints.At(CurrentTrackpointIndex);
    public event EventHandler<TrackpointUpdatedEventArgs>? TrackpointUpdatedEvent;

    public bool GotoTrackpoint(int index)
    {
        if (!Trackpoints.At(index).IsSome()) return false;
        SetProgressionContext(null);
        CurrentTrackpointIndex = index;
        NotifyUpdateTrackpoint();
        return true;
    }

    public async Task<bool> Progress(IProgressor progressor, bool backward = false)
    {
        if (InProgress) return false;
        var thisContext = new ProgressionContext(this, backward);
        SetProgressionContext(thisContext);
        await progressor.Consume(thisContext);
        if (!thisContext.Active) return false;
        var trackpoint =
            new Trackpoint
            {
                ForwardSteps = thisContext.Steps.ToPSequence(),
                Progressor = progressor,
                Selections = thisContext.Selections.ToPSequence(),
            };

        // DEV/OPTIMIZE: this could be optimized
        // PSequence should be indexable by range
        Trackpoints =
            CurrentTrackpointIndex == Trackpoints.Count - 1
                ? Trackpoints.WithEntries(trackpoint)
                : Trackpoints.Elements.Take(CurrentTrackpointIndex + 1).Also(trackpoint.Yield()).ToPSequence();
        CurrentTrackpointIndex++;
        NotifyUpdateTrackpoint();
        return true;
    }

    private void NotifyUpdateTrackpoint() =>
        TrackpointUpdatedEvent?.Invoke(
        this, new()
        {
            NewTrackpoint = CurrentTrackpoint.Unwrap(),
        });

    private void SetProgressionContext(ProgressionContext? context) => _activeProgressionContext = context is null ? INACTIVE_CONTEXT : context.AsSome();

    private class ProgressionContext : IProgressionContext
    {
        private readonly IEnumerator<Trackpoint.Step>? _backwardIterator;
        private readonly InputWrapper _input;
        private readonly Session _session;

        public ProgressionContext(Session session, bool backward)
        {
            _session = session;
            _input = new(this, session.InputHandler);
            IsBackward = backward;
            _backwardIterator = backward ? BackwardIteration().GetEnumerator() : null;
        }

        public bool IsBackward { get; }
        public bool Active => _session._activeProgressionContext.Check(out var active) && active == this;
        public List<Trackpoint.Step> Steps { get; } = new();
        public List<int[]> Selections { get; } = new();
        private IEnumerable<Trackpoint.Step> BackwardIteration() => _session.Trackpoints.Elements.Reverse().Map(x => x.ForwardSteps.Elements.Reverse()).Flatten();

        private async Task<IOption<Trackpoint.Step>> EvaluateNextStep()
        {
            if (!Active) return new None<Trackpoint.Step>();
            if (IsBackward)
            {
                return _backwardIterator!.MoveNext()
                    ? _backwardIterator.Current.AsSome()
                    : new None<Trackpoint.Step>();
            }
            var currentState =
                Steps.Count == 0
                    ? _session.CurrentTrackpoint.RemapAs(x => x.ForwardSteps.At(x.ForwardSteps.Count - 1).Unwrap().State).Or(_session.Root)
                    : Steps[^1].ExprAs(prev => prev.NextStep.RemapOk(step => prev.State.WithStep(step)).KeepOk().Or(null));
            if (currentState == null) return new None<Trackpoint.Step>();
            return new Trackpoint.Step
            {
                State = currentState,
                NextStep = await _session.Processor.GetNextStep(currentState, _input),
            }.AsSome();
        }


        public async Task<IOption<Trackpoint.Step>> Next()
        {
            var nextStep = await EvaluateNextStep();
            if (nextStep.Check(out var entry)) Steps.Add(entry);
            return nextStep;
        }

        private class InputWrapper(ProgressionContext parentContext, IInputFZO implementation) : IInputFZO
        {
            public async Task<int[]> GetSelection(Rog[] pool, int minCount, int maxCount)
            {
                int[] o = await implementation.GetSelection(pool, minCount, maxCount);
                parentContext.Selections.Add(o);
                return o;
            }
        }
    }
}

public class TrackpointUpdatedEventArgs : EventArgs
{
    public required Trackpoint NewTrackpoint { get; init; }
}

public interface IProgressionContext
{
    public bool IsBackward { get; }
    public Task<IOption<Trackpoint.Step>> Next();
}

public interface IProgressor
{
    public string Identifier { get; }
    public Task Consume(IProgressionContext context);
}
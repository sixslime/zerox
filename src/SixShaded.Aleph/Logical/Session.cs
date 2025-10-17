namespace SixShaded.Aleph.Logical;

internal class Session
{
    public Session()
    {

    }
    private IOption<ProgressionContext> _activeProgressionContext = new None<ProgressionContext>();

    public required IStateFZO Root { get; init; }
    public required IProcessorFZO Processor { get; init; }
    public bool InProgress => _activeProgressionContext.IsSome();
    public PSequence<Trackpoint> Trackpoints { get; private set; } = new();
    public int CurrentTrackpointIndex { get; private set; } = -1;
    public IOption<Trackpoint> CurrentTrackpoint => CurrentTrackpointIndex >= 0 ? Trackpoints.At(CurrentTrackpointIndex) : new None<Trackpoint>();
    public event EventHandler<TrackpointUpdatedEventArgs>? TrackpointUpdatedEvent;
    public event EventHandler<SelectionPromptedEventArgs>? SelectionPromptedEvent;
    public event EventHandler<SelectionCancelledEventArgs>? SelectionCancelledEvent;

    public bool GotoTrackpoint(int index)
    {
        if (!Trackpoints.At(index).IsSome()) return false;
        SetProgressionContext(null);
        CurrentTrackpointIndex = index;
        NotifyTrackpointUpdated();
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

        // DEV/OPTIMIZE: PSequence should be indexable by range
        Trackpoints =
            CurrentTrackpointIndex == Trackpoints.Count - 1
                ? Trackpoints.WithEntries(trackpoint)
                : Trackpoints.Elements.Take(CurrentTrackpointIndex + 1).Also(trackpoint.Yield()).ToPSequence();
        CurrentTrackpointIndex++;
        NotifyTrackpointUpdated();
        return true;
    }

    private void NotifySelectionPrompted(SelectionPromptedEventArgs args)
    {
        SelectionPromptedEvent?.Invoke(this, args);
    }

    private void NotifySelectionCancelled(SelectionPromptedEventArgs originalArgs)
    {
        SelectionCancelledEvent?.Invoke(
        this, new()
        {
            OriginalArgs = originalArgs,
        });
    }
    private void NotifyTrackpointUpdated() =>
        TrackpointUpdatedEvent?.Invoke(
        this, new()
        {
            Trackpoint = CurrentTrackpoint.Unwrap(),
        });

    private void SetProgressionContext(ProgressionContext? context)
    {
        if (_activeProgressionContext.Check(out var activeContext))
        {
            activeContext.Dispose();
        }
        _activeProgressionContext = context.NullToNone();
    }

    private class ProgressionContext : IProgressionContext, IDisposable
    {
        private readonly IEnumerator<Trackpoint.Step>? _backwardIterator;
        private readonly Session _session;
        private readonly InputHandler _input;

        public ProgressionContext(Session session, bool backward)
        {
            _session = session;
            IsBackward = backward;
            _backwardIterator = backward ? BackwardIteration().GetEnumerator() : null;
            _input = new(this);
        }

        public bool Active { get; private set; } = true;
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
                    : Steps[^1].ExprAs(prev => prev.NextStep.KeepOk().RemapAs(step => prev.State.WithStep(step)).Or(null));
            if (currentState == null) return new None<Trackpoint.Step>();
            using var inputHandler = new InputHandler(this);

            try
            {
                return new Trackpoint.Step
                {
                    State = currentState,
                    NextStep = await _session.Processor.GetNextStep(currentState, inputHandler),
                }.AsSome();
            }
            catch (TaskCanceledException)
            {
                return new None<Trackpoint.Step>();
            }
        }


        public bool IsBackward { get; }

        public async Task<IOption<Trackpoint.Step>> Next()
        {
            var nextStep = await EvaluateNextStep();
            if (nextStep.Check(out var entry)) Steps.Add(entry);
            return nextStep;
        }

        public void Dispose()
        {
            _input.Dispose();
            Active = false;
        }

        // this sucks
        private class InputHandler : IInputFZO, IDisposable
        {
            private bool _active = true;
            private SelectionPromptedEventArgs? _selectionArgs;
            private TaskCompletionSource<int[]>? _selectionCompletionSource;
            private readonly ProgressionContext _parentContext;

            public InputHandler(ProgressionContext parentContext)
            {
                _parentContext = parentContext;
                _parentContext._session.SelectionCancelledEvent += HandleCancel;
            }
            private void HandleCancel(object? sender, SelectionCancelledEventArgs args)
            {
                if (args.OriginalArgs != _selectionArgs) return;
                _selectionCompletionSource?.TrySetCanceled();
            }
            private void MakeSelection(int[] selections)
            {
                _selectionCompletionSource!.TrySetResult(selections);
                _selectionArgs = null;
            }
            async Task<int[]> IInputFZO.GetSelection(Rog[] pool, int minCount, int maxCount)
            {
                if (!_active) throw new Exception("Selection requested on inactive InputHandler.");
                _selectionArgs =
                    new()
                    {
                        Pool = pool,
                        MinCount = minCount,
                        MaxCount = maxCount,
                        Callback = new SelectionCallback(this),
                    };
                _selectionCompletionSource = new();
                _parentContext._session.NotifySelectionPrompted(_selectionArgs);

                var o = await _selectionCompletionSource.Task.ConfigureAwait(true);
                _parentContext.Selections.Add(o);
                return o;
            }
            private class SelectionCallback(InputHandler parent) : ISelectionCallback
            {
                private readonly InputHandler _parent = parent;
                private bool _active = true;
                public void Select(int[] selections)
                {
                    if (!_active) return;
                    _active = false;
                    _parent.MakeSelection(selections);
                }
            }
            public void Dispose()
            {
                _active = false;
                _parentContext._session.SelectionCancelledEvent -= HandleCancel;
                _selectionCompletionSource?.TrySetCanceled();
                if (_selectionArgs is not null)
                {
                    _parentContext._session.NotifySelectionCancelled(_selectionArgs);
                }
                _selectionArgs = null;
            }
        }
    }
}
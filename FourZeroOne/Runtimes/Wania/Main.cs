
using ControlledFlows;
using FourZeroOne.Resolution;
using FourZeroOne.Rule;
using FourZeroOne.Token.Unsafe;
using Perfection;
using MorseCode.ITask;
using FourZeroOne;
using FourZeroOne.Runtime;

#nullable enable
namespace FourZeroOne.Runtimes
{
    using System;
    using System.Collections.Generic;
    using FourZeroOne.Proxy.Unsafe;
    using FourZeroOne.Resolution.Unsafe;
    using FourZeroOne.Token;
    using LookNicePls;
    using ResObj = Resolution.IResolution;
    public partial class Wania : IRuntime
    {

        public bool IsRunning => throw new NotImplementedException();

        private readonly Wania.TransformationHandler SIGNAL;

        public Wania()
        {
            SIGNAL = new(this);
        }
        public void Backtrack(int resolvedOperationAmount)
        {
            throw new NotImplementedException();
        }

        public bool RunProgram(IState startingState, IToken program)
        {
            if (IsRunning) return false;
            throw new NotImplementedException();
        }

        public bool SendSelectionResponse<R>(SelectionRequest request, params int[] selectedIndicies) where R : class, ResObj
        {
            throw new NotImplementedException();
        }

        private async Task Main()
        {
            
        }
        // should be fine to literally just push to next token?
        private ITask<IOption<R>> TokenMetaExecute<R>(IToken<R> token, IEnumerable<ITiple<IStateAddress, IOption<ResObj>>> args) where R : class, ResObj
        {
            throw new NotImplementedException();
        }
        private ITask<IOption<IHasElements<R>>> TokenReadSelection<R>(IHasElements<R> from, int count)
        {
            throw new NotImplementedException();
            
        }

    }
}
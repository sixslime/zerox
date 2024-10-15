
using Perfection;
using System;


#nullable enable
namespace FourZeroOne
{
    using Token.Unsafe;
    using ResObj = Resolution.IResolution;
    using r = Core.Resolutions;
    using rb = Core.Resolutions.Objects.Board;
    
    public interface IState
    {
        public IEnumerable<(Resolution.Unsafe.IStateAddress address, ResObj obj)> Objects { get; }
        public IEnumerable<Rule.IRule> Rules { get; }
        public Updater<IEnumerable<Rule.IRule>> dRules { init; }
        public IState WithResolution(ResObj resolution);
        public IOption<R> GetObject<R>(Resolution.IStateAddress<R> address) where R : class, ResObj;
        public IState WithObjects<R>(IEnumerable<(Resolution.IStateAddress<R> address, R obj)> insertions) where R : class, ResObj;
    }
}
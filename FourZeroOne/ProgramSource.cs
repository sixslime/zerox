using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace FourZeroOne
{
    public readonly struct ProgramSource<R> where R : class, Resolution.IResolution
    {
        public IState StartingState { get; init; }
        public Token.IToken<R> Expression { get; init; }
    }
}
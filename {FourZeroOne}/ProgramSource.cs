using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace FourZeroOne
{
    public readonly struct ProgramSource
    {
        public State StartingState { get; init; }
        public Token.IToken<Core.Resolutions.Objects.Board.Player> Expression { get; init; }
    }
}
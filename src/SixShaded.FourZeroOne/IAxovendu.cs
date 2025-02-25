namespace SixShaded.FourZeroOne;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAxovendu
{
    Axodu Axodu { get; }
    string Identifier { get; }
    string TypeExpression { get; }
}
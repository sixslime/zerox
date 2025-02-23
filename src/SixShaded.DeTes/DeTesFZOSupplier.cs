namespace SixShaded.DeTes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record DeTesFZOSupplier : IDeTesFZOSupplier
{
    public required IStateFZO UnitializedState { get; init; }
    public required IProcessorFZO Processor { get; init; }
}

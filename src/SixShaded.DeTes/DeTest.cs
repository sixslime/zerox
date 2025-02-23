namespace SixShaded.DeTes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record DeTest : IDeTesTest
{
    public required IMemoryFZO InitialMemory { get; init; }
    public required TokenDeclaration Tok { get; init; }
}


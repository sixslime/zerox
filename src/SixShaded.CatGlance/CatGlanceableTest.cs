namespace SixShaded.CatGlance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CatGlanceableTest : ICatGlanceable
{
    public CatGlanceableTest() { Name = "(unnamed)"; }
    public CatGlanceableTest(string name) { Name = name; }
    public string Name { get; }
    public required IMemoryFZO InitialMemory { get; init; }
    public required DeTes.Declaration.DeTesDeclaration Declaration { get; init; }
}
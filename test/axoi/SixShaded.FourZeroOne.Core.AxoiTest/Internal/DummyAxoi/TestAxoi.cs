namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TestAxoi : IsAxoi
{
    private TestAxoi()
    { }

    public static Axodu Du =>
        new()
        {
            Name = "coretest",
        };
}
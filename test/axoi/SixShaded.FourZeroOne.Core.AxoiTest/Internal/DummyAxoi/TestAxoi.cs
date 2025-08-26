namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axoi = FourZeroOne.Axoi;

internal class TestAxoi : Axoi
{
    private TestAxoi()
    { }

    public static Axodu Du =>
        new()
        {
            Name = "coretest",
        };
}
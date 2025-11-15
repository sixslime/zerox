namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TestAxoi(Master.AxoiCreationKey key) : IsAxoi(key)
{
    public override string Name => "test";
}
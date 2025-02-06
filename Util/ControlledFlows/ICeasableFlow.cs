using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MC = MorseCode.ITask;

#nullable enable
namespace ControlledFlows
{
    public interface ICeasableFlow : MC.ITask
    {
        public void Cease();
    }
    public interface ICeasableFlow<out T> : MC.ITask<T>
    {
        public ICeasableAwaiter<T> GetCeasableAwaiter();
        public void Cease();
    }
}

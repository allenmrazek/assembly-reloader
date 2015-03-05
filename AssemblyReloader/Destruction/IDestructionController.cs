using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Destruction
{
    public interface IDestructionController
    {
        void Destroy<T>(T target);
        void Register<T>(Action<T> f);
    }
}

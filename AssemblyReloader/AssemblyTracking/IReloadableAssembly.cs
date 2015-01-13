using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Tracking
{
    interface IReloadableAssembly
    {
        void Load();
        void Unload();
    }
}

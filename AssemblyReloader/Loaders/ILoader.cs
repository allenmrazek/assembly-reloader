using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AddonTracking;

namespace AssemblyReloader.Loaders
{
    interface ILoader
    {
        void DoLevelLoad(KSPAddon.Startup startup);
    }
}

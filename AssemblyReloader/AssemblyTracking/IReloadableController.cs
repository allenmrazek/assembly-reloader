using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AssemblyTracking.Implementations;

namespace AssemblyReloader.AssemblyTracking
{
    public interface IReloadableController
    {
        void ReloadAll();

        IEnumerable<IReloadableIdentity> ReloadableAssemblies { get; }
    }
}

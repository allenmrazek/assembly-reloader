using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IPartModuleLoader
    {
        // why enumerable? because we might end up loaded multiple PartModules for one descriptor if we're in flight
        // and there are loaded instances of the prefab the descriptor refers to
        IEnumerable<IDisposable> Load(PartModuleDescriptor descriptor, bool inFlight);
    }
}

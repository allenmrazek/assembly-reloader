using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public interface IPartModuleOnStartRunner
    {
        void RunPartModuleOnStarts(IEnumerable<PartModuleDescriptor> descriptors);
    }
}

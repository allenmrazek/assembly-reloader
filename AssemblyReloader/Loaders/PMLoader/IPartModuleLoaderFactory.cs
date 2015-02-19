using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IPartModuleLoaderFactory
    {
        IPartModuleLoader Create(IReloadablePlugin plugin);
    }
}

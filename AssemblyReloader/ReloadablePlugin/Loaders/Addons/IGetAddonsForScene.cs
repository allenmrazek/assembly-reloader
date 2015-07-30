using System;
using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IGetAddonTypesForScene
    {
        IEnumerable<KeyValuePair<Type, ReloadableAddonAttribute>> Get(KSPAddon.Startup scene,
            ILoadedAssemblyHandle handle);
    }
}

using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules.old
{
    public interface IPartModuleFactory
    {
        void Create(IPart part, Type pmType, ConfigNode config);
    }
}

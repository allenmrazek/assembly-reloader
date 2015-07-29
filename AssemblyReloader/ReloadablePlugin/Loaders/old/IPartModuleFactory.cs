using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.old
{
    public interface IPartModuleFactory
    {
        void Create(IPart part, Type pmType, ConfigNode config);
    }
}

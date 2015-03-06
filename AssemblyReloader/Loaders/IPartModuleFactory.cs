using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.Loaders
{
    public interface IPartModuleFactory
    {
        void Create(IPart part, Type pmType, ConfigNode config);
    }
}

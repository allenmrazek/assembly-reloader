using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IPartModuleFactory
    {
        //PartModule AddModule(Type type, Part part, ConfigNode config, bool forceAwake);
        PartModule Create(PartModuleDescriptor descriptor);
    }
}

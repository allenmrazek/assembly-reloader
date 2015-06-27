using System;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public interface IPartModuleUnloader
    {
        void Unload(Type type);
    }
}

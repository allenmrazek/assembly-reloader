using System;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public interface IPartModuleLoader
    {
        void Load(Type type);
    }
}

using System;

namespace AssemblyReloader.Loaders
{
    public interface IPartModuleLoader
    {
        void Load(Type type);
    }
}

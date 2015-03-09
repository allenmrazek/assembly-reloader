using System;

namespace AssemblyReloader.Loaders
{
    public interface IPartModuleUnloader
    {
        void Unload(Type type);
    }
}

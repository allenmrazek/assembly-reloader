using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleUnloader
    {
        void Unload(Type type);
    }
}

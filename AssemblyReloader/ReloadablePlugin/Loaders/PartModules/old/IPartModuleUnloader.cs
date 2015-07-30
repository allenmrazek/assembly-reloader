using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules.old
{
    public interface IPartModuleUnloader
    {
        void Unload(Type type);
    }
}

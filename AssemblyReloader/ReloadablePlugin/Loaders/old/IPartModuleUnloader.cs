using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.old
{
    public interface IPartModuleUnloader
    {
        void Unload(Type type);
    }
}

using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IReloadableObjectFacade
    {
        void Load(Assembly assembly, IFile location);
        void Unload(Assembly assembly, IFile location);
    }
}

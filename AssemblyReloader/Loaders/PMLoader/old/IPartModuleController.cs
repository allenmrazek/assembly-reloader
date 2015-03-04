using System.Reflection;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IPartModuleController
    {
        void LoadPartModules(Assembly assembly);
        void UnloadPartModules(Assembly assembly);
    }
}

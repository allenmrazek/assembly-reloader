using System.Reflection;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IPartModuleLoader
    {
        void LoadPartModuleTypes(Assembly assembly);
        void ClearPartModuleTypes();
    }
}

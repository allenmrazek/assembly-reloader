extern alias KSP;
using System.Reflection;

namespace AssemblyReloader.Config
{
    public interface ILoadedAssemblyUninstaller
    {
        bool Uninstall(Assembly target);
    }
}

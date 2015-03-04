using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public interface IAddonController
    {
        void StartAddonsFrom(Assembly assembly, IFile location);
        void DestroyAddonsFrom(Assembly assembly, IFile location);
    }
}

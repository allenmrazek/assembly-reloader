using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public interface IReloadableObjectController
    {
        void Load(Assembly assembly, IFile location);
        void Unload(Assembly assembly, IFile location);
    }
}

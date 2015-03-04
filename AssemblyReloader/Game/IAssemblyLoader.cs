using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface IAssemblyLoader
    {
        void Load(Assembly assembly, IFile location);
        void Unload(Assembly assembly, IFile location);
    }
}

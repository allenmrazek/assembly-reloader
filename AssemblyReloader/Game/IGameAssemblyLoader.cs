using System;
using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface IGameAssemblyLoader
    {
        IDisposable Load(Assembly assembly, IFile location);

        AssemblyLoader.LoadedAssembyList LoadedAssemblies { get; set; }
    }
}

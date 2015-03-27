using System;
using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface ILoadedAssemblyFactory
    {
        IDisposable Create(Assembly assembly, IFile location);
    }
}

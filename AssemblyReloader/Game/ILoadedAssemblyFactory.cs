using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Game
{
    public interface ILoadedAssemblyFactory
    {
        AssemblyLoader.LoadedAssembly Create(Assembly assembly, IFile location);
    }
}

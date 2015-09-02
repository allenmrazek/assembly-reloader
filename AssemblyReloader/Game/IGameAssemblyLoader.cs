using System.Collections.Generic;
using ReeperAssemblyLibrary;

namespace AssemblyReloader.Game
{
    public interface IGameAssemblyLoader
    {
        IEnumerable<ILoadedAssemblyHandle> LoadedAssemblies { get; }
    }
}

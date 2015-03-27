using System;

namespace AssemblyReloader.Game
{
    public interface IDisposeLoadedAssemblyCommandFactory
    {
        IDisposable Create(AssemblyLoader.LoadedAssembly la);
    }
}

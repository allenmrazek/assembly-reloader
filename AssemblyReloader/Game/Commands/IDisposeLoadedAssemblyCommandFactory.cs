using System;

namespace AssemblyReloader.Game.Commands
{
    public interface IDisposeLoadedAssemblyCommandFactory
    {
        IDisposable Create(AssemblyLoader.LoadedAssembly la);
    }
}

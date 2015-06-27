using System;
using System.Reflection;

namespace AssemblyReloader.Game
{
    public interface ILoadedAssemblyHandle : IDisposable
    {
        Assembly Assembly { get; }
    }
}

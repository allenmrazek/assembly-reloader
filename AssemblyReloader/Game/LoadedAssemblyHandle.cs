using System;
using System.Reflection;

namespace AssemblyReloader.Game
{
    public class LoadedAssemblyHandle : ILoadedAssemblyHandle
    {
        private readonly AssemblyLoader.LoadedAssembly _loadedAssembly;

        public LoadedAssemblyHandle(AssemblyLoader.LoadedAssembly loadedAssembly)
        {
            if (loadedAssembly == null) throw new ArgumentNullException("loadedAssembly");

            _loadedAssembly = loadedAssembly;
        }


        ~LoadedAssemblyHandle()
        {
            Dispose(false);
        }

        
        public void Dispose()
        {
            Dispose(true);
        }


        private void Dispose(bool managed)
        {
            GC.SuppressFinalize(this);

            if (!managed) return;

            for (int idx = 0; idx < AssemblyLoader.loadedAssemblies.Count; ++idx)
                if (ReferenceEquals(_loadedAssembly, AssemblyLoader.loadedAssemblies[idx]))
                {
                    AssemblyLoader.loadedAssemblies.RemoveAt(idx);
                    return;
                }

            throw new Exception("Failed to find assembly " + _loadedAssembly.dllName + " (location " + _loadedAssembly.url +
                                ") in loaded assembly list");
        }


        public Assembly Assembly
        {
            get { return _loadedAssembly.assembly; }
        }
    }
}

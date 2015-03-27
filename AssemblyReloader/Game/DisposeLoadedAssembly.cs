using System;

namespace AssemblyReloader.Game
{
    public class DisposeLoadedAssembly : IDisposable
    {
        private readonly AssemblyLoader.LoadedAssembly _loadedAssembly;

        public DisposeLoadedAssembly(AssemblyLoader.LoadedAssembly loadedAssembly)
        {
            if (loadedAssembly == null) throw new ArgumentNullException("loadedAssembly");

            _loadedAssembly = loadedAssembly;
        }


        ~DisposeLoadedAssembly()
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
    }
}

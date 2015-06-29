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
        

        public AssemblyLoader.LoadedAssembly LoadedAssembly
        {
            get { return _loadedAssembly; }
        }
    }
}

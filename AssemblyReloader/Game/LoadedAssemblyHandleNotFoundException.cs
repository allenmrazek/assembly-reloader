using System;

namespace AssemblyReloader.Game
{
    public class LoadedAssemblyHandleNotFoundException : Exception
    {
        public LoadedAssemblyHandleNotFoundException()
            : base("Loaded assembly handle not found in AssemblyLoader.loadedAssemblies")
        {
            
        }

        public LoadedAssemblyHandleNotFoundException(ILoadedAssemblyHandle handle)
            : base("Loaded assembly handle for " + handle.LoadedAssembly.dllName + " not found in AssemblyLoader.loadedAssemblies")
        {
            
        }

        public LoadedAssemblyHandleNotFoundException(string message)
            : base(message)
        {
            
        }

        public LoadedAssemblyHandleNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}

using System;
using ReeperAssemblyLibrary;

namespace AssemblyReloader.Config
{
    public class ReeperAssemblyNotInCacheException : Exception
    {
        public ReeperAssemblyNotInCacheException() : base("ReeperAssembly was not found in cache")
        {
            
        }

        public ReeperAssemblyNotInCacheException(string message) : base(message)
        {
            
        }

        public ReeperAssemblyNotInCacheException(string message, Exception inner) : base(message, inner)
        {
            
        }

        public ReeperAssemblyNotInCacheException(ILoadedAssemblyHandle assembly)
            : base(assembly.LoadedAssembly.name + " not found in cache")
        {
            
        }
    }
}

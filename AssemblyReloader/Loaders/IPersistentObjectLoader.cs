using System;
using System.Collections.Generic;
using AssemblyReloader.Loaders.PMLoader;

namespace AssemblyReloader.Loaders
{
    public interface IPersistentObjectLoader
    {
        void Load(Type type, bool inFlight);
    }
}

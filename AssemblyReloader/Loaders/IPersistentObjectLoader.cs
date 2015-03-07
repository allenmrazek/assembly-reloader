using System;

namespace AssemblyReloader.Loaders
{
    public interface IPersistentObjectLoader
    {
        void Load(Type type, bool inFlight);
    }
}

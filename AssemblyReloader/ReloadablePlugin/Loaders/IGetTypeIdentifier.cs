using System;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IGetTypeIdentifier
    {
        TypeIdentifier Get(Type type);
    }
}

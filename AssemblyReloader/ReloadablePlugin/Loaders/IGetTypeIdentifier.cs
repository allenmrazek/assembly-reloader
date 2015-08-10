using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IGetTypeIdentifier
    {
        TypeIdentifier Get(Type type);
    }
}

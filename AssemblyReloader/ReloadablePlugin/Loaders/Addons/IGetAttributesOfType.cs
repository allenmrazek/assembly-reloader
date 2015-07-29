using System;
using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IGetAttributesOfType<T>
    {
        IEnumerable<T> Get(Type type);
    }
}

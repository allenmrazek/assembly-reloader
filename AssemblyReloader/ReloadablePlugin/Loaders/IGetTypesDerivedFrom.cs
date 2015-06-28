using System;
using System.Collections.Generic;
using System.Reflection;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IGetTypesDerivedFrom<T>
    {
        IEnumerable<Type> Get(Assembly assembly);
    }
}

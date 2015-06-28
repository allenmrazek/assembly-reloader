using System;
using System.Collections.Generic;
using System.Reflection;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IGetTypesFromAssembly
    {
        IEnumerable<Type> Get(Assembly assembly);
    }
}

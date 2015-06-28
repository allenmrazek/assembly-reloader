using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class GetTypesDerivedFrom<T> : IGetTypesDerivedFrom<T>, IGetTypesFromAssembly
    {
        public IEnumerable<Type> Get(Assembly assembly)
        {
            return assembly.GetTypes().Where(ty => ty.IsSubclassOf(typeof (T)) && !ty.IsAbstract && !ty.IsGenericType);
        }
    }
}

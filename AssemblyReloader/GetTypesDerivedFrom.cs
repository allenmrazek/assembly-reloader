using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.ReloadablePlugin.Loaders;

namespace AssemblyReloader
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetTypesDerivedFrom<T> : IGetTypesDerivedFrom<T>, IGetTypesFromAssembly
    {
        public IEnumerable<Type> Get(Assembly assembly)
        {
            return assembly.GetTypes().Where(ty => ty.IsSubclassOf(typeof (T)) && !ty.IsAbstract && !ty.IsGenericType);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyReloader.Queries.AssemblyQueries
{
    public class TypesDerivedFromQuery<T> : ITypesDerivedFromQuery<T>
    {
        public IEnumerable<Type> Get(Assembly assembly)
        {
            return assembly.GetTypes().Where(ty => ty.IsSubclassOf(typeof (T)) && !ty.IsAbstract);
        }
    }
}

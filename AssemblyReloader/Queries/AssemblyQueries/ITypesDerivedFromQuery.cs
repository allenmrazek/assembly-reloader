using System;
using System.Collections.Generic;
using System.Reflection;

namespace AssemblyReloader.Queries.AssemblyQueries
{
    public interface ITypesDerivedFromQuery<T>
    {
        IEnumerable<Type> Get(Assembly assembly);
    }
}

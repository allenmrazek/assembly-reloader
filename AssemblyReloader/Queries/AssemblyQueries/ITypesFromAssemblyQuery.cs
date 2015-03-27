using System;
using System.Collections.Generic;
using System.Reflection;

namespace AssemblyReloader.Queries.AssemblyQueries
{
    public interface ITypesFromAssemblyQuery
    {
        IEnumerable<Type> Get(Assembly assembly);
    }
}

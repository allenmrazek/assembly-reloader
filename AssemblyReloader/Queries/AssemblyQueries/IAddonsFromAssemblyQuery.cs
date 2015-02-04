using System;
using System.Collections.Generic;

namespace AssemblyReloader.Queries.AssemblyQueries
{
    public interface IAddonsFromAssemblyQuery
    {
        IEnumerable<Type> Get();
    }
}

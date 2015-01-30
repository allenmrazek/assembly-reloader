using System;
using System.Collections.Generic;

namespace AssemblyReloader.Queries
{
    public interface IAddonsFromAssemblyQuery
    {
        IEnumerable<Type> Get();
    }
}

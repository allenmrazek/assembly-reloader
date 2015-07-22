using System.Collections.Generic;
using System.Reflection;

namespace AssemblyReloader.Unsorted
{
    public interface IGetTypesFromAssembly<T>
    {
        IEnumerable<T> Get(Assembly assembly);
    }
}

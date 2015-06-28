using System.Collections.Generic;
using System.Reflection;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IGetTypesFromAssembly<T>
    {
        IEnumerable<T> Get(Assembly assembly);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyReloader.ReloadablePlugin
{
    public class GetAttributesOfType<T> : IGetAttributesOfType<T>
    {
        public IEnumerable<T> Get(Type type)
        {
            return type.GetCustomAttributes(typeof (T), false).Cast<T>();
        }
    }
}

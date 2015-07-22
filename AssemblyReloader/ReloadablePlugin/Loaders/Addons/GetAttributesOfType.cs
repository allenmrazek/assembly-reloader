using System;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetAttributesOfType<T> : IGetAttributesOfType<T>
    {
        public IEnumerable<T> Get(Type type)
        {
            return type.GetCustomAttributes(typeof (T), false).Cast<T>();
        }
    }
}

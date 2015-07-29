using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GetAttributesOfType<T> : IGetAttributesOfType<T>
    {
        public IEnumerable<T> Get(Type type)
        {
            return type.GetCustomAttributes(typeof(T), true).Cast<T>();
        }
    }
}

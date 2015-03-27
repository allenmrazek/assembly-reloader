using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

namespace AssemblyReloader.Providers.Game
{
    public class LoadedComponentProvider : ILoadedComponentProvider
    {
        public IEnumerable<Object> GetLoaded(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (!type.IsSubclassOf(typeof (Object)))
                throw new ArgumentException(type.FullName + " is not derived from UnityEngine.Object");

            return Object.FindObjectsOfType(type);
        }
    }

    public class LoadedComponentProvider<T> : ILoadedComponentProvider<T> where T : UnityEngine.Object
    {
        public IEnumerable<T> Get()
        {
            return Object.FindObjectsOfType(typeof (T)).Cast<T>();
        }
    }
}

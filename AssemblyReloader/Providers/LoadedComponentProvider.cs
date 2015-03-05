using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.Providers
{
    public class LoadedComponentProvider : ILoadedComponentProvider
    {
        public IEnumerable<Object> GetLoaded(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (!type.IsSubclassOf(typeof (UnityEngine.Object)))
                throw new ArgumentException(type.FullName + " is not derived from UnityEngine.Object");

            return UnityEngine.Object.FindObjectsOfType(type);
        }
    }
}

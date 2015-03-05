using System;
using System.Collections.Generic;

namespace AssemblyReloader.Providers
{
    public interface ILoadedComponentProvider
    {
        IEnumerable<UnityEngine.Object> GetLoaded(Type type);
    }
}

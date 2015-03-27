using System;
using System.Collections.Generic;

namespace AssemblyReloader.Providers.Game
{
    public interface ILoadedComponentProvider
    {
        IEnumerable<UnityEngine.Object> GetLoaded(Type type);
    }

    public interface ILoadedComponentProvider<T> where T : UnityEngine.Object
    {
        IEnumerable<T> Get();
    }
}

using System;
using System.Collections.Generic;

namespace AssemblyReloader.Game.Queries
{
    public interface ILoadedComponentQuery
    {
        IEnumerable<UnityEngine.Object> GetLoaded(Type type);
    }

    public interface ILoadedComponentQuery<T> where T : UnityEngine.Object
    {
        IEnumerable<T> Get();
    }
}

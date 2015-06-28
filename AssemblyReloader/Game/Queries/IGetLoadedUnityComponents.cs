using System;
using System.Collections.Generic;

namespace AssemblyReloader.Game.Queries
{
    public interface IGetLoadedUnityComponents
    {
        IEnumerable<UnityEngine.Object> GetLoaded(Type type);
    }

    public interface IGetLoadedUnityComponents<T> where T : UnityEngine.Object
    {
        IEnumerable<T> Get();
    }
}

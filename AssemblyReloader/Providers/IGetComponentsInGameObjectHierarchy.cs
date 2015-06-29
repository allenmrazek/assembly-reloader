using System.Collections.Generic;
using UnityEngine;

namespace AssemblyReloader.Providers
{
    public interface IGetComponentsInGameObjectHierarchy<T> where T:Component
    {
        IEnumerable<T> Get(GameObject gameObject);
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace AssemblyReloader.Unsorted
{
    public interface IGetComponentsInGameObjectHierarchy<T> where T:Component
    {
        IEnumerable<T> Get(GameObject gameObject);
    }
}

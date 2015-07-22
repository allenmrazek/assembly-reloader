using System.Collections.Generic;
using UnityEngine;

namespace AssemblyReloader.Unsorted
{
    public class GetComponentsInGameObjectHierarchy<T> 
        : IGetComponentsInGameObjectHierarchy<T> where T:Component
    {
        public IEnumerable<T> Get(GameObject gameObject)
        {
            return gameObject.GetComponentsInChildren<T>(true);
        }
    }
}

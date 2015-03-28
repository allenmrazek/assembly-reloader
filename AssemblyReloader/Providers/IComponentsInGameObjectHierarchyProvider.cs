using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyReloader.Providers
{
    public interface IComponentsInGameObjectHierarchyProvider<T> where T:Component
    {
        IEnumerable<T> Get(GameObject gameObject);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyReloader.Providers
{
    public class ComponentsInGameObjectHierarchyProvider<T> 
        : IComponentsInGameObjectHierarchyProvider<T> where T:Component
    {
        public IEnumerable<T> Get(GameObject gameObject)
        {
            return gameObject.GetComponentsInChildren<T>(true);
        }
    }
}
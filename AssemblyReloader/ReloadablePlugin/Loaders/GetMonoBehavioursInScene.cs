using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class GetMonoBehavioursInScene : IGetMonoBehavioursInScene
    {
        public IEnumerable<MonoBehaviour> Get(Type target)
        {
            if (!target.IsSubclassOf(typeof (MonoBehaviour)))
                throw new TypeMustBeAMonoBehaviourException(target);

            return UnityEngine.Object.FindObjectsOfType(target).Cast<MonoBehaviour>();
        }
    }
}

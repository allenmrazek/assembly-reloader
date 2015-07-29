using System;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class MonoBehaviourFactory : IMonoBehaviourFactory
    {
        public MonoBehaviour Create(Type addonType)
        {
            if (addonType == null) throw new ArgumentNullException("addonType");
            if (!addonType.IsSubclassOf(typeof (MonoBehaviour)))
                throw new TypeMustBeAMonoBehaviourException(addonType);
            if (addonType.IsAbstract)
                throw new TypeCannotBeAbstractException(addonType);

            var go = new GameObject(addonType.Name);

            return Create(addonType, go);
        }


        public MonoBehaviour Create(Type addonType, GameObject gameObject)
        {
            if (addonType == null) throw new ArgumentNullException("addonType");
            if (gameObject == null) throw new ArgumentNullException("gameObject");

            var mb = gameObject.AddComponent(addonType) as MonoBehaviour;

            if (mb == null)
                throw new ArgumentException("Failed to create " + addonType.FullName + " as MonoBehaviour");

            return mb;
        }
    }
}

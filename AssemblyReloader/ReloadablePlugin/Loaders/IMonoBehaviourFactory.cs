using System;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IMonoBehaviourFactory
    {
        MonoBehaviour Create(Type addonType);
        MonoBehaviour Create(Type addonType, GameObject gameObject);
    }
}

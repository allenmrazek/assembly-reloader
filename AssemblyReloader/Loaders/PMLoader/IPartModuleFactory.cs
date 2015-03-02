using System;
using System.Collections.Generic;
using AssemblyReloader.Game;
using UnityEngine;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IPartModuleFactory
    {
        IDisposable Create(Type type, GameObject gameObject, ConfigNode config);
    }
}

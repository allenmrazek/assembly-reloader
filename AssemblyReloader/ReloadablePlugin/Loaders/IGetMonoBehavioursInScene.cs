using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IGetMonoBehavioursInScene
    {
        IEnumerable<MonoBehaviour> Get(Type target);
    }
}

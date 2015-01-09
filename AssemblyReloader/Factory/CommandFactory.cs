using System;
using AssemblyReloader.Loaders;
using UnityEngine;

namespace AssemblyReloader.Factory
{
    class CommandFactory
    {
        public Action CreateUntrackAddon(GameObject go, IAddonLoader loader)
        {
            return () => loader.StopTrackingAddon(go);
        }
    }
}

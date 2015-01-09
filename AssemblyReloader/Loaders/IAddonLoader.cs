using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyReloader.Loaders
{
    interface IAddonLoader
    {
        void StartTrackingAddon(GameObject go);
        void StopTrackingAddon(GameObject go);
    }
}

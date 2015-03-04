using System;
using System.Collections.Generic;
using AssemblyReloader.Game;
using UnityEngine;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IPartModuleFactory
    {
        // Target PartModule, Proxy PartModule
        ILoadedPartModuleHandle Create(PartModuleDescriptor descriptor);
        ILoadedPartModuleHandle Create(PartModuleDescriptor descriptor, ConfigNode withConfig);
    }
}

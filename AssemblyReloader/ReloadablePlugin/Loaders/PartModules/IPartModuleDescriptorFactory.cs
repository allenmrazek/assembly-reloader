using System;
using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleDescriptorFactory
    {
        IEnumerable<PartModuleDescriptor> Create(Type pmType);
    }
}

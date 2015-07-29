using System;
using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.old
{
    public interface IPartModuleDescriptorFactory
    {
        IEnumerable<PartModuleDescriptor> Create(Type pmType);
    }
}

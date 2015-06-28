using System;
using System.Collections.Generic;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public interface IPartModuleDescriptorFactory
    {
        IEnumerable<PartModuleDescriptor> Create(Type pmType);
    }
}

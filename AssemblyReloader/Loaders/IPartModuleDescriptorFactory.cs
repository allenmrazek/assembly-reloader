using System;
using System.Collections.Generic;

namespace AssemblyReloader.Loaders
{
    public interface IPartModuleDescriptorFactory
    {
        IEnumerable<PartModuleDescriptor> Create(Type pmType);
    }
}

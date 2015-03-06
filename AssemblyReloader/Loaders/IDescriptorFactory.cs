using System;
using System.Collections.Generic;
using AssemblyReloader.Loaders.PMLoader;

namespace AssemblyReloader.Loaders
{
    public interface IDescriptorFactory
    {
        IEnumerable<PartModuleDescriptor> Create(Type pmType);
    }
}

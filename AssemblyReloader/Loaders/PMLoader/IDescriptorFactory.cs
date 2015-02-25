using System;
using System.Collections.Generic;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IDescriptorFactory
    {
        IEnumerable<PartModuleDescriptor> Create(Type pmType);
    }
}

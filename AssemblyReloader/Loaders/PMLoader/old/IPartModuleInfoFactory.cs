using System;
using System.Collections.Generic;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface IPartModuleInfoFactory
    {
        IEnumerable<PartModuleInfo> Create(Type pmType);
    }
}

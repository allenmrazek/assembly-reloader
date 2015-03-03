using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface ILoadedPartModuleHandleFactory
    {
        ILoadedPartModuleHandle Create(PartModule target, PartModuleDescriptor descriptor);
    }
}

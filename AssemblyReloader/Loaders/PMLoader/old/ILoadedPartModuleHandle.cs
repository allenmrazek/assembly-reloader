using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Loaders.PMLoader
{
    public interface ILoadedPartModuleHandle
    {
        PartModule Target { get; }
        PartModuleDescriptor Descriptor { get; }

        void Destroy();
    }
}

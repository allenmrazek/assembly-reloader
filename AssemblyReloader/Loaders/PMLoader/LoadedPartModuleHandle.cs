using System;
using System.Collections.Generic;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class LoadedPartModuleHandle : ILoadedPartModuleHandle
    {
        private readonly IPartModuleUnloader _unloader;

        public LoadedPartModuleHandle(
            PartModuleDescriptor descriptor,
            PartModule target,
            IPartModuleUnloader unloader)
        {
            if (descriptor == null) throw new ArgumentNullException("descriptor");
            if (target == null) throw new ArgumentNullException("target");
            if (unloader == null) throw new ArgumentNullException("unloader");

            Descriptor = descriptor;
            Target = target;
            _unloader = unloader;
        }


        public PartModule Target { get; private set; }
        public PartModuleDescriptor Descriptor { get; private set; }


        public void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}

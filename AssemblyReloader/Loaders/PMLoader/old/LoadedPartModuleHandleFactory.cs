using System;
using System.Collections.Generic;
using AssemblyReloader.Providers;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class LoadedPartModuleHandleFactory : ILoadedPartModuleHandleFactory
    {
        private readonly IPartModuleUnloader _unloader;

        public LoadedPartModuleHandleFactory(
            IPartModuleUnloader unloader)
        {
            if (unloader == null) throw new ArgumentNullException("unloader");

            _unloader = unloader;
        }


        public ILoadedPartModuleHandle Create(PartModule target, PartModuleDescriptor descriptor)
        {
            return new LoadedPartModuleHandle(descriptor, target, _unloader);
        }
    }
}

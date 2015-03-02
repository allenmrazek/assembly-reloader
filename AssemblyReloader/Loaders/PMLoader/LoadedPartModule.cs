using System;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class LoadedPartModule : IDisposable
    {
        private readonly PartModuleDescriptor _descriptor;
        private readonly IPartModuleUnloader _unloader;

        public LoadedPartModule(
            PartModuleDescriptor descriptor,
            IPartModuleUnloader unloader)
        {
            if (descriptor == null) throw new ArgumentNullException("descriptor");
            if (unloader == null) throw new ArgumentNullException("unloader");

            _descriptor = descriptor;
            _unloader = unloader;
        }


        ~LoadedPartModule()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
        }


        private void Dispose(bool managed)
        {
            GC.SuppressFinalize(this);

            if (managed)
            {
                _unloader.Destroy(_descriptor);
            } 
        }
    }
}

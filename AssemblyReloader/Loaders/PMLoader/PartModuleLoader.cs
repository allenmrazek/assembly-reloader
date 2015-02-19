using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Messages;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IReloadablePlugin _owner;

        public PartModuleLoader(IReloadablePlugin owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");
            _owner = owner;
        }


        public void CreateProxyModules()
        {
            throw new NotImplementedException();
        }
    }
}

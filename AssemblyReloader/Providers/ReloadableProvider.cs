using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AddonTracking;

namespace AssemblyReloader.Providers
{
    class ReloadableProvider
    {
        public IEnumerable<ReloadableAssembly> Get()
        {
            return null;
        }
    }
}

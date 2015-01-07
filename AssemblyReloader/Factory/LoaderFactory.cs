using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;

namespace AssemblyReloader.Factory
{
    class LoaderFactory
    {
        private readonly KspAddonProvider _addonProvider;

        public LoaderFactory(KspAddonProvider addonProvider)
        {
            if (addonProvider == null) throw new ArgumentNullException("addonProvider");
            _addonProvider = addonProvider;
        }


        List<ILoader> CreateLoaders()
        {
            return new List<ILoader>
            {
                new KspAddonLoader(_addonProvider)
            };

        }
    }
}

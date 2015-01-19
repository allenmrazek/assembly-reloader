using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Loaders;

namespace AssemblyReloader.Factory
{
    interface ILoaderFactory
    {
        IAddonLoader CreateAddonLoader(IEnumerable<Type> addonTypesInAssembly)
    }
}

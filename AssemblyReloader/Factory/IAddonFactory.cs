using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.TypeTracking;

namespace AssemblyReloader.Factory
{
    interface IAddonFactory
    {
        IDisposable Create(AddonInfo addonInfo);
    }
}

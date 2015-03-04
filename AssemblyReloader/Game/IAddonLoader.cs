using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AssemblyReloader.Game
{
    public interface IAddonLoader
    {
        void StartAddons(Assembly assembly, KSPAddon.Startup scene);
    }
}

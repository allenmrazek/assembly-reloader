using System;
using System.Reflection;
using AssemblyReloader.Addon;
using AssemblyReloader.Loaders.Addon;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
    public interface ILoaderFactory
    {
        IAddonLoader CreateAddonLoader(Assembly assembly, ILog log);
        IDisposable CreatePartModuleLoader(Assembly assembly, ILog log);
    }
}

using System.Reflection;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.AddonLoader
{
    public interface IAddonLoaderFactory
    {
        IAddonLoader Create(Assembly assembly, ILog log);
    }
}

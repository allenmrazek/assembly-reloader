using System.Reflection;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
    public interface ILoaderFactory
    {
        IAddonLoader CreateAddonLoader(Assembly assembly, ILog log);
    }
}

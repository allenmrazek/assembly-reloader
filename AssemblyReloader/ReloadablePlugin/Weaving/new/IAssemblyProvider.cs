using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Weaving.@new
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get();
    }
}

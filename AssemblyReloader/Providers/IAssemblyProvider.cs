using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get();
    }

    public interface IAssemblyProvider<TContext>
    {
        Maybe<Assembly> Get(TContext context);
    }
}

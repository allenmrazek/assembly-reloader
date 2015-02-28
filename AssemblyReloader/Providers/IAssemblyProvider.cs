using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get();

        string Name { get; }
    }

    public interface IAssemblyProvider<TContext>
    {
        Maybe<Assembly> Get(TContext context);

        string Name { get; }
    }
}

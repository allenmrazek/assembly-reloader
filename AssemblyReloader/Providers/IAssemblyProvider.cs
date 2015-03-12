using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers
{
    public interface IAssemblyProvider
    {
        Maybe<Assembly> Get(AssemblyDefinition definition);
    }
}

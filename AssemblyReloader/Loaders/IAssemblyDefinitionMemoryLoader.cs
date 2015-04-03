using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Loaders
{
    public interface IAssemblyDefinitionMemoryLoader
    {
        Maybe<Assembly> LoadDefinition(AssemblyDefinition definition);
    }
}

using System.Reflection;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IAssemblyDefinitionLoader
    {
        Maybe<Assembly> Get(AssemblyDefinition definition);
    }
}

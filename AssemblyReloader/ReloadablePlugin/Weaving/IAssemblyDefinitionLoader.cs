extern alias Cecil96;
using System.Reflection;
using ReeperCommon.Containers;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IAssemblyDefinitionLoader
    {
        Maybe<Assembly> Get(AssemblyDefinition definition);
    }
}

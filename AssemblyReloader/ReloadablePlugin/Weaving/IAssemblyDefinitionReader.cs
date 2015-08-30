extern alias Cecil96;
using ReeperCommon.Containers;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IAssemblyDefinitionReader
    {
        Maybe<AssemblyDefinition> Read();
    }
}

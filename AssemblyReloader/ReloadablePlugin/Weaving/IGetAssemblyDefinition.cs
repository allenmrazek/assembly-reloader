extern alias Cecil96;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IGetAssemblyDefinition
    {
        Maybe<AssemblyDefinition> Get(IFile definitionFile);
    }
}

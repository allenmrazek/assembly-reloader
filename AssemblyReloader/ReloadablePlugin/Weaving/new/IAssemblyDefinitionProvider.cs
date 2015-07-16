using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IAssemblyDefinitionProvider
    {
        Maybe<Mono.Cecil.AssemblyDefinition> Get(IFile location);
    }
}

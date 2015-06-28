using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public interface IAssemblyDefinitionProvider
    {
        Maybe<Mono.Cecil.AssemblyDefinition> Get(IFile location);
    }
}

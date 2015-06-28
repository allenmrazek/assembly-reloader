using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Definition
{
    public interface IAssemblyDefinitionProvider
    {
        Maybe<Mono.Cecil.AssemblyDefinition> Get(IFile location);
    }
}

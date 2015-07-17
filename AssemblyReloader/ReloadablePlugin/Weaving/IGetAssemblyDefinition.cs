using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IGetAssemblyDefinition
    {
        Maybe<AssemblyDefinition> Get(IFile definitionFile);
    }
}

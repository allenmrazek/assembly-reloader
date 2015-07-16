using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public interface IAssemblyDefinitionReader
    {
        Maybe<AssemblyDefinition> Read(IFile location);
    }
}

using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old
{
    public interface IAssemblyDefinitionReader
    {
        Maybe<AssemblyDefinition> Get();

        IFile Location { get; }
    }
}

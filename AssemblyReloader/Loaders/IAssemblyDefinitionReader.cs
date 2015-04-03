using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Loaders
{
    public interface IAssemblyDefinitionReader
    {
        Maybe<AssemblyDefinition> Get();

        IFile Location { get; }
    }
}

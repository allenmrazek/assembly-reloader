using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Disk
{
    public interface IAssemblyDefinitionReader
    {
        Maybe<AssemblyDefinition> Get();
        IFile Location { get; }
        string Name { get; }
    }
}

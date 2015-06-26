using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public interface IAssemblyDefinitionProvider
    {
        Maybe<AssemblyDefinition> Get(IFile location);
    }
}

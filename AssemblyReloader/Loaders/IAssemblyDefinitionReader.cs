using Mono.Cecil;
using ReeperCommon.Containers;

namespace AssemblyReloader.Loaders
{
    public interface IAssemblyDefinitionReader
    {
        Maybe<AssemblyDefinition> Get();
        string Name { get; }
    }
}

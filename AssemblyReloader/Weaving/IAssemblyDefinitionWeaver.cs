using Mono.Cecil;

namespace AssemblyReloader.Weaving
{
    public interface IAssemblyDefinitionWeaver
    {
        bool Weave(AssemblyDefinition assemblyDefinition);
    }
}

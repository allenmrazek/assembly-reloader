using Mono.Cecil;

namespace AssemblyReloader.Weaving.old
{
    public interface IAssemblyDefinitionWeaver
    {
        bool Weave(AssemblyDefinition assemblyDefinition);
    }
}

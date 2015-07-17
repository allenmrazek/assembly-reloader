using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old
{
    public interface IAssemblyDefinitionWeaver
    {
        bool Weave(AssemblyDefinition assemblyDefinition);
    }
}

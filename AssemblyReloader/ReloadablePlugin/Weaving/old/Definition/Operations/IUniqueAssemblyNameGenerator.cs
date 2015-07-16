using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition.Operations
{
    public interface IUniqueAssemblyNameGenerator
    {
        string Get(AssemblyDefinition definition);
    }
}

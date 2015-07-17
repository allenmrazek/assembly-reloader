using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition.Operations
{
    public interface IUniqueAssemblyNameGenerator
    {
        string Get(AssemblyDefinition definition);
    }
}

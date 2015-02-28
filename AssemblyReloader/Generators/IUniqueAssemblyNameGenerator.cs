using Mono.Cecil;

namespace AssemblyReloader.Generators
{
    public interface IUniqueAssemblyNameGenerator
    {
        string Get(AssemblyDefinition definition);
    }
}

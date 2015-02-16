using Mono.Cecil;

namespace AssemblyReloader.ILModifications.Assembly
{
    public interface IModifiedAssemblyFactory
    {
        IModifiedAssembly Create(AssemblyDefinition definition);
    }
}

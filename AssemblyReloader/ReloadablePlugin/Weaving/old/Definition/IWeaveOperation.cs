using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public interface IWeaveOperation
    {
        void Run(AssemblyDefinition definition);
        void OnEachType(TypeDefinition typeDefinition);
        void OnEachMethod(MethodDefinition methodDefinition);
    }
}

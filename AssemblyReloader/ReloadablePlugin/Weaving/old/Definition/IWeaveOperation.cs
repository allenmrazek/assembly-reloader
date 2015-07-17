using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition
{
    public interface IWeaveOperation
    {
        void Run(AssemblyDefinition definition);
        void OnEachType(TypeDefinition typeDefinition);
        void OnEachMethod(MethodDefinition methodDefinition);
    }
}

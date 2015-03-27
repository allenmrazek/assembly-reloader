using Mono.Cecil;

namespace AssemblyReloader.Weaving
{
    public interface IWeaveOperation
    {
        void Run(AssemblyDefinition definition);
        void OnEachType(TypeDefinition typeDefinition);
        void OnEachMethod(MethodDefinition methodDefinition);
    }
}

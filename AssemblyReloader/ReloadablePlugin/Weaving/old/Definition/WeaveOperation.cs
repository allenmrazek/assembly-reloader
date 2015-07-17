using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition
{
    public abstract class WeaveOperation : IWeaveOperation
    {
        public abstract void Run(AssemblyDefinition definition);

        public virtual void OnEachType(TypeDefinition typeDefinition)
        {
            
        }

        public virtual void OnEachMethod(MethodDefinition methodDefinition)
        {
            
        }
    }
}

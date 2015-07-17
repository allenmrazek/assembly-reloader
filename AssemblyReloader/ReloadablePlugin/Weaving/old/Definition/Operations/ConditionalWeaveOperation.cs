using System;
using AssemblyReloader.Properties;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition.Operations
{
    public class ConditionalWeaveOperation : IWeaveOperation
    {
        private readonly IWeaveOperation _operation;
        private readonly Func<bool> _condition;

        public ConditionalWeaveOperation([NotNull] IWeaveOperation operation, [NotNull] Func<bool> condition)
        {
            if (operation == null) throw new ArgumentNullException("operation");
            if (condition == null) throw new ArgumentNullException("condition");
            _operation = operation;
            _condition = condition;
        }


        public void Run(AssemblyDefinition definition)
        {
            if (_condition()) _operation.Run(definition);
        }

        public void OnEachType(TypeDefinition typeDefinition)
        {
            if (_condition()) _operation.OnEachType(typeDefinition);
        }

        public void OnEachMethod(MethodDefinition methodDefinition)
        {
            if (_condition()) _operation.OnEachMethod(methodDefinition);
        }
    }
}

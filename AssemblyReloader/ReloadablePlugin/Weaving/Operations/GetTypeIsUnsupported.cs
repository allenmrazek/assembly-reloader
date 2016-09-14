extern alias Cecil96;
using System;
using System.Linq;
using Cecil96::Mono.Cecil;
using Contracts;
using Contracts.Agents;
using Experience;
using ReeperCommon.Containers;
using strange.extensions.injector.api;
using Strategies;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    [Implements(typeof(IGetTypeIsUnsupported), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetTypeIsUnsupported : IGetTypeIsUnsupported
    {
        private static readonly Type[] Unsupported =
        {
            typeof(Part),

            typeof(ScienceExperiment),

            typeof(ContractPredicate),
            typeof(IContractParameterHost),
            typeof(Agent),
            typeof(AgentMentality),

            typeof(ExperienceEffect),
            typeof(ExperienceTrait),

            typeof(Strategy),
            typeof(StrategyEffect)

        };

        public bool Get(TypeDefinition typeDefinition)
        {
            return Unsupported.Any(ut => IsTypeUnsupported(typeDefinition, ut));
        }

        private static bool IsTypeUnsupported(TypeDefinition type, Type check)
        {
            var imported = type.Module.Import(check);

            do
            {
                if (type == imported || type.Interfaces.Any(i => i.Resolve() == imported.Resolve()))
                    return true;

                type = type.BaseType.Return(tr => tr.Resolve(), null);
            } while (type != null);

            return false;
        }
    }
}

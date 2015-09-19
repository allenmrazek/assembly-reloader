extern alias KSP;
extern alias Cecil96;
using System;
using System.Linq;
using ReeperCommon.Containers;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    [Implements(typeof(IGetTypeIsUnsupported), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetTypeIsUnsupported : IGetTypeIsUnsupported
    {
        private static readonly Type[] Unsupported = new[]
        {
            typeof(KSP::Contracts.ContractPredicate),
            typeof(KSP::Contracts.IContractParameterHost),
            typeof(KSP::Contracts.Agents.Agent),
            typeof(KSP::Contracts.Agents.AgentMentality),

            typeof(KSP::Experience.ExperienceEffect),
            typeof(KSP::Experience.ExperienceTrait),

            typeof(KSP::Strategies.Strategy),
            typeof(KSP::Strategies.StrategyEffect)

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

using ReeperKSP.Serialization;
using strange.extensions.injector.api;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    [Implements(typeof(CoreConfiguration), InjectionBindingScope.CROSS_CONTEXT)]
    public class CoreConfiguration
    {
        public const string NodeName = "CoreConfiguration";

        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming

        [ReeperPersistent] public string PlaceholderValue = "Placeholder string";
    }
}

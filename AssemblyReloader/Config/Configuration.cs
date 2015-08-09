using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    [Implements(typeof(CoreConfiguration), InjectionBindingScope.CROSS_CONTEXT)]
    public class CoreConfiguration
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming

        //[ReeperPersistent] public Setting<bool> ReloadAllReloadablesUponWindowFocus = false;
        //[ReeperPersistent] public Setting<bool> StartKSPAddonsForCurrentScene = false;

    }
}

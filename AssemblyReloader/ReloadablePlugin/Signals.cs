using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin
{
    // prompt a plugin reload
    public class SignalReloadPlugin : Signal<Maybe<ILoadedAssemblyHandle>>
    {
        
    }


    public class SignalInstallPluginTypes : Signal<ILoadedAssemblyHandle>
    {
        
    }

    public class SignalUninstallPluginTypes : Signal<ILoadedAssemblyHandle>
    {
        
    }

    // plugin was loaded and types installed
    public class SignalPluginWasLoaded : Signal<ILoadedAssemblyHandle>
    {
        
    }


    // plugin was unloaded and types uninstalled
    public class SignalPluginWasUnloaded : Signal
    {
        
    }
}

using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin
{
    // prompt a plugin reload
    public class SignalReloadPlugin : Signal<Maybe<ILoadedAssemblyHandle>>
    {
        
    }

    // sent after the next iteration of assembly's definition was created and loaded into memory
    // successfully: types from the previous plugin should be unloaded at this point
    public class SignalUnloadPlugin : Signal<ILoadedAssemblyHandle>
    {
        
    }



    public class SignalPluginWasLoaded : Signal<ILoadedAssemblyHandle>
    {
        
    }


    public class SignalPluginWasUnloaded : Signal
    {
        
    }
}

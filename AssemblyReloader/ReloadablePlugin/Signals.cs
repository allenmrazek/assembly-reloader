using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using strange.extensions.signal.impl;

// ReSharper disable ClassNeverInstantiated.Global
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


    // Restore LoadedAssembly object to AssemblyLoader
    public class SignalReinstallPlugin : Signal<Maybe<ILoadedAssemblyHandle>>
    {
        
    }

    public class SignalPluginCannotBeLoaded : Signal<string>
    {
        
    }

    public class SignalErrorWhileUnloading : Signal<string>
    {
        
    }
}

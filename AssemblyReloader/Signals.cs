using AssemblyReloader.Gui;
using strange.extensions.signal.impl;

// ReSharper disable ClassNeverInstantiated.Global
namespace AssemblyReloader
{
    public class SignalStart : Signal
    {
    }


    public class SignalCloseAllWindows : Signal
    {

    }


    public class SignalTogglePluginConfigurationView : Signal<IPluginInfo>
    {
        
    }


    public class SignalOnSaveConfiguration : Signal<ConfigNode>
    {
    }


    public class SignalOnLoadConfiguration : Signal<ConfigNode>
    {

    }
}

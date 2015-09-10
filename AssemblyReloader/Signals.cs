extern alias KSP;
using AssemblyReloader.Gui;
using strange.extensions.signal.impl;

namespace AssemblyReloader
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalStart : Signal
    {
    }


// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalCloseAllWindows : Signal
    {

    }


    public class SignalTogglePluginConfigurationView : Signal<IPluginInfo>
    {
        
    }

    public class SignalSaveConfiguration : Signal<KSP::ConfigNode>
    {
    }

    public class SignalLoadConfiguration : Signal<KSP::ConfigNode>
    {

    }
}

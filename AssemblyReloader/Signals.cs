﻿using AssemblyReloader.Gui;
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
}

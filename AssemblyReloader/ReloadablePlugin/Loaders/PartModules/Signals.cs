﻿using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SignalPartModuleCreated : Signal<IPart, PartModule, PartModuleDescriptor>
    {
    }
}
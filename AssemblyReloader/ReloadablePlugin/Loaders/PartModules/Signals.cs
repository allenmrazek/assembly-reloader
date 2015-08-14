extern alias KSP;
using strange.extensions.signal.impl;


namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SignalPartModuleCreated : Signal<IPart, KSP::PartModule, PartModuleDescriptor>
    {
    }
}

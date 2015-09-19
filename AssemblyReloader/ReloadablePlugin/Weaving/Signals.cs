extern alias Cecil96;
using strange.extensions.signal.impl;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalWeaveDefinition : Signal<AssemblyDefinition>
    {
    }
}

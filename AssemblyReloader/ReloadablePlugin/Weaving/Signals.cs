extern alias Cecil96;
using strange.extensions.signal.impl;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class SignalWeaveDefinition : Signal<AssemblyDefinition>
    {
    }


}

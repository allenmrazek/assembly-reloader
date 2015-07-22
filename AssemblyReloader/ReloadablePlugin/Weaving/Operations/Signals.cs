using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class SignalHelperDefinitionCreated : Signal<AssemblyDefinition, TypeDefinition>
    {
        
    }
}

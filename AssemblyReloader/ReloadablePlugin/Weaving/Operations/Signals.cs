using Mono.Cecil;
using strange.extensions.signal.impl;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class SignalHelperDefinitionCreated : Signal<AssemblyDefinition, TypeDefinition>
    {
        
    }
}

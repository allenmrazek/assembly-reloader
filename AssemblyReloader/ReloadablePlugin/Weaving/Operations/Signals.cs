extern alias Cecil96;
using strange.extensions.signal.impl;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class SignalHelperDefinitionCreated : Signal<AssemblyDefinition, TypeDefinition>
    {
        
    }
}

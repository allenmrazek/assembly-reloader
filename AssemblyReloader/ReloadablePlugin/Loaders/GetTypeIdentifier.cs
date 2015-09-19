using System;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    // Where type identifier is the name KSP would use to search for this type in assemblies loaded by
    // its AssemblyLoader
    [Implements(typeof(IGetTypeIdentifier), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class GetTypeIdentifier : IGetTypeIdentifier
    {
        public TypeIdentifier Get(Type type)
        {
            return new TypeIdentifier(type.Name);
        }
    }
}
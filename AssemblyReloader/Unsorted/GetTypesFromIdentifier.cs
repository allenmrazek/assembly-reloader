using System;
using AssemblyReloader.DataObjects;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;

namespace AssemblyReloader.Unsorted
{
    // Where type identifier is the name KSP would use to search for this type in assemblies loaded by
    // its AssemblyLoader
    [Implements(typeof(IGetTypeIdentifier), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetTypeIdentifier : IGetTypeIdentifier
    {
        public ITypeIdentifier Get(Type type)
        {
            return new TypeIdentifier(type.Name);
        }
    }
}
using System;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Unsorted
{
    // Where type identifier is the name KSP would use to search for this type in assemblies loaded by
    // its AssemblyLoader
    public class GetTypeIdentifier : IGetTypeIdentifier
    {
        public ITypeIdentifier Get(Type type)
        {
            return new TypeIdentifier(type.Name);
        }
    }
}

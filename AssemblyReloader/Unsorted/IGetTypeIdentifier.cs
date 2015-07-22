using System;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Unsorted
{
    public interface IGetTypeIdentifier
    {
        ITypeIdentifier Get(Type type);
    }
}

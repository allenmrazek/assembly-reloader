using System;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Unsorted
{
    public interface IGetTypeIdentifier
    {
        TypeIdentifier Get(Type type);
    }
}

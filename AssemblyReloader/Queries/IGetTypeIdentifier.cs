using System;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Queries
{
    public interface IGetTypeIdentifier
    {
        ITypeIdentifier Get(Type type);
    }
}

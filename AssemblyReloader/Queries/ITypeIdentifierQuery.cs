using System;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Queries
{
    public interface ITypeIdentifierQuery
    {
        ITypeIdentifier Get(Type type);
    }
}

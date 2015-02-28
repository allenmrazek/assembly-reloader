using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Queries
{
    public interface ITypeIdentifierQuery
    {
        ITypeIdentifier Get(Type type);
    }
}

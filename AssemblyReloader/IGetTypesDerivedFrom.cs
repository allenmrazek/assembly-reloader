using System;
using System.Collections.Generic;
using System.Reflection;

namespace AssemblyReloader
{
    public interface IGetTypesDerivedFrom<T>
    {
        IEnumerable<Type> Get(Assembly assembly);
    }
}

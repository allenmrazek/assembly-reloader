using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Generators
{
    public interface IUniqueFlightIdGenerator
    {
        uint Get();
    }
}

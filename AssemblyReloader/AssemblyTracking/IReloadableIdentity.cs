using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.AssemblyTracking
{
    public interface IReloadableIdentity
    {
        string Name { get; }
        string Location { get; }
    }
}

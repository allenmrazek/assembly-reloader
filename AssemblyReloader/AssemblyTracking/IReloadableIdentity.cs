using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.AssemblyTracking
{
    interface IReloadableIdentity
    {
        string Name { get; }
        string Location { get; }
    }
}

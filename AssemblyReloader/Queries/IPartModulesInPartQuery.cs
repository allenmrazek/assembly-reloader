using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;

namespace AssemblyReloader.Queries
{
    public interface IPartModulesInPartQuery
    {
        IEnumerable<PartModule> Get(IPart part, Type target);
    }
}

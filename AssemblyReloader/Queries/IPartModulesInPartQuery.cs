using System;
using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.Queries
{
    public interface IPartModulesInPartQuery
    {
        IEnumerable<PartModule> Get(IPart part, Type target);
    }
}

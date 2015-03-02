using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;

namespace AssemblyReloader.Queries
{
    public class PartModulesInPartQuery : IPartModulesInPartQuery
    {
        public IEnumerable<PartModule> Get(IPart part, Type target)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (target == null) throw new ArgumentNullException("target");

            return part.GameObject.GetComponents(target).Cast<PartModule>();
        }
    }
}

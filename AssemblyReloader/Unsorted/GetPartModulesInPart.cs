using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;

namespace AssemblyReloader.Unsorted
{
    public class GetPartModulesInPart : IGetPartModulesInPart
    {
        public IEnumerable<PartModule> Get(IPart part, Type target)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (target == null) throw new ArgumentNullException("target");

            return part.GameObject.GetComponents(target).Cast<PartModule>();
        }
    }
}

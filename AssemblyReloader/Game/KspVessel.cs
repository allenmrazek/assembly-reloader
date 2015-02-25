using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    public class KspVessel : IVessel
    {
        private readonly Vessel _vessel;

        public KspVessel(Vessel vessel)
        {
            _vessel = vessel;
            if (vessel == null) throw new ArgumentNullException("vessel");
        }
    }
}

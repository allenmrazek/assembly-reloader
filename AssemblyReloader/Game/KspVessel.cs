using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Game
{
    public class KspVessel : IVessel
    {
        private readonly Vessel _vessel;
        private readonly IKspFactory _kspFactory;


        public KspVessel(Vessel vessel, IKspFactory kspFactory)
        {
            if (vessel == null) throw new ArgumentNullException("vessel");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");


            _vessel = vessel;
            _kspFactory = kspFactory;
        }


        public List<IPart> Parts
        {
            get { return _vessel.Parts.Select(p => _kspFactory.Create(p)).ToList(); }
        }
    }
}

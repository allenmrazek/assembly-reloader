using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Loaders;

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


        public ReadOnlyCollection<IPart> Parts
        {
            get { return _vessel.Parts.Select(p => _kspFactory.Create(p)).ToList().AsReadOnly(); }
        }

        public Vessel.Situations Situation
        {
            get { return _vessel.situation; }
        }

        public Guid ID
        {
            get { return _vessel.id; }
        }
    }
}

extern alias KSP;
using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class KspAvailablePart : IAvailablePart
    {
        private readonly KSP::AvailablePart _ap;
        private readonly IKspFactory _kspFactory;

        public KspAvailablePart(KSP::AvailablePart ap, IKspFactory kspFactory)
        {
            if (ap == null) throw new ArgumentNullException("ap");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            _ap = ap;
            _kspFactory = kspFactory;
        }


        public string Name
        {
            get { return _ap.name; }
        }


        public IPart PartPrefab
        {
            get { return _kspFactory.Create(_ap.partPrefab); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyReloader.Game.Providers
{
    class LoadedVesselProvider : ILoadedVesselProvider
    {
        private readonly IKspFactory _kspFactory;

        public LoadedVesselProvider(IKspFactory kspFactory)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            _kspFactory = kspFactory;
        }


        public IEnumerable<IVessel> Get()
        {
            return FlightGlobals.Vessels.Where(v => v.loaded).Select(v => _kspFactory.Create(v));
        }
    }
}

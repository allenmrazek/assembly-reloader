using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;

namespace AssemblyReloader.Providers
{
    class LoadedVesselProvider : ILoadedVesselProvider
    {
        public IEnumerable<IVessel> Get()
        {
            return FlightGlobals.Vessels.Where(v => v.loaded).Select(v => new KspVessel(v)).Cast<IVessel>();
        }
    }
}

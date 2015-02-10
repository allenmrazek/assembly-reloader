using System.Collections.Generic;
using System.Linq;

namespace AssemblyReloader.Providers
{
    class LoadedVesselProvider : ILoadedVesselProvider
    {
        public IEnumerable<Vessel> Get()
        {
            return FlightGlobals.Vessels.Where(v => v.loaded);
        }
    }
}

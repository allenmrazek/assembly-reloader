using System.Collections.Generic;

namespace AssemblyReloader.Providers
{
    public interface ILoadedVesselProvider
    {
        IEnumerable<Vessel> Get();
    }
}

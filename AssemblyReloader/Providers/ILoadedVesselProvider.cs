using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.Providers
{
    public interface ILoadedVesselProvider
    {
        IEnumerable<IVessel> Get();
    }
}

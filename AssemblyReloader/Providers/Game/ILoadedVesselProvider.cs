using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.Providers.Game
{
    public interface ILoadedVesselProvider
    {
        IEnumerable<IVessel> Get();
    }
}

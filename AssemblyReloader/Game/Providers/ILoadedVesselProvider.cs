using System.Collections.Generic;

namespace AssemblyReloader.Game.Providers
{
    public interface ILoadedVesselProvider
    {
        IEnumerable<IVessel> Get();
    }
}

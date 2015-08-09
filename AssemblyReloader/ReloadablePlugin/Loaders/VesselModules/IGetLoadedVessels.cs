using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public interface IGetLoadedVessels
    {
        IEnumerable<IVessel> Get();
    }
}

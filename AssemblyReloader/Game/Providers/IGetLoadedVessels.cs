using System.Collections.Generic;

namespace AssemblyReloader.Game.Providers
{
    public interface IGetLoadedVessels
    {
        IEnumerable<IVessel> Get();
    }
}

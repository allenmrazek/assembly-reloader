using System.Collections.Generic;

namespace AssemblyReloader.Game
{
    public interface IVessel
    {
        List<IPart> Parts { get; }
        Vessel.Situations situation { get; }
    }
}

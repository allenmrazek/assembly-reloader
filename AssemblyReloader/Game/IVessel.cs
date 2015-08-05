using System;
using System.Collections.Generic;

namespace AssemblyReloader.Game
{
    public interface IVessel
    {
        List<IPart> Parts { get; }
        Vessel.Situations Situation { get; }

// ReSharper disable once InconsistentNaming
        Guid ID { get; }
    }
}

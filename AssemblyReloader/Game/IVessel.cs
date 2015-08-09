using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AssemblyReloader.Game
{
    public interface IVessel
    {
        ReadOnlyCollection<IPart> Parts { get; }
        Vessel.Situations Situation { get; }

// ReSharper disable once InconsistentNaming
        Guid ID { get; }
        GameObject gameObject { get; }
    }
}

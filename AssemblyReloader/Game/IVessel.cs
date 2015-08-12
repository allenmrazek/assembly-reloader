extern alias KSP;
using System;
using System.Collections.ObjectModel;
using AssemblyReloader.ReloadablePlugin.Loaders;
using UnityEngine;
using Vessel = KSP::Vessel;

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

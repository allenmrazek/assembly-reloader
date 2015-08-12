extern alias KSP;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IPart
    {
        void RemoveModule(KSP::PartModule pm);

        GameObject GameObject { get; }
        KSP::PartModuleList Modules { get; }
        string Name { get; }
        string PartName { get; }
        IAvailablePart PartInfo { get; }
        uint FlightID { get; set; }
        Maybe<IVessel> Vessel { get; }
    }
}

using AssemblyReloader.Game;
using ReeperCommon.Containers;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IPart
    {
        void RemoveModule(PartModule pm);

        GameObject GameObject { get; }
        PartModuleList Modules { get; }
        string Name { get; }
        string PartName { get; }
        IAvailablePart PartInfo { get; }
        uint FlightID { get; set; }
        Maybe<IVessel> Vessel { get; }
    }
}

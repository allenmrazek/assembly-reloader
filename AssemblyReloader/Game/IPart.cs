using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AssemblyReloader.Game
{
    public interface IPart
    {
        void RemoveModule(PartModule pm);

        GameObject GameObject { get; }
        PartModuleList Modules { get; }
        string Name { get; }
        string PartName { get; }
        IAvailablePart PartInfo { get; }
        uint FlightID { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;

namespace AssemblyReloader.Loaders
{
    public interface IPartModuleSnapshotGenerator
    {
        void Snapshot(IPart part, PartModule instance);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public interface IGameEventProxy
    {
        void AddRegistry(ILoadedAssemblyHandle handle, IGameEventRegistry registry);
        void RemoveRegistry(ILoadedAssemblyHandle handle);
    }
}

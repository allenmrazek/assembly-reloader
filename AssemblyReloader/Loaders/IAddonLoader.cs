using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Loaders
{
    interface IAddonLoader : ILoader, IDisposable
    {
        void LoadAddonsForScene(GameScenes scene);
    }
}

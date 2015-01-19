using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Loaders
{
    interface IAddonLoader : IDisposable
    {
        void LoadAddonsForScene(GameScenes scene);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Loaders
{
    interface IAddonLoader : ILoader
    {
        void LoadAddonsForScene(GameScenes scene);
        void Dispose();
    }
}

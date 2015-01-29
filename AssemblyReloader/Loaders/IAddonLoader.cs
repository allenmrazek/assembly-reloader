using System;

namespace AssemblyReloader.Loaders
{
    interface IAddonLoader : IDisposable
    {
        void LoadAddonsForScene(GameScenes scene);
    }
}

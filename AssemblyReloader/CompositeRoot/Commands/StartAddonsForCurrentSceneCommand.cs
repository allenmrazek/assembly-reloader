using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using AssemblyReloader.Providers.SceneProviders;

namespace AssemblyReloader.CompositeRoot.Commands
{
    public class StartAddonsForCurrentSceneCommand : ICommand
    {
        private readonly ICurrentStartupSceneProvider _currentSceneProvider;
        private readonly IAssemblyProvider _targetAssemblyProvider;
        private readonly IAddonLoader _addonLoader;

        public StartAddonsForCurrentSceneCommand(
            IAddonLoader addonLoader,
            ICurrentStartupSceneProvider currentSceneProvider,
            IAssemblyProvider targetAssemblyProvider
            )
        {
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");
            if (currentSceneProvider == null) throw new ArgumentNullException("currentSceneProvider");
            if (targetAssemblyProvider == null) throw new ArgumentNullException("targetAssemblyProvider");

            _addonLoader = addonLoader;
            _currentSceneProvider = currentSceneProvider;
            _targetAssemblyProvider = targetAssemblyProvider;
        }


        public void Execute()
        {
            var assembly = _targetAssemblyProvider.Get();

            if (!assembly.Any())
                throw new Exception("Failed to retrieve assembly");

            _addonLoader.StartAddons(assembly.Single(), _currentSceneProvider.Get());
        }
    }
}

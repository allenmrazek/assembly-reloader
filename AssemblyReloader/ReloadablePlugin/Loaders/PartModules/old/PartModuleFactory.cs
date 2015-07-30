using System;
using AssemblyReloader.Commands.old;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using AssemblyReloader.Unsorted;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules.old
{
    public class PartModuleFactory : IPartModuleFactory
    {
        private readonly IGetIsPartPrefab _getIsPartPrefab;
        private readonly ICommand<PartModule> _awakenPartModule;
        private readonly IPartModuleOnStartRunner _onStartRunner;

        public PartModuleFactory(
            [NotNull] IGetIsPartPrefab getIsPartPrefab, 
            [NotNull] ICommand<PartModule> awakenPartModule,
            [NotNull] IPartModuleOnStartRunner onStartRunner)
        {
            if (getIsPartPrefab == null) throw new ArgumentNullException("getIsPartPrefab");
            if (awakenPartModule == null) throw new ArgumentNullException("awakenPartModule");
            if (onStartRunner == null) throw new ArgumentNullException("onStartRunner");

            _getIsPartPrefab = getIsPartPrefab;
            _awakenPartModule = awakenPartModule;
            _onStartRunner = onStartRunner;
        }


        public void Create(IPart part, Type pmType, ConfigNode config)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (pmType == null) throw new ArgumentNullException("pmType");
            if (config == null) throw new ArgumentNullException("config");

            if (!pmType.IsSubclassOf(typeof (PartModule)))
                throw new ArgumentException("type " + pmType.FullName + " is not a PartModule");

            var result = part.GameObject.AddComponent(pmType) as PartModule;

            if (result == null)
                throw new Exception("Failed to add " + pmType.FullName + " to " + part.PartName);

            part.Modules.Add(result);


            // if this is the prefab GameObject, it will never become active again and awake will never
            // get called so we must do it ourselves
            if (_getIsPartPrefab.Get(part))
                _awakenPartModule.Execute(result);

            result.OnLoad(config);
            _onStartRunner.Add(result);
        }
    }
}

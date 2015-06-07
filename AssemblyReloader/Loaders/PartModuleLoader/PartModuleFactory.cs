using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Game;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public class PartModuleFactory : IPartModuleFactory
    {
        private readonly IPartIsPrefabQuery _partIsPrefabQuery;
        private readonly ICommand<PartModule> _awakenPartModule;
        private readonly IPartModuleOnStartRunner _onStartRunner;

        public PartModuleFactory(
            [NotNull] IPartIsPrefabQuery partIsPrefabQuery, 
            [NotNull] ICommand<PartModule> awakenPartModule,
            [NotNull] IPartModuleOnStartRunner onStartRunner)
        {
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (awakenPartModule == null) throw new ArgumentNullException("awakenPartModule");
            if (onStartRunner == null) throw new ArgumentNullException("onStartRunner");

            _partIsPrefabQuery = partIsPrefabQuery;
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
            if (_partIsPrefabQuery.Get(part))
                _awakenPartModule.Execute(result);

            result.OnLoad(config);
            _onStartRunner.Add(result);
        }
    }
}

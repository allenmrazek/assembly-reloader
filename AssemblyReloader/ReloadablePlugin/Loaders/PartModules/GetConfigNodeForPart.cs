extern alias KSP;
using System;
using System.Linq;
using ReeperCommon.Containers;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    [Implements(typeof(IGetConfigNodeForPart), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class GetConfigNodeForPart : IGetConfigNodeForPart
    {
        private readonly IGameDatabase _gameDatabase;

        public GetConfigNodeForPart(IGameDatabase gameDatabase)
        {
            if (gameDatabase == null) throw new ArgumentNullException("gameDatabase");
            _gameDatabase = gameDatabase;
        }


        public Maybe<KSP::ConfigNode> Get(IAvailablePart availablePart)
        {
            var found = _gameDatabase.GetConfigs("PART")
                .FirstOrDefault(u => u.name.Replace('_', '.') == availablePart.Name);

            return found == null ? Maybe<KSP::ConfigNode>.None : Maybe<KSP::ConfigNode>.With(found.config);
        }
    }
}

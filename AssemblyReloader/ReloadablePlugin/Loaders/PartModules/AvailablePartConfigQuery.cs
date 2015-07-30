using System;
using System.Linq;
using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class GetConfigNodeForPart : IGetConfigNodeForPart
    {
        private readonly IGameDatabase _gameDatabase;

        public GetConfigNodeForPart(IGameDatabase gameDatabase)
        {
            if (gameDatabase == null) throw new ArgumentNullException("gameDatabase");
            _gameDatabase = gameDatabase;
        }


        public Maybe<ConfigNode> Get(IAvailablePart availablePart)
        {
            var found = _gameDatabase.GetConfigs("PART")
                .FirstOrDefault(u => u.name.Replace('_', '.') == availablePart.Name);

            return found == null ? Maybe<ConfigNode>.None : Maybe<ConfigNode>.With(found.config);
        }
    }
}

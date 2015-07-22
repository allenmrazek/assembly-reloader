using System;
using AssemblyReloader.Properties;
using AssemblyReloader.Unsorted;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game.Providers
{
    public class GetCurrentGame : IGetCurrentGame
    {
        private readonly IGetTypeIdentifier _getTypeIdentifier;

        public GetCurrentGame([NotNull] IGetTypeIdentifier getTypeIdentifier)
        {
            if (getTypeIdentifier == null) throw new ArgumentNullException("getTypeIdentifier");
            _getTypeIdentifier = getTypeIdentifier;
        }


        public Maybe<IGame> Get()
        {
            return HighLogic.CurrentGame == null ? Maybe<IGame>.None : Maybe<IGame>.With(new KspGame(HighLogic.CurrentGame, _getTypeIdentifier));
        }
    }
}

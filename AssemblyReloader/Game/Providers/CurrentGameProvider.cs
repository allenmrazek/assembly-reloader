using System;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game.Providers
{
    public class CurrentGameProvider : ICurrentGameProvider
    {
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;

        public CurrentGameProvider([NotNull] ITypeIdentifierQuery typeIdentifierQuery)
        {
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            _typeIdentifierQuery = typeIdentifierQuery;
        }


        public Maybe<IGame> Get()
        {
            return HighLogic.CurrentGame == null ? Maybe<IGame>.None : Maybe<IGame>.With(new KspGame(HighLogic.CurrentGame, _typeIdentifierQuery));
        }
    }
}

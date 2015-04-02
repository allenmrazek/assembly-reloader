using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using UnityEngine;

namespace AssemblyReloader.Providers.Game
{
    public class GameObjectComponentQuery : IGameObjectComponentQuery
    {
        private readonly IGameObjectProvider _runnerProvider;

        public GameObjectComponentQuery([NotNull] IGameObjectProvider runnerProvider)
        {
            if (runnerProvider == null) throw new ArgumentNullException("runnerProvider");
            _runnerProvider = runnerProvider;
        }


        public IEnumerable<Component> Get(Type componentType)
        {
            var runner = _runnerProvider.Get();

            return !runner.Any() ? new Component[0] : runner.First().GetComponentsInChildren(componentType, true);
        }
    }
}

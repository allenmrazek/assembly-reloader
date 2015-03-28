using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using UnityEngine;

namespace AssemblyReloader.Providers.Game
{
    public class ScenarioRunnerComponentQuery : IScenarioRunnerComponentQuery
    {
        private readonly IScenarioRunnerProvider _runnerProvider;

        public ScenarioRunnerComponentQuery([NotNull] IScenarioRunnerProvider runnerProvider)
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

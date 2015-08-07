using System;
using System.Collections.Generic;

namespace AssemblyReloader.Game
{
    public class KspScenarioRunner : IScenarioRunner
    {
        private readonly ScenarioRunner _runner;
        private readonly IKspFactory _kspFactory;


        public KspScenarioRunner(ScenarioRunner runner, IKspFactory kspFactory)
        {
            if (runner == null) throw new ArgumentNullException("runner");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _runner = runner;
            _kspFactory = kspFactory;
        }


        public List<IProtoScenarioModule> GetUpdatedProtoScenarioModules()
        {
            throw new NotImplementedException();
        }
    }
}

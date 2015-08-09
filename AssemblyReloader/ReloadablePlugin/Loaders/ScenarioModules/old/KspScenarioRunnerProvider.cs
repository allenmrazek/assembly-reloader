//using System;
//using AssemblyReloader.Game;
//using AssemblyReloader.StrangeIoC.extensions.implicitBind;
//using AssemblyReloader.StrangeIoC.extensions.injector.api;
//using ReeperCommon.Containers;

//namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
//{
//    [Implements(typeof(IScenarioRunnerProvider), InjectionBindingScope.CROSS_CONTEXT)]
//    public class KspScenarioRunnerProvider : IScenarioRunnerProvider
//    {
//        private readonly IKspFactory _kspFactory;

//        public KspScenarioRunnerProvider(IKspFactory kspFactory)
//        {
//            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
//            _kspFactory = kspFactory;
//        }


//        public Maybe<IScenarioRunner> Get()
//        {
//            return ScenarioRunner.fetch != null
//                ? Maybe<IScenarioRunner>.With(_kspFactory.Create(ScenarioRunner.fetch))
//                : Maybe<IScenarioRunner>.None;
//        }
//    }
//}

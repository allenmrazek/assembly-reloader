using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Providers.Game;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Loaders
{
    public class ScenarioModuleLoader : IScenarioModuleLoader
    {
        private readonly IProtoScenarioModuleProvider _psmProvider;


        public ScenarioModuleLoader(
            [NotNull] IProtoScenarioModuleProvider psmProvider)
        {
            if (psmProvider == null) throw new ArgumentNullException("psmProvider");

            _psmProvider = psmProvider;
        }


        public void Load([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            // note: it's possible in theory for there to be multiple, duplicate ScenarioModules
            // note: we do not add ScenarioModule entries ourselves: the stock behaviour is that these
            //       are added during the transition from main menu into space center and I want to duplicate
            //       that behaviour
            foreach (var psm in _psmProvider.Get(type))
                InstallScenarioModule(type, psm);
        }


        private void InstallScenarioModule(Type type, IProtoScenarioModule psm)
        {
            if (psm.moduleRef.Any())
                throw new InvalidOperationException("Cannot install " + type.FullName +
                                                    " because the given ProtoScenarioModule already contains a reference to an existing instance");

            psm.Load();
        }
    }
}

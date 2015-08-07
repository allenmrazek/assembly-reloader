using System;
using System.Linq;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class ModuleRefAlreadyExistsException : Exception
    {
        public ModuleRefAlreadyExistsException(IProtoScenarioModule psm)
            : base(
                "ProtoScenarioModule for " + psm.moduleName + " already contains a ScenarioModule reference of " +
                psm.moduleRef.Single().GetType().FullName)
        {
            
        }
    }
}

using System;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
// ReSharper disable once UnusedMember.Global
    [Implements(typeof(IScenarioModuleConfigNodeRepository))]
    public class ScenarioModuleConfigNodeRepository : DictionaryQueue<Type, ConfigNode>, IScenarioModuleConfigNodeRepository
    {

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.Replacements;
using AssemblyReloader.StrangeIoC.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class CommandInsertScenarioRunnerProxies : Command
    {
        private const string GetLoadedModulesName = "GetLoadedModules";

        public override void Execute()
        {
            //var targetMethod = typeof (ScenarioRunner).GetMethod(GetLoadedModulesName, BindingFlags.Static);
            //var replacementMethod = typeof(ScenarioRunnerProxyMethods).GetMethod(
        }
    }
}

﻿extern alias KSP;
using System.Collections.Generic;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IGameDatabase), InjectionBindingScope.CROSS_CONTEXT)]
    public class KspGameDatabase : IGameDatabase
    {
        public IEnumerable<KSP::UrlDir.UrlConfig> GetConfigs(string typeName)
        {
            return KSP::GameDatabase.Instance.GetConfigs(typeName);
        }
    }
}
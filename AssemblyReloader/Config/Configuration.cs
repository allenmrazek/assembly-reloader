using System;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
    public class Configuration
    {
        // ReSharper disable UnusedField.Compiler
        [ReeperPersistent] public Setting<bool> ReloadAllReloadablesUponWindowFocus = false;

        [ReeperPersistent] public Setting<bool> StartKSPAddonsForCurrentScene = false;


        //public void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        //{
        //    throw new NotImplementedException();
        //}

        //public void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

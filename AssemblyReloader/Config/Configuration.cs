using System;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
    public class Configuration : IReeperPersistent
    {
        // ReSharper disable UnusedField.Compiler
        [ReeperPersistent] public Setting<Boolean> ReloadAllReloadablesUponWindowFocus = false;



        public void Serialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(IConfigNodeSerializer formatter, ConfigNode node)
        {
            throw new NotImplementedException();
        }
    }
}

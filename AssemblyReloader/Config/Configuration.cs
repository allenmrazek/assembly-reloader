using System;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
    public class Configuration
    {
        // ReSharper disable UnusedField.Compiler
        [ReeperPersistent] public Setting<Boolean> ReloadAllReloadablesUponWindowFocus = false;
    }
}

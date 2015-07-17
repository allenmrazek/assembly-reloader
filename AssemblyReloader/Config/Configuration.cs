using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class Configuration
    {
        // ReSharper disable UnusedMember.Global
        // ReSharper disable InconsistentNaming

        [ReeperPersistent] public Setting<bool> ReloadAllReloadablesUponWindowFocus = false;
        [ReeperPersistent] public Setting<bool> StartKSPAddonsForCurrentScene = false;



        //private readonly IConfigurationPathProvider _configPathProvider;
        //private readonly IConfigNodeSerializer _configNodeSerializer;
        //private readonly ILog _log;


        //public Configuration(
        //    IConfigurationPathProvider configPathProvider, 
        //    IConfigNodeSerializer configNodeSerializer, 
        //    [Name(LogNames.Configuration)] ILog log)
        //{
        //    if (configPathProvider == null) throw new ArgumentNullException("configPathProvider");
        //    if (configNodeSerializer == null) throw new ArgumentNullException("configNodeSerializer");
        //    if (log == null) throw new ArgumentNullException("log");

        //    _configPathProvider = configPathProvider;
        //    _configNodeSerializer = configNodeSerializer;
        //    _log = log;
        //}


        //public void Save()
        //{
        //    var fullPath = _configPathProvider.Get();

        //    _log.Verbose("Saving Configuration to \"" + fullPath + "\"");

        //    var config = new ConfigNode("Configuration");
        //    _configNodeSerializer.Serialize(this, config);

        //    if (!config.HasData || !config.Save(fullPath, "AssemblyReloader Configuration"))
        //        _log.Error("Failed to serialize Configuration");
        //    else _log.Normal("Configuration saved.");
        //}


        //public void Load()
        //{
        //    throw new NotImplementedException();
        //}
    }
}

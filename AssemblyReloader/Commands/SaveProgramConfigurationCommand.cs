using System;
using System.Runtime.Serialization;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Queries.FileSystemQueries;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Commands
{
    public class SaveProgramConfigurationCommand : ICommand
    {
        private readonly Configuration _programConfiguration;
        private readonly IConfigNodeFormatter _formatter;
        private readonly IProgramConfigurationFilePathQuery _configPathQuery;
        private readonly ILog _log;

        public SaveProgramConfigurationCommand(
            [NotNull] Configuration programConfiguration,
            [NotNull] IConfigNodeFormatter formatter, 
            [NotNull] IProgramConfigurationFilePathQuery configPathQuery, 
            [NotNull] ILog log)
        {
            if (programConfiguration == null) throw new ArgumentNullException("programConfiguration");
            if (formatter == null) throw new ArgumentNullException("formatter");
            if (configPathQuery == null) throw new ArgumentNullException("configPathQuery");
            if (log == null) throw new ArgumentNullException("log");

            _programConfiguration = programConfiguration;
            _formatter = formatter;
            _configPathQuery = configPathQuery;
            _log = log;
        }

        public void Execute()
        {
            var cfg = new ConfigNode("AssemblyReloaderConfiguration");
            _formatter.Serialize(_programConfiguration, cfg);

            if (!cfg.HasData) throw new SerializationException("Failed to serialize program configuration");

            var configPath = _configPathQuery.Get();

            _log.Verbose("Saving program configuration settings to \"" + configPath + "\"");
            cfg.Save(configPath, "AssemblyReloader program configuration");
            _log.Verbose("Program configuration settings saved.");
        }
    }
}

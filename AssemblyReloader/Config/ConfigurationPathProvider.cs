using System;
using System.IO;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Config
{
    public class ConfigurationPathProvider : IConfigurationPathProvider
    {
        private readonly IFile _assemblyLocation;
        private readonly string _configExtension;

        public ConfigurationPathProvider(IFile assemblyLocation, string configExtension)
        {
            if (assemblyLocation == null) throw new ArgumentNullException("assemblyLocation");
            if (configExtension == null) throw new ArgumentNullException("configExtension");

            _assemblyLocation = assemblyLocation;
            _configExtension = configExtension;

            if (!_configExtension.StartsWith("."))
                _configExtension = "." + _configExtension;
        }


        public string Get()
        {
            return Path.Combine(Path.GetDirectoryName(_assemblyLocation.FullPath) 
                ?? string.Empty, Path.GetFileNameWithoutExtension(_assemblyLocation.FullPath) + _configExtension);
        }
    }
}

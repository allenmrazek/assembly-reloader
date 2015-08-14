extern alias KSP;
using System;
using System.IO;
using AssemblyReloader.ReloadablePlugin;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;
using strange.extensions.command.impl;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadConfiguration : Command
    {
        [Inject] public CoreConfiguration CoreConfiguration { get; set; }
        [Inject] public IConfigNodeSerializer Serializer { get; set; }
        [Inject] public IGetConfigurationFilePath Path { get; set; }
        [Inject] public IFile Core { get; set; }
        [Inject] public ILog Log { get; set; }


        public override void Execute()
        {
            var fullPath = Path.Get(Core);

            if (!File.Exists(fullPath))
            {
                Log.Warning("No configuration path found at \"" + fullPath + "\"; using default configuration settings");
                return;
            }
            
            var config = KSP::ConfigNode.Load(fullPath);
            if (config == null || !config.HasData)
            {
                Log.Error("Failed to load ConfigNode at " + fullPath);
                return;
            }

            try
            {
                Serializer.Deserialize(CoreConfiguration, config);
                Log.Normal("Loaded configuration successfully");
            }
            catch (Exception e)
            {
                Log.Error("Exception while deserializing configuration: " + e);
            }
        }
    }
}

using System;
using System.IO;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;
using File = System.IO.File;

namespace AssemblyReloader.Commands
{
    public class DeserializeObjectCommand : ICommand<SerializationContext>
    {
        public event Action<ConfigNode> OnDeserialized = delegate { };
 
        private readonly IConfigNodeSerializer _serializer;
        private readonly ILog _log;

        public DeserializeObjectCommand([NotNull] IConfigNodeSerializer serializer, [NotNull] ILog log)
        {
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (log == null) throw new ArgumentNullException("log");

            _serializer = serializer;
            _log = log;
        }


        public void Execute([NotNull] SerializationContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var configPath = context.PathProvider.Get();
            if (!File.Exists(configPath))
            {
                _log.Warning("Couldn't deserialize " + context.Target.GetType().FullName + "; " + configPath +
                             " not found");
                return;
            }

            var config = ConfigNode.Load(configPath);
            if (config == null) throw new FileLoadException("Failed to load config", configPath);

            _serializer.Deserialize(context.Target, config);

            OnDeserialized(config);
        }
    }
}

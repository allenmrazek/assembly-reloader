using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Commands
{
    public class SerializeObjectCommand : ICommand<SerializationContext>
    {
        public event Action<ConfigNode> OnSerialized = delegate { };

        private readonly IConfigNodeSerializer _serializer;
        private readonly ConfigNode _node;

 
        public SerializeObjectCommand(
            [NotNull] IConfigNodeSerializer serializer, [NotNull] ConfigNode node)
        {
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (node == null) throw new ArgumentNullException("node");

            _serializer = serializer;
            _node = node;
        }


        public void Execute(SerializationContext context)
        {
            _serializer.Serialize(context.Target, _node);

            OnSerialized(_node);
        }
    }
}

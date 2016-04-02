using System.Linq;
using System.Reflection;
using ReeperCommon.Serialization;
using System;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once UnusedMember.Global
    public class PluginConfigurationSurrogate : IConfigNodeItemSerializer<PluginConfiguration>
    {
        private static readonly PropertyInfo[] Properties = typeof (PluginConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        public void Serialize(Type type, ref object target, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (target == null) throw new ArgumentNullException("target");
            if (key == null) throw new ArgumentNullException("key");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!typeof (PluginConfiguration).IsAssignableFrom(type))
                throw new ArgumentException("This surrogate is for PluginConfiguration", "type");

            foreach (var prop in Properties)
            {
                var propertyToSerialize = prop;

                var itemSerializer = serializer.SerializerSelector.GetSerializer(prop.PropertyType);

                if (!itemSerializer.Any())
                    continue;

                var propertyValue = propertyToSerialize.GetValue(target, null);

                itemSerializer.Single().Serialize(propertyToSerialize.PropertyType, ref propertyValue, propertyToSerialize.Name, config, serializer);
            }
        }


        public void Deserialize(Type type, ref object target, string key, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (target == null) throw new ArgumentNullException("target");
            if (key == null) throw new ArgumentNullException("key");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!typeof(PluginConfiguration).IsAssignableFrom(type))
                throw new ArgumentException("This surrogate is for PluginConfiguration", "type");

            foreach (var prop in Properties)
            {
                var propertyToDeserialize = prop;
                var currentValue = propertyToDeserialize.GetValue(target, null);
                var itemSerializer = serializer.SerializerSelector.GetSerializer(prop.PropertyType);

                if (!itemSerializer.Any())
                    continue;

                itemSerializer
                    .Single()
                    .Deserialize(propertyToDeserialize.PropertyType, ref currentValue, propertyToDeserialize.Name,
                        config, serializer);

                propertyToDeserialize.SetValue(target, currentValue, null);
            }
        }
    }
}

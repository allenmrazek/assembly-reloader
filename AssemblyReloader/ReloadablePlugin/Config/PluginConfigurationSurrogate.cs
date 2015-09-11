extern alias KSP;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using UnityEngine;
using ConfigNode = KSP::ConfigNode;
using System;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class PluginConfigurationSurrogate : IConfigNodeItemSerializer<PluginConfiguration>
    {
        private static readonly PropertyInfo[] Properties = typeof (PluginConfiguration).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (target == null) throw new ArgumentNullException("target");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!typeof (PluginConfiguration).IsAssignableFrom(type))
                throw new ArgumentException("This surrogate is for PluginConfiguration", "type");

            foreach (var prop in Properties)
            {

                var propertyToSerialize = prop;

                serializer.ConfigNodeItemSerializerSelector
                    .GetSerializer(prop.PropertyType)
                    .Do(
                        s => s.Serialize(propertyToSerialize.PropertyType, propertyToSerialize.GetValue(target, null), propertyToSerialize.Name, config, serializer));
            }
        }

        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (target == null) throw new ArgumentNullException("target");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (!typeof(PluginConfiguration).IsAssignableFrom(type))
                throw new ArgumentException("This surrogate is for PluginConfiguration", "type");

            foreach (var prop in Properties)
            {
                var propertyToDeserialize = prop;
                var currentValue = propertyToDeserialize.GetValue(target, null);

                var result = serializer.ConfigNodeItemSerializerSelector
                    .GetSerializer(prop.PropertyType)
                    .SingleOrDefault()
                    .Return(
                        s => s.Deserialize(propertyToDeserialize.PropertyType, currentValue, propertyToDeserialize.Name, config, serializer),
                        currentValue);
             
                propertyToDeserialize.SetValue(target, result, null);
            }

            return target;
        }
    }
}

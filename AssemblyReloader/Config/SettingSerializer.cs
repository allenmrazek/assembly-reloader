using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Exceptions;

namespace AssemblyReloader.Config
{
    public class SettingSerializer<T> : IConfigNodeItemSerializer
    {
        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            CheckParameters(type, uniqueKey, config, serializer);

            var tSerializer = GetSerializerForT(serializer.ConfigNodeItemSerializerSelector);

            if (!tSerializer.Any())
                throw new Exception("Can't serialize Setting<" + type.FullName + "> because no serializer for " +
                                    typeof (T).FullName + " was found");

            var setting = target as Setting<T>;
                if (setting == null)
                    return; // no value to be serialized

            tSerializer.Single().Serialize(typeof(T), setting.Value, uniqueKey, config, serializer);
        }


        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
        {
            var tSerializer = GetSerializerForT(serializer.ConfigNodeItemSerializerSelector);

            if (!tSerializer.Any())
                throw new Exception("Can't serialize Setting<" + type.FullName + "> because no serializer for " +
                                    typeof(T).FullName + " was found");

            return tSerializer.Single().Deserialize(typeof(T), ((target as Setting<T>) ?? new Setting<T>()).Value, uniqueKey, config, serializer);
        }


        private Maybe<IConfigNodeItemSerializer> GetSerializerForT(IConfigNodeItemSerializerSelector selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");

            return selector.GetSerializer(typeof (T));
        }


        private static void CheckParameters(Type type, string uniqueKey, ConfigNode config,
            IConfigNodeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
            if (config == null) throw new ArgumentNullException("config");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (type != typeof(Setting<T>))
                throw new WrongSerializerException(type, typeof(Setting<T>)); 
        }
    }


    public static class SettingSerializerFactory
    {
        public static Maybe<IConfigNodeItemSerializer> Create(Type target)
        {
            return target.IsGenericType && !target.ContainsGenericParameters && target.GetGenericTypeDefinition() == typeof(Setting<>) && target.GetGenericArguments().Length == 1
                    ? Maybe<IConfigNodeItemSerializer>.With(
                        (IConfigNodeItemSerializer)Activator.CreateInstance(
                            typeof(SettingSerializer<>).MakeGenericType(new[] { target.GetGenericArguments()[0] }))) // SettingSerializer will already know we'll be giving it a Setting<T>. No need to add extra indirection
                    : Maybe<IConfigNodeItemSerializer>.None;
        }
    }
}

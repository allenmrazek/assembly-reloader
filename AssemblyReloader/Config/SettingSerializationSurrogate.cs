//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;
//using Microsoft.SqlServer.Server;
//using ReeperCommon.Containers;
//using ReeperCommon.Logging;
//using ReeperCommon.Serialization;
//using ReeperCommon.Serialization.Exceptions;
//using UnityEngine;

//namespace AssemblyReloader.Config
//{
//    // ReSharper disable once UnusedMember.Global
//    public class SettingSerializationSurrogate : ISurrogateSerializer<Setting<bool>>,
//                                                 ISurrogateSerializer<Setting<string>>
//    {
//        //public void Serialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
//        //{
//        //    var log = new DebugLog("SurrogateTest");
//        //    log.Warning("Serializing " + field.FieldType.FullName);

//        //    CheckGenericArguments(field.FieldType);

//        //    // In a Setting<T>, this is the T
//        //    var settingInnerType = field.FieldType.GetGenericArguments()[0];

//        //    var getMethod = typeof(ISurrogateSerializer<>).GetMethod("Get");

//        //    var getter = getMethod.MakeGenericMethod(settingInnerType);
//        //    var currentValue = getter.Invoke(field.GetValue(fieldOwner), null); // this is the value of Setting<T>.Get()

//        //    throw new NotImplementedException();


//        //    //var settingsInstance = field.GetValue(fieldOwner);
//        //    //if (settingsInstance == null) return;

//        //    //// note: remember field.FieldType is Settings<T>. What we really want to get is a converter
//        //    //// for T
//        //    //var settingsType = field.FieldType.GetGenericArguments()[0];
//        //    //log.Normal("SettingType = " + settingsType.FullName);

//        //    //// firstly, convert Settings<T> into value T. This is the value we'll be serializing
//        //    //var convertSettingsToT = TypeDescriptor.GetConverter(settingsType);
//        //    //if (!convertSettingsToT.CanConvertFrom(field.FieldType))
//        //    //    throw new Exception("Can't convert from " + field.FieldType.FullName + " to " + settingsType.FullName);

//        //    //var valueToWrite = convertSettingsToT.ConvertFrom(settingsInstance);

//        //    //// now convert from T to string
//        //    //var tc = TypeDescriptor.GetConverter(settingsType);

//        //    //config.AddValue(field.Name, tc.ConvertToString(valueToWrite));
//        //}


//        //public void Deserialize(object fieldOwner, FieldInfo field, ConfigNode config, IConfigNodeSerializer formatter)
//        //{
//        //    CheckGenericArguments(field.FieldType);

//        //    //if (!config.HasValue(field.Name)) return;

//        //    //var strValue = config.GetValue(field.Name);

//        //    //var tc = TypeDescriptor.GetConverter(field.FieldType);

//        //    //if (!tc.CanConvertFrom(typeof (string)))
//        //    //    throw new Exception("Can't convert from string to " + field.FieldType.Name);

//        //    //var result = tc.ConvertFromString(strValue);

//        //    //new DebugLog("Converter").Warning("result: " + result);

//        //    //field.SetValue(fieldOwner, result);

//        //    throw new NotImplementedException();
//        //}


//        //private void CheckGenericArguments(Type type)
//        //{
//        //    if (type == null) throw new ArgumentNullException("type");

//        //    foreach (var ga in type.GetGenericArguments())
//        //        new DebugLog("GenericArgument").Normal(ga.FullName);

//        //    if (type.GetGenericArguments().Length != 1)
//        //        throw new ArgumentException("Type " + type.FullName + " doesn't contain expected generic argument");
//        //}

//        private interface IProxyAccessor
//        {
//            object Value { get; }
//        }

//        public class ProxyAccessor<T> : IProxyAccessor
//        {
//            private readonly Setting<T> _setting;

//            public ProxyAccessor(Setting<T> setting)
//            {
//                _setting = setting;
//            }

//            public object Value
//            {
//                get { return _setting.Value; }
//                set { _setting.Value = (T) value; }
//            }
//        }


//        public void Serialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
//        {
//            if (type == null) throw new ArgumentNullException("type");
//            if (uniqueKey == null) throw new ArgumentNullException("uniqueKey");
//            if (config == null) throw new ArgumentNullException("config");
//            if (serializer == null) throw new ArgumentNullException("serializer");
//            // note: delay target null check so that other runtime exceptions can be detected as early as possible in cycle

//            var wrapped = GetSettingWrappedTypes(type).ToList();

//            if (!wrapped.Any())
//                throw new WrongSerializerException(type, typeof (Setting<>));

//            if (wrapped.Count > 1)
//                throw new Exception(type.FullName + " implements multiple ISetting<>: " +
//                                    string.Join(",", GetSettingWrappedTypes(type).Select(t => t.FullName).ToArray())
//                                    + "; type to be serialized is ambiguous");

//            // for a Setting<T>, get a serializer for T
//            var wrappedSerializer = serializer.ConfigNodeItemSerializerSelector.GetSerializer(wrapped.Single());

//            if (!wrappedSerializer.Any())
//                throw new NoSerializerFoundException(wrapped.Single());

//            if (target == null) return;

//            var accessor = Activator.CreateInstance(typeof (ProxyAccessor<>).MakeGenericType(wrapped.Single()), new [] { target}) as IProxyAccessor;
//            if (accessor == null) throw new Exception("Failed to cast ProxyAccessor instance to IProxyAccessor");

//            wrappedSerializer.Single().Serialize(wrapped.Single(), accessor.Value, uniqueKey, config, serializer);
//        }


//        public object Deserialize(Type type, object target, string uniqueKey, ConfigNode config, IConfigNodeSerializer serializer)
//        {
//            throw new NotImplementedException();
//        }


//        public static IEnumerable<Type> GetSettingWrappedTypes(Type type)
//        {
//            if (type == null) throw new ArgumentNullException("type");

//            return type.GetInterfaces()
//                .Where(i => i.IsGenericType)
//                .Where(i => i.GetGenericTypeDefinition() == typeof (Setting<>))
//                .Select(i => i.GetGenericArguments().First());
//        }
//    }
//}

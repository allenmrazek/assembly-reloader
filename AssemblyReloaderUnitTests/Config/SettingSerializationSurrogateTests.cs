//using System;
//using System.Linq;
//using System.Runtime.Serialization;
//using AssemblyReloader.Config;
//using AssemblyReloaderTests.Fixtures;
//using NSubstitute;
//using ReeperCommon.Containers;
//using ReeperCommon.Serialization;
//using UnityEngine;
//using Xunit;
//using Xunit.Extensions;

//namespace AssemblyReloaderTests.Config
//{
//    public class SettingSerializationSurrogateTests
//    {
//        [Theory, AutoDomainData]
//        public void SerializeTest(SettingSerializationSurrogate sut, string key, ConfigNode config)
//        {
//            const string strValue = "SomeSetting";

//            var configSerializer = Substitute.For<IConfigNodeSerializer>();
//            var selector = Substitute.For<IConfigNodeItemSerializerSelector>();
//            var typeSerializer = Substitute.For<IConfigNodeItemSerializer>();

//            var target = new Setting<string>(strValue);
            
//            configSerializer.ConfigNodeItemSerializerSelector.Returns(ci => selector);
//            selector.GetSerializer(
//                Arg.Is<Type>(ty => ty == typeof(string)))
//                    .Returns(ci => Maybe<IConfigNodeItemSerializer>.With(typeSerializer));

//            sut.Serialize(target.GetType(), target, key, config, configSerializer);

//            typeSerializer.DidNotReceive()
//                .Deserialize(Arg.Any<Type>(), Arg.Any<object>(), Arg.Any<string>(), Arg.Any<ConfigNode>(),
//                    Arg.Any<IConfigNodeSerializer>());

//            typeSerializer.Received(1)
//                .Serialize(Arg.Is(typeof (string)), Arg.Is<object>(o => ReferenceEquals(o, strValue)), Arg.Is(key),
//                    Arg.Is(config), Arg.Is(configSerializer));

//        }


//        [Fact()]
//        public void DeserializeTest()
//        {
//            throw new NotImplementedException();
//        }


//        [Theory, AutoDomainData]
//        public void GetSettingWrappedType_WithSingleType()
//        {
//            var settingInstance = Substitute.For<ISetting<bool>>();
//            var actual = SettingSerializationSurrogate.GetSettingWrappedTypes(settingInstance.GetType()).ToList();

//            Assert.NotEmpty(actual);
//            Assert.Contains(typeof(bool), actual);
//            Assert.True(actual.Count == 1);
//        }


//        [Theory, AutoDomainData]
//        public void GetSettingWrappedType_WithMultipleTypes()
//        {
//            var settingInstance = Substitute.For<ISetting<bool>, ISetting<float>, ISetting<string>>();
//            var actual = SettingSerializationSurrogate.GetSettingWrappedTypes(settingInstance.GetType()).ToList();

//            Assert.NotEmpty(actual);
//            Assert.Contains(typeof (bool), actual);
//            Assert.Contains(typeof (float), actual);
//            Assert.Contains(typeof (string), actual);
//            Assert.True(actual.Count == 3);
//        }


//        [Theory, AutoDomainData]
//        public void GetSettingWrappedType_WithTypeThatDoesntImplementISetting()
//        {
//            var notASetting = Substitute.For<IPersistenceSave>();
//            var actual = SettingSerializationSurrogate.GetSettingWrappedTypes(notASetting.GetType());

//            Assert.Empty(actual);
//        }
//    }
//}

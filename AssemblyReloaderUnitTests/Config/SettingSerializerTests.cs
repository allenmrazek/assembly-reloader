using System;
using System.Linq;
using AssemblyReloader.Config;
using AssemblyReloader.Properties;
using AssemblyReloaderTests.Fixtures;
using NSubstitute;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using Xunit;
using Xunit.Extensions;

namespace AssemblyReloaderTests.Config
{
    public abstract class SettingSerializerTests<T>
    {
        [Theory, AutoDomainData]
        public void Serialize_DelegatesSerializationTo_InnerType(string key, ConfigNode config)
        {
            var setting = new Setting<T>();

            var serializer = Substitute.For<IConfigNodeItemSerializer>();
            var selector = Substitute.For<IConfigNodeItemSerializerSelector>();

            selector.GetSerializer(Arg.Is(setting.GetType())).Returns(ci => Maybe<IConfigNodeItemSerializer>.With(serializer));
            selector.GetSerializer(Arg.Is(typeof (T)))
                .Returns(ci => Maybe<IConfigNodeItemSerializer>.With(Substitute.For<IConfigNodeItemSerializer>()));

            var configSerializer = Substitute.For<IConfigNodeSerializer>();
            configSerializer.ConfigNodeItemSerializerSelector.Returns(ci => selector);

            var sut = new SettingSerializer<T>();

            sut.Serialize(setting.GetType(), setting, key, config, configSerializer);

            selector.Received(1).GetSerializer(Arg.Is(typeof (T)));
        }


        [Theory, AutoDomainData]
        public void Deserialize_DelegatesSerialization_ToInnerType(string key, ConfigNode config)
        {
            var setting = new Setting<T>();

            var serializer = Substitute.For<IConfigNodeItemSerializer>();
            var selector = Substitute.For<IConfigNodeItemSerializerSelector>();

            selector.GetSerializer(Arg.Is(setting.GetType())).Returns(ci => Maybe<IConfigNodeItemSerializer>.With(serializer));
            selector.GetSerializer(Arg.Is(typeof(T)))
                .Returns(ci => Maybe<IConfigNodeItemSerializer>.With(Substitute.For<IConfigNodeItemSerializer>()));

            var configSerializer = Substitute.For<IConfigNodeSerializer>();
            configSerializer.ConfigNodeItemSerializerSelector.Returns(ci => selector);

            var sut = new SettingSerializer<T>();

            sut.Deserialize(setting.GetType(), setting, key, config, configSerializer);

            selector.Received(1).GetSerializer(Arg.Is(typeof(T)));
        }
    }


    public class FloatSettingTester : SettingSerializerTests<float>
    {
    }

    public class StringSettingTester : SettingSerializerTests<string>
    {
    }

    public class ConfigNodeSettingTester : SettingSerializerTests<ConfigNode>
    {
    
    }


    public class SettingSerializerFactoryTests
    {
        private class SomeGenericObject<T>
        {
            
        }


        [Fact]
        public void Create_OnNonGenericType_ReturnsNothing()
        {
            Assert.False(SettingSerializerFactory.Create(typeof (float)).Any());
            Assert.False(SettingSerializerFactory.Create(typeof (string)).Any());
            Assert.False(SettingSerializerFactory.Create(typeof (ConfigNode)).Any());
        }


        [Fact]
        public void Create_OnGenericType_ThatIsntSettingT_ReturnsNothing()
        {
            Assert.False(SettingSerializerFactory.Create(typeof (SomeGenericObject<float>)).Any());
            Assert.False(SettingSerializerFactory.Create(typeof(SomeGenericObject<string>)).Any());
            Assert.False(SettingSerializerFactory.Create(typeof(SomeGenericObject<ConfigNode>)).Any());
        }


        [Fact]
        public void Create_OnGenericType_ThatIsSetting_ButWithoutParamsSet_ReturnsNothing()
        {
            Assert.False(SettingSerializerFactory.Create(typeof(Setting<>)).Any());
        }


        [Fact]
        public void Create_OnGenericSetting_WithParamsSet_ReturnsCorrect()
        {
            Assert.True(SettingSerializerFactory.Create(typeof (Setting<float>)).Any());
            Assert.True(SettingSerializerFactory.Create(typeof(Setting<string>)).Any());
            Assert.True(SettingSerializerFactory.Create(typeof(Setting<ConfigNode>)).Any());
        }


        [Theory, AutoDomainData]
        public void Create_OnGenericSetting_WithParamsSet_ReturnsUsefulSerialize(string key, ConfigNode config)
        {
            var setting = new Setting<float>();
            var selector = Substitute.For<IConfigNodeItemSerializerSelector>();
            selector.GetSerializer(Arg.Is(typeof(float)))
                .Returns(ci => Maybe<IConfigNodeItemSerializer>.With(Substitute.For<IConfigNodeItemSerializer>()));

            var serializer = Substitute.For<IConfigNodeSerializer>();
            serializer.ConfigNodeItemSerializerSelector.Returns(ci => selector);

            var sut = SettingSerializerFactory.Create(typeof (Setting<float>));

            Assert.True(sut.Any());

            Assert.DoesNotThrow(() => sut.Single().Serialize(setting.GetType(), setting, key, config, serializer));
        }


        [Theory, AutoDomainData]
        public void Create_OnGenericSetting_WithParamsSet_ReturnsUsefulDeserialize(string key, ConfigNode config)
        {
            var setting = new Setting<float>();
            var selector = Substitute.For<IConfigNodeItemSerializerSelector>();
            selector.GetSerializer(Arg.Is(typeof(float)))
                .Returns(ci => Maybe<IConfigNodeItemSerializer>.With(Substitute.For<IConfigNodeItemSerializer>()));

            var serializer = Substitute.For<IConfigNodeSerializer>();
            serializer.ConfigNodeItemSerializerSelector.Returns(ci => selector);

            var sut = SettingSerializerFactory.Create(typeof(Setting<float>));

            Assert.True(sut.Any());

            Assert.DoesNotThrow(() => sut.Single().Deserialize(setting.GetType(), setting, key, config, serializer));
        }
    }
}

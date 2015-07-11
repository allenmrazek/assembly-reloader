//using System;
//using System.Linq;
//using AssemblyReloader.Config;
//using AssemblyReloaderTests.Fixtures;
//using NSubstitute;
//using ReeperCommon.Containers;
//using ReeperCommon.Serialization;
//using Xunit;
//using Xunit.Extensions;

//namespace AssemblyReloaderTests.Config
//{
//    public class SettingSerializerSelectorTests
//    {
//        [Fact]
//        public void SettingSerializerSelector_ThrowsOnNullParameter()
//        {
//            Assert.Throws<ArgumentNullException>(() => new SettingSerializerSelector(null));
//        }


//        [Theory, AutoDomainData]
//        public void GetSerializer_UsesProvidedSelector_ToGetSettingWrappedType(Setting<string> data)
//        {
//            var defaultSelector = Substitute.For<IConfigNodeItemSerializerSelector>();
//            var sut = new SettingSerializerSelector(defaultSelector);

//            sut.GetSerializer(data.GetType());

//            defaultSelector.Received(1).GetSerializer(Arg.Is<Type>(recvd => recvd == data.GetType()));
//        }


//        [Theory, AutoDomainData]
//        public void GetSerializer_ReturnsWrappedTypeSerializer_WhenGivenSettingType(Setting<string> data)
//        {
//            var defaultSelector = Substitute.For<IConfigNodeItemSerializerSelector>();
//            var stringSerializer = Substitute.For<IConfigNodeItemSerializer>();
//            defaultSelector.GetSerializer(Arg.Is(data.GetType()))
//                .Returns(ci => Maybe<IConfigNodeItemSerializer>.With(stringSerializer));

//            var sut = new SettingSerializerSelector(defaultSelector);

//            var actual = sut.GetSerializer(data.GetType());

//            Assert.NotEmpty(actual);
//            Assert.Same(stringSerializer, actual.Single());
//        }
//    }
//}

using System.Linq;
using System.Reflection;
using AssemblyReloaderTests.Fixtures;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception.Tests
{
    public class GetGameEventTypesTests
    {
        private class FieldOwner
        {
            public static EventVoid OnTestEvent = new EventVoid("TestEventNongeneric");
            public static EventData<bool> OnGenericOne = new EventData<bool>("TestEventOne");
            public static EventData<bool, int> OnGenericTwo = new EventData<bool, int>("TestEventTwo");
            public static EventData<bool, int, float> OnGenericThree = new EventData<bool, int, float>("TestEventThree"); 
        }


        [Theory, AutoDomainData]
        [InlineData(0), InlineData(1), InlineData(2), InlineData(3)]
        public void Get_Test_CallsItsDependency(int i)
        {
            var dependency = Substitute.For<IGetGameEventFields>();
            var sut = new GetGameEventTypes(dependency);

            var result = sut.Get(1).ToList();

            dependency.Get().Received(1);
        }


        [Theory, InlineData(0), InlineData(1), InlineData(2), InlineData(3)]
        public void Get_Test_ForEventVoidValues(int paramCount)
        {
            var dependency = Substitute.For<IGetGameEventFields>();
            var sut = new GetGameEventTypes(dependency);

            dependency.Get().Returns(ci => typeof (FieldOwner).GetFields(BindingFlags.Public | BindingFlags.Static));

            var result = sut.Get(paramCount).ToList();

            Assert.NotEmpty(result);
            Assert.True(result.Count == 1);
        }
    }
}

using System.Linq;
using AssemblyReloaderTests.Fixtures;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception.Tests
{
    public class GetGameEventTypesTests
    {
        [Theory, AutoDomainData]
        public void Get_Test_ForEventVoidValues(GetGameEventTypes sut)
        {
            var result = sut.Get(0).ToList();

            Assert.NotEmpty(result);
        }


        [Theory, AutoDomainData]
        public void Get_Test_ForEventDataValues_WithOneParameter(GetGameEventTypes sut)
        {
            var result = sut.Get(1).ToList();

            Assert.NotEmpty(result);
        }


        [Theory, AutoDomainData]
        public void Get_Test_ForEventDataValues_WithTwoParameters(GetGameEventTypes sut)
        {
            var result = sut.Get(2).ToList();

            Assert.NotEmpty(result);
        }


        [Theory, AutoDomainData]
        public void Get_Test_ForEventDataValues_WithThreeParameters(GetGameEventTypes sut)
        {
            var result = sut.Get(3).ToList();

            Assert.NotEmpty(result);
        }
    }
}

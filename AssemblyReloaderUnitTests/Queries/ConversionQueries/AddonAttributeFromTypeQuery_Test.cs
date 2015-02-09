using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries.ConversionQueries;
using Xunit;

namespace AssemblyReloaderUnitTests.Queries.ConversionQueries
{
    public class AddonAttributeFromTypeQuery_Test
    {
        [Fact]
        void Get_Returns_KSPAddonAttribute_Correctly()
        {
            var sut = new AddonAttributeFromTypeQuery();

            Assert.True(sut.Get(typeof (Fixture.TestAddon_Public)).Any());
        }


        [Fact]
        void Get_Returns_Null_WhenTypeHasNoKSPAddon()
        {
            var sut = new AddonAttributeFromTypeQuery();

            Assert.False(sut.Get(typeof (Fixture.MonoBehaviour_WithNoAddon)).Any());
        }


        [Fact]
        void Get_Returns_RightAttribute_WhenTypeHasMultipleDifferent()
        {
            var sut = new AddonAttributeFromTypeQuery();

            var result = sut.Get(typeof (Fixture.TestAddon_MultipleAttributes));

            Assert.True(result.Any());
            Assert.True(typeof (KSPAddon).BaseType == result.Single().GetType().BaseType);
        }
    }
}

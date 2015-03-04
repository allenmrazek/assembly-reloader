using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.ConversionQueries;
using AssemblyReloaderUnitTests.FixtureCustomizations;
using AssemblyReloaderUnitTests.TestData;
using AssemblyReloaderUnitTests.TestData.Addons;
using Ploeh.AutoFixture;
using UnityEngine;
using Xunit;

namespace AssemblyReloaderUnitTests.Queries.AssemblyQueries
{


    public class AddonsFromAssemblyQuery_Test
    {
        [Fact]
        void Get_Returns_TestAddons()
        {
            var fixture = new Fixture();
            fixture.Customize(new AssemblyIsLocalAssemblyCustomization());

            var sut = new AddonsFromAssemblyQuery(new AddonAttributesFromTypeQuery());

            var results = sut.Get(fixture.CreateAnonymous<Assembly>()).ToList();

            Assert.NotEmpty(results);
            Assert.Contains(typeof (TestAddon_Public), results);
            Assert.Contains(typeof (TestAddon_Private), results);
            Assert.Contains(typeof (TestAddon_MultipleAttributes), results);

        }

        [Fact]
        void Get_DoesNotReturn_InvalidEntries()
        {
            var fixture = new Fixture();
            fixture.Customize(new AssemblyIsLocalAssemblyCustomization());

            var sut = new AddonsFromAssemblyQuery(new AddonAttributesFromTypeQuery());

            var results = sut.Get(fixture.CreateAnonymous<Assembly>()).ToList();

            Assert.NotEmpty(results);
            Assert.DoesNotContain(typeof(TestAddon_InvalidResultPartModule), results);
            Assert.DoesNotContain(typeof(TestAddon_InvalidResult), results);
            Assert.DoesNotContain(typeof(MonoBehaviour_WithNoAddon), results);
        }
    }
}

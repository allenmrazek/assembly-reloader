using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloaderUnitTests.FixtureCustomizations;
using AssemblyReloaderUnitTests.TestData;
using AssemblyReloaderUnitTests.TestData.PartModules;
using Ploeh.AutoFixture;
using Xunit;

namespace AssemblyReloaderUnitTests.Queries.AssemblyQueries
{
    public class PartModulesFromAssemblyQuery_Test
    {
        [Fact]
        void Get_Returns_TestPartModuleAndDerived()
        {
            var fixture = new Fixture();
            fixture.Customize(new AssemblyIsLocalAssemblyCustomization());

            var sut = new PartModulesFromAssemblyQuery();

            var results = sut.Get(fixture.CreateAnonymous<Assembly>()).ToList();

            Assert.NotEmpty(results);
            Assert.Contains(typeof (TestPartModule), results);
            Assert.Contains(typeof (DerivedPartModule), results);
            Assert.True(results.All(ty => typeof (PartModule).IsAssignableFrom(ty)));
        }


        [Fact]
        void Get_Returns_PrivateAndInner_PartModules()
        {
            var fixture = new Fixture();
            fixture.Customize(new AssemblyIsLocalAssemblyCustomization());
            var sut = new PartModulesFromAssemblyQuery();

            var results = sut.Get(fixture.CreateAnonymous<Assembly>()).ToList();

            Assert.NotEmpty(results);
            Assert.Contains(typeof(PartModuleContainerClass.InnerPartModule), results);
            Assert.Contains(typeof(PartModuleContainerClass.DerivedInnerPartModule), results);
            Assert.True(results.All(ty => typeof(PartModule).IsAssignableFrom(ty)));
            Assert.True(results.Any(ty => ty.Name == "HiddenInternalPartModule"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloaderUnitTests.Fixture;
using Xunit;

namespace AssemblyReloaderUnitTests.Queries.AssemblyQueries
{
    public class PartModulesFromAssemblyQuery_Test
    {
        [Fact]
        void Get_Returns_TestPartModuleAndDerived()
        {
            var sut = new PartModulesFromAssemblyQuery();

            var results = sut.Get(Assembly.GetExecutingAssembly());

            Assert.NotEmpty(results);
            Assert.Contains(typeof (TestPartModule), results);
            Assert.Contains(typeof (DerivedPartModule), results);
            Assert.True(results.All(ty => typeof (PartModule).IsAssignableFrom(ty)));
        }


        [Fact]
        void Get_Returns_PrivateAndInner_PartModules()
        {
            var sut = new PartModulesFromAssemblyQuery();

            var results = sut.Get(Assembly.GetExecutingAssembly());

            Assert.NotEmpty(results);
            Assert.Contains(typeof(PartModuleContainerClass.InnerPartModule), results);
            Assert.Contains(typeof(PartModuleContainerClass.DerivedInnerPartModule), results);
            Assert.True(results.All(ty => typeof(PartModule).IsAssignableFrom(ty)));
            Assert.True(results.Any(ty => ty.Name == "HiddenInternalPartModule"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloaderUnitTests.Fixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace AssemblyReloaderUnitTests.Queries.CecilQueries
{
    public class PartModuleDefinitionsQuery_Test
    {
        [Theory, AutoData]
        void Get_Returns_PartModules(PartModuleDefinitionsQuery sut)
        {
            var assembly = new Fixture_TestAssembly();
            var result = sut.Get(assembly.Definition).Select(def => def.FullName).ToList();

            Assert.NotEmpty(result);
            Assert.True(result.Contains("TestProject.TestPartModule"));
            Assert.True(result.Contains("TestProject.DerivativePartModule"));
            Assert.True(result.Contains("TestProject.InternalPartModule"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloaderUnitTests.FixtureCustomizations;
using Mono.Cecil;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace AssemblyReloaderUnitTests.Queries.CecilQueries
{
    public class PartModuleMethodDefinitionQuery_Test
    {
        [Fact]
        void GetOnLoad_Returns_OnLoad_Correctly()
        {
            var fixture = new Fixture();
            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

            var sut =
                new PartModuleMethodDefinitionQuery(fixture.CreateAnonymous<TypeDefinition>());

            var results = sut.GetOnLoad().ToList();

            Assert.True(results.Any());
        }


        [Fact]
        void GetOnSave_Returns_OnSave_Correctly()
        {
            var fixture = new Fixture();
            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

            var sut =
                new PartModuleMethodDefinitionQuery(fixture.CreateAnonymous<TypeDefinition>());

            var results = sut.GetOnLoad().ToList();

            Assert.True(results.Any());
        }


        [Fact]
        void GetOnLoad_Returns_Nothing_OnCorrectlyNamedMethods_But_WrongParametersOrNotVirtual()
        {
            var fixture = new Fixture();
            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

            var sut = new PartModuleMethodDefinitionQuery(fixture.CreateAnonymous<TypeDefinition>());


            var results = sut.GetOnLoad();

            Assert.True(results.Any());
            Assert.True(results.Single().FullName ==
                        "System.Void TestProject.TestData.PartModuleTest_WithVariousOnLoadOnSave::OnLoad(ConfigNode)");
        }

        [Fact]
        void GetOnSave_Returns_Nothing_OnCorrectlyNamedMethods_But_WrongParametersOrNotVirtual()
        {
            var fixture = new Fixture();
            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

            var sut = new PartModuleMethodDefinitionQuery(fixture.CreateAnonymous<TypeDefinition>());


            var results = sut.GetOnSave();

            Assert.True(results.Any());
            Assert.True(results.Single().FullName ==
                        "System.Void TestProject.TestData.PartModuleTest_WithVariousOnLoadOnSave::OnSave(ConfigNode)");
        }
    }
}

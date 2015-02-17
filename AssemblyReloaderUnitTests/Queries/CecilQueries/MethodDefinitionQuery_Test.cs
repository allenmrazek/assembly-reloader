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
    public class MethodDefinitionQuery_Test
    {
        [Theory]
        [InlineAutoData("OnLoad")]
        void Get_Returns_OnLoad_Correctly(string methodName, MethodDefinitionQuery sut, PartModuleDefinitionsQuery pmQuery)
        {
            var fixture = new Fixture();
            fixture.Customize(new UnmodifiedTestProjectTypeDefinitionCustomization());

            var results = sut.Get(fixture.CreateAnonymous<TypeDefinition>(), methodName).ToList();

            Assert.NotEmpty(results);
            Assert.True(results.Count == 1);
            
        }


        [Theory, AutoData]
        void Get_Returns_Nothing_WithBadNamed(string methodName, MethodDefinitionQuery sut)
        {
            var fixture = new Fixture();
            fixture.Customize(new UnmodifiedTestProjectTypeDefinitionCustomization());

            var results = sut.Get(fixture.CreateAnonymous<TypeDefinition>(), methodName).ToList();

            Assert.Empty(results);
        }


        [Theory]
        [InlineAutoData("OnLoad")]
        void Get_Returns_Nothing_OnCorrectlyNamedMethods_But_WrongParametersOrNotVirtual(string methodName, MethodDefinitionQuery sut)
        {
            var fixture = new Fixture();
            fixture.Customize(new UnmodifiedTestProjectTypeDefinitionCustomization());

            var query = new PartModuleDefinitionsQuery();

            var pmDefinitions = query.Get(fixture.CreateAnonymous<AssemblyDefinition>())
                .Where(def => def.FullName == "TestProject.TestData.PartModuleTest_SuppressedOnLoad" ||
                              def.FullName == "TestProject.TestData.PartModuleTest_WithVariousOnLoad")
                .ToList();

            Assert.NotEmpty(pmDefinitions);
            Assert.True(pmDefinitions.Count == 2);

            var results = pmDefinitions.SelectMany(pmDef => sut.Get(pmDef, methodName)).ToList();

            Assert.True(results.Count == 1); // only WithVariousOnLoad, and one correct example
            Assert.True(results.Single().FullName == "System.Void TestProject.TestData.PartModuleTest_WithVariousOnLoad::OnLoad(ConfigNode)");
        }
    }
}

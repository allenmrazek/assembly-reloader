//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using AssemblyReloader.Queries.CecilQueries;
//using AssemblyReloaderUnitTests.FixtureCustomizations;
//using Mono.Cecil;
//using Ploeh.AutoFixture;
//using Ploeh.AutoFixture.Xunit;
//using Xunit;
//using Xunit.Extensions;

//namespace AssemblyReloaderUnitTests.Queries.CecilQueries
//{
//    public class PartModuleDefinitionsQuery_Test
//    {
//        [Theory, AutoData]
//        void Get_Returns_PartModules(PartModuleDefinitionsQuery sut)
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssemblyCustomization());

//            var result = sut.Get(fixture.CreateAnonymous<AssemblyDefinition>()).Select(def => def.FullName).ToList();

//            Assert.NotEmpty(result);
//            Assert.True(result.Contains("TestProject.TestData.PartModuleUnitTestVersion"));
//            Assert.True(result.Contains("TestProject.TestData.DerivativePartModule"));
//            Assert.True(result.Contains("TestProject.TestData.InternalPartModule"));
//            Assert.True(result.Contains("TestProject.TestData.PartModuleTest_SuppressedOnLoad"));
//            Assert.True(result.Contains("TestProject.TestData.PartModuleTest_WithVariousOnLoadOnSave"));
//        }
//    }
//}

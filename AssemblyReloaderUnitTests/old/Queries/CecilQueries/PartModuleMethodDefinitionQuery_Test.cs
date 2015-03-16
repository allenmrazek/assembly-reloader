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
//    public class PartModuleMethodDefinitionQuery_Test
//    {
//        [Fact]
//        void GetOnLoadDefinition_Returns_OnLoad_Correctly()
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

//            var sut = new PartModuleMethodDefinitionQuery();
            
//            var results = sut.GetOnLoadDefinition((fixture.CreateAnonymous<TypeDefinition>())).ToList();

//            Assert.True(results.Any());
//        }


//        [Fact]
//        void GetOnSaveDefinition_Returns_OnSave_Correctly()
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

//            var sut = new PartModuleMethodDefinitionQuery();

//            var results = sut.GetOnLoadDefinition(fixture.CreateAnonymous<TypeDefinition>()).ToList();

//            Assert.True(results.Any());
//        }


//        [Fact]
//        void GetOnLoadDefinition_Returns_Nothing_OnCorrectlyNamedMethods_But_WrongParametersOrNotVirtual()
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

//            var sut = new PartModuleMethodDefinitionQuery();


//            var results = sut.GetOnLoadDefinition(fixture.CreateAnonymous<TypeDefinition>());

//            Assert.True(results.Any());
//            Assert.True(results.Single().FullName ==
//                        "System.Void TestProject.TestData.PartModuleTest_WithVariousOnLoadOnSave::OnLoad(ConfigNode)");
//        }

//        [Fact]
//        void GetOnSaveDefinition_Returns_Nothing_OnCorrectlyNamedMethods_But_WrongParametersOrNotVirtual()
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

//            var sut = new PartModuleMethodDefinitionQuery();


//            var results = sut.GetOnSaveDefinition(fixture.CreateAnonymous<TypeDefinition>());

//            Assert.True(results.Any());
//            Assert.True(results.Single().FullName ==
//                        "System.Void TestProject.TestData.PartModuleTest_WithVariousOnLoadOnSave::OnSave(ConfigNode)");
//        }


//        [Fact]
//        void GetOnLoadMethod_Returns_Correct()
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

//            var sut = new PartModuleMethodDefinitionQuery();

//            var results = sut.GetOnLoadMethod(typeof (TestData.PartModules.TestPartModule));

//            Assert.True(results.Any());
//        }


//        [Fact]
//        void GetOnLoadMethod_Returns_Nothing_WhenPartModuleHasNoDefinedOnLoad()
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

//            var sut = new PartModuleMethodDefinitionQuery();

//            var results = sut.GetOnLoadMethod(typeof(TestData.PartModules.PartModuleContainerClass.InnerPartModule));

//            Assert.False(results.Any());
//        }


//        [Fact]
//        void GetOnSaveMethod_Returns_Correct()
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

//            var sut = new PartModuleMethodDefinitionQuery();

//            var results = sut.GetOnSaveMethod(typeof (TestData.PartModules.TestPartModule));

//            Assert.True(results.Any());
//        }


//        [Fact]
//        void GetOnSaveMethod_Returns_Nothing_WhenPartModuleHasNoDefinedOnLoad()
//        {
//            var fixture = new Fixture();
//            fixture.Customize(new TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization());

//            var sut = new PartModuleMethodDefinitionQuery();

//            var results = sut.GetOnSaveMethod(typeof(TestData.PartModules.PartModuleContainerClass.InnerPartModule));

//            Assert.False(results.Any());
//        }
//    }
//}

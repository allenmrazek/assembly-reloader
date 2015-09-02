extern alias Cecil96;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Weaving;
using AssemblyReloaderTests.Fixtures;
using WeavingTestData.NestedTypeData;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloaderTests.ReloadablePlugin.Weaving
{
    public class GetAllTypesInAssemblyDefinitionTests
    {
        [Theory, AutoDomainData]
        public void Get_WithSimpleNested_Test(GetAllTypesInAssemblyDefinition sut, Cecil96::Mono.Cecil.AssemblyDefinition definition)
        {
            var simple =
                definition.MainModule.Types.Single(td => td.FullName == "WeavingTestData.NestedTypeData.SimpleNested").NestedTypes.First();

            var results = sut.Get(definition);

            Assert.Contains(simple, results);
        }

        [Theory, AutoDomainData]
        public void Get_WithComplexNested_Test(GetAllTypesInAssemblyDefinition sut, Cecil96::Mono.Cecil.AssemblyDefinition definition)
        {
            var complexBase =
                definition.MainModule.Types.Single(td => td.FullName == "WeavingTestData.NestedTypeData.ComplexNested");
            var levelThree = definition.MainModule.Import(typeof (ComplexNested.InnerOne.InnerTwo.InnerThree)).Resolve();
            var levelOne = definition.MainModule.Import(typeof (ComplexNested.InnerOne)).Resolve();
            var levelTwo = definition.MainModule.Import(typeof (ComplexNested.InnerOne.InnerTwo)).Resolve();
            
            var results = sut.Get(definition).Select(td => td.Resolve().FullName).ToList();

            var complex = definition.MainModule.Import(typeof (ComplexNested)).Resolve();

            Assert.Equal(complexBase.FullName, complex.FullName);

            Assert.Contains(complexBase.FullName, results);
            //Assert.True(results.Any(td => td == levelOne));
            Assert.Contains(levelOne.FullName, results);
            Assert.Contains(levelTwo.FullName, results);
            Assert.Contains(levelThree.FullName, results);
        }
    }
}

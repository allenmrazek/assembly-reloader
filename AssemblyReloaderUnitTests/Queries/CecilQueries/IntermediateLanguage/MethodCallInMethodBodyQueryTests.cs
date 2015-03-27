using System.Linq;
using System.Reflection;
using AssemblyReloaderTests.FixtureCustomizations;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.Queries.CecilQueries.IntermediateLanguage.Tests
{
    public class MethodCallInMethodBodyQueryTests
    {
        [Theory, AutoDomainData]
        public void GetTest(AssemblyDefinition definition)
        {
            //Fixture.Register(
            //    () =>
            //        assemblyDefinition.MainModule.Types.Single(td => td.FullName == "TestData.PartModules.TestPartModule").Methods.Single(
            //            md => md.Name == "OnSave"));

            //var types = definition.MainModule.Types;

            //var target =
            //    types.SingleOrDefault(td => td.FullName == "AssemblyReloaderTests.TestData.PartModules.TestPartModule");

            //var targetMethod = target.Methods.SingleOrDefault(md => md.Name == "OnSave");

            var sut =
                new MethodCallInMethodBodyQuery(
                    typeof (PartModule).GetMethod("OnSave", BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public),
                    OpCodes.Call);

            var targetMethod = definition.MainModule.Types.Single(td => td.FullName == "AssemblyReloaderTests.TestData.PartModules.TestPartModule").Methods.Single(
                        md => md.Name == "OnSave");

            var results = sut.Get(targetMethod);

            Assert.NotNull(targetMethod);
            Assert.NotEmpty(results);
        }
    }
}

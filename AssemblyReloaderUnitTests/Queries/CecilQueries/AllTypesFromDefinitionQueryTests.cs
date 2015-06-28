using AssemblyReloader.ReloadablePlugin.Definition;
using AssemblyReloaderTests.FixtureCustomizations;
using Mono.Cecil;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.Queries.CecilQueries.Tests
{
    public class AllTypesFromDefinitionQueryTests
    {
        [Theory, AutoDomainData]
        public void GetTest([Frozen] AssemblyDefinition definition, GetAllTypesFromDefinition sut)
        {
            var results = sut.Get(definition);

            Assert.NotEmpty(results);
        }
    }
}

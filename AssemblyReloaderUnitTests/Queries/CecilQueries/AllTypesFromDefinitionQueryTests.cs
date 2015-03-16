using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloaderUnitTests.FixtureCustomizations;
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
        public void GetTest([Frozen] AssemblyDefinition definition, AllTypesFromDefinitionQuery sut)
        {
            var results = sut.Get(definition);

            Assert.NotEmpty(results);
        }
    }
}

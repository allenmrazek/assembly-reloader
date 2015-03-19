using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloaderTests.FixtureCustomizations;
using Mono.Cecil;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.Queries.CecilQueries.Tests
{
    public class AllMethodsFromDefinitionQueryTests
    {
        [Theory, AutoDomainData]
        public void GetTest(TypeDefinition typeWithAtLeastOneMethod)
        {
            var sut = new AllMethodsFromDefinitionQuery();

            var results = sut.Get(typeWithAtLeastOneMethod);


            Assert.True(typeWithAtLeastOneMethod.Methods.Count > 0);
            Assert.NotEmpty(results);
        }
    }
}

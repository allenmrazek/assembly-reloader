using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.ConversionQueries;
using AssemblyReloaderTests.FixtureCustomizations;
using AssemblyReloaderTests.TestData.Addons;
using Mono.Cecil;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.Queries.AssemblyQueries.Tests
{
    public class AddonsFromAssemblyQueryTests
    {
        [Theory, AutoDomainData]
        public void GetTest(Assembly assembly)
        {
            var sut = new AddonsFromAssemblyQuery(new AddonAttributesFromTypeQuery());

            var results = sut.Get(assembly).ToList();

            Assert.NotEmpty(results);
            Assert.Contains(typeof (TestAddon_Private), results);
            Assert.True(results.All(result => result.GetCustomAttributes(true).Any(attr => attr is KSPAddon)));
        }
    }
}

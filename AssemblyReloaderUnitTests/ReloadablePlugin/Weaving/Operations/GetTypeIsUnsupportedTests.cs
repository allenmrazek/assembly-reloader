using System;
using System.Linq;
using AssemblyReloaderTests.Fixtures;
using Mono.Cecil;
using WeavingTestData.UnsupportedTypes.Contracts;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.Tests
{
    public class GetTypeIsUnsupportedTests
    {
        private static TypeDefinition GetTestDefinition(AssemblyDefinition definition, string name)
        {
            const string targetNamespace = "WeavingTestData.UnsupportedTypes";

            return definition.Modules.SelectMany(m => m.Types)
                .Where(t => t.Namespace.StartsWith(targetNamespace))
                .SelectMany(m => new[] {m}
                    .Union(m.NestedTypes))
                .SingleOrDefault(td => td.FullName.EndsWith(name) || (
                    td.Namespace == "" && td.BaseType != null && td.FullName.EndsWith(name)));
        }

        [Theory]
        [AutoDomainDataWithInlineData("Contracts.TestContract")]
        [AutoDomainDataWithInlineData("Contracts.TestContractFromInterface")]
        [AutoDomainDataWithInlineData("Contracts.TestContractNested/InnerContract")]
        public void Get_Test(string testContractName, GetTypeIsUnsupported sut, AssemblyDefinition definition)
        {
            var testContractDef = GetTestDefinition(definition, testContractName);
            Assert.NotNull(testContractDef);

            var result = sut.Get(testContractDef);

            Assert.True(result);
        }


        [Theory]
        [AutoDomainDataWithInlineData("Contracts.TestContract")]
        [AutoDomainDataWithInlineData("Contracts.TestContractFromInterface")]
        [AutoDomainDataWithInlineData("Contracts.TestContractNested/InnerContract")]
        public void Get_ReturnsAllBadTypeDefinitions(string typeName, GetTypeIsUnsupported sut, AssemblyDefinition definition)
        {
            var searchedType = GetTestDefinition(definition, typeName);
            var result = definition.Modules.SelectMany(m => m.Types).SelectMany(td => td.NestedTypes.Union(new [] { td})).Where(sut.Get);

            Assert.NotNull(searchedType);
            Assert.Contains(searchedType, result);
        }



        [Theory, AutoDomainDataWithInlineData("Contracts.NotAContract")]
        public void Get_ReturnsFalseOnSupportedTypes(string typeName, GetTypeIsUnsupported sut,
            AssemblyDefinition definition)
        {
            var validType = GetTestDefinition(definition, typeName);
            Assert.NotNull(validType);

            var result = sut.Get(validType);

            Assert.False(result);
        }
    }
}

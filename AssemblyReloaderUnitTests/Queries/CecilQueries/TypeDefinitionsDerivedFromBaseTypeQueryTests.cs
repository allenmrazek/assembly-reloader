using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloaderUnitTests.FixtureCustomizations;
using Mono.Cecil;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using UnityEngine;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.Queries.CecilQueries.Tests
{
    public class TypeDefinitionsDerivedFromBaseTypeQueryTests<T> where T : MonoBehaviour
    {
        [Theory, AutoDomainData]
        public virtual void GetTest([Frozen] AssemblyDefinition definition)
        {
            var sut = new TypeDefinitionsDerivedFromBaseTypeQuery<T>(new AllTypesFromDefinitionQuery());

            var results = sut.Get(definition);

            Assert.NotEmpty(results);
        }
    }


    public class TypeDefinitionsDerivedFromMonoBehaviourTest : TypeDefinitionsDerivedFromBaseTypeQueryTests<MonoBehaviour>
    {
        
    }

    public class TypeDefinitionsDerivedFromPartModuleTest : TypeDefinitionsDerivedFromBaseTypeQueryTests<PartModule>
    {

    }

    public class TypeDefinitionsDerivedFromScenarioModuleTest : TypeDefinitionsDerivedFromBaseTypeQueryTests<ScenarioModule>
    {

    }
}

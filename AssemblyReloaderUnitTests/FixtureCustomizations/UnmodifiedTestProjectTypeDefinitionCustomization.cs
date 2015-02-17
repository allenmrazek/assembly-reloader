using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using Ploeh.AutoFixture;

namespace AssemblyReloaderUnitTests.FixtureCustomizations
{
    public class UnmodifiedTestProjectTypeDefinitionCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new UnmodifiedTestProjectAssemblyDefinitionCustomization());

            fixture.Register(
                () =>
                    new PartModuleDefinitionsQuery().Get(fixture.CreateAnonymous<AssemblyDefinition>())
                        .Single(def => def.FullName == "TestProject.TestData.PartModuleUnitTestVersion"));
        }
    }
}

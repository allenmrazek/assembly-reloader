using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using Ploeh.AutoFixture;

namespace AssemblyReloaderUnitTests.FixtureCustomizations
{
    class TestProjectAssembly_WithVariousOnLoadOnSavePartModuleCustomization : TestProjectAssemblyCustomization
    {
        public override void Customize(IFixture fixture)
        {
            base.Customize(fixture);


            fixture.Register(
                () =>
                        new PartModuleDefinitionsQuery().Get(fixture.CreateAnonymous<AssemblyDefinition>())
                    .Single(def => def.FullName == "TestProject.TestData.PartModuleTest_WithVariousOnLoadOnSave"));

        }
    }
}

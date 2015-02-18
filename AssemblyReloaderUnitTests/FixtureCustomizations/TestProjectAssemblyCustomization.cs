using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using Ploeh.AutoFixture;

namespace AssemblyReloaderUnitTests.FixtureCustomizations
{
    public class TestProjectAssemblyCustomization : ICustomization
    {
        public virtual void Customize(IFixture fixture)
        {
            var filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/TestProject.dll";

                if (!File.Exists(filename))
                    throw new FileNotFoundException(filename);

            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filename);

            fixture.Inject(assemblyDefinition);

            // TypeDefinition (PartModuleUnitTestVersion)
            fixture.Register(
                () =>
                    new PartModuleDefinitionsQuery().Get(fixture.CreateAnonymous<AssemblyDefinition>())
                        .Single(def => def.FullName == "TestProject.TestData.PartModuleUnitTestVersion"));
        }
    }
}
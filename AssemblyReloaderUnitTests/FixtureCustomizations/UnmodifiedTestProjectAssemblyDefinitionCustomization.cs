using System.IO;
using System.Reflection;
using Mono.Cecil;
using Ploeh.AutoFixture;

namespace AssemblyReloaderUnitTests.FixtureCustomizations
{
    public class UnmodifiedTestProjectAssemblyDefinitionCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/TestProject.dll";

                if (!System.IO.File.Exists(filename))
                    throw new FileNotFoundException(filename);

            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filename);

            fixture.Inject(assemblyDefinition);
        }
    }
}
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace AssemblyReloaderUnitTests.FixtureCustomizations
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(new Fixture().Customize(new DomainCustomization()))
        {
            var filename = Assembly.GetExecutingAssembly().Location;

            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filename);

            Fixture.Register(() => assemblyDefinition);


            Fixture.Register(() => Assembly.GetExecutingAssembly());
        }
    }
}
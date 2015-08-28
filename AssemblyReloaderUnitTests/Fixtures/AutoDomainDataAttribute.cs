using System.IO;
using Mono.Cecil;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace AssemblyReloaderTests.Fixtures
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        protected static readonly DefaultAssemblyResolver Resolver = new DefaultAssemblyResolver();

        public AutoDomainDataAttribute()
            : base(new Fixture().Customize(new CompositeCustomization(
                new DomainCustomization(),
                new AutoNSubstituteCustomization())))
        {
            const string filename = "WeavingTestData.dll";

            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            
            Fixture.Register(() => Resolver);
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filename, new ReaderParameters { AssemblyResolver = Resolver });

            Fixture.Register(() => assemblyDefinition);
        }
    }
}
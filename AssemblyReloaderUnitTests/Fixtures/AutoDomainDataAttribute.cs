extern alias Cecil96;
using System.IO;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;
using DefaultAssemblyResolver = Cecil96::Mono.Cecil.DefaultAssemblyResolver;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using ReaderParameters = Cecil96::Mono.Cecil.ReaderParameters;

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
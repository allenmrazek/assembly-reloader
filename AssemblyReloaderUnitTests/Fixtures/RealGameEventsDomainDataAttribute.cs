extern alias Cecil96;
using System.IO;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;
using Ploeh.AutoFixture;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using ReaderParameters = Cecil96::Mono.Cecil.ReaderParameters;

namespace AssemblyReloaderTests.Fixtures
{
    class RealGameEventsDomainDataAttribute: AutoDomainDataAttribute
    {
        public RealGameEventsDomainDataAttribute()
            : base()
        {
            const string filename = "WeavingTestData.dll";

            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            // changes will be made so we must provide a fresh one for each test
            var assembly = AssemblyDefinition.ReadAssembly(filename, new ReaderParameters {AssemblyResolver = Resolver});
            Fixture.Register(() => assembly);
            //Fixture.Register(() => AssemblyDefinition.ReadAssembly(filename, new ReaderParameters { AssemblyResolver = Resolver }));

            Fixture.Register(() => new GetGameEventTypes(new GetGameEventFields()));
        }
    }
}

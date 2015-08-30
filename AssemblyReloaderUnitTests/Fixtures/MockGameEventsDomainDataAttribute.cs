extern alias Cecil96;
using System.Linq;
using Ploeh.AutoFixture;
using WeavingTestData.GameEventsMock;
using Cecil96::Mono.Cecil;

namespace AssemblyReloaderTests.Fixtures
{
    class MockGameEventsDomainDataAttribute : AutoDomainDataAttribute
    {
        public MockGameEventsDomainDataAttribute() : base()
        {
            var definition = Fixture.CreateAnonymous<AssemblyDefinition>();

            // TypeDefinition
            Fixture.Register(
                () => definition.MainModule.Types.Single(t => t.FullName == typeof (MockedGameEvents).FullName));

        }
    }
}

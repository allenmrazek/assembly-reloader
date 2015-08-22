using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Ploeh.AutoFixture;
using WeavingTestData.GameEventsMock;

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

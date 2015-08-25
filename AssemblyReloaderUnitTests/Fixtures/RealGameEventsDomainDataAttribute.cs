using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;
using Mono.Cecil;
using Ploeh.AutoFixture;
using WeavingTestData.GameEventsMock;

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
            Fixture.Register(() => AssemblyDefinition.ReadAssembly(filename));

            Fixture.Register(() => new GetGameEventTypes(new GetGameEventFields()));
        }
    }
}

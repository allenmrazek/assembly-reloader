using System.ComponentModel;
using System.Linq;
using AssemblyReloader.Queries.CecilQueries;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace AssemblyReloaderUnitTests.FixtureCustomizations
{
    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization() : base(new AutoNSubstituteCustomization())
        {
        }
    }
}
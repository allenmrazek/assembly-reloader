using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace AssemblyReloaderTests.FixtureCustomizations
{
    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization() : base(new AutoNSubstituteCustomization())
        {
        }
    }
}
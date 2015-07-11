using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace AssemblyReloaderTests.Fixtures
{
    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization() : base(new AutoNSubstituteCustomization())
        {
        }
    }
}
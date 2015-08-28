using Ploeh.AutoFixture.Xunit;
using Xunit.Extensions;

namespace AssemblyReloaderTests.Fixtures
{
    internal class AutoDomainDataWithInlineDataAttribute : CompositeDataAttribute
    {
        internal AutoDomainDataWithInlineDataAttribute(params object[] values)
            : base(
                new InlineDataAttribute(values),
                new AutoDomainDataAttribute())
        {
        }
    }
}
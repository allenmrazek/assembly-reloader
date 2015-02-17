using System.Reflection;
using Ploeh.AutoFixture;

namespace AssemblyReloaderUnitTests.FixtureCustomizations
{
    public class AssemblyIsLocalAssemblyCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => Assembly.GetExecutingAssembly());
        }
    }
}

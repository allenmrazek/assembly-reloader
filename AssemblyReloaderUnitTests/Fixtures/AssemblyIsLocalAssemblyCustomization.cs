using System.Reflection;
using Ploeh.AutoFixture;

namespace AssemblyReloaderTests.FixtureCustomizations
{
    public class AssemblyIsLocalAssemblyCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => Assembly.GetExecutingAssembly());
        }
    }
}

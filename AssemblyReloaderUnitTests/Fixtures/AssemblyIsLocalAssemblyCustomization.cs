using System.Reflection;
using Ploeh.AutoFixture;

namespace AssemblyReloaderTests.Fixtures
{
    public class AssemblyIsLocalAssemblyCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => Assembly.GetExecutingAssembly());
        }
    }
}

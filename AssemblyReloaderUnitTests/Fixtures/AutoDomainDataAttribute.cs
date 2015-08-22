using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Config;
using Mono.Cecil;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace AssemblyReloaderTests.Fixtures
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(new Fixture().Customize(new DomainCustomization()))
        {
            const string filename = "WeavingTestData.dll";

            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            var assemblyDefinition = AssemblyDefinition.ReadAssembly(filename);

            // Context
            Fixture.Register(() => assemblyDefinition);

            // TypeDefintiion
            //var typesWithAtLeastOneMethod =
            //    assemblyDefinition.Modules.SelectMany(md => md.Types).Where(td => td.Methods.Count > 0);

            //Fixture.Register(() => typesWithAtLeastOneMethod.First());

            // Assembly => ExecutingAssembly
            
            //Fixture.Register(() => Assembly.GetExecutingAssembly());
            //Fixture.Register(() => new SettingSerializationSurrogate());

            // IAddonAttributesFromAssembly
            //Fixture.Register(() => new GetAddonAttributesFromType());


            // MethodDefinition => TestPartModule.OnSave
            //Fixture.Register(
            //    () =>
            //        assemblyDefinition.MainModule.Types.Single(td => td.FullName == "AssemblyReloaderTests.TestData.PartModules.TestPartModule").Methods.Single(
            //            md => md.Name == "OnSave"));

        }
    }
}
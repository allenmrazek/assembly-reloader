using System.Reflection;
using AssemblyReloader.Loaders;
using AssemblyReloader.Loaders.Addon;
using AssemblyReloader.Providers.ConfigNodeProviders;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using NSubstitute;
using ReeperCommon.Logging;
using Xunit;

namespace AssemblyReloaderUnitTests.Loaders
{
    public class LoaderFactory_Test
    {
        //[Fact]
        //void CreateAddonLoader_QueriesAssemblyForTypes_Creates_ThenLoadsCurrentSceneAddons()
        //{
        //    var addonFactory = Substitute.For<IAddonFactory>();
        //    var addonQuery = Substitute.For<IAddonsFromAssemblyQuery>();
        //    var pmQuery = Substitute.For<IPartModulesFromAssemblyQuery>();
        //    var curScene = Substitute.For<ICurrentStartupSceneProvider>();
        //    var cfgProvider = Substitute.For<IPartConfigProvider>();

        //    var sut = new LoaderFactory(addonFactory, addonQuery, pmQuery, curScene, cfgProvider);

        //    Assert.NotNull(sut.CreateAddonLoader(Assembly.GetExecutingAssembly(), Substitute.For<ILog>()));

        //    addonQuery.Received(1).Get(Arg.Any<Assembly>());
        //    curScene.Received(1).Get();
        //    pmQuery.DidNotReceive().Get(Arg.Any<Assembly>());
        //}



        //[Fact]
        //void CreatePartModuleLoader_QueriesAssemblyForTypes()
        //{
        //    var addonFactory = Substitute.For<IAddonFactory>();
        //    var addonQuery = Substitute.For<IAddonsFromAssemblyQuery>();
        //    var pmQuery = Substitute.For<IPartModulesFromAssemblyQuery>();
        //    var curScene = Substitute.For<ICurrentStartupSceneProvider>();
        //    var cfgProvider = Substitute.For<IPartConfigProvider>();

        //    curScene.Get().Returns(KSPAddon.Startup.SpaceCentre);

        //    var sut = new LoaderFactory(addonFactory, addonQuery, pmQuery, curScene, cfgProvider);

        //    var result = sut.CreatePartModuleLoader(Assembly.GetExecutingAssembly(), Substitute.For<ILog>());

        //    Assert.NotNull(result);
        //    addonQuery.DidNotReceive().Get(Arg.Any<Assembly>());
        //    pmQuery.Received(1).Get(Arg.Any<Assembly>());
        //    curScene.Received(1).Get();
        //}
    }
}

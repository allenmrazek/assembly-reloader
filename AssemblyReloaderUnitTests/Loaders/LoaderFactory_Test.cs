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

        //    var sut = new AddonLoaderFactory(addonFactory, addonQuery, pmQuery, curScene, cfgProvider);

        //    Assert.NotNull(sut.Create(Assembly.GetExecutingAssembly(), Substitute.For<ILog>()));

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

        //    var sut = new AddonLoaderFactory(addonFactory, addonQuery, pmQuery, curScene, cfgProvider);

        //    var result = sut.CreatePartModuleLoader(Assembly.GetExecutingAssembly(), Substitute.For<ILog>());

        //    Assert.NotNull(result);
        //    addonQuery.DidNotReceive().Get(Arg.Any<Assembly>());
        //    pmQuery.Received(1).Get(Arg.Any<Assembly>());
        //    curScene.Received(1).Get();
        //}
    }
}

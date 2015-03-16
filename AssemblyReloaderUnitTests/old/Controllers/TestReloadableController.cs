//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AssemblyReloader.Controllers;
//using AssemblyReloader.PluginTracking;
//using AssemblyReloader.Providers.SceneProviders;
//using AssemblyReloader.Queries;
//using NSubstitute;
//using Xunit;

//namespace AssemblyReloaderUnitTests.Controllers
//{
//    public class TestReloadableController
//    {
//        private class ReloadableControllerFactory
//        {
//            public static ReloadablePluginController Create()
//            {
//                return new ReloadablePluginController(
//                    new[]
//                    {
//                        Substitute.For<IReloadablePlugin>(), Substitute.For<IReloadablePlugin>()
//                    });
//            }
//        }



//        [Fact]
//        public void Constructor_NullArg_ThrowsException()
//        {
//            Assert.Throws<ArgumentNullException>(() =>
//                new ReloadablePluginController(
                     
//                    null));
//        }




//        [Fact]
//        public void ReloadableAssemblies_Returns_WhatWasContructedWith()
//        {
//            var first = Substitute.For<IReloadablePlugin>();
//            var second = Substitute.For<IReloadablePlugin>();

//            first.Name.Returns("First");
//            second.Name.Returns("Second");

//            IEnumerable<IReloadablePlugin> reloadables = new[]
//            {
//                first,
//                second
//            };

//            var sut = new ReloadablePluginController(reloadables);

//            Assert.NotEmpty(sut.Plugins);
//            Assert.Contains("First", sut.Plugins.Select(i => i.Name));
//            Assert.Contains("Second", sut.Plugins.Select(i => i.Name));
//        }



//        [Fact]
//        public void ReloadAll_Calls_Unload_Then_Load()
//        {
//            var reloadable = Substitute.For<IReloadablePlugin>();

//            var sut = new ReloadablePluginController(new[] { reloadable });

//            sut.ReloadAll();


//            reloadable.Received(1).Unload();
//            reloadable.Received(1).Load();
//        }
//    }
//}

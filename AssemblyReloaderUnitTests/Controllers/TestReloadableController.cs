using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Controllers;
using AssemblyReloader.PluginTracking;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries;
using NSubstitute;
using Xunit;

namespace AssemblyReloaderUnitTests.AssemblyTracking
{
    public class TestReloadableController
    {
        private class ReloadableControllerFactory
        {
            public static ReloadableController Create()
            {
                return new ReloadableController(
                    new[]
                    {
                        Substitute.For<IReloadablePlugin>(), Substitute.For<IReloadablePlugin>()
                    },
                    Substitute.For<IQueryFactory>(),
                    Substitute.For<ICurrentGameSceneProvider>());
            }
        }



        [Fact]
        public void Constructor_NullArg_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ReloadableController(
                     
                    Substitute.For<
                        IEnumerable<IReloadablePlugin>
                    >(),
                    null,
                    Substitute.For<ICurrentGameSceneProvider>()));

            Assert.Throws<ArgumentNullException>(() =>
                new ReloadableController(
                    null,
                    Substitute.For<IQueryFactory>(),
                    Substitute.For<ICurrentGameSceneProvider>()));

            Assert.Throws<ArgumentNullException>(() =>
                new ReloadableController(
                    Substitute.For<
                                    IEnumerable<IReloadablePlugin>
                                >(),
                    Substitute.For<IQueryFactory>(),
                    null));
        }




        [Fact]
        public void ReloadableAssemblies_Returns_WhatWasContructedWith()
        {
            var first = Substitute.For<IReloadablePlugin>();
            var second = Substitute.For<IReloadablePlugin>();

            first.Name.Returns("First");
            second.Name.Returns("Second");

            IEnumerable<IReloadablePlugin> reloadables = new[]
            {
                first,
                second
            };

            var sut = new ReloadableController(reloadables, Substitute.For<IQueryFactory>(), Substitute.For<ICurrentGameSceneProvider>());

            Assert.NotEmpty(sut.Plugins);
            Assert.Contains("First", sut.Plugins.Select(i => i.Name));
            Assert.Contains("Second", sut.Plugins.Select(i => i.Name));
        }



        [Fact]
        public void ReloadAll_Calls_Unload_Then_Load()
        {
            var reloadable = Substitute.For<IReloadablePlugin>();
            var query = Substitute.For<IQueryFactory>();


            var sut = new ReloadableController(new[] { reloadable }, query, Substitute.For<ICurrentGameSceneProvider>());

            sut.ReloadAll();


            reloadable.Received(1).Unload();
            reloadable.Received(1).Load();
        }
    }
}

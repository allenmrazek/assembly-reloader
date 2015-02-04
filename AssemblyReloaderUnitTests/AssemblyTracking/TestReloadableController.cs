using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.AssemblyTracking;
using AssemblyReloader.AssemblyTracking.Implementations;
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
                    Substitute.For<IQueryFactory>(),
                    new[]
                    {
                        Substitute.For<ReloadableAssembly>(), Substitute.For<ReloadableAssembly>()
                    });
            }
        }



        [Fact]
        public void Constructor_NullArg_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ReloadableController(
                    null, 
                    Substitute.For<
                        IEnumerable<IReloadableAssembly>
                    >()));

            Assert.Throws<ArgumentNullException>(() =>
                new ReloadableController(
                    Substitute.For<IQueryFactory>(),
                    null));
        }




        [Fact]
        public void ReloadableAssemblies_Returns_WhatWasContructedWith()
        {
            var first = Substitute.For<IReloadableAssembly>();
            var second = Substitute.For<IReloadableAssembly>();

            first.Name.Returns("First");
            second.Name.Returns("Second");

            IEnumerable<IReloadableAssembly> reloadables = new[]
            {
                first,
                second
            };

            var sut = new ReloadableController(Substitute.For<IQueryFactory>(), reloadables);

            Assert.NotEmpty(sut.ReloadableAssemblies);
            Assert.Contains("First", sut.ReloadableAssemblies.Select(i => i.Name));
            Assert.Contains("Second", sut.ReloadableAssemblies.Select(i => i.Name));
        }



        [Fact]
        public void ReloadAll_Calls_Unload_Then_Load_Then_StartAddons()
        {
            var reloadable = Substitute.For<IReloadableAssembly>();
            var query = Substitute.For<IQueryFactory>();


            var sut = new ReloadableController(query, new[] {reloadable});

            sut.ReloadAll();


            reloadable.Received(1).Unload();
            reloadable.Received(1).Load();
            reloadable.Received(1).StartAddons(Arg.Any<KSPAddon.Startup>());
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.AssemblyTracking;
using AssemblyReloader.AssemblyTracking.Implementations;
using AssemblyReloader.Queries;
using NSubstitute;
using ReeperCommon.Logging;
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
                    Substitute.For<ILog>(),
                    new[]
                    {
                        Substitute.For<ReloadableAssembly>(), Substitute.For<ReloadableAssembly>()
                    });
            }
        }


        public void TestConstructor()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new ReloadableController(null, 
                    Substitute.For<ILog>(),
                    Substitute.For<
                        IEnumerable<IReloadableAssembly>
                    >()));

        }
    }
}

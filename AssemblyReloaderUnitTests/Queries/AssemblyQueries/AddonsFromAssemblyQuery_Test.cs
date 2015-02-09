using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.ConversionQueries;
using UnityEngine;
using Xunit;

namespace AssemblyReloaderUnitTests.Queries.AssemblyQueries
{


    public class AddonsFromAssemblyQuery_Test
    {


        [Fact]
        void Get_Returns_TestAddons()
        {
            var sut = new AddonsFromAssemblyQuery(new AddonAttributeFromTypeQuery());

            var results = sut.Get(Assembly.GetExecutingAssembly());

            Assert.NotEmpty(results);
            Assert.Contains(typeof (Fixture.TestAddon_Public), results);
            Assert.Contains(typeof (Fixture.TestAddon_Private), results);
            Assert.Contains(typeof (Fixture.TestAddon_MultipleAttributes), results);

        }

        [Fact]
        void Get_DoesNotReturn_InvalidEntries()
        {
            var sut = new AddonsFromAssemblyQuery(new AddonAttributeFromTypeQuery());

            var results = sut.Get(Assembly.GetExecutingAssembly());

            Assert.NotEmpty(results);
            Assert.DoesNotContain(typeof(Fixture.TestAddon_InvalidResultPartModule), results);
            Assert.DoesNotContain(typeof(Fixture.TestAddon_InvalidResult), results);
            Assert.DoesNotContain(typeof(Fixture.MonoBehaviour_WithNoAddon), results);
        }
    }
}

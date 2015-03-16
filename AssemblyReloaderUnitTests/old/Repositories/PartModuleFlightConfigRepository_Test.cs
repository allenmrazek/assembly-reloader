using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Repositories;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace AssemblyReloaderUnitTests.Repositories
{
    public class PartModuleFlightConfigRepository_Test
    {
        [Theory, AutoData]
        void Store_Then_Retrieve_ReturnsCorrectResult(uint flightid, ITypeIdentifier key, ConfigNode data, FlightConfigRepository sut)
        {
            sut.Store(flightid, key, data);

            var result = sut.Retrieve(flightid, key);

            Assert.True(result.Any());
            Assert.False(sut.Retrieve(flightid, key).Any()); // we should only be able to retrieve data once
            Assert.Same(data, result.Single());
        }


        [Theory, AutoData]
        void Retrieve_NonexistingData_Fails(uint flightid, ITypeIdentifier key, ConfigNode data,
            FlightConfigRepository sut)
        {
            var result = sut.Retrieve(flightid, key);

            Assert.False(result.Any());
        }


        [Theory, AutoData]
        void Inserting_Then_Retrieving_Is_FIFO(uint flightid, ITypeIdentifier key, FlightConfigRepository sut)
        {
            var data1 = new ConfigNode();
            var data2 = new ConfigNode();

            sut.Store(flightid, key, data1);
            sut.Store(flightid, key, data2);

            var result1 = sut.Retrieve(flightid, key);
            var result2 = sut.Retrieve(flightid, key);

            Assert.True(result1.Any());
            Assert.True(result2.Any());

            Assert.Same(data1, result1.Single());
            Assert.Same(data2, result2.Single());
        }
    }
}

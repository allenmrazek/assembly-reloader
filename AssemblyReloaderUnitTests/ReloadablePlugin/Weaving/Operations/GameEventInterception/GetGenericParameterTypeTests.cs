using System;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;
using AssemblyReloaderTests.Fixtures;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloaderTests.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    class GenericClass<TT1>
    {

    }

    class GenericClass<TT1, TT2>
    {

    }

    class GenericClass<TT1, TT2, TT3>
    {

    }

    public abstract class GetGenericParameterTypeTests<T1, T2, T3>
    {


        [Theory, AutoDomainData]
        public void Get_ThrowsExceptionOnNonGenericType(GetGenericParameterType sut)
        {
            Assert.Throws<ArgumentException>(() => sut.Get(typeof (string), 0));
        }


        [Theory, AutoDomainData]
        public void Get_ThrowsExceptionOnNull(GetGenericParameterType sut)
        {
            Assert.Throws<ArgumentNullException>(() => sut.Get(null, 0));
        }


        [Theory, AutoDomainData]
        public void Get_ThrowsExceptionOnBadIndex(GetGenericParameterType sut)
        {
            Assert.Throws<ArgumentException>(() => sut.Get(typeof (GenericClass<T1>), -1));
            Assert.Throws<ArgumentException>(() => sut.Get(typeof (GenericClass<T1>), 1));
        }


        [Theory, AutoDomainData]
        public void Get_WithOneGenericParameter()
        {
            var sut = new GetGenericParameterType();

            var result = sut.Get(typeof (GenericClass<T1>), 0);

            Assert.NotNull(result);
            Assert.Equal(typeof(T1), result);
        }


        [Theory, AutoDomainData]
        public void Get_WithTwoGenericParameters()
        {
            var sut = new GetGenericParameterType();

            var result1 = sut.Get(typeof(GenericClass<T1, T2>), 0);
            var result2 = sut.Get(typeof (GenericClass<T1, T2>), 1);

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(typeof(T1), result1);
            Assert.Equal(typeof (T2), result2);
        }



        [Theory, AutoDomainData]
        public void Get_WithThreeGenericParameters()
        {
            var sut = new GetGenericParameterType();

            var result1 = sut.Get(typeof(GenericClass<T1, T2>), 0);
            var result2 = sut.Get(typeof(GenericClass<T1, T2>), 1);
            var result3 = sut.Get(typeof (GenericClass<T1, T2, T3>), 2);

            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.Equal(typeof(T1), result1);
            Assert.Equal(typeof(T2), result2);
            Assert.Equal(typeof (T3), result3);
        }
    }


    public class TestVariaton1 : GetGenericParameterTypeTests<string, int, float>
    {
    }

    public class TestVariation2 : GetGenericParameterTypeTests<EventReport, ProtoCrewMember, Part>
    {
    }
}

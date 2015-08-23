using System;
using System.Collections.Generic;
using AssemblyReloaderTests.Fixtures;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception.Tests
{
    public abstract class GameEventReferenceTests<T1, T2, T3>
    {
        [Theory, AutoDomainData]
        public void GameEventReference_Constructor_DoesNotThrowOnValidEventTypesTest([Frozen] string evtName)
        {
            Assert.DoesNotThrow(() => new GameEventReference(new EventVoid(evtName)));
            Assert.DoesNotThrow(() => new GameEventReference(new EventData<T1>(evtName)));
            Assert.DoesNotThrow(() => new GameEventReference(new EventData<T1, T2>(evtName)));
            Assert.DoesNotThrow(() => new GameEventReference(new EventData<T1, T2, T3>(evtName)));
        }

        [Theory, AutoDomainData]
        public void GameEventReference_Constructor_ThrowsOnInvalidTypesTest([Frozen] string evtName)
        {
            Assert.Throws<ArgumentException>(() => new GameEventReference(new Dictionary<T1, T2>()));
            Assert.Throws<ArgumentException>(() => new GameEventReference(evtName));
            Assert.Throws<ArgumentNullException>(() => new GameEventReference(null));
        }


        [Fact]
        public void GameEventReference_EqualityTest()
        {
            var evt = new EventVoid("event");
            var someList = new List<GameEventReference>();

            var sut1 = new GameEventReference(evt);
            var sut2 = new GameEventReference(evt);
            someList.Add(sut1);

            Assert.Equal(sut1, sut2);
            Assert.Contains(sut2, someList);
        }
    }

    public class TestSimple : GameEventReferenceTests<double, float, int>
    {
       
    }

    public class TestKspTypes : GameEventReferenceTests<Vessel, Part, PartModule>
    {
        
    }

    public class TestKspTypesTricky :
        GameEventReferenceTests
            <GameEvents.FromToAction<Part, Part>, GameEvents.HostTargetAction<Vessel, Vessel>,
                GameEvents.ExplosionReaction>
    {
        
    }
}

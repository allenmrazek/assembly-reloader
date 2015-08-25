using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloaderTests.Fixtures;
using Ploeh.AutoFixture.Xunit;
using ReeperCommon.Containers;
using Xunit;
using Xunit.Extensions;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;

// ReSharper disable once CheckNamespace
namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception.Tests
{
    public abstract class GameEventReferenceTests<T1, T2, T3>
    {
        [Theory, AutoDomainData]
        public void GameEventReference_Constructor_DoesNotThrowOnValidEventTypesTest([Frozen] string evtName)
        {
            Assert.DoesNotThrow(() => new GameEventReference(new EventVoid(evtName), evtName));
            Assert.DoesNotThrow(() => new GameEventReference(new EventData<T1>(evtName), evtName));
            Assert.DoesNotThrow(() => new GameEventReference(new EventData<T1, T2>(evtName), evtName));
            Assert.DoesNotThrow(() => new GameEventReference(new EventData<T1, T2, T3>(evtName), evtName));
        }

        [Theory, AutoDomainData]
        public void GameEventReference_Constructor_ThrowsOnInvalidTypesTest([Frozen] string evtName)
        {
            Assert.Throws<ArgumentException>(() => new GameEventReference(new Dictionary<T1, T2>(), evtName));
            Assert.Throws<ArgumentException>(() => new GameEventReference(evtName, evtName));
            Assert.Throws<ArgumentNullException>(() => new GameEventReference(null, evtName));
        }


        [Theory, AutoDomainData]
        public void GameEventReference_EqualityTest([Frozen] string evtName)
        {
            var evt = new EventVoid("event");
            var someList = new List<GameEventReference>();

            var sut1 = new GameEventReference(evt, evtName);
            var sut2 = new GameEventReference(evt, evtName);
            someList.Add(sut1);

            Assert.Equal(sut1, sut2);
            Assert.Contains(sut2, someList);
        }


        [Theory, AutoDomainData]
        public void TryGetValueTest([Frozen] string evtName)
        {
            Action del = () => { };
            var entries = new Dictionary<GameEventReference, List<GameEventCallback>>();
            var evt = new EventVoid(evtName);
            var sut = new GameEventReference(evt, evtName);

            entries.Add(sut, new List<GameEventCallback>());

            List<GameEventCallback> result;

            var wasFound = entries.TryGetValue(sut, out result);

            Assert.True(wasFound);
            Assert.Contains(new GameEventReference(evt, evtName), entries.Keys);
            Assert.True(entries.ContainsKey(sut));
            Assert.True(entries.ContainsKey(new GameEventReference(evt, evtName)));
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

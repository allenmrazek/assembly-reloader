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


        [Fact]
        public void DelegateEqualityTest_NonGeneric()
        {
            var sut = new EventVoid.OnEvent(OnVoidCallback);
            var sut2 = new EventVoid.OnEvent(OnVoidCallback);

            Assert.Equal(sut, sut2);
        }


        [Fact]
        public void DelegateEqualityTest_Generic()
        {
            var sut = new EventData<T1>.OnEvent(OnCallback);
            var sut2 = new EventData<T1>.OnEvent(OnCallback);

            Assert.Equal(sut, sut2);
        }


        [Theory, AutoDomainData]
        public void DelegateEqualityTest_NonGeneric_AsObject([Frozen] string evtName)
        {
            var evt = new EventVoid(evtName);
            var list = new List<object>();

            object sut = new EventVoid.OnEvent(OnVoidCallback);
            object sut2 = new EventVoid.OnEvent(OnVoidCallback);

            list.Add(sut);

            Assert.Equal(sut, sut2);
            Assert.Contains(sut2, list);
        }


        private void OnVoidCallback()
        {
            
        }

        private void OnCallback(T1 arg)
        {
            
        }
    }

// ReSharper disable once InconsistentNaming
    public class GameEventReferenceTest_Simple : GameEventReferenceTests<double, float, int>
    {
       
    }

// ReSharper disable once InconsistentNaming
    public class GameEventReferenceTest_KspTypes : GameEventReferenceTests<Vessel, Part, PartModule>
    {
        
    }

// ReSharper disable once InconsistentNaming
    public class GameEventReferenceTest_Tricky :
        GameEventReferenceTests
            <GameEvents.FromToAction<Part, Part>, GameEvents.HostTargetAction<Vessel, Vessel>,
                GameEvents.ExplosionReaction>
    {
        
    }
}

using System.Reflection;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception;
using AssemblyReloaderTests.Fixtures;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

// ReSharper disable once CheckNamespace
namespace AssemblyReloaderTests.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public abstract class GameEventRegistryTests<T1, T2, T3>
    {
        [Theory, AutoDomainData]
        public void AddTest_WithEventVoid_WithRefEqualCallbackAndEvent(GameEventRegistry sut, [Frozen] string evtName)
        {
            var evt = new GameEventReference(new EventVoid(evtName), evtName);
            var callback = new GameEventCallback(new EventVoid.OnEvent(OnVoidCallback), ReeperCommon.Containers.Maybe<MethodBase>.None);

            sut.Add(evt, callback);

            Assert.True(sut.Count > 0);
            Assert.True(sut.Remove(evt, callback));
        }

        [Theory, AutoDomainData]
        public void AddTest_WithEventVoid_WithNotRefEqualCallbackAndEvent(GameEventRegistry sut, [Frozen] string evtName)
        {
            var evt = new EventVoid(evtName);

            var evt1 = new GameEventReference(evt, evtName);
            var evt2 = new GameEventReference(evt, evtName);
            var callback1 = new GameEventCallback(new EventVoid.OnEvent(OnVoidCallback), ReeperCommon.Containers.Maybe<MethodBase>.None);
            var callback2 = new GameEventCallback(new EventVoid.OnEvent(OnVoidCallback), ReeperCommon.Containers.Maybe<MethodBase>.None);

            sut.Add(evt1, callback1);

            Assert.True(sut.Count > 0);
            Assert.True(sut.Remove(evt2, callback2));
        }


        [Theory, AutoDomainData]
        public void AddTest_WithEventVoid_WithRefEqualEvent_WithNonRefEqualCallback(GameEventRegistry sut, [Frozen] string evtName)
        {
            var evt = new GameEventReference(new EventVoid(evtName), evtName);
            var callback1 = new GameEventCallback(new EventVoid.OnEvent(OnVoidCallback), ReeperCommon.Containers.Maybe<MethodBase>.None);
            var callback2 = new GameEventCallback(new EventVoid.OnEvent(OnVoidCallback), ReeperCommon.Containers.Maybe<MethodBase>.None);

            sut.Add(evt, callback1);

            Assert.True(sut.Count > 0);
            Assert.True(sut.Remove(evt, callback2));
        }

        //[Fact()]
        //public void RemoveTest()
        //{
        //    Assert.True(false, "not implemented yet");
        //}

        //[Fact()]
        //public void ClearCallbacksTest()
        //{
        //    Assert.True(false, "not implemented yet");
        //}

        //[Fact()]
        //public void GetEnumeratorTest()
        //{
        //    Assert.True(false, "not implemented yet");
        //}


        private void OnVoidCallback()
        {
            
        }
    }

    public class GameEventRegistryTestsSimple : GameEventRegistryTests<int, float, string>
    {
        
    }

    public class GameEventRegistryTestsKsp : GameEventRegistryTests<ProtoCrewMember, EventReport, Vessel>
    {

    }
}

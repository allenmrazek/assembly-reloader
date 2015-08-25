using System;
using System.Collections.Generic;
using System.Reflection;
using ReeperCommon.Containers;
using Xunit;
// ReSharper disable once CheckNamespace
namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception.Tests
{
    public class GameEventCallbackTests
    {
        [Fact()]
        public void GameEventCallbackTest()
        {
            Assert.Throws<ArgumentNullException>(() => new GameEventCallback(null, Maybe<MethodBase>.None));
            Assert.Throws<ArgumentException>(() => new GameEventCallback("notADelegate", Maybe<MethodBase>.None));
            Assert.DoesNotThrow(() => new GameEventCallback(new EventVoid.OnEvent(() => { }), Maybe<MethodBase>.None));
        }


        [Fact()]
        public void EqualsTest()
        {
            Action del = () => { };
            var list = new List<GameEventCallback>();

            var sut1 = new GameEventCallback(del, Maybe<MethodBase>.None);
            var sut2 = new GameEventCallback(del, Maybe<MethodBase>.None);
            list.Add(sut1);

            Assert.Equal(sut1, sut2);
            Assert.Contains(sut2, list);
        }

    }
}

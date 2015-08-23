using System;
using System.Collections.Generic;
using Xunit;
// ReSharper disable once CheckNamespace
namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception.Tests
{
    public class GameEventCallbackTests
    {
        [Fact()]
        public void GameEventCallbackTest()
        {
            Assert.Throws<ArgumentNullException>(() => new GameEventCallback(null));
            Assert.Throws<ArgumentException>(() => new GameEventCallback("notADelegate"));
            Assert.DoesNotThrow(() => new GameEventCallback(new EventVoid.OnEvent(() => { })));
        }


        [Fact()]
        public void EqualsTest()
        {
            Action del = () => { };
            var list = new List<GameEventCallback>();

            var sut1 = new GameEventCallback(del);
            var sut2 = new GameEventCallback(del);
            list.Add(sut1);

            Assert.Equal(sut1, sut2);
            Assert.Contains(sut2, list);
        }
    }
}

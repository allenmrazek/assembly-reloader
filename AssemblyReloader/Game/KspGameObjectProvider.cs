using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.Containers;

namespace AssemblyReloader.Game
{
    [Implements(typeof(IGameObjectProvider))]
    public class KspGameObjectProvider : IGameObjectProvider
    {
        public Maybe<ScenarioRunner> Get()
        {
            return ScenarioRunner.fetch != null
                ? Maybe<ScenarioRunner>.With(ScenarioRunner.fetch)
                : Maybe<ScenarioRunner>.None;
        }
    }
}

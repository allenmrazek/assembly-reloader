using UnityEngine;

namespace AssemblyReloader.Config
{
    public class CoreContext : SignalContext
    {
        public CoreContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            commandBinder.Bind<StartSignal>().To<StartCommand>().Once();
        }
    }
}

using AssemblyReloader.StrangeIoC.extensions.command.api;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.context.impl;
using UnityEngine;

namespace AssemblyReloader.Config
{
    public class SignalContext : MVCSContext
    {
        public SignalContext(MonoBehaviour view, bool autoStartup) : base(view, autoStartup)
        {
        }


        protected override void mapBindings()
        {
            base.mapBindings();
            implicitBinder.ScanForAnnotatedClasses(new[] {"AssemblyReloader"});
        }


        protected override void postBindings()
        {
            base.postBindings();
            Object.DontDestroyOnLoad((GameObject)contextView);
        }


        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }



    }
}

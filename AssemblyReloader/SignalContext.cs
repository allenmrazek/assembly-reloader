using System.Reflection;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.implicitBind.api;
using UnityEngine;

namespace AssemblyReloader
{
    public class SignalContext : MVCSContext
    {
        public SignalContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }


        protected override void mapBindings()
        {
            base.mapBindings();
            implicitBinder.ScanForAnnotatedClasses(new []
            {
                new AnnotatedAssembly(Assembly.GetExecutingAssembly(), new [] { "AssemblyReloader" })
            });
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

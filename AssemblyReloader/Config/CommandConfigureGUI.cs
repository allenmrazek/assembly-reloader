using AssemblyReloader.Gui;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using UnityEngine;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
    public class CommandConfigureGUI : Command
    {
        [Inject (Keys.GameObjectKeys.CoreContext)] public GameObject gameObject { get; set; }

        public override void Execute()
        {
            injectionBinder.Bind<SignalCloseAllWindows>().ToSingleton().CrossContext();

            var mainView = new GameObject("MainView");
            var configView = new GameObject("ConfigurationView");

            // views will bubble up the transform hierarchy looking for a context to attach to.
            // For config and mainview, this will be the core context
            configView.transform.parent = mainView.transform.parent = gameObject.transform;

            Object.DontDestroyOnLoad(mainView);
            Object.DontDestroyOnLoad(configView);

            mainView.AddComponent<MainView>();
            configView.AddComponent<ConfigurationView>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Gui;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
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
        [Inject] public ILog Log { get; set; }


        public override void Execute()
        {
            gameObject.PrintComponents(Log);

            var mainView = new GameObject("MainView");
            var configView = new GameObject("ConfigurationView");
            
            
            configView.transform.parent = mainView.transform.parent = gameObject.transform;

            UnityEngine.Object.DontDestroyOnLoad(mainView);
            UnityEngine.Object.DontDestroyOnLoad(configView);

            mainView.AddComponent<MainView>();
            configView.AddComponent<ConfigurationView>();

            injectionBinder.Bind<SignalCloseAllWindows>().ToSingleton().CrossContext();

        }
    }
}

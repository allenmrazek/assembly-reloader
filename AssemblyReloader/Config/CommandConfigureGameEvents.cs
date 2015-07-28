using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using UnityEngine;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandConfigureGameEvents : Command
    {
// ReSharper disable once InconsistentNaming
// ReSharper disable once MemberCanBePrivate.Global
        [Inject (Keys.GameObjectKeys.CoreContext)] public GameObject gameObject { get; set; }


        public override void Execute()
        {
            //var eventView = AddonLoader.Instance.gameObject.AddComponent<GameEventView>();
            var eventView = new GameObject("AssemblyReloader.GameEventView", typeof (GameEventView));
            eventView.transform.parent = gameObject.transform;

            Object.DontDestroyOnLoad(eventView);
        }
    }
}

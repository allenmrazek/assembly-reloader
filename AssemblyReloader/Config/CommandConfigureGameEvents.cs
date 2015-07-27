using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using UnityEngine;

namespace AssemblyReloader.Config
{
    public class CommandConfigureGameEvents : Command
    {
        [Inject (Keys.GameObjectKeys.CoreContext)] public GameObject gameObject { get; set; }


        public override void Execute()
        {
            //var eventView = new GameObject("GameEventView", typeof (GameEventView));
            var eventView = AddonLoader.Instance.gameObject.AddComponent<GameEventView>();

            eventView.transform.parent = gameObject.transform;

            UnityEngine.Object.DontDestroyOnLoad(eventView);
        }
    }
}

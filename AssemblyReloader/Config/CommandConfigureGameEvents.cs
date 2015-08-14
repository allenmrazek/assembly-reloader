using AssemblyReloader.Game;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandConfigureGameEvents : Command
    {
// ReSharper disable once InconsistentNaming
// ReSharper disable once MemberCanBePrivate.Global
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject gameObject { get; set; }


        public override void Execute()
        {
            //var eventView = AddonLoader.Instance.gameObject.AddComponent<GameEventView>();
            var eventView = new GameObject("AssemblyReloader.GameEventView", typeof (GameEventView));
            eventView.transform.parent = gameObject.transform;

            Object.DontDestroyOnLoad(eventView);
        }
    }
}

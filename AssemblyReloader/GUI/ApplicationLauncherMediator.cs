using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using strange.extensions.injector;
using strange.extensions.mediation.impl;

namespace AssemblyReloader.Gui
{
    public class ApplicationLauncherMediator : Mediator
    {
        [Inject] public ApplicationLauncherView View { get; set; }
        [Inject] public SignalApplicationLauncherButtonToggle ToggleSignal { get; set; }
        [Inject] public SignalMainViewVisibilityChanged MainViewVisibilitySignal { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            View.Toggle.AddListener(OnButtonToggle);
            MainViewVisibilitySignal.AddListener(OnMainViewVisibilityChanged);
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.Toggle.RemoveListener(OnButtonToggle);
            MainViewVisibilitySignal.RemoveListener(OnMainViewVisibilityChanged);
        }


        private void OnButtonToggle(bool tf)
        {
            ToggleSignal.Dispatch(tf);
        }


        private void OnMainViewVisibilityChanged(bool isVisible)
        {
            View.SetToggleState(isVisible);
        }
    }
}

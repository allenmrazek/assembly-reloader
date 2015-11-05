extern alias KSP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Config.Keys;
using ReeperCommon.Containers;
using ReeperCommon.Gui.Window;
using strange.extensions.injector;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;
using ApplicationLauncher = KSP::ApplicationLauncher;
using ApplicationLauncherButton = KSP::ApplicationLauncherButton;
using RUIToggleButton = KSP::RUIToggleButton;

namespace AssemblyReloader.Gui
{
    public class ApplicationLauncherView : View
    {
        [Inject(TextureNameKey.CloseButton)] public Texture2D ButtonTexture { get; set; }
        [Inject] public IRoutineRunner CoroutineRunner { get; set; }

        internal readonly Signal<bool> Toggle = new Signal<bool>();

        private ApplicationLauncherButton _button;

        protected override void Start()
        {
            base.Start();
            CoroutineRunner.StartCoroutine(SetupButton());
        }


        protected override void OnDestroy()
        {
            ApplicationLauncher.Instance.Do(l => _button.Do(l.RemoveApplication));

            base.OnDestroy();
        }


        private IEnumerator SetupButton()
        {
            while (ApplicationLauncher.Instance == null)
                yield return 0;
            
            _button = ApplicationLauncher.Instance.AddApplication(OnTrue, OnFalse, () =>
            { }, () => { }, delegate { }, () => { }, ButtonTexture);
        }

        private void OnFalse()
        {
            Toggle.Dispatch(false);
        }

        private void OnTrue()
        {
            Toggle.Dispatch(true);
        }

        public void SetToggleState(bool tf)
        {
            if (tf)
                _button.SetTrue(false);
            else _button.SetFalse(false);
        }
    }
}

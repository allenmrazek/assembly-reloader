using System;
using System.Collections;
using System.Linq;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.Gui;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.injector.api;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandCreateGui : Command
    {
        private const string GuiSkinPrefabName = "KSP window 4";

        private readonly GameObject _contextView;
        private readonly IResourceRepository _resources;
        private readonly IRoutineRunner _runner;


        public CommandCreateGui(
            [Name(ContextKeys.CONTEXT_VIEW)] GameObject contextView,
            IResourceRepository resources,
            IRoutineRunner runner)
        {
            if (contextView == null) throw new ArgumentNullException("contextView");
            if (resources == null) throw new ArgumentNullException("resources");
            if (runner == null) throw new ArgumentNullException("runner");

            _contextView = contextView;
            _resources = resources;
            _runner = runner;
        }


        public override void Execute()
        {
            BindSignals();
            SetupTexturesAndSkin();

            Retain();
            _runner.StartCoroutine(CreateViews());
        }


        private void BindSignals()
        {
            injectionBinder.Bind<SignalCloseAllWindows>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalTogglePluginConfigurationView>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalToggleConfigurationView>().ToSingleton();
            injectionBinder.Bind<SignalApplicationLauncherButtonToggle>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalMainViewVisibilityChanged>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalApplicationLauncherButtonCreated>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalInterfaceScaleChanged>().ToSingleton().CrossContext();
        }


        private IEnumerator CreateViews()
        {
            var mainView = new GameObject("MainView");
            var configView = new GameObject("ConfigurationView");
            var appLauncherView = new GameObject("ApplicationLauncherView");

          
            // views will bubble up the transform hierarchy looking for a context to attach to.
            // For config and mainview, this will be the core context
            configView.transform.parent = 
            mainView.transform.parent =
            appLauncherView.transform.parent = _contextView.transform;

            mainView.AddComponent<MainView>();
            configView.AddComponent<ConfigurationView>();
            appLauncherView.AddComponent<ApplicationLauncherView>();

            yield return 0; // wait until next frame so the views can initialize

            Release();
        }


  
        private void SetupTexturesAndSkin()
        {
            var skinPrefab = Resources.FindObjectsOfTypeAll<GUISkin>().Single(sk => sk.name == "KSPSkin");

            var defaultSkin = Object.Instantiate(skinPrefab);
            if (defaultSkin == null) throw new UnityInstantiationFailedException("GUISkin");

            defaultSkin.label.wordWrap = false;

            injectionBinder.Bind<GUIStyle>().To(ConfigureTitleBarButtonStyle()).ToName(StyleKey.TitleBarButtonStyle).ToSingleton().CrossContext();
            injectionBinder.Bind<GUISkin>()
                .To(defaultSkin)
                .CrossContext();

            BindTextureToName(_resources, "Resources/btnClose", TextureNameKey.CloseButton).CrossContext();
            BindTextureToName(_resources, "Resources/btnWrench", TextureNameKey.SettingsButton).CrossContext();
            BindTextureToName(_resources, "Resources/cursor", TextureNameKey.ResizeCursor).CrossContext();
            BindTextureToName(_resources, "Resources/btnScale", TextureNameKey.RescaleCursor).CrossContext();
            BindTextureToName(_resources, "Resources/AppButton", TextureNameKey.AppLauncherButton).CrossContext();
        }


        private static GUIStyle ConfigureTitleBarButtonStyle()
        {
            var style = new GUIStyle(HighLogic.Skin.button) { border = new RectOffset(), padding = new RectOffset() };
            style.fixedHeight = style.fixedWidth = 16f;
            style.margin = new RectOffset();

            return style;
        }


        private IInjectionBinding BindTextureToName(IResourceRepository resourceRepo, string url, object name)
        {
            if (resourceRepo == null) throw new ArgumentNullException("resourceRepo");
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("url is null or empty");

            var tex = resourceRepo.GetTexture(url);

            if (!tex.Any())
                throw new Exception("Couldn't bind texture at \"" + url + "\"; texture not found");

            return injectionBinder.Bind<Texture2D>().ToValue(tex.Single()).ToName(name);
        }
    }
}

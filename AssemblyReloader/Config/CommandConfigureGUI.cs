using System;
using System.Linq;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.Gui;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using ReeperCommon.Repositories;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
    public class CommandConfigureGUI : Command
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject gameObject { get; set; }

        [Inject]
        public IResourceRepository Resources { get; set; }

        public override void Execute()
        {
            BindSignals();
            SetupTexturesAndSkin();
            CreateViews();
        }


        private void BindSignals()
        {
            injectionBinder.Bind<SignalCloseAllWindows>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalTogglePluginConfigurationView>().ToSingleton().CrossContext();
            injectionBinder.Bind<SignalToggleConfigurationView>().ToSingleton();
        }


        private void CreateViews()
        {
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


        private void SetupTexturesAndSkin()
        {
            var defaultSkin = Object.Instantiate(AssetBase.GetGUISkin("KSP window 4")) as GUISkin;
            if (defaultSkin == null) throw new InvalidCastException("Failed to cast to GUISkin");

            defaultSkin.label.wordWrap = false;

            injectionBinder.Bind<GUIStyle>().To(ConfigureTitleBarButtonStyle()).ToName(Styles.TitleBarButtonStyle).ToSingleton().CrossContext();
            injectionBinder.Bind<GUISkin>()
                .To(defaultSkin)
                .CrossContext();

            BindTextureToName(Resources, "Resources/btnClose", TextureNames.CloseButton).CrossContext();
            BindTextureToName(Resources, "Resources/btnWrench", TextureNames.SettingsButton).CrossContext();
            BindTextureToName(Resources, "Resources/cursor", TextureNames.ResizeCursor).CrossContext();
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

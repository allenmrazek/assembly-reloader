using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.FileSystem;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using ReeperCommon.Serialization;
using UnityEngine;

namespace AssemblyReloader
{
    public class ReeperCommonContext : SignalContext
    {
        public ReeperCommonContext(MonoBehaviour view, ContextStartupFlags startup)
            : base(view, startup)
        {
        }


        protected override void mapBindings()
        {
            base.mapBindings();

            var log = new DebugLog("ART");
            injectionBinder.Bind<ILog>().To(log).ToSingleton().CrossContext();

            

            // cross context bindings
            injectionBinder.Bind<IUrlDirProvider>().To<KSPGameDataUrlDirProvider>().ToSingleton();
            injectionBinder.Bind<IUrlDir>().To(new KSPUrlDir(injectionBinder.GetInstance<IUrlDirProvider>().Get())).CrossContext();
            injectionBinder.Bind<IFileSystemFactory>().To<KSPFileSystemFactory>().ToSingleton().CrossContext();

            var ourLocation = GetAssemblyFileLocation(Assembly.GetExecutingAssembly());

            injectionBinder.Bind<IDirectory>().To(ourLocation.Directory).CrossContext();
            injectionBinder.Bind<IDirectory>().To(ourLocation.Directory).ToName(DirectoryKeys.Core).CrossContext();
            injectionBinder.Bind<IDirectory>().To(injectionBinder.GetInstance<IFileSystemFactory>().GameData).ToName(DirectoryKeys.GameData).CrossContext();
            injectionBinder.Bind<IFile>().To(ourLocation);

            injectionBinder.Bind<IResourceRepository>()
                .To(ConfigureResourceRepository(injectionBinder.GetInstance<IDirectory>()))
                .CrossContext();

            injectionBinder.Bind<GUIStyle>().To(ConfigureTitleBarButtonStyle()).ToName(Styles.TitleBarButtonStyle).ToSingleton().CrossContext();
            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/btnClose", TextureNames.CloseButton).CrossContext();
            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/btnWrench", TextureNames.SettingsButton).CrossContext();
            BindTextureToName(injectionBinder.GetInstance<IResourceRepository>(), "Resources/cursor", TextureNames.ResizeCursor).CrossContext();

            var serializerSelector =
                new DefaultConfigNodeItemSerializerSelector(new SurrogateProvider(new[] 
                    { 
                        Assembly.GetExecutingAssembly(), 
                        typeof(IConfigNodeSerializer).Assembly 
                    }));

            injectionBinder.Bind<IConfigNodeSerializer>().To(
                new ConfigNodeSerializer(serializerSelector, new GetSerializableFieldsRecursiveType())).CrossContext();
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


        private static IResourceRepository ConfigureResourceRepository(IDirectory dllDirectory)
        {
            // Removes extension from string, if an extension exists
            // Also converts windows-style \\ to / 
            Func<string, string> stripExtensionFromId = id =>
            {
                if (!Path.HasExtension(id) || string.IsNullOrEmpty(id)) return id;

                var dir = Path.GetDirectoryName(id) ?? "";
                var woExt = Path.Combine(dir, Path.GetFileNameWithoutExtension(id)).Replace('\\', '/');

                return !string.IsNullOrEmpty(woExt) ? woExt : id;
            };

            // Appends assembly name and replaces slashes of both types with periods to come up with a manifest resource name
            Func<string, string> convertUrlToResourceName = id =>
            {
                var prepend = Assembly.GetExecutingAssembly().GetName().Name + ".";

                if (!id.StartsWith(prepend))
                    id = prepend + id;

                return id.Replace('/', '.').Replace('\\', '.');
            };

            var currentAssemblyResource = new ResourceFromEmbeddedResource(Assembly.GetExecutingAssembly());


            return new ResourceRepositoryComposite(
                // search GameDatabase first
                //   note: GameDatabase expects extensionless urls
                new ResourceIdentifierAdapter(stripExtensionFromId, new ResourceFromGameDatabase()),

                // then look at physical file system. These work on a list of items cached
                // by GameDatabase rather than working directly with the disk (unless a resource 
                // is accessed from here, of course)
                new ResourceFromDirectory(dllDirectory),

                // finally search embedded resource
                // we need to handle both cases of the url containing the extension or not...
                new ResourceRepositoryComposite(
                // don't strip extension
                    new ResourceIdentifierAdapter(convertUrlToResourceName, currentAssemblyResource),

                    // strip extension
                    new ResourceIdentifierAdapter(stripExtensionFromId,
                        new ResourceIdentifierAdapter(convertUrlToResourceName, currentAssemblyResource)),

                    // there's a potential third issue: if the incoming id doesn't contain an extension,
                // we might not fully match any of the manifest resource names. We'll add a special adapter
                // that will select the most-closely matching name if there's only one match
                    new ResourceIdentifierAdapter(convertUrlToResourceName,
                        new ResourceIdentifierAdapter(s =>
                        {
                            var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                                .Where(name => name.StartsWith(s))
                                .ToList();

                            return resourceNames.Count == 1 ? resourceNames.First() : s;
                        }, currentAssemblyResource)
                        )));
        }


        private IFile GetAssemblyFileLocation(Assembly target)
        {
            if (target == null) throw new ArgumentNullException("target");

            var mf = injectionBinder.GetInstance<IGetAssemblyFileLocation>().Get(target);

            if (!mf.Any()) throw new Exception("Failed to find file location of " + target.FullName);
            return mf.Single();
        }
    }
}

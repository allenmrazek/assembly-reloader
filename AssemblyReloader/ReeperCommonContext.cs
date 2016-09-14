using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Config.Keys;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using ReeperKSP.FileSystem;
using ReeperKSP.FileSystem.Providers;
using ReeperKSP.Repositories;
using ReeperKSP.Serialization;
using strange.extensions.context.api;
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

            

            MapCrossContextBindings();
        }


        private void MapCrossContextBindings()
        {
            injectionBinder.Bind<IUrlDirProvider>().To<KSPGameDataUrlDirProvider>().ToSingleton().CrossContext();

            // note: these required as dependencies of KspAssemblyLoader, which is in turn a dependency of
            // GetAssemblyFileLocation, so they need to be initialized here instead of the more logical
            // CoreContext
            injectionBinder.Bind<ILoadedAssemblyInstaller>().To<LoadedAssemblyInstaller>()
                .ToSingleton()
                .CrossContext();

            injectionBinder.Bind<IReeperAssemblyFactory>()
                .To(new ReeperAssemblyFactory("reloadable", "mdb"))
                .CrossContext();

            injectionBinder.Bind<IUrlDir>().To(new KSPUrlDir(injectionBinder.GetInstance<IUrlDirProvider>().Get())).CrossContext();
            injectionBinder.Bind<IFileSystemFactory>().To<KSPFileSystemFactory>().ToSingleton().CrossContext();

            var ourLocation = GetAssemblyFileLocation(Assembly.GetExecutingAssembly());

            injectionBinder.Bind<IDirectory>().To(ourLocation.Directory).CrossContext();
            injectionBinder.Bind<IDirectory>().To(ourLocation.Directory).ToName(DirectoryKey.Core).CrossContext();
            injectionBinder.Bind<IDirectory>().To(injectionBinder.GetInstance<IFileSystemFactory>().GameData).ToName(DirectoryKey.GameData).CrossContext();
            injectionBinder.Bind<IFile>().To(ourLocation);

            injectionBinder.Bind<IResourceRepository>()
                .To(ConfigureResourceRepository(injectionBinder.GetInstance<IDirectory>()))
                .CrossContext();

            injectionBinder.Bind<IConfigNodeSerializer>().To(ConfigureConfigNodeSerializer()).CrossContext();
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
                                .Where(name => name.StartsWith(s, StringComparison.InvariantCulture))
                                .ToList();

                            return resourceNames.Count == 1 ? resourceNames.First() : s;
                        }, currentAssemblyResource)
                        )));
        }


        private static IConfigNodeSerializer ConfigureConfigNodeSerializer()
        {
            var assembliesToScanForSurrogates = new[]
            {typeof (IConfigNodeSerializer).Assembly, Assembly.GetExecutingAssembly()}
            .Distinct() // in release mode, ReeperCommon gets mashed into executing assembly
            .ToArray();

            var supportedTypeQuery = new GetSurrogateSupportedTypes();
            var surrogateQuery = new GetSerializationSurrogates(supportedTypeQuery);
            var serializableFieldQuery = new GetSerializableFieldsRecursiveType();

            var standardSerializerSelector =
                new SerializerSelector(
                    new CompositeSurrogateProvider(
                        new GenericSurrogateProvider(surrogateQuery, supportedTypeQuery, assembliesToScanForSurrogates),
                        new SurrogateProvider(surrogateQuery, supportedTypeQuery, assembliesToScanForSurrogates)));


            var preferNativeSelector = new PreferNativeSerializer(standardSerializerSelector);

            var selectorOrder = new CompositeSerializerSelector(
                preferNativeSelector,          // always prefer native serializer first 
                standardSerializerSelector);   // otherwise, find any surrogate

            
            var includePersistentFieldsSelector = new SerializerSelectorDecorator(
                selectorOrder,
                s => Maybe<IConfigNodeItemSerializer>.With(new FieldSerializer(s, serializableFieldQuery)));

            return new ConfigNodeSerializer(includePersistentFieldsSelector);
        }


        private IFile GetAssemblyFileLocation(Assembly target)
        {
            if (target == null) throw new ArgumentNullException("target");

            var mf = injectionBinder.GetInstance<IGetAssemblyFileLocation>().Get(target);

            if (!mf.Any()) throw new AssemblyFileLocationNotFoundException(target);
            return mf.Single();
        }
    }
}

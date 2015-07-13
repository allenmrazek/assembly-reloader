using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Config;
using AssemblyReloader.Config.Names;
using AssemblyReloader.FileSystem;
using AssemblyReloader.StrangeIoC.extensions.context.api;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Providers;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using ReeperCommon.Serialization;
using UnityEngine;

namespace AssemblyReloader.Common
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

            var ourLocation =
                injectionBinder.GetInstance<IGetAssemblyFileLocation>().Get(Assembly.GetExecutingAssembly());

            if (!ourLocation.Any())
                throw new Exception("Failed to locate IFile for AssemblyReloader");

            injectionBinder.Bind<IDirectory>().To(ourLocation.Single().Directory).CrossContext();
            injectionBinder.Bind<IDirectory>().To(ourLocation.Single().Directory).ToName(IDirectoryKeys.Core).CrossContext();
            injectionBinder.Bind<IDirectory>().To(injectionBinder.GetInstance<IFileSystemFactory>().GameData).ToName(IDirectoryKeys.GameData).CrossContext();

            
            var serializerSelector =
                new DefaultConfigNodeItemSerializerSelector(new SurrogateProvider(new[] 
                    { 
                        Assembly.GetExecutingAssembly(), 
                        typeof(IConfigNodeSerializer).Assembly 
                    }));

            injectionBinder.Bind<IConfigNodeSerializer>().To(
                new ConfigNodeSerializer(serializerSelector, new GetSerializableFieldsRecursiveType())).CrossContext();

            injectionBinder.Bind<IResourceRepository>()
                .To(ConfigureResourceRepository(injectionBinder.GetInstance<IDirectory>()))
                .CrossContext();


            // regular bindings
            injectionBinder.Bind<IFile>().To(ourLocation.Single());

        }


        private IResourceRepository ConfigureResourceRepository(IDirectory dllDirectory)
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
    }
}

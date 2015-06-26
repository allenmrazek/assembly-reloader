using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Controllers;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;
using KSPAchievements;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.CompositeRoot
{
    public class ReloadablePluginFactory
    {
        private readonly IPluginConfigurationProvider _configProvider;

        public ReloadablePluginFactory([NotNull] IPluginConfigurationProvider configProvider)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            _configProvider = configProvider;
        }


        IReloadablePlugin Create(IFile location)
        {
            var pluginConfiguration = _configProvider.Get(location);
            //var assemblyLoader = new Loaders.AssemblyLoader(new AssemblyProvider(new AssemblyDefinitionFromDiskReader(

            //log.Normal("Loading " + file.Name);
            //injectionBinder.Bind<IFile>().ToValue(file);
            //injectionBinder.Bind<IDirectory>().ToValue(file.Directory);
            //injectionBinder.Bind<PluginConfiguration>()
            //    .ToValue(injectionBinder.GetInstance<IPluginConfigurationProvider>().Get(file));
            //injectionBinder.Bind<IAssemblyDefinitionWeaver>().To(ConfigureDefinitionWeaver(
            //    file, injectionBinder.GetInstance<PluginConfiguration>(), injectionBinder.GetInstance<ILog>().CreateTag("Weaver")));


            //var r = injectionBinder.GetInstance<IReloadablePlugin>();



            //r.Load();


            //injectionBinder.Unbind<IAssemblyDefinitionWeaver>();
            //injectionBinder.Unbind<IDirectory>();
            //injectionBinder.Unbind<IFile>();
            //injectionBinder.Unbind<PluginConfiguration>();

            //return r;
            return null;
        }
    }
}

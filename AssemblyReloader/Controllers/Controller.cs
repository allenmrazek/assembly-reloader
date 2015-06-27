using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Gui;
using ReeperCommon.Logging;

namespace AssemblyReloader.Controllers
{
    public class Controller : IController
    {
        private readonly IModel _model;
        private readonly ILog _log;


        public Controller([NotNull] IModel model, [NotNull] ILog log)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (log == null) throw new ArgumentNullException("log");

            _model = model;
            _log = log;
        }


        public void Reload([NotNull] IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            _log.Normal("Reloading " + plugin.Name + " + from " + plugin.Location.Url);

            _model.Reload(plugin);

            //var target = GetReloadablePlugin(plugin);
            //if (!target.Any())
            //    throw new Exception("Could not find plugin associated with " + plugin.Name);

            //Reload(target.Single(), plugin);
        }


        public void SaveConfiguration()
        {
            //_saveConfiguration.Execute();
        }


        public void SavePluginConfiguration(IPluginInfo plugin)
        {
            //_savePluginConfiguration.Execute(plugin);
        }


        //private void Reload([NotNull] IReloadablePlugin plugin, [NotNull] IPluginInfo info)
        //{
        //    if (plugin == null) throw new ArgumentNullException("plugin");
        //    if (info == null) throw new ArgumentNullException("info");

        //    try
        //    {
        //        plugin.Unload();
        //    }
        //    catch (Exception e)
        //    {
        //        // todo: spawn popup message?
        //        _log.Error("Error while unloading plugin " + info.Name + ": " + e);
        //        throw;
        //    }


        //    try
        //    {
        //        plugin.Load();
        //    }
        //    catch (Exception e)
        //    {
        //        // todo: spawn popup message?
        //        _log.Error("Error while loading plugin " + info.Name + ": " + e);
        //        throw;
        //    }
        //}


        //private Maybe<IReloadablePlugin> GetReloadablePlugin([NotNull] IPluginInfo info)
        //{
        //    if (info == null) throw new ArgumentNullException("info");

        //    IReloadablePlugin target;

        //    return _plugins.TryGetValue(info, out target)
        //        ? Maybe<IReloadablePlugin>.With(target)
        //        : Maybe<IReloadablePlugin>.None;

        //}
    }
}

using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
// ReSharper disable once UnusedMember.Global
    public class ScenarioModuleConfigNodeRepository : DictionaryQueue<Type, ConfigNode>, IScenarioModuleConfigNodeRepository
    {
        //private readonly DictionaryQueue<Type, ConfigNode> _items = new DictionaryQueue<Type, ConfigNode>();

        //public void Store(Type smType, IProtoScenarioModule psm)
        //{
        //    if (psm == null) throw new ArgumentNullException("psm");

        //    Store(smType, psm.GetData().CreateCopy());
        //}


        //public void Store(Type smType, ConfigNode config)
        //{
        //    if (smType == null) throw new ArgumentNullException("smType");

        //    _items.Store(smType, config);
        //}


        //public Maybe<ConfigNode> Retrieve(Type smType)
        //{
        //    if (smType == null) throw new ArgumentNullException("smType");

        //    return _items.Retrieve(smType);
        //}
    }
}

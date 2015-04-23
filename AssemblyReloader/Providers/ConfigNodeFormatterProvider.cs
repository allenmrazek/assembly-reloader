using System;
using AssemblyReloader.Annotations;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Providers
{
    public class ConfigNodeFormatterProvider : IConfigNodeFormatterProvider
    {
        private readonly ISurrogateSelector _surrogateSelector;
        private readonly IFieldInfoQuery _serializableFieldQuery;

        public ConfigNodeFormatterProvider(
            [NotNull] ISurrogateSelector surrogateSelector, 
            [NotNull] IFieldInfoQuery serializableFieldQuery)
        {
            if (surrogateSelector == null) throw new ArgumentNullException("surrogateSelector");
            if (serializableFieldQuery == null) throw new ArgumentNullException("serializableFieldQuery");

            _surrogateSelector = surrogateSelector;
            _serializableFieldQuery = serializableFieldQuery;
        }


        public IConfigNodeFormatter Get()
        {
            return new ConfigNodeFormatter(_surrogateSelector, _serializableFieldQuery);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Properties;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Queries
{
    public class CompositeFieldInfoQuery : IFieldInfoQuery
    {
        private readonly IFieldInfoQuery[] _queries;

        public CompositeFieldInfoQuery([NotNull] params IFieldInfoQuery[] queries)
        {
            if (queries == null) throw new ArgumentNullException("queries");
            _queries = queries;
        }

        public IEnumerable<FieldInfo> Get(object target)
        {
            return _queries.SelectMany(q => q.Get(target));
        }
    }
}

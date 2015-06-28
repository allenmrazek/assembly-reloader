using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Properties;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Queries
{
    public class GetFieldInfoComposite : IGetFieldInfo
    {
        private readonly IGetFieldInfo[] _queries;

        public GetFieldInfoComposite([NotNull] params IGetFieldInfo[] queries)
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

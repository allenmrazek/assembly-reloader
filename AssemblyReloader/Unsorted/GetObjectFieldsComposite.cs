using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Properties;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Unsorted
{
    public class GetObjectFieldsComposite : IGetObjectFields
    {
        private readonly IGetObjectFields[] _queries;

        public GetObjectFieldsComposite([NotNull] params IGetObjectFields[] queries)
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

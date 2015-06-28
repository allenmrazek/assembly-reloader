using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Properties;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Queries
{
    public class FieldsMarkedWithAttributeQuery<T> : IFieldInfoQuery where T:Attribute
    {
        private readonly BindingFlags _fieldFlags;

        public FieldsMarkedWithAttributeQuery(BindingFlags fieldFlags)
        {
            _fieldFlags = fieldFlags;
        }


        public IEnumerable<FieldInfo> Get([NotNull] object target)
        {
            if (target == null) throw new ArgumentNullException("target");

            return
                target.GetType().GetFields(_fieldFlags).Where(fi => fi.GetCustomAttributes(true).Any(attr => attr is T));
        }
    }
}

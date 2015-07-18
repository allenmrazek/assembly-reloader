using System;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class HelperTypeParameters
    {
        public string Namespace { get; private set; }
        public string TypeName { get; private set; }

        public HelperTypeParameters(string @namespace, string typeName)
        {
            if (@namespace == null) throw new ArgumentNullException("namespace");
            if (typeName == null) throw new ArgumentNullException("typeName");

            Namespace = @namespace;
            TypeName = typeName;
        }
    }
}

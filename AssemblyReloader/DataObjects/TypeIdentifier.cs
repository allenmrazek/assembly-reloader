using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.DataObjects
{
    public class TypeIdentifier : ITypeIdentifier
    {
        public TypeIdentifier(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name must contain a value");

            if (name.Any(c => c == '.' || c == '/' || c == '\\'))
                throw new ArgumentException(name + " is an invalid identifier");

            Identifier = name;
        }

        public string Identifier { get; private set; }
    }
}

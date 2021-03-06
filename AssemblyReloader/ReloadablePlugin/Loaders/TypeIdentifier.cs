using System;
using System.Linq;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class TypeIdentifier : IEquatable<TypeIdentifier>
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


        public bool Equals(TypeIdentifier other)
        {
            return Identifier == other.Identifier;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }


        public override string ToString()
        {
            return "TypeIdentifier: " + Identifier;
        }
    }
}

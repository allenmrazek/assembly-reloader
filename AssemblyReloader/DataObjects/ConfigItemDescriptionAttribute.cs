using System;

namespace AssemblyReloader.DataObjects
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ConfigItemDescriptionAttribute : Attribute
    {
        public readonly string Description = "No description";

        public ConfigItemDescriptionAttribute(string desc)
        {
            Description = desc;
        }
    }
}

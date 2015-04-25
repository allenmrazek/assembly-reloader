using System;

namespace AssemblyReloader.DataObjects
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ConfigItemDescriptionAttribute : Attribute
    {
        public string Description = "No description";

        public ConfigItemDescriptionAttribute(string desc)
        {
            Description = desc;
        }
    }
}

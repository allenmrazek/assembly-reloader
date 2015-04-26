using System;

namespace AssemblyReloader.DataObjects
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PanelCategoryAttribute : Attribute
    {
        public enum CategoryType
        {
            Addon,
            PartModule,
            ScenarioModule,
            IntermediateLanguage
        }

        public readonly CategoryType Category = CategoryType.Addon;

        public PanelCategoryAttribute(CategoryType cat)
        {
            Category = cat;
        }
    }
}

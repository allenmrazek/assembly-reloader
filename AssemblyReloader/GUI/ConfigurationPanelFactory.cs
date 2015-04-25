using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.Serialization;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class ConfigurationPanelFactory : IConfigurationPanelFactory
    {
        private readonly IExpandablePanelFactory _expandablePanelFactory;
        private readonly IFieldInfoQuery _configPanelFieldQuery;
        private const float ExpandablePanelContentMarginOffset = 12f;

        private class ExpandablePanelToggleDrawObject
        {
            private readonly Configuration _configuration;
            private readonly Dictionary<FieldInfo, string> _content;

            public ExpandablePanelToggleDrawObject(
                [NotNull] Configuration configuration, 
                [NotNull] Dictionary<FieldInfo, string> content)
            {
                if (configuration == null) throw new ArgumentNullException("configuration");
                if (content == null) throw new ArgumentNullException("content");

                _configuration = configuration;
                _content = content;
            }

            public void Draw(IEnumerable<GUILayoutOption> options)
            {
                var keys = _content.Keys;

                foreach (var key in keys)
                    key.SetValue(_configuration, GUILayout.Toggle((bool) key.GetValue(_configuration), _content[key]));
            }
        }


        public ConfigurationPanelFactory(
            [NotNull] IExpandablePanelFactory expandablePanelFactory, [NotNull] IFieldInfoQuery configPanelFieldQuery)
        {
            if (expandablePanelFactory == null) throw new ArgumentNullException("expandablePanelFactory");
            if (configPanelFieldQuery == null) throw new ArgumentNullException("configPanelFieldQuery");

            _expandablePanelFactory = expandablePanelFactory;
            _configPanelFieldQuery = configPanelFieldQuery;
        }


        public IEnumerable<IExpandablePanel> CreatePanelsFor([NotNull] Configuration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            var configurationFields = _configPanelFieldQuery.Get(configuration);

            return (from enumValue in Enum.GetValues(typeof (PanelCategoryAttribute.CategoryType)).Cast<PanelCategoryAttribute.CategoryType>() 
                    let fieldsForThisValue = GetFieldsForPanel(configurationFields, enumValue).ToList() 
                    where fieldsForThisValue.Any() 
                    let drawObject = new ExpandablePanelToggleDrawObject(configuration, fieldsForThisValue.ToDictionary(fi => fi, fi => ((ConfigItemDescriptionAttribute) fi.GetCustomAttributes(true).First(attr => attr is ConfigItemDescriptionAttribute)).Description)) 
                    select _expandablePanelFactory.Create(enumValue.ToString(), ExpandablePanelContentMarginOffset, drawObject.Draw, false));
        }


        private IEnumerable<FieldInfo> GetFieldsForPanel(IEnumerable<FieldInfo> fieldSet, PanelCategoryAttribute.CategoryType cat)
        {
            return
                fieldSet.Where(
                    fi =>
                        ((PanelCategoryAttribute)
                            fi.GetCustomAttributes(true).First(attr => attr is PanelCategoryAttribute)).Category == cat);
        }


        
    }
}

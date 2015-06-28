//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using AssemblyReloader.DataObjects;
//using ReeperCommon.Serialization;

//namespace AssemblyReloader.Queries
//{
//    public class ConfigurationWindowPanelFieldQuery : IGetFieldInfo
//    {
//        public IEnumerable<FieldInfo> Get(object target)
//        {
//            return target.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
//                .Where(
//                    fi =>
//                        fi.GetCustomAttributes(true).Any(attr => attr is PanelCategoryAttribute) &&
//                        fi.GetCustomAttributes(true).Any(attr => attr is ConfigItemDescriptionAttribute) &&
//                        fi.FieldType == typeof(bool)).ToList();
//        }
//    }
//}

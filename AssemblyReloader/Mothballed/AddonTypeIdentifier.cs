using System;
using System.Linq;
using Mono.Cecil;


namespace AssemblyReloader
{
    class AddonTypeIdentifier// : IAddonTypeIdentifier
    {
        //public AddonType IdentifyAssembly(Log log, AssemblyDefinition definition)
        //{
        //    if (definition == null) throw new ArgumentNullException("definition");

        //    definition.MainModule.Types
        //        .Where(ty => ty.IsClass && ty.HasCustomAttributes)
        //        .ToList().ForEach(td =>
        //        {
        //            log.Normal("Found class with attributes: {0}", td.FullName);
        //            td.CustomAttributes.ToList().ForEach(ca => log.Normal("  attr: " + ca.AttributeType.Name)); // KSPAddon
        //        });

        //    return AddonType.Unknown;
        //}
    }
}

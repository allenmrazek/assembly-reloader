extern alias Cecil96;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeReference = Cecil96::Mono.Cecil.TypeReference;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;
using OpCodes = Cecil96::Mono.Cecil.Cil.OpCodes;
using CustomAttribute = Cecil96::Mono.Cecil.CustomAttribute;
using CustomAttributeArgument = Cecil96::Mono.Cecil.CustomAttributeArgument;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once InconsistentNaming
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandReplaceKSPAddonWithReloadableAddon : Command
    {
        private readonly AssemblyDefinition _context;
        private readonly IGetAllTypesInAssemblyDefinition _allTypesInAssembly;
        private readonly ILog _log;

        public CommandReplaceKSPAddonWithReloadableAddon(
            AssemblyDefinition context, 
            IGetAllTypesInAssemblyDefinition allTypesInAssembly,
            ILog log)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (allTypesInAssembly == null) throw new ArgumentNullException("allTypesInAssembly");
            if (log == null) throw new ArgumentNullException("log");

            _context = context;
            _allTypesInAssembly = allTypesInAssembly;
            _log = log;
        }

        public override void Execute()
        {
            // todo: move all this logic out of the command?

            _log.Debug("Replacing KSPAddon attributes with ReloadableAddonAttribute");

            foreach (var decoratedType in GetAddonTypeDefinitions())
            {
                _log.Verbose("Replacing KSPAddon attribute on " + decoratedType.FullName);

                var kspAddon = GetAddonAttribute(decoratedType).Single();
                var reloadableAddon = CreateReloadableAttribute(kspAddon, decoratedType);

                decoratedType.CustomAttributes.Add(reloadableAddon);
                decoratedType.CustomAttributes.Remove(kspAddon);
            }

            _log.Debug("Replacing KSPAddon references with ReloadableAddonAttribute references");

            foreach (var type in _context.Modules.SelectMany(module => module.Types))
                ReplaceKSPAddonReferencesWithReloadableAddon(type);
    
        }



// ReSharper disable once InconsistentNaming
        private void ReplaceKSPAddonReferencesWithReloadableAddon(TypeDefinition type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var kspAddonReference = type.Module.Import(typeof (KSPAddon));
            var reloadableAddonReference = type.Module.Import(typeof (ReloadableAddonAttribute));

            foreach (var method in type.Methods.Where(m => m.HasBody))
            {
                try
                {
                    method.Body.Instructions
                        .Where(inst => inst.OpCode == OpCodes.Ldtoken)
                        .Where(
                            inst =>
                                inst.Operand is TypeReference &&
                                ((TypeReference) inst.Operand).FullName == kspAddonReference.FullName)
                        .ToList()
                        .ForEach(
                            inst =>
                                inst.Operand = reloadableAddonReference);
                }
                catch (Exception e) // todo: handle better
                {
                    _log.Error("Exception occurred while replacing references inside " + method.FullName + ": " + e);
                }
            }
        }


        private static CustomAttribute CreateReloadableAttribute(CustomAttribute addonAttribute, TypeDefinition decoratedType)
        {
            if (!addonAttribute.HasConstructorArguments)
                throw new NoConstructorArgumentFoundException(addonAttribute, decoratedType);

            if (addonAttribute.ConstructorArguments.Count != 2)
                throw new ArgumentException("KSPAddon attribute has incorrect number of arguments");

            var reloadableAddonConstructor =
                    decoratedType.Module.Import(typeof(ReloadableAddonAttribute).GetConstructor(new[] { typeof(KSPAddon.Startup), typeof(bool) }));

            return new CustomAttribute(reloadableAddonConstructor)
            {
                ConstructorArguments =
                    {
                        new CustomAttributeArgument(decoratedType.Module.Import(typeof(KSPAddon.Startup)), 
                            addonAttribute.ConstructorArguments[0].Value),

                        new CustomAttributeArgument(decoratedType.Module.Import(typeof (bool)), 
                            addonAttribute.ConstructorArguments[1].Value)
                    }
            };
        }


        private IEnumerable<TypeDefinition> GetAddonTypeDefinitions()
        {
            return _allTypesInAssembly.Get(_context)
                    .Where(td => td.HasCustomAttributes && GetAddonAttribute(td).Any());
        }


        private static Maybe<CustomAttribute> GetAddonAttribute(TypeDefinition type)
        {
            var addon =
                type.CustomAttributes.FirstOrDefault(ca => ca.AttributeType.FullName == type.Module.Import(typeof (KSPAddon)).FullName);

            return addon != null ? Maybe<CustomAttribute>.With(addon) : Maybe<CustomAttribute>.None;
        }
    }
}

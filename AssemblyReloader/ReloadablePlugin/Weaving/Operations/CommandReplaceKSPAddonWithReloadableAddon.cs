﻿using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once InconsistentNaming
    public class CommandReplaceKSPAddonWithReloadableAddon : Command
    {
        [Inject]
        public AssemblyDefinition Context { get; set; }

        [Inject]
        public ILog Log { get; set; }


        public override void Execute()
        {
            Log.Debug("Replacing KSPAddon attributes with ReloadableAddonAttribute");

            foreach (var decoratedType in GetAddonTypeDefinitions())
            {
                Log.Verbose("Replacing KSPAddon attribute on " + decoratedType.FullName);

                var kspAddon = GetAddonAttribute(decoratedType).Single();
                var reloadableAddon = CreateReloadableAttribute(kspAddon, decoratedType);

                decoratedType.CustomAttributes.Add(reloadableAddon);
                decoratedType.CustomAttributes.Remove(kspAddon);
            }

            Log.Debug("Replacing KSPAddon references with ReloadableAddonAttribute references");

            foreach (var type in Context.Modules.SelectMany(module => module.Types))
                ReplaceKSPAddonReferencesWithReloadableAddon(type);
        }


        private void ReplaceKSPAddonReferencesWithReloadableAddon(TypeDefinition type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var kspAddonReference = type.Module.Import(typeof (KSPAddon));
            var reloadableAddonReference = type.Module.Import(typeof (ReloadableAddonAttribute));

            foreach (var method in type.Methods)
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
        }


        private static CustomAttribute CreateReloadableAttribute(CustomAttribute addonAttribute, TypeDefinition decoratedType)
        {
            if (!addonAttribute.HasConstructorArguments)
                throw new NoConstructorArgumentFoundsException(addonAttribute, decoratedType);

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
            return Context.Modules.SelectMany(module =>
                module.Types
                    .Where(td => td.HasCustomAttributes && GetAddonAttribute(td).Any()));
        }


        private Maybe<CustomAttribute> GetAddonAttribute(TypeDefinition type)
        {
            var addon =
                type.CustomAttributes.FirstOrDefault(ca => ca.AttributeType.FullName == type.Module.Import(typeof (KSPAddon)).FullName);

            return addon != null ? Maybe<CustomAttribute>.With(addon) : Maybe<CustomAttribute>.None;
        }
    }
}

using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewEvents;

namespace SOMD.NewUnitParts
{
    public class UnitPartSecretsofMagicalDiscipline : UnitPart, ISpontaneousConversionHandler, ILearnSpellHandler
    {

        public override void OnPostLoad()
        {
            base.OnPostLoad();
            UpdateConversions();
        }

        public void AddSecretsofMagicalDisciplineSpellList(BlueprintSpellListReference spellList, EntityFact source)
        {
            if (SpellLists.Any(r => r.Source == source && r.SpellList.deserializedGuid == spellList.deserializedGuid)) { return; }
            SpellLists.Add(new SecretsofMagicalDisciplineSpellLists(spellList, source));
            UpdateConversions();
        }

        public void AddSecretsofMagicalDisciplineSpellbook(BlueprintSpellbookReference spellbook, EntityFact source)
        {
            if (Spellbooks.Any(r => r.Source == source)) { return; }
            Spellbooks.Add(new SecretsofMagicalDisciplineSpellListsBooks(spellbook, source));
        }

        public void SetSecretsofMagicalDisciplineResource(BlueprintAbilityResourceReference resource)
        {
            m_Resource = resource;
        }

        public void RemoveEntry(EntityFact source)
        {
            SpellLists.RemoveAll((list) => list.Source == source);
            Spellbooks.RemoveAll((book) => book.Source == source);
            TryRemove();
        }

        private void TryRemove()
        {
            if (!SpellLists.Any() && !Spellbooks.Any()) { this.RemoveSelf(); }
        }

        public void HandleLearnSpell()
        {
            UpdateConversions();
        }

        public void HandleGetConversions(AbilityData ability, ref IEnumerable<AbilityData> conversions)
        {
            var conversionList = conversions.ToList();
            if (!Spellbooks.Any(r => r.Spellbook.deserializedGuid == ability.SpellbookBlueprint?.AssetGuid)) { return; }
            foreach (var spell in GetConversionSpells(ability.SpellLevel))
            {
                AbilityData.AddAbilityUnique(ref conversionList, new SecretsofMagicalDisciplineAbilityData(ability, spell)
                {
                    OverridenResourceLogic = new SecretsofMagicalDisciplineResourceOverride()
                    {
                        m_RequiredResource = m_Resource,
                        cost = 1
                    }
                });
            }
            conversions = conversionList;
        }

        public void UpdateConversions()
        {
            for (int level = 0; level < cachedConversions.Length; level++)
            {
                cachedConversions[level] = SpellLists
                    .Select(list => list.SpellList.Get())
                    .SelectMany(list => list.SpellsByLevel)
                    .Where(spellList => spellList.SpellLevel != 0)
                    .Where(spellList => spellList.SpellLevel == level)
                    .SelectMany(level => level.Spells)

                    .SelectMany(spell => {
                        AbilityVariants variantComponent = spell.GetComponent<AbilityVariants>();
                        if (variantComponent != null)
                        {
                            return variantComponent.Variants.AsEnumerable();
                        }
                        return new BlueprintAbility[] { spell };
                    })
                    .Distinct()
                    .Where(spell => !spell.GetComponent<AbilityShadowSpell>())
                    .Select(spell => spell.ToReference<BlueprintAbilityReference>())
                    .ToList();
            }
        }

        public IEnumerable<BlueprintAbility> GetConversionSpells(int level)
        {
            return cachedConversions[Math.Max(0, Math.Min(cachedConversions.Length - 1, level))].Select(spell => spell.Get());
        }
        private readonly List<BlueprintAbilityReference>[] cachedConversions = new List<BlueprintAbilityReference>[10];
        private readonly List<SecretsofMagicalDisciplineSpellLists> SpellLists = new();
        private readonly List<SecretsofMagicalDisciplineSpellListsBooks> Spellbooks = new();
        private BlueprintAbilityResourceReference m_Resource;

        internal class SecretsofMagicalDisciplineSpellLists
        {
            [JsonProperty]
            public BlueprintSpellListReference SpellList;
            [JsonProperty]
            public EntityFactRef Source;

            public SecretsofMagicalDisciplineSpellLists() { }
            public SecretsofMagicalDisciplineSpellLists(BlueprintSpellListReference spellList, EntityFact source)
            {
                SpellList = spellList;
                Source = source;
            }
        }
        internal class SecretsofMagicalDisciplineSpellListsBooks
        {
            [JsonProperty]
            public BlueprintSpellbookReference Spellbook;
            [JsonProperty]
            public EntityFactRef Source;

            public SecretsofMagicalDisciplineSpellListsBooks() { }
            public SecretsofMagicalDisciplineSpellListsBooks(BlueprintSpellbookReference spellbook, EntityFact source)
            {
                Spellbook = spellbook;
                Source = source;
            }
        }

        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.RequireFullRoundAction), MethodType.Getter)]
        private static class AbilityData_RuntimeActionType_QuickChannel_Patch
        {
            static void Postfix(AbilityData __instance, ref bool __result)
            {
                switch (__instance)
                {
                    case SecretsofMagicalDisciplineAbilityData abilityData:
                        __result = abilityData.RequireFullRoundAction;
                        break;
                }
            }
        }

        internal class SecretsofMagicalDisciplineAbilityData : AbilityData
        {

            public SecretsofMagicalDisciplineAbilityData() : base() { }
            public SecretsofMagicalDisciplineAbilityData(
                BlueprintAbility blueprint,
                UnitDescriptor caster,
                [CanBeNull] Ability fact,
                [CanBeNull] BlueprintSpellbook spellbookBlueprint) : base(blueprint, caster, fact, spellbookBlueprint)
            {
            }

            public SecretsofMagicalDisciplineAbilityData(AbilityData other, BlueprintAbility replaceBlueprint) : this(replaceBlueprint ?? other.Blueprint, other.Caster, other.Fact, other.SpellbookBlueprint)
            {
                this.MetamagicData = null;
                this.m_ConvertedFrom = other;
            }

            public new bool RequireFullRoundAction
            {
                get
                {
                    return this.MetamagicData != null ? !this.MetamagicData.Has(Metamagic.Quicken) : true;
                }
            }
        }
        internal class SecretsofMagicalDisciplineResourceOverride : IAbilityResourceLogic
        {
            public SecretsofMagicalDisciplineResourceOverride() : base() { }

            public BlueprintAbilityResource RequiredResource => m_RequiredResource.Get();

            public bool IsSpendResource => true;

            public int CalculateCost(AbilityData ability)
            {
                return cost;
            }

            public void Spend(AbilityData ability)
            {
                UnitEntityData unit = ability.Caster.Unit;
                if (unit == null)
                {
                    PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                    return;
                }
                if (unit.Blueprint.IsCheater)
                {
                    return;
                }
                unit.Descriptor.Resources.Spend(this.RequiredResource, cost);
            }

            [JsonProperty]
            public BlueprintAbilityResourceReference m_RequiredResource;
            [JsonProperty]
            public Metamagic addedMetamagic;
            [JsonProperty]
            public int cost = 1;
        }
    }
}

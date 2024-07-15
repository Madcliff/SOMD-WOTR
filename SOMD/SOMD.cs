using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TabletopTweaks.Base.NewContent.Feats;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using TabletopTweaks.Core.Utilities;
using System.Runtime.Remoting.Contexts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using Kingmaker.Blueprints.Root;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using static TabletopTweaks.Core.Utilities.SpellTools;
using Kingmaker.Designers.Mechanics.Buffs;
using System.ComponentModel;
using static Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite;
using BlueprintCore.Utils;
using static SOMD.Main;
using SOMD.NewComponents;


namespace SOMD

{
    static class SecretsofMagicalDiscipline
    {
        private static readonly BlueprintFeature ClericProficiencies = BlueprintTools.GetBlueprint<BlueprintFeature>("8c971173613282844888dc20d572cfc9");
        private static readonly BlueprintFeatureSelection ChannelEnergySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("d332c1748445e8f4f9e92763123e31bd");
        private static readonly BlueprintFeatureSelection DomainsSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("48525e5da45c9c243a343fc6545dbdb9");
        private static readonly BlueprintFeatureSelection SecondDomainsSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("43281c3d7fe18cc4d91928395837cd1e");

        private static readonly BlueprintSpellList ClericSpells = SpellListRefs.ClericSpellList.Reference.Get();
        private static readonly BlueprintSpellList DruidSpells = SpellListRefs.DruidSpellList.Reference.Get();
        private static readonly BlueprintSpellList PaladinSpells = SpellListRefs.PaladinSpellList.Reference.Get();

        private static readonly BlueprintCharacterClass MysticTheurgeClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>("0920ea7e4fd7a404282e3d8b0ac41838");
        private static readonly BlueprintFeatureSelection MysticTheurgeDivineSpellbookSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("7cd057944ce7896479717778330a4933");
        private static readonly BlueprintProgression MysticTheurgeClericProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("8bac42667e6f67047acbcbd668cf2029");


        //add features
        public static LevelEntry AddFeatures(this LevelEntry levelEntry, params BlueprintFeatureBase[] features)
        {
            levelEntry.SetFeatures(levelEntry.Features.Concat(features));

            return levelEntry;
        }

        public static void AddFeatures(this BlueprintProgression progression, int level, params BlueprintFeatureBase[] features)
        {
            var entry = progression.LevelEntries.FirstOrDefault(e => e.Level == level);

            if (entry is null)
            {
                entry = new LevelEntry { Level = level };
                progression.LevelEntries = progression.LevelEntries.Append(entry).ToArray();
            }

            entry.AddFeatures(features);
        }






        public static void addSecretsofMagicalDiscipline()
        {


            var LoremasterProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("2bcd2330cc2c5a747968a8c782d4fa0a");



            var LoremasterSecretsofMagicalDisciplineResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(SOMDContext, "LoremasterSecretsofMagicalDisciplineResource", bp => {
                bp.m_MaxAmount = new BlueprintAbilityResource.Amount()
                {
                    m_Class = new BlueprintCharacterClassReference[0],
                    m_ClassDiv = new BlueprintCharacterClassReference[0],
                    m_Archetypes = new BlueprintArchetypeReference[0],
                    m_ArchetypesDiv = new BlueprintArchetypeReference[0],
                    BaseValue = 1,
                    LevelIncrease = 0,
                    IncreasedByLevel = false,
                    IncreasedByStat = false
                };
            });



            var LoremasterSecretsofMagicalDiscipline = Helpers.CreateBlueprint<BlueprintFeature>(SOMDContext, "LoremasterSecretsofMagicalDiscipline", bp => {
                bp.SetName(SOMDContext, "Loremaster Secrets of Magical Discipline");
                bp.SetDescription(SOMDContext, "A powerful ability to cast any spell as one wills");
                bp.IsClassFeature = true;
                bp.Ranks = 1;
                bp.AddComponent<SecretsofMagicalDisciplineComponent>(c => {
                    c.m_Resource = LoremasterSecretsofMagicalDisciplineResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_SpellLists = new BlueprintSpellListReference[] {
                        SpellTools.SpellList.WizardSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.ClericSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.DruidSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.WitchSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.BardSpellList.ToReference<BlueprintSpellListReference>()
                    };
                    c.m_Spellbook = SpellTools.Spellbook.WizardSpellbook.ToReference<BlueprintSpellbookReference>();
                });
                bp.AddComponent<SecretsofMagicalDisciplineClericComponent>(c => {
                    c.m_Resource = LoremasterSecretsofMagicalDisciplineResource.ToReference<BlueprintAbilityResourceReference>();
                    c.m_SpellLists = new BlueprintSpellListReference[] {
                        SpellTools.SpellList.WizardSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.ClericSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.DruidSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.WitchSpellList.ToReference<BlueprintSpellListReference>(),
                        SpellTools.SpellList.BardSpellList.ToReference<BlueprintSpellListReference>()
                    };
                    c.m_Spellbook = SpellTools.Spellbook.ClericSpellbook.ToReference<BlueprintSpellbookReference>();
                });
                bp.AddComponent<AddAbilityResources>(c => {
                    c.m_Resource = LoremasterSecretsofMagicalDisciplineResource.ToReference<BlueprintAbilityResourceReference>();
                    c.RestoreAmount = true;
                });
            });

            var LoremasterSecretsofMagicalDisciplineExtraUse = FeatTools.CreateExtraResourceFeat(SOMDContext, "LoremasterSecretsofMagicalDisciplineExtraUse", LoremasterSecretsofMagicalDisciplineResource, 1, bp => {
                bp.SetName(SOMDContext, "Loremaster Secrets of Magical Discipline Extra Use");
                bp.SetDescription(SOMDContext, "You gain 1 more uses of the Secrets of Magical Discipline ability " +
                    "increases by that amount.\nYou can take this feat multiple times. Its effects stack.");
                bp.AddPrerequisiteFeature(LoremasterSecretsofMagicalDiscipline, GroupType.Any);
                bp.Ranks = 1000;
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = LoremasterSecretsofMagicalDisciplineResource.ToReference<BlueprintAbilityResourceReference>();
                    c.Value = 1;
                });
            });

            var itembondfeature = BlueprintTools.GetBlueprint<BlueprintFeature>("2fb5e65bd57caa943b45ee32d825e9b9");
            var ItemBondResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("fbc6de6f8be4fad47b8e3ec148de98c2");

            var UniversalistSchoolFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("38aab7423d96de84d8e6ab2cdbccce63");
            var UniversalistSchoolGreaterResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("42fd5b455f986f94293b15b13f38d6a5");

            var FocusedSpellFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("ee18ad78a7b046e789a763ab860cd50a");
            var FocusedSpellResource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("32947324b3054fdea55092735de9c82c");






            var UniversalistSchoolBonuses = FeatTools.CreateExtraResourceFeat(SOMDContext, "UniversalistSchoolBonuses", ItemBondResource, 5, bp => {
                bp.SetName(SOMDContext, "Spell Master Abilities Extra Use");
                bp.SetDescription(SOMDContext, "You gain 5 more uses of the Spell Master Archetype abilities " +
                    "increases by that amount.\nYou can take this feat multiple times. Its effects stack.");
                bp.AddPrerequisiteFeature(itembondfeature, GroupType.Any);
                bp.Ranks = 1000;

                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = FocusedSpellResource;
                    c.Value = 5;
                });
                bp.AddComponent<IncreaseResourceAmount>(c => {
                    c.m_Resource = UniversalistSchoolGreaterResource;
                    c.Value = 5;
                });
            });




            FeatTools.AddAsFeat(LoremasterSecretsofMagicalDiscipline);
            FeatTools.AddAsFeat(LoremasterSecretsofMagicalDisciplineExtraUse);
            FeatTools.AddAsFeat(UniversalistSchoolBonuses);
            FeatTools.AddAsMythicAbility(UniversalistSchoolBonuses);
            FeatTools.AddAsMythicFeat(UniversalistSchoolBonuses);
            FeatTools.AddAsMythicAbility(LoremasterSecretsofMagicalDisciplineExtraUse);
            FeatTools.AddAsMythicFeat(LoremasterSecretsofMagicalDisciplineExtraUse);



            var ArcaneMetamastery = BlueprintTools.GetModBlueprintReference<BlueprintFeatureReference>(SOMDContext, "ArcaneMetamastery");
            var ArcaneMetamasteryResource = BlueprintTools.GetModBlueprintReference<BlueprintAbilityResourceReference>(SOMDContext, "ArcaneMetamasteryResource");

            var ArcaneMetmasteryUse = FeatTools.CreateExtraResourceFeat(SOMDContext, "ArcaneMetmasteryUse", ArcaneMetamasteryResource, 5, bp => {
                bp.SetName(SOMDContext, "Arcane Metamastery Extra Uses");
                bp.SetDescription(SOMDContext, "You gain 5 more uses of the Arcane Metamastery ability " +
                    "increases by that amount.\nYou can take this feat multiple times. Its effects stack.");
                bp.AddPrerequisiteFeature(ArcaneMetamastery, GroupType.Any);
                bp.Ranks = 1000;
            });


            FeatTools.AddAsFeat(ArcaneMetmasteryUse);
            FeatTools.AddAsMythicAbility(ArcaneMetmasteryUse);


            var SpellMasterArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("bb571f1b6cea462e99db7893685a0982");

            var WizardFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("8c3102c2ff3b69444b139a98521a4899");

            var BonusFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            var MythicAbilitySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("ba0e5a900b775be4a99702f1ed08914d");
            var MythicFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("9ee0f6745f555484299b0a1563b99d81");


            var SpecialisationSchoolUniversalistProgression = BlueprintTools.GetBlueprint<BlueprintProgression>("0933849149cfc9244ac05d6a5b57fd80");

            SpecialisationSchoolUniversalistProgression.AddFeatures(1, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(2, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(3, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(4, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(5, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(6, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(7, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(8, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(9, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(10, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(11, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(12, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(13, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(14, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(15, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(16, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(17, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(18, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(19, BonusFeatSelection, BonusFeatSelection);
            SpecialisationSchoolUniversalistProgression.AddFeatures(20, BonusFeatSelection, BonusFeatSelection);







            var mythicselection = Helpers.CreateCopy<BlueprintFeatureSelection>(MythicAbilitySelection);
            mythicselection.Group = FeatureGroup.Feat;
            mythicselection.Group2 = FeatureGroup.TricksterFeat;


            var SpellsCheatsProgression = Helpers.CreateBlueprint<BlueprintProgression>(SOMDContext, "SpellCheatsProgression", bp => {
                bp.SetName(SOMDContext, "Mythic Soul Powers");
                bp.SetDescription(SOMDContext, "The powers of the soul allow an anticipation of a future mysthic status");
                bp.Groups = SpecialisationSchoolUniversalistProgression.Groups;
                bp.IsClassFeature = true;
                bp.GiveFeaturesForPreviousLevels = true;
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(2, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(3, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(4, BonusFeatSelection, BonusFeatSelection,  MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(5, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(6, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(7, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(8, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(9, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(10, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(11, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(12, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(13, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(14, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(15, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(16, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(17, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(18, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(19, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection),
                    Helpers.CreateLevelEntry(20, BonusFeatSelection, BonusFeatSelection, MythicAbilitySelection, MythicAbilitySelection, MythicFeatSelection, MythicFeatSelection),
                };
            });




            FeatTools.AddAsFeat(SpellsCheatsProgression);

            var EcclesitheurgeArchetype = BlueprintTools.GetBlueprint<BlueprintArchetype>("472af8cb3de628f4a805dc4a038971bc");


            var clericspells = SpellTools.SpellList.ClericSpellList;

            var ArchivistSpellList = SpellListConfigurator.New("ArchivistSpellList", "0795f6da-24aa-49c5-b847-e86a8615aec8")
                .AddToSpellsByLevel(ClericSpells.SpellsByLevel)
            .Configure();

            var list = new List<BlueprintSpellList>() {
                SpellListRefs.DruidSpellList.Reference.Get(),
                SpellListRefs.InquisitorSpellList.Reference.Get(),
                SpellListRefs.PaladinSpellList.Reference.Get(),
                SpellListRefs.RangerSpellList.Reference.Get(),
                SpellListRefs.ShamanSpelllist.Reference.Get(),
                SpellListRefs.WarpriestSpelllist.Reference.Get()
            };
            var domainlist = FeatureSelectionRefs.DomainsSelection.Reference.Get().m_AllFeatures;
            foreach (var feature in domainlist)
            {
                var spelllist = feature.Get().GetComponent<LearnSpellList>().m_SpellList;
                list.Add(spelllist);
            }
            foreach (var level in ArchivistSpellList.SpellsByLevel)
            {
                foreach (var clazz in list)
                {
                    foreach (var spell in clazz.SpellsByLevel[level.SpellLevel].Spells)
                    {
                        level.m_Spells.Add(spell.ToReference<BlueprintAbilityReference>());
                    }
                }
            }



            var ArchivistSpellbook = ClassTools.Classes.ClericClass.Spellbook.CreateCopy(SOMDContext, "ArchivistSpellbook", bp => {
                bp.CastingAttribute = StatType.Intelligence;
                bp.m_SpellList = ArchivistSpellList.ToReference<BlueprintSpellListReference>();
                bp.m_SpellsKnown = ClassTools.Classes.WizardClass.Spellbook.m_SpellsKnown;
                bp.AllSpellsKnown = ClassTools.Classes.WizardClass.Spellbook.AllSpellsKnown;
                bp.SpellsPerLevel = ClassTools.Classes.WizardClass.Spellbook.SpellsPerLevel;
                bp.m_SpellsPerDay = ClassTools.Classes.WizardClass.Spellbook.m_SpellsPerDay;
                bp.CanCopyScrolls = ClassTools.Classes.WizardClass.Spellbook.CanCopyScrolls;
                SpellTools.Spellbook.AllSpellbooks.Add(bp);

            });




            var ArchivistArchetype = Helpers.CreateBlueprint<BlueprintArchetype>(SOMDContext, "ArchivistArchetype", bp => {
                bp.SetName(SOMDContext, "Archivist");
                bp.SetDescription(SOMDContext, "While most clerics who fall out of favor with their deities " +
                    "simply lose their divine connection and the powers it granted, a few continue to go through the motions of prayer and obedience, persisting " +
                    "in the habits of faith even when their faith itself has faded. Among these, an even smaller number find that while their original deity no " +
                    "longer answers their prayers, something else does: an unknown entity or force of the universe channeling its power through a trained and " +
                    "practicing vessel.");
                bp.m_ReplaceSpellbook = ArchivistSpellbook.ToReference<BlueprintSpellbookReference>();
                bp.RemoveFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1,
                        ChannelEnergySelection,
                        DomainsSelection,
                        SecondDomainsSelection
                    ),
                };
                bp.AddFeatures = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1,
                        ClericProficiencies
                    ),
                };
            });
            ClassTools.Classes.ClericClass.m_Archetypes = ClassTools.Classes.ClericClass.m_Archetypes.AppendToArray(ArchivistArchetype.ToReference<BlueprintArchetypeReference>());


            ClassTools.Classes.WizardClass.TemporaryContext(bp => {
            });

            var MysticTheurgeArchivistLevelUp = Helpers.CreateBlueprint<BlueprintFeature>(SOMDContext, "MysticTheurgeArchivistLevelUp", bp => {
                bp.SetName(SOMDContext, "Archivist");
                bp.SetDescription(SOMDContext, "At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 10;
                bp.HideInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.AddComponent<AddSpellbookLevel>(c => {
                    c.m_Spellbook = ArchivistSpellbook.ToReference<BlueprintSpellbookReference>();
                });
            });
            var MysticTheurgeArchivistProgression = Helpers.CreateBlueprint<BlueprintProgression>(SOMDContext, "MysticTheurgeArchivistProgression", bp => {
                bp.SetName(SOMDContext, "Archivist");
                bp.SetDescription(SOMDContext, "At 1st level, the mystic theurge selects a divine {g|Encyclopedia:Spell}spellcasting{/g} class she belonged to before adding the prestige class. " +
                    "When a new mystic theurge level is gained, the character gains new spells per day and new spells known as if she had also gained a level in that spellcasting class.");
                bp.Ranks = 1;
                bp.HideInUI = true;
                bp.HideInCharacterSheetAndLevelUp = true;
                bp.HideNotAvailibleInUI = true;
                bp.Groups = new FeatureGroup[] { FeatureGroup.MysticTheurgeDivineSpellbook };
                bp.IsClassFeature = true;
                bp.m_Classes = new BlueprintProgression.ClassWithLevel[] {
                    new BlueprintProgression.ClassWithLevel{
                        m_Class = MysticTheurgeClass.ToReference<BlueprintCharacterClassReference>()
                    }
                };
                bp.LevelEntries = new LevelEntry[] {
                    Helpers.CreateLevelEntry(1, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(2, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(3, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(4, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(5, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(6, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(7, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(8, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(9, MysticTheurgeArchivistLevelUp),
                    Helpers.CreateLevelEntry(10, MysticTheurgeArchivistLevelUp)
                };
                bp.AddPrerequisite<PrerequisiteClassSpellLevel>(c => {
                    c.m_CharacterClass = ClassTools.Classes.ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.RequiredSpellLevel = 2;
                });
                bp.AddComponent<MysticTheurgeSpellbook>(c => {
                    c.m_CharacterClass = ClassTools.Classes.ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_MysticTheurge = MysticTheurgeClass.ToReference<BlueprintCharacterClassReference>();
                });
                bp.AddPrerequisite<PrerequisiteArchetypeLevel>(c => {
                    c.m_CharacterClass = ClassTools.Classes.ClericClass.ToReference<BlueprintCharacterClassReference>();
                    c.m_Archetype = ArchivistArchetype.ToReference<BlueprintArchetypeReference>();
                    c.Level = 1;
                });
            });
        }
    }
}

using System.Collections.Generic;

namespace PBS.Databases
{
    public partial class Effects
    {
        // ---FILTER EFFECTS---
        /// <summary>
        /// Collection of all Filter Effects that can be used to restrict other effects.
        /// </summary>
        public class Filter
        {
            public class FilterEffect
            {
                /// <summary>
                /// The type of effect that this is.
                /// </summary>
                public FilterEffectType effectType;
                /// <summary>
                /// Inverts the checks of this filter if true.
                /// </summary>
                public bool invert;

                public FilterEffect(
                    FilterEffectType effectType,
                    bool invert = false
                    )
                {
                    this.effectType = effectType;
                    this.invert = invert;
                }
                public FilterEffect Clone()
                {
                    return
                        this is BurningJealousy ? (this as BurningJealousy).Clone()
                        : this is Harvest ? (this as Harvest).Clone()
                        : this is MoveCheck ? (this as MoveCheck).Clone()
                        : this is PollenPuff ? (this as PollenPuff).Clone()
                        : this is TypeList ? (this as TypeList).Clone()
                        : new FilterEffect(
                            effectType: effectType, invert: invert
                            );
                }
            }

            /// <summary>
            /// Causes the effect to fail if the target if the target didn't have their stats raised during the turn.
            /// </summary>
            public class BurningJealousy : FilterEffect
            {
                public enum TargetType
                {
                    Self,
                    AllyTeam,
                    Target,
                    TargetTeam,
                }
                /// <summary>
                /// The target being considered for stat changes.
                /// </summary>
                public TargetType targetType;

                public enum StatChangeType
                {
                    Unchecked,
                    NoChange,
                    Raise,
                    Lower,
                }
                /// <summary>
                /// The effect is enabled if this stat was modified in its specified way. 
                /// </summary>
                public StatChangeType ATKMod, DEFMod, SPAMod, SPDMod, SPEMod, ACCMod, EVAMod;

                public BurningJealousy(
                    TargetType targetType = TargetType.Target,
                    StatChangeType ATKMod = StatChangeType.Raise, StatChangeType DEFMod = StatChangeType.Raise,
                    StatChangeType SPAMod = StatChangeType.Raise, StatChangeType SPDMod = StatChangeType.Raise,
                    StatChangeType SPEMod = StatChangeType.Raise,
                    StatChangeType ACCMod = StatChangeType.Raise, StatChangeType EVAMod = StatChangeType.Raise,

                    bool invert = false
                    )
                    : base(effectType: FilterEffectType.BurningJealousy, invert: invert)
                {
                    this.targetType = targetType;
                    this.ATKMod = ATKMod;
                    this.DEFMod = DEFMod;
                    this.SPAMod = SPAMod;
                    this.SPDMod = SPDMod;
                    this.SPEMod = SPEMod;
                    this.ACCMod = ACCMod;
                    this.EVAMod = EVAMod;
                }
                public new BurningJealousy Clone()
                {
                    return new BurningJealousy(
                        targetType: targetType,
                        ATKMod: ATKMod, DEFMod: DEFMod, SPAMod: SPAMod, SPDMod: SPDMod, SPEMod: SPEMod,
                        ACCMod: ACCMod, EVAMod: EVAMod,

                        invert: invert
                        );
                }
                public bool DoesPokemonPassStatCheck(Main.Pokemon.Pokemon pokemon)
                {
                    bool success = false;

                    // ATK
                    if (ATKMod == StatChangeType.Raise && pokemon.bProps.ATKRaised) return true;
                    if (ATKMod == StatChangeType.Lower && pokemon.bProps.ATKLowered) return true;
                    if (ATKMod == StatChangeType.NoChange && !pokemon.bProps.ATKRaised && !pokemon.bProps.ATKLowered) return true;

                    // DEF
                    if (DEFMod == StatChangeType.Raise && pokemon.bProps.DEFRaised) return true;
                    if (DEFMod == StatChangeType.Lower && pokemon.bProps.DEFLowered) return true;
                    if (DEFMod == StatChangeType.NoChange && !pokemon.bProps.DEFRaised && !pokemon.bProps.DEFLowered) return true;

                    // SPA
                    if (SPAMod == StatChangeType.Raise && pokemon.bProps.SPARaised) return true;
                    if (SPAMod == StatChangeType.Lower && pokemon.bProps.SPALowered) return true;
                    if (SPAMod == StatChangeType.NoChange && !pokemon.bProps.SPARaised && !pokemon.bProps.SPALowered) return true;

                    // SPD
                    if (SPDMod == StatChangeType.Raise && pokemon.bProps.SPDRaised) return true;
                    if (SPDMod == StatChangeType.Lower && pokemon.bProps.SPDLowered) return true;
                    if (SPDMod == StatChangeType.NoChange && !pokemon.bProps.SPDRaised && !pokemon.bProps.SPDLowered) return true;

                    // SPE
                    if (SPEMod == StatChangeType.Raise && pokemon.bProps.SPERaised) return true;
                    if (SPEMod == StatChangeType.Lower && pokemon.bProps.SPELowered) return true;
                    if (SPEMod == StatChangeType.NoChange && !pokemon.bProps.SPERaised && !pokemon.bProps.SPELowered) return true;

                    // ACC
                    if (ACCMod == StatChangeType.Raise && pokemon.bProps.ACCRaised) return true;
                    if (ACCMod == StatChangeType.Lower && pokemon.bProps.ACCLowered) return true;
                    if (ACCMod == StatChangeType.NoChange && !pokemon.bProps.ACCRaised && !pokemon.bProps.ACCLowered) return true;

                    // EVA
                    if (EVAMod == StatChangeType.Raise && pokemon.bProps.EVARaised) return true;
                    if (EVAMod == StatChangeType.Lower && pokemon.bProps.EVALowered) return true;
                    if (EVAMod == StatChangeType.NoChange && !pokemon.bProps.EVARaised && !pokemon.bProps.EVALowered) return true;

                    return success;
                }
            }

            /// <summary>
            /// Causes the effect to fail if the given status conditions are not present.
            /// </summary>
            public class Harvest : FilterEffect
            {
                public enum ConditionType
                {
                    Pokemon,
                    Team,
                    Battle
                }
                public ConditionType conditionType;
                public List<string> conditions;

                /// <summary>
                /// If non-empty, this effect fails if the status doesn't have the specified effect type.
                /// </summary>
                public HashSet<PokemonSEType> statusPKTypes;
                /// <summary>
                /// If non-empty, this effect fails if the status doesn't have the specified effect type.
                /// </summary>
                public HashSet<TeamSEType> statusTETypes;
                /// <summary>
                /// If non-empty, this effect fails if the status doesn't have the specified effect type.
                /// </summary>
                public HashSet<BattleSEType> statusBTLTypes;

                public Harvest(
                    ConditionType conditionType = ConditionType.Pokemon,
                    IEnumerable<string> conditions = null,
                    IEnumerable<PokemonSEType> statusPKTypes = null,
                    IEnumerable<TeamSEType> statusTETypes = null,
                    IEnumerable<BattleSEType> statusBTLTypes = null,
                    bool invert = false
                    )
                    : base(effectType: FilterEffectType.Harvest, invert: invert)
                {
                    this.conditionType = conditionType;
                    this.conditions = conditions == null ? new List<string>()
                        : new List<string>(conditions);
                    this.statusPKTypes = statusPKTypes == null ? new HashSet<PokemonSEType>()
                        : new HashSet<PokemonSEType>(statusPKTypes);
                    this.statusTETypes = statusTETypes == null ? new HashSet<TeamSEType>()
                        : new HashSet<TeamSEType>(statusTETypes);
                    this.statusBTLTypes = statusBTLTypes == null ? new HashSet<BattleSEType>()
                        : new HashSet<BattleSEType>(statusBTLTypes);
                }
                public new Harvest Clone()
                {
                    return new Harvest(
                        conditionType: conditionType,
                        conditions: conditions,
                        statusPKTypes: statusPKTypes, statusTETypes: statusTETypes, statusBTLTypes: statusBTLTypes,
                        invert: invert
                        );
                }

                public bool DoesStatusSatisfy(StatusPKData statusData)
                {
                    if (conditionType == ConditionType.Pokemon)
                    {
                        if (conditions.Count > 0)
                        {
                            for (int i = 0; i < conditions.Count; i++)
                            {
                                if (statusData.ID == conditions[i]
                                    || statusData.IsABaseID(conditions[i]))
                                {
                                    return true;
                                }
                            }
                            return false;
                        }

                        if (statusPKTypes.Count > 0)
                        {
                            List<PokemonSEType> listTypes = new List<PokemonSEType>(statusPKTypes);
                            for (int i = 0; i < listTypes.Count; i++)
                            {
                                if (statusData.GetEffectNew(listTypes[i]) != null)
                                {
                                    return true;
                                }
                            }
                            return false;
                        }
                    }
                    return false;
                }
                public bool DoesStatusSatisfy(StatusTEData statusData)
                {
                    if (conditionType == ConditionType.Team)
                    {
                        if (conditions.Count > 0)
                        {
                            for (int i = 0; i < conditions.Count; i++)
                            {
                                if (statusData.ID == conditions[i]
                                    || statusData.IsABaseID(conditions[i]))
                                {
                                    return true;
                                }
                            }
                            return false;
                        }

                        if (statusTETypes.Count > 0)
                        {
                            List<TeamSEType> listTypes = new List<TeamSEType>(statusTETypes);
                            for (int i = 0; i < listTypes.Count; i++)
                            {
                                if (statusData.GetEffectNew(listTypes[i]) != null)
                                {
                                    return true;
                                }
                            }
                            return false;
                        }
                    }
                    return false;
                }
                public bool DoesStatusSatisfy(StatusBTLData statusData)
                {
                    if (conditionType == ConditionType.Battle)
                    {
                        if (conditions.Count > 0)
                        {
                            for (int i = 0; i < conditions.Count; i++)
                            {
                                if (statusData.ID == conditions[i]
                                    || statusData.IsABaseID(conditions[i]))
                                {
                                    return true;
                                }
                            }
                            return false;
                        }

                        if (statusBTLTypes.Count > 0)
                        {
                            List<BattleSEType> listTypes = new List<BattleSEType>(statusBTLTypes);
                            for (int i = 0; i < listTypes.Count; i++)
                            {
                                if (statusData.GetEffectNew(listTypes[i]) != null)
                                {
                                    return true;
                                }
                            }
                            return false;
                        }
                    }
                    return false;
                }
            }

            /// <summary>
            /// Causes the effect to fail if the item parameter doesn't satisfy the given conditions.
            /// </summary>
            public class ItemCheck : FilterEffect
            {
                /// <summary>
                /// If non-empty, this effect will fail if the item is not one of the listed item IDs.
                /// </summary>
                public List<string> specificItemIDs;
                /// <summary>
                /// If non-empty, this effect will fail if the item is classified as one of the listed pockets.
                /// </summary>
                public HashSet<ItemPocket> pockets;
                /// <summary>
                /// If non-empty, this effect will fail if the item doesn't have any effects listed here.
                /// </summary>
                public HashSet<ItemEffectType> effects;
                /// <summary>
                /// If non-empty, this effect will fail if the item doesn't have any tags listed here.
                /// </summary>
                public HashSet<ItemTag> tags;

                public ItemCheck(
                    IEnumerable<string> specificItemIDs = null,
                    IEnumerable<ItemPocket> pockets = null,
                    IEnumerable<ItemEffectType> effects = null,
                    IEnumerable<ItemTag> tags = null,
                    bool damagingOnly = false
                    )
                    : base(effectType: FilterEffectType.ItemCheck)
                {
                    this.specificItemIDs = specificItemIDs == null ? new List<string>()
                        : new List<string>(specificItemIDs);
                    this.pockets = pockets == null ? new HashSet<ItemPocket>()
                        : new HashSet<ItemPocket>(pockets);
                    this.effects = effects == null ? new HashSet<ItemEffectType>()
                        : new HashSet<ItemEffectType>(effects);
                    this.tags = tags == null ? new HashSet<ItemTag>() : new HashSet<ItemTag>(tags);
                }
                public new ItemCheck Clone()
                {
                    return new ItemCheck(
                        specificItemIDs: specificItemIDs,
                        pockets: pockets,
                        effects: effects,
                        tags: tags
                        );
                }
                public bool DoesItemPassFilter(Item item)
                {
                    bool success = true;

                    // Specific Items
                    if (specificItemIDs.Count > 0)
                    {
                        success = false;
                        for (int i = 0; i < specificItemIDs.Count && !success; i++)
                        {
                            if (item.data.ID == specificItemIDs[i])
                            {
                                success = true;
                            }
                        }
                    }

                    // Pockets

                    // Effects

                    // Tags

                    return success;
                }
            }

            /// <summary>
            /// Causes the effect to fail if the move parameter doesn't satisfy the given conditions.
            /// </summary>
            public class MoveCheck : FilterEffect
            {
                /// <summary>
                /// If non-empty, this effect will fail if the move is not one of the listed move IDs.
                /// </summary>
                public List<string> specificMoveIDs;
                /// <summary>
                /// If non-empty, this effect will fail if the move is not one of the listed categories.
                /// </summary>
                public HashSet<MoveCategory> moveCategories;
                /// <summary>
                /// If non-empty, this effect will fail if the move doesn't have any effects listed here.
                /// </summary>
                public HashSet<MoveEffectType> moveEffects;
                /// <summary>
                /// If non-empty, this effect will fail if the move doesn't have any tags listed here.
                /// </summary>
                public HashSet<MoveTag> moveTags;

                /// <summary>
                /// If true, the effect will fail if the move is not a damaging move.
                /// </summary>
                public bool damagingOnly;
                /// <summary>
                /// If true, the effect will fail if the move is not a healing move.
                /// </summary>
                public bool healingOnly;
                /// <summary>
                /// If true, the effect will fail if the move would not trigger Sheer Force.
                /// </summary>
                public bool sheerForceOnly;

                public MoveCheck(
                    IEnumerable<string> specificMoveIDs = null,
                    IEnumerable<MoveCategory> moveCategories = null,
                    IEnumerable<MoveEffectType> moveEffects = null,
                    IEnumerable<MoveTag> moveTags = null,
                    bool damagingOnly = false, bool healingOnly = false,
                    bool sheerForceOnly = false
                    )
                    : base(effectType: FilterEffectType.MoveCheck)
                {
                    this.specificMoveIDs = specificMoveIDs == null ? new List<string>()
                        : new List<string>(specificMoveIDs);
                    this.moveCategories = moveCategories == null ? new HashSet<MoveCategory>()
                        : new HashSet<MoveCategory>(moveCategories);
                    this.moveEffects = moveEffects == null ? new HashSet<MoveEffectType>()
                        : new HashSet<MoveEffectType>(moveEffects);
                    this.moveTags = moveTags == null ? new HashSet<MoveTag>() : new HashSet<MoveTag>(moveTags);
                    this.damagingOnly = damagingOnly;
                    this.healingOnly = healingOnly;
                    this.sheerForceOnly = sheerForceOnly;
                }
                public new MoveCheck Clone()
                {
                    return new MoveCheck(
                        specificMoveIDs: specificMoveIDs,
                        moveCategories: moveCategories,
                        moveEffects: moveEffects,
                        moveTags: moveTags,
                        damagingOnly: damagingOnly, healingOnly: healingOnly,
                        sheerForceOnly: sheerForceOnly
                        );
                }
            }

            /// <summary>
            /// Causes the effect to fail if the target isn't a specified type of target.
            /// </summary>
            public class PollenPuff : FilterEffect
            {
                public enum TargetType
                {
                    Self,
                    Ally,
                    Enemy,
                }
                public HashSet<TargetType> targetTypes;

                public PollenPuff(IEnumerable<TargetType> targetTypes, bool invert = false)
                    : base(effectType: FilterEffectType.PollenPuff, invert: invert)
                {
                    this.targetTypes = targetTypes == null ? new HashSet<TargetType> { TargetType.Ally }
                        : new HashSet<TargetType>(targetTypes);
                }
                public new PollenPuff Clone()
                {
                    return new PollenPuff(targetTypes: targetTypes, invert: invert);
                }
            }

            /// <summary>
            /// Causes the effect to fail if the target isn't a specified type.
            /// </summary>
            public class TypeList : FilterEffect
            {
                public enum TargetType
                {
                    Pokemon,
                    Move
                }
                /// <summary>
                /// The mechanic that we are checking for type.
                /// </summary>
                public TargetType targetType;

                /// <summary>
                /// The types specified.
                /// </summary>
                public List<string> types;
                /// <summary>
                /// If true, the effect fails if the target doesn't contain all the specified types.
                /// </summary>
                public bool exact;

                public TypeList(
                    TargetType targetType = TargetType.Pokemon,
                    IEnumerable<string> types = null, bool exact = false,

                    bool invert = false)
                    : base(effectType: FilterEffectType.TypeList, invert: invert)
                {
                    this.targetType = targetType;
                    this.types = types == null ? new List<string>() : new List<string>(types);
                    this.exact = exact;
                    this.invert = invert;
                }
                public new TypeList Clone()
                {
                    return new TypeList(
                        targetType: targetType, types: types, exact: exact,
                        invert: invert);
                }
            }
        }

    }
}
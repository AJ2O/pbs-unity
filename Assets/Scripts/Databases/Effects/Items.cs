using System.Collections.Generic;

namespace PBS.Databases.Effects.Items
{
    public class ItemEffect
    {
        /// <summary>
        /// The type of effect that this is.
        /// </summary>
        public ItemEffectType effectType;

        /// <summary>
        /// Additional restrictions on how the effect is applied.
        /// </summary>
        public List<Filter.FilterEffect> filters;

        /// <summary>
        /// The chance of the effect working.
        /// </summary>
        public float chance;

        /// <summary>
        /// If set to true, this effect can be applied if the item is consumed.
        /// </summary>
        public bool applyOnConsume;
        /// <summary>
        /// If set to true, this effect can be applied if the item is used.
        /// </summary>
        public bool applyOnUse;

        public ItemEffect(
            ItemEffectType effectType,
            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1,
            bool applyOnConsume = false, bool applyOnUse = false
            )
        {
            this.effectType = effectType;
            this.filters = filters == null ? new List<Filter.FilterEffect>()
                : new List<Filter.FilterEffect>(filters);
            this.chance = chance;
            this.applyOnConsume = applyOnConsume;
            this.applyOnUse = applyOnUse;
        }
        public ItemEffect Clone()
        {
            return
                this is Charcoal ? (this as Charcoal).Clone()
                : this is ChoiceBand ? (this as ChoiceBand).Clone()
                : this is ChoiceBandStats ? (this as ChoiceBandStats).Clone()
                : this is FocusBand ? (this as FocusBand).Clone()
                : this is GriseousOrb ?

                    this is ArceusPlate ? (this as ArceusPlate).Clone()
                    : this is MegaStone ? (this as MegaStone).Clone()
                    : this is RKSMemory ? (this as RKSMemory).Clone()
                    : (this as GriseousOrb).Clone()

                : this is Judgment ? (this as Judgment).Clone()
                : this is LiechiBerry ? (this as LiechiBerry).Clone()
                : this is LifeOrb ? (this as LifeOrb).Clone()
                : this is LumBerry ? (this as LumBerry).Clone()
                : this is LumBerryTrigger ? (this as LumBerryTrigger).Clone()
                : this is MegaStone ? (this as MegaStone).Clone()
                : this is NaturalGift ? (this as NaturalGift).Clone()
                : this is PokeBall ? (this as PokeBall).Clone()
                : this is Potion ? (this as Potion).Clone()
                : this is QuickClaw ? (this as QuickClaw).Clone()
                : this is ShedShell ? (this as ShedShell).Clone()
                : this is TriggerSitrusBerry ? (this as TriggerSitrusBerry).Clone()
                : this is YacheBerry ? (this as YacheBerry).Clone()
                : this is ZCrystal ? (this as ZCrystal).Clone()
                : this is ZCrystalSignature ? (this as ZCrystalSignature).Clone()
                : new ItemEffect(
                    effectType: effectType,
                    filters: filters,
                    chance: chance, applyOnConsume: applyOnConsume, applyOnUse: applyOnUse
                    );
        }
    }

    // Form-Changing Items
    /// <summary>
    /// To be used alongside <seealso cref="GriseousOrb"/>. Will only enable FormChange if the user has an 
    /// ability with the effect <seealso cref="Abilities.Multitype"/>.
    /// </summary>
    public class ArceusPlate : GriseousOrb
    {
        public ArceusPlate(string baseFormID, string formID) : base(baseFormID: baseFormID, formID: formID)
        { }
        public new ArceusPlate Clone()
        {
            return new ArceusPlate(baseFormID: baseFormID, formID: formID);
        }
    }
    /// <summary>
    /// Changes eligible Pokémon into another form if this item is held. Cannot be forcibly removed under most 
    /// circumstances if the holder is a valid Pokémon.
    /// </summary>
    public class GriseousOrb : ItemEffect
    {
        /// <summary>
        /// The base Pokémon ID that can change into <seealso cref="formID"/>.
        /// </summary>
        public string baseFormID;
        /// <summary>
        /// The Pokémon ID that <seealso cref="baseFormID"/> is changed into.
        /// </summary>
        public string formID;

        public GriseousOrb(string baseFormID, string formID) : base(effectType: ItemEffectType.GriseousOrb)
        {
            this.baseFormID = baseFormID;
            this.formID = formID;
        }
        public new GriseousOrb Clone()
        {
            return new GriseousOrb(baseFormID: baseFormID, formID: formID);
        }
    }
    /// <summary>
    /// Heals the specified status conditions.
    /// </summary>
    public class LumBerry : ItemEffect
    {
        /// <summary>
        /// The statuses that are healed.
        /// </summary>
        public List<string> statuses;

        /// <summary>
        /// Statuses with the specified effect types are healed.
        /// </summary>
        public HashSet<PokemonSEType> statusEffectTypes;

        /// <summary>
        /// The text displayed when the item fails to heal any status.
        /// </summary>
        public string failText;

        public LumBerry(
            IEnumerable<string> statuses = null,
            IEnumerable<PokemonSEType> statusEffectTypes = null,
            string failText = "item-lumberry-fail",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: ItemEffectType.LumBerry, filters: filters)
        {
            this.statuses = statuses == null ? new List<string>() : new List<string>(statuses);
            this.statusEffectTypes = statusEffectTypes == null ? new HashSet<PokemonSEType>()
                : new HashSet<PokemonSEType>(statusEffectTypes);
            this.failText = failText;
        }
        public new LumBerry Clone()
        {
            return new LumBerry(
                statuses: statuses,
                statusEffectTypes: statusEffectTypes,
                failText: failText,
                filters: filters
                );
        }
        public bool IsStatusHealed(string statusID)
        {
            // check explicit statuses
            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i] == statusID)
                {
                    return true;
                }
            }

            // check status effects
            PBS.Data.PokemonStatus statusData = PBS.Databases.PokemonStatuses.instance.GetStatusData(statusID);
            foreach (PokemonSEType effectType in statusEffectTypes)
            {
                if (statusData.GetEffectNew(effectType) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
    /// <summary>
    /// Heals the specified status conditions.
    /// </summary>
    public class LumBerryTrigger : ItemEffect
    {
        /// <summary>
        /// The statuses that are healed.
        /// </summary>
        public List<string> statuses;

        /// <summary>
        /// Statuses with the specified effect types are healed.
        /// </summary>
        public HashSet<PokemonSEType> statusEffectTypes;

        public LumBerryTrigger(
            IEnumerable<string> statuses = null,
            IEnumerable<PokemonSEType> statusEffectTypes = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: ItemEffectType.LumBerryTrigger, filters: filters)
        {
            this.statuses = statuses == null ? new List<string>() : new List<string>(statuses);
            this.statusEffectTypes = statusEffectTypes == null ? new HashSet<PokemonSEType>()
                : new HashSet<PokemonSEType>(statusEffectTypes);
        }
        public new LumBerryTrigger Clone()
        {
            return new LumBerryTrigger(
                statuses: statuses,
                statusEffectTypes: statusEffectTypes,
                filters: filters
                );
        }
        public bool IsStatusHealed(string statusID)
        {
            // check explicit statuses
            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i] == statusID)
                {
                    return true;
                }
            }

            // check status effects
            PBS.Data.PokemonStatus statusData = PBS.Databases.PokemonStatuses.instance.GetStatusData(statusID);
            foreach (PokemonSEType effectType in statusEffectTypes)
            {
                if (statusData.GetEffectNew(effectType) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
    /// <summary>
    /// Similar to <seealso cref="GriseousOrb"/>, but requires a turn of activation before form is changed.
    /// </summary>
    public class MegaStone : GriseousOrb
    {
        public MegaStone(string baseFormID, string formID) : base(baseFormID: baseFormID, formID: formID)
        { }
        public new MegaStone Clone()
        {
            return new MegaStone(baseFormID: baseFormID, formID: formID);
        }
    }
    /// <summary>
    /// To be used alongside <seealso cref="GriseousOrb"/>. Will only enable FormChange if the user has an 
    /// ability with the effect <seealso cref="Abilities.RKSSystem"/>.
    /// </summary>
    public class RKSMemory : GriseousOrb
    {
        public RKSMemory(string baseFormID, string formID) : base(baseFormID: baseFormID, formID: formID)
        { }
        public new RKSMemory Clone()
        {
            return new RKSMemory(baseFormID: baseFormID, formID: formID);
        }
    }

    /// <summary>
    /// Scales the power of the holder's moves if they satisfy the specified conditions.
    /// </summary>
    public class Charcoal : ItemEffect
    {
        /// <summary>
        /// The amount to scale move power by.
        /// </summary>
        public float powerModifier;

        public Charcoal(
            float powerModifier = 1.2f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: ItemEffectType.Charcoal, filters: filters)
        {
            this.powerModifier = powerModifier;
        }
        public new Charcoal Clone()
        {
            return new Charcoal(
                powerModifier: powerModifier,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Locks the holder into its first-selected move until it leaves the field.
    /// </summary>
    public class ChoiceBand : ItemEffect
    {
        public ChoiceBand(
            )
            : base(effectType: ItemEffectType.ChoiceBand)
        {

        }
        public new ChoiceBand Clone()
        {
            return new ChoiceBand();
        }
    }

    /// <summary>
    /// Scales the user's stats.
    /// </summary>
    public class ChoiceBandStats : ItemEffect
    {
        /// <summary>
        /// The stats to scale.
        /// </summary>
        public General.StatScale statScale;
        /// <summary>
        /// If true, the stat scales apply to all ally Pokémon on the field.
        /// </summary>
        public bool victoryStar;
        /// <summary>
        /// If non-negative, the user's HP must be at or below this percentage to apply the stat scaling.
        /// </summary>
        public float defeatistThreshold;

        public ChoiceBandStats(
            General.StatScale statScale,
            bool victoryStar = false,
            float defeatistThreshold = -1
            )
            : base(effectType: ItemEffectType.ChoiceBandStats)
        {
            this.statScale = statScale.Clone();
            this.victoryStar = victoryStar;
            this.defeatistThreshold = defeatistThreshold;
        }
        public new ChoiceBandStats Clone()
        {
            return new ChoiceBandStats(
                statScale: statScale,
                victoryStar: victoryStar,
                defeatistThreshold: defeatistThreshold
                );
        }
    }

    /// <summary>
    /// Prevents this pokemon from fainting from a direct hit at a certain HP threshold.
    /// </summary>
    public class FocusBand : ItemEffect
    {
        /// <summary>
        /// The HP % the user's HP must at least be at to trigger this effect.
        /// </summary>
        public float hpThreshold;

        /// <summary>
        /// The text displayed when the user hangs on.
        /// </summary>
        public string displayText;

        public FocusBand(
            float hpThreshold = 1f,
            string displayText = "item-focusband",

            float chance = 1f,
            IEnumerable<Filter.FilterEffect> filters = null
            ) : base(effectType: ItemEffectType.FocusBand, chance: chance, filters: filters)
        {
            this.displayText = displayText;
            this.hpThreshold = hpThreshold;
        }
        public new FocusBand Clone()
        {
            return new FocusBand(
                hpThreshold: hpThreshold,
                displayText: displayText,
                chance: chance, filters: filters);
        }
    }

    /// <summary>
    /// Changes the type of the holder's moves with the <seealso cref="Moves.Judgment"/> effect.
    /// </summary>
    public class Judgment : ItemEffect
    {
        /// <summary>
        /// The type that eligible moves become.
        /// </summary>
        public string moveType;

        public Judgment(string moveType) : base(effectType: ItemEffectType.Judgment)
        {
            this.moveType = moveType;
        }
        public new Judgment Clone()
        {
            return new Judgment(moveType: moveType);
        }
    }

    /// <summary>
    /// Modifies the user's stats.
    /// </summary>
    public class LiechiBerry : ItemEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        public LiechiBerry(
            General.StatStageMod statStageMod,
            bool applyOnConsume = false, bool applyOnUse = true)
            : base(effectType: ItemEffectType.LiechiBerry,
                    applyOnConsume: applyOnConsume, applyOnUse: applyOnUse)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new LiechiBerry Clone()
        {
            return new LiechiBerry(statStageMod: statStageMod,
                applyOnConsume: applyOnConsume, applyOnUse: applyOnUse);
        }
    }

    /// <summary>
    /// Deals proportional damage to the user after move use.
    /// </summary>
    public class LifeOrb : ItemEffect
    {
        public enum RecoilMode
        {
            /// <summary>
            /// Damage dealt to the user is proportional to the total damage dealt/
            /// </summary>
            Damage,
            /// <summary>
            /// Damage dealt to the user is proportional to the user's max HP.
            /// </summary>
            MaxHP
        }
        /// <summary>
        /// The type of recoil the user takes.
        /// </summary>
        public RecoilMode recoilMode;
        /// <summary>
        /// Damage dealt to the user determined by <seealso cref="recoilMode"/>.
        /// </summary>
        public float hpLossPercent;
        /// <summary>
        /// If false, the user will not take recoil if they have the <seealso cref="Abilities.RockHead"/>
        /// ability effect.
        /// </summary>
        public bool bypassRockHead;

        /// <summary>
        /// The text displayed when the user takes recoil damage.
        /// </summary>
        public string displayText;

        public LifeOrb(
            RecoilMode recoilMode = RecoilMode.MaxHP,
            float hpLossPercent = 0.25f, bool bypassRockHead = false,
            string displayText = "item-lifeorb"
            )
            : base(effectType: ItemEffectType.LifeOrb)
        {
            this.recoilMode = recoilMode;
            this.hpLossPercent = hpLossPercent;
            this.bypassRockHead = bypassRockHead;
            this.displayText = displayText;
        }
        public new LifeOrb Clone()
        {
            return new LifeOrb(
                recoilMode: recoilMode,
                hpLossPercent: hpLossPercent, bypassRockHead: bypassRockHead,
                displayText: displayText
                );
        }
    }

    /// <summary>
    /// This item can be used by Natural Gift and transforms its type and power.
    /// </summary>
    public class NaturalGift : ItemEffect
    {
        /// <summary>
        /// If null, the move's type does not change.
        /// </summary>
        public string moveType;
        /// <summary>
        /// If less than 1, the move's base power does not change.
        /// </summary>
        public int basePower;

        public NaturalGift(
            string moveType = "normal", int basePower = 80
            )
            : base(effectType: ItemEffectType.NaturalGift)
        {
            this.moveType = moveType;
            this.basePower = basePower;
        }
        public new NaturalGift Clone()
        {
            return new NaturalGift(
                moveType: moveType, basePower: basePower
                );
        }
    }

    /// <summary>
    /// Contains all effects relating to Poké Ball catch rates and modifiers.
    /// </summary>
    public class PokeBall : ItemEffect
    {
        /// <summary>
        /// The base catch rate modifier for this 
        /// </summary>
        public float catchRateModifier;

        public PokeBall(
            float catchRateModifier = 1f
            )
            : base(effectType: ItemEffectType.PokeBall)
        {
            this.catchRateModifier = catchRateModifier;
        }
        public new PokeBall Clone()
        {
            return new PokeBall(
                catchRateModifier: catchRateModifier
                );
        }
    }

    /// <summary>
    /// Heals the Pokémon by a given amount of HP.
    /// </summary>
    public class Potion : ItemEffect
    {
        /// <summary>
        /// Defines how HP is recovered.
        /// </summary>
        public General.HealHP healHP;

        public Potion(
            General.HealHP healHP,
            bool applyOnConsume = true, bool applyOnUse = true
            )
            : base(effectType: ItemEffectType.Potion, applyOnConsume: applyOnConsume, applyOnUse: applyOnUse)
        {
            this.healHP = healHP.Clone();
        }
        public new Potion Clone()
        {
            return new Potion(
                healHP: healHP,
                applyOnConsume: applyOnConsume, applyOnUse: applyOnUse
                );
        }
    }

    /// <summary>
    /// The holder has a chance of attacking first within its priority bracket.
    /// </summary>
    public class QuickClaw : ItemEffect
    {
        /// <summary>
        /// The text displayed if the effect is triggered.
        /// </summary>
        public string displayText;

        public QuickClaw(
            string displayText = "item-quickclaw",
            float chance = 0.2f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: ItemEffectType.QuickClaw, chance: chance, filters: filters)
        {
            this.displayText = displayText;
        }
        public new QuickClaw Clone()
        {
            return new QuickClaw(
                displayText: displayText,
                chance: chance, filters: filters
                );
        }
    }

    /// <summary>
    /// Allows the holder to switch out, regardless of trapping moves or ability.
    /// </summary>
    public class ShedShell : ItemEffect
    {
        public ShedShell() : base(effectType: ItemEffectType.ShedShell)
        {

        }
        public new ShedShell Clone()
        {
            return new ShedShell();
        }
    }

    /// <summary>
    /// Enables this item to be consumed if the holder's HP falls below the given threshold.
    /// </summary>
    public class TriggerSitrusBerry : ItemEffect
    {
        /// <summary>
        /// Activates this item if the holder's HP falls below this threshold.
        /// </summary>
        public float hpThreshold;

        public TriggerSitrusBerry(
            float hpThreshold = 0.25f
            )
            : base(effectType: ItemEffectType.TriggerOnHPLoss)
        {
            this.hpThreshold = hpThreshold;
        }
        public new TriggerSitrusBerry Clone()
        {
            return new TriggerSitrusBerry(
                hpThreshold: hpThreshold
                );
        }
    }

    /// <summary>
    /// Scales damage taken if the holder is hit by moves that satisfy the given conditions.
    /// </summary>
    public class YacheBerry : ItemEffect
    {
        /// <summary>
        /// The amount to scale move power by.
        /// </summary>
        public float damageModifier;

        /// <summary>
        /// The text displayed when the user hangs on.
        /// </summary>
        public string displayText;

        /// <summary>
        /// This item only triggers when the move used is super-effective against the holder.
        /// </summary>
        public bool mustBeSuperEffective;

        public YacheBerry(
            float damageModifier = 0.5f,
            string displayText = "item-yacheberry",
            bool mustBeSuperEffective = true,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: ItemEffectType.YacheBerry, filters: filters)
        {
            this.damageModifier = damageModifier;
            this.displayText = displayText;
            this.mustBeSuperEffective = mustBeSuperEffective;
        }

        public new YacheBerry Clone()
        {
            return new YacheBerry(
                damageModifier: damageModifier,
                displayText: displayText,
                mustBeSuperEffective: mustBeSuperEffective,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Transforms damaging moves of a given type into Z-Moves. Activates Z-Effects for moves of the given type.
    /// </summary>
    public class ZCrystal : ItemEffect
    {
        /// <summary>
        /// The type of moves that are eligible.
        /// </summary>
        public string moveType;
        /// <summary>
        /// The Z-Move that eligible moves are transformed into.
        /// </summary>
        public string ZMove;

        public ZCrystal(
            string moveType,
            string ZMove
            )
            : base(effectType: ItemEffectType.ZCrystal)
        {
            this.moveType = moveType;
            this.ZMove = ZMove;
        }
        public new ZCrystal Clone()
        {
            return new ZCrystal(moveType: moveType, ZMove: ZMove);
        }
    }

    /// <summary>
    /// Transforms eligible moves into a Z-Move for eligible Pokémon only.
    /// </summary>
    public class ZCrystalSignature : ItemEffect
    {
        /// <summary>
        /// The Z-Move that eligible moves will be turned into.
        /// </summary>
        public string ZMove;
        /// <summary>
        /// The eligible moves that can be transformed into <seealso cref="zMove"/>.
        /// </summary>
        public List<string> eligibleMoves;
        /// <summary>
        /// The eligible Pokémon IDs that are able to use this Z-Crystal.
        /// </summary>
        public List<string> pokemonIDs;

        public ZCrystalSignature(
            string ZMove,
            IEnumerable<string> eligibleMoves,
            IEnumerable<string> eligiblePokemonIDs
            )
            : base(effectType: ItemEffectType.ZCrystalSignature)
        {
            this.ZMove = ZMove;
            this.eligibleMoves = eligibleMoves == null ? new List<string>() : new List<string>(eligibleMoves);
            pokemonIDs = eligiblePokemonIDs == null ? new List<string>()
                : new List<string>(eligiblePokemonIDs);
        }
        public new ZCrystalSignature Clone()
        {
            return new ZCrystalSignature(
                ZMove: ZMove,
                eligibleMoves: eligibleMoves,
                eligiblePokemonIDs: pokemonIDs);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases.Effects.Moves
{
    public class MoveEffect
    {
        /// <summary>
        /// The type of effect that this is.
        /// </summary>
        public MoveEffectType effectType;
        /// <summary>
        /// The timing that triggers this effect.
        /// </summary>
        public MoveEffectTiming timing;
        /// <summary>
        /// Who or what the effect is applied to.
        /// </summary>
        public MoveEffectTargetType targetType;
        /// <summary>
        /// The amount of times the effect is executed.
        /// </summary>
        public MoveEffectOccurrence occurrence;

        /// <summary>
        /// Additional restrictions on how the effect is applied.
        /// </summary>
        public List<Filter.FilterEffect> filters;

        /// <summary>
        /// The chance of the effect working.
        /// </summary>
        public float chance;
        /// <summary>
        /// Does not check the effect's chance against each target, but uses one check.
        /// </summary>
        public bool oneTimeChance;
        /// <summary>
        /// If applicable, always show when this effect fails or succeeds.
        /// </summary>
        public bool forceEffectDisplay;

        public MoveEffect(
            MoveEffectType effectType = MoveEffectType.None,
            MoveEffectTiming timing = MoveEffectTiming.Unique,
            MoveEffectTargetType targetType = MoveEffectTargetType.Unique,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.None,
            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false)
        {
            this.effectType = effectType;
            this.timing = timing;
            this.targetType = targetType;
            this.occurrence = occurrence;
            this.filters = new List<Filter.FilterEffect>();
            if (filters != null)
            {
                List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                for (int i = 0; i < tempFilters.Count; i++)
                {
                    this.filters.Add(tempFilters[i].Clone());
                }
            }

            this.chance = chance;
            this.oneTimeChance = oneTimeChance;
            this.forceEffectDisplay = forceEffectDisplay;
        }
        public MoveEffect Clone()
        {
            return
                this is Absorb ? (this as Absorb).Clone()
                : this is AuraWheel ? (this as AuraWheel).Clone()
                : this is BasePowerMultiplier ?

                    this is LashOut ? (this as LashOut).Clone()
                    : (this as BasePowerMultiplier).Clone()


                : this is BeatUp ? (this as BeatUp).Clone()
                : this is BurningJealousy ? (this as BurningJealousy).Clone()
                : this is CoreEnforcer ? (this as CoreEnforcer).Clone()
                : this is CorrosiveGas ? (this as CorrosiveGas).Clone()
                : this is Covet ? (this as Covet).Clone()
                : this is DamageMultiplier ? (this as DamageMultiplier).Clone()
                : this is DoubleEdge ? (this as DoubleEdge).Clone()
                : this is DoubleKick ? (this as DoubleKick).Clone()
                : this is DragonRage ? (this as DragonRage).Clone()
                : this is Endure ? (this as Endure).Clone()
                : this is Eruption ? (this as Eruption).Clone()
                : this is ExpandingForce ? (this as ExpandingForce).Clone()
                : this is ExpandingForcePower ? (this as ExpandingForcePower).Clone()
                : this is FailNotPokemon ? (this as FailNotPokemon).Clone()
                : this is FakeOut ? (this as FakeOut).Clone()
                : this is Feint ? (this as Feint).Clone()
                : this is FuryAttack ? (this as FuryAttack).Clone()
                : this is GrassyGlide ? (this as GrassyGlide).Clone()
                : this is Guillotine ? (this as Guillotine).Clone()
                : this is GuillotineAccuracy ? (this as GuillotineAccuracy).Clone()
                : this is HealBeforeUse ? (this as HealBeforeUse).Clone()
                : this is HeavySlam ? (this as HeavySlam).Clone()
                : this is HiddenPower ? (this as HiddenPower).Clone()
                : this is InflictStatus ? (this as InflictStatus).Clone()
                : this is Judgment ? (this as Judgment).Clone()
                : this is KarateChop ? (this as KarateChop).Clone()
                : this is KnockOff ? (this as KnockOff).Clone()
                : this is LowKick ? (this as LowKick).Clone()
                : this is MagicCoat ? (this as MagicCoat).Clone()
                : this is Magnitude ? (this as Magnitude).Clone()
                : this is NaturalGift ? (this as NaturalGift).Clone()
                : this is PhotonGeyser ? (this as PhotonGeyser).Clone()
                : this is Poltergeist ? (this as Poltergeist).Clone()
                : this is Protect ? (this as Protect).Clone()
                : this is Psywave ? (this as Psywave).Clone()
                : this is Punishment ? (this as Punishment).Clone()
                : this is Pursuit ? (this as Pursuit).Clone()
                : this is Refresh ? (this as Refresh).Clone()
                : this is RelicSong ? (this as RelicSong).Clone()
                : this is Reversal ? (this as Reversal).Clone()
                : this is RisingVoltage ? (this as RisingVoltage).Clone()
                : this is Rollout ? (this as Rollout).Clone()
                : this is SecretPower ? (this as SecretPower).Clone()
                : this is SeismicToss ? (this as SeismicToss).Clone()
                : this is ShellSideArm ? (this as ShellSideArm).Clone()
                : this is Snore ? (this as Snore).Clone()
                : this is StatStageMod ? (this as StatStageMod).Clone()
                : this is SteelRoller ? (this as SteelRoller).Clone()
                : this is StoredPower ? (this as StoredPower).Clone()
                : this is SuckerPunch ? (this as SuckerPunch).Clone()
                : this is SunsteelStrike ? (this as SunsteelStrike).Clone()
                : this is SuperFang ? (this as SuperFang).Clone()
                : this is Synchronoise ? (this as Synchronoise).Clone()
                : this is TerrainPulse ? (this as TerrainPulse).Clone()
                : this is TripleKick ? (this as TripleKick).Clone()
                : this is WeatherBall ? (this as WeatherBall).Clone()
                : this is Whirlwind ? (this as Whirlwind).Clone()
                : this is WorrySeed ? (this as WorrySeed).Clone()
                : new MoveEffect(
                    effectType: effectType, timing: timing, targetType: targetType, occurrence: occurrence,
                    filters: filters,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                    );
        }
    }

    /// <summary>
    /// Recovers a % of damage dealt as HP.
    /// </summary>
    public class Absorb : MoveEffect
    {
        /// <summary>
        /// The % of damage dealt that is absorbed.
        /// </summary>
        public float healPercent;

        /// <summary>
        /// The text displayed when the user recovers HP.
        /// </summary>
        public string displayText;

        public Absorb(
            float healPercent = 0.5f,
            string displayText = "move-absorb"
            )
            : base(effectType: MoveEffectType.HPDrain)
        {
            this.healPercent = healPercent;
            this.displayText = displayText;
        }
        public new Absorb Clone()
        {
            return new Absorb(
                healPercent: healPercent,
                displayText: displayText
                );
        }
    }

    /// <summary>
    /// Changes the move's type depending on the user's form.
    /// </summary>
    public class AuraWheel : MoveEffect
    {
        /// <summary>
        /// The type that this move is changed to.
        /// </summary>
        public string type;
        /// <summary>
        /// If the user's Pokémon ID is in this list, the move's type is changed to <seealso cref="type"/>.
        /// </summary>
        public List<string> pokemonIDs;

        public AuraWheel(
            string type,
            IEnumerable<string> pokemonIDs
            ) : base(effectType: MoveEffectType.AuraWheel)
        {
            this.type = type;
            this.pokemonIDs = pokemonIDs == null ? new List<string>() : new List<string>(pokemonIDs);
        }
        public new AuraWheel Clone()
        {
            return new AuraWheel(type: type, pokemonIDs: pokemonIDs);
        }
    }

    /// <summary>
    /// Scales base power if the accompanying filters are satisfied.
    /// </summary>
    public class BasePowerMultiplier : MoveEffect
    {
        /// <summary>
        /// The amount by which base power is scaled.
        /// </summary>
        public float powerScale;

        public BasePowerMultiplier(
            float powerScale = 1f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: MoveEffectType.BasePowerMultiplier, filters: filters)
        {
            this.powerScale = powerScale;
        }
        public new BasePowerMultiplier Clone()
        {
            return new BasePowerMultiplier(powerScale: powerScale, filters: filters);
        }
    }

    /// <summary>
    /// Hits for as many able party members there are.
    /// </summary>
    public class BeatUp : MoveEffect
    {
        public BeatUp() : base(effectType: MoveEffectType.BeatUp)
        {

        }
        public new BeatUp Clone()
        {
            return new BeatUp();
        }
    }

    /// <summary>
    /// Inflicts a status condition on the target if they had their stats raised during the turn.
    /// </summary>
    public class BurningJealousy : InflictStatus
    {
        public BurningJealousy(
            General.InflictStatus inflictStatus,
            MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
            MoveEffectTargetType targetType = MoveEffectTargetType.Target,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,

            float chance = -1
            )
            : base(inflictStatus: inflictStatus,
                    timing: timing, targetType: targetType, occurrence: occurrence,
                    chance: chance,
                    filters: new Filter.FilterEffect[]
                    {
                    new Filter.BurningJealousy()
                    })
        { }
    }

    /// <summary>
    /// Suppresses the target's ability as long as it remains in battle (if possible).
    /// </summary>
    public class CoreEnforcer : MoveEffect
    {
        /// <summary>
        /// The text to be displayed when the ability is suppressed.
        /// </summary>
        public string displayText;
        /// <summary>
        /// The text to be displayed when the ability cannot be suppressed.
        /// </summary>
        public string failText;

        public CoreEnforcer(
            string displayText = "move-coreenforcer",
            string failText = "move-coreenforcer-fail"
            )
            : base(effectType: MoveEffectType.CoreEnforcer)
        {
            this.displayText = displayText;
            this.failText = failText;
        }
        public new CoreEnforcer Clone()
        {
            return new CoreEnforcer(
                displayText: displayText,
                failText: failText
                );
        }
    }

    /// <summary>
    /// Renders the target's held item unusable for the remainder of the battle.
    /// </summary>
    public class CorrosiveGas : MoveEffect
    {
        /// <summary>
        /// The text displayed when an item is made useless.
        /// </summary>
        public string displayText;

        public CorrosiveGas(
            string displayText = "move-corrosivegas",
            float powerScale = 2f
            )
            : base(effectType: MoveEffectType.CorrosiveGas, occurrence: MoveEffectOccurrence.OnceForEachTarget)
        {
            this.displayText = displayText;
        }
        public new CorrosiveGas Clone()
        {
            return new CorrosiveGas(displayText: displayText);
        }
    }

    /// <summary>
    /// May steal the target's held item.
    /// </summary>
    public class Covet : MoveEffect
    {
        /// <summary>
        /// The text displayed when an item is stolen.
        /// </summary>
        public string displayText;

        public Covet(
            string displayText = "move-covet",
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: MoveEffectType.Covet, occurrence: MoveEffectOccurrence.OnceForEachTarget, filters: filters)
        {
            this.displayText = displayText;
        }
        public new Covet Clone()
        {
            return new Covet(
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Scales damage if the accompanying filters are satisfied.
    /// </summary>
    public class DamageMultiplier : MoveEffect
    {
        /// <summary>
        /// The amount by which damage is scaled.
        /// </summary>
        public float damageScale;

        public DamageMultiplier(
            float damageScale = 1f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: MoveEffectType.DamageMultiplier, filters: filters)
        {
            this.damageScale = damageScale;
        }
        public new DamageMultiplier Clone()
        {
            return new DamageMultiplier(damageScale: damageScale, filters: filters);
        }
    }

    /// <summary>
    /// Deals proportional damage to the user after the move's use.
    /// </summary>
    public class DoubleEdge : MoveEffect
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

        public DoubleEdge(
            RecoilMode recoilMode = RecoilMode.Damage,
            float hpLossPercent = 0.25f, bool bypassRockHead = false,
            string displayText = "move-doubleedge"
            )
            : base(effectType: MoveEffectType.Recoil)
        {
            this.recoilMode = recoilMode;
            this.hpLossPercent = hpLossPercent;
            this.bypassRockHead = bypassRockHead;
            this.displayText = displayText;
        }
        public new DoubleEdge Clone()
        {
            return new DoubleEdge(
                recoilMode: recoilMode,
                hpLossPercent: hpLossPercent, bypassRockHead: bypassRockHead,
                displayText: displayText
                );
        }
    }

    /// <summary>
    /// Hits multiple times.
    /// </summary>
    public class DoubleKick : MoveEffect
    {
        /// <summary>
        /// The amount of hits for this attack.
        /// </summary>
        public int hits;

        public DoubleKick(int hits = 2)
            : base(effectType: MoveEffectType.DoubleKick)
        {
            this.hits = hits; ;
        }
        public new TripleKick Clone()
        {
            return new TripleKick(hits: hits);
        }

    }

    /// <summary>
    /// Makes this move deal a set amount of damage.
    /// </summary>
    public class DragonRage : MoveEffect
    {
        /// <summary>
        /// The exact damage dealt.
        /// </summary>
        public int damage;

        public DragonRage(int damage = 40) : base(effectType: MoveEffectType.DragonRage)
        {
            this.damage = damage;
        }
        public new DragonRage Clone()
        {
            return new DragonRage(damage: damage);
        }
    }

    /// <summary>
    /// Allows the user to survive direct attacks with 1 HP for the rest of the turn. 
    /// Less likely to succeed with consecutive uses.
    /// </summary>
    public class Endure : MoveEffect
    {
        /// <summary>
        /// Text displayed when the Pokémon starts enduring.
        /// </summary>
        public string displayText;
        /// <summary>
        /// Text displayed when the Pokémon successfully survives due to Endure.
        /// </summary>
        public string protectText;

        /// <summary>
        /// If true, this move can be used consecutively without a chance of failing.
        /// </summary>
        public bool consecutiveUse;

        public Endure(
            string displayText = "move-endure", string protectText = "move-endure-success",
            bool consecutiveUse = false,

            MoveEffectTargetType targetType = MoveEffectTargetType.Self,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.Once,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: MoveEffectType.Endure,
                    timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                    targetType: targetType, occurrence: occurrence, filters: filters)
        {
            this.displayText = displayText;
            this.protectText = protectText;
            this.consecutiveUse = consecutiveUse;
        }
        public new Endure Clone()
        {
            return new Endure(
                displayText: displayText, protectText: protectText,
                consecutiveUse: consecutiveUse,

                targetType: targetType, occurrence: occurrence, filters: filters
                );
        }
    }

    /// <summary>
    /// Scales base power according to the user's HP %.
    /// </summary>
    public class Eruption : MoveEffect
    {
        public Eruption() : base(effectType: MoveEffectType.Eruption) { }
        public new Eruption Clone()
        {
            return new Eruption();
        }
    }

    /// <summary>
    /// Changes the move target type if the given battle conditions are active.
    /// </summary>
    public class ExpandingForce : MoveEffect
    {
        /// <summary>
        /// The new target type if the conditions exist.
        /// </summary>
        public MoveTargetType newTargetType;
        /// <summary>
        /// The relevant battle conditions.
        /// </summary>
        public List<string> conditions;

        public ExpandingForce(
            MoveTargetType newTargetType,
            IEnumerable<string> conditions = null
            )
            : base(effectType: MoveEffectType.ExpandingForce)
        {
            this.newTargetType = newTargetType;
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
        }
        public new ExpandingForce Clone()
        {
            return new ExpandingForce(newTargetType: newTargetType, conditions: conditions);
        }
    }

    /// <summary>
    /// Increases power if the user is affected by the given battle conditions.
    /// </summary>
    public class ExpandingForcePower : MoveEffect
    {
        /// <summary>
        /// The amount that damage is scaled by.
        /// </summary>
        public float damageScale;
        /// <summary>
        /// The relevant battle conditions.
        /// </summary>
        public List<string> conditions;

        public ExpandingForcePower(
            float damageScale = 1f,
            IEnumerable<string> conditions = null
            )
            : base(effectType: MoveEffectType.ExpandingForcePower)
        {
            this.damageScale = damageScale;
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
        }
        public new ExpandingForcePower Clone()
        {
            return new ExpandingForcePower(damageScale: damageScale, conditions: conditions);
        }
    }

    /// <summary>
    /// The move immediately fails to be used if the user isn't a specified Pokémon.
    /// </summary>
    public class FailNotPokemon : MoveEffect
    {
        /// <summary>
        /// The text that displays if the user can't use the move.
        /// </summary>
        public string failText;
        /// <summary>
        /// The text that displays if the user can't use the move because it wasn't a specific listed form.
        /// </summary>
        public string failFormText;

        /// <summary>
        /// Invert filters to invert the allowable Pokémon, including additional filters set.
        /// </summary>
        public bool invert;
        /// <summary>
        /// Allow Pokémon forms derived from those listed to use the move.
        /// </summary>
        public bool allowDerivatives;
        /// <summary>
        /// Allow Pokémon transformed into one of those listed to be able to use the move.
        /// </summary>
        public bool allowTransform;

        /// <summary>
        /// The list of Pokémon that are allowed to use the move.
        /// </summary>
        public List<string> pokemonIDs;

        public FailNotPokemon(
            IEnumerable<string> pokemonIDs,
            string failText = "move-FAIL-pokemon", string failFormText = "move-FAIL-form",
            bool invert = false, bool allowDerivatives = false, bool allowTransform = true
            ) : base(effectType: MoveEffectType.FailNotPokemon)
        {
            this.failText = failText;
            this.failFormText = failFormText;
            this.invert = invert;
            this.allowDerivatives = allowDerivatives;
            this.allowTransform = allowTransform;
            this.pokemonIDs = pokemonIDs == null ? new List<string>() : new List<string>(pokemonIDs);
        }
        public new FailNotPokemon Clone()
        {
            return new FailNotPokemon(
                pokemonIDs: pokemonIDs,
                failText: failText, failFormText: failFormText,
                invert: invert, allowDerivatives: allowDerivatives, allowTransform: allowTransform
                );
        }
    }

    /// <summary>
    /// This move automatically fails if used after the first turn that the user is in battle.
    /// </summary>
    public class FakeOut : MoveEffect
    {
        /// <summary>
        /// After this turn, this move will fail if used.
        /// </summary>
        public int maxTurn;

        public FakeOut(int maxTurn = 1) : base(effectType: MoveEffectType.FakeOut)
        {
            this.maxTurn = maxTurn;
        }
        public new FakeOut Clone()
        {
            return new FakeOut(maxTurn: maxTurn);
        }
    }

    /// <summary>
    /// Lifts the effects of protection moves from the target and/or target team.
    /// </summary>
    public class Feint : MoveEffect
    {
        /// <summary>
        /// The text displayed when protection effects are lifted.
        /// </summary>
        public string displayText;
        /// <summary>
        /// The text displayed when mat block effects are lifted.
        /// </summary>
        public string displayTextMatBlock;
        /// <summary>
        /// If false, this move cannot lift the effects of Max Guard (though it may still hit
        /// through Max Guard.
        /// </summary>
        public bool liftMaxGuard;

        public Feint(
            string displayText = "move-feint", string displayTextMatBlock = "move-feint-matblock",
            bool liftMaxGuard = false
            )
            : base(effectType: MoveEffectType.Feint)
        {
            this.displayText = displayText;
            this.displayTextMatBlock = displayTextMatBlock;
            this.liftMaxGuard = liftMaxGuard;
        }
        public new Feint Clone()
        {
            return new Feint(
                displayText: displayText, displayTextMatBlock: displayTextMatBlock,
                liftMaxGuard: liftMaxGuard
                );
        }
    }

    /// <summary>
    /// Hits between a range of hits by chance.
    /// </summary>
    public class FuryAttack : MoveEffect
    {
        /// <summary>
        /// The lowest amount of hits possible for this move;
        /// </summary>
        public int lowestHits;
        /// <summary>
        /// The length of this + <seealso cref="lowestHits"/> determines the highest possible hits. Each index i
        /// represents the probability of getting hits: i + <seealso cref="lowestHits"/>. These probabilities
        /// are automatically normalized.
        /// </summary>
        public List<float> hitChances;

        public FuryAttack(
            int lowestHits = 2,
            IEnumerable<float> hitChances = null
            )
            : base(effectType: MoveEffectType.FuryAttack)
        {
            this.lowestHits = lowestHits;
            this.hitChances = hitChances != null ? new List<float>(hitChances)
                : new List<float> { 1f / 3, 1f / 3, 1f / 6, 1f / 6 };
        }
        public new FuryAttack Clone()
        {
            return new FuryAttack(lowestHits: lowestHits, hitChances: hitChances);
        }

    }

    /// <summary>
    /// Modifies this move's priority during certain battle conditions. 
    /// </summary>
    public class GrassyGlide : MoveEffect
    {
        public enum PriorityMode
        {
            Add,
            Set,
        }
        /// <summary>
        /// The mode by which priority is determined.
        /// </summary>
        public PriorityMode mode;
        /// <summary>
        /// Is added onto existing priority with <seealso cref="PriorityMode.Add"/>, and is hard set
        /// with <seealso cref="PriorityMode.Set"/>.
        /// </summary>
        public int priority;

        /// <summary>
        /// The relevant battle conditions.
        /// </summary>
        public List<string> conditions;

        public GrassyGlide(
            PriorityMode mode = PriorityMode.Add, int priority = 0,
            IEnumerable<string> conditions = null
            )
            : base(effectType: MoveEffectType.GrassyGlide)
        {
            this.mode = mode;
            this.priority = priority;
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
        }
        public new GrassyGlide Clone()
        {
            return new GrassyGlide(mode: mode, priority: priority, conditions: conditions);
        }
    }

    /// <summary>
    /// This move is a one-hit KO (OHKO), instantly causing the target to faint.
    /// </summary>
    public class Guillotine : MoveEffect
    {
        /// <summary>
        /// The text displayed when this move connects.
        /// </summary>
        public string displayText;
        /// <summary>
        /// If true, this move does not work against higher-leveled targets.
        /// </summary>
        public bool upperBoundLevel;

        public Guillotine(
            string displayText = "move-guillotine",
            bool upperBoundLevel = true
            )
            : base(effectType: MoveEffectType.Guillotine)
        {
            this.displayText = displayText;
            this.upperBoundLevel = upperBoundLevel;
        }
        public new Guillotine Clone()
        {
            return new Guillotine(displayText: displayText, upperBoundLevel: upperBoundLevel);
        }
    }
    /// <summary>
    /// Overrides accuracy calculation as: (user.level - target.level) + baseAccuracy.
    /// </summary>
    public class GuillotineAccuracy : MoveEffect
    {
        public GuillotineAccuracy() : base(effectType: MoveEffectType.GuillotineAccuracy) { }
        public new GuillotineAccuracy Clone() { return new GuillotineAccuracy(); }
    }

    /// <summary>
    /// Immediately heals the user's specified status conditions right before the move is used.
    /// </summary>
    public class HealBeforeUse : MoveEffect
    {
        /// <summary>
        /// The statuses listed here are healed immediately before use.
        /// </summary>
        public List<string> statuses;

        public HealBeforeUse(
            IEnumerable<string> statuses = null
            )
            : base(effectType: MoveEffectType.HealBeforeUse)
        {
            this.statuses = statuses == null ? new List<string>() : new List<string>(statuses);
        }
        public new HealBeforeUse Clone()
        {
            return new HealBeforeUse(statuses: statuses);
        }
    }

    /// <summary>
    /// Overwrites base power depending on the user's weight relative to the target's weight.
    /// </summary>
    public class HeavySlam : MoveEffect
    {
        /// <summary>
        /// The lowest base power that this move can be - i.e. weight strictly lower than the first threshold
        /// in <seealso cref="relativeWeightThresholds"/>.
        /// </summary>
        public int lowestPower;
        /// <summary>
        /// The highest base power that this move can be - i.e. weight at least as high as the last threshold
        /// in <seealso cref="relativeWeightThresholds"/>.
        /// </summary>
        public int highestPower;

        /// <summary>
        /// The range of power between each threshold in <seealso cref="relativeWeightThresholds"/>.
        /// This is one item less as long as <seealso cref="relativeWeightThresholds"/>.
        /// </summary>
        public List<int> powerRange;
        /// <summary>
        /// The relative weight thresholds determining this move's base power.
        /// </summary>
        public List<float> relativeWeightThresholds;

        public HeavySlam(
            int lowestPower = 40, int highestPower = 120,
            IEnumerable<int> powerRange = null,
            IEnumerable<float> relativeWeightThresholds = null
            )
            : base(effectType: MoveEffectType.LowKick)
        {
            this.lowestPower = lowestPower;
            this.highestPower = highestPower;
            this.powerRange = powerRange == null ? new List<int>()
                : new List<int> { 60, 80, 100 };
            this.powerRange.Sort();
            this.relativeWeightThresholds = relativeWeightThresholds == null ? new List<float>()
                : new List<float> { 0.5f, 1f / 3, .25f, .2f };
            this.relativeWeightThresholds.Sort();
            this.relativeWeightThresholds.Reverse();
        }
        public new HeavySlam Clone()
        {
            return new HeavySlam(
                lowestPower: lowestPower, highestPower: highestPower,
                powerRange: powerRange, relativeWeightThresholds: relativeWeightThresholds
                );
        }
    }

    /// <summary>
    /// Changes the move's type and base power according to the user's IVs. Both these features
    /// and types available are fully customizable.
    /// </summary>
    public class HiddenPower : MoveEffect
    {
        /// <summary>
        /// Set to true if this move will change the base power of the move. This is done using
        /// the user's IVs.
        /// </summary>
        public bool calculateDamage;
        /// <summary>
        /// Set to true if this move will change the type of the move. This is done using
        /// the user's IVs.
        /// </summary>
        public bool calculateType;

        /// <summary>
        /// If <seealso cref="calculateDamage"/> is true, this is the lowest base power possible.
        /// </summary>
        public int lowestBasePower;
        /// <summary>
        /// If <seealso cref="calculateDamage"/> is true, this is the highest base power possible.
        /// </summary>
        public int highestBasePower;

        /// <summary>
        /// If <seealso cref="calculateType"/> is true, these are the possible types that the move can be.
        /// The order of the types listed matters.
        /// </summary>
        public List<string> types;

        public HiddenPower(
            bool calculateDamage = false, bool calculateType = true,
            int lowestBasePower = 30, int highestBasePower = 70,
            IEnumerable<string> types = null
            )
            : base(effectType: MoveEffectType.HiddenPower)
        {
            this.calculateDamage = calculateDamage;
            this.calculateType = calculateType;
            this.lowestBasePower = lowestBasePower;
            this.highestBasePower = highestBasePower;
            this.types = types != null ? new List<string>(types)
                : new List<string>
                {
                // Default Hidden Power
                "fighting", "flying", "poison", "ground", "rock", "bug", "ghost", "steel",
                "fire", "water", "grass", "electric", "psychic", "ice", "dragon", "dark",
                };
        }
        public new HiddenPower Clone()
        {
            return new HiddenPower(
                calculateDamage: calculateDamage, calculateType: calculateType,
                lowestBasePower: lowestBasePower, highestBasePower: highestBasePower,
                types: types
                );
        }
    }

    /// <summary>
    /// Induces a status condition on the specified type of target (Pokémon, Team, or Battle).
    /// </summary>
    public class InflictStatus : MoveEffect
    {
        public General.InflictStatus inflictStatus;

        public InflictStatus(
            General.InflictStatus inflictStatus,

            MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
            MoveEffectTargetType targetType = MoveEffectTargetType.Target,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,
            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false
            )
            : base(
                    effectType: MoveEffectType.InflictStatus, timing: timing, targetType: targetType,
                    occurrence: occurrence, filters: filters,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay)
        {
            this.inflictStatus = inflictStatus.Clone();
        }
        public new InflictStatus Clone()
        {
            return new InflictStatus(
                inflictStatus: inflictStatus,
                timing: timing, targetType: targetType, filters: filters, occurrence: occurrence,
                chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                );
        }
    }

    /// <summary>
    /// Changes the move's type if the user's held item has the effect <seealso cref="Items.Judgment"/>.
    /// </summary>
    public class Judgment : MoveEffect
    {
        public Judgment() : base(effectType: MoveEffectType.Judgment) { }
        public new Judgment Clone()
        {
            return new Judgment();
        }
    }

    /// <summary>
    /// Increases the base critical hit rate for this move.
    /// </summary>
    public class KarateChop : MoveEffect
    {
        /// <summary>
        /// If true, this move always results in critical hits.
        /// </summary>
        public bool alwaysCritical;
        /// <summary>
        /// The critical level added on to this move.
        /// </summary>
        public int criticalBoost;

        public KarateChop(
            bool alwaysCritical = false, int criticalBoost = 1
            )
            : base(effectType: MoveEffectType.KarateChop)
        {
            this.alwaysCritical = alwaysCritical;
            this.criticalBoost = criticalBoost;
        }
        public new KarateChop Clone()
        {
            return new KarateChop(alwaysCritical: alwaysCritical, criticalBoost: criticalBoost);
        }
    }

    /// <summary>
    /// Removes the target's held item (if possible). If the item is able to be removed, this attack gets
    /// a boost in power.
    /// </summary>
    public class KnockOff : MoveEffect
    {
        /// <summary>
        /// The text displayed when an item is knocked off.
        /// </summary>
        public string displayText;
        /// <summary>
        /// The amount by which the base power is scaled if the item can be knocked off.
        /// </summary>
        public float damageScale;

        public KnockOff(
            string displayText = "move-knockoff",
            float damageScale = 1.5f
            )
            : base(effectType: MoveEffectType.KnockOff)
        {
            this.displayText = displayText;
            this.damageScale = damageScale;
        }
        public new KnockOff Clone()
        {
            return new KnockOff(displayText: displayText, damageScale: damageScale);
        }
    }

    /// <summary>
    /// Scales base power if the user's stats were lowered during the turn.
    /// </summary>
    public class LashOut : BasePowerMultiplier
    {
        public LashOut(float powerScale = 2f)
            : base(powerScale: powerScale,
                    filters: new Filter.FilterEffect[]
                    {
                    new Filter.BurningJealousy(
                        targetType: Filter.BurningJealousy.TargetType.Self,
                        ATKMod: Filter.BurningJealousy.StatChangeType.Lower, DEFMod: Filter.BurningJealousy.StatChangeType.Lower,
                        SPAMod: Filter.BurningJealousy.StatChangeType.Lower, SPDMod: Filter.BurningJealousy.StatChangeType.Lower,
                        SPEMod: Filter.BurningJealousy.StatChangeType.Lower,
                        ACCMod: Filter.BurningJealousy.StatChangeType.Lower, EVAMod: Filter.BurningJealousy.StatChangeType.Lower
                        ),
                    }
                    )
        { }
    }

    /// <summary>
    /// Overwrites base power depending on the target's weight
    /// </summary>
    public class LowKick : MoveEffect
    {
        /// <summary>
        /// The lowest base power that this move can be - i.e. weight strictly lower than the first threshold
        /// in <seealso cref="weightThresholds"/>.
        /// </summary>
        public int lowestPower;
        /// <summary>
        /// The highest base power that this move can be - i.e. weight at least as high as the last threshold
        /// in <seealso cref="weightThresholds"/>.
        /// </summary>
        public int highestPower;

        /// <summary>
        /// The range of power between each threshold in <seealso cref="weightThresholds"/>.
        /// This is one item less as long as <seealso cref="weightThresholds"/>.
        /// </summary>
        public List<int> powerRange;
        /// <summary>
        /// The weight thresholds determining this move's base power.
        /// </summary>
        public List<float> weightThresholds;

        public LowKick(
            int lowestPower = 20, int highestPower = 120,
            IEnumerable<int> powerRange = null,
            IEnumerable<float> weightThresholds = null
            )
            : base(effectType: MoveEffectType.LowKick)
        {
            this.lowestPower = lowestPower;
            this.highestPower = highestPower;
            this.powerRange = powerRange == null ? new List<int>()
                : new List<int> { 40, 60, 80, 100 };
            this.powerRange.Sort();
            this.weightThresholds = weightThresholds == null ? new List<float>()
                : new List<float> { 10f, 25f, 50f, 100f, 200f };
            this.weightThresholds.Sort();
        }
        public new LowKick Clone()
        {
            return new LowKick(
                lowestPower: lowestPower, highestPower: highestPower,
                powerRange: powerRange, weightThresholds: weightThresholds
                );
        }
    }

    /// <summary>
    /// Reflects certain moves back to their attackers.
    /// </summary>
    public class MagicCoat : MoveEffect
    {
        /// <summary>
        /// Defines how moves are reflected.
        /// </summary>
        public General.MagicCoat magicCoat;

        public MagicCoat(
            General.MagicCoat magicCoat,

            MoveEffectTargetType targetType = MoveEffectTargetType.Self,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.Once,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: MoveEffectType.MagicCoat,
                    timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                    targetType: targetType, occurrence: occurrence,
                    filters: filters)
        {
            this.magicCoat = magicCoat.Clone();
        }
        public new MagicCoat Clone()
        {
            return new MagicCoat(
                magicCoat: magicCoat,

                targetType: targetType, occurrence: occurrence,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Randomly determines the strength for a move from a selection of levels, each level resulting in a different
    /// base power.
    /// </summary>
    public class Magnitude : MoveEffect
    {
        /// <summary>
        /// The different magnitude levels, each with their own power and probability.
        /// </summary>
        public List<MagnitudeLevel> magnitudeLevels;
        /// <summary>
        /// The text displayed for a magnitude level.
        /// </summary>
        public string displayText;

        public Magnitude(
            IEnumerable<MagnitudeLevel> magnitudeLevels = null, string displayText = "move-magnitude"
            )
            : base(effectType: MoveEffectType.Magnitude)
        {
            this.magnitudeLevels = new List<MagnitudeLevel>();
            this.displayText = displayText;
            if (magnitudeLevels != null)
            {
                this.magnitudeLevels = new List<MagnitudeLevel>(magnitudeLevels);
            }
            else
            {
                this.magnitudeLevels = new List<MagnitudeLevel>
            {
                new MagnitudeLevel(level: 4, basePower: 10, chance: 0.05f),
                new MagnitudeLevel(level: 5, basePower: 30, chance: 0.1f),
                new MagnitudeLevel(level: 6, basePower: 50, chance: 0.2f),
                new MagnitudeLevel(level: 7, basePower: 70, chance: 0.3f),
                new MagnitudeLevel(level: 8, basePower: 90, chance: 0.2f),
                new MagnitudeLevel(level: 9, basePower: 110, chance: 0.1f),
                new MagnitudeLevel(level: 10, basePower: 150, chance: 0.05f),
            };
            }
        }
        public new Magnitude Clone()
        {
            return new Magnitude(magnitudeLevels: magnitudeLevels, displayText: displayText);
        }
        public MagnitudeLevel GetAMagnitudeLevel()
        {
            MagnitudeLevel level = magnitudeLevels.Count == 0 ? null : magnitudeLevels[0];

            List<float> levelChances = new List<float>();
            float totalChance = 0;
            for (int i = 0; i < magnitudeLevels.Count; i++)
            {
                totalChance += magnitudeLevels[i].chance;
                levelChances.Add(totalChance);
            }

            // Normalize level chances
            if (totalChance > 0)
            {
                for (int i = 0; i < levelChances.Count; i++)
                {
                    levelChances[i] /= totalChance;
                }

                // Calculate level by chance
                float randValue = Random.value;
                for (int i = 0; i < levelChances.Count; i++)
                {
                    if (randValue <= levelChances[i])
                    {
                        return magnitudeLevels[i];
                    }
                    else if (i == levelChances.Count - 1)
                    {
                        return magnitudeLevels[i];
                    }
                }
            }

            return level;
        }
        public class MagnitudeLevel
        {
            public int level;
            public int basePower;
            public float chance;
            public MagnitudeLevel(int level = 4, int basePower = 10, float chance = 0.05f)
            {
                this.level = level;
                this.basePower = basePower;
                this.chance = chance;
            }
            public MagnitudeLevel Clone()
            {
                return new MagnitudeLevel(level: level, basePower: basePower, chance: chance);
            }
        }
    }

    /// <summary>
    /// Specialized version of protect that is suited for team-based protection.
    /// </summary>
    public class MatBlock : Protect
    {
        public MatBlock(
            General.Protect protect,
            IEnumerable<Filter.FilterEffect> filters = null)
            : base(
                    protect: protect,
                    targetType: MoveEffectTargetType.SelfTeam, occurrence: MoveEffectOccurrence.Once,
                    filters: filters)
        {

        }
    }

    /// <summary>
    /// Changes the move's type and power based on the berry held. Fails if the user is not
    /// holding a berry, or can't use its berry.
    /// </summary>
    public class NaturalGift : MoveEffect
    {
        public NaturalGift() : base(effectType: MoveEffectType.NaturalGift)
        { }
        public new NaturalGift Clone()
        {
            return new NaturalGift();
        }
    }

    /// <summary>
    /// Becomes a <seealso cref="MoveCategory.Physical"/> move if the user's Attack is higher than its Special
    /// Attack. Becomes <seealso cref="MoveCategory.Special"/> move if the user's Special Attack is higher.
    /// </summary>
    public class PhotonGeyser : MoveEffect
    {
        public PhotonGeyser() : base(effectType: MoveEffectType.PhotonGeyser)
        { }
        public new PhotonGeyser Clone()
        {
            return new PhotonGeyser();
        }
    }

    /// <summary>
    /// Attacks using the target's item.
    /// </summary>
    public class Poltergeist : MoveEffect
    {
        /// <summary>
        /// Set this flag to cause this move to fail if the target doesn't have any held item.
        /// </summary>
        public bool failOnNoItem;
        /// <summary>
        /// Text displayed when using the target's item.
        /// </summary>
        public string displayText;

        public Poltergeist(
            bool failOnNoItem = true,
            string displayText = "move-poltergeist"
            )
            : base(effectType: MoveEffectType.Poltergeist)
        {
            this.failOnNoItem = failOnNoItem;
            this.displayText = displayText;
        }
        public new Poltergeist Clone()
        {
            return new Poltergeist(failOnNoItem: failOnNoItem, displayText: displayText);
        }
    }

    /// <summary>
    /// Protects the user from most moves. Less likely to succeed with consecutive uses.
    /// </summary>
    public class Protect : MoveEffect
    {
        /// <summary>
        /// Defines how the user is protected.
        /// </summary>
        public General.Protect protect;

        public Protect(
            General.Protect protect,

            MoveEffectTargetType targetType = MoveEffectTargetType.Self,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.Once,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: MoveEffectType.Protect,
                    timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                    targetType: targetType, occurrence: occurrence, filters: filters)
        {
            this.protect = protect.Clone();
        }
        public new Protect Clone()
        {
            return new Protect(
                protect: protect,
                targetType: targetType, occurrence: occurrence, filters: filters
                );
        }
    }

    /// <summary>
    /// Inflicts a random set amount of damage that factors in the user's level.
    /// </summary>
    public class Psywave : MoveEffect
    {
        /// <summary>
        /// The lowest scaling value to apply to the damage.
        /// </summary>
        public int lowestScaleValue;

        public Psywave(int lowestScaleValue = 50) : base(effectType: MoveEffectType.Psywave)
        {
            this.lowestScaleValue = lowestScaleValue;
        }
        public new Psywave Clone()
        {
            return new Psywave(lowestScaleValue: lowestScaleValue);
        }
    }

    /// <summary>
    /// Increases base power for each positive stat boost.
    /// </summary>
    public class Punishment : MoveEffect
    {
        /// <summary>
        /// The lowest base power for this move.
        /// </summary>
        public int minimumPower;
        /// <summary>
        /// The highest base power for this move.
        /// </summary>
        public int maximumPower;

        /// <summary>
        /// The base power boost given for positive stat stage changes for this stat.
        /// </summary>
        public int ATKPlus, DEFPlus, SPAPlus, SPDPlus, SPEPlus, ACCPlus, EVAPlus;
        /// <summary>
        /// The base power boost given for negative stat stage changes for this stat.
        /// </summary>
        public int ATKMinus, DEFMinus, SPAMinus, SPDMinus, SPEMinus, ACCMinus, EVAMinus;

        public Punishment(
            int minimumPower = 60, int maximumPower = 200,
            int ATKPlus = 20, int DEFPlus = 20, int SPAPlus = 20, int SPDPlus = 20,
            int SPEPlus = 20, int ACCPlus = 20, int EVAPlus = 20,
            int ATKMinus = 0, int DEFMinus = 0, int SPAMinus = 0, int SPDMinus = 0,
            int SPEMinus = 0, int ACCMinus = 0, int EVAMinus = 0
            )
            : base(effectType: MoveEffectType.Punishment)
        {
            this.minimumPower = minimumPower;
            this.maximumPower = maximumPower;

            this.ATKPlus = ATKPlus;
            this.DEFPlus = DEFPlus;
            this.SPAPlus = SPAPlus;
            this.SPDPlus = SPDPlus;
            this.SPEPlus = SPEPlus;
            this.ACCPlus = ACCPlus;
            this.EVAPlus = EVAPlus;

            this.ATKMinus = ATKMinus;
            this.DEFMinus = DEFMinus;
            this.SPAMinus = SPAMinus;
            this.SPDMinus = SPDMinus;
            this.SPEMinus = SPEMinus;
            this.ACCMinus = ACCMinus;
            this.EVAMinus = EVAMinus;
        }
        public new Punishment Clone()
        {
            return new Punishment(
                minimumPower: minimumPower, maximumPower: maximumPower,
                ATKPlus: ATKPlus, DEFPlus: DEFPlus, SPAPlus: SPAPlus, SPDPlus: SPDPlus,
                SPEPlus: SPEPlus, ACCPlus: ACCPlus, EVAPlus: EVAPlus,
                ATKMinus: ATKMinus, DEFMinus: DEFMinus, SPAMinus: SPAMinus, SPDMinus: SPDMinus,
                SPEMinus: SPEMinus, ACCMinus: ACCMinus, EVAMinus: EVAMinus
                );
        }
        public int GetBasePower(Main.Pokemon.Pokemon pokemon)
        {
            int basePower = minimumPower;

            // Attack
            if (pokemon.bProps.ATKStage > 0)
            {
                basePower += ATKPlus * pokemon.bProps.ATKStage;
            }
            else if (pokemon.bProps.ATKStage < 0)
            {
                basePower += ATKMinus * Mathf.Abs(pokemon.bProps.ATKStage);
            }

            // Defense
            if (pokemon.bProps.DEFStage > 0)
            {
                basePower += DEFPlus * pokemon.bProps.DEFStage;
            }
            else if (pokemon.bProps.DEFStage < 0)
            {
                basePower += DEFMinus * Mathf.Abs(pokemon.bProps.DEFStage);
            }

            // Special Attack
            if (pokemon.bProps.SPAStage > 0)
            {
                basePower += SPAPlus * pokemon.bProps.SPAStage;
            }
            else if (pokemon.bProps.SPAStage < 0)
            {
                basePower += SPAMinus * Mathf.Abs(pokemon.bProps.SPAStage);
            }

            // Special Defense
            if (pokemon.bProps.SPDStage > 0)
            {
                basePower += SPDPlus * pokemon.bProps.SPDStage;
            }
            else if (pokemon.bProps.SPDStage < 0)
            {
                basePower += SPDMinus * Mathf.Abs(pokemon.bProps.SPDStage);
            }

            // Speed
            if (pokemon.bProps.SPEStage > 0)
            {
                basePower += SPEPlus * pokemon.bProps.SPEStage;
            }
            else if (pokemon.bProps.SPEStage < 0)
            {
                basePower += SPEMinus * Mathf.Abs(pokemon.bProps.SPEStage);
            }

            // Accuracy
            if (pokemon.bProps.ACCStage > 0)
            {
                basePower += ACCPlus * pokemon.bProps.ACCStage;
            }
            else if (pokemon.bProps.ACCStage < 0)
            {
                basePower += ACCMinus * Mathf.Abs(pokemon.bProps.ACCStage);
            }

            // Evasion
            if (pokemon.bProps.EVAStage > 0)
            {
                basePower += EVAPlus * pokemon.bProps.EVAStage;
            }
            else if (pokemon.bProps.EVAStage < 0)
            {
                basePower += EVAMinus * Mathf.Abs(pokemon.bProps.EVAStage);
            }

            return Mathf.Clamp(basePower, minimumPower, maximumPower);
        }
    }

    /// <summary>
    /// Allows for this move to hit its target right before it can switch out.
    /// </summary>
    public class Pursuit : MoveEffect
    {
        /// <summary>
        /// The amount by which base power is scaled for a target switching out.
        /// </summary>
        public float damageScale;
        /// <summary>
        /// This effect can apply to enemy Pokémon.
        /// </summary>
        public bool applyToEnemies;
        /// <summary>
        /// This effect can apply to ally Pokémon.
        /// </summary>
        public bool applyToAllies;

        public Pursuit(
            float damageScale = 2f,
            bool applyToEnemies = true, bool applyToAllies = false)
            : base(effectType: MoveEffectType.Pursuit)
        {
            this.damageScale = damageScale;
            this.applyToEnemies = applyToEnemies;
            this.applyToAllies = applyToAllies;
        }
        public new Pursuit Clone()
        {
            return new Pursuit(
                damageScale: damageScale,
                applyToEnemies: applyToEnemies, applyToAllies: applyToAllies);
        }
    }

    /// <summary>
    /// Heals the target's specified status conditions.
    /// </summary>
    public class Refresh : MoveEffect
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
        /// The text displayed when refresh fails to heal any status.
        /// </summary>
        public string failText;

        public Refresh(
            IEnumerable<string> statuses = null,
            IEnumerable<PokemonSEType> statusEffectTypes = null,
            string failText = "move-refresh-fail",

            MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
            MoveEffectTargetType targetType = MoveEffectTargetType.Target,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,
            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false
            )
            : base(effectType: MoveEffectType.Refresh, timing: timing, targetType: targetType,
                    occurrence: occurrence, filters: filters,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay)
        {
            this.statuses = statuses == null ? new List<string>() : new List<string>(statuses);
            this.statusEffectTypes = statusEffectTypes == null ? new HashSet<PokemonSEType>()
                : new HashSet<PokemonSEType>(statusEffectTypes);
            this.failText = failText;
        }
        public new Refresh Clone()
        {
            return new Refresh(
                statuses: statuses,
                statusEffectTypes: statusEffectTypes,
                failText: failText,
                timing: timing, targetType: targetType, filters: filters, occurrence: occurrence,
                chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                );
        }
    }

    /// <summary>
    /// Alternates the user's forms after successful use, if it is one of the given forms.
    /// </summary>
    public class RelicSong : MoveEffect
    {
        /// <summary>
        /// Pokémon ID 1.
        /// </summary>
        public string form1;
        /// <summary>
        /// Pokémon ID 2.
        /// </summary>
        public string form2;
        /// <summary>
        /// The text that displays after the form change.
        /// </summary>
        public string afterText;

        public RelicSong(
            string form1, string form2,
            string afterText = "pokemon-changeform"
            )
            : base(effectType: MoveEffectType.RelicSong)
        {
            this.form1 = form1;
            this.form2 = form2;
            this.afterText = afterText;
        }
        public new RelicSong Clone()
        {
            return new RelicSong(
                form1: form1, form2: form2,
                afterText: afterText
                );
        }
    }

    /// <summary>
    /// Determines base power based on the user's remaining HP %. The HP % must be below specified thresholds
    /// to determine base power.
    /// </summary>
    public class Reversal : MoveEffect
    {
        public class ReversalPower
        {
            public int basePower;
            public float HPThreshold;

            public ReversalPower(int basePower = 40, float HPThreshold = 0.6875f)
            {
                this.basePower = basePower;
                this.HPThreshold = HPThreshold;
            }
            public ReversalPower Clone()
            {
                return new ReversalPower(basePower: basePower, HPThreshold: HPThreshold);
            }
        }

        /// <summary>
        /// The lowest possible base power for this move.
        /// </summary>
        public int lowestBasePower;

        /// <summary>
        /// An list of reversal base powers ordered descending by the HP threshold needed to obtain that power.
        /// </summary>
        public List<ReversalPower> reversalPowers;

        public Reversal(
            int lowestBasePower = 20,
            IEnumerable<ReversalPower> reversalPowers = null
            )
            : base(effectType: MoveEffectType.Reversal)
        {
            this.reversalPowers = reversalPowers != null ? new List<ReversalPower>(reversalPowers)
                : new List<ReversalPower>
                {
                new ReversalPower(basePower: 40, HPThreshold: 0.6875f),
                new ReversalPower(basePower: 80, HPThreshold: 0.3542f),
                new ReversalPower(basePower: 100, HPThreshold: 0.2083f),
                new ReversalPower(basePower: 150, HPThreshold: 0.1042f),
                new ReversalPower(basePower: 200, HPThreshold: 0.0417f),
                };
        }
        public new Reversal Clone()
        {
            return new Reversal(lowestBasePower: lowestBasePower, reversalPowers: reversalPowers);
        }
    }

    /// <summary>
    /// Increases power if the target is affected by the given battle conditions.
    /// </summary>
    public class RisingVoltage : MoveEffect
    {
        /// <summary>
        /// The amount that damage is scaled by.
        /// </summary>
        public float damageScale;
        /// <summary>
        /// The relevant battle conditions.
        /// </summary>
        public List<string> conditions;

        public RisingVoltage(
            float damageScale = 1f,
            IEnumerable<string> conditions = null
            )
            : base(effectType: MoveEffectType.RisingVoltage)
        {
            this.damageScale = damageScale;
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
        }
        public new RisingVoltage Clone()
        {
            return new RisingVoltage(damageScale: damageScale, conditions: conditions);
        }
    }

    /// <summary>
    /// The move is used consecutively for a set amount of turns, and consecutively scales
    /// for each consecutive execution.
    /// </summary>
    public class Rollout : MoveEffect
    {
        /// <summary>
        /// The amount by which damage is scaled for a consecutive hit.
        /// </summary>
        public float damageScale;
        /// <summary>
        /// The maximum amount of move executions before the damage scale resets, or the move ends.
        /// WARNING: Setting it to -1 will use the move for an infinite amount of turns.
        /// </summary>
        public int maxExecutions;

        /// <summary>
        /// Unsets this move if the move was disrupted.
        /// </summary>
        public bool endOnFail;
        /// <summary>
        /// Unsets this move if it is on the maximum execution.
        /// </summary>
        public bool endOnMaxHits;

        public Rollout(
            float damageScale = 2f,
            int maxExecutions = 5,
            bool endOnFail = true, bool endOnMaxHits = false
            )
            : base(effectType: MoveEffectType.Rollout)
        {
            this.damageScale = damageScale;
            this.maxExecutions = maxExecutions;
            this.endOnFail = endOnFail;
            this.endOnMaxHits = endOnMaxHits;
        }
        public new Rollout Clone()
        {
            return new Rollout(
                damageScale: damageScale,
                maxExecutions: maxExecutions,
                endOnFail: endOnFail, endOnMaxHits: endOnMaxHits
                );
        }
    }

    /// <summary>
    /// Adds secondary effects to this move depending on the environment or terrain.
    /// </summary>
    public class SecretPower : MoveEffect
    {
        /// <summary>
        /// The chance for each additional effect to occur.
        /// </summary>
        public float secondaryEffectChance;

        public SecretPower(
            float secondaryEffectChance = -1,

            MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
            MoveEffectTargetType targetType = MoveEffectTargetType.Target,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,
            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false
            )
            : base(
                    effectType: MoveEffectType.InflictPokemonSC, timing: timing, targetType: targetType,
                    filters: filters,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay)
        {
            this.secondaryEffectChance = secondaryEffectChance;
        }
        public new SecretPower Clone()
        {
            return new SecretPower(
                secondaryEffectChance: secondaryEffectChance,
                timing: timing, targetType: targetType, filters: filters,
                chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                );
        }
    }

    /// <summary>
    /// Makes this move deal a set amount of damage equal to the user's level.
    /// </summary>
    public class SeismicToss : MoveEffect
    {
        public SeismicToss() : base(effectType: MoveEffectType.SeismicToss) { }
        public new SeismicToss Clone()
        {
            return new SeismicToss();
        }
    }

    /// <summary>
    /// Becomes a <seealso cref="MoveCategory.Physical"/> move if the target's Defense is lower than its Special
    /// Defense. Becomes <seealso cref="MoveCategory.Special"/> move if the target's Special Defense is lower.
    /// </summary>
    public class ShellSideArm : MoveEffect
    {
        public ShellSideArm() : base(effectType: MoveEffectType.ShellSideArm)
        { }
        public new ShellSideArm Clone()
        {
            return new ShellSideArm();
        }
    }

    /// <summary>
    /// Enables the use of a move when the Pokémon is asleep.
    /// </summary>
    public class Snore : MoveEffect
    {
        /// <summary>
        /// If true, this move can only be used when the Pokémon is asleep.
        /// </summary>
        public bool onlyAsleep;

        public Snore(
            bool onlyAsleep = true
            )
            : base(effectType: MoveEffectType.Snore)
        {
            this.onlyAsleep = onlyAsleep;
        }
        public new Snore Clone()
        {
            return new Snore(onlyAsleep: onlyAsleep);
        }
    }

    /// <summary>
    /// Adds or Subtracts the stat stages of the chosen target Pokémon.
    /// </summary>
    public class StatStageMod : MoveEffect
    {
        public General.StatStageMod statStageMod;

        public StatStageMod(
            General.StatStageMod statStageMod,

            MoveEffectTiming timing = MoveEffectTiming.AfterTargetImpact,
            MoveEffectTargetType targetType = MoveEffectTargetType.Target,
            MoveEffectOccurrence occurrence = MoveEffectOccurrence.OnceForEachTarget,
            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1, bool oneTimeChance = false, bool forceEffectDisplay = false
            )
            : base(
                    effectType: MoveEffectType.InflictPokemonSC, timing: timing, targetType: targetType,
                    occurrence: occurrence, filters: filters,
                    chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new StatStageMod Clone()
        {
            return new StatStageMod(
                statStageMod: statStageMod,

                timing: timing, targetType: targetType, filters: filters, occurrence: occurrence,
                chance: chance, oneTimeChance: oneTimeChance, forceEffectDisplay: forceEffectDisplay
                );
        }
    }

    /// <summary>
    /// Immediately destroys the existing terrain.
    /// </summary>
    public class SteelRoller : MoveEffect
    {
        /// <summary>
        /// The text displayed when the terrain is destroyed.
        /// </summary>
        public string displayText;
        /// <summary>
        /// If true, this move fails when there's no terrain.
        /// </summary>
        public bool failOnNoTerrain;

        public SteelRoller(
            string displayText = "move-steelroller",
            bool failOnNoTerrain = true
            )
            : base(effectType: MoveEffectType.SteelRoller,
                    targetType: MoveEffectTargetType.Battlefield,
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.Once)
        {
            this.displayText = displayText;
            this.failOnNoTerrain = failOnNoTerrain;
        }
        public new SteelRoller Clone()
        {
            return new SteelRoller(displayText: displayText, failOnNoTerrain: failOnNoTerrain);
        }
    }

    /// <summary>
    /// Increases base power for each positive stat boost.
    /// </summary>
    public class StoredPower : MoveEffect
    {
        /// <summary>
        /// The base power boost given for positive stat stage changes for this stat.
        /// </summary>
        public int ATKPlus, DEFPlus, SPAPlus, SPDPlus, SPEPlus, ACCPlus, EVAPlus;
        /// <summary>
        /// The base power boost given for negative stat stage changes for this stat.
        /// </summary>
        public int ATKMinus, DEFMinus, SPAMinus, SPDMinus, SPEMinus, ACCMinus, EVAMinus;

        public StoredPower(
            int ATKPlus = 20, int DEFPlus = 20, int SPAPlus = 20, int SPDPlus = 20,
            int SPEPlus = 20, int ACCPlus = 20, int EVAPlus = 20,
            int ATKMinus = 0, int DEFMinus = 0, int SPAMinus = 0, int SPDMinus = 0,
            int SPEMinus = 0, int ACCMinus = 0, int EVAMinus = 0
            )
            : base(effectType: MoveEffectType.StoredPower)
        {
            this.ATKPlus = ATKPlus;
            this.DEFPlus = DEFPlus;
            this.SPAPlus = SPAPlus;
            this.SPDPlus = SPDPlus;
            this.SPEPlus = SPEPlus;
            this.ACCPlus = ACCPlus;
            this.EVAPlus = EVAPlus;

            this.ATKMinus = ATKMinus;
            this.DEFMinus = DEFMinus;
            this.SPAMinus = SPAMinus;
            this.SPDMinus = SPDMinus;
            this.SPEMinus = SPEMinus;
            this.ACCMinus = ACCMinus;
            this.EVAMinus = EVAMinus;
        }
        public new StoredPower Clone()
        {
            return new StoredPower(
                ATKPlus: ATKPlus, DEFPlus: DEFPlus, SPAPlus: SPAPlus, SPDPlus: SPDPlus,
                SPEPlus: SPEPlus, ACCPlus: ACCPlus, EVAPlus: EVAPlus,
                ATKMinus: ATKMinus, DEFMinus: DEFMinus, SPAMinus: SPAMinus, SPDMinus: SPDMinus,
                SPEMinus: SPEMinus, ACCMinus: ACCMinus, EVAMinus: EVAMinus
                );
        }
        public int GetPowerBoost(Main.Pokemon.Pokemon pokemon)
        {
            int basePower = 0;

            // Attack
            if (pokemon.bProps.ATKStage > 0)
            {
                basePower += ATKPlus * pokemon.bProps.ATKStage;
            }
            else if (pokemon.bProps.ATKStage < 0)
            {
                basePower += ATKMinus * Mathf.Abs(pokemon.bProps.ATKStage);
            }

            // Defense
            if (pokemon.bProps.DEFStage > 0)
            {
                basePower += DEFPlus * pokemon.bProps.DEFStage;
            }
            else if (pokemon.bProps.DEFStage < 0)
            {
                basePower += DEFMinus * Mathf.Abs(pokemon.bProps.DEFStage);
            }

            // Special Attack
            if (pokemon.bProps.SPAStage > 0)
            {
                basePower += SPAPlus * pokemon.bProps.SPAStage;
            }
            else if (pokemon.bProps.SPAStage < 0)
            {
                basePower += SPAMinus * Mathf.Abs(pokemon.bProps.SPAStage);
            }

            // Special Defense
            if (pokemon.bProps.SPDStage > 0)
            {
                basePower += SPDPlus * pokemon.bProps.SPDStage;
            }
            else if (pokemon.bProps.SPDStage < 0)
            {
                basePower += SPDMinus * Mathf.Abs(pokemon.bProps.SPDStage);
            }

            // Speed
            if (pokemon.bProps.SPEStage > 0)
            {
                basePower += SPEPlus * pokemon.bProps.SPEStage;
            }
            else if (pokemon.bProps.SPEStage < 0)
            {
                basePower += SPEMinus * Mathf.Abs(pokemon.bProps.SPEStage);
            }

            // Accuracy
            if (pokemon.bProps.ACCStage > 0)
            {
                basePower += ACCPlus * pokemon.bProps.ACCStage;
            }
            else if (pokemon.bProps.ACCStage < 0)
            {
                basePower += ACCMinus * Mathf.Abs(pokemon.bProps.ACCStage);
            }

            // Evasion
            if (pokemon.bProps.EVAStage > 0)
            {
                basePower += EVAPlus * pokemon.bProps.EVAStage;
            }
            else if (pokemon.bProps.EVAStage < 0)
            {
                basePower += EVAMinus * Mathf.Abs(pokemon.bProps.EVAStage);
            }

            return basePower;
        }
    }

    /// <summary>
    /// Specialized version of Karate Chop that guarantees critical hits.
    /// </summary>
    public class StormThrow : KarateChop
    {
        public StormThrow() : base(alwaysCritical: true) { }
    }

    /// <summary>
    /// The move immediately fails to be used if the target does not use a physical or special attack
    /// or has already moved this turn.
    /// </summary>
    public class SuckerPunch : MoveEffect
    {
        /// <summary>
        /// The target must select a move in one of these categories.
        /// </summary>
        public HashSet<MoveCategory> categories;
        /// <summary>
        /// Any moves used here will also make Sucker Punch valid.
        /// </summary>
        public List<string> allowableMoves;
        /// <summary>
        /// Invert filters to invert the allowable moves, including additional filters set.
        /// </summary>
        public bool invertFilter;

        public SuckerPunch(
            IEnumerable<MoveCategory> categories = null,
            IEnumerable<string> allowableMoves = null,
            bool invertFilter = false
            )
            : base(effectType: MoveEffectType.SuckerPunch)
        {
            this.categories = categories == null
                ? new HashSet<MoveCategory> { MoveCategory.Physical, MoveCategory.Special }
                : new HashSet<MoveCategory>(categories);
            this.allowableMoves = allowableMoves == null
                ? new List<string> { "mefirst" }
                : new List<string>(allowableMoves);
            this.invertFilter = invertFilter;
        }
        public new SuckerPunch Clone()
        {
            return new SuckerPunch(
                categories: categories,
                allowableMoves: allowableMoves,
                invertFilter: invertFilter
                );
        }
    }

    /// <summary>
    /// This move ignores the effects of ignorable abilities.
    /// </summary>
    public class SunsteelStrike : MoveEffect
    {
        public SunsteelStrike() : base(effectType: MoveEffectType.SunteelStrike)
        { }
        public new SunsteelStrike Clone()
        {
            return new SunsteelStrike();
        }
    }

    /// <summary>
    /// Makes this move deal a set amount of damage equal to a percentage of the target's remaining HP.
    /// </summary>
    public class SuperFang : MoveEffect
    {
        /// <summary>
        /// The percentage of the target's remaining HP that is dealt as damage.
        /// </summary>
        public float damagePercent;

        public SuperFang(float damagePercent = 0.5f) : base(effectType: MoveEffectType.SuperFang)
        {
            this.damagePercent = damagePercent;
        }
        public new SuperFang Clone()
        {
            return new SuperFang(damagePercent: damagePercent);
        }
    }

    /// <summary>
    /// This move fails if the target doesn't share types with the user.
    /// </summary>
    public class Synchronoise : MoveEffect
    {
        /// <summary>
        /// Set to true to fail the move if the target's types don't exactly match the user's.
        /// </summary>
        public bool exactMatch;
        /// <summary>
        /// Set to true to invert the checks.
        /// </summary>
        public bool invert;
        /// <summary>
        /// Special text that displays on move failure.
        /// </summary>
        public string failTextID;

        public Synchronoise(
            bool exactMatch = false, bool invert = false,
            string failTextID = null
            ) : base(effectType: MoveEffectType.Synchronoise)
        {
            this.exactMatch = exactMatch;
            this.invert = invert;
            this.failTextID = failTextID;
        }
        public new Synchronoise Clone()
        {
            return new Synchronoise(
                exactMatch: exactMatch, invert: invert, failTextID: failTextID
                );
        }
    }

    /// <summary>
    /// Scales damage if the terrain allows for it.
    /// </summary>
    public class TerrainPulse : MoveEffect
    {
        /// <summary>
        /// The amount by which damage is scaled.
        /// </summary>
        public float damageScale;

        public TerrainPulse(float damageScale = 2f)
            : base(effectType: MoveEffectType.TerrainPulse)
        {
            this.damageScale = damageScale;
        }
        public new TerrainPulse Clone()
        {
            return new TerrainPulse(damageScale: damageScale);
        }
    }

    /// <summary>
    /// To be used with status moves. Forces this move to account for type immunities.
    /// </summary>
    public class ThunderWave : MoveEffect
    {
        public ThunderWave() : base(effectType: MoveEffectType.ThunderWave) { }
        public new ThunderWave Clone()
        {
            return new ThunderWave();
        }
    }

    /// <summary>
    /// Hits multiple times, each consecutive hit increasing in base power.
    /// </summary>
    public class TripleKick : MoveEffect
    {
        /// <summary>
        /// The amount of hits for this attack.
        /// </summary>
        public int hits;

        public TripleKick(int hits = 3)
            : base(effectType: MoveEffectType.TripleKick)
        {
            this.hits = hits;
        }
        public new TripleKick Clone()
        {
            return new TripleKick(hits: hits);
        }

    }

    /// <summary>
    /// Scales damage if the weather allows for it.
    /// </summary>
    public class WeatherBall : MoveEffect
    {
        /// <summary>
        /// The amount by which damage is scaled.
        /// </summary>
        public float damageScale;

        public WeatherBall(float damageScale = 2f)
            : base(effectType: MoveEffectType.WeatherBall)
        {
            this.damageScale = damageScale;
        }
        public new WeatherBall Clone()
        {
            return new WeatherBall(damageScale: damageScale);
        }
    }

    /// <summary>
    /// Renders the target's held item unusable for the remainder of the battle.
    /// </summary>
    public class Whirlwind : MoveEffect
    {
        /// <summary>
        /// The text displayed when the target is forced out of battle.
        /// </summary>
        public string forceOutText;
        /// <summary>
        /// The text displayed when a Pokémon is forced into battle.
        /// </summary>
        public string forceInText;
        /// <summary>
        /// The text displayed when a Pokémon fails to be switched out.
        /// </summary>
        public string failText;

        public Whirlwind(
            string forceOutText = "move-whirlwind", string forceInText = "pokemon-forcein",
            string failText = "move-whirlwind-fail",
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: MoveEffectType.Whirlwind, filters: filters)
        {
            this.forceOutText = forceOutText;
            this.forceInText = forceInText;
        }
        public new Whirlwind Clone()
        {
            return new Whirlwind(
                forceOutText: forceOutText,
                forceInText: forceInText,
                failText: failText,
                filters: filters);
        }
    }

    /// <summary>
    /// Sets the target's ability to the specified abilities.
    /// </summary>
    public class WorrySeed : MoveEffect
    {
        /// <summary>
        /// The abilities to set.
        /// </summary>
        public List<string> abilities;
        /// <summary>
        /// If true, the abilities set are the same as the users.
        /// </summary>
        public bool entrainment;

        /// <summary>
        /// The text displayed for the abilities the target gains.
        /// </summary>
        public string gainText;
        /// <summary>
        /// The text displayed for the abilities that are replaced on the target.
        /// </summary>
        public string loseText;
        /// <summary>
        /// The text displayed for when the target fails to acquire these abilities.
        /// </summary>
        public string failText;

        public WorrySeed(
            IEnumerable<string> abilities = null,
            bool entrainment = false,
            string gainText = "pokemon-ability-gain",
            string loseText = "pokemon-ability-lose",
            string failText = "move-worryseed-fail"
            )
            : base(effectType: MoveEffectType.WorrySeed)
        {
            this.abilities = abilities == null ? new List<string>() : new List<string>(abilities);
            this.entrainment = entrainment;
            this.gainText = gainText;
            this.loseText = loseText;
            this.failText = failText;
        }
        public new WorrySeed Clone()
        {
            return new WorrySeed(
                abilities: abilities,
                entrainment: entrainment,
                gainText: gainText, loseText: loseText, failText: failText
                );
        }
    }
}
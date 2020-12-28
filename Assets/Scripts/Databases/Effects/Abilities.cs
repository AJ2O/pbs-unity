using PBS.Data;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases.Effects.Abilities
{
    public class AbilityEffect
    {
        /// <summary>
        /// The type of effect that this is.
        /// </summary>
        public AbilityEffectType effectType;
        /// <summary>
        /// Additional restrictions on how the effect is applied.
        /// </summary>
        public List<Filter.FilterEffect> filters;

        /// <summary>
        /// The chance of the effect working.
        /// </summary>
        public float chance;

        public AbilityEffect(
            AbilityEffectType effectType,
            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1)
        {
            this.effectType = effectType;
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
        }
        public AbilityEffect Clone()
        {
            return
                this is Adaptability ? (this as Adaptability).Clone()
                : this is Aerilate ? (this as Aerilate).Clone()
                : this is Aftermath ? (this as Aftermath).Clone()
                : this is AirLock ? (this as AirLock).Clone()
                : this is Analytic ? (this as Analytic).Clone()
                : this is AngerPoint ? (this as AngerPoint).Clone()
                : this is Anticipation ? (this as Anticipation).Clone()
                : this is AuraBreak ? (this as AuraBreak).Clone()
                : this is BadDreams ? (this as BadDreams).Clone()
                : this is BallFetch ? (this as BallFetch).Clone()
                : this is Battery ? (this as Battery).Clone()
                : this is BattleBond ? (this as BattleBond).Clone()
                : this is BattleArmor ? (this as BattleArmor).Clone()
                : this is BeastBoost ? (this as BeastBoost).Clone()
                : this is Berserk ? (this as Berserk).Clone()
                : this is Cacophony ? (this as Cacophony).Clone()
                : this is CheekPouch ? (this as CheekPouch).Clone()
                : this is ColorChange ? (this as ColorChange).Clone()
                : this is Comatose ? (this as Comatose).Clone()
                : this is CompoundEyes ? (this as CompoundEyes).Clone()
                : this is Contrary ? (this as Contrary).Clone()
                : this is Corrosion ? (this as Corrosion).Clone()
                : this is Damp ? (this as Damp).Clone()
                : this is Dancer ? (this as Dancer).Clone()
                : this is DarkAura ? (this as DarkAura).Clone()
                : this is Defiant ? (this as Defiant).Clone()
                : this is Disguise ? (this as Disguise).Clone()
                : this is Download ? (this as Download).Clone()
                : this is Drought ? (this as Drought).Clone()
                : this is DrySkin ? (this as DrySkin).Clone()
                : this is EarlyBird ? (this as EarlyBird).Clone()
                : this is FlameBody ? (this as FlameBody).Clone()
                : this is Forecast ? (this as Forecast).Clone()
                : this is Forewarn ? (this as Forewarn).Clone()
                : this is FriendGuard ? (this as FriendGuard).Clone()
                : this is Frisk ? (this as Frisk).Clone()
                : this is Gluttony ? (this as Gluttony).Clone()
                : this is Gooey ? (this as Gooey).Clone()
                : this is GorillaTactics ? (this as GorillaTactics).Clone()
                : this is GulpMissile ? (this as GulpMissile).Clone()
                : this is Guts ? (this as Guts).Clone()
                : this is Harvest ? (this as Harvest).Clone()
                : this is Healer ? (this as Healer).Clone()
                : this is HeavyMetal ? (this as HeavyMetal).Clone()
                : this is HoneyGather ? (this as HoneyGather).Clone()
                : this is HungerSwitch ? (this as HungerSwitch).Clone()
                : this is Hustle ? (this as Hustle).Clone()
                : this is Hydration ? (this as Hydration).Clone()
                : this is HyperCutter ? (this as HyperCutter).Clone()
                : this is IceScales ? (this as IceScales).Clone()
                : this is Illusion ? (this as Illusion).Clone()
                : this is Infiltrator ? (this as Infiltrator).Clone()
                : this is Intimidate ? (this as Intimidate).Clone()
                : this is IntimidateBlock ? (this as IntimidateBlock).Clone()
                : this is IntimidateTrigger ? (this as IntimidateTrigger).Clone()
                : this is IntrepidSword ? (this as IntrepidSword).Clone()
                : this is IronFist ? (this as IronFist).Clone()
                : this is Justified ? (this as Justified).Clone()
                : this is Klutz ? (this as Klutz).Clone()
                : this is Levitate ? (this as Levitate).Clone()
                : this is LightningRod ? (this as LightningRod).Clone()
                : this is Limber ? (this as Limber).Clone()
                : this is LiquidOoze ? (this as LiquidOoze).Clone()
                : this is LongReach ? (this as LongReach).Clone()
                : this is MagicBounce ? (this as MagicBounce).Clone()
                : this is MagicGuard ? (this as MagicGuard).Clone()
                : this is Magician ? (this as Magician).Clone()
                : this is Mimicry ? (this as Mimicry).Clone()
                : this is Minus ? (this as Minus).Clone()
                : this is MirrorArmor ? (this as MirrorArmor).Clone()
                : this is MoldBreaker ? (this as MoldBreaker).Clone()
                : this is Moody ? (this as Moody).Clone()
                : this is Moxie ? (this as Moxie).Clone()
                : this is Multitype ? (this as Multitype).Clone()
                : this is Multiscale ? (this as Multiscale).Clone()
                : this is Mummy ? (this as Mummy).Clone()
                : this is NaturalCure ? (this as NaturalCure).Clone()
                : this is NeutralizingGas ? (this as NeutralizingGas).Clone()
                : this is NoGuard ? (this as NoGuard).Clone()
                : this is Oblivious ? (this as Oblivious).Clone()
                : this is Overcoat ? (this as Overcoat).Clone()
                : this is ParentalBond ? (this as ParentalBond).Clone()
                : this is Pickpocket ? (this as Pickpocket).Clone()
                : this is Pickup ? (this as Pickup).Clone()
                : this is PoisonHeal ? (this as PoisonHeal).Clone()
                : this is PoisonPoint ? (this as PoisonPoint).Clone()
                : this is PoisonTouch ? (this as PoisonTouch).Clone()
                : this is PowerOfAlchemy ? (this as PowerOfAlchemy).Clone()
                : this is Prankster ? (this as Prankster).Clone()
                : this is PropellerTail ? (this as PropellerTail).Clone()
                : this is Protean ? (this as Protean).Clone()
                : this is Pressure ? (this as Pressure).Clone()
                : this is QueenlyMajesty ? (this as QueenlyMajesty).Clone()
                : this is QuickDraw ? (this as QuickDraw).Clone()
                : this is Ripen ? (this as Ripen).Clone()
                : this is Rivalry ? (this as Rivalry).Clone()
                : this is RKSSystem ? (this as RKSSystem).Clone()
                : this is RockHead ? (this as RockHead).Clone()
                : this is RoughSkin ? (this as RoughSkin).Clone()
                : this is Scrappy ? (this as Scrappy).Clone()
                : this is ScreenCleaner ? (this as ScreenCleaner).Clone()
                : this is SereneGrace ? (this as SereneGrace).Clone()
                : this is ShadowTag ? (this as ShadowTag).Clone()
                : this is ShieldDust ? (this as ShieldDust).Clone()
                : this is ShieldsDown ? (this as ShieldsDown).Clone()
                : this is Simple ? (this as Simple).Clone()
                : this is SkillLink ? (this as SkillLink).Clone()
                : this is SlowStart ? (this as SlowStart).Clone()
                : this is Sniper ? (this as Sniper).Clone()
                : this is SolidRock ? (this as SolidRock).Clone()
                : this is SoulHeart ? (this as SoulHeart).Clone()
                : this is SpeedBoost ? (this as SpeedBoost).Clone()
                : this is Stall ? (this as Stall).Clone()
                : this is Stakeout ? (this as Stakeout).Clone()
                : this is StanceChange ? (this as StanceChange).Clone()
                : this is Steadfast ? (this as Steadfast).Clone()
                : this is StickyHold ? (this as StickyHold).Clone()
                : this is Sturdy ? (this as Sturdy).Clone()
                : this is SuctionCups ? (this as SuctionCups).Clone()
                : this is SuperLuck ? (this as SuperLuck).Clone()
                : this is Symbiosis ? (this as Symbiosis).Clone()
                : this is Synchronize ? (this as Synchronize).Clone()
                : this is Technician ? (this as Technician).Clone()
                : this is Telepathy ? (this as Telepathy).Clone()
                : this is TintedLens ? (this as TintedLens).Clone()
                : this is Trace ? (this as Trace).Clone()
                : this is Truant ? (this as Truant).Clone()
                : this is Unaware ? (this as Unaware).Clone()
                : this is Unburden ? (this as Unburden).Clone()
                : this is UnseenFist ? (this as UnseenFist).Clone()
                : this is VoltAbsorb ? (this as VoltAbsorb).Clone()
                : this is WimpOut ? (this as WimpOut).Clone()
                : this is WonderGuard ? (this as WonderGuard).Clone()
                : this is WonderSkin ? (this as WonderSkin).Clone()
                : this is ZenMode ? (this as ZenMode).Clone()
                : new AbilityEffect(
                    effectType: effectType,
                    chance: chance,
                    filters: filters);
        }
    }


    /// <summary>
    /// Modifies the user's STAB multiplier.
    /// </summary>
    public class Adaptability : AbilityEffect
    {
        /// <summary>
        /// The new STAB multiplier.
        /// </summary>
        public float STABMultiplier;

        public Adaptability(float STABMultiplier = 2f) : base(effectType: AbilityEffectType.Adaptability)
        {
            this.STABMultiplier = STABMultiplier;
        }
        public new Adaptability Clone()
        {
            return new Adaptability(STABMultiplier: STABMultiplier);
        }
    }

    /// <summary>
    /// Changes the types of certain moves, and modifies their base power.
    /// </summary>
    public class Aerilate : AbilityEffect
    {
        /// <summary>
        /// The power multiplier to apply to affected moves.
        /// </summary>
        public float powerMultiplier;
        /// <summary>
        /// The move type that will be changed.
        /// </summary>
        public string baseMoveType;
        /// <summary>
        /// The type that the move is transformed into.
        /// </summary>
        public string toMoveType;

        public Aerilate(
            float powerMultiplier = 1.2f,
            string baseMoveType = "normal", string toMoveType = "flying",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Aerilate, filters: filters)
        {
            this.powerMultiplier = powerMultiplier;
            this.baseMoveType = baseMoveType;
            this.toMoveType = toMoveType;
        }
        public new Aerilate Clone()
        {
            return new Aerilate(
                powerMultiplier: powerMultiplier,
                baseMoveType: baseMoveType, toMoveType: toMoveType,
                filters: filters);
        }
    }

    /// <summary>
    /// When this Pokémon faints by a direct attack, the attacker loses a portion of their HP.
    /// </summary>
    public class Aftermath : AbilityEffect
    {
        /// <summary>
        /// Defines how damage is dealt.
        /// </summary>
        public General.Damage damage;

        /// <summary>
        /// This effect is only triggered if the Pokémon faints due to a contact move.
        /// </summary>
        public bool onlyContact;
        /// <summary>
        /// If true, this ability is blocked if a Pokémon on the field has the Damp ability.
        /// </summary>
        public bool blockedByDamp;

        public Aftermath(
            General.Damage damage,
            bool onlyContact = true, bool blockedByDamp = true
            )
            : base(effectType: AbilityEffectType.Aftermath)
        {
            this.damage = damage.Clone();
            this.onlyContact = onlyContact;
            this.blockedByDamp = blockedByDamp;
        }
        public new Aftermath Clone()
        {
            return new Aftermath(
                damage: damage,
                onlyContact: onlyContact, blockedByDamp: blockedByDamp
                );
        }
    }

    /// <summary>
    /// Eliminates the effects of weather.
    /// </summary>
    public class AirLock : AbilityEffect
    {
        /// <summary>
        /// The text that displays when weather is negated.
        /// </summary>
        public string displayText;

        public AirLock(string displayText = "ability-airlock") : base(effectType: AbilityEffectType.AirLock)
        {
            this.displayText = displayText;
        }
        public new AirLock Clone()
        {
            return new AirLock(displayText: displayText);
        }
    }

    /// <summary>
    /// Increases move power against targets that have already acted during the turn.
    /// </summary>
    public class Analytic : AbilityEffect
    {
        /// <summary>
        /// The power multiplier applied to moves.
        /// </summary>
        public float powerMultiplier;

        public Analytic(float powerMultiplier = 1.3f) : base(effectType: AbilityEffectType.Analytic)
        {
            this.powerMultiplier = powerMultiplier;
        }
        public new Analytic Clone()
        {
            return new Analytic(powerMultiplier: powerMultiplier);
        }
    }

    /// <summary>
    /// Modifies stats if the user is struck by a critical hit.
    /// </summary>
    public class AngerPoint : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied when critically hit.
        /// </summary>
        public General.StatStageMod statStageMod;

        public AngerPoint(General.StatStageMod statStageMod)
            : base(effectType: AbilityEffectType.AngerPoint)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new AngerPoint Clone()
        {
            return new AngerPoint(statStageMod: statStageMod);
        }
    }

    /// <summary>
    /// Causes this Pokémon to shudder if the opposing Pokémon have super-effective or OHKO moves.
    /// </summary>
    public class Anticipation : AbilityEffect
    {
        /// <summary>
        /// Text displayed when Anticipation triggers.
        /// </summary>
        public string displayText;
        /// <summary>
        /// If true, this ability will trigger if the opposing Pokémon has OHKO moves.
        /// </summary>
        public bool notifyOHKO;

        public Anticipation(
            string displayText = "ability-anticipation",
            bool notifyOHKO = true
            )
            : base(effectType: AbilityEffectType.Anticipation)
        {
            this.displayText = displayText;
            this.notifyOHKO = notifyOHKO;
        }
        public new Anticipation Clone()
        {
            return new Anticipation(displayText: displayText, notifyOHKO: notifyOHKO);
        }
    }

    /// <summary>
    /// Reverses the effect of <seealso cref="DarkAura"/> abilities that are active.
    /// </summary>
    public class AuraBreak : AbilityEffect
    {
        /// <summary>
        /// The text displayed when this Pokémon enters battle.
        /// </summary>
        public string displayText;

        public AuraBreak(
            string displayText = "ability-aurabreak"
            ) : base(effectType: AbilityEffectType.AuraBreak)
        {
            this.displayText = displayText;
        }
        public new AuraBreak Clone()
        {
            return new AuraBreak(displayText: displayText);
        }
    }

    /// <summary>
    /// Reduces the HP of all sleeping opposing Pokémon every turn.
    /// </summary>
    public class BadDreams : AbilityEffect
    {
        /// <summary>
        /// The affected non-volatile statuses that opposing Pokémon must have in order to take damage.
        /// </summary>
        public List<string> affectedStatuses;
        /// <summary>
        /// The percentage of HP that the opposing Pokémon loses each turn.
        /// </summary>
        public float hpLossPercent;
        /// <summary>
        /// The text displayed when opposing Pokémon lose their HP.
        /// </summary>
        public string displayText;

        public BadDreams(
            float hpLossPercent = 1f / 8,
            string displayText = "ability-baddreams",
            IEnumerable<string> affectedStatuses = null
            )
            : base(effectType: AbilityEffectType.BadDreams)
        {
            this.hpLossPercent = hpLossPercent;
            this.displayText = displayText;
            this.affectedStatuses = affectedStatuses != null ? new List<string>(affectedStatuses)
                : new List<string>
                {
                "sleep"
                };
        }
        public new BadDreams Clone()
        {
            return new BadDreams(
                hpLossPercent: hpLossPercent, displayText: displayText,
                affectedStatuses: affectedStatuses
                );
        }
    }

    /// <summary>
    /// If holding no item, the user retrieves its trainer's last failed thrown Poké Ball.
    /// </summary>
    public class BallFetch : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the user retrieves a Poké Ball.
        /// </summary>
        public string displayText;

        public BallFetch(
            string displayText = "ability-ballfetch"
            )
            : base(effectType: AbilityEffectType.BallFetch)
        {
            this.displayText = displayText;
        }
        public new BallFetch Clone()
        {
            return new BallFetch(displayText: displayText);
        }
    }

    /// <summary>
    /// Increases the power of ally Pokémon's attacks if they satisfy filters.
    /// </summary>
    public class Battery : AbilityEffect
    {
        /// <summary>
        /// If true, this boost also affects the user.
        /// </summary>
        public bool affectsSelf;

        /// <summary>
        /// The power multiplier applied to moves.
        /// </summary>
        public float powerMultiplier;

        public Battery(
            bool affectsSelf = false,
            float powerMultiplier = 1.3f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Battery, filters: filters)
        {
            this.affectsSelf = affectsSelf;
            this.powerMultiplier = powerMultiplier;
        }
        public new Battery Clone()
        {
            return new Battery(
                affectsSelf: affectsSelf,
                powerMultiplier: powerMultiplier,
                filters: filters);
        }
    }

    /// <summary>
    /// Prevents this Pokémon from being struck by critical hits.
    /// </summary>
    public class BattleArmor : AbilityEffect
    {
        public BattleArmor() : base(effectType: AbilityEffectType.BattleArmor) { }
        public new BattleArmor Clone()
        {
            return new BattleArmor();
        }
    }

    /// <summary>
    /// Changes the Pokémon's form after defeating an opposing Pokémon.
    /// </summary>
    public class BattleBond : AbilityEffect
    {
        public class BattleBondTransformation
        {
            public string preForm;
            public string toForm;

            public BattleBondTransformation(string preForm, string toForm)
            {
                this.preForm = preForm;
                this.toForm = toForm;
            }
            public BattleBondTransformation Clone()
            {
                return new BattleBondTransformation(preForm: preForm, toForm: toForm);
            }
        }

        /// <summary>
        /// Accompanying text before the form change.
        /// </summary>
        public string beforeText;
        /// <summary>
        /// Accompanying text after the form change.
        /// </summary>
        public string afterText;

        /// <summary>
        /// The list of transformations.
        /// </summary>
        public List<BattleBondTransformation> transformations;

        public BattleBond(
            string beforeText = "ability-battlebond", string afterText = "ability-battlebond-form",
            IEnumerable<BattleBondTransformation> transformations = null
            )
            : base(effectType: AbilityEffectType.BattleBond)
        {
            this.beforeText = beforeText;
            this.afterText = afterText;
            this.transformations = new List<BattleBondTransformation>();
            if (transformations != null)
            {
                List<BattleBondTransformation> preList = new List<BattleBondTransformation>(transformations);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.transformations.Add(preList[i].Clone());
                }
            }
        }
        public new BattleBond Clone()
        {
            return new BattleBond(
                beforeText: beforeText, afterText: afterText,
                transformations: transformations
                );
        }
    }

    /// <summary>
    /// Modifies the user's highest stat after knocking out an opposing Pokémon.
    /// </summary>
    public class BeastBoost : AbilityEffect
    {
        /// <summary>
        /// The value to modify the highest stat by.
        /// </summary>
        public int statMod;
        public BeastBoost(int statMod = 1) : base(effectType: AbilityEffectType.BeastBoost)
        {
            this.statMod = statMod;
        }
        public new BeastBoost Clone()
        {
            return new BeastBoost(statMod: statMod);
        }
    }

    /// <summary>
    /// Modifies stats if the user's HP falls below a certain threshold by a direct hit.
    /// </summary>
    public class Berserk : AbilityEffect
    {
        /// <summary>
        /// The HP threshold that the user's HP must fall below.
        /// </summary>
        public float hpThreshold;

        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        public Berserk(
            General.StatStageMod statStageMod,
            float hpThreshold = 0.5f)
            : base(effectType: AbilityEffectType.Berserk)
        {
            this.hpThreshold = hpThreshold;
            this.statStageMod = statStageMod.Clone();
        }
        public new Berserk Clone()
        {
            return new Berserk(
                hpThreshold: hpThreshold,
                statStageMod: statStageMod);
        }
    }

    /// <summary>
    /// Negates the effect of moves on the user that are of the specific tags.
    /// </summary>
    public class Cacophony : AbilityEffect
    {
        /// <summary>
        /// If a move has a tag contained here, it is blocked.
        /// </summary>
        public HashSet<MoveTag> blockedMoveTags;

        /// <summary>
        /// The text displayed if the move is blocked.
        /// </summary>
        public string displayText;

        public Cacophony(
            IEnumerable<MoveTag> blockedMoves = null,
            string displayText = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Cacophony, filters: filters)
        {
            blockedMoveTags = blockedMoves == null ? new HashSet<MoveTag>()
                : new HashSet<MoveTag>(blockedMoves);
            this.displayText = displayText;
        }
        public new Cacophony Clone()
        {
            return new Cacophony(
                blockedMoves: blockedMoveTags,
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Restores HP upon consumption of items.
    /// </summary>
    public class CheekPouch : AbilityEffect
    {
        /// <summary>
        /// The HP gained as a % of the user's max HP.
        /// </summary>
        public float hpGainPercent;

        /// <summary>
        /// If true, this ability activates on consumed berries.
        /// </summary>
        public bool onBerry;

        public CheekPouch(
            float hpGainPercent = 1f / 3,
            bool onBerry = true
            )
            : base(effectType: AbilityEffectType.CheekPouch)
        {
            this.hpGainPercent = hpGainPercent;
            this.onBerry = onBerry;
        }
        public new CheekPouch Clone()
        {
            return new CheekPouch(
                hpGainPercent: hpGainPercent,
                onBerry: onBerry
                );
        }
    }

    /// <summary>
    /// Changes the user's type to the type of the move it is was just hit by.
    /// </summary>
    public class ColorChange : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the user's type is changed;
        /// </summary>
        public string displayText;
        /// <summary>
        /// Type only changes when hit by damaging attacks.
        /// </summary>
        public bool onlyDamaging;

        public ColorChange(
            string displayText = "ability-colorchange",
            bool onlyDamaging = true
            )
            : base(effectType: AbilityEffectType.ColorChange)
        {
            this.displayText = displayText;
            this.onlyDamaging = onlyDamaging;
        }
        public new ColorChange Clone()
        {
            return new ColorChange(
                displayText: displayText,
                onlyDamaging: onlyDamaging);
        }
    }

    /// <summary>
    /// Simulates a status condition, without actually having the condition.
    /// </summary>
    public class Comatose : AbilityEffect
    {
        /// <summary>
        /// The status condition being simulated.
        /// </summary>
        public string statusID;

        public Comatose(
            string statusID = "sleep"
            )
            : base(effectType: AbilityEffectType.Comatose)
        {
            this.statusID = statusID;
        }
        public new Comatose Clone()
        {
            return new Comatose(statusID: statusID);
        }
    }

    /// <summary>
    /// Scales the user's stats.
    /// </summary>
    public class CompoundEyes : AbilityEffect
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

        public CompoundEyes(
            General.StatScale statScale,
            bool victoryStar = false,
            float defeatistThreshold = -1,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.CompoundEyes, filters: filters)
        {
            this.statScale = statScale.Clone();
            this.victoryStar = victoryStar;
            this.defeatistThreshold = defeatistThreshold;
        }
        public new CompoundEyes Clone()
        {
            return new CompoundEyes(
                statScale: statScale,
                victoryStar: victoryStar,
                defeatistThreshold: defeatistThreshold,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Reverses the user's stat stage changes.
    /// </summary>
    public class Contrary : AbilityEffect
    {
        public Contrary() : base(effectType: AbilityEffectType.Contrary) { }
        public new Contrary Clone()
        {
            return new Contrary();
        }
    }

    /// <summary>
    /// Can inflict status conditions regardless of type-based immunities.
    /// </summary>
    public class Corrosion : AbilityEffect
    {
        /// <summary>
        /// Inflicted statuses that ignore type-based immunities.
        /// </summary>
        public List<string> statuses;

        public Corrosion(
            IEnumerable<string> statuses = null
            )
            : base(effectType: AbilityEffectType.Corrosion)
        {
            this.statuses = statuses == null ? new List<string>() : new List<string>(statuses);
        }
        public new Corrosion Clone()
        {
            return new Corrosion(statuses: statuses);
        }
    }

    /// <summary>
    /// Prevents the use of certain moves while the user is in battle.
    /// </summary>
    public class Damp : AbilityEffect
    {
        /// <summary>
        /// Moves with tags contained here are blocked.
        /// </summary>
        public HashSet<MoveTag> moveTags;

        public Damp(IEnumerable<MoveTag> moveTags = null) : base(effectType: AbilityEffectType.Damp)
        {
            this.moveTags = moveTags == null ? new HashSet<MoveTag>() : new HashSet<MoveTag>(moveTags);
        }
        public new Damp Clone()
        {
            return new Damp(moveTags: moveTags);
        }
    }

    /// <summary>
    /// Immediately copies the the use of a move if it is tagged with certain <seealso cref="MoveTag"/>s.
    /// </summary>
    public class Dancer : AbilityEffect
    {
        /// <summary>
        /// Moves with tags contained here are copied.
        /// </summary>
        public HashSet<MoveTag> moveTags;

        public Dancer(IEnumerable<MoveTag> moveTags = null) : base(effectType: AbilityEffectType.Dancer)
        {
            this.moveTags = moveTags == null ? new HashSet<MoveTag>() : new HashSet<MoveTag>(moveTags);
        }
        public new Dancer Clone()
        {
            return new Dancer(moveTags: moveTags);
        }
    }

    /// <summary>
    /// Modifies the damage dealt by specific-typed moves by all Pokémon on the field while this ability
    /// is active. Does not stack with itself. Effect is reversed if a Pokémon with <seealso cref="AuraBreak"/>
    /// is on the field.
    /// </summary>
    public class DarkAura : AbilityEffect
    {
        /// <summary>
        /// The multiplier to add to affected moves.
        /// </summary>
        public float damageMultiplier;
        /// <summary>
        /// The text displayed when this Pokémon enters battle.
        /// </summary>
        public string displayText;

        public DarkAura(
            float damageMultiplier = 4f / 3,
            string displayText = "ability-darkaura",

            IEnumerable<string> moveTypes = null,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.DarkAura, filters: filters)
        {
            this.damageMultiplier = damageMultiplier;
            this.displayText = displayText;

            Filter.TypeList typeList = new Filter.TypeList(
                targetType: Filter.TypeList.TargetType.Move,
                types: moveTypes);
            this.filters.Add(typeList);
        }
        public new DarkAura Clone()
        {
            return new DarkAura(
                damageMultiplier: damageMultiplier, displayText: displayText,
                filters: filters);
        }
    }

    /// <summary>
    /// Triggers stat stage changes when the user's stats are lowered.
    /// </summary>
    public class Defiant : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        /// <summary>
        /// Defiant activated if the specified stats were lowered. 
        /// </summary>
        public HashSet<PokemonStats> lowerTriggers;
        /// <summary>
        /// Defiant activated if the specified stats were raised. 
        /// </summary>
        public HashSet<PokemonStats> raiseTriggers;

        /// <summary>
        /// If true, this can only be activated if the stats were changed by an opponent.
        /// </summary>
        public bool onlyOpposing;

        public Defiant(
            General.StatStageMod statStageMod = null,
            IEnumerable<PokemonStats> lowerTriggers = null, IEnumerable<PokemonStats> raiseTriggers = null,
            bool onlyOpposing = true
            )
            : base(effectType: AbilityEffectType.Defiant)
        {
            this.statStageMod = statStageMod == null ? null : statStageMod.Clone();
            this.lowerTriggers = lowerTriggers == null ? new HashSet<PokemonStats>()
                : new HashSet<PokemonStats>(lowerTriggers);
            this.raiseTriggers = raiseTriggers == null ? new HashSet<PokemonStats>()
                : new HashSet<PokemonStats>(raiseTriggers);
            this.onlyOpposing = onlyOpposing;
        }
        public new Defiant Clone()
        {
            return new Defiant(
                statStageMod: statStageMod,
                lowerTriggers: lowerTriggers, raiseTriggers: raiseTriggers,
                onlyOpposing: onlyOpposing
                );
        }
    }

    /// <summary>
    /// The user is immune to direct hits for a turn, and the disguise breaks afterward for the rest of battle,
    /// changing the Pokémon's form.
    /// </summary>
    public class Disguise : AbilityEffect
    {

        /// <summary>
        /// The % of HP the user loses when its disguise is broken.
        /// </summary>
        public float hpLossPercent;
        /// <summary>
        /// The disguise forms that this ability applies to.
        /// </summary>
        public List<General.FormTransformation> disguiseForms;

        /// <summary>
        /// The text displayed when the Pokémon changes form.
        /// </summary>
        public string displayText;

        public Disguise(
            IEnumerable<General.FormTransformation> disguiseForms = null,
            float hpLossPercent = 1f / 8, string displayText = "ability-disguise",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Disguise, filters: filters)
        {
            this.disguiseForms = new List<General.FormTransformation>();
            if (disguiseForms != null)
            {
                List<General.FormTransformation> preList = new List<General.FormTransformation>(disguiseForms);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.disguiseForms.Add(preList[i].Clone());
                }
            }
            this.hpLossPercent = hpLossPercent;
            this.displayText = displayText;
        }
        public new Disguise Clone()
        {
            return new Disguise(
                disguiseForms: disguiseForms,
                hpLossPercent: hpLossPercent, displayText: displayText,
                filters: filters
                );
        }
        public bool IsPokemonDisguised(Main.Pokemon.Pokemon pokemon)
        {
            return GetDisguiseForm(pokemon) != null;
        }
        public General.FormTransformation GetDisguiseForm(Main.Pokemon.Pokemon pokemon)
        {
            for (int i = 0; i < disguiseForms.Count; i++)
            {
                if (disguiseForms[i].IsPokemonAPreForm(pokemon)
                    && !disguiseForms[i].IsPokemonAToForm(pokemon))
                {
                    return disguiseForms[i];
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Modifies stats the user's stats when it enters battle depending on the opponent's stats.
    /// </summary>
    public class Download : AbilityEffect
    {
        public class DownloadCompare
        {
            public General.StatStageMod statStageMod1, statStageMod2;
            public PokemonStats stats1, stats2;

            public DownloadCompare(
                General.StatStageMod statStageMod1,
                General.StatStageMod statStageMod2,
                PokemonStats stats1 = PokemonStats.Defense, PokemonStats stats2 = PokemonStats.SpecialDefense
                )
            {
                this.statStageMod1 = statStageMod1.Clone();
                this.statStageMod2 = statStageMod2.Clone();
                this.stats1 = stats1;
                this.stats2 = stats2;
            }
            public DownloadCompare Clone()
            {
                return new DownloadCompare(
                    statStageMod1: statStageMod1, statStageMod2: statStageMod2,
                    stats1: stats1, stats2: stats2
                    );
            }
        }

        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public List<DownloadCompare> downloadComparisons;

        public Download(
            IEnumerable<DownloadCompare> downloadComparisons = null,
            IEnumerable<Filter.FilterEffect> filters = null)
            : base(effectType: AbilityEffectType.Download, filters: filters)
        {
            this.downloadComparisons = new List<DownloadCompare>();
            if (downloadComparisons != null)
            {
                List<DownloadCompare> preList = new List<DownloadCompare>(downloadComparisons);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.downloadComparisons.Add(preList[i].Clone());
                }
            }
        }
        public new Download Clone()
        {
            return new Download(
                downloadComparisons: downloadComparisons,
                filters: filters);
        }
    }

    /// <summary>
    /// Starts a battle condition when the user enters battle.
    /// </summary>
    public class Drought : AbilityEffect
    {
        /// <summary>
        /// The condition inflicted.
        /// </summary>
        public General.InflictStatus inflictStatus;
        /// <summary>
        /// This condition disappears once there are no other Drought users in-battle that cast the same
        /// condition.
        /// </summary>
        public bool desolateLand;

        public Drought(
            General.InflictStatus inflictStatus,
            bool desolateLand = false
            )
            : base(effectType: AbilityEffectType.Drought)
        {
            this.inflictStatus = inflictStatus.Clone();
            this.desolateLand = desolateLand;
        }
        public new Drought Clone()
        {
            return new Drought(
                inflictStatus: inflictStatus,
                desolateLand: desolateLand
                );
        }
    }

    /// <summary>
    /// The user loses or recovers HP every turn if the specified battle conditions are active.
    /// </summary>
    public class DrySkin : AbilityEffect
    {
        public class DrySkinCondition
        {
            public List<string> conditions;
            public float hpGainPercent;
            public float hpLosePercent;

            public DrySkinCondition(
                IEnumerable<string> conditions,
                float hpGainPercent = 0f, float hpLosePercent = 0f
                )
            {
                this.conditions = new List<string>(conditions);
                this.hpGainPercent = hpGainPercent;
                this.hpLosePercent = hpLosePercent;
            }
            public DrySkinCondition Clone()
            {
                return new DrySkinCondition(
                    conditions: conditions,
                    hpGainPercent: hpGainPercent, hpLosePercent: hpLosePercent
                    );
            }
        }

        /// <summary>
        /// The specified dry skin conditions.
        /// </summary>
        public List<DrySkinCondition> conditions;

        public DrySkin(
            IEnumerable<DrySkinCondition> conditions = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.DrySkin, filters: filters)
        {
            this.conditions = new List<DrySkinCondition>();
            if (conditions != null)
            {
                List<DrySkinCondition> preList = new List<DrySkinCondition>(conditions);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.conditions.Add(preList[i].Clone());
                }
            }
        }
        public new DrySkin Clone()
        {
            return new DrySkin(
                conditions: conditions,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Modifies the turns applied for status conditions
    /// </summary>
    public class EarlyBird : AbilityEffect
    {
        /// <summary>
        /// The status conditions cut.
        /// </summary>
        public List<string> conditions;
        /// <summary>
        /// The amount to modify the turns the status is inflicted for.
        /// </summary>
        public float turnModifier;

        public EarlyBird(
            IEnumerable<string> conditions = null,
            float turnModifier = 0.5f
            )
            : base(effectType: AbilityEffectType.EarlyBird)
        {
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
            this.turnModifier = turnModifier;
        }
        public new EarlyBird Clone()
        {
            return new EarlyBird(
                conditions: conditions,
                turnModifier: turnModifier
                );
        }
    }

    /// <summary>
    /// Inflicts the specified status upon the user being attacked.
    /// </summary>
    public class FlameBody : AbilityEffect
    {
        public class EffectSporeCondition
        {
            public General.InflictStatus inflictStatus;
            public float chance;

            public EffectSporeCondition(
                General.InflictStatus inflictStatus,
                float chance = 1f / 3
                )
            {
                this.inflictStatus = inflictStatus.Clone();
                this.chance = chance;
            }
            public EffectSporeCondition Clone()
            {
                return new EffectSporeCondition(
                    inflictStatus: inflictStatus,
                    chance: chance
                    );
            }
        }
        public List<EffectSporeCondition> effectSpores;

        public General.InflictStatus inflictStatus;

        /// <summary>
        /// If true, the user also applies the status to itself. 
        /// </summary>
        public bool perishBody;
        /// <summary>
        /// If true, this effect only occurs on damaging moves.
        /// </summary>
        public bool onlyDamaging;
        /// <summary>
        /// If set to non-empty, this effect only occurs if the move has one of the specified tags.
        /// </summary>
        public HashSet<MoveTag> triggerTags;

        public FlameBody(
            General.InflictStatus inflictStatus = null,
            IEnumerable<EffectSporeCondition> effectSpores = null,
            bool perishBody = false,
            bool onlyDamaging = false,
            IEnumerable<MoveTag> triggerTags = null,

            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1
            )
            : base(effectType: AbilityEffectType.FlameBody,
                    filters: filters, chance: chance
                    )
        {
            this.inflictStatus = inflictStatus == null ? null : inflictStatus.Clone();
            this.effectSpores = new List<EffectSporeCondition>();
            if (effectSpores != null)
            {
                List<EffectSporeCondition> preList = new List<EffectSporeCondition>(effectSpores);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.effectSpores.Add(preList[i].Clone());
                }
            }

            this.perishBody = perishBody;
            this.onlyDamaging = onlyDamaging;
            this.triggerTags = triggerTags == null ? new HashSet<MoveTag>()
                : new HashSet<MoveTag>(triggerTags);
        }
        public new FlameBody Clone()
        {
            return new FlameBody(
                inflictStatus: inflictStatus,
                effectSpores: effectSpores,
                perishBody: perishBody,
                onlyDamaging: onlyDamaging, triggerTags: triggerTags,

                filters: filters, chance: chance
                );
        }
        public General.InflictStatus GetAnEffectSporeStatus()
        {
            General.InflictStatus status = effectSpores.Count == 0 ? null : effectSpores[0].inflictStatus;

            List<float> levelChances = new List<float>();
            float totalChance = 0;
            for (int i = 0; i < effectSpores.Count; i++)
            {
                totalChance += effectSpores[i].chance;
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
                        return effectSpores[i].inflictStatus;
                    }
                    else if (i == levelChances.Count - 1)
                    {
                        return effectSpores[i].inflictStatus;
                    }
                }
            }

            return status;
        }
    }

    /// <summary>
    /// Scales the user's move's power if it has the specified status conditions.
    /// </summary>
    public class FlareBoost : AbilityEffect
    {
        /// <summary>
        /// The boost to damage dealt.
        /// </summary>
        public float powerMultiplier;

        /// <summary>
        /// The status conditions that trigger this effect.
        /// </summary>
        public Filter.Harvest conditionCheck;

        public FlareBoost(
            Filter.Harvest conditionCheck,
            float powerMultiplier = 1.5f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.FlareBoost, filters: filters)
        {
            this.conditionCheck = conditionCheck.Clone();
            this.powerMultiplier = powerMultiplier;
        }
        public new FlareBoost Clone()
        {
            return new FlareBoost(
                powerMultiplier: powerMultiplier,
                conditionCheck: conditionCheck,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Changes the Pokémon's form depending on the weather.
    /// </summary>
    public class Forecast : AbilityEffect
    {
        public class ForecastTransformation
        {
            public List<string> conditions;
            public General.FormTransformation transformation;

            public ForecastTransformation(
                General.FormTransformation transformation,
                IEnumerable<string> conditions = null)
            {
                this.transformation = transformation.Clone();
                this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
            }
            public ForecastTransformation Clone()
            {
                return new ForecastTransformation(
                    transformation: transformation,
                    conditions: conditions
                    );
            }
        }

        /// <summary>
        /// Accompanying text after the form change.
        /// </summary>
        public string displayText;

        /// <summary>
        /// The list of transformations.
        /// </summary>
        public List<ForecastTransformation> transformations;
        /// <summary>
        /// If non-null, the Pokémon reverts to this form if no <seealso cref="ForecastTransformation"/>
        /// was satisfied.
        /// </summary>
        public General.FormTransformation defaultTransformation;

        public Forecast(
            string displayText = null,
            IEnumerable<ForecastTransformation> transformations = null,
            General.FormTransformation defaultTransformation = null
            )
            : base(effectType: AbilityEffectType.Forecast)
        {
            this.displayText = displayText;
            this.transformations = new List<ForecastTransformation>();
            if (transformations != null)
            {
                List<ForecastTransformation> preList = new List<ForecastTransformation>(transformations);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.transformations.Add(preList[i].Clone());
                }
            }
            this.defaultTransformation = defaultTransformation == null ? null : defaultTransformation.Clone();
        }
        public new Forecast Clone()
        {
            return new Forecast(
                displayText: displayText,
                transformations: transformations, defaultTransformation: defaultTransformation
                );
        }
    }

    /// <summary>
    /// Indicates the opposing Pokémon's highest-powered moves.
    /// </summary>
    public class Forewarn : AbilityEffect
    {
        /// <summary>
        /// Text displayed when Forewarn triggers.
        /// </summary>
        public string displayText;

        public Forewarn(
            string displayText = "ability-forewarn"
            )
            : base(effectType: AbilityEffectType.Forewarn)
        {
            this.displayText = displayText;
        }
        public new Forewarn Clone()
        {
            return new Forewarn(displayText: displayText);
        }
    }

    /// <summary>
    /// Scales damage dealt to allies.
    /// </summary>
    public class FriendGuard : AbilityEffect
    {
        /// <summary>
        /// Damage scaled for allies.
        /// </summary>
        public float damageModifier;

        public FriendGuard(
            float damageModifier = 0.75f,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.FriendGuard, filters: filters)
        {
            this.damageModifier = damageModifier;
        }
        public new FriendGuard Clone()
        {
            return new FriendGuard(
                damageModifier: damageModifier,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Indicates the opposing Pokémon's highest-powered moves.
    /// </summary>
    public class Frisk : AbilityEffect
    {
        /// <summary>
        /// Text displayed when Forewarn triggers.
        /// </summary>
        public string displayText;

        public Frisk(
            string displayText = "ability-frisk"
            )
            : base(effectType: AbilityEffectType.Frisk)
        {
            this.displayText = displayText;
        }
        public new Frisk Clone()
        {
            return new Frisk(displayText: displayText);
        }
    }

    /// <summary>
    /// Modifies this move's priority depending on the move's type. 
    /// </summary>
    public class GaleWings : AbilityEffect
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

        public GaleWings(
            PriorityMode mode = PriorityMode.Add, int priority = 1,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.GaleWings, filters: filters)
        {
            this.mode = mode;
            this.priority = priority;
        }
        public new GaleWings Clone()
        {
            return new GaleWings(
                mode: mode,
                priority: priority,
                filters: filters);
        }
    }

    /// <summary>
    /// Scales held berries' HP-threshold value.
    /// </summary>
    public class Gluttony : AbilityEffect
    {
        /// <summary>
        /// The scale applied to HP-thresholds.
        /// </summary>
        public float thresholdScale;
        /// <summary>
        /// The minimum item HP-threshold this effect applies to.
        /// </summary>
        public float minItemHPThreshold;
        /// <summary>
        /// The maximum item HP-threshold this effect applies to.
        /// </summary>
        public float maxItemHPThreshold;

        public Gluttony(
            float thresholdScale = 2f,
            float minItemHPThreshold = 0.25f,
            float maxItemHPThreshold = 0.25f
            )
            : base(effectType: AbilityEffectType.Gluttony)
        {
            this.thresholdScale = thresholdScale;
            this.minItemHPThreshold = minItemHPThreshold;
            this.maxItemHPThreshold = maxItemHPThreshold;
        }
        public new Gluttony Clone()
        {
            return new Gluttony(
                thresholdScale: thresholdScale,
                minItemHPThreshold: minItemHPThreshold, maxItemHPThreshold: maxItemHPThreshold
                );
        }
    }

    /// <summary>
    /// Modifies the attacker's stat stages when the user is struck by an attack.
    /// </summary>
    public class Gooey : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;
        /// <summary>
        /// If true, the stat stages are applied to all other Pokémon on the field.
        /// </summary>
        public bool cottonDown;

        /// <summary>
        /// If true, this effect only occurs on damaging moves.
        /// </summary>
        public bool onlyDamaging;
        /// <summary>
        /// If set to non-empty, this effect only occurs if the move has one of the specified tags.
        /// </summary>
        public HashSet<MoveTag> triggerTags;

        public Gooey(
            General.StatStageMod statStageMod,
            bool cottonDown = false,
            bool onlyDamaging = false,
            IEnumerable<MoveTag> triggerTags = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Gooey, filters: filters)
        {
            this.statStageMod = statStageMod.Clone();
            this.cottonDown = cottonDown;
            this.onlyDamaging = onlyDamaging;
            this.triggerTags = triggerTags == null ? new HashSet<MoveTag>() : new HashSet<MoveTag>(triggerTags);
        }
        public new Gooey Clone()
        {
            return new Gooey(
                statStageMod: statStageMod,
                cottonDown: cottonDown, onlyDamaging: onlyDamaging,
                triggerTags: triggerTags,

                filters: filters
                );
        }
    }

    /// <summary>
    /// Locks the user into its first-selected move as long the user remains in battle.
    /// </summary>
    public class GorillaTactics : AbilityEffect
    {
        public GorillaTactics(
            )
            : base(effectType: AbilityEffectType.GorillaTactics)
        {

        }
        public new GorillaTactics Clone()
        {
            return new GorillaTactics();
        }
    }

    /// <summary>
    /// Changes the Pokémon's form depending on the weather.
    /// </summary>
    public class GulpMissile : AbilityEffect
    {
        public class Missile
        {
            public float hpThreshold;
            public float hpLossPercent;
            public string displayText;
            public General.InflictStatus inflictStatus;
            public General.StatStageMod statStageMod;

            public Missile(
                float hpThreshold = 0.5f,
                float hpLossPercent = 0.25f,
                string displayText = "ability-gulpmissile",
                General.InflictStatus inflictStatus = null, General.StatStageMod statStageMod = null
                )
            {
                this.hpThreshold = hpThreshold;
                this.hpLossPercent = hpLossPercent;
                this.displayText = displayText;
                this.inflictStatus = inflictStatus == null ? null : inflictStatus.Clone();
                this.statStageMod = statStageMod == null ? null : statStageMod.Clone();
            }
            public Missile Clone()
            {
                return new Missile(
                    hpThreshold: hpThreshold,
                    hpLossPercent: hpLossPercent,
                    displayText: displayText,
                    inflictStatus: inflictStatus, statStageMod: statStageMod
                    );
            }
        }
        public class GulpTransformation
        {
            public float hpThreshold;
            public List<string> moves;
            public General.FormTransformation transformation;
            public List<Missile> missiles;

            public GulpTransformation(
                General.FormTransformation transformation,
                IEnumerable<Missile> missiles,
                IEnumerable<string> moves = null)
            {
                this.transformation = transformation.Clone();
                this.missiles = new List<Missile>();
                if (missiles != null)
                {
                    List<Missile> preList = new List<Missile>(missiles);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.missiles.Add(preList[i].Clone());
                    }
                }
                this.moves = moves == null ? new List<string>() : new List<string>(moves);
            }
            public GulpTransformation Clone()
            {
                return new GulpTransformation(
                    transformation: transformation,
                    missiles: missiles,
                    moves: moves
                    );
            }
        }

        /// <summary>
        /// Accompanying text after the "gulping" form change.
        /// </summary>
        public string gulpText;
        /// <summary>
        /// Accompanying text after "spit up" form change.
        /// </summary>
        public string spitUpText;

        /// <summary>
        /// The list of transformations done when using "gulping" moves.
        /// </summary>
        public List<GulpTransformation> gulpTransformations;
        /// <summary>
        /// The list of transformations done when spitting up Gulp Missiles.
        /// </summary>
        public List<General.FormTransformation> spitUpTransformations;

        public GulpMissile(
            string gulpText = "pokemon-changeform",
            string spitUpText = "pokemon-changeform",
            IEnumerable<GulpTransformation> gulpTransformations = null,
            IEnumerable<General.FormTransformation> spitUpTransformations = null
            )
            : base(effectType: AbilityEffectType.GulpMissile)
        {
            this.gulpText = gulpText;
            this.spitUpText = spitUpText;

            this.gulpTransformations = new List<GulpTransformation>();
            if (gulpTransformations != null)
            {
                List<GulpTransformation> preList = new List<GulpTransformation>(gulpTransformations);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.gulpTransformations.Add(preList[i].Clone());
                }
            }
            this.spitUpTransformations = new List<General.FormTransformation>();
            if (spitUpTransformations != null)
            {
                List<General.FormTransformation> preList = new List<General.FormTransformation>(spitUpTransformations);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.spitUpTransformations.Add(preList[i].Clone());
                }
            }
        }
        public new GulpMissile Clone()
        {
            return new GulpMissile(
                gulpText: gulpText, spitUpText: spitUpText,
                gulpTransformations: gulpTransformations,
                spitUpTransformations: spitUpTransformations
                );
        }
    }

    /// <summary>
    /// Scales the user's stats if they have the specified status conditions.
    /// </summary>
    public class Guts : AbilityEffect
    {
        /// <summary>
        /// The stat scaling to be applied.
        /// </summary>
        public General.StatScale statScale;

        /// <summary>
        /// The status conditions that trigger this effect.
        /// </summary>
        public Filter.Harvest conditionCheck;

        public Guts(
            General.StatScale statScale,
            Filter.Harvest conditionCheck,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Guts, filters: filters)
        {
            this.statScale = statScale.Clone();
            this.conditionCheck = conditionCheck.Clone();
        }
        public new Guts Clone()
        {
            return new Guts(
                statScale: statScale,
                conditionCheck: conditionCheck,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Has a chance to recover consumed items at the end of the turn.
    /// </summary>
    public class Harvest : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the item is recovered.
        /// </summary>
        public string displayText;

        /// <summary>
        /// If the consumed item's pocket is listed here, it is eligible to be recovered.
        /// </summary>
        public HashSet<ItemPocket> pockets;

        public Harvest(
            string displayText = "ability-harvest",
            IEnumerable<ItemPocket> pockets = null,
            float chance = 0.5f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Harvest, chance: chance, filters: filters)
        {
            this.displayText = displayText;
            this.pockets = pockets == null ? new HashSet<ItemPocket>()
                : new HashSet<ItemPocket>(pockets);
        }
        public new Harvest Clone()
        {
            return new Harvest(
                displayText: displayText,
                pockets: pockets,
                chance: chance, filters: filters
                );
        }
    }

    /// <summary>
    /// Has a chance to heal teammates of their status conditions.
    /// </summary>
    public class Healer : AbilityEffect
    {
        /// <summary>
        /// If true, there is one chance check. If satisfied, all teammates status conditions' are healed.
        /// </summary>
        public bool oneTimeAll;

        /// <summary>
        /// If the teammate has a status condition listed here, it can be healed.
        /// </summary>
        public List<string> conditions;
        /// <summary>
        /// If the teammate's status condition has an effect listed here, it can be healed.
        /// </summary>
        public HashSet<PokemonSEType> statusTypes;

        public Healer(
            bool oneTimeAll = false,
            IEnumerable<string> conditions = null,
            IEnumerable<PokemonSEType> statusTypes = null,

            float chance = 0.3f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Healer, chance: chance, filters: filters)
        {
            this.oneTimeAll = oneTimeAll;
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
            this.statusTypes = statusTypes == null ? new HashSet<PokemonSEType>()
                : new HashSet<PokemonSEType>(statusTypes);
        }
        public new Healer Clone()
        {
            return new Healer(
                oneTimeAll: oneTimeAll,
                conditions: conditions,
                statusTypes: statusTypes,
                chance: chance, filters: filters
                );
        }
    }

    /// <summary>
    /// Factors a multiplier for the user's weight.
    /// </summary>
    public class HeavyMetal : AbilityEffect
    {
        /// <summary>
        /// The multiplier applied to the user's weight.
        /// </summary>
        public float weightMultiplier;

        public HeavyMetal(
            float weightMultiplier = 2f
            )
            : base(effectType: AbilityEffectType.HeavyMetal)
        {
            this.weightMultiplier = weightMultiplier;
        }
        public new HeavyMetal Clone()
        {
            return new HeavyMetal(weightMultiplier: weightMultiplier);
        }
    }

    /// <summary>
    /// May pick up an item after battle.
    /// </summary>
    public class HoneyGather : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the user picks up an item.
        /// </summary>
        public string displayText;

        public HoneyGather(
            string displayText = "ability-honeygather",
            float baseChance = 0.05f
            )
            : base(effectType: AbilityEffectType.HoneyGather)
        {
            this.displayText = displayText;
        }
        public new HoneyGather Clone()
        {
            return new HoneyGather(displayText: displayText);
        }
    }

    /// <summary>
    /// Changes the Pokémon's form at the end of the turn.
    /// </summary>
    public class HungerSwitch : AbilityEffect
    {
        public enum ChangeMode
        {
            /// <summary>
            /// If the Pokémon is of the form <seealso cref="pokemonID1"/>, it will switch into 
            /// <seealso cref="pokemonID2"/> and vice versa.
            /// </summary>
            Alternating,
            /// <summary>
            /// The Pokémon will only switch from <seealso cref="pokemonID1"/> into
            /// <seealso cref="pokemonID2"/>.
            /// </summary>
            Consecutive
        }

        /// <summary>
        /// The method by switch to switch form.
        /// </summary>
        public ChangeMode mode;

        /// <summary>
        /// First Pokémon ID.
        /// </summary>
        public string pokemonID1;
        /// <summary>
        /// Second Pokémon ID.
        /// </summary>
        public string pokemonID2;
        /// <summary>
        /// Accompanying text for the form change.
        /// </summary>
        public string displayText;

        public HungerSwitch(
            string pokemonID1, string pokemonID2, string displayText = null,
            ChangeMode mode = ChangeMode.Alternating
            ) : base(effectType: AbilityEffectType.HungerSwitch)
        {
            this.mode = mode;
            this.pokemonID1 = pokemonID1;
            this.pokemonID2 = pokemonID2;
            this.displayText = displayText;
        }
        public new HungerSwitch Clone()
        {
            return new HungerSwitch(
                pokemonID1: pokemonID1, pokemonID2: pokemonID2, displayText: displayText,
                mode: mode
                );
        }

    }

    /// <summary>
    /// Scales accuracy and evasion while using certain moves.
    /// </summary>
    public class Hustle : AbilityEffect
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

        /// <summary>
        /// If the move's category is contained here, the stat scales apply.
        /// </summary>
        public HashSet<MoveCategory> moveCategories;

        public Hustle(
            General.StatScale statScale,
            bool victoryStar = false,
            float defeatistThreshold = -1,

            IEnumerable<MoveCategory> moveCategories = null
            )
            : base(effectType: AbilityEffectType.Hustle)
        {
            this.statScale = statScale.Clone();
            this.victoryStar = victoryStar;
            this.defeatistThreshold = defeatistThreshold;
            this.moveCategories = moveCategories == null ? new HashSet<MoveCategory>()
                : new HashSet<MoveCategory>(moveCategories);
        }
        public new Hustle Clone()
        {
            return new Hustle(
                statScale: statScale,
                victoryStar: victoryStar,
                defeatistThreshold: defeatistThreshold,
                moveCategories: moveCategories
                );
        }
    }

    /// <summary>
    /// Has a chance to heal the user or its teammates of their status conditions.
    /// </summary>
    public class Hydration : AbilityEffect
    {
        /// <summary>
        /// If true, there is one chance check. If satisfied, all teammates status conditions' are healed.
        /// </summary>
        public bool oneTimeAll;
        /// <summary>
        /// If true, the user can heal itself.
        /// </summary>
        public bool healSelf;
        /// <summary>
        /// If true, the user can heal its allies.
        /// </summary>
        public bool healer;

        /// <summary>
        /// If the teammate has a status condition listed here, it can be healed.
        /// </summary>
        public List<string> conditions;
        /// <summary>
        /// If the teammate's status condition has an effect listed here, it can be healed.
        /// </summary>
        public HashSet<PokemonSEType> statusTypes;

        public Hydration(
            bool oneTimeAll = false,
            bool healSelf = true, bool healer = false,
            IEnumerable<string> conditions = null,
            IEnumerable<PokemonSEType> statusTypes = null,

            float chance = 0.3f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Hydration, chance: chance, filters: filters)
        {
            this.oneTimeAll = oneTimeAll;
            this.healSelf = healSelf;
            this.healer = healer;
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
            this.statusTypes = statusTypes == null ? new HashSet<PokemonSEType>()
                : new HashSet<PokemonSEType>(statusTypes);
        }
        public new Hydration Clone()
        {
            return new Hydration(
                oneTimeAll: oneTimeAll,
                healSelf: healSelf, healer: healer,
                conditions: conditions,
                statusTypes: statusTypes,
                chance: chance, filters: filters
                );
        }
    }

    /// <summary>
    /// Prevents the lowering of the specified Pokémon's stats.
    /// </summary>
    public class HyperCutter : AbilityEffect
    {
        /// <summary>
        /// The stats protected by this ability.
        /// </summary>
        public HashSet<PokemonStats> affectedStats;
        /// <summary>
        /// If true, all stats are protected.
        /// </summary>
        public bool clearBody;

        /// <summary>
        /// The text displayed when the stat changes are blocked.
        /// </summary>
        public string displayText;

        /// <summary>
        /// If true, the specified stats cannot be lowered.
        /// </summary>
        public bool preventLower;
        /// <summary>
        /// If true, the specified stats cannot be raised.
        /// </summary>
        public bool preventRaise;
        /// <summary>
        /// If true, the the user cannot lower its own stats.
        /// </summary>
        public bool affectSelf;

        public HyperCutter(
            IEnumerable<PokemonStats> affectedStats = null,
            bool clearBody = false,
            string displayText = "ability-hypercutter",
            bool preventLower = true, bool preventRaise = false, bool affectSelf = false
            )
            : base(effectType: AbilityEffectType.HyperCutter)
        {
            this.affectedStats = affectedStats != null ? new HashSet<PokemonStats>(affectedStats)
                : new HashSet<PokemonStats>();
            this.clearBody = clearBody;
            this.displayText = displayText;
            this.preventLower = preventLower;
            this.preventRaise = preventRaise;
            this.affectSelf = affectSelf;
        }
        public new HyperCutter Clone()
        {
            return new HyperCutter(
                affectedStats: affectedStats,
                clearBody: clearBody,
                displayText: displayText,
                preventLower: preventLower, preventRaise: preventRaise, affectSelf: affectSelf
                );
        }
    }

    /// <summary>
    /// Scales the damage taken from certain move types.
    /// </summary>
    public class IceScales : AbilityEffect
    {
        /// <summary>
        /// The amount to scale taken damage by.
        /// </summary>
        public float damageModifier;

        /// <summary>
        /// If true, the move category check will occur.
        /// </summary>
        public bool useCategory;
        /// <summary>
        /// If <seealso cref="useCategory"/> is set to true, the move must match this category.
        /// </summary>
        public MoveCategory category;

        /// <summary>
        /// If non-empty, the move must have a tag contained here.
        /// </summary>
        public HashSet<MoveTag> tags;

        public IceScales(
            float damageModifier = 1f,
            bool useCategory = false, MoveCategory category = MoveCategory.Special,
            IEnumerable<MoveTag> tags = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.IceScales, filters: filters)
        {
            this.damageModifier = damageModifier;
            this.useCategory = useCategory;
            this.category = category;
            this.tags = tags == null ? new HashSet<MoveTag>() : new HashSet<MoveTag>(tags);
        }
        public new IceScales Clone()
        {
            return new IceScales(
                damageModifier: damageModifier,
                useCategory: useCategory, category: category,
                tags: tags,

                filters: filters
                );
        }
    }

    /// <summary>
    /// Changes the appearance of the Pokémon to that of the last conscious, non-Egg Pokémon 
    /// in its trainer's party. Breaks once the user takes direct damage.
    /// </summary>
    public class Illusion : AbilityEffect
    {
        /// <summary>
        /// Text displayed when Illusion breaks.
        /// </summary>
        public string displayText;

        public Illusion(
            string displayText = "ability-illusion"
            )
            : base(effectType: AbilityEffectType.Illusion)
        {
            this.displayText = displayText;
        }
        public new Illusion Clone()
        {
            return new Illusion(displayText: displayText);
        }
    }

    /// <summary>
    /// Ignores substitutes and screens when attacking.
    /// </summary>
    public class Infiltrator : AbilityEffect
    {
        /// <summary>
        /// If true, substitutes are ignored.
        /// </summary>
        public bool bypassSubstitute;
        /// <summary>
        /// If true, screens are ignored.
        /// </summary>
        public bool bypassScreens;

        public Infiltrator(
            bool bypassSubstitute = true,
            bool bypassScreens = true
            )
            : base(effectType: AbilityEffectType.Infiltrator)
        {
            this.bypassSubstitute = bypassSubstitute;
            this.bypassScreens = bypassScreens;
        }
        public new Infiltrator Clone()
        {
            return new Infiltrator(
                bypassSubstitute: bypassSubstitute,
                bypassScreens: bypassScreens
                );
        }
    }

    /// <summary>
    /// Modifies stats the opposing team's stats when it enters battle.
    /// </summary>
    public class Intimidate : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        public Intimidate(
            General.StatStageMod statStageMod,
            IEnumerable<Filter.FilterEffect> filters = null)
            : base(effectType: AbilityEffectType.Intimidate, filters: filters)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new Intimidate Clone()
        {
            return new Intimidate(
                statStageMod: statStageMod,
                filters: filters);
        }
    }
    /// <summary>
    /// Blocks the effect of specified <seealso cref="Intimidate"/> abilities.
    /// </summary>
    public class IntimidateBlock : AbilityEffect
    {
        /// <summary>
        /// The <seealso cref="Intimidate"/> abilities blocked.
        /// </summary>
        public HashSet<string> abilitiesBlocked;

        /// <summary>
        /// The text displayed when an ability is blocked.
        /// </summary>
        public string displayText;

        public IntimidateBlock(
            IEnumerable<string> abilitiesBlocked = null,
            string displayText = "pokemon-unaffect"
            )
            : base(effectType: AbilityEffectType.IntimidateBlock)
        {
            this.abilitiesBlocked = abilitiesBlocked == null ? new HashSet<string>()
                : new HashSet<string>(abilitiesBlocked);
            this.displayText = displayText;
        }
        public new IntimidateBlock Clone()
        {
            return new IntimidateBlock(
                abilitiesBlocked: abilitiesBlocked,
                displayText: displayText);
        }
    }
    /// <summary>
    /// Modifies stats the opposing team's stats when it enters battle.
    /// </summary>
    public class IntimidateTrigger : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        /// <summary>
        /// The <seealso cref="Intimidate"/> abilities blocked.
        /// </summary>
        public HashSet<string> abilityTriggers;

        public IntimidateTrigger(
            General.StatStageMod statStageMod,
            IEnumerable<string> abilityTriggers = null,
            IEnumerable<Filter.FilterEffect> filters = null)
            : base(effectType: AbilityEffectType.IntimidateTrigger, filters: filters)
        {
            this.statStageMod = statStageMod.Clone();
            this.abilityTriggers = abilityTriggers == null ? new HashSet<string>()
                : new HashSet<string>(abilityTriggers);
        }
        public new IntimidateTrigger Clone()
        {
            return new IntimidateTrigger(
                statStageMod: statStageMod,
                abilityTriggers: abilityTriggers,
                filters: filters);
        }
    }

    /// <summary>
    /// Modifies stats the user's stats when it enters battle.
    /// </summary>
    public class IntrepidSword : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        public IntrepidSword(
            General.StatStageMod statStageMod,
            IEnumerable<Filter.FilterEffect> filters = null)
            : base(effectType: AbilityEffectType.Justified, filters: filters)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new IntrepidSword Clone()
        {
            return new IntrepidSword(
                statStageMod: statStageMod,
                filters: filters);
        }
    }

    /// <summary>
    /// Boosts the power of the user's moves that match filter criteria.
    /// </summary>
    public class IronFist : AbilityEffect
    {
        /// <summary>
        /// The power multiplier applied to moves.
        /// </summary>
        public float powerMultiplier;

        /// <summary>
        /// If positive, the user's HP must be under this threshold to activate Iron Fist.
        /// </summary>
        public float blazeThreshold;

        /// <summary>
        /// If true, this effect also applies to teammate's moves as well.
        /// </summary>
        public bool steelySpirit;

        public IronFist(
            float powerMultiplier = 1.2f,
            float blazeThreshold = 0f,
            bool steelySpirit = false,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.IronFist, filters: filters)
        {
            this.powerMultiplier = powerMultiplier;
            this.blazeThreshold = blazeThreshold;
            this.steelySpirit = steelySpirit;
        }
        public new IronFist Clone()
        {
            return new IronFist(
                powerMultiplier: powerMultiplier,
                blazeThreshold: blazeThreshold,
                steelySpirit: steelySpirit,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Modifies stats if the user is hit by the specified attacks.
    /// </summary>
    public class Justified : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        /// <summary>
        /// This effect only triggers on damaging moves.
        /// </summary>
        public bool onlyDamaging;

        /// <summary>
        /// If true, the move must match the specified category.
        /// </summary>
        public bool mustMatchCategory;
        /// <summary>
        /// The category the move must match, if <seealso cref="mustMatchCategory"/> is true.
        /// </summary>
        public MoveCategory category;

        public Justified(
            General.StatStageMod statStageMod,
            bool onlyDamaging = true,
            bool mustMatchCategory = false,
            MoveCategory category = MoveCategory.Physical,

            IEnumerable<Filter.FilterEffect> filters = null)
            : base(effectType: AbilityEffectType.Justified, filters: filters)
        {
            this.statStageMod = statStageMod.Clone();
            this.onlyDamaging = onlyDamaging;
            this.mustMatchCategory = mustMatchCategory;
            this.category = category;
        }
        public new Justified Clone()
        {
            return new Justified(
                statStageMod: statStageMod,
                onlyDamaging: onlyDamaging,
                mustMatchCategory: mustMatchCategory,
                category: category,
                filters: filters);
        }
    }

    /// <summary>
    /// Forces this Pokémon to become airborne.
    /// </summary>
    public class Levitate : AbilityEffect
    {
        public Levitate(
            )
            : base(effectType: AbilityEffectType.Levitate)
        {

        }
        public new Levitate Clone()
        {
            return new Levitate();
        }
    }

    /// <summary>
    /// Prevents the user from using any held items.
    /// </summary>
    public class Klutz : AbilityEffect
    {
        public Klutz(
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Klutz, filters: filters)
        {

        }
        public new Klutz Clone()
        {
            return new Klutz(
                filters: filters);
        }
    }

    /// <summary>
    /// Draws the specified moves to this Pokémon.
    /// </summary>
    public class LightningRod : AbilityEffect
    {
        /// <summary>
        /// If true, this ability will also draw ally moves.
        /// </summary>
        public bool affectAlly;

        public LightningRod(
            bool affectAlly = true,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.LightningRod, filters: filters)
        {
            this.affectAlly = affectAlly;
        }
        public new LightningRod Clone()
        {
            return new LightningRod(
                affectAlly: affectAlly,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Prevents this Pokémon from being afflicted with the given status conditions.
    /// </summary>
    public class Limber : AbilityEffect
    {
        /// <summary>
        /// If the Pokémon has a status condition listed here, it can be blocked.
        /// </summary>
        public List<string> conditions;
        /// <summary>
        /// If the teammate's status condition has an effect listed here, it can be healed.
        /// </summary>
        public HashSet<PokemonSEType> statusTypes;

        /// <summary>
        /// If true, the user can heal itself.
        /// </summary>
        public bool healSelf;
        /// <summary>
        /// If true, the conditions are blocked for the Pokémon's team.
        /// </summary>
        public bool pastelVeil;

        public Limber(
            IEnumerable<string> conditions = null,
            IEnumerable<PokemonSEType> statusTypes = null,
            bool healSelf = true, bool pastelVeil = false,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Limber, filters: filters)
        {
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
            this.statusTypes = statusTypes == null ? new HashSet<PokemonSEType>()
                : new HashSet<PokemonSEType>(statusTypes);
            this.healSelf = healSelf;
            this.pastelVeil = pastelVeil;
        }
        public new Limber Clone()
        {
            return new Limber(
                conditions: conditions,
                statusTypes: statusTypes,
                healSelf: healSelf, pastelVeil: pastelVeil,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Inverts the effects of HP-draining moves against this Pokemon.
    /// </summary>
    public class LiquidOoze : AbilityEffect
    {
        /// <summary>
        /// The percentage of absorbed damage dealt.
        /// </summary>
        public float damagePercent;

        public LiquidOoze(
            float damagePercent = 1f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.LiquidOoze, filters: filters)
        {
            this.damagePercent = damagePercent;
        }
        public new LiquidOoze Clone()
        {
            return new LiquidOoze(
                damagePercent: damagePercent,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Removes move tags from used moves.
    /// </summary>
    public class LongReach : AbilityEffect
    {
        /// <summary>
        /// The tags that are removed from used moves.
        /// </summary>
        public HashSet<MoveTag> moveTags;

        public LongReach(
            IEnumerable<MoveTag> moveTags = null,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.LongReach, filters: filters)
        {
            this.moveTags = moveTags == null ? null : new HashSet<MoveTag>(moveTags);
        }
        public new LongReach Clone()
        {
            return new LongReach(
                moveTags: moveTags,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Reflects certain moves back to their attackers.
    /// </summary>
    public class MagicBounce : AbilityEffect
    {
        /// <summary>
        /// Defines how moves are reflected.
        /// </summary>
        public General.MagicCoat magicCoat;

        public MagicBounce(
            General.MagicCoat magicCoat,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.MagicBounce, filters: filters)
        {
            this.magicCoat = magicCoat.Clone();
        }
        public new MagicBounce Clone()
        {
            return new MagicBounce(
                magicCoat: magicCoat,

                filters: filters
                );
        }
    }

    /// <summary>
    /// Prevents damage from all sources except direct hits.
    /// </summary>
    public class MagicGuard : AbilityEffect
    {
        public MagicGuard(
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.MagicGuard, filters: filters)
        {

        }
        public new MagicGuard Clone()
        {
            return new MagicGuard(
                filters: filters
                );
        }
    }

    /// <summary>
    /// May steal the target's held item when attacking.
    /// </summary>
    public class Magician : AbilityEffect
    {
        /// <summary>
        /// The text displayed when an item is stolen.
        /// </summary>
        public string displayText;

        public Magician(
            string displayText = "ability-magician",
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Magician, filters: filters)
        {
            this.displayText = displayText;
        }
        public new Magician Clone()
        {
            return new Magician(
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Specialized case of <seealso cref="ShadowTag"/> where specific-typed Pokémon are trapped.
    /// </summary>
    public class MagnetPull : ShadowTag
    {
        public MagnetPull(
            IEnumerable<string> types = null,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(filters: filters)
        {
            Filter.TypeList typeList = new Filter.TypeList(types: types);
            this.filters.Add(typeList);
        }
    }

    /// <summary>
    /// Depending on the terrain, Mimicry will change the type of a Pokémon with this Ability.
    /// </summary>
    public class Mimicry : AbilityEffect
    {
        public class MimicryCondition
        {
            public List<string> conditions;
            public List<string> types;
            public string displayText;

            public MimicryCondition(
                IEnumerable<string> conditions,
                IEnumerable<string> types,
                string displayText = "pokemon-changetype"
                )
            {
                this.conditions = new List<string>(conditions);
                this.types = new List<string>(types);
                this.displayText = displayText;
            }
            public MimicryCondition Clone()
            {
                return new MimicryCondition(
                    conditions: conditions,
                    types: types,
                    displayText: displayText
                    );
            }
        }
        /// <summary>
        /// The conditions for which Mimicry changes the user's type.
        /// </summary>
        public List<MimicryCondition> conditions;

        /// <summary>
        /// The text displayed when the user reverts back to its original typing.
        /// </summary>
        public string revertText;

        public Mimicry(
            IEnumerable<MimicryCondition> conditions,
            string revertText = "pokemon-changetype",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Mimicry, filters: filters)
        {
            this.conditions = new List<MimicryCondition>(conditions);
            this.revertText = revertText;
        }
        public new Mimicry Clone()
        {
            return new Mimicry(
                conditions: conditions,
                revertText: revertText,

                filters: filters
                );
        }
    }

    /// <summary>
    /// Scales the user's stats if it has an ally in battle with a specific ability.
    /// </summary>
    public class Minus : AbilityEffect
    {
        /// <summary>
        /// The stats to scale.
        /// </summary>
        public General.StatScale statScale;

        /// <summary>
        /// The abilities that an ally may have that triggers stat scaling.
        /// </summary>
        public List<string> allyAbilities;

        public Minus(
            General.StatScale statScale,
            IEnumerable<string> allyAbilities,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Minus, filters: filters)
        {
            this.statScale = statScale.Clone();
            this.allyAbilities = new List<string>(allyAbilities);
        }
        public new Minus Clone()
        {
            return new Minus(
                statScale: statScale,
                allyAbilities: allyAbilities,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Reflects the stat-lowering effects of moves and abilities back at the user.
    /// </summary>
    public class MirrorArmor : AbilityEffect
    {
        /// <summary>
        /// Defiant activated if the specified stats were lowered. 
        /// </summary>
        public HashSet<PokemonStats> lowerTriggers;
        /// <summary>
        /// Defiant activated if the specified stats were raised. 
        /// </summary>
        public HashSet<PokemonStats> raiseTriggers;

        /// <summary>
        /// If true, this can only be activated if the stats were changed by an opponent.
        /// </summary>
        public bool onlyOpposing;

        public MirrorArmor(
            IEnumerable<PokemonStats> lowerTriggers = null, IEnumerable<PokemonStats> raiseTriggers = null,
            bool onlyOpposing = true
            )
            : base(effectType: AbilityEffectType.MirrorArmor)
        {
            this.lowerTriggers = lowerTriggers == null ? new HashSet<PokemonStats>()
                : new HashSet<PokemonStats>(lowerTriggers);
            this.raiseTriggers = raiseTriggers == null ? new HashSet<PokemonStats>()
                : new HashSet<PokemonStats>(raiseTriggers);
            this.onlyOpposing = onlyOpposing;
        }
        public new MirrorArmor Clone()
        {
            return new MirrorArmor(
                lowerTriggers: lowerTriggers, raiseTriggers: raiseTriggers,
                onlyOpposing: onlyOpposing
                );
        }
    }

    /// <summary>
    /// Ignores ignorable abilities when attacking.
    /// </summary>
    public class MoldBreaker : AbilityEffect
    {
        /// <summary>
        /// The text that displays when the Pokémon enters battle.
        /// </summary>
        public string displayText;

        public MoldBreaker(
            string displayText = "ability-moldbreaker"
            )
            : base(effectType: AbilityEffectType.MoldBreaker)
        {
            this.displayText = displayText;
        }
        public new MoldBreaker Clone()
        {
            return new MoldBreaker(displayText: displayText);
        }
    }

    /// <summary>
    /// Randomly modifies 2 of the user's stats at the end of each turn.
    /// </summary>
    public class Moody : AbilityEffect
    {
        public List<General.StatStageMod> statStageMods1;
        public List<General.StatStageMod> statStageMods2;

        public Moody(
            IEnumerable<General.StatStageMod> statStageMods1 = null,
            IEnumerable<General.StatStageMod> statStageMods2 = null)
            : base(effectType: AbilityEffectType.Moody)
        {
            this.statStageMods1 = statStageMods1 == null ? null
                : new List<General.StatStageMod>(statStageMods1);
            this.statStageMods2 = statStageMods2 == null ? null
                : new List<General.StatStageMod>(statStageMods2);
        }
        public new Moody Clone()
        {
            return new Moody(
                statStageMods1: statStageMods1,
                statStageMods2: statStageMods2
                );
        }
    }

    /// <summary>
    /// Modifies stats if the user knocks out an opposing Pokémon.
    /// </summary>
    public class Moxie : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        public Moxie(General.StatStageMod statStageMod)
            : base(effectType: AbilityEffectType.Moxie)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new Moxie Clone()
        {
            return new Moxie(statStageMod: statStageMod);
        }
    }

    /// <summary>
    /// Can change the user's form if it is holding an item with the effect <seealso cref="Items.ArceusPlate"/>.
    /// </summary>
    public class Multitype : AbilityEffect
    {
        public Multitype(
            )
            : base(effectType: AbilityEffectType.Multitype)
        {

        }
        public new Multitype Clone()
        {
            return new Multitype();
        }
    }

    /// <summary>
    /// Scales damage against the user if they are at least at the specified HP Threshold.
    /// </summary>
    public class Multiscale : AbilityEffect
    {
        /// <summary>
        /// Scaled damage.
        /// </summary>
        public float damageModifier;
        /// <summary>
        /// The HP % the user needs to be to activate Multiscale.
        /// </summary>
        public float hpThreshold;

        public Multiscale(
            float damageModifier = 0.5f,
            float hpThreshold = 1f,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Multiscale, filters: filters)
        {
            this.damageModifier = damageModifier;
            this.hpThreshold = hpThreshold;
        }
        public new Multiscale Clone()
        {
            return new Multiscale(
                damageModifier: damageModifier,
                hpThreshold: hpThreshold,
                filters: filters
                );
        }
    }

    /// <summary>
    /// If attacked, the attacker's ability changes to this one.
    /// </summary>
    public class Mummy : AbilityEffect
    {
        /// <summary>
        /// If true, the user will switch this ability with the target's ability.
        /// </summary>
        public bool wanderingSpirit;

        /// <summary>
        /// The text displayed when the attacker gains the ability.
        /// </summary>
        public string displayText;

        /// <summary>
        /// If non-empty, then the attacker's abilities are replaced by these listed abilities.
        /// </summary>
        public List<string> setAbilities;

        public Mummy(
            bool wanderingSpirit = false,
            IEnumerable<string> setAbilities = null,
            string displayText = "ability-mummy",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Mummy, filters: filters)
        {
            this.wanderingSpirit = wanderingSpirit;
            this.setAbilities = setAbilities == null ? null : new List<string>(setAbilities);
            this.displayText = displayText;

        }
        public new Mummy Clone()
        {
            return new Mummy(
                wanderingSpirit: wanderingSpirit,
                setAbilities: setAbilities,
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Heals the user's status conditions upon switch out.
    /// </summary>
    public class NaturalCure : AbilityEffect
    {
        /// <summary>
        /// Defines the status conditions to be healed.
        /// </summary>
        public List<Filter.Harvest> conditions;

        /// <summary>
        /// If non-null, recovers the user's HP upon switch out.
        /// </summary>
        public General.HealHP regenerator;

        public NaturalCure(
            IEnumerable<Filter.Harvest> conditions = null,
            General.HealHP regenerator = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.NaturalCure, filters: filters)
        {
            this.conditions = new List<Filter.Harvest>();
            if (conditions != null)
            {
                List<Filter.Harvest> tempConditions = new List<Filter.Harvest>(conditions);
                for (int i = 0; i < tempConditions.Count; i++)
                {
                    this.conditions.Add(tempConditions[i].Clone());
                }
            }
            this.regenerator = regenerator == null ? null : regenerator.Clone();
        }
        public new NaturalCure Clone()
        {
            return new NaturalCure(
                conditions: conditions,
                regenerator: regenerator,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Nullifies all other abilities while this ability is active.
    /// </summary>
    public class NeutralizingGas : AbilityEffect
    {
        /// <summary>
        /// Text displayed when this ability becomes active.
        /// </summary>
        public string displayText;

        public NeutralizingGas(
            string displayText = "ability-neutralizinggas",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.NeutralizingGas, filters: filters)
        {
            this.displayText = displayText;
        }
        public new NeutralizingGas Clone()
        {
            return new NeutralizingGas(
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Bypasses accuracy checks when attacking or being attacked.
    /// </summary>
    public class NoGuard : AbilityEffect
    {
        /// <summary>
        /// If true, the user bypasses accuracy checks when attacking.
        /// </summary>
        public bool bypassWhenAttacking;
        /// <summary>
        /// If true, attacks targeting the user bypass accuracy checks.
        /// </summary>
        public bool bypassWhenAttacked;

        public NoGuard(
            bool bypassWhenAttacking = true,
            bool bypassWhenAttacked = true,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.NoGuard, filters: filters)
        {
            this.bypassWhenAttacking = bypassWhenAttacking;
            this.bypassWhenAttacked = bypassWhenAttacked;
        }
        public new NoGuard Clone()
        {
            return new NoGuard(
                bypassWhenAttacking: bypassWhenAttacking,
                bypassWhenAttacked: bypassWhenAttacked,
                filters: filters
                );
        }
    }

    /// <summary>
    /// The Pokémon is immune to certain status effect types.
    /// </summary>
    public class Oblivious : AbilityEffect
    {
        /// <summary>
        /// Accompanying text to display when blocking a status.
        /// </summary>
        public string displayText;
        /// <summary>
        /// If true, the effects are blocked for the Pokémon's team.
        /// </summary>
        public bool aromaVeil;
        /// <summary>
        /// Status effects that are blocked.
        /// </summary>
        public HashSet<PokemonSEType> effectsBlocked;

        public Oblivious(
            string displayText = "ability-oblivious",
            bool aromaVeil = false,
            IEnumerable<PokemonSEType> effectsBlocked = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Oblivious, filters: filters)
        {
            this.displayText = displayText;
            this.aromaVeil = aromaVeil;
            this.effectsBlocked = effectsBlocked == null
                ? new HashSet<PokemonSEType> { PokemonSEType.Infatuation }
                : new HashSet<PokemonSEType>(effectsBlocked);
        }
        public new Oblivious Clone()
        {
            return new Oblivious(
                displayText: displayText,
                aromaVeil: aromaVeil,
                effectsBlocked: effectsBlocked);
        }
    }

    /// <summary>
    /// The Pokémon is immune to passive damage from weather.
    /// </summary>
    public class Overcoat : AbilityEffect
    {
        /// <summary>
        /// If true, all weather damage is blocked.
        /// </summary>
        public bool allWeather;

        /// <summary>
        /// The battle conditions defined here have weather damage blocked.
        /// </summary>
        public List<string> conditions;

        public Overcoat(
            bool allWeather = false,
            IEnumerable<string> conditions = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Overcoat, filters: filters)
        {
            this.allWeather = allWeather;
            this.conditions = conditions == null ? new List<string>() : new List<string>(conditions);
        }
        public new Overcoat Clone()
        {
            return new Overcoat(
                allWeather: allWeather,
                conditions: conditions,
                filters: filters);
        }
    }

    /// <summary>
    /// The user's moves have their hit count multiplied.
    /// </summary>
    public class ParentalBond : AbilityEffect
    {
        public class BondedHit
        {
            public float damageModifier;
            public BondedHit(float damageModifier = 0.25f)
            {
                this.damageModifier = damageModifier;
            }
            public BondedHit Clone()
            {
                return new BondedHit(damageModifier: damageModifier);
            }
        }
        public List<BondedHit> bondedHits;

        public ParentalBond(
            IEnumerable<BondedHit> bondedHits = null,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.ParentalBond, filters: filters)
        {
            this.bondedHits = new List<BondedHit>();
            if (bondedHits != null)
            {
                List<BondedHit> preList = new List<BondedHit>();
                for (int i = 0; i < preList.Count; i++)
                {
                    this.bondedHits.Add(preList[i].Clone());
                }
            }
        }
        public new ParentalBond Clone()
        {
            return new ParentalBond(
                bondedHits: bondedHits,
                filters: filters
                );
        }
    }

    /// <summary>
    /// May steal the attackers's held item when being attacked.
    /// </summary>
    public class Pickpocket : AbilityEffect
    {
        /// <summary>
        /// The text displayed when an item is stolen.
        /// </summary>
        public string displayText;

        public Pickpocket(
            string displayText = "ability-magician",
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Pickpocket, filters: filters)
        {
            this.displayText = displayText;
        }
        public new Pickpocket Clone()
        {
            return new Pickpocket(
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Has a chance to recover used items at the end of the turn.
    /// </summary>
    public class Pickup : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the item is picked up.
        /// </summary>
        public string displayText;

        public Pickup(
            string displayText = "ability-pickup",

            float chance = 1f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Pickup, chance: chance, filters: filters)
        {
            this.displayText = displayText;
        }
        public new Pickup Clone()
        {
            return new Pickup(
                displayText: displayText,
                chance: chance, filters: filters
                );
        }
    }

    /// <summary>
    /// Heals the user's HP instead of losing HP if the user is afflicted by certain status conditions.
    /// </summary>
    public class PoisonHeal : AbilityEffect
    {
        public class HealCondition
        {
            public List<Filter.Harvest> conditions;
            public General.HealHP heal;

            public HealCondition(
                IEnumerable<Filter.Harvest> conditions = null,
                General.HealHP heal = null
                )
            {
                this.conditions = new List<Filter.Harvest>();
                if (conditions != null)
                {
                    List<Filter.Harvest> preList = new List<Filter.Harvest>(conditions);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.conditions.Add(preList[i].Clone());
                    }
                }
                this.heal = heal == null ? null : heal.Clone();
            }
            public HealCondition Clone()
            {
                return new HealCondition(
                    conditions: conditions,
                    heal: heal
                    );
            }
        }
        public List<HealCondition> conditions;

        public PoisonHeal(
            IEnumerable<HealCondition> conditions = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.PoisonHeal, filters: filters)
        {
            this.conditions = new List<HealCondition>();
            if (conditions != null)
            {
                List<HealCondition> tempConditions = new List<HealCondition>(conditions);
                for (int i = 0; i < tempConditions.Count; i++)
                {
                    this.conditions.Add(tempConditions[i].Clone());
                }
            }
        }
        public new PoisonHeal Clone()
        {
            return new PoisonHeal(
                conditions: conditions,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Inflicts status conditions or effects upon attackers.
    /// </summary>
    public class PoisonPoint : AbilityEffect
    {
        /// <summary>
        /// The status that is inflicted upon the target when attacking this Pokémon.
        /// </summary>
        public General.InflictStatus inflictStatus;
        /// <summary>
        /// If true, the status will only be inflicted if the attacker makes contact with this Pokémon.
        /// </summary>
        public bool requiresContact;

        public PoisonPoint(
            General.InflictStatus inflictStatus = null,
            bool requiresContact = true,

            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1)
            : base(effectType: AbilityEffectType.PoisonPoint, filters: filters, chance: chance)
        {
            this.inflictStatus = inflictStatus;
            this.requiresContact = requiresContact;
        }
        public new PoisonPoint Clone()
        {
            return new PoisonPoint(
                inflictStatus: inflictStatus,
                requiresContact: requiresContact,
                chance: chance
                );
        }
    }

    /// <summary>
    /// Inflicts the specified status upon the target when attacking.
    /// </summary>
    public class PoisonTouch : AbilityEffect
    {
        public class EffectSporeCondition
        {
            public General.InflictStatus inflictStatus;
            public float chance;

            public EffectSporeCondition(
                General.InflictStatus inflictStatus,
                float chance = 1f / 3
                )
            {
                this.inflictStatus = inflictStatus.Clone();
                this.chance = chance;
            }
            public EffectSporeCondition Clone()
            {
                return new EffectSporeCondition(
                    inflictStatus: inflictStatus,
                    chance: chance
                    );
            }
        }
        public List<EffectSporeCondition> effectSpores;

        public General.InflictStatus inflictStatus;

        public PoisonTouch(
            General.InflictStatus inflictStatus = null,
            IEnumerable<EffectSporeCondition> effectSpores = null,

            IEnumerable<Filter.FilterEffect> filters = null,
            float chance = -1
            )
            : base(effectType: AbilityEffectType.PoisonTouch,
                    filters: filters, chance: chance
                    )
        {
            this.inflictStatus = inflictStatus == null ? null : inflictStatus.Clone();
            this.effectSpores = new List<EffectSporeCondition>();
            if (effectSpores != null)
            {
                List<EffectSporeCondition> preList = new List<EffectSporeCondition>(effectSpores);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.effectSpores.Add(preList[i].Clone());
                }
            }
        }
        public new PoisonTouch Clone()
        {
            return new PoisonTouch(
                inflictStatus: inflictStatus,
                effectSpores: effectSpores,
                filters: filters, chance: chance
                );
        }
        public General.InflictStatus GetAnEffectSporeStatus()
        {
            General.InflictStatus status = effectSpores.Count == 0 ? null : effectSpores[0].inflictStatus;

            List<float> levelChances = new List<float>();
            float totalChance = 0;
            for (int i = 0; i < effectSpores.Count; i++)
            {
                totalChance += effectSpores[i].chance;
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
                        return effectSpores[i].inflictStatus;
                    }
                    else if (i == levelChances.Count - 1)
                    {
                        return effectSpores[i].inflictStatus;
                    }
                }
            }

            return status;
        }
    }

    /// <summary>
    /// When an ally faints, the user gains the ally's ability.
    /// </summary>
    public class PowerOfAlchemy : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the user gains its ally's ability.
        /// </summary>
        public string displayText;

        public PowerOfAlchemy(
            string displayText = "ability-powerofalchemy",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.PowerOfAlchemy, filters: filters)
        {
            this.displayText = displayText;

        }
        public new PowerOfAlchemy Clone()
        {
            return new PowerOfAlchemy(
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Provides an immunity to target Pokémon determined by this effect's filters.
    /// </summary>
    public class Prankster : AbilityEffect
    {
        public Prankster(
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Prankster, filters: filters)
        {

        }
        public new Prankster Clone()
        {
            return new Prankster(filters: filters);
        }
    }

    /// <summary>
    /// Scales PP usage of attackers when attacking this Pokémon.
    /// </summary>
    public class Pressure : AbilityEffect
    {
        public enum DeductionMode
        {
            Scale,
            Flat
        }
        public DeductionMode mode;

        /// <summary>
        /// The PP usage multiplier to apply.
        /// </summary>
        public float ppLoss;
        /// <summary>
        /// The text that displays when the Pokémon exerts Pressure.
        /// </summary>
        public string displayText;

        public Pressure(
            DeductionMode mode = DeductionMode.Flat,
            float ppLoss = 1f,
            string displayText = "ability-pressure",
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Pressure, filters: filters)
        {
            this.mode = mode;
            this.ppLoss = ppLoss;
            this.displayText = displayText;
        }
        public new Pressure Clone()
        {
            return new Pressure(
                mode: mode,
                ppLoss: ppLoss,
                displayText: displayText,
                filters: filters);
        }
    }

    /// <summary>
    /// Ignore the target-redirecting effects of moves and abilities when attacking.
    /// </summary>
    public class PropellerTail : AbilityEffect
    {
        public PropellerTail()
            : base(effectType: AbilityEffectType.PropellerTail)
        {
        }
        public new PropellerTail Clone()
        {
            return new PropellerTail();
        }
    }

    /// <summary>
    /// Changes the user's type to the type of the move it is about to use.
    /// </summary>
    public class Protean : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the user's type is changed;
        /// </summary>
        public string displayText;

        public Protean(
            string displayText = "ability-protean"
            )
            : base(effectType: AbilityEffectType.Protean)
        {
            this.displayText = displayText;
        }
        public new Protean Clone()
        {
            return new Protean(displayText: displayText);
        }
    }

    /// <summary>
    /// The user is immune to opponent elevated-priority moves.
    /// </summary>
    public class QueenlyMajesty : AbilityEffect
    {
        /// <summary>
        /// If true, this protection affects teammates as well.
        /// </summary>
        public bool affectsTeam;

        public QueenlyMajesty(
            bool affectsTeam = true
            )
            : base(effectType: AbilityEffectType.QueenlyMajesty)
        {
            this.affectsTeam = affectsTeam;
        }
        public new QueenlyMajesty Clone()
        {
            return new QueenlyMajesty(affectsTeam: affectsTeam);
        }
    }

    /// <summary>
    /// The user has a chance of attacking first within its priority bracket.
    /// </summary>
    public class QuickDraw : AbilityEffect
    {
        /// <summary>
        /// The text displayed if the effect is triggered.
        /// </summary>
        public string displayText;

        public QuickDraw(
            string displayText = "ability-quickdraw",
            float chance = 0.3f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.QuickDraw, chance: chance, filters: filters)
        {
            this.displayText = displayText;
        }
        public new QuickDraw Clone()
        {
            return new QuickDraw(
                displayText: displayText,
                chance: chance, filters: filters
                );
        }
    }

    /// <summary>
    /// Doubles the effect of berries when consumed.
    /// </summary>
    public class Ripen : AbilityEffect
    {
        /// <summary>
        /// The factor by which effects are scaled.
        /// </summary>
        public float effectMultiplier;
        /// <summary>
        /// The text that displays when Ripen triggers.
        /// </summary>
        public string displayText;

        public Ripen(
            float effectMultiplier = 2f,
            string displayText = "ability-ripen",
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Ripen, filters: filters)
        {
            this.effectMultiplier = effectMultiplier;
            this.displayText = displayText;
        }
        public new Ripen Clone()
        {
            return new Ripen(
                effectMultiplier: effectMultiplier,
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Scales move power according to the target's gender compared to the user's own.
    /// </summary>
    public class Rivalry : AbilityEffect
    {
        /// <summary>
        /// The power multiplier applied when the target is of the same gender as the user.
        /// </summary>
        public float sameMultiplier;
        /// <summary>
        /// The power multiplier applied when the target is of the opposite gender to the user.
        /// </summary>
        public float oppositeMultiplier;

        public Rivalry(
            float sameMultiplier = 1.25f, float oppositeMultiplier = 0.75f
            )
            : base(effectType: AbilityEffectType.Rivalry)
        {
            this.sameMultiplier = sameMultiplier;
            this.oppositeMultiplier = oppositeMultiplier;
        }
        public new Rivalry Clone()
        {
            return new Rivalry(
                sameMultiplier: sameMultiplier, oppositeMultiplier: oppositeMultiplier
                );
        }
    }

    /// <summary>
    /// Can change the user's form if it is holding an item with the effect <seealso cref="Items.RKSMemory"/>.
    /// </summary>
    public class RKSSystem : AbilityEffect
    {
        public RKSSystem(
            )
            : base(effectType: AbilityEffectType.RKSSystem)
        {

        }
        public new RKSSystem Clone()
        {
            return new RKSSystem();
        }
    }

    /// <summary>
    /// Prevents the user from taking recoil damage.
    /// </summary>
    public class RockHead : AbilityEffect
    {
        public RockHead(
            )
            : base(effectType: AbilityEffectType.RockHead)
        {

        }
        public new RockHead Clone()
        {
            return new RockHead();
        }
    }

    /// <summary>
    /// When this Pokémon is hit by a direct attack, the attacker loses a portion of their HP.
    /// </summary>
    public class RoughSkin : AbilityEffect
    {
        /// <summary>
        /// Defines how damage is dealt.
        /// </summary>
        public General.Damage damage;

        /// <summary>
        /// This effect is only triggered if the Pokémon is hit by a contact move.
        /// </summary>
        public bool onlyContact;

        public RoughSkin(
            General.Damage damage,
            bool onlyContact = true,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.RoughSkin, filters: filters)
        {
            this.damage = damage.Clone();
            this.onlyContact = onlyContact;
        }
        public new RoughSkin Clone()
        {
            return new RoughSkin(
                damage: damage,
                onlyContact: onlyContact,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Ensures the user can always flee regardless of trapping moves and abilities (does not affect switching).
    /// </summary>
    public class RunAway : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the user flees using this ability.
        /// </summary>
        public string displayText;

        public RunAway(
            string displayText = "ability-runaway"
            )
            : base(effectType: AbilityEffectType.RunAway)
        {
            this.displayText = displayText;
        }
        public new RunAway Clone()
        {
            return new RunAway(displayText: displayText);
        }
    }

    /// <summary>
    /// Bypasses the given type immunities.
    /// </summary>
    public class Scrappy : AbilityEffect
    {
        /// <summary>
        /// The immunities that are bypassed.
        /// </summary>
        public List<string> bypassImmunities;

        public Scrappy(
            IEnumerable<string> bypassImmunities = null
            )
            : base(effectType: AbilityEffectType.Scrappy)
        {
            this.bypassImmunities = bypassImmunities == null ? new List<string>()
                : new List<string>(bypassImmunities);
        }
        public new Scrappy Clone()
        {
            return new Scrappy(bypassImmunities: bypassImmunities);
        }
    }

    /// <summary>
    /// Ends the effects of screens when sent into battle.
    /// </summary>
    public class ScreenCleaner : AbilityEffect
    {
        /// <summary>
        /// If true, screens end for the ally team.
        /// </summary>
        public bool affectAlly;
        /// <summary>
        /// If true, screens end for the opposing team.
        /// </summary>
        public bool affectOpposing;

        public ScreenCleaner(
            bool affectAlly = true, bool affectOpposing = true
            )
            : base(effectType: AbilityEffectType.ScreenCleaner)
        {
            this.affectAlly = affectAlly;
            this.affectOpposing = affectOpposing;
        }
        public new ScreenCleaner Clone()
        {
            return new ScreenCleaner(
                affectAlly: affectAlly, affectOpposing: affectOpposing
                );
        }
    }

    /// <summary>
    /// Scales the user's chances for additional move effects.
    /// </summary>
    public class SereneGrace : AbilityEffect
    {
        /// <summary>
        /// The factor by which to multiply chances for additional effects.
        /// </summary>
        public float chanceMultiplier;

        public SereneGrace(
            float chanceMultiplier = 2f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.SereneGrace, filters: filters)
        {
            this.chanceMultiplier = chanceMultiplier;
        }
        public new SereneGrace Clone()
        {
            return new SereneGrace(
                chanceMultiplier: chanceMultiplier,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Prevents opposing Pokémon from switching out or escaping. 
    /// </summary>
    public class ShadowTag : AbilityEffect
    {
        /// <summary>
        /// If true, this ability cannot trap others with the same ability.
        /// </summary>
        public bool immuneToSelf;
        /// <summary>
        /// If true, Pokémon are only trapped if they are adjacent to this Pokémon.
        /// </summary>
        public bool mustBeAdjacent;
        /// <summary>
        /// If true, this ability traps grounded Pokémon.
        /// </summary>
        public bool arenaTrap;

        /// <summary>
        /// The text displayed when the Pokémon is prevented from escaping.
        /// </summary>
        public string displayText;

        public ShadowTag(
            bool immuneToSelf = false, bool mustBeAdjacent = true, bool arenaTrap = false,
            string displayText = "ability-shadowtag",
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.ShadowTag, filters: filters)
        {
            this.immuneToSelf = immuneToSelf;
            this.mustBeAdjacent = mustBeAdjacent;
            this.arenaTrap = arenaTrap;
            this.displayText = displayText;
        }
        public new ShadowTag Clone()
        {
            return new ShadowTag(
                immuneToSelf: immuneToSelf, mustBeAdjacent: mustBeAdjacent, arenaTrap: arenaTrap,
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Removes additional move effects in favour of scaling move power.
    /// </summary>
    public class SheerForce : AbilityEffect
    {
        /// <summary>
        /// The power multiplier applied to moves.
        /// </summary>
        public float powerMultiplier;

        public SheerForce(
            float powerMultiplier = 1.3f
            )
            : base(effectType: AbilityEffectType.SheerForce)
        {
            this.powerMultiplier = powerMultiplier;
        }
        public new SheerForce Clone()
        {
            return new SheerForce(powerMultiplier: powerMultiplier);
        }
    }

    /// <summary>
    /// The user is unaffected by additional affects of moves.
    /// </summary>
    public class ShieldDust : AbilityEffect
    {
        public ShieldDust() : base(effectType: AbilityEffectType.ShieldDust)
        {
        }
        public new ShieldDust Clone()
        {
            return new ShieldDust();
        }
    }

    /// <summary>
    /// Blocks statuses depending on the user's form.
    /// </summary>
    public class ShieldsDown : AbilityEffect
    {
        public class MeteorForm
        {
            public List<string> forms;
            public List<Filter.Harvest> blockedStatuses;

            public MeteorForm(
                IEnumerable<string> forms,
                IEnumerable<Filter.Harvest> blockedStatuses
                )
            {
                this.forms = new List<string>(forms);
                this.blockedStatuses = new List<Filter.Harvest>();
                if (blockedStatuses != null)
                {
                    List<Filter.Harvest> preList = new List<Filter.Harvest>(blockedStatuses);
                    for (int i = 0; i < preList.Count; i++)
                    {
                        this.blockedStatuses.Add(preList[i].Clone());
                    }
                }
            }
            public MeteorForm Clone()
            {
                return new MeteorForm(
                    forms: forms,
                    blockedStatuses: blockedStatuses
                    );
            }
            public bool IsAForm(Main.Pokemon.Pokemon pokemon)
            {
                for (int i = 0; i < forms.Count; i++)
                {
                    if (pokemon.pokemonID == forms[i])
                    {
                        return true;
                    }
                }
                return false;
            }
            public bool IsStatusBlocked(StatusPKData statusData)
            {
                for (int i = 0; i < blockedStatuses.Count; i++)
                {
                    if (blockedStatuses[i].DoesStatusSatisfy(statusData))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public List<MeteorForm> meteorForms;

        public ShieldsDown(
            IEnumerable<MeteorForm> meteorForms = null
            )
            : base(effectType: AbilityEffectType.ShieldsDown)
        {
            this.meteorForms = new List<MeteorForm>();
            if (meteorForms != null)
            {
                List<MeteorForm> preList = new List<MeteorForm>();
                for (int i = 0; i < preList.Count; i++)
                {
                    this.meteorForms.Add(preList[i].Clone());
                }
            }
        }
        public new ShieldsDown Clone()
        {
            return new ShieldsDown(
                meteorForms: meteorForms
                );
        }
    }

    /// <summary>
    /// Reverses the user's stat stage changes.
    /// </summary>
    public class Simple : AbilityEffect
    {
        /// <summary>
        /// The amount by which stat stages are scaled.
        /// </summary>
        public int statModScale;

        public Simple(
            int statModScale = 2
            )
            : base(effectType: AbilityEffectType.Simple)
        {
            this.statModScale = statModScale;
        }
        public new Simple Clone()
        {
            return new Simple(statModScale: statModScale);
        }
    }

    /// <summary>
    /// Maximizes the number of hits for the user's <seealso cref="Moves.FuryAttack"/> moves.
    /// </summary>
    public class SkillLink : AbilityEffect
    {
        public SkillLink() : base(effectType: AbilityEffectType.SkillLink)
        {
        }
        public new SkillLink Clone()
        {
            return new SkillLink();
        }
    }

    /// <summary>
    /// Scales the user's stats for the first X turns of the battle.
    /// </summary>
    public class SlowStart : AbilityEffect
    {
        /// <summary>
        /// The stats to scale.
        /// </summary>
        public General.StatScale statScale;
        /// <summary>
        /// The amount of turns this effect stays active for.
        /// </summary>
        public int turnsActive;
        /// <summary>
        /// The text displayed when the user enters battle.
        /// </summary>
        public string displayText;

        public SlowStart(
            General.StatScale statScale,
            int turnsActive = 5,
            string displayText = "ability-slowstart",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.SlowStart, filters: filters)
        {
            this.statScale = statScale.Clone();
            this.turnsActive = turnsActive;
            this.displayText = displayText;
        }
        public new SlowStart Clone()
        {
            return new SlowStart(
                statScale: statScale,
                turnsActive: turnsActive,
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Scales the damage of critical hits by the user's moves.
    /// </summary>
    public class Sniper : AbilityEffect
    {
        /// <summary>
        /// The boost to apply to critical damage.
        /// </summary>
        public float criticalBoost;

        public Sniper(
            float criticalBoost = 1.5f,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Sniper, filters: filters)
        {
            this.criticalBoost = criticalBoost;
        }
        public new Sniper Clone()
        {
            return new Sniper(
                criticalBoost: criticalBoost,
                filters: filters);
        }
    }

    /// <summary>
    /// Scales super-effective damage dealt to the user. 
    /// </summary>
    public class SolidRock : AbilityEffect
    {
        /// <summary>
        /// Damage scaled for super-effective attacks.
        /// </summary>
        public float superEffectiveModifier;
        /// <summary>
        /// Damage scaled for not-very effective attacks.
        /// </summary>
        public float notVeryEffectiveModifier;

        public SolidRock(
            float superEffectiveModifier = 0.75f,
            float notVeryEffectiveModifier = 1f,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.SolidRock, filters: filters)
        {
            this.superEffectiveModifier = superEffectiveModifier;
            this.notVeryEffectiveModifier = notVeryEffectiveModifier;
        }
        public new SolidRock Clone()
        {
            return new SolidRock(
                superEffectiveModifier: superEffectiveModifier,
                notVeryEffectiveModifier: notVeryEffectiveModifier,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Modifies stats if an opposing Pokémon faints.
    /// </summary>
    public class SoulHeart : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        public SoulHeart(General.StatStageMod statStageMod)
            : base(effectType: AbilityEffectType.SoulHeart)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new SoulHeart Clone()
        {
            return new SoulHeart(statStageMod: statStageMod);
        }
    }

    /// <summary>
    /// Modifies the user's stats at the end of each turn.
    /// </summary>
    public class SpeedBoost : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        public SpeedBoost(General.StatStageMod statStageMod)
            : base(effectType: AbilityEffectType.SpeedBoost)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new SpeedBoost Clone()
        {
            return new SpeedBoost(statStageMod: statStageMod);
        }
    }

    /// <summary>
    /// Increases move power against targets that have switched in this turn.
    /// </summary>
    public class Stakeout : AbilityEffect
    {
        /// <summary>
        /// The power multiplier applied to moves.
        /// </summary>
        public float powerMultiplier;

        public Stakeout(float powerMultiplier = 2f) : base(effectType: AbilityEffectType.Stakeout)
        {
            this.powerMultiplier = powerMultiplier;
        }
        public new Stakeout Clone()
        {
            return new Stakeout(powerMultiplier: powerMultiplier);
        }
    }

    /// <summary>
    /// The user always moves last within its priority bracket.
    /// </summary>
    public class Stall : AbilityEffect
    {
        public Stall(
            IEnumerable<Filter.FilterEffect> filters = null
            ) : base(effectType: AbilityEffectType.Stall, filters: filters)
        {
        }
        public new Stall Clone()
        {
            return new Stall(filters: filters);
        }
    }

    /// <summary>
    /// Changes the Pokémon's form right before moves are used, depending on the type of move used.
    /// </summary>
    public class StanceChange : AbilityEffect
    {
        public class Transformation
        {
            public Filter.MoveCheck moveCheck;
            public General.FormTransformation transformation;
            public string displayText;

            public Transformation(
                Filter.MoveCheck moveCheck,
                General.FormTransformation transformation,
                string displayText = null)
            {
                this.moveCheck = moveCheck.Clone();
                this.transformation = transformation.Clone();
                this.displayText = displayText;
            }
            public Transformation Clone()
            {
                return new Transformation(
                    moveCheck: moveCheck,
                    transformation: transformation,
                    displayText: displayText
                    );
            }
        }

        /// <summary>
        /// The list of transformations.
        /// </summary>
        public List<Transformation> transformations;

        public StanceChange(
            IEnumerable<Transformation> transformations = null
            )
            : base(effectType: AbilityEffectType.StanceChange)
        {
            this.transformations = new List<Transformation>();
            if (transformations != null)
            {
                List<Transformation> preList = new List<Transformation>(transformations);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.transformations.Add(preList[i].Clone());
                }
            }
        }
        public new StanceChange Clone()
        {
            return new StanceChange(
                transformations: transformations
                );
        }
    }

    /// <summary>
    /// Modifies stats if the user when flinched.
    /// </summary>
    public class Steadfast : AbilityEffect
    {
        /// <summary>
        /// The stat stage changes applied.
        /// </summary>
        public General.StatStageMod statStageMod;

        public Steadfast(
            General.StatStageMod statStageMod,
            IEnumerable<Filter.FilterEffect> filters = null)
            : base(effectType: AbilityEffectType.Steadfast, filters: filters)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new Steadfast Clone()
        {
            return new Steadfast(
                statStageMod: statStageMod,
                filters: filters);
        }
    }

    /// <summary>
    /// The user cannot lose their held item.
    /// </summary>
    public class StickyHold : AbilityEffect
    {
        public StickyHold(
            IEnumerable<Filter.FilterEffect> filters = null
            ) : base(effectType: AbilityEffectType.StickyHold, filters: filters)
        {
        }
        public new StickyHold Clone()
        {
            return new StickyHold(filters: filters);
        }
    }

    /// <summary>
    /// Prevents this pokemon from fainting from a direct hit at a certain HP threshold.
    /// </summary>
    public class Sturdy : AbilityEffect
    {
        /// <summary>
        /// The HP % the user's HP must at least be at to trigger this effect.
        /// </summary>
        public float hpThreshold;

        /// <summary>
        /// The text displayed when the user hangs on.
        /// </summary>
        public string displayText;

        public Sturdy(
            float hpThreshold = 1f,
            string displayText = "ability-sturdy",

            float chance = 1f,
            IEnumerable<Filter.FilterEffect> filters = null
            ) : base(effectType: AbilityEffectType.Sturdy, chance: chance, filters: filters)
        {
            this.displayText = displayText;
            this.hpThreshold = hpThreshold;
        }
        public new Sturdy Clone()
        {
            return new Sturdy(
                hpThreshold: hpThreshold,
                displayText: displayText,
                chance: chance, filters: filters);
        }
    }

    /// <summary>
    /// The user cannot be forced out.
    /// </summary>
    public class SuctionCups : AbilityEffect
    {
        public SuctionCups(
            IEnumerable<Filter.FilterEffect> filters = null
            ) : base(effectType: AbilityEffectType.SuctionCups, filters: filters)
        {
        }
        public new SuctionCups Clone()
        {
            return new SuctionCups(filters: filters);
        }
    }

    /// <summary>
    /// Increases the base critical hit rate for this Pokémon's moves.
    /// </summary>
    public class SuperLuck : AbilityEffect
    {
        /// <summary>
        /// If true, this move always results in critical hits.
        /// </summary>
        public bool alwaysCritical;
        /// <summary>
        /// The critical level added on to this move.
        /// </summary>
        public int criticalBoost;

        public SuperLuck(
            bool alwaysCritical = false, int criticalBoost = 1,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.SuperLuck, filters: filters)
        {
            this.alwaysCritical = alwaysCritical;
            this.criticalBoost = criticalBoost;
        }
        public new SuperLuck Clone()
        {
            return new SuperLuck(
                alwaysCritical: alwaysCritical,
                criticalBoost: criticalBoost,
                filters: filters);
        }
    }

    /// <summary>
    /// Scales stats based on the current battle conditions (if the user would be affected).
    /// </summary>
    public class SwiftSwim : AbilityEffect
    {
        public class SwiftSwimCondition
        {
            public List<string> conditions;
            public General.StatScale statScale;
            /// <summary>
            /// If true, the stat scales apply to all ally Pokémon on the field.
            /// </summary>
            public bool flowerGift;

            public SwiftSwimCondition(
                IEnumerable<string> conditions,
                General.StatScale statScale,
                bool flowerGift = false
                )
            {
                this.conditions = new List<string>(conditions);
                this.statScale = statScale.Clone();
                this.flowerGift = flowerGift;
            }
            public SwiftSwimCondition Clone()
            {
                return new SwiftSwimCondition(
                    conditions: conditions,
                    statScale: statScale,
                    flowerGift: flowerGift
                    );
            }
        }
        public List<SwiftSwimCondition> conditions;

        public SwiftSwim(
            IEnumerable<SwiftSwimCondition> conditions
            )
            : base(effectType: AbilityEffectType.SwiftSwim)
        {
            this.conditions = new List<SwiftSwimCondition>();
            if (conditions != null)
            {
                List<SwiftSwimCondition> preList = new List<SwiftSwimCondition>(conditions);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.conditions.Add(preList[i].Clone());
                }
            }
        }
    }

    /// <summary>
    /// The user passes its item to an ally that has used up an item.
    /// </summary>
    public class Symbiosis : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the user's item is passed.
        /// </summary>
        public string displayText;

        public Symbiosis(
            string displayText = "ability-symbiosis",
            IEnumerable<Filter.FilterEffect> filters = null
            ) : base(effectType: AbilityEffectType.Symbiosis, filters: filters)
        {
            this.displayText = displayText;
        }
        public new Symbiosis Clone()
        {
            return new Symbiosis(
                displayText: displayText,
                filters: filters);
        }
    }

    /// <summary>
    /// Reflects status conditions back at the user.
    /// </summary>
    public class Synchronize : AbilityEffect
    {
        public Filter.Harvest conditionCheck;

        public Synchronize(
            Filter.Harvest conditionCheck,
            IEnumerable<string> statuses = null
            )
            : base(effectType: AbilityEffectType.Synchronize)
        {
            this.conditionCheck = conditionCheck.Clone();
        }
        public new Synchronize Clone()
        {
            return new Synchronize(conditionCheck: conditionCheck);
        }
    }

    /// <summary>
    /// Increases move power if the base power of the move is at or below a specific threshold.
    /// </summary>
    public class Technician : AbilityEffect
    {
        /// <summary>
        /// Base power threshold (after move multipliers).
        /// </summary>
        public int threshold;
        /// <summary>
        /// The power multiplier applied to moves.
        /// </summary>
        public float powerMultiplier;

        public Technician(
            int threshold = 60, float powerMultiplier = 1.5f
            )
            : base(effectType: AbilityEffectType.Technician)
        {
            this.threshold = threshold;
            this.powerMultiplier = powerMultiplier;
        }
        public new Technician Clone()
        {
            return new Technician(threshold: threshold, powerMultiplier: powerMultiplier);
        }
    }

    /// <summary>
    /// The user is immune to ally moves.
    /// </summary>
    public class Telepathy : AbilityEffect
    {

        public Telepathy(
            IEnumerable<Filter.FilterEffect> filters = null)
            : base(effectType: AbilityEffectType.Telepathy, filters: filters)
        {
        }
        public new Telepathy Clone()
        {
            return new Telepathy(filters: filters);
        }
    }

    /// <summary>
    /// Scales super-effective damage dealt to the user. 
    /// </summary>
    public class TintedLens : AbilityEffect
    {
        /// <summary>
        /// Damage scaled for super-effective attacks.
        /// </summary>
        public float neuroforceModifier;
        /// <summary>
        /// Damage scaled for not-very effective attacks.
        /// </summary>
        public float notVeryEffectiveModifier;

        public TintedLens(
            float neuroforceModifier = 0.75f,
            float notVeryEffectiveModifier = 1f,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.TintedLens, filters: filters)
        {
            this.neuroforceModifier = neuroforceModifier;
            this.notVeryEffectiveModifier = notVeryEffectiveModifier;
        }
        public new TintedLens Clone()
        {
            return new TintedLens(
                neuroforceModifier: neuroforceModifier,
                notVeryEffectiveModifier: notVeryEffectiveModifier,
                filters: filters
                );
        }
    }

    /// <summary>
    /// The user copies an opponent's ability.
    /// </summary>
    public class Trace : AbilityEffect
    {
        /// <summary>
        /// The text displayed when the user traces a target's ability.
        /// </summary>
        public string displayText;

        public Trace(
            string displayText = "ability-trace",

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Trace, filters: filters)
        {
            this.displayText = displayText;

        }
        public new Trace Clone()
        {
            return new Trace(
                displayText: displayText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// The user does not attack every second turn.
    /// </summary>
    public class Truant : AbilityEffect
    {
        /// <summary>
        /// The turns waiting between attacking.
        /// </summary>
        public int turnsWaiting;
        /// <summary>
        /// The text that displays when the Pokémon enters battle.
        /// </summary>
        public string displayText;

        public Truant(
            int turnsWaiting = 1,
            string displayText = "ability-truant"
            )
            : base(effectType: AbilityEffectType.Truant)
        {
            this.turnsWaiting = turnsWaiting;
            this.displayText = displayText;
        }
        public new Truant Clone()
        {
            return new Truant(turnsWaiting: turnsWaiting, displayText: displayText);
        }
    }

    /// <summary>
    /// Ignores the stat changes of the target when attacking.
    /// </summary>
    public class Unaware : AbilityEffect
    {
        /// <summary>
        /// The stat changes of the target Pokémon that are ignored when the user is attacking.
        /// </summary>
        public HashSet<PokemonStats> targetStatsIgnored;
        /// <summary>
        /// The stat changes of the attacking Pokémon that are ignored when the user is being attacked.
        /// </summary>
        public HashSet<PokemonStats> attackerStatsIgnored;

        public Unaware(
            IEnumerable<PokemonStats> targetStatsIgnored = null,
            IEnumerable<PokemonStats> attackerStatsIgnored = null
            )
            : base(effectType: AbilityEffectType.Unaware)
        {
            this.targetStatsIgnored = targetStatsIgnored != null
                ? new HashSet<PokemonStats>(targetStatsIgnored)
                : new HashSet<PokemonStats>(GameSettings.btlPkmnStats);
            this.attackerStatsIgnored = attackerStatsIgnored != null
                ? new HashSet<PokemonStats>(attackerStatsIgnored)
                : new HashSet<PokemonStats>(GameSettings.btlPkmnStats);
        }
        public new Unaware Clone()
        {
            return new Unaware(
                targetStatsIgnored: targetStatsIgnored,
                attackerStatsIgnored: attackerStatsIgnored
                );
        }
    }

    /// <summary>
    /// Scales the user's stats once its held item is lost.
    /// </summary>
    public class Unburden : AbilityEffect
    {
        /// <summary>
        /// The stats to scale.
        /// </summary>
        public General.StatScale statScale;

        public Unburden(
            General.StatScale statScale,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.Unburden, filters: filters)
        {
            this.statScale = statScale.Clone();
        }
        public new Unburden Clone()
        {
            return new Unburden(
                statScale: statScale,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Allows the user's attack to bypass protection moves.
    /// </summary>
    public class UnseenFist : AbilityEffect
    {
        public bool ignoreProtect;
        public bool ignoreCraftyShield;
        public bool ignoreMaxGuard;

        public UnseenFist(
            bool ignoreProtect = true,
            bool ignoreCraftyShield = true,
            bool ignoreMaxGuard = true,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.UnseenFist, filters: filters)
        {
            this.ignoreProtect = ignoreProtect;
            this.ignoreCraftyShield = ignoreCraftyShield;
            this.ignoreMaxGuard = ignoreMaxGuard;
        }
        public new UnseenFist Clone()
        {
            return new UnseenFist(
                ignoreProtect: ignoreProtect,
                ignoreCraftyShield: ignoreCraftyShield,
                ignoreMaxGuard: ignoreMaxGuard,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Provides an immunity to the given types, and extra effects can be specified.
    /// </summary>
    public class VoltAbsorb : AbilityEffect
    {
        public class VoltAbsorbCondition
        {
            /// <summary>
            /// The move types the ability protects against.
            /// </summary>
            public List<string> moveTypes;

            /// <summary>
            /// The % of HP recovered when hit by a move in <seealso cref="moveTypes"/>.
            /// </summary>
            public float absorbPercent;
            /// <summary>
            /// If positive, this boost is applied to the given move-types as long as the user
            /// remains in battle.
            /// </summary>
            public float flashFireBoost;
            /// <summary>
            /// The stat stage applied when hit by a move in <seealso cref="moveTypes"/>.
            /// </summary>
            public General.StatStageMod motorDrive;

            public VoltAbsorbCondition(
                IEnumerable<string> moveTypes = null,
                float absorbPercent = 0f, float flashFireBoost = 0f,
                General.StatStageMod motorDrive = null
                )
            {
                this.moveTypes = moveTypes == null ? new List<string>() : new List<string>(moveTypes);
                this.absorbPercent = absorbPercent;
                this.flashFireBoost = flashFireBoost;
                this.motorDrive = motorDrive == null ? null : motorDrive.Clone();
            }
            public VoltAbsorbCondition Clone()
            {
                return new VoltAbsorbCondition(
                    moveTypes: moveTypes,
                    absorbPercent: absorbPercent, flashFireBoost: flashFireBoost,
                    motorDrive: motorDrive
                    );
            }
        }

        /// <summary>
        /// The volt absorb conditions
        /// </summary>
        public List<VoltAbsorbCondition> conditions;

        /// <summary>
        /// The text displayed when the move is blocked.
        /// </summary>
        public string displayText;

        public VoltAbsorb(
            IEnumerable<VoltAbsorbCondition> conditions = null,
            string displayText = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.VoltAbsorb, filters: filters)
        {
            this.conditions = new List<VoltAbsorbCondition>();
            if (conditions != null)
            {
                List<VoltAbsorbCondition> preList = new List<VoltAbsorbCondition>(conditions);
                for (int i = 0; i < preList.Count; i++)
                {
                    this.conditions.Add(preList[i].Clone());
                }
            }
            this.displayText = displayText;
        }
        public new VoltAbsorb Clone()
        {
            return new VoltAbsorb(
                conditions: conditions,
                displayText: displayText,

                filters: filters
                );
        }
    }

    /// <summary>
    /// Prompts the Pokémon to switch out if it's HP drops below a certain threshold.
    /// </summary>
    public class WimpOut : AbilityEffect
    {
        /// <summary>
        /// The HP threshold needed to fall below to allow doe switching out.
        /// </summary>
        public float hpThreshold;

        public WimpOut(
            float hpThreshold = 0.5f
            )
            : base(effectType: AbilityEffectType.WimpOut)
        {
            this.hpThreshold = hpThreshold;
        }
        public new WimpOut Clone()
        {
            return new WimpOut(
                hpThreshold: hpThreshold
                );
        }
    }

    /// <summary>
    /// Causes immunity to moves unless they have certain effectiveness.
    /// </summary>
    public class WonderGuard : AbilityEffect
    {
        /// <summary>
        /// If true, attacks can hit if they hit neutrally.
        /// </summary>
        public bool affectNeutral;
        /// <summary>
        /// If true, attacks can hit if they hit super-effectively.
        /// </summary>
        public bool affectSuperEffective;
        /// <summary>
        /// If true, attacks can hit if they hit not-very effectively.
        /// </summary>
        public bool affectNotVeryEffective;

        public WonderGuard(
            bool affectNeutral = false,
            bool affectSuperEffective = true,
            bool affectNotVeryEffective = false,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.WonderGuard, filters: filters)
        {
            this.affectNeutral = affectNeutral;
            this.affectSuperEffective = affectSuperEffective;
            this.affectNotVeryEffective = affectNotVeryEffective;
        }
        public new WonderGuard Clone()
        {
            return new WonderGuard(
                affectNeutral: affectNeutral,
                affectSuperEffective: affectSuperEffective,
                affectNotVeryEffective: affectNotVeryEffective,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Changes the accuracy of status moves used against this Pokémon.
    /// </summary>
    public class WonderSkin : AbilityEffect
    {
        public enum AccuracyMode
        {
            /// <summary>
            /// Forces the accuracy of moves to be equal to <seealso cref="accuracyValue"/>.
            /// </summary>
            Set,
            /// <summary>
            /// Scales the accuracy of moves by <seealso cref="accuracyValue"/>.
            /// </summary>
            Multiplier
        }
        public AccuracyMode mode;

        /// <summary>
        /// The accuracy value.
        /// </summary>
        public float accuracyValue;

        public WonderSkin(
            AccuracyMode mode = AccuracyMode.Set,
            float accuracyValue = 0.5f,
            bool steelySpirit = false,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: AbilityEffectType.WonderSkin, filters: filters)
        {
            this.mode = mode;
            this.accuracyValue = accuracyValue;
        }
        public new WonderSkin Clone()
        {
            return new WonderSkin(
                mode: mode,
                accuracyValue: accuracyValue,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Changes the Pokemon's form depending on its HP.
    /// </summary>
    public class ZenMode : AbilityEffect
    {
        /// <summary>
        /// The transformation that occurs.
        /// </summary>
        public General.FormTransformation transformation;
        /// <summary>
        /// The HP threshold check for the form change to trigger.
        /// </summary>
        public float hpThreshold;
        /// <summary>
        /// If true, the user's HP must be below the threshold. If false, its HP must be above the threshold.
        /// </summary>
        public bool checkBelow;
        /// <summary>
        /// Accompanying text after the form change.
        /// </summary>
        public string displayText;

        public ZenMode(
            General.FormTransformation transformation,
            float hpThreshold = 0.5f, bool checkBelow = true,
            string displayText = "pokemon-changeform"
            )
            : base(effectType: AbilityEffectType.ZenMode)
        {
            this.displayText = displayText;
            this.hpThreshold = hpThreshold;
            this.checkBelow = checkBelow;
            this.transformation = transformation.Clone();
        }
        public new ZenMode Clone()
        {
            return new ZenMode(
                transformation: transformation,
                hpThreshold: hpThreshold, checkBelow: checkBelow,
                displayText: displayText
                );
        }
    }
}
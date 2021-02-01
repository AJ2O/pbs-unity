using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases.Effects.PokemonStatuses
{
    public class PokemonSE
    {
        /// <summary>
        /// The type of Pokémon status effect it is.
        /// </summary>
        public PokemonSEType effectType;
        /// <summary>
        /// The time that this effect applies itself.
        /// </summary>
        public PokemonSETiming timing;
        /// <summary>
        /// If true, this effect can stack where applicable.
        /// </summary>
        public bool isStackable;

        /// <summary>
        /// Additional restrictions on how the effect is applied.
        /// </summary>
        public List<Filter.FilterEffect> filters;

        public PokemonSE(
            PokemonSEType effectType,
            PokemonSETiming timing = PokemonSETiming.Unique,
            bool isStackable = false,
            IEnumerable<Filter.FilterEffect> filters = null
            )
        {
            this.effectType = effectType;
            this.timing = timing;
            this.isStackable = isStackable;
            this.filters = new List<Filter.FilterEffect>();
            if (filters != null)
            {
                List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                for (int i = 0; i < tempFilters.Count; i++)
                {
                    this.filters.Add(tempFilters[i].Clone());
                }
            }
        }
        public PokemonSE Clone()
        {
            return
                this is Bound ? (this as Bound).Clone()
                : this is Burn ? (this as Burn).Clone()
                : this is DefenseCurl ? (this as DefenseCurl).Clone()
                : this is Disable ? (this as Disable).Clone()
                : this is Electrify ? (this as Electrify).Clone()
                : this is Flinch ? (this as Flinch).Clone()
                : this is Freeze ? (this as Freeze).Clone()
                : this is HPLoss ? (this as HPLoss).Clone()
                : this is Identification ? (this as Identification).Clone()
                : this is Imprison ? (this as Imprison).Clone()
                : this is Infatuation ? (this as Infatuation).Clone()
                : this is NonVolatile ? (this as NonVolatile).Clone()
                : this is Octolock ? (this as Octolock).Clone()
                : this is Paralysis ? (this as Paralysis).Clone()
                : this is PerishSong ? (this as PerishSong).Clone()
                : this is Sleep ? (this as Sleep).Clone()
                : this is TarShot ? (this as TarShot).Clone()
                : this is TypeImmunity ? (this as TypeImmunity).Clone()
                : this is Volatile ?
                    this is MoveLimiting ? (this as MoveLimiting).Clone()
                    : this is Embargo ? (this as Embargo).Clone()
                    : (this as Volatile).Clone()
                : this is Yawn ? (this as Yawn).Clone()
                : new PokemonSE(
                    effectType: effectType,
                    timing: timing,
                    isStackable: isStackable,
                    filters: filters
                    );
        }
    }

    /// <summary>
    /// Flags this status as a bound status, preventing this Pokémon from switching out as long as its
    /// trapper is still in battle.
    /// </summary>
    public class Bound : PokemonSE
    {
        /// <summary>
        /// The priority value of this status condition. Low priority bound statuses can't overwrite 
        /// higher priority ones.
        /// </summary>
        public int priority;

        /// <summary>
        /// The text that displays when this status condition negates a lower priority one.
        /// </summary>
        public string negateTextID;

        public Bound(
            int priority = 1,
            string negateTextID = null
            )
            : base(PokemonSEType.Bound)
        {
            this.priority = priority;
            this.negateTextID = negateTextID;
        }
        public new Bound Clone()
        {
            return new Bound(priority: priority, negateTextID: negateTextID);
        }
    }

    /// <summary>
    /// Scales the user's stats.
    /// </summary>
    public class Burn : PokemonSE
    {
        /// <summary>
        /// The stats to scale.
        /// </summary>
        public General.StatScale statScale;

        public Burn(
            General.StatScale statScale
            )
            : base(effectType: PokemonSEType.Burn)
        {
            this.statScale = statScale.Clone();
        }
        public new Burn Clone()
        {
            return new Burn(
                statScale: statScale
                );
        }
    }

    /// <summary>
    /// Scales the damage of the user's rolling attacks.
    /// </summary>
    public class DefenseCurl : PokemonSE
    {
        /// <summary>
        /// The factor by which to scale damage.
        /// </summary>
        public float damageScale;

        public DefenseCurl(
            float damageScale = 2f
            )
            : base(effectType: PokemonSEType.DefenseCurl)
        {
            this.damageScale = damageScale;
        }
        public new DefenseCurl Clone()
        {
            return new DefenseCurl(damageScale: damageScale);
        }
    }

    /// <summary>
    /// Prevents this Pokemon from using its last used move for a few turns.
    /// </summary>
    public class Disable : MoveLimiting
    {
        public Disable(
            string startText = "status-disable-start", string endText = "status-disable-end",
            string alreadyText = "status-disable-already", string failText = "status-disable-fail",
            string attemptText = "status-disable-attempt",
            General.DefaultTurns defaultTurns = null
            )
            : base(
                    effectType: PokemonSEType.Disable,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    )
        {

        }
        public new Disable Clone()
        {
            return new Disable(
                startText: startText, endText: endText,
                alreadyText: alreadyText, failText: failText,
                attemptText: attemptText,
                defaultTurns: defaultTurns
                );
        }
    }

    /// <summary>
    /// Forces moves used by this Pokémon to be a certain type.
    /// </summary>
    public class Electrify : PokemonSE
    {
        /// <summary>
        /// The type that this Pokémon's moves are forced to be.
        /// </summary>
        public string moveType;
        /// <summary>
        /// The text displayed when this effect is successful.
        /// </summary>
        public string displayText;

        public Electrify(
            string moveType = "electric",
            string displayText = "status-electrify")
            : base(effectType: PokemonSEType.Electrify)
        {
            this.moveType = moveType;
            this.displayText = displayText;
        }
        public new Electrify Clone()
        {
            return new Electrify(moveType: moveType, displayText: displayText);
        }
    }

    /// <summary>
    /// Prevents this Pokemon from using any items for a few turns.
    /// </summary>
    public class Embargo : Volatile
    {
        /// <summary>
        /// The text displayed when an item is blocked from being used.
        /// </summary>
        public string attemptText;

        public Embargo(
            string startText = "status-embargo-start", string endText = "status-embargo-end",
            string alreadyText = "status-embargo-already", string failText = "status-embargo-fail",
            string attemptText = "status-embargo-attempt",
            General.DefaultTurns defaultTurns = null
            )
            : base(
                    effectType: PokemonSEType.Embargo,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    defaultTurns: defaultTurns
                    )
        {
            this.attemptText = attemptText;
        }
        public new Embargo Clone()
        {
            return new Embargo(
                startText: startText, endText: endText,
                alreadyText: alreadyText, failText: failText,
                attemptText: attemptText,
                defaultTurns: defaultTurns
                );
        }
    }

    /// <summary>
    /// Prevents this Pokemon from any other move but its encored move for a few turns.
    /// </summary>
    public class Encore : MoveLimiting
    {
        public Encore(
            string startText = "status-encore-start", string endText = "status-encore-end",
            string alreadyText = "status-encore-already", string failText = "status-encore-fail",
            string attemptText = "status-encore-attempt",
            General.DefaultTurns defaultTurns = null
            )
            : base(
                    effectType: PokemonSEType.Encore,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    )
        {

        }
        public new Encore Clone()
        {
            return new Encore(
                startText: startText, endText: endText,
                alreadyText: alreadyText, failText: failText,
                attemptText: attemptText,
                defaultTurns: defaultTurns
                );
        }
    }

    /// <summary>
    /// Prevents the Pokémon from using their move during the rest of the turn.
    /// </summary>
    public class Flinch : PokemonSE
    {
        /// <summary>
        /// The text that displays when the Pokémon flinches.
        /// </summary>
        public string flinchText;

        public Flinch(string displayText = "status-flinch") : base(effectType: PokemonSEType.Flinch)
        {
            flinchText = displayText;
        }
        public new Flinch Clone()
        {
            return new Flinch(displayText: flinchText);
        }
    }

    /// <summary>
    /// May prevent this Pokémon from attacking.
    /// </summary>
    public class Freeze : PokemonSE
    {
        /// <summary>
        /// The chance of full paralysis when attacking.
        /// </summary>
        public float thawChance;
        /// <summary>
        /// The text displayed when frozen.
        /// </summary>
        public string displayText;

        /// <summary>
        /// The move types that thaw
        /// </summary>
        public List<string> thawMoveTypes;

        public Freeze(
            float chance = 0.2f,
            string displayText = "status-freeze",
            IEnumerable<string> thawMoveTypes = null
            )
            : base(effectType: PokemonSEType.Freeze)
        {
            thawChance = chance;
            this.displayText = displayText;
            this.thawMoveTypes = thawMoveTypes == null ? new List<string>() : new List<string>(thawMoveTypes);
        }
        public new Freeze Clone()
        {
            return new Freeze(
                chance: thawChance,
                displayText: displayText,
                thawMoveTypes: thawMoveTypes);
        }
    }

    /// <summary>
    /// Prevents this Pokemon from using healing moves for a few turns.
    /// </summary>
    public class HealBlock : MoveLimiting
    {
        public HealBlock(
            string startText = "status-healblock-start", string endText = "status-healblock-end",
            string alreadyText = "status-healblock-already", string failText = "status-healblock-fail",
            string attemptText = "status-healblock-attempt",
            General.DefaultTurns defaultTurns = null
            )
            : base(
                    effectType: PokemonSEType.HealBlock,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    )
        {

        }
        public new HealBlock Clone()
        {
            return new HealBlock(
                startText: startText, endText: endText,
                alreadyText: alreadyText, failText: failText,
                attemptText: attemptText,
                defaultTurns: defaultTurns
                );
        }
    }

    /// <summary>
    /// Damages affected Pokémon by a percentage of their maximum HP.
    /// </summary>
    public class HPLoss : PokemonSE
    {
        /// <summary>
        /// The text to display if Pokémon lose HP.
        /// </summary>
        public string displayText;

        /// <summary>
        /// The percentage of HP lost from this effect.
        /// </summary>
        public float hpLossPercent;
        /// <summary>
        /// If true, the HP Loss stacks in a similar effect every turn like Toxic.
        /// </summary>
        public bool toxicStack;

        public HPLoss(
            string displayText = null,
            float hpLossPercent = 1f / 16,
            bool toxicStack = false,

            PokemonSETiming timing = PokemonSETiming.EndOfTurn,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: PokemonSEType.HPLoss, timing: timing, filters: filters)
        {
            this.displayText = displayText;
            this.hpLossPercent = hpLossPercent;
            this.toxicStack = toxicStack;
        }
        public new HPLoss Clone()
        {
            return new HPLoss(
                displayText: displayText, hpLossPercent: hpLossPercent, toxicStack: toxicStack,

                timing: timing, filters: filters
                );
        }
    }

    /// <summary>
    /// Removes specified type immunities from the Pokémon. Also ignores the Pokémon's positive
    /// evasion stat changes.
    /// </summary>
    public class Identification : PokemonSE
    {
        /// <summary>
        /// The text that displays when the target is identified.
        /// </summary>
        public string identifiedText;
        /// <summary>
        /// The text that displays if this Pokémon was already identified.
        /// </summary>
        public string alreadyText;
        /// <summary>
        /// The types that have their immunities removed.
        /// </summary>
        public List<string> types;

        public Identification(
            string identifiedText = "status-identification", string alreadyText = "status-identification-already",
            IEnumerable<string> types = null
            ) : base(effectType: PokemonSEType.Identified)
        {
            this.identifiedText = identifiedText;
            this.alreadyText = alreadyText;
            this.types = types == null ? new List<string>() : new List<string>(types);
        }
        public new Identification Clone()
        {
            return new Identification(
                identifiedText: identifiedText, alreadyText: alreadyText,
                types: types
                );
        }
    }

    /// <summary>
    /// While this status is active, opposing Pokémon cannot use any move that this pokemon knows.
    /// </summary>
    public class Imprison : PokemonSE
    {
        /// <summary>
        /// The text that displays when the pokemon starts imprison.
        /// </summary>
        public string startText;
        /// <summary>
        /// The text that displays when imprison blocks the use of a move.
        /// </summary>
        public string negateText;
        /// <summary>
        /// The text that displays to notify the player that a move can't be used due to Imprison.
        /// </summary>
        public string chooseText;

        public Imprison(
            string startText = "status-imprison",
            string negateText = "status-imprison-negate",
            string chooseText = "status-imprison-choose"
            )
            : base(effectType: PokemonSEType.Imprison)
        {
            this.startText = startText;
            this.negateText = negateText;
            this.chooseText = chooseText;
        }
        public new Imprison Clone()
        {
            return new Imprison(startText: startText, negateText: negateText, chooseText: chooseText);
        }
    }

    /// <summary>
    /// The Pokémon can't attack part of the time, even against the Pokémon it is infatuated by.
    /// </summary>
    public class Infatuation : PokemonSE
    {
        /// <summary>
        /// The Pokémon causing infatuation.
        /// </summary>
        public string infatuator;

        /// <summary>
        /// Text displayed when the Pokémon becomes infatuated.
        /// </summary>
        public string startText;
        /// <summary>
        /// Text displayed when the Pokémon is longer infatuated.
        /// </summary>
        public string endText;
        /// <summary>
        /// Text displayed when the Pokémon is about to make a move.
        /// </summary>
        public string moveText;
        /// <summary>
        /// Text displayed when the Pokémon fails to make a move.
        /// </summary>
        public string moveFailText;
        /// <summary>
        /// Text displayed when the Pokémon fails to become infatuated.
        /// </summary>
        public string failText;
        /// <summary>
        /// Text displayed when the Pokémon is already infatuated.
        /// </summary>
        public string alreadyText;

        /// <summary>
        /// The chance of the move failing.
        /// </summary>
        public float moveFailChance;

        public Infatuation(
            string infatuator = null,

            string startText = "status-infatuation",
            string endText = "status-infatuation-end",
            string moveText = "status-infatuation-move", string moveFailText = "status-infatuation-movefail",
            string failText = "status-infatuation-fail", string alreadyText = "status-infatuation-already",
            float moveFailChance = 0.5f
            )
            : base(effectType: PokemonSEType.Infatuation)
        {
            this.infatuator = infatuator;
            this.startText = startText;
            this.endText = endText;
            this.moveText = moveText;
            this.moveFailText = moveFailText;
            this.failText = failText;
            this.alreadyText = alreadyText;
            this.moveFailChance = moveFailChance;
        }
        public new Infatuation Clone()
        {
            return new Infatuation(
                infatuator: infatuator,
                startText: startText, endText: endText,
                moveText: moveText, moveFailText: moveFailText, failText: failText, alreadyText: alreadyText,
                moveFailChance: moveFailChance
                );
        }
    }

    /// <summary>
    /// Flags this status effect as a move-limiting type status effect (ex. Taunt, Torment, etc.)
    /// </summary>
    public class MoveLimiting : Volatile
    {
        /// <summary>
        /// If true, this move can be used during the turn it becomes limited (but not after).
        /// </summary>
        public bool canUseMiddleOfTurn;

        /// <summary>
        /// The text displayed when the Pokémon attempts to use the blocked move.
        /// </summary>
        public string attemptText;

        public MoveLimiting(
            PokemonSEType effectType,
            string startText = null, string endText = null,
            string alreadyText = null, string failText = null,
            string attemptText = null,
            bool canUseMiddleOfTurn = false,
            General.DefaultTurns defaultTurns = null
            )
            : base(
                    effectType: effectType,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    defaultTurns: defaultTurns)
        {
            this.attemptText = attemptText;
            this.canUseMiddleOfTurn = canUseMiddleOfTurn;
        }
        public new MoveLimiting Clone()
        {
            return 
                this is Disable ? (this as Disable).Clone()
                : this is Encore ? (this as Encore).Clone()
                : this is HealBlock ? (this as HealBlock).Clone()
                : this is Taunt ? (this as Taunt).Clone()
                : this is Torment ? (this as Torment).Clone()
                : new MoveLimiting(
                    effectType: effectType,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    canUseMiddleOfTurn: canUseMiddleOfTurn,
                    defaultTurns: defaultTurns
                    );
        }
    }

    /// <summary>
    /// Flags this status as non-volatile.
    /// </summary>
    public class NonVolatile : PokemonSE
    {
        /// <summary>
        /// The priority value of this status condition. Low priority statuses can't overwrite 
        /// higher priority ones.
        /// </summary>
        public int priority;

        /// <summary>
        /// The text that displays when this status condition negates a lower priority one.
        /// </summary>
        public string negateTextID;

        public NonVolatile(
            int priority = 1,
            string negateTextID = null
            )
            : base(PokemonSEType.NonVolatile)
        {
            this.priority = priority;
            this.negateTextID = negateTextID;
        }
        public new NonVolatile Clone()
        {
            return new NonVolatile(priority: priority, negateTextID: negateTextID);
        }
    }

    /// <summary>
    /// Causes stat stage changes every turn that this status is active.
    /// </summary>
    public class Octolock : PokemonSE
    {
        public General.StatStageMod statStageMod;

        public Octolock(
            General.StatStageMod statStageMod,
            PokemonSETiming timing = PokemonSETiming.EndOfTurn,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(
                    effectType: PokemonSEType.Octolock, timing: timing, filters: filters)
        {
            this.statStageMod = statStageMod.Clone();
        }
        public new Octolock Clone()
        {
            return new Octolock(
                statStageMod: statStageMod,
                timing: timing, filters: filters
                );
        }
    }

    /// <summary>
    /// May prevent this Pokémon from attacking.
    /// </summary>
    public class Paralysis : PokemonSE
    {
        /// <summary>
        /// The chance of full paralysis when attacking.
        /// </summary>
        public float chance;
        /// <summary>
        /// The text displayed when fully paralyzed.
        /// </summary>
        public string displayText;

        public Paralysis(
            float chance = 0.25f,
            string displayText = "status-paralysis"
            )
            : base(effectType: PokemonSEType.Paralysis)
        {
            this.chance = chance;
            this.displayText = displayText;
        }
        public new Paralysis Clone()
        {
            return new Paralysis(chance: chance, displayText: displayText);
        }
    }

    /// <summary>
    /// Causes the Pokémon to instantly faint after a certain amount of turns. 
    /// </summary>
    public class PerishSong : PokemonSE
    {
        /// <summary>
        /// The amount of turns left before this Pokémon faints.
        /// </summary>
        public int turnsLeft;

        /// <summary>
        /// The text displayed when Perish Song is inflicted.
        /// </summary>
        public string startText;
        /// <summary>
        /// The text displayed when Perish Song counts down.
        /// </summary>
        public string countText;

        public PerishSong(
            int turnsLeft = 3,
            string startText = "status-perishsong",
            string countText = "status-perishsong-count"
            )
            : base(effectType: PokemonSEType.PerishSong)
        {
            this.turnsLeft = Mathf.Max(0, turnsLeft);
            this.startText = startText;
            this.countText = countText;
        }
        public new PerishSong Clone()
        {
            return new PerishSong(
                turnsLeft: turnsLeft,
                startText: startText, countText: countText);
        }
    }

    /// <summary>
    /// May prevent this Pokémon from attacking.
    /// </summary>
    public class Sleep : PokemonSE
    {
        /// <summary>
        /// The text displayed when fully paralyzed.
        /// </summary>
        public string displayText;

        public Sleep(
            string displayText = "status-sleep"
            )
            : base(effectType: PokemonSEType.Sleep)
        {
            this.displayText = displayText;
        }
        public new Sleep Clone()
        {
            return new Sleep(displayText: displayText);
        }
    }

    /// <summary>
    /// Adds additional type resistances, weaknesses, or immunities to the afflicted Pokémon.
    /// </summary>
    public class TarShot : PokemonSE
    {
        /// <summary>
        /// The unique identifier for this instance of Tar Shot. A Pokémon cannot have multiple Tar Shots
        /// with the same ID.
        /// </summary>
        public string tarShotID;
        /// <summary>
        /// The text displayed when Tar Shot is afflicted to a Pokémon.
        /// </summary>
        public string startText;

        /// <summary>
        /// The additional resistances.
        /// </summary>
        public List<string> resistances;
        /// <summary>
        /// The additional weaknesses.
        /// </summary>
        public List<string> weaknesses;
        /// <summary>
        /// The additional immunities.
        /// </summary>
        public List<string> immunities;

        public TarShot(
            string tarShotID = "tarshot",
            string startText = null,
            IEnumerable<string> resistances = null,
            IEnumerable<string> weaknesses = null,
            IEnumerable<string> immunities = null
            )
            : base(effectType: PokemonSEType.TarShot)
        {
            this.tarShotID = tarShotID;
            this.startText = startText;
            this.resistances = resistances == null ? new List<string>() : new List<string>(resistances);
            this.weaknesses = weaknesses == null ? new List<string>() : new List<string>(weaknesses);
            this.immunities = immunities == null ? new List<string>() : new List<string>(immunities);
        }
        public new TarShot Clone()
        {
            return new TarShot(
                tarShotID: tarShotID, startText: startText,
                resistances: resistances, weaknesses: weaknesses, immunities: immunities
                );
        }

    }

    /// <summary>
    /// Prevents this Pokemon from using status moves for a few turns.
    /// </summary>
    public class Taunt : MoveLimiting
    {
        /// <summary>
        /// The move category that is blocked by Taunt.
        /// </summary>
        public MoveCategory category;

        public Taunt(
            MoveCategory category = MoveCategory.Status,
            string startText = "status-taunt-start", string endText = "status-taunt-end",
            string alreadyText = "status-taunt-already", string failText = "status-taunt-fail",
            string attemptText = "status-taunt-attempt",
            General.DefaultTurns defaultTurns = null
            )
            : base(
                    effectType: PokemonSEType.Taunt,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    )
        {
            this.category = category;
        }
        public new Taunt Clone()
        {
            return new Taunt(
                category: category,
                startText: startText, endText: endText,
                alreadyText: alreadyText, failText: failText,
                attemptText: attemptText,
                defaultTurns: defaultTurns
                );
        }
    }

    /// <summary>
    /// Prevents this Pokemon from using the same move twice in a row.
    /// </summary>
    public class Torment : MoveLimiting
    {
        public Torment(
            string startText = "status-torment-start", string endText = "status-torment-end",
            string alreadyText = "status-torment-already", string failText = "status-torment-fail",
            string attemptText = "status-torment-attempt",
            General.DefaultTurns defaultTurns = null
            )
            : base(
                    effectType: PokemonSEType.Torment,
                    startText: startText, endText: endText,
                    alreadyText: alreadyText, failText: failText,
                    attemptText: attemptText,
                    defaultTurns: defaultTurns
                    )
        {

        }
        public new Torment Clone()
        {
            return new Torment(
                startText: startText, endText: endText,
                alreadyText: alreadyText, failText: failText,
                attemptText: attemptText,
                defaultTurns: defaultTurns
                );
        }
    }

    /// <summary>
    /// Specifies types immune to this status.
    /// </summary>
    public class TypeImmunity : PokemonSE
    {
        public TypeImmunity(
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: PokemonSEType.TypeImmunity, filters: filters)
        {

        }
        public new TypeImmunity Clone()
        {
            return new TypeImmunity(
                filters: filters
                );
        }
    }

    /// <summary>
    /// Flags this status as volatile. This effect will be applied independently from its parent status condition.
    /// </summary>
    public class Volatile : PokemonSE
    {
        public string startText;
        public string endText;
        public string alreadyText;
        public string failText;
        public General.DefaultTurns defaultTurns;

        public Volatile(
            PokemonSEType effectType,
            string startText = null, string endText = null,
            string alreadyText = null, string failText = null,
            General.DefaultTurns defaultTurns = null
            )
            : base(effectType: effectType)
        {
            this.startText = startText;
            this.endText = endText;
            this.alreadyText = alreadyText;
            this.failText = failText;
            this.defaultTurns = defaultTurns == null ? new General.DefaultTurns()
                : defaultTurns.Clone();
        }
        public new Volatile Clone()
        {
            return new Volatile(
                effectType: effectType,
                startText: startText, endText: endText,
                alreadyText: alreadyText, failText: failText,
                defaultTurns: defaultTurns
                );
        }
    }

    /// <summary>
    /// The Pokémon becomes drowsy, and falls asleep on the next turn.
    /// </summary>
    public class Yawn : PokemonSE
    {
        /// <summary>
        /// The status inflicted at the end of the Yawn turns.
        /// </summary>
        public string statusID;

        /// <summary>
        /// The turns left for this status effect.
        /// </summary>
        public int turnsLeft;

        /// <summary>
        /// The text displayed when the Pokémon becomes drowsy.
        /// </summary>
        public string startText;
        /// <summary>
        /// The text displayed on the turns the Pokémon is drowsy but is not yet inflicted with the status.
        /// </summary>
        public string waitText;

        public Yawn(
            string statusID = null,
            int turnsLeft = 0,
            string startText = "status-yawn",
            string waitText = "status-yawn-wait"
            )
            : base(effectType: PokemonSEType.Yawn)
        {
            this.statusID = statusID;
            this.turnsLeft = turnsLeft;
            this.startText = startText;
            this.waitText = waitText;
        }
        public new Yawn Clone()
        {
            return new Yawn(statusID: statusID, turnsLeft: turnsLeft, startText: startText, waitText: waitText);
        }
    }
}
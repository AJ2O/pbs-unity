using System.Collections.Generic;

namespace PBS.Databases.Effects.BattleStatuses
{
    public class BattleSE
    {
        /// <summary>
        /// The type of battle status effect it is.
        /// </summary>
        public BattleSEType effectType;
        /// <summary>
        /// The time that this effect applies itself.
        /// </summary>
        public BattleSETiming timing;
        /// <summary>
        /// If true, this effect can stack where applicable.
        /// </summary>
        public bool isStackable;

        /// <summary>
        /// Additional restrictions on how the effect is applied.
        /// </summary>
        public List<Filter.FilterEffect> filters;

        public BattleSE(
            BattleSEType effectType,
            BattleSETiming timing = BattleSETiming.Unique,
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
        public BattleSE Clone()
        {
            return
                this is BattleEnvironment ?

                    this is MagicRoom ? (this as MagicRoom).Clone()
                    : this is Gravity ? (this as Gravity).Clone()
                    : this is Terrain ? (this as Terrain).Clone()
                    : this is TrickRoom ? (this as TrickRoom).Clone()
                    : this is Weather ? (this as Weather).Clone()
                    : this is WonderRoom ? (this as WonderRoom).Clone()
                    : (this as BattleEnvironment).Clone()

                : this is BlockMoves ? (this as BlockMoves).Clone()
                : this is BlockStatus ? (this as BlockStatus).Clone()
                : this is DesolateLand ? (this as DesolateLand).Clone()
                : this is HPGain ? (this as HPGain).Clone()
                : this is HPLoss ? (this as HPLoss).Clone()
                : this is IonDeluge ? (this as IonDeluge).Clone()
                : this is MoveDamageModifier ? (this as MoveDamageModifier).Clone()
                : this is StatScale ? (this as StatScale).Clone()
                : this is StrongWinds ? (this as StrongWinds).Clone()
                : this is TypeDamageModifier ? (this as TypeDamageModifier).Clone()
                : new BattleSE(
                    effectType: effectType,
                    timing: timing,
                    isStackable: isStackable,
                    filters: filters
                    );
        }
    }


    /// <summary>
    /// Defines a battle environmental factor, such as <seealso cref="Weather"/>, <see cref="Gravity"/>,
    /// or <seealso cref="TrickRoom"/>.
    /// </summary>
    public class BattleEnvironment : BattleSE
    {
        /// <summary>
        /// The priority that this environmental condition has. This condition cannot overwrite conditions
        /// of higher priority. Set to -1 to overwrite any other priority.
        /// </summary>
        public int priority;
        /// <summary>
        /// Text that displays if this condition has negated a lesser priority condition.
        /// </summary>
        public string negateText;
        /// <summary>
        /// The move that Nature Power becomes during this condition. Leaving it to null
        /// makes Nature Power ignore this condition when considering its transformed move.
        /// </summary>
        public string naturePowerMove;

        public BattleEnvironment(
            BattleSEType conditionType = BattleSEType.BattleEnvironment,
            string naturePowerMove = null, int priority = 0, string negateText = null
            )
            : base(effectType: conditionType)
        {
            this.naturePowerMove = naturePowerMove;
            this.priority = priority;
            this.negateText = negateText;
        }
        public new BattleEnvironment Clone()
        {
            return new BattleEnvironment(
                naturePowerMove: naturePowerMove,
                conditionType: effectType,
                priority: priority,
                negateText: negateText);
        }
    }

    /// <summary>
    /// Makes the Pokémon affected by this battle condition immune to certain moves.
    /// </summary>
    public class BlockMoves : BattleSE
    {
        /// <summary>
        /// Specific moves that are blocked during this condition.
        /// </summary>
        public List<string> moves;
        /// <summary>
        /// Blocks priority moves against affected Pokémon.
        /// </summary>
        public bool psychicTerrain;

        /// <summary>
        /// The text played when a move is successfully blocked by this effect.
        /// </summary>
        public string blockText;

        public BlockMoves(
            IEnumerable<string> moves = null,
            bool psychicTerrain = false,
            string blockText = null,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: BattleSEType.BlockMoves, filters: filters)
        {
            this.moves = moves == null ? new List<string>() : new List<string>(moves);
            this.psychicTerrain = psychicTerrain;
            this.blockText = blockText;
        }
        public new BlockMoves Clone()
        {
            return new BlockMoves(
                moves: moves, psychicTerrain: psychicTerrain, blockText: blockText,
                filters: filters
                );
        }
    }

    /// <summary>
    /// Makes the Pokémon affected by this battle condition immune to the given status conditions.
    /// </summary>
    public class BlockStatus : BattleSE
    {
        /// <summary>
        /// The statuses blocked for Pokémon affected by this battle condition.
        /// </summary>
        public List<string> statusIDs;
        /// <summary>
        /// The status effect types blocked for Pokémon affected by this battle condition.
        /// </summary>
        public HashSet<PokemonSEType> SETypes;

        /// <summary>
        /// The text played when a status is successfully blocked by this effect.
        /// </summary>
        public string blockText;

        public BlockStatus(
            IEnumerable<string> statusIDs = null,
            IEnumerable<PokemonSEType> SETypes = null,
            string blockText = null,

            BattleSETiming timing = BattleSETiming.Unique,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: BattleSEType.BlockStatus, timing: timing, filters: filters)
        {
            this.statusIDs = statusIDs == null ? new List<string>() : new List<string>(statusIDs);
            this.SETypes = SETypes == null ? new HashSet<PokemonSEType>() : new HashSet<PokemonSEType>(SETypes);
            this.blockText = blockText;
        }
        public new BlockStatus Clone()
        {
            return new BlockStatus(
                statusIDs: statusIDs, SETypes: SETypes, blockText: blockText,
                timing: timing, filters: filters
                );
        }

    }

    /// <summary>
    /// Negates the use of moves of the given types.
    /// </summary>
    public class DesolateLand : BattleSE
    {
        /// <summary>
        /// The text displayed when a move is negated.
        /// </summary>
        public string negateText;

        /// <summary>
        /// The move types negated.
        /// </summary>
        public List<string> types;
        /// <summary>
        /// Set to true to invert the move types that would be affected.
        /// </summary>
        public bool invert;


        public DesolateLand(
            string negateText,
            IEnumerable<string> types = null, bool invert = false
            )
            : base(effectType: BattleSEType.DesolateLand)
        {
            this.negateText = negateText;
            this.types = types == null ? new List<string>() : new List<string>(types);
            this.invert = invert;
        }
        public new DesolateLand Clone()
        {
            return new DesolateLand(negateText: negateText, types: types, invert: invert);
        }
    }

    /// <summary>
    /// Defines a type of gravity setting, such as Gravity.
    /// </summary>
    public class Gravity : BattleEnvironment
    {
        /// <summary>
        /// If true, gravity is intensified while this effect is active.
        /// </summary>
        public bool intensifyGravity;
        /// <summary>
        /// Text displayed when Pokémon become forcefully grounded.
        /// </summary>
        public string groundedText;
        /// <summary>
        /// Text displayed when a move is blocked by gravity.
        /// </summary>
        public string moveFailText;

        public Gravity(
            bool intensifyGravity = false,
            string groundedText = "bStatus-gravity-intensify", string moveFailText = "bStatus-gravity-movefail",

            int priority = 0, string negateText = null
            ) : base(conditionType: BattleSEType.Gravity, priority: priority, negateText: negateText)
        {
            this.intensifyGravity = intensifyGravity;
            this.groundedText = groundedText;
            this.moveFailText = moveFailText;
        }
        public new Gravity Clone()
        {
            return new Gravity(
                intensifyGravity: intensifyGravity,
                groundedText: groundedText, moveFailText: moveFailText,
                priority: priority, negateText: negateText);
        }
    }

    /// <summary>
    /// Heals affected Pokémon by a percentage of their maximum HP.
    /// </summary>
    public class HPGain : BattleSE
    {
        /// <summary>
        /// The text to display if Pokémon gain HP.
        /// </summary>
        public string displayText;

        /// <summary>
        /// The percentage of HP gained from this effect.
        /// </summary>
        public float hpGainPercent;

        public HPGain(
            string displayText = null,
            float hpGainPercent = 1f / 16,

            BattleSETiming timing = BattleSETiming.EndOfTurn,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: BattleSEType.HPGain, timing: timing, filters: filters)
        {
            this.displayText = displayText;
            this.hpGainPercent = hpGainPercent;
        }
        public new HPGain Clone()
        {
            return new HPGain(
                displayText: displayText, hpGainPercent: hpGainPercent,

                timing: timing, filters: filters
                );
        }
    }

    /// <summary>
    /// Damages affected Pokémon by a percentage of their maximum HP.
    /// </summary>
    public class HPLoss : BattleSE
    {
        /// <summary>
        /// The text to display if Pokémon lose HP.
        /// </summary>
        public string displayText;

        /// <summary>
        /// The percentage of HP lost from this effect.
        /// </summary>
        public float hpLossPercent;

        public HPLoss(
            string displayText = null,
            float hpLossPercent = 1f / 16,

            BattleSETiming timing = BattleSETiming.EndOfTurn,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: BattleSEType.HPLoss, timing: timing, filters: filters)
        {
            this.displayText = displayText;
            this.hpLossPercent = hpLossPercent;
        }
        public new HPLoss Clone()
        {
            return new HPLoss(
                displayText: displayText, hpLossPercent: hpLossPercent,

                timing: timing, filters: filters
                );
        }

    }

    /// <summary>
    /// While active, changes moves of the given types into a new type.
    /// </summary>
    public class IonDeluge : BattleSE
    {
        /// <summary>
        /// The type that moves are changed into.
        /// </summary>
        public string toType;
        /// <summary>
        /// The types that moves
        /// </summary>
        public List<string> fromTypes;

        public IonDeluge(
            string toType = "electric",
            IEnumerable<string> fromTypes = null
            )
            : base(effectType: BattleSEType.IonDeluge)
        {
            this.toType = toType;
            this.fromTypes = fromTypes == null ? new List<string>() : new List<string>(fromTypes);
        }
        public new IonDeluge Clone()
        {
            return new IonDeluge(toType: toType, fromTypes: fromTypes);
        }
    }

    /// <summary>
    /// Defines a type of environment that has an effect on held items, such as Magic Room.
    /// </summary>
    public class MagicRoom : BattleEnvironment
    {
        /// <summary>
        /// If true, held items have their effects suppressed (if possible).
        /// </summary>
        public bool suppressItems;

        public MagicRoom(
            bool suppressItems = false,

            int priority = 0, string negateText = null
            ) : base(conditionType: BattleSEType.MagicRoom, priority: priority, negateText: negateText)
        {
            this.suppressItems = suppressItems;
        }
        public new MagicRoom Clone()
        {
            return new MagicRoom(
                suppressItems: suppressItems,
                priority: priority, negateText: negateText
                );
        }
    }

    /// <summary>
    /// Scales damage done by moves of the specified moves.
    /// </summary>
    public class MoveDamageModifier : BattleSE
    {
        /// <summary>
        /// The amount to scale damage for affected types by.
        /// </summary>
        public float damageScale;
        /// <summary>
        /// The moves affected by the damage scale.
        /// </summary>
        public List<string> moves;

        /// <summary>
        /// If true, a check is performed first to see if the user is affected by the condition.
        /// </summary>
        public bool offensiveCheck;
        /// <summary>
        /// If true, a check is performed first to see if the target is affected by the condition.
        /// </summary>
        public bool defensiveCheck;

        public MoveDamageModifier(
            float damageScale = 1f,
            IEnumerable<string> moves = null,
            bool offensiveCheck = false, bool defensiveCheck = false,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: BattleSEType.MoveDamageModifier, filters: filters)
        {
            this.damageScale = damageScale;
            this.moves = moves == null ? new List<string>() : new List<string>(moves);
            this.offensiveCheck = offensiveCheck;
            this.defensiveCheck = defensiveCheck;
        }
        public new MoveDamageModifier Clone()
        {
            return new MoveDamageModifier(
                damageScale: damageScale, moves: moves,
                offensiveCheck: offensiveCheck, defensiveCheck: defensiveCheck,
                filters: filters);
        }
    }

    /// <summary>
    /// Scales certain stats during a battle condition.
    /// </summary>
    public class StatScale : BattleSE
    {
        /// <summary>
        /// The stat being scaled.
        /// </summary>
        public float ATKMod, DEFMod, SPAMod, SPDMod, SPEMod, ACCMod, EVAMod;

        public StatScale(
            float ATKMod = 1f, float DEFMod = 1f, float SPAMod = 1f, float SPDMod = 1f,
            float SPEMod = 1f, float ACCMod = 1f, float EVAMod = 1f,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: BattleSEType.StatScale, filters: filters)
        {
            this.ATKMod = ATKMod;
            this.DEFMod = DEFMod;
            this.SPAMod = SPAMod;
            this.SPDMod = SPDMod;
            this.SPEMod = SPEMod;
            this.ACCMod = ACCMod;
            this.EVAMod = EVAMod;
        }
        public new StatScale Clone()
        {
            return new StatScale(
                ATKMod: ATKMod, DEFMod: DEFMod, SPAMod: SPAMod, SPDMod: SPDMod,
                SPEMod: SPEMod, ACCMod: ACCMod, EVAMod: EVAMod,

                filters: filters
                );
        }
        public float GetStatMod(PokemonStats statType)
        {
            return statType == PokemonStats.Attack ? ATKMod
                : statType == PokemonStats.Defense ? DEFMod
                : statType == PokemonStats.SpecialAttack ? SPAMod
                : statType == PokemonStats.SpecialDefense ? SPDMod
                : statType == PokemonStats.Speed ? SPEMod
                : statType == PokemonStats.Accuracy ? ACCMod
                : statType == PokemonStats.Evasion ? EVAMod
                : 1f;
        }
    }

    /// <summary>
    /// Changes the effectiveness of moves against the listed types.
    /// </summary>
    public class StrongWinds : BattleSE
    {
        /// <summary>
        /// The text displayed when a move is affected.
        /// </summary>
        public string changeText;

        /// <summary>
        /// The move types affected.
        /// </summary>
        public List<string> types;
        /// <summary>
        /// Set to true to invert the move types that would be affected.
        /// </summary>
        public bool invert;

        /// <summary>
        /// This effect is triggered if the listed types had an effectiveness contained here.
        /// </summary>
        public HashSet<TypeEffectiveness> effectivenessFilter;
        /// <summary>
        /// The effeciveness that overwrites the effectiveness in the effectiveness filter.
        /// </summary>
        public TypeEffectiveness forceEffectiveness;

        public StrongWinds(
            string changeText = null,
            IEnumerable<string> types = null, bool invert = false,
            IEnumerable<TypeEffectiveness> effectivenessFilter = null,
            TypeEffectiveness forceEffectiveness = TypeEffectiveness.Neutral
            )
            : base(effectType: BattleSEType.StrongWinds)
        {
            this.changeText = changeText;
            this.types = types == null ? new List<string>() : new List<string>(types);
            this.invert = invert;
            this.effectivenessFilter = effectivenessFilter == null ? new HashSet<TypeEffectiveness>()
                : new HashSet<TypeEffectiveness>(effectivenessFilter);
            this.forceEffectiveness = forceEffectiveness;
        }
        public new StrongWinds Clone()
        {
            return new StrongWinds(
                changeText: changeText, types: types, invert: invert,
                effectivenessFilter: effectivenessFilter, forceEffectiveness: forceEffectiveness
                );
        }
        public float GetEffectiveness()
        {
            return forceEffectiveness == TypeEffectiveness.Neutral ? 1f
                : forceEffectiveness == TypeEffectiveness.SuperEffective ? GameSettings.btlSuperEffectivenessMultiplier
                : forceEffectiveness == TypeEffectiveness.NotVeryEffective ? GameSettings.btlNotVeryEffectivenessMultiplier
                : forceEffectiveness == TypeEffectiveness.Immune ? GameSettings.btlImmuneEffectivenessMultiplier
                : 1f;
        }
    }

    /// <summary>
    /// Defines a type of battle terrain, such as Electric Terrain or Misty Terrain.
    /// </summary>
    public class Terrain : BattleEnvironment
    {
        /// <summary>
        /// The type that a move with the TerranPulse effect becomes. Set to null for no change.
        /// </summary>
        public string terrainPulseType;
        public Terrain(
            string terrainPulseType = null,
            string naturePowerMove = null, int priority = 0, string negateText = null
            ) : base(
                conditionType: BattleSEType.Terrain,
                naturePowerMove: naturePowerMove,
                priority: priority,
                negateText: negateText)
        {
            this.terrainPulseType = terrainPulseType;
        }
        public new Terrain Clone()
        {
            return new Terrain(
                terrainPulseType: terrainPulseType, naturePowerMove: naturePowerMove,
                priority: priority, negateText: negateText);
        }
    }

    /// <summary>
    /// Defines a type of environment to determines turn order, such as Trick Room.
    /// </summary>
    public class TrickRoom : BattleEnvironment
    {
        /// <summary>
        /// The stat used to determine turn order.
        /// </summary>
        public PokemonStats speedStat;
        /// <summary>
        /// Set to true to reverse the resulting order.
        /// </summary>
        public bool reverse;

        public TrickRoom(
            PokemonStats speedStat = PokemonStats.Speed,
            bool reverse = false,

            int priority = 0, string negateText = null
            ) : base(conditionType: BattleSEType.TrickRoom, priority: priority, negateText: negateText)
        {
            this.speedStat = speedStat;
            this.reverse = reverse;
        }
        public new TrickRoom Clone()
        {
            return new TrickRoom(
                speedStat: speedStat, reverse: reverse,
                priority: priority, negateText: negateText
                );
        }
    }

    /// <summary>
    /// Scales damage done by moves of the specified types.
    /// </summary>
    public class TypeDamageModifier : BattleSE
    {
        /// <summary>
        /// The amount to scale damage for affected types by.
        /// </summary>
        public float damageScale;
        /// <summary>
        /// The types affected by the damage scale.
        /// </summary>
        public List<string> types;
        /// <summary>
        /// Set to true to invert the types that would be affected by the damage scale.
        /// </summary>
        public bool invert;

        /// <summary>
        /// If true, a check is performed first to see if the user is affected by the condition.
        /// </summary>
        public bool offensiveCheck;
        /// <summary>
        /// If true, a check is performed first to see if the target is affected by the condition.
        /// </summary>
        public bool defensiveCheck;


        public TypeDamageModifier(
            float damageScale = 1f,
            IEnumerable<string> types = null,
            bool invert = false,
            bool offensiveCheck = false, bool defensiveCheck = false,

            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: BattleSEType.TypeDamageModifier, filters: filters)
        {
            this.damageScale = damageScale;
            this.types = types == null ? new List<string>() : new List<string>(types);
            this.invert = invert;
            this.offensiveCheck = offensiveCheck;
            this.defensiveCheck = defensiveCheck;
        }
        public new TypeDamageModifier Clone()
        {
            return new TypeDamageModifier(
                damageScale: damageScale, types: types, invert: invert,
                offensiveCheck: offensiveCheck, defensiveCheck: defensiveCheck,
                filters: filters);
        }
    }

    /// <summary>
    /// Defines a type of battle weather, such as Harsh Sunlight, Heavy Rain, or Sandstorm.
    /// </summary>
    public class Weather : BattleEnvironment
    {
        /// <summary>
        /// The type that a move with the WeatherBall effect becomes. Set to null for no change.
        /// </summary>
        public string weatherBallType;
        /// <summary>
        /// Set to true to activate any applicable boosts to Weather Ball.
        /// </summary>
        public bool weatherBallBoost;

        public Weather(
            string weatherBallType = null, bool weatherBallBoost = true,
            int priority = 0, string negateText = null
            ) : base(conditionType: BattleSEType.Weather, priority: priority, negateText: negateText)
        {
            this.weatherBallType = weatherBallType;
            this.weatherBallBoost = weatherBallBoost;
        }
        public new Weather Clone()
        {
            return new Weather(
                weatherBallType: weatherBallType, weatherBallBoost: weatherBallBoost,
                priority: priority, negateText: negateText);
        }
    }

    /// <summary>
    /// Defines a type of environment that remaps stats, such as Wonder Room.
    /// </summary>
    public class WonderRoom : BattleEnvironment
    {
        public PokemonStats ATKMap, DEFMap, SPAMap, SPDMap, SPEMap;

        public WonderRoom(
            PokemonStats ATKMap = PokemonStats.Attack, PokemonStats DEFMap = PokemonStats.Defense,
            PokemonStats SPAMap = PokemonStats.SpecialAttack, PokemonStats SPDMap = PokemonStats.SpecialDefense,
            PokemonStats SPEMap = PokemonStats.Speed,

            int priority = 0, string negateText = null
            ) : base(conditionType: BattleSEType.WonderRoom, priority: priority, negateText: negateText)
        {
            this.ATKMap = ATKMap;
            this.DEFMap = DEFMap;
            this.SPAMap = SPAMap;
            this.SPDMap = SPDMap;
            this.SPEMap = SPEMap;
        }
        public new WonderRoom Clone()
        {
            return new WonderRoom(
                ATKMap: ATKMap, DEFMap: DEFMap,
                SPAMap: SPAMap, SPDMap: SPDMap,
                SPEMap: SPEMap,
                priority: priority, negateText: negateText);
        }
        public PokemonStats GetMappedStat(PokemonStats statType)
        {
            return statType == PokemonStats.Attack ? ATKMap
                : statType == PokemonStats.Defense ? DEFMap
                : statType == PokemonStats.SpecialAttack ? SPAMap
                : statType == PokemonStats.SpecialDefense ? SPDMap
                : statType == PokemonStats.Speed ? SPEMap
                : statType;
        }
    }
}
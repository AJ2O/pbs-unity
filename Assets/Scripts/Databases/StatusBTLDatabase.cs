using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases
{

}

public class StatusBTLDatabase
{
    //create an object of SingleObject
    private static StatusBTLDatabase singleton = new StatusBTLDatabase();

    //make the constructor private so that this class cannot be
    //instantiated
    private StatusBTLDatabase() { }

    //Get the only object available
    public static StatusBTLDatabase instance
    {
        get
        {
            return singleton;
        }
        private set
        {
            singleton = value;
        }
    }

    // Database
    private Dictionary<string, StatusBTLData> database = new Dictionary<string, StatusBTLData>
    {
        // Null / Placeholder
        {"",
            new StatusBTLData(
                ID: ""
                ) },

        // ---DEFAULTS---

        // Clear Skies
        {"clearskies",
            new StatusBTLData(
                ID: "clearskies",
                conditionName: "Clear Skies",

                tags: new BattleSTag[]
                {
                    BattleSTag.Default,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(weatherBallBoost: false),
                }
                ) },

        // Clear Terrain
        {"clearterrain",
            new StatusBTLData(
                ID: "clearterrain",
                conditionName: "Clear Terrain",

                tags: new BattleSTag[]
                {
                    BattleSTag.Default,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Terrain(),
                }
                ) },

        // Regular Gravity
        {"regulargravity",
            new StatusBTLData(
                ID: "regulargravity",
                conditionName: "Regular Gravity",

                tags: new BattleSTag[]
                {
                    BattleSTag.Default,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Gravity(),
                }
                ) },

        // Default Magic Room
        {"defaultmagicroom",
            new StatusBTLData(
                ID: "defaultmagicroom",
                conditionName: "Default Magic Room",

                tags: new BattleSTag[]
                {
                    BattleSTag.Default,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.MagicRoom(),
                }
                ) },

        // Default Trick Room
        {"defaulttrickroom",
            new StatusBTLData(
                ID: "defaulttrickroom",
                conditionName: "Default Trick Room",

                tags: new BattleSTag[]
                {
                    BattleSTag.Default,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.TrickRoom()
                }
                ) },

        // Default Wonder Room
        {"defaultwonderroom",
            new StatusBTLData(
                ID: "defaultwonderroom",
                conditionName: "Default Wonder Room",

                tags: new BattleSTag[]
                {
                    BattleSTag.Default,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.WonderRoom(),
                }
                ) },


        // ---GRAVITY---

        // Gravity
        {"gravity",
            new StatusBTLData(
                ID: "gravity",
                conditionName: "Gravity",
                startTextID: "bStatus-gravity-start",
                endTextID: "bStatus-gravity-end",

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd,
                    BattleSTag.UndoesSelf,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Gravity(intensifyGravity: true),
                }
                ) },


        // ---ROOMS---

        // Magic Room
        {"magicroom",
            new StatusBTLData(
                ID: "magicroom",
                conditionName: "Magic Room",
                startTextID: "bStatus-magicroom-start",
                endTextID: "bStatus-magicroom-end",

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd,
                    BattleSTag.UndoesSelf,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.MagicRoom(suppressItems: true),
                }
                ) },

        // Trick Room
        {"trickroom",
            new StatusBTLData(
                ID: "trickroom",
                conditionName: "Trick Room",
                startTextID: "battle-status-inflict-trickroom",
                endTextID: "battle-status-heal-trickroom",

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd,
                    BattleSTag.UndoesSelf,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.TrickRoom(speedStat: PokemonStats.Speed, reverse: true)
                }
                ) },

        // Wonder Room
        {"wonderroom",
            new StatusBTLData(
                ID: "wonderroom",
                conditionName: "Wonder Room",
                startTextID: "battle-status-inflict-wonderroom",
                endTextID: "battle-status-heal-wonderroom",

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd,
                    BattleSTag.UndoesSelf,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.WonderRoom(
                        DEFMap: PokemonStats.SpecialDefense, SPEMap: PokemonStats.Defense
                        )
                }
                ) },


        // ---TERRAIN---

        // Electric Terrain
        {"electricterrain",
            new StatusBTLData(
                ID: "electricterrain",
                conditionName: "Electric Terrain",

                startTextID: "terrain-electricterrain-start",
                natureTextID: "terrain-electricterrain-nature",
                endTextID: "terrain-electricterrain-end",
                alreadyTextID: "terrain-electricterrain-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),

                tags: new BattleSTag[]
                {
                    BattleSTag.IsGrounded,
                    BattleSTag.TurnsDecreaseOnEnd,
                },
                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Terrain(
                        terrainPulseType: "electric",
                        naturePowerMove: "thunderbolt"
                        ),
                    new EffectDatabase.StatusBTLEff.TypeDamageModifier(
                        damageScale: 1.3f,
                        types: new string[] { "electric" },
                        offensiveCheck: true
                        ),
                    new EffectDatabase.StatusBTLEff.BlockStatus(
                        statusIDs: new string[] { "sleep" },
                        blockText: "terrain-electricterrain-block"
                        ),
                }
                ) },

        // Grassy Terrain
        {"grassyterrain",
            new StatusBTLData(
                ID: "grassyterrain",
                conditionName: "Grassy Terrain",

                startTextID: "terrain-grassyterrain-start",
                natureTextID: "terrain-grassyterrain-nature",
                endTextID: "terrain-grassyterrain-end",
                alreadyTextID: "terrain-grassyterrain-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),

                tags: new BattleSTag[]
                {
                    BattleSTag.IsGrounded,
                    BattleSTag.TurnsDecreaseOnEnd,
                },
                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Terrain(
                        terrainPulseType: "grass",
                        naturePowerMove: "energyball"
                        ),
                    new EffectDatabase.StatusBTLEff.TypeDamageModifier(
                        damageScale: 1.3f,
                        types: new string[] { "grass" },
                        offensiveCheck: true
                        ),
                    new EffectDatabase.StatusBTLEff.MoveDamageModifier(
                        damageScale: 0.5f,
                        moves: new string[] { "bulldoze", "earthquake", "magnitude" },
                        defensiveCheck: true
                        ),
                    new EffectDatabase.StatusBTLEff.HPGain(
                        displayText: "terrain-grassyterrain-heal",
                        hpGainPercent: 1f/16,
                        timing: BattleSETiming.EndOfTurn
                        ),
                }
                ) },

        // Misty Terrain
        {"mistyterrain",
            new StatusBTLData(
                ID: "mistyterrain",
                conditionName: "Misty Terrain",

                startTextID: "terrain-mistyterrain-start",
                natureTextID: "terrain-mistyterrain-nature",
                endTextID: "terrain-mistyterrain-end",
                alreadyTextID: "terrain-mistyterrain-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),

                tags: new BattleSTag[]
                {
                    BattleSTag.IsGrounded,
                    BattleSTag.TurnsDecreaseOnEnd,
                },
                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Terrain(
                        terrainPulseType: "fairy",
                        naturePowerMove: "moonblast"
                        ),
                    new EffectDatabase.StatusBTLEff.TypeDamageModifier(
                        damageScale: 0.5f,
                        types: new string[] { "dragon" },
                        defensiveCheck: true
                        ),
                    new EffectDatabase.StatusBTLEff.BlockStatus(
                        statusIDs: new string[] 
                        { 
                            "burn", "freeze", "paralysis", "poison",
                            "poison2", "sleep", "confusion",
                        },
                        blockText: "terrain-mistyterrain-block"
                        ),
                }
                ) },

        // Psychic Terrain
        {"psychicterrain",
            new StatusBTLData(
                ID: "psychicterrain",
                conditionName: "Psychic Terrain",

                startTextID: "terrain-psychicterrain-start",
                natureTextID: "terrain-psychicterrain-nature",
                endTextID: "terrain-psychicterrain-end",
                alreadyTextID: "terrain-psychicterrain-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),

                tags: new BattleSTag[]
                {
                    BattleSTag.IsGrounded,
                    BattleSTag.TurnsDecreaseOnEnd,
                },
                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Terrain(
                        terrainPulseType: "psychic",
                        naturePowerMove: "psychic"
                        ),
                    new EffectDatabase.StatusBTLEff.TypeDamageModifier(
                        damageScale: 1.3f,
                        types: new string[] { "psychic" },
                        offensiveCheck: true
                        ),
                    new EffectDatabase.StatusBTLEff.BlockMoves(
                        psychicTerrain: true,
                        blockText: "terrain-psychicterrain-block"
                        ),
                }
                ) },


        // ---WEATHER---

        // Extremely Harsh Sunglight
        {"extremelyharshsunlight",
            new StatusBTLData(
                ID: "extremelyharshsunlight",
                baseID: "harshsunlight",
                conditionName: "Extremely Harsh Sunlight",

                startTextID: "weather-extremelyharshsunlight-start",
                natureTextID: "weather-extremelyharshsunlight-nature",
                endTextID: "weather-extremelyharshsunlight-end",
                alreadyTextID: "weather-extremelyharshsunlight-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),

                combineBaseTags: true,
                combineBaseEffects: true,

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(
                        weatherBallType: "fire",
                        priority: 1, negateText: "weather-extremelyharshsunlight-block"
                        ),
                    new EffectDatabase.StatusBTLEff.DesolateLand(
                        negateText: "weather-extremelyharshsunlight-negate",
                        types: new string[] { "water" }
                        ),
                }
                ) },

        // Fog
        {"fog",
            new StatusBTLData(
                ID: "fog",
                conditionName: "Fog",

                startTextID: "weather-fog-start",
                natureTextID: "weather-fog-nature",
                endTextID: "weather-fog-end",
                alreadyTextID: "weather-fog-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd
                },

                effects: new BattleCEff[]
                {
                    new BattleCEff(
                        effectType: BattleSEType.StatScale,
                        floatParams: new float[] { 3f/5 },
                        stringParams: new string[] { "acc" }
                        ),
                },
                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(),
                }
                ) },

        // Hail
        {"hail",
            new StatusBTLData(
                ID: "hail",
                conditionName: "Hail",

                startTextID: "weather-hail-start",
                natureTextID: "weather-hail-nature",
                endTextID: "weather-hail-end",
                alreadyTextID: "weather-hail-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(
                        weatherBallType: "ice"
                        ),
                    new EffectDatabase.StatusBTLEff.HPLoss(
                        displayText: "weather-hail-buffet",
                        hpLossPercent: 1f/16,
                        timing: BattleSETiming.EndOfTurn,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                types: new string[] { "ice" },
                                invert: true
                                ),
                        }
                        ),
                }
                ) },

        // Harsh Sunlight
        {"harshsunlight",
            new StatusBTLData(
                ID: "harshsunlight",
                conditionName: "Harsh Sunlight",

                startTextID: "weather-harshsunlight-start",
                natureTextID: "weather-harshsunlight-nature",
                endTextID: "weather-harshsunlight-end",
                alreadyTextID: "weather-harshsunlight-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(
                        weatherBallType: "fire"
                        ),
                    new EffectDatabase.StatusBTLEff.TypeDamageModifier(
                        damageScale: 1.5f,
                        types: new string[] { "fire" }
                        ),
                    new EffectDatabase.StatusBTLEff.TypeDamageModifier(
                        damageScale: 0.5f,
                        types: new string[] { "water" }
                        ),
                }
                ) },

        // Heavy Rain
        {"heavyrain",
            new StatusBTLData(
                ID: "heavyrain",
                baseID: "rain",
                conditionName: "Heavy Rain",

                startTextID: "weather-heavyrain-start",
                natureTextID: "weather-heavyrain-nature",
                endTextID: "weather-heavyrain-end",
                alreadyTextID: "weather-heavyrain-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),

                combineBaseTags: true,
                combineBaseEffects: true,
                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(
                        weatherBallType: "water",
                        priority: 1, negateText: "weather-heavyrain-block"
                        ),
                    new EffectDatabase.StatusBTLEff.DesolateLand(
                        negateText: "weather-heavyrain-negate",
                        types: new string[] { "fire" }
                        ),
                }
                ) },

        // Rain
        {"rain",
            new StatusBTLData(
                ID: "rain",
                conditionName: "Rain",

                startTextID: "weather-rain-start",
                natureTextID: "weather-rain-nature",
                endTextID: "weather-rain-end",
                alreadyTextID: "weather-rain-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(
                        weatherBallType: "water"
                        ),
                    new EffectDatabase.StatusBTLEff.TypeDamageModifier(
                        damageScale: 1.5f,
                        types: new string[] { "water" }
                        ),
                    new EffectDatabase.StatusBTLEff.TypeDamageModifier(
                        damageScale: 0.5f,
                        types: new string[] { "fire" }
                        ),
                }
                ) },

        // Sandstorm
        {"sandstorm",
            new StatusBTLData(
                ID: "sandstorm",
                conditionName: "Sandstorm",

                startTextID: "weather-sandstorm-start",
                natureTextID: "weather-sandstorm-nature",
                endTextID: "weather-sandstorm-end",
                alreadyTextID: "weather-sandstorm-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(
                        weatherBallType: "rock"
                        ),
                    new EffectDatabase.StatusBTLEff.HPLoss(
                        displayText: "weather-sandstorm-buffet",
                        hpLossPercent: 1f/16,
                        timing: BattleSETiming.EndOfTurn,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                types: new string[] { "rock", "ground", "steel" },
                                invert: true
                                ),
                        }
                        ),
                    new EffectDatabase.StatusBTLEff.StatScale(
                        SPDMod: 1.5f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                types: new string[] { "rock" }
                                ),
                        }
                        ),
                }
                ) },

        // Strong Winds
        {"strongwinds",
            new StatusBTLData(
                ID: "strongwinds",
                conditionName: "Strong Winds",

                startTextID: "weather-strongwinds-start",
                endTextID: "weather-strongwinds-end",
                alreadyTextID: "weather-strongwinds-already",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.Weather(
                        weatherBallBoost: false,
                        priority: 1, negateText: "weather-strongwinds-block"
                        ),
                    new EffectDatabase.StatusBTLEff.StrongWinds(
                        changeText: "weather-strongwinds-weaken",
                        types: new string[] { "flying" },
                        effectivenessFilter: new HashSet<TypeEffectiveness> { TypeEffectiveness.SuperEffective },
                        forceEffectiveness: TypeEffectiveness.Neutral
                        )
                }
                ) },


        // ---MOVES---

        // Ion Deluge
        {"iondeluge",
            new StatusBTLData(
                ID: "iondeluge",
                conditionName: "Ion Deluge",
                startTextID: "battle-status-inflict-iondeluge",

                defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 0),

                tags: new BattleSTag[]
                {
                    BattleSTag.TurnsDecreaseOnEnd,
                },

                effectsNew: new EffectDatabase.StatusBTLEff.BattleSE[]
                {
                    new EffectDatabase.StatusBTLEff.IonDeluge(
                        toType: "electric",
                        fromTypes: new string[] { "normal" }
                        )
                }
                ) },

    };

    // Methods
    public StatusBTLData GetStatusData(string ID)
    {
        if (database.ContainsKey(ID))
        {
            return database[ID];
        }
        Debug.LogWarning("Could not find condition with ID: " + ID);
        return database[""];
    }

    public StatusBTLData GetDefaultGravity()
    {
        return GetStatusData("regulargravity");
    }
    public StatusBTLData GetDefaultMagicRoom()
    {
        return GetStatusData("defaultmagicroom");
    }
    public StatusBTLData GetDefaultTerrain()
    {
        return GetStatusData("clearterrain");
    }
    public StatusBTLData GetDefaultTrickRoom()
    {
        return GetStatusData("defaulttrickroom");
    }
    public StatusBTLData GetDefaultWeather()
    {
        return GetStatusData("clearskies");
    }
    public StatusBTLData GetDefaultWonderRoom()
    {
        return GetStatusData("defaultwonderroom");
    }

}

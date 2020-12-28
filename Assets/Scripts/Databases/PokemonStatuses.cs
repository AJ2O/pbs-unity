using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases
{
    public class PokemonStatuses
    {
        //create an object of SingleObject
        private static PokemonStatuses singleton = new PokemonStatuses();

        //make the constructor private so that this class cannot be
        //instantiated
        private PokemonStatuses() { }

        //Get the only object available
        public static PokemonStatuses instance
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
        private Dictionary<string, StatusPKData> database = new Dictionary<string, StatusPKData>
        {
            // Null / Placeholder
            {"",
                new StatusPKData(
                    ID: "",
                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.NonStick
                    }
                    ) },

            // NON-VOLATILE

            // Healthy
            {"healthy",
                new StatusPKData(
                    ID: "healthy",
                    conditionName: "Healthy",

                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.IsDefault,
                        PokemonSTag.NonVolatile,
                    },

                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.NonVolatile(priority: 0),
                    }
                    ) },

            // Burn
            {"burn",
                new StatusPKData(
                    ID: "burn",
                    conditionName: "Burn", shortName: "BRN",
                    inflictTextID: "status-burn-start",
                    healTextID: "status-burn-end",
                    alreadyTextID: "status-burn-already",
                    failTextID: "status-burn-fail",

                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),
                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.NonVolatile,
                        PokemonSTag.TurnsDecreaseOnEnd,
                    },
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.NonVolatile(),
                        new EffectDatabase.StatusPKEff.TypeImmunity(
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.TypeList(
                                    targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "fire" },
                                    invert: true
                                    ),
                            }
                            ),
                        new EffectDatabase.StatusPKEff.HPLoss(
                            displayText: "status-burn-hploss",
                            hpLossPercent: 1f/16
                            ),
                        new EffectDatabase.StatusPKEff.Burn(
                            statScale: new EffectDatabase.General.StatScale(
                                scaleMap: new Dictionary<PokemonStats, float>
                                {
                                    { PokemonStats.Attack, 0.5f },
                                })
                            )
                    }
                    ) },

            // Freeze
            {"freeze",
                new StatusPKData(
                    ID: "freeze",
                    conditionName: "Freeze",
                    shortName: "FRZ",
                    inflictTextID: "status-freeze-start",
                    healTextID: "status-freeze-heal",
                    alreadyTextID: "status-freeze-already",
                    failTextID: "status-freeze-fail",

                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),
                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.NonVolatile,
                        PokemonSTag.TurnsDecreaseOnEnd,
                    },
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.NonVolatile(),
                        new EffectDatabase.StatusPKEff.Freeze(
                            thawMoveTypes: new string[] { "fire" }),
                        new EffectDatabase.StatusPKEff.TypeImmunity(
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.TypeList(
                                    targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "ice", },
                                    invert: true
                                    ),
                            }
                            ),
                    }
                    ) },

            // Paralysis
            {"paralysis",
                new StatusPKData(
                    ID: "paralysis",
                    conditionName: "Paralysis",
                    shortName: "PRZ",
                    inflictTextID: "status-paralysis-start",
                    healTextID: "status-paralysis-end",
                    alreadyTextID: "status-paralysis-already",
                    failTextID: "status-paralysis-fail",

                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),
                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.NonVolatile,
                        PokemonSTag.TurnsDecreaseOnEnd,
                    },
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.NonVolatile(),
                        new EffectDatabase.StatusPKEff.Paralysis(),
                        new EffectDatabase.StatusPKEff.Burn(
                            statScale: new EffectDatabase.General.StatScale(
                                scaleMap: new Dictionary<PokemonStats, float>
                                {
                                    { PokemonStats.Speed, 0.5f },
                                })
                            ),
                        new EffectDatabase.StatusPKEff.TypeImmunity(
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.TypeList(
                                    targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "electric", },
                                    invert: true
                                    ),
                            }
                            ),
                    }
                    ) },

            // Poison
            {"poison",
                new StatusPKData(
                    ID: "poison",
                    conditionName: "Poison", shortName: "PSN",
                    inflictTextID: "status-poison-start",
                    healTextID: "status-poison-end",
                    alreadyTextID: "status-poison-already",
                    failTextID: "status-poison-fail",

                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),
                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.NonVolatile,
                        PokemonSTag.TurnsDecreaseOnEnd,
                    },
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.NonVolatile(),
                        new EffectDatabase.StatusPKEff.HPLoss(
                            displayText: "status-poison-hploss",
                            hpLossPercent: 1f/8
                            ),
                        new EffectDatabase.StatusPKEff.TypeImmunity(
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.TypeList(
                                    targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "poison", "steel" },
                                    invert: true
                                    ),
                            }
                            ),
                    }
                    ) },

            // Toxic / Badly Poisoned
            {"poison2",
                new StatusPKData(
                    ID: "poison2",
                    baseID: "poison",
                    conditionName: "Toxic Poison", shortName: "TXC",
                    inflictTextID: "status-toxic-start",
                    healTextID: "status-poison-end",
                    alreadyTextID: "status-poison-already",
                    failTextID: "status-poison-fail",

                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1),

                    combineBaseTags: true,
                    combineBaseEffects: true,
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.HPLoss(
                            displayText: "status-poison-hploss",
                            hpLossPercent: 1f/16,
                            toxicStack: true
                            ),
                    }
                    ) },

            // Sleep
            {"sleep",
                new StatusPKData(
                    ID: "sleep",
                    conditionName: "Sleep", shortName: "SLP",
                    inflictTextID: "status-sleep-start",
                    healTextID: "status-sleep-end",
                    alreadyTextID: "status-sleep-already",
                    failTextID: "status-sleep-fail",

                    defaultTurns: new EffectDatabase.General.DefaultTurns(useTurnRange: true, lowestTurns: 1, highestTurns: 3),
                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.NonVolatile,
                        PokemonSTag.TurnsDecreaseOnMove
                    },
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.NonVolatile(),
                        new EffectDatabase.StatusPKEff.Sleep(),
                    }
                    ) },


            // VOLATILE
        
            // Bound
            {"bound",
                new StatusPKData(
                    ID: "bound",
                    conditionName: "Bound",

                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.NonStick,
                        PokemonSTag.RequiresPokemonCauseInBattle,
                        PokemonSTag.TurnsDecreaseOnEnd,
                    },

                    conditionEffects: new PokemonCEff[]
                    {
                        new PokemonCEff(
                            effectType: PokemonSEType.Bound,
                            effectTiming: PokemonSETiming.EndOfTurn,
                            floatParams: new float[] { 1f/16 },
                            stringParams: new string[]
                            {
                                "status-bound-inflict",
                                "status-bound-hploss",
                                "status-bound-end",
                                "status-bound-switch",
                            }
                            ),
                    }
                    ) },

            // Confusion
            {"confusion",
                new StatusPKData(
                    ID: "confusion",
                    conditionName: "Confusion",
                    inflictTextID: "status-inflict-confusion",
                    healTextID: "status-heal-confusion",
                    alreadyTextID: "status-already-confusion",
                    failTextID: "status-fail-confusion",

                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.NonStick,
                        PokemonSTag.TurnsDecreaseOnMove
                    },

                    conditionEffects: new PokemonCEff[]
                    {
                        new PokemonCEff(
                            effectType: PokemonSEType.Confusion,
                            boolParams: new bool[] { true },
                            floatParams: new float[] { 1f/3, 40 },
                            stringParams: new string[] { "DEFAULT", "DEFAULT" }
                            ),
                        new PokemonCEff(
                            effectType: PokemonSEType.DefaultTurnsLeft,
                            boolParams: new bool[] { true },
                            floatParams: new float[] { 2, 5 }
                            ),
                    }
                    ) },

            // Encore
            {"encore",
                new StatusPKData(
                    ID: "encore",
                    conditionName: "Encore",
                    statusTags: new PokemonSTag[]
                    {
                        PokemonSTag.TurnsDecreaseOnEnd
                    },

                    conditionEffects: new PokemonCEff[]
                    {
                        new PokemonCEff(
                            effectType: PokemonSEType.Encore,
                            stringParams: new string[] { "DEFAULT" }
                            ),
                        new PokemonCEff(
                            effectType: PokemonSEType.DefaultTurnsLeft,
                            boolParams: new bool[] { false },
                            floatParams: new float[] { 4, 0 }
                            ),
                    }
                    ) },

            // Defense Curl
            {"defensecurl",
                new StatusPKData(
                    ID: "defensecurl",
                    conditionName: "Defense Curl",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.DefenseCurl(),
                    }
                    ) },

            // Disable
            {"disable",
                new StatusPKData(
                    ID: "disable",
                    conditionName: "Disable",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Disable(
                            defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 4)
                            ),
                    }
                    ) },

            // Electrify
            {"electrify",
                new StatusPKData(
                    ID: "electrify",
                    conditionName: "Electrify",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Electrify(),
                    }
                    ) },

            // Embargo
            {"embargo",
                new StatusPKData(
                    ID: "embargo",
                    conditionName: "Embargo",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Embargo(
                            defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5)
                            ),
                    }
                    ) },

            // Flinch
            {"flinch",
                new StatusPKData(
                    ID: "flinch",
                    conditionName: "Flinch",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Flinch(),
                    }
                    ) },

            // Heal Block
            {"healblock",
                new StatusPKData(
                    ID: "healblock",
                    conditionName: "Heal Block",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.HealBlock(
                            defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5)
                            ),
                    }
                    ) },

            // Identification
            {"identification",
                new StatusPKData(
                    ID: "identification",
                    conditionName: "identification",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Identification(
                            types: new string[] { "ghost" }
                            ),
                    }
                    ) },

            // Imprison
            {"imprison",
                new StatusPKData(
                    ID: "imprison",
                    conditionName: "Imprison",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Imprison(),
                    }
                    ) },

            // Infatuation
            {"infatuation",
                new StatusPKData(
                    ID: "infatuation",
                    conditionName: "Infatuation",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Infatuation(),
                    }
                    ) },

            // Perish Song
            {"perishsong",
                new StatusPKData(
                    ID: "perishsong",
                    conditionName: "Perish Song",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.PerishSong(),
                    }
                    ) },

            // Tar Shot
            {"tarshot",
                new StatusPKData(
                    ID: "tarshot",
                    conditionName: "Tar Shot",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.TarShot(
                            startText: "status-tarshot",
                            weaknesses: new string[] { "fire" }
                            )
                    }
                    ) },

            // Taunt
            {"taunt",
                new StatusPKData(
                    ID: "taunt",
                    conditionName: "Taunt",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Taunt(
                            defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 3)
                            ),
                    }
                    ) },

            // Torment
            {"torment",
                new StatusPKData(
                    ID: "torment",
                    conditionName: "Torment",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Torment(
                            defaultTurns: new EffectDatabase.General.DefaultTurns(turns: -1)
                            ),
                    }
                    ) },

            // Yawn
            {"yawn",
                new StatusPKData(
                    ID: "yawn",
                    conditionName: "Yawn",
                    effectsNew: new EffectDatabase.StatusPKEff.PokemonSE[]
                    {
                        new EffectDatabase.StatusPKEff.Yawn(
                            statusID: "sleep",
                            turnsLeft: 1
                            ),
                    }
                    ) },

        };

        // Methods
        public StatusPKData GetStatusData(string ID)
        {
            if (database.ContainsKey(ID))
            {
                return database[ID];
            }
            Debug.LogWarning("Could not find move with ID: " + ID);
            return database[""];
        }
        public StatusPKData GetDefaultStatusData()
        {
            return GetStatusData("healthy");
        }
    }
}
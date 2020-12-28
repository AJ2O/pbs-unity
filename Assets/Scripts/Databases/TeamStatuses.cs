using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases
{
    public class TeamStatuses
    {
        //create an object of SingleObject
        private static TeamStatuses singleton = new TeamStatuses();

        //make the constructor private so that this class cannot be
        //instantiated
        private TeamStatuses() { }

        //Get the only object available
        public static TeamStatuses instance
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
        private Dictionary<string, StatusTEData> database = new Dictionary<string, StatusTEData>
        {
            // Null / Placeholder
            {"",
                new StatusTEData(
                    ID: ""
                    ) },

            // G-MAX EFFECTS
            {"gmaxcannonade",
                new StatusTEData(
                    ID: "gmaxcannonade",
                    conditionName: "G-Max Cannonade",
                    inflictTextID: "gmax-wildfire",
                    endTextID: "gmax-wildfire-end",
                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 4),
                    tags: new TeamSTag[]
                    {

                    },
                    effectsNew: new EffectDatabase.StatusTEEff.TeamSE[]
                    {
                        new EffectDatabase.StatusTEEff.GMaxWildfirePriority(),
                        new EffectDatabase.StatusTEEff.HPLoss(
                            displayText: "gmax-wildfire-damage",
                            hpLossPercent: 1f / 6,
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.TypeList(
                                    targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "water" },
                                    invert: true
                                    ),
                            }
                            )
                    }
                    ) },
            {"gmaxvinelash",
                new StatusTEData(
                    ID: "gmaxvinelash",
                    conditionName: "G-Max Vine Lash",
                    inflictTextID: "gmax-wildfire",
                    endTextID: "gmax-wildfire-end",
                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 4),
                    tags: new TeamSTag[]
                    {

                    },
                    effectsNew: new EffectDatabase.StatusTEEff.TeamSE[]
                    {
                        new EffectDatabase.StatusTEEff.GMaxWildfirePriority(),
                        new EffectDatabase.StatusTEEff.HPLoss(
                            displayText: "gmax-wildfire-damage",
                            hpLossPercent: 1f / 6,
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.TypeList(
                                    targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "grass" },
                                    invert: true
                                    ),
                            }
                            )
                    }
                    ) },
            {"gmaxvolcalith",
                new StatusTEData(
                    ID: "gmaxvolcalith",
                    conditionName: "G-Max Volcalith",
                    inflictTextID: "gmax-wildfire",
                    endTextID: "gmax-wildfire-end",
                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 4),
                    tags: new TeamSTag[]
                    {

                    },
                    effectsNew: new EffectDatabase.StatusTEEff.TeamSE[]
                    {
                        new EffectDatabase.StatusTEEff.GMaxWildfirePriority(),
                        new EffectDatabase.StatusTEEff.HPLoss(
                            displayText: "gmax-wildfire-damage",
                            hpLossPercent: 1f / 6,
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.TypeList(
                                    targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "rock" },
                                    invert: true
                                    ),
                            }
                            )
                    }
                    ) },
            {"gmaxwildfire",
                new StatusTEData(
                    ID: "gmaxwildfire",
                    conditionName: "G-Max Wildfire",
                    inflictTextID: "gmax-wildfire",
                    endTextID: "gmax-wildfire-end",
                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 4),
                    tags: new TeamSTag[]
                    {

                    },
                    effectsNew: new EffectDatabase.StatusTEEff.TeamSE[]
                    {
                        new EffectDatabase.StatusTEEff.GMaxWildfirePriority(),
                        new EffectDatabase.StatusTEEff.HPLoss(
                            displayText: "gmax-wildfire-damage",
                            hpLossPercent: 1f / 6,
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.TypeList(
                                    targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "fire" },
                                    invert: true
                                    ),
                            }
                            )
                    }
                    ) },

            // HAZARDS

            // LIGHT SCREENS
            {"auroraveil",
                new StatusTEData(
                    ID: "auroraveil",
                    conditionName: "Aurora Veil",
                    inflictTextID: "tStatus-auroraveil-start",
                    endTextID: "tStatus-auroraveil-end",
                    alreadyTextID: "tStatus-auroraveil-already",
                    failTextID: "tStatus-auroraveil-fail",
                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),
                    effectsNew: new EffectDatabase.StatusTEEff.TeamSE[]
                    {
                        new EffectDatabase.StatusTEEff.LightScreen(
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.MoveCheck(
                                    moveCategories: new MoveCategory[] { MoveCategory.Physical, MoveCategory.Special }
                                    )
                            }
                            )
                    }
                    ) },
            {"lightscreen",
                new StatusTEData(
                    ID: "lightscreen",
                    conditionName: "Light Screen",
                    inflictTextID: "tStatus-lightscreen-start",
                    endTextID: "tStatus-lightscreen-end",
                    alreadyTextID: "tStatus-lightscreen-already",
                    failTextID: "tStatus-lightscreen-fail",
                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),
                    effectsNew: new EffectDatabase.StatusTEEff.TeamSE[]
                    {
                        new EffectDatabase.StatusTEEff.LightScreen(
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.MoveCheck(
                                    moveCategories: new MoveCategory[] { MoveCategory.Special }
                                    )
                            }
                            )
                    }
                    ) },
            {"reflect",
                new StatusTEData(
                    ID: "reflect",
                    conditionName: "Reflect",
                    inflictTextID: "tStatus-reflect-start",
                    endTextID: "tStatus-reflect-end",
                    alreadyTextID: "tStatus-reflect-already",
                    failTextID: "tStatus-reflect-fail",
                    defaultTurns: new EffectDatabase.General.DefaultTurns(turns: 5),
                    effectsNew: new EffectDatabase.StatusTEEff.TeamSE[]
                    {
                        new EffectDatabase.StatusTEEff.LightScreen(
                            filters: new EffectDatabase.Filter.FilterEffect[]
                            {
                                new EffectDatabase.Filter.MoveCheck(
                                    moveCategories: new MoveCategory[] { MoveCategory.Physical }
                                    )
                            }
                            )
                    }
                    ) },

            // STATS

        };

        // Methods
        public StatusTEData GetStatusData(string ID)
        {
            if (database.ContainsKey(ID))
            {
                return database[ID];
            }
            Debug.LogWarning("Could not find move with ID: " + ID);
            return database[""];
        }

    }
}
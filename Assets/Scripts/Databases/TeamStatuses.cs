using PBS.Data;
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
        private Dictionary<string, TeamStatus> database = new Dictionary<string, TeamStatus>
        {
            // Null / Placeholder
            {"",
                new TeamStatus(
                    ID: ""
                    ) },

            // G-MAX EFFECTS
            {"gmaxcannonade",
                new TeamStatus(
                    ID: "gmaxcannonade",
                    conditionName: "G-Max Cannonade",
                    inflictTextID: "gmax-wildfire",
                    endTextID: "gmax-wildfire-end",
                    defaultTurns: new Effects.General.DefaultTurns(turns: 4),
                    tags: new TeamSTag[]
                    {

                    },
                    effectsNew: new Effects.TeamStatuses.TeamSE[]
                    {
                        new Effects.TeamStatuses.GMaxWildfirePriority(),
                        new Effects.TeamStatuses.HPLoss(
                            displayText: "gmax-wildfire-damage",
                            hpLossPercent: 1f / 6,
                            filters: new Effects.Filter.FilterEffect[]
                            {
                                new Effects.Filter.TypeList(
                                    targetType: Effects.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "water" },
                                    invert: true
                                    ),
                            }
                            )
                    }
                    ) },
            {"gmaxvinelash",
                new TeamStatus(
                    ID: "gmaxvinelash",
                    conditionName: "G-Max Vine Lash",
                    inflictTextID: "gmax-wildfire",
                    endTextID: "gmax-wildfire-end",
                    defaultTurns: new Effects.General.DefaultTurns(turns: 4),
                    tags: new TeamSTag[]
                    {

                    },
                    effectsNew: new Effects.TeamStatuses.TeamSE[]
                    {
                        new Effects.TeamStatuses.GMaxWildfirePriority(),
                        new Effects.TeamStatuses.HPLoss(
                            displayText: "gmax-wildfire-damage",
                            hpLossPercent: 1f / 6,
                            filters: new Effects.Filter.FilterEffect[]
                            {
                                new Effects.Filter.TypeList(
                                    targetType: Effects.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "grass" },
                                    invert: true
                                    ),
                            }
                            )
                    }
                    ) },
            {"gmaxvolcalith",
                new TeamStatus(
                    ID: "gmaxvolcalith",
                    conditionName: "G-Max Volcalith",
                    inflictTextID: "gmax-wildfire",
                    endTextID: "gmax-wildfire-end",
                    defaultTurns: new Effects.General.DefaultTurns(turns: 4),
                    tags: new TeamSTag[]
                    {

                    },
                    effectsNew: new Effects.TeamStatuses.TeamSE[]
                    {
                        new Effects.TeamStatuses.GMaxWildfirePriority(),
                        new Effects.TeamStatuses.HPLoss(
                            displayText: "gmax-wildfire-damage",
                            hpLossPercent: 1f / 6,
                            filters: new Effects.Filter.FilterEffect[]
                            {
                                new Effects.Filter.TypeList(
                                    targetType: Effects.Filter.TypeList.TargetType.Pokemon,
                                    types: new string[] { "rock" },
                                    invert: true
                                    ),
                            }
                            )
                    }
                    ) },
            {"gmaxwildfire",
                new TeamStatus(
                    ID: "gmaxwildfire",
                    conditionName: "G-Max Wildfire",
                    inflictTextID: "gmax-wildfire",
                    endTextID: "gmax-wildfire-end",
                    defaultTurns: new Effects.General.DefaultTurns(turns: 4),
                    tags: new TeamSTag[]
                    {

                    },
                    effectsNew: new Effects.TeamStatuses.TeamSE[]
                    {
                        new Effects.TeamStatuses.GMaxWildfirePriority(),
                        new Effects.TeamStatuses.HPLoss(
                            displayText: "gmax-wildfire-damage",
                            hpLossPercent: 1f / 6,
                            filters: new Effects.Filter.FilterEffect[]
                            {
                                new Effects.Filter.TypeList(
                                    targetType: Effects.Filter.TypeList.TargetType.Pokemon,
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
                new TeamStatus(
                    ID: "auroraveil",
                    conditionName: "Aurora Veil",
                    inflictTextID: "tStatus-auroraveil-start",
                    endTextID: "tStatus-auroraveil-end",
                    alreadyTextID: "tStatus-auroraveil-already",
                    failTextID: "tStatus-auroraveil-fail",
                    defaultTurns: new Effects.General.DefaultTurns(turns: 5),
                    effectsNew: new Effects.TeamStatuses.TeamSE[]
                    {
                        new Effects.TeamStatuses.LightScreen(
                            filters: new Effects.Filter.FilterEffect[]
                            {
                                new Effects.Filter.MoveCheck(
                                    moveCategories: new MoveCategory[] { MoveCategory.Physical, MoveCategory.Special }
                                    )
                            }
                            )
                    }
                    ) },
            {"lightscreen",
                new TeamStatus(
                    ID: "lightscreen",
                    conditionName: "Light Screen",
                    inflictTextID: "tStatus-lightscreen-start",
                    endTextID: "tStatus-lightscreen-end",
                    alreadyTextID: "tStatus-lightscreen-already",
                    failTextID: "tStatus-lightscreen-fail",
                    defaultTurns: new Effects.General.DefaultTurns(turns: 5),
                    effectsNew: new Effects.TeamStatuses.TeamSE[]
                    {
                        new Effects.TeamStatuses.LightScreen(
                            filters: new Effects.Filter.FilterEffect[]
                            {
                                new Effects.Filter.MoveCheck(
                                    moveCategories: new MoveCategory[] { MoveCategory.Special }
                                    )
                            }
                            )
                    }
                    ) },
            {"reflect",
                new TeamStatus(
                    ID: "reflect",
                    conditionName: "Reflect",
                    inflictTextID: "tStatus-reflect-start",
                    endTextID: "tStatus-reflect-end",
                    alreadyTextID: "tStatus-reflect-already",
                    failTextID: "tStatus-reflect-fail",
                    defaultTurns: new Effects.General.DefaultTurns(turns: 5),
                    effectsNew: new Effects.TeamStatuses.TeamSE[]
                    {
                        new Effects.TeamStatuses.LightScreen(
                            filters: new Effects.Filter.FilterEffect[]
                            {
                                new Effects.Filter.MoveCheck(
                                    moveCategories: new MoveCategory[] { MoveCategory.Physical }
                                    )
                            }
                            )
                    }
                    ) },

            // STATS

        };

        // Methods
        public TeamStatus GetStatusData(string ID)
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
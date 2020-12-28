using PBS.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases
{
    public class Abilities
    {
        //create an object of SingleObject
        private static Abilities singleton = new Abilities();

        //make the constructor private so that this class cannot be
        //instantiated
        private Abilities() { }

        //Get the only object available
        public static Abilities instance
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
        private Dictionary<string, AbilityData> database = new Dictionary<string, AbilityData>
    {
        // Null / Placeholder
        {"",
            new AbilityData(
                ID: "",
                abilityName: "",
                tags: new AbilityTag[]
                {

                },
                effects: new AbilityEffect[]
                {

                }
                ) },

        // Adaptability
        {"adaptability",
            new AbilityData(
                ID: "adaptability",
                abilityName: "Adaptability",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Adaptability(),
                }
                ) },

        // Aerilate
        {"aerilate",
            new AbilityData(
                ID: "aerilate",
                abilityName: "Aerilate",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Aerilate(
                        toMoveType: "flying",
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "normal" }
                                ),
                        }
                        ),
                }
                ) },

        // Aftermath
        {"aftermath",
            new AbilityData(
                ID: "aftermath",
                abilityName: "Aftermath",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Aftermath(
                        damage: new Effects.General.Damage(
                            mode: Effects.General.Damage.DamageMode.MaxHPPercent,
                            value: 0.25f
                            )
                        ),
                }
                ) },

        // Air Lock
        {"airlock",
            new AbilityData(
                ID: "airlock",
                abilityName: "Air Lock",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.AirLock(),
                }
                ) },

        // Analytic
        {"analytic",
            new AbilityData(
                ID: "analytic",
                abilityName: "Analytic",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Analytic(),
                }
                ) },

        // Anger Point
        {"angerpoint",
            new AbilityData(
                ID: "angerpoint",
                abilityName: "Anger Point",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.AngerPoint(
                        statStageMod: new Effects.General.StatStageMod(maxATK: true)
                        ),
                }
                ) },

        // Anticipation
        {"anticipation",
            new AbilityData(
                ID: "anticipation",
                abilityName: "Anticipation",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Anticipation(),
                }
                ) },

        // Arena Trap
        {"arenatrap",
            new AbilityData(
                ID: "arenatrap",
                abilityName: "Arena Trap",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ShadowTag(
                        arenaTrap: true,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(types: new string[]{ "ghost" }, invert: true),
                        }
                        ),
                }
                ) },

        // Aroma Veil
        {"aromaveil",
            new AbilityData(
                ID: "aromaveil",
                abilityName: "Aroma Veil",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Oblivious(
                        aromaVeil: true,
                        effectsBlocked: new PokemonSEType[]
                        {
                            PokemonSEType.Disable,
                            PokemonSEType.Encore,
                            PokemonSEType.HealBlock,
                            PokemonSEType.Infatuation,
                            PokemonSEType.Taunt,
                            PokemonSEType.Torment
                        }
                        ),
                }
                ) },

        // Aura Break
        {"aurabreak",
            new AbilityData(
                ID: "aurabreak",
                abilityName: "Aura Break",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.AuraBreak(),
                }
                ) },

        // Bad Dreams
        {"baddreams",
            new AbilityData(
                ID: "baddreams",
                abilityName: "Bad Dreams",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.BadDreams(),
                }
                ) },

        // Ball Fetch
        {"ballfetch",
            new AbilityData(
                ID: "ballfetch",
                abilityName: "Ball Fetch",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.BallFetch(),
                }
                ) },

        // Battery
        {"battery",
            new AbilityData(
                ID: "battery",
                abilityName: "Battery",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Battery(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Special }
                                )
                        }
                        ),
                }
                ) },

        // Battle Armor
        {"battlearmor",
            new AbilityData(
                ID: "battlearmor",
                abilityName: "Battle Armor",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.BattleArmor(),
                }
                ) },

        // Battle Bond
        {"battlebond",
            new AbilityData(
                ID: "battlebond",
                abilityName: "Battle Bond",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay, AbilityTag.CannotRolePlayUser,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress,
                    AbilityTag.CannotWorrySeed,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.BattleBond(
                        transformations: new Effects.Abilities.BattleBond.BattleBondTransformation[]
                        {
                            new Effects.Abilities.BattleBond.BattleBondTransformation(
                                preForm: "greninja-battlebond", toForm: "greninja-ash"),
                        }
                        ),
                }
                ) },

        // Beast Boost
        {"beastboost",
            new AbilityData(
                ID: "beastboost",
                abilityName: "Beast Boost",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.BeastBoost(statMod: 1),
                }
                ) },

        // Berserk
        {"berserk",
            new AbilityData(
                ID: "berserk",
                abilityName: "Berserk",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Berserk(
                        hpThreshold: 0.5f,
                        statStageMod: new Effects.General.StatStageMod(SPAMod: 1)
                        ),
                }
                ) },

        // Big Pecks
        {"bigpecks",
            new AbilityData(
                ID: "bigpecks",
                abilityName: "Big Pecks",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HyperCutter(
                        affectedStats: new PokemonStats[] { PokemonStats.Defense, }
                        )
                }
                ) },

        // Blaze
        {"blaze",
            new AbilityData(
                ID: "blaze",
                abilityName: "Blaze",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.5f,
                        blazeThreshold: 1f/3,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fire" }
                                )
                        }
                        ),
                }
                ) },

        // Bulletproof
        {"bulletproof",
            new AbilityData(
                ID: "bulletproof",
                abilityName: "Bulletproof",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Cacophony(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.BallMove, MoveTag.BombMove }
                                )
                        }
                        ),
                }
                ) },

        // Cacophony
        {"cacophony",
            new AbilityData(
                ID: "cacophony",
                abilityName: "Cacophony",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Cacophony(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.SoundMove, }
                                )
                        }
                        ),
                }
                ) },

        // Cheek Pouch
        {"cheekpouch",
            new AbilityData(
                ID: "cheekpouch",
                abilityName: "Cheek Pouch",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.CheekPouch(),
                }
                ) },

        // Chlorophyll
        {"chlorophyll",
            new AbilityData(
                ID: "chlorophyll",
                abilityName: "Chlorophyll",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SwiftSwim(
                        conditions: new List<Effects.Abilities.SwiftSwim.SwiftSwimCondition>
                        {
                            new Effects.Abilities.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "harshsunlight" },
                                statScale: new Effects.General.StatScale(
                                    scaleMap: new Dictionary<PokemonStats, float>
                                    {
                                        { PokemonStats.Speed, 2f }
                                    })
                                )
                        })
                }
                ) },

        // Clear Body
        {"clearbody",
            new AbilityData(
                ID: "clearbody",
                abilityName: "Clear Body",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HyperCutter(
                        clearBody: true,
                        displayText: "ability-clearbody"),
                }
                ) },

        // Color Change
        {"colorchange",
            new AbilityData(
                ID: "colorchange",
                abilityName: "Color Change",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ColorChange(),
                }
                ) },

        // Comatose
        {"comatose",
            new AbilityData(
                ID: "comatose",
                abilityName: "Comatose",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Comatose(),
                }
                ) },

        // Competitive
        {"competitive",
            new AbilityData(
                ID: "competitive",
                abilityName: "Competitive",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Defiant(
                        statStageMod: new Effects.General.StatStageMod(SPAMod: 2),
                        lowerTriggers: new PokemonStats[]
                        {
                            PokemonStats.Attack, PokemonStats.Defense,
                            PokemonStats.SpecialAttack, PokemonStats.SpecialDefense,
                            PokemonStats.Speed, PokemonStats.Accuracy, PokemonStats.Evasion
                        }
                        ),
                }
                ) },

        // Compound Eyes
        {"compoundeyes",
            new AbilityData(
                ID: "compoundeyes",
                abilityName: "Compound Eyes",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Accuracy, 4f/3 }
                            })
                    )
                }
                ) },

        // Contrary
        {"contrary",
            new AbilityData(
                ID: "contrary",
                abilityName: "Contrary",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Contrary(),
                }
                ) },

        // Corrosion
        {"corrosion",
            new AbilityData(
                ID: "corrosion",
                abilityName: "Corrosion",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Corrosion(
                        statuses: new string[] { "poison" }
                        ),
                }
                ) },

        // Cotton Down
        {"cottondown",
            new AbilityData(
                ID: "cottondown",
                abilityName: "Cotton Down",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Gooey(
                        statStageMod: new Effects.General.StatStageMod(SPEMod: -1),
                        cottonDown: true,
                        onlyDamaging: true
                        ),
                }
                ) },

        // Cursed Body
        {"cursedbody",
            new AbilityData(
                ID: "cursedbody",
                abilityName: "Cursed Body",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlameBody(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "disable"
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        },
                        chance: 0.3f
                        ),
                }
                ) },

        // Cute Charm
        {"cutecharm",
            new AbilityData(
                ID: "cutecharm",
                abilityName: "Cute Charm",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlameBody(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "infatuation"
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        },
                        chance: 0.3f
                        ),
                }
                ) },

        // Damp
        {"damp",
            new AbilityData(
                ID: "damp",
                abilityName: "Damp",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Damp(moveTags: new MoveTag[] { MoveTag.ExplosiveMove }),
                }
                ) },

        // Dancer
        {"dancer",
            new AbilityData(
                ID: "dancer",
                abilityName: "Dancer",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Dancer(moveTags: new MoveTag[] { MoveTag.DanceMove }),
                }
                ) },

        // Dark Aura
        {"darkaura",
            new AbilityData(
                ID: "darkaura",
                abilityName: "Dark Aura",
                tags: new AbilityTag[]
                {

                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.DarkAura(
                        damageMultiplier: 4f/3,
                        moveTypes: new string[]{ "dark" }),
                }
                ) },

        // Dauntless Shield
        {"dauntlessshield",
            new AbilityData(
                ID: "dauntlessshield",
                abilityName: "Dauntless Shield",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IntrepidSword(
                        statStageMod: new Effects.General.StatStageMod(DEFMod: 1)
                        ),
                }
                ) },

        // Dazzling
        {"dazzling",
            new AbilityData(
                ID: "dazzling",
                abilityName: "Dazzling",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.QueenlyMajesty(),
                }
                ) },

        // Defeatist
        {"defeatist",
            new AbilityData(
                ID: "defeatist",
                abilityName: "Defeatist",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.CompoundEyes(
                        defeatistThreshold: 0.5f,
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Attack, 0.5f },
                                { PokemonStats.SpecialAttack, 0.5f }
                            })
                    )
                }
                ) },

        // Defiant
        {"defiant",
            new AbilityData(
                ID: "defiant",
                abilityName: "Defiant",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Defiant(
                        statStageMod: new Effects.General.StatStageMod(ATKMod: 2),
                        lowerTriggers: new PokemonStats[]
                        {
                            PokemonStats.Attack, PokemonStats.Defense,
                            PokemonStats.SpecialAttack, PokemonStats.SpecialDefense,
                            PokemonStats.Speed, PokemonStats.Accuracy, PokemonStats.Evasion
                        }
                        ),
                }
                ) },

        // Delta Stream
        {"deltastream",
            new AbilityData(
                ID: "deltastream",
                abilityName: "Delta Stream",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        desolateLand: true,
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "strongwinds"
                            )
                        ),
                }
                ) },

        // Desolate Land
        {"desolateland",
            new AbilityData(
                ID: "desolateland",
                abilityName: "Desolate Land",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        desolateLand: true,
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "extremelyharshsunlight"
                            )
                        ),
                }
                ) },

        // Disguise
        {"disguise",
            new AbilityData(
                ID: "disguise",
                abilityName: "Disguise",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay, AbilityTag.CannotRolePlayUser,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress,
                    AbilityTag.CannotWorrySeed,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Disguise(
                        disguiseForms: new Effects.General.FormTransformation[]
                        {
                            new Effects.General.FormTransformation(
                                preForm: "mimikyu", toForm: "mimikyu-busted"),
                        },
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Physical, MoveCategory.Special }
                                )
                        }
                        ),
                }
                ) },

        // Download
        {"download",
            new AbilityData(
                ID: "download",
                abilityName: "Download",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Download(
                        downloadComparisons: new Effects.Abilities.Download.DownloadCompare[]
                        {
                            new Effects.Abilities.Download.DownloadCompare(
                                statStageMod1: new Effects.General.StatStageMod(ATKMod: 1),
                                statStageMod2: new Effects.General.StatStageMod(SPAMod: 1),
                                stats1: PokemonStats.Defense,
                                stats2: PokemonStats.SpecialDefense
                                )
                        }
                        ),
                }
                ) },

        // Drizzle
        {"drizzle",
            new AbilityData(
                ID: "drizzle",
                abilityName: "Drizzle",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "rain"
                            )
                        ),
                }
                ) },

        // Drought
        {"drought",
            new AbilityData(
                ID: "drought",
                abilityName: "Drought",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "harshsunlight"
                            )
                        ),
                }
                ) },

        // Dry Skin
        {"dryskin",
            new AbilityData(
                ID: "dryskin",
                abilityName: "Dry Skin",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.DrySkin(
                        conditions: new Effects.Abilities.DrySkin.DrySkinCondition[]
                        {
                            new Effects.Abilities.DrySkin.DrySkinCondition(
                                conditions: new string[] { "rain" },
                                hpGainPercent: 1f/8
                                ),
                            new Effects.Abilities.DrySkin.DrySkinCondition(
                                conditions: new string[] { "harshsunlight" },
                                hpLosePercent: 1f/8
                                ),
                        }
                        ),

                    new Effects.Abilities.VoltAbsorb(
                        conditions: new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "water" },
                                absorbPercent: 0.25f
                                ),
                        }),
                    new Effects.Abilities.IceScales(
                        damageModifier: 1.25f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fire" }
                                )
                        }
                    ),
                }
                ) },

        // Early Bird
        {"earlybird",
            new AbilityData(
                ID: "earlybird",
                abilityName: "Early Bird",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.EarlyBird(
                        conditions: new string[] { "sleep" },
                        turnModifier: 0.5f),
                }
                ) },

        // Effect Spore
        {"effectspore",
            new AbilityData(
                ID: "effectspore",
                abilityName: "Effect Spore",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlameBody(
                        effectSpores: new Effects.Abilities.FlameBody.EffectSporeCondition[]
                        {
                            new Effects.Abilities.FlameBody.EffectSporeCondition(
                                inflictStatus: new Effects.General.InflictStatus(
                                    statusType: StatusType.Pokemon,
                                    statusID: "poison"
                                    ),
                                chance: 1f / 3
                                ),
                            new Effects.Abilities.FlameBody.EffectSporeCondition(
                                inflictStatus: new Effects.General.InflictStatus(
                                    statusType: StatusType.Pokemon,
                                    statusID: "paralysis"
                                    ),
                                chance: 1f / 3
                                ),
                            new Effects.Abilities.FlameBody.EffectSporeCondition(
                                inflictStatus: new Effects.General.InflictStatus(
                                    statusType: StatusType.Pokemon,
                                    statusID: "sleep"
                                    ),
                                chance: 1f / 3
                                ),
                        },
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        },
                        chance: 0.3f
                        ),
                }
                ) },

        // Electric Surge
        {"electricsurge",
            new AbilityData(
                ID: "electricsurge",
                abilityName: "Electric Surge",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "electricterrain"
                            )
                        ),
                }
                ) },

        // Emergency Exit
        {"emergencyexit",
            new AbilityData(
                ID: "emergencyexit",
                abilityName: "Emergency Exit",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.WimpOut(hpThreshold: 0.5f),
                }
                ) },

        // Fairy Aura
        {"fairyaura",
            new AbilityData(
                ID: "fairyaura",
                abilityName: "Fairy Aura",
                tags: new AbilityTag[]
                {

                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.DarkAura(
                        damageMultiplier: 4f/3,
                        moveTypes: new string[]{ "fairy" }),
                }
                ) },

        // Filter
        {"filter",
            new AbilityData(
                ID: "filter",
                abilityName: "Filter",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SolidRock(
                        superEffectiveModifier: 0.75f),
                }
                ) },

        // Flame Body
        {"flamebody",
            new AbilityData(
                ID: "flamebody",
                abilityName: "Flame Body",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlameBody(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "burn"
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        },
                        chance: 0.3f
                        ),
                }
                ) },

        // Flare Boost
        {"flareboost",
            new AbilityData(
                ID: "flareboost",
                abilityName: "Flare Boost",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlareBoost(
                        powerMultiplier: 1.5f,
                        conditionCheck: new Effects.Filter.Harvest(
                            conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn" }
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Special })
                        }
                    )
                }
                ) },

        // Flash Fire
        {"flashfire",
            new AbilityData(
                ID: "flashfire",
                abilityName: "Flash Fire",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.VoltAbsorb(
                        conditions: new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "fire" },
                                flashFireBoost: 1.5f
                                ),
                        }),
                }
                ) },

        // Flower Gift
        {"flowergift",
            new AbilityData(
                ID: "flowergift",
                abilityName: "Flower Gift",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Forecast(
                        transformations: new Effects.Abilities.Forecast.ForecastTransformation[]
                        {
                            // Sunshine Cherrim
                            new Effects.Abilities.Forecast.ForecastTransformation(
                                conditions: new string[] { "harshsunlight" },
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "cherrim",
                                    toForm: "cherrim-sunshine"
                                    )
                                ),
                        },
                        defaultTransformation: new Effects.General.FormTransformation(
                            preForm: "cherrim-sunshine",
                            toForm: "cherrim"
                            )
                        ),
                    new Effects.Abilities.SwiftSwim(
                        conditions: new List<Effects.Abilities.SwiftSwim.SwiftSwimCondition>
                        {
                            new Effects.Abilities.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "harshsunlight" },
                                flowerGift: true,
                                statScale: new Effects.General.StatScale(
                                    scaleMap: new Dictionary<PokemonStats, float>
                                    {
                                        { PokemonStats.Attack, 1.5f },
                                        { PokemonStats.SpecialDefense, 1.5f }
                                    })
                                )
                        })
                }
                ) },

        // Fluffy
        {"fluffy",
            new AbilityData(
                ID: "fluffy",
                abilityName: "Fluffy",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IceScales(
                        damageModifier: 0.5f,
                        tags: new MoveTag[] { MoveTag.MakesContact, }
                    ),
                    new Effects.Abilities.IceScales(
                        damageModifier: 2f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fire", })
                        }
                    )
                }
                ) },

        // Forecast
        {"forecast",
            new AbilityData(
                ID: "forecast",
                abilityName: "Forecast",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    // Castform
                    new Effects.Abilities.Forecast(
                        transformations: new Effects.Abilities.Forecast.ForecastTransformation[]
                        {
                            // Sunny
                            new Effects.Abilities.Forecast.ForecastTransformation(
                                conditions: new string[] { "harshsunlight" },
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "castform", toForm: "castform-sunny"
                                    )
                                ),
                            // Rainy
                            new Effects.Abilities.Forecast.ForecastTransformation(
                                conditions: new string[] { "rain" },
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "castform", toForm: "castform-rainy"
                                    )
                                ),
                            // Snowy
                            new Effects.Abilities.Forecast.ForecastTransformation(
                                conditions: new string[] { "hail" },
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "castform", toForm: "castform-snowy"
                                    )
                                ),
                        },
                        defaultTransformation: new Effects.General.FormTransformation(
                            preForms: new string[] { "castform-sunny", "castform-rainy", "castform-snowy" },
                            toForm: "castform"
                            )
                        ),
                }
                ) },

        // Forewarn
        {"forewarn",
            new AbilityData(
                ID: "forewarn",
                abilityName: "Forewarn",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Forewarn(),
                }
                ) },

        // Friend Guard
        {"friendguard",
            new AbilityData(
                ID: "friendguard",
                abilityName: "Friend Guard",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FriendGuard(
                        damageModifier: 0.75f),
                }
                ) },

        // Frisk
        {"frisk",
            new AbilityData(
                ID: "frisk",
                abilityName: "Frisk",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Frisk(),
                }
                ) },

        // Full Metal Body
        {"fullmetalbody",
            new AbilityData(
                ID: "fullmetalbody",
                abilityName: "Full Metal Body",
                tags: new AbilityTag[]
                {
                    AbilityTag.BypassMoldBreaker,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HyperCutter(
                        clearBody: true,
                        displayText: "ability-clearbody"),
                }
                ) },

        // Fur Coat
        {"furcoat",
            new AbilityData(
                ID: "furcoat",
                abilityName: "Fur Coat",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Defense, 2f },
                            })
                    )
                }
                ) },

        // Gale Wings
        {"galewings",
            new AbilityData(
                ID: "galewings",
                abilityName: "Gale Wings",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.GaleWings(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "flying" }
                                )
                        }
                        )
                }
                ) },

        // Galvanize
        {"galvanize",
            new AbilityData(
                ID: "galvanize",
                abilityName: "Galvanize",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Aerilate(
                        toMoveType: "electric",
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "normal" }
                                ),
                        }
                        ),
                }
                ) },

        // Gluttony
        {"gluttony",
            new AbilityData(
                ID: "gluttony",
                abilityName: "Gluttony",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Gluttony(),
                }
                ) },

        // Gooey
        {"gooey",
            new AbilityData(
                ID: "gooey",
                abilityName: "Gooey",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Gooey(
                        statStageMod: new Effects.General.StatStageMod(SPEMod: -1),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact, }
                        ),
                }
                ) },

        // Gorilla Tactics
        {"gorillatactics",
            new AbilityData(
                ID: "gorillatactics",
                abilityName: "Gorilla Tactics",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.GorillaTactics(),
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Attack, 1.5f }
                            })
                    )
                }
                ) },

        // Grass Pelt
        {"grasspelt",
            new AbilityData(
                ID: "grasspelt",
                abilityName: "Grass Pelt",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SwiftSwim(
                        conditions: new List<Effects.Abilities.SwiftSwim.SwiftSwimCondition>
                        {
                            new Effects.Abilities.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "grassyterrain" },
                                statScale: new Effects.General.StatScale(
                                    scaleMap: new Dictionary<PokemonStats, float>
                                    {
                                        { PokemonStats.Defense, 1.5f }
                                    })
                                )
                        })
                }
                ) },

        // Grassy Surge
        {"grassysurge",
            new AbilityData(
                ID: "grassysurge",
                abilityName: "Grassy Surge",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "grassyterrain"
                            )
                        ),
                }
                ) },

        // Gulp Missile
        {"gulpmissile",
            new AbilityData(
                ID: "gulpmissile",
                abilityName: "Gulp Missile",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay, AbilityTag.CannotRolePlayUser,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress,
                    AbilityTag.CannotWorrySeed,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.GulpMissile(
                        gulpTransformations: new Effects.Abilities.GulpMissile.GulpTransformation[]
                        {
                            // Gulping (Arrokuda)
                            new Effects.Abilities.GulpMissile.GulpTransformation(
                                moves: new string[] { "surf", "dive" },
                                missiles: new Effects.Abilities.GulpMissile.Missile[]
                                {
                                    new Effects.Abilities.GulpMissile.Missile(
                                        hpThreshold: 1f, hpLossPercent: 0.25f,
                                        statStageMod: new Effects.General.StatStageMod(DEFMod: -1)
                                        )
                                },
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "cramorant", toForm: "cramorant-gulping"
                                    )
                                ),
                            // Gorging (Pikachu)
                            new Effects.Abilities.GulpMissile.GulpTransformation(
                                moves: new string[] { "surf", "dive" },
                                missiles: new Effects.Abilities.GulpMissile.Missile[]
                                {
                                    new Effects.Abilities.GulpMissile.Missile(
                                        hpThreshold: 0.5f, hpLossPercent: 0.25f,
                                        inflictStatus: new Effects.General.InflictStatus(
                                            statusType: StatusType.Pokemon,
                                            statusID: "paralysis"
                                            )
                                        )
                                },
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "cramorant", toForm: "cramorant-gorging"
                                    )
                                ),
                        },
                        spitUpTransformations: new Effects.General.FormTransformation[]
                        {
                            new Effects.General.FormTransformation(
                                preForms: new string[] { "cramorant-gulping", "cramorant-gorging" },
                                toForm: "cramorant"
                                )
                        }

                        ),
                }
                ) },

        // Guts
        {"guts",
            new AbilityData(
                ID: "guts",
                abilityName: "Guts",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Guts(
                        conditionCheck: new Effects.Filter.Harvest(
                            conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn", "paralysis", "poison", "sleep" }
                            ),
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Attack, 1.5f },
                            })
                    )
                }
                ) },

        // Harvest
        {"harvest",
            new AbilityData(
                ID: "harvest",
                abilityName: "Harvest",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    // 50% base chance
                    new Effects.Abilities.Harvest(
                        chance: 0.5f,
                        pockets: new ItemPocket[] { ItemPocket.Berries }
                        ),
                    // Always works during sunlight
                    new Effects.Abilities.Harvest(
                        chance: 1f,
                        pockets: new ItemPocket[] { ItemPocket.Berries },
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.Harvest(
                                conditionType: Effects.Filter.Harvest.ConditionType.Battle,
                                conditions: new string[] { "harshsunlight" }
                                )
                        }
                        )
                }
                ) },

        // Healer
        {"healer",
            new AbilityData(
                ID: "healer",
                abilityName: "Healer",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Hydration(
                        chance: 0.3f,
                        healSelf: false, healer: true,
                        statusTypes: new PokemonSEType[] { PokemonSEType.NonVolatile }
                        ),
                }
                ) },

        // Heatproof
        {"heatproof",
            new AbilityData(
                ID: "heatproof",
                abilityName: "Heatproof",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IceScales(
                        damageModifier: 0.5f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fire", })
                        }
                    )
                }
                ) },
        
        // Heavy Metal
        {"heavymetal",
            new AbilityData(
                ID: "heavymetal",
                abilityName: "Heavy Metal",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HeavyMetal(),
                }
                ) },

        // Huge Power
        {"hugepower",
            new AbilityData(
                ID: "hugepower",
                abilityName: "Huge Power",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Attack, 2f },
                            })
                    )
                }
                ) },

        // Hunger Switch
        {"hungerswitch",
            new AbilityData(
                ID: "hungerswitch",
                abilityName: "Hunger Switch",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HungerSwitch(
                        pokemonID1: "morpeko", pokemonID2: "morpeko-hangry"
                        ),
                }
                ) },

        // Hustle
        {"hustle",
            new AbilityData(
                ID: "hustle",
                abilityName: "Hustle",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Attack, 1.5f }
                            })
                    ),
                    new Effects.Abilities.Hustle(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Accuracy, 0.8f }
                            }),
                        moveCategories: new MoveCategory[] { MoveCategory.Physical }
                    )
                }
                ) },

        // Hydration
        {"hydration",
            new AbilityData(
                ID: "hydration",
                abilityName: "Hydration",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Hydration(
                        chance: 1f,
                        statusTypes: new PokemonSEType[] { PokemonSEType.NonVolatile },
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.Harvest(
                                conditionType: Effects.Filter.Harvest.ConditionType.Battle,
                                conditions: new string[] { "rain" }
                                )
                        }
                        ),
                }
                ) },

        // Hyper Cutter
        {"hypercutter",
            new AbilityData(
                ID: "hypercutter",
                abilityName: "Hyper Cutter",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HyperCutter(
                        affectedStats: new PokemonStats[] { PokemonStats.Attack, }
                        )
                }
                ) },

        // Ice Body
        {"icebody",
            new AbilityData(
                ID: "icebody",
                abilityName: "Ice Body",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.DrySkin(
                        conditions: new Effects.Abilities.DrySkin.DrySkinCondition[]
                        {
                            new Effects.Abilities.DrySkin.DrySkinCondition(
                                conditions: new string[] { "hail" },
                                hpGainPercent: 1f/16
                                ),
                        }
                        ),
                }
                ) },

        // Ice Face
        {"iceface",
            new AbilityData(
                ID: "iceface",
                abilityName: "Ice Face",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay, AbilityTag.CannotRolePlayUser,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress,
                    AbilityTag.CannotWorrySeed,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Disguise(
                        disguiseForms: new Effects.General.FormTransformation[]
                        {
                            new Effects.General.FormTransformation(
                                preForm: "eiscue", toForm: "eiscue-noice"),
                        },
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Physical }
                                )
                        }
                        ),
                    new Effects.Abilities.Forecast(
                        transformations: new Effects.Abilities.Forecast.ForecastTransformation[]
                        {
                            new Effects.Abilities.Forecast.ForecastTransformation(
                                conditions: new string[] { "hail" },
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "eiscue-noice", toForm: "eiscue"
                                    )
                                ),
                        }
                        ),
                }
                ) },

        // Ice Scales
        {"icescales",
            new AbilityData(
                ID: "icescales",
                abilityName: "Ice Scales",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IceScales(
                        damageModifier: 0.5f,
                        useCategory: true, category: MoveCategory.Special
                    )
                }
                ) },

        // Illusion
        {"illusion",
            new AbilityData(
                ID: "illusion",
                abilityName: "Illusion",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlayUser,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Illusion(),
                }
                ) },

        // Immunity
        {"immunity",
            new AbilityData(
                ID: "immunity",
                abilityName: "Immunity",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        conditions: new string[] { "poison" }
                        ),
                }
                ) },

        // Infiltrator
        {"infiltrator",
            new AbilityData(
                ID: "infiltrator",
                abilityName: "Infiltrator",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Infiltrator(),
                }
                ) },

        // Innards Out
        {"innardsout",
            new AbilityData(
                ID: "innardsout",
                abilityName: "Innards Out",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Aftermath(
                        damage: new Effects.General.Damage(
                            mode: Effects.General.Damage.DamageMode.InnardsOut,
                            value: 1f
                            ),
                        onlyContact: false,
                        blockedByDamp: false
                        ),
                }
                ) },

        // Inner Focus
        {"innerfocus",
            new AbilityData(
                ID: "innerfocus",
                abilityName: "Inner Focus",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Oblivious(
                        effectsBlocked: new PokemonSEType[]{ PokemonSEType.Flinch }
                        ),
                    new Effects.Abilities.IntimidateBlock(
                        abilitiesBlocked: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Insomnia
        {"insomnia",
            new AbilityData(
                ID: "insomnia",
                abilityName: "Insomnia",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        conditions: new string[] { "sleep" }
                        ),
                }
                ) },

        // Intimidate
        {"intimidate",
            new AbilityData(
                ID: "intimidate",
                abilityName: "Intimidate",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Intimidate(
                        statStageMod: new Effects.General.StatStageMod(ATKMod: -1)
                        ),
                }
                ) },

        // Intrepid Sword
        {"intrepidsword",
            new AbilityData(
                ID: "intrepidsword",
                abilityName: "Intrepid Sword",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IntrepidSword(
                        statStageMod: new Effects.General.StatStageMod(ATKMod: 1)
                        ),
                }
                ) },

        // Iron Barbs
        {"ironbarbs",
            new AbilityData(
                ID: "ironbarbs",
                abilityName: "Iron Barbs",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.RoughSkin(
                        damage: new Effects.General.Damage(
                            mode: Effects.General.Damage.DamageMode.MaxHPPercent,
                            value: 1f/8
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                ),
                        }
                        ),
                }
                ) },

        // Iron Fist
        {"ironfist",
            new AbilityData(
                ID: "ironfist",
                abilityName: "Iron Fist",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.2f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.PunchMove }
                                )
                        }
                        ),
                }
                ) },

        // Justified
        {"justified",
            new AbilityData(
                ID: "justified",
                abilityName: "Justified",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Justified(
                        statStageMod: new Effects.General.StatStageMod(ATKMod: 1),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "dark" }
                                ),
                        }
                        ),
                }
                ) },

        // Keen Eye
        {"keeneye",
            new AbilityData(
                ID: "keeneye",
                abilityName: "Keen Eye",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HyperCutter(
                        affectedStats: new PokemonStats[] { PokemonStats.Accuracy, }
                        ),
                    new Effects.Abilities.Unaware(
                        targetStatsIgnored: new PokemonStats[] { PokemonStats.Evasion },
                        attackerStatsIgnored: new PokemonStats[] { }
                        ),
                }
                ) },

        // Leaf Guard
        {"leafguard",
            new AbilityData(
                ID: "leafguard",
                abilityName: "Leaf Guard",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        statusTypes: new PokemonSEType[] { PokemonSEType.NonVolatile }
                        ),
                }
                ) },

        // Levitate
        {"levitate",
            new AbilityData(
                ID: "levitate",
                abilityName: "Levitate",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Levitate(),
                }
                ) },

        // Libero
        {"libero",
            new AbilityData(
                ID: "libero",
                abilityName: "Libero",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Protean(),
                }
                ) },

        // Light Metal
        {"lightmetal",
            new AbilityData(
                ID: "lightmetal",
                abilityName: "Light Metal",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HeavyMetal(weightMultiplier: 0.5f),
                }
                ) },

        // Lightning Rod
        {"lightningrod",
            new AbilityData(
                ID: "lightningrod",
                abilityName: "Lightning Rod",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.LightningRod(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "electric" }
                                ),
                        }),
                    new Effects.Abilities.VoltAbsorb(
                        conditions: new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "electric" },
                                motorDrive: new Effects.General.StatStageMod(SPAMod: 1)
                                ),
                        }),
                }
                ) },

        // Limber
        {"limber",
            new AbilityData(
                ID: "limber",
                abilityName: "Limber",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        conditions: new string[] { "paralysis" }
                        ),
                }
                ) },

        // Liquid Ooze
        {"liquidooze",
            new AbilityData(
                ID: "liquidooze",
                abilityName: "Liquid Ooze",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.LiquidOoze(),
                }
                ) },

        // Liquid Voice
        {"liquidvoice",
            new AbilityData(
                ID: "liquidvoice",
                abilityName: "Liquid Voice",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Aerilate(
                        toMoveType: "water",
                        powerMultiplier: 1f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.SoundMove }
                                ),
                        }
                        ),
                }
                ) },

        // Magic Bounce
        {"magicbounce",
            new AbilityData(
                ID: "magicbounce",
                abilityName: "Magic Bounce",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.MagicBounce(
                        magicCoat: new Effects.General.MagicCoat(),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MagicCoatSusceptible }
                                )
                        }
                        ),
                }
                ) },

        // Magic Guard
        {"magicguard",
            new AbilityData(
                ID: "magicguard",
                abilityName: "Magic Guard",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Overcoat(allWeather: true),
                    new Effects.Abilities.MagicGuard(),
                }
                ) },

        // Magician
        {"magician",
            new AbilityData(
                ID: "magician",
                abilityName: "Magician",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Magician(),
                }
                ) },

        // Magma Armor
        {"magmaarmor",
            new AbilityData(
                ID: "magmaarmor",
                abilityName: "Magma Armor",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        conditions: new string[] { "freeze" }
                        ),
                }
                ) },

        // Magnet Pull
        {"magnetpull",
            new AbilityData(
                ID: "magnetpull",
                abilityName: "Magnet Pull",
                tags: new AbilityTag[]
                {

                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.MagnetPull(
                        types: new string[] {"steel"},
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(types: new string[]{ "ghost" }, invert: true),
                        }
                        ),
                }
                ) },

        // Marvel Scale
        {"marvelscale",
            new AbilityData(
                ID: "marvelscale",
                abilityName: "Marvel Scale",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Guts(
                        conditionCheck: new Effects.Filter.Harvest(
                            conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn", "freeze", "paralysis", "poison", "sleep" }
                            ),
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Defense, 1.5f },
                            })
                    )
                }
                ) },

        // Mega Launcher
        {"megalauncher",
            new AbilityData(
                ID: "megalauncher",
                abilityName: "Mega Launcher",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.5f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.PulseMove }
                                )
                        }
                        ),
                }
                ) },

        // Merciless
        {"merciless",
            new AbilityData(
                ID: "merciless",
                abilityName: "Merciless",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SuperLuck(
                        alwaysCritical: true,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.Harvest(
                                conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                                conditions: new string[] { "poison" }
                                )
                        }
                        ),
                }
                ) },

        // Mimicry
        {"mimicry",
            new AbilityData(
                ID: "mimicry",
                abilityName: "Mimicry",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Mimicry(
                        conditions: new Effects.Abilities.Mimicry.MimicryCondition[]
                        {
                            new Effects.Abilities.Mimicry.MimicryCondition(
                                conditions: new string[] { "electricterrain" },
                                types: new string[] { "electric" }
                                ),
                            new Effects.Abilities.Mimicry.MimicryCondition(
                                conditions: new string[] { "grassyterrain" },
                                types: new string[] { "grass" }
                                ),
                            new Effects.Abilities.Mimicry.MimicryCondition(
                                conditions: new string[] { "mistyterrain" },
                                types: new string[] { "fairy" }
                                ),
                            new Effects.Abilities.Mimicry.MimicryCondition(
                                conditions: new string[] { "psychicterrain" },
                                types: new string[] { "psychic" }
                                ),
                        }
                        ),
                }
                ) },

        // Minus
        {"minus",
            new AbilityData(
                ID: "minus",
                abilityName: "Minus",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Minus(
                        allyAbilities: new string[] { "minus", "plus" },
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.SpecialAttack, 1.5f }
                            }
                            )
                        ),
                }
                ) },

        // Mirror Armor
        {"mirrorarmor",
            new AbilityData(
                ID: "mirrorarmor",
                abilityName: "Mirror Armor",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.MirrorArmor(
                        lowerTriggers: new PokemonStats[]
                        {
                            PokemonStats.Attack, PokemonStats.Defense,
                            PokemonStats.SpecialAttack, PokemonStats.SpecialDefense,
                            PokemonStats.Speed, PokemonStats.Accuracy, PokemonStats.Evasion
                        }
                        ),
                }
                ) },

        // Misty Surge
        {"mistysurge",
            new AbilityData(
                ID: "mistysurge",
                abilityName: "Misty Surge",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "mistyterrain"
                            )
                        ),
                }
                ) },

        // Mold Breaker
        {"moldbreaker",
            new AbilityData(
                ID: "moldbreaker",
                abilityName: "Mold Breaker",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.MoldBreaker(),
                }
                ) },

        // Moody
        {"moody",
            new AbilityData(
                ID: "moody",
                abilityName: "Moody",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Moody(
                        statStageMods1: new List<Effects.General.StatStageMod>
                        {
                            new Effects.General.StatStageMod(ATKMod: 2),
                            new Effects.General.StatStageMod(DEFMod: 2),
                            new Effects.General.StatStageMod(SPAMod: 2),
                            new Effects.General.StatStageMod(SPDMod: 2),
                            new Effects.General.StatStageMod(SPEMod: 2),
                        },
                        statStageMods2: new List<Effects.General.StatStageMod>
                        {
                            new Effects.General.StatStageMod(ATKMod: -1),
                            new Effects.General.StatStageMod(DEFMod: -1),
                            new Effects.General.StatStageMod(SPAMod: -1),
                            new Effects.General.StatStageMod(SPDMod: -1),
                            new Effects.General.StatStageMod(SPEMod: -1),
                        }
                        ),
                }
                ) },

        // Motor Drive
        {"motordrive",
            new AbilityData(
                ID: "motordrive",
                abilityName: "Motor Drive",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.VoltAbsorb(
                        conditions: new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "electric" },
                                motorDrive: new Effects.General.StatStageMod(SPEMod: 1)
                                ),
                        }),
                }
                ) },

        // Moxie
        {"moxie",
            new AbilityData(
                ID: "moxie",
                abilityName: "Moxie",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Moxie(
                        statStageMod: new Effects.General.StatStageMod(ATKMod: 1)
                        ),
                }
                ) },

        // Multitype
        {"multitype",
            new AbilityData(
                ID: "multitype",
                abilityName: "Multitype",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay, AbilityTag.CannotRolePlayUser,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress, AbilityTag.CannotNeutralize,
                    AbilityTag.CannotWorrySeed,
                },
                effects: new AbilityEffect[]
                {
                    new AbilityEffect(effectType: AbilityEffectType.Multitype)
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Multitype(),
                }
                ) },

        // Multiscale
        {"multiscale",
            new AbilityData(
                ID: "multiscale",
                abilityName: "Multiscale",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Multiscale(),
                }
                ) },

        // Mummy
        {"mummy",
            new AbilityData(
                ID: "mummy",
                abilityName: "Mummy",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Mummy(),
                }
                ) },

        // Natural Cure
        {"naturalcure",
            new AbilityData(
                ID: "naturalcure",
                abilityName: "Natural Cure",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.NaturalCure(
                        conditions: new Effects.Filter.Harvest[]
                        {
                            new Effects.Filter.Harvest(
                                conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                                conditions: new string[] { "burn", "freeze", "paralysis", "poison", "sleep" }
                                )
                        }
                        ),
                }
                ) },

        // Neuroforce
        {"neuroforce",
            new AbilityData(
                ID: "neuroforce",
                abilityName: "Neuroforce",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.TintedLens(
                        neuroforceModifier: 1.2f),
                }
                ) },

        // Neutralizing Gas
        {"neutralizinggas",
            new AbilityData(
                ID: "neutralizinggas",
                abilityName: "Neutralizing Gas",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotNeutralize,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.NeutralizingGas(),
                }
                ) },

        // No Guard
        {"noguard",
            new AbilityData(
                ID: "noguard",
                abilityName: "No Guard",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.NoGuard(),
                }
                ) },

        // Normalize
        {"normalize",
            new AbilityData(
                ID: "normalize",
                abilityName: "Normalize",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Aerilate(
                        toMoveType: "normal"
                        ),
                }
                ) },

        // Oblivious
        {"oblivious",
            new AbilityData(
                ID: "oblivious",
                abilityName: "Oblivious",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Oblivious(
                        effectsBlocked: new PokemonSEType[]{ PokemonSEType.Infatuation, PokemonSEType.Taunt }
                        ),
                    new Effects.Abilities.IntimidateBlock(
                        abilitiesBlocked: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Overcoat
        {"overcoat",
            new AbilityData(
                ID: "overcoat",
                abilityName: "Overcoat",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Overcoat(allWeather: true),
                    new Effects.Abilities.Cacophony(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.PowderMove }
                                )
                        }
                        ),
                }
                ) },

        // Overgrow
        {"overgrow",
            new AbilityData(
                ID: "overgrow",
                abilityName: "Overgrow",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.5f,
                        blazeThreshold: 1f/3,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "grass" }
                                )
                        }
                        ),
                }
                ) },

        // Own Tempo
        {"owntempo",
            new AbilityData(
                ID: "owntempo",
                abilityName: "Own Tempo",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Oblivious(
                        effectsBlocked: new PokemonSEType[]{ PokemonSEType.Confusion }
                        ),
                    new Effects.Abilities.IntimidateBlock(
                        abilitiesBlocked: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Parental Bond
        {"parentalbond",
            new AbilityData(
                ID: "parentalbond",
                abilityName: "Parental Bond",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ParentalBond(
                        bondedHits: new Effects.Abilities.ParentalBond.BondedHit[]
                        {
                            new Effects.Abilities.ParentalBond.BondedHit(damageModifier: 1f),
                            new Effects.Abilities.ParentalBond.BondedHit(damageModifier: 0.25f),
                        }
                        ),
                }
                ) },

        // Pastel Veil
        {"pastelveil",
            new AbilityData(
                ID: "pastelveil",
                abilityName: "Pastel Veil",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        pastelVeil: true,
                        conditions: new string[] { "poison" }
                        ),
                }
                ) },

        // Perish Body
        {"perishbody",
            new AbilityData(
                ID: "perishbody",
                abilityName: "Perish Body",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlameBody(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "perishsong"
                            ),
                        perishBody: true,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        }
                        ),
                }
                ) },

        // Pickpocket
        {"pickpocket",
            new AbilityData(
                ID: "pickpocket",
                abilityName: "Pickpocket",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Pickpocket(),
                }
                ) },

        // Pixilate
        {"pixilate",
            new AbilityData(
                ID: "pixilate",
                abilityName: "Pixilate",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Aerilate(
                        toMoveType: "fairy",
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "normal" }
                                ),
                        }
                        ),
                }
                ) },

        // Plus
        {"plus",
            new AbilityData(
                ID: "plus",
                abilityName: "Plus",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Minus(
                        allyAbilities: new string[] { "minus", "plus" },
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.SpecialAttack, 1.5f }
                            }
                            )
                        ),
                }
                ) },

        // Poison Heal
        {"poisonheal",
            new AbilityData(
                ID: "poisonheal",
                abilityName: "Poison Heal",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.PoisonHeal(
                        conditions: new Effects.Abilities.PoisonHeal.HealCondition[]
                        {
                            new Effects.Abilities.PoisonHeal.HealCondition(
                                heal: new Effects.General.HealHP(
                                    healMode: Effects.General.HealHP.HealMode.MaxHPPercent,
                                    healValue: 1f/8
                                    ),
                                conditions: new Effects.Filter.Harvest[]
                                {
                                    new Effects.Filter.Harvest(
                                        conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                                        conditions: new string[] { "poison" }
                                        )
                                }
                                ),
                        }
                        )
                }
                ) },

        // Poison Point
        {"poisonpoint",
            new AbilityData(
                ID: "poisonpoint",
                abilityName: "Poison Point",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlameBody(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "poison"
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        },
                        chance: 0.3f
                        ),
                }
                ) },

        // Poison Touch
        {"poisontouch",
            new AbilityData(
                ID: "poisontouch",
                abilityName: "Poison Touch",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.PoisonTouch(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "poison"
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        }
                        ),
                }
                ) },

        // Power Construct
        {"powerconstruct",
            new AbilityData(
                ID: "powerconstruct",
                abilityName: "Power Construct",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress,
                    AbilityTag.CannotWorrySeed,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ZenMode(
                        hpThreshold: 0.5f, checkBelow: true,
                        transformation: new Effects.General.FormTransformation(
                            preForms: new string[] { "zygarde", "zygarde-10" },
                            toForm: "zygarde-complete"
                            )
                        ),
                }
                ) },

        // Power Of Alchemy
        {"powerofalchemy",
            new AbilityData(
                ID: "powerofalchemy",
                abilityName: "Power Of Alchemy",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.PowerOfAlchemy(),
                }
                ) },

        // Power Spot
        {"powerspot",
            new AbilityData(
                ID: "powerspot",
                abilityName: "Power Spot",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Battery(),
                }
                ) },

        // Prankster
        {"prankster",
            new AbilityData(
                ID: "prankster",
                abilityName: "Prankster",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.GaleWings(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Status }
                                )
                        }
                        ),
                    new Effects.Abilities.Prankster(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Pokemon,
                                types: new string[] { "dark" }
                                ),
                            new Effects.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Status }
                                ),
                        }
                        )
                }
                ) },

        // Pressure
        {"pressure",
            new AbilityData(
                ID: "pressure",
                abilityName: "Pressure",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Pressure()
                }
                ) },

        // Primordial Sea
        {"primordialsea",
            new AbilityData(
                ID: "primordialsea",
                abilityName: "Primordial Sea",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        desolateLand: true,
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "heavyrain"
                            )
                        ),
                }
                ) },

        // Prism Armor
        {"prismarmor",
            new AbilityData(
                ID: "prismarmor",
                abilityName: "Prism Armor",
                tags: new AbilityTag[]
                {
                    AbilityTag.BypassMoldBreaker,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SolidRock(
                        superEffectiveModifier: 0.75f),
                }
                ) },

        // Propeller Tail
        {"propellertail",
            new AbilityData(
                ID: "propellertail",
                abilityName: "Propeller Tail",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.PropellerTail(),
                }
                ) },

        // Protean
        {"protean",
            new AbilityData(
                ID: "protean",
                abilityName: "Protean",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Protean(),
                }
                ) },

        // Psychic Surge
        {"psychicsurge",
            new AbilityData(
                ID: "psychicsurge",
                abilityName: "Psychic Surge",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "psychicterrain"
                            )
                        ),
                }
                ) },

        // Punk Rock
        {"punkrock",
            new AbilityData(
                ID: "punkrock",
                abilityName: "Punk Rock",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.3f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.SoundMove }
                                )
                        }
                        ),
                    new Effects.Abilities.IceScales(
                        damageModifier: 0.5f,
                        tags: new MoveTag[] { MoveTag.SoundMove, }
                        ),
                }
                ) },

        // Pure Power
        {"purepower",
            new AbilityData(
                ID: "purepower",
                abilityName: "Pure Power",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Attack, 2f },
                            })
                    )
                }
                ) },

        // Queenly Majesty
        {"queenlymajesty",
            new AbilityData(
                ID: "queenlymajesty",
                abilityName: "Queenly Majesty",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.QueenlyMajesty(),
                }
                ) },

        // Quick Draw
        {"quickdraw",
            new AbilityData(
                ID: "quickdraw",
                abilityName: "Quick Draw",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.QuickDraw(),
                }
                ) },

        // Quick Feet
        {"quickfeet",
            new AbilityData(
                ID: "quickfeet",
                abilityName: "Quick Feet",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Guts(
                        conditionCheck: new Effects.Filter.Harvest(
                            conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn", "freeze", "paralysis", "poison", "sleep" }
                            ),
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Speed, 1.5f },
                            })
                    )
                }
                ) },

        // Rain Dish
        {"raindish",
            new AbilityData(
                ID: "raindish",
                abilityName: "Rain Dish",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.DrySkin(
                        conditions: new Effects.Abilities.DrySkin.DrySkinCondition[]
                        {
                            new Effects.Abilities.DrySkin.DrySkinCondition(
                                conditions: new string[] { "rain" },
                                hpGainPercent: 1f/16
                                ),
                        }
                        ),
                }
                ) },

        // Rattled
        {"rattled",
            new AbilityData(
                ID: "rattled",
                abilityName: "Rattled",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Justified(
                        statStageMod: new Effects.General.StatStageMod(SPEMod: 1),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "bug", "dark", "ghost" }
                                ),
                        }
                        ),
                    new Effects.Abilities.IntimidateTrigger(
                        statStageMod: new Effects.General.StatStageMod(SPEMod: 1),
                        abilityTriggers: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Receiver
        {"receiver",
            new AbilityData(
                ID: "receiver",
                abilityName: "Receiver",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.PowerOfAlchemy(),
                }
                ) },

        // Reckless
        {"reckless",
            new AbilityData(
                ID: "reckless",
                abilityName: "Reckless",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.2f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveEffects: new MoveEffectType[] { MoveEffectType.JumpKick, MoveEffectType.Recoil, }
                                )
                        }
                        ),
                }
                ) },

        // Refrigerate
        {"refrigerate",
            new AbilityData(
                ID: "refrigerate",
                abilityName: "Refrigerate",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Aerilate(
                        toMoveType: "ice",
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "normal" }
                                ),
                        }
                        ),
                }
                ) },

        // Regenerator
        {"regenerator",
            new AbilityData(
                ID: "regenerator",
                abilityName: "Regenerator",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.NaturalCure(
                        regenerator: new Effects.General.HealHP(
                            healMode: Effects.General.HealHP.HealMode.MaxHPPercent,
                            healValue: 1f/3
                            )
                        ),
                }
                ) },

        // Ripen
        {"ripen",
            new AbilityData(
                ID: "ripen",
                abilityName: "Ripen",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Ripen(
                        effectMultiplier: 2f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.ItemCheck(
                                pockets: new ItemPocket[] { ItemPocket.Berries }
                                )
                        }
                        ),
                }
                ) },

        // Rivalry
        {"rivalry",
            new AbilityData(
                ID: "rivalry",
                abilityName: "Rivalry",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Rivalry(),
                }
                ) },

        // RKS System
        {"rkssystem",
            new AbilityData(
                ID: "rkssystem",
                abilityName: "RKS System",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay, AbilityTag.CannotRolePlayUser,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress, AbilityTag.CannotNeutralize,
                    AbilityTag.CannotWorrySeed,
                },
                effects: new AbilityEffect[]
                {
                    new AbilityEffect(effectType: AbilityEffectType.RKSSystem)
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.RKSSystem(),
                }
                ) },

        // Rock Head
        {"rockhead",
            new AbilityData(
                ID: "rockhead",
                abilityName: "Rock Head",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.RockHead(),
                }
                ) },

        // Rough Skin
        {"roughskin",
            new AbilityData(
                ID: "roughskin",
                abilityName: "Rough Skin",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.RoughSkin(
                        damage: new Effects.General.Damage(
                            mode: Effects.General.Damage.DamageMode.MaxHPPercent,
                            value: 1f/8
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                ),
                        }
                        ),
                }
                ) },

        // Run Away
        {"runaway",
            new AbilityData(
                ID: "runaway",
                abilityName: "Run Away",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.RunAway(),
                }
                ) },

        // Sand Force
        {"sandforce",
            new AbilityData(
                ID: "sandforce",
                abilityName: "Sand Force",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Overcoat(
                        conditions: new string[] { "sandstorm" }
                        ),
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.3f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "ground", "rock", "steel", }
                                )
                        }
                        ),
                }
                ) },

        // Sand Rush
        {"sandrush",
            new AbilityData(
                ID: "sandrush",
                abilityName: "Sand Rush",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Overcoat(
                        conditions: new string[] { "sandstorm" }
                        ),
                    new Effects.Abilities.SwiftSwim(
                        conditions: new List<Effects.Abilities.SwiftSwim.SwiftSwimCondition>
                        {
                            new Effects.Abilities.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "sandstorm" },
                                statScale: new Effects.General.StatScale(
                                    scaleMap: new Dictionary<PokemonStats, float>
                                    {
                                        { PokemonStats.Speed, 2f }
                                    })
                                )
                        })
                }
                ) },

        // Sand Spit
        {"sandspit",
            new AbilityData(
                ID: "sandspit",
                abilityName: "Sand Spit",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlameBody(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "sandstorm"
                            )
                        ),
                }
                ) },

        // Sand Stream
        {"sandstream",
            new AbilityData(
                ID: "sandstream",
                abilityName: "Sand Stream",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "sandstorm"
                            )
                        ),
                }
                ) },

        // Sand Veil
        {"sandveil",
            new AbilityData(
                ID: "sandveil",
                abilityName: "Sand Veil",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Overcoat(
                        conditions: new string[] { "sandstorm" }
                        ),
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Evasion, 1.2f },
                            }),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.Harvest(
                                conditionType: Effects.Filter.Harvest.ConditionType.Battle,
                                conditions: new string[] { "sandstorm" }
                                )
                        }
                    )
                }
                ) },

        // Sap Sipper
        {"sapsipper",
            new AbilityData(
                ID: "sapsipper",
                abilityName: "Sap Sipper",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.VoltAbsorb(
                        conditions: new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "grass" },
                                motorDrive: new Effects.General.StatStageMod(ATKMod: 1)
                                ),
                        }),
                }
                ) },

        // Schooling
        {"schooling",
            new AbilityData(
                ID: "schooling",
                abilityName: "Schooling",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress,
                    AbilityTag.CannotWorrySeed,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ZenMode(
                        hpThreshold: 0.25f, checkBelow: false,
                        transformation: new Effects.General.FormTransformation(
                            preForm: "wishiwashi", toForm: "wishiwashi-school"
                            )
                        ),
                    new Effects.Abilities.ZenMode(
                        hpThreshold: 0.25f, checkBelow: true,
                        transformation: new Effects.General.FormTransformation(
                            preForm: "wishiwashi-school", toForm: "wishiwashi"
                            )
                        ),
                }
                ) },

        // Scrappy
        {"scrappy",
            new AbilityData(
                ID: "scrappy",
                abilityName: "Scrappy",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Scrappy(
                        bypassImmunities: new string[] { "ghost" }
                        ),
                    new Effects.Abilities.IntimidateBlock(
                        abilitiesBlocked: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Screen Cleaner
        {"screencleaner",
            new AbilityData(
                ID: "screencleaner",
                abilityName: "Screen Cleaner",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ScreenCleaner(),
                }
                ) },

        // Serene Grace
        {"serenegrace",
            new AbilityData(
                ID: "serenegrace",
                abilityName: "Serene Grace",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SereneGrace(),
                }
                ) },

        // Shadow Shield
        {"shadowshield",
            new AbilityData(
                ID: "shadowshield",
                abilityName: "Shadow Shield",
                tags: new AbilityTag[]
                {
                    AbilityTag.BypassMoldBreaker
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Multiscale(),
                }
                ) },

        // Shadow Tag
        {"shadowtag",
            new AbilityData(
                ID: "shadowtag",
                abilityName: "Shadow Tag",
                tags: new AbilityTag[]
                {

                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ShadowTag(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(types: new string[]{ "ghost" }, invert: true),
                        }
                        ),
                }
                ) },

        // Shed Skin
        {"shedskin",
            new AbilityData(
                ID: "shedskin",
                abilityName: "Shed Skin",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Hydration(
                        chance: 0.3f,
                        statusTypes: new PokemonSEType[] { PokemonSEType.NonVolatile }
                        ),
                }
                ) },

        // Sheer Force
        {"sheerforce",
            new AbilityData(
                ID: "sheerforce",
                abilityName: "Sheer Force",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SheerForce(),
                }
                ) },

        // Shell Armor
        {"shellarmor",
            new AbilityData(
                ID: "shellarmor",
                abilityName: "Shell Armor",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.BattleArmor(),
                }
                ) },

        // Shield Dust
        {"shielddust",
            new AbilityData(
                ID: "shielddust",
                abilityName: "Shield Dust",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ShieldDust(),
                }
                ) },

        // Shields Down
        {"shieldsdown",
            new AbilityData(
                ID: "shieldsdown",
                abilityName: "Shields Down",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress,
                    AbilityTag.CannotWorrySeed,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ZenMode(
                        hpThreshold: 0.5f, checkBelow: true,
                        transformation: new Effects.General.FormTransformation(
                            preForm: "minior", toForm: "minior-core"
                            )
                        ),
                    new Effects.Abilities.ZenMode(
                        hpThreshold: 0.5f, checkBelow: false,
                        transformation: new Effects.General.FormTransformation(
                            preForm: "minior-core", toForm: "minior"
                            )
                        ),
                    new Effects.Abilities.ShieldsDown(
                        meteorForms: new Effects.Abilities.ShieldsDown.MeteorForm[]
                        {
                            new Effects.Abilities.ShieldsDown.MeteorForm(
                                forms: new string[] { "minior" },
                                blockedStatuses: new Effects.Filter.Harvest[]
                                {
                                    new Effects.Filter.Harvest(
                                        conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                                        statusPKTypes: new PokemonSEType[] { PokemonSEType.NonVolatile }
                                        )
                                }
                                )
                        }
                        ),
                }
                ) },

        // Simple
        {"simple",
            new AbilityData(
                ID: "simple",
                abilityName: "Simple",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Simple(),
                }
                ) },

        // Skill Link
        {"skilllink",
            new AbilityData(
                ID: "skilllink",
                abilityName: "Skill Link",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SkillLink(),
                }
                ) },

        // Slow Start
        {"slowstart",
            new AbilityData(
                ID: "slowstart",
                abilityName: "Slow Start",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SlowStart(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Attack, 0.5f },
                                { PokemonStats.Speed, 0.5f }
                            })
                    )
                }
                ) },

        // Slush Rush
        {"slushrush",
            new AbilityData(
                ID: "slushrush",
                abilityName: "Slush Rush",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SwiftSwim(
                        conditions: new List<Effects.Abilities.SwiftSwim.SwiftSwimCondition>
                        {
                            new Effects.Abilities.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "hail" },
                                statScale: new Effects.General.StatScale(
                                    scaleMap: new Dictionary<PokemonStats, float>
                                    {
                                        { PokemonStats.Speed, 2f }
                                    })
                                )
                        })
                }
                ) },

        // Sniper
        {"sniper",
            new AbilityData(
                ID: "sniper",
                abilityName: "Sniper",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Sniper(),
                }
                ) },

        // Snow Cloak
        {"snowcloak",
            new AbilityData(
                ID: "snowcloak",
                abilityName: "Snow Cloak",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Overcoat(
                        conditions: new string[] { "hail" }
                        ),
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Evasion, 1.2f },
                            }),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.Harvest(
                                conditionType: Effects.Filter.Harvest.ConditionType.Battle,
                                conditions: new string[] { "hail" }
                                )
                        }
                    )
                }
                ) },

        // Snow Warning
        {"snowwarning",
            new AbilityData(
                ID: "snowwarning",
                abilityName: "Snow Warning",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Drought(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "hail"
                            )
                        ),
                }
                ) },

        // Solar Power
        {"solarpower",
            new AbilityData(
                ID: "solarpower",
                abilityName: "Solar Power",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.DrySkin(
                        conditions: new Effects.Abilities.DrySkin.DrySkinCondition[]
                        {
                            new Effects.Abilities.DrySkin.DrySkinCondition(
                                conditions: new string[] { "harshsunlight" },
                                hpLosePercent: 1f/8
                                ),
                        }
                        ),
                    new Effects.Abilities.CompoundEyes(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.SpecialAttack, 1.5f },
                            }),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.Harvest(
                                conditionType: Effects.Filter.Harvest.ConditionType.Battle,
                                conditions: new string[] { "harshsunlight" }
                                )
                        }
                    )
                }
                ) },

        // Solid Rock
        {"solidrock",
            new AbilityData(
                ID: "solidrock",
                abilityName: "Solid Rock",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SolidRock(
                        superEffectiveModifier: 0.75f),
                }
                ) },

        // Soundproof
        {"soundproof",
            new AbilityData(
                ID: "soundproof",
                abilityName: "Soundproof",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Cacophony(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.SoundMove }
                                )
                        }
                        ),
                }
                ) },

        // Soul-Heart
        {"soulheart",
            new AbilityData(
                ID: "soulheart",
                abilityName: "Soul-Heart",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SoulHeart(
                        statStageMod: new Effects.General.StatStageMod(SPAMod: 1)
                        ),
                }
                ) },

        // Speed Boost
        {"speedboost",
            new AbilityData(
                ID: "speedboost",
                abilityName: "Speed Boost",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SpeedBoost(
                        statStageMod: new Effects.General.StatStageMod(SPEMod: 1)
                        ),
                }
                ) },

        // Stakeout
        {"stakeout",
            new AbilityData(
                ID: "stakeout",
                abilityName: "Stakeout",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Stakeout(),
                }
                ) },

        // Stall
        {"stall",
            new AbilityData(
                ID: "stall",
                abilityName: "Stall",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Stall(),
                }
                ) },

        // Stalwart
        {"stalwart",
            new AbilityData(
                ID: "stalwart",
                abilityName: "Stalwart",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.PropellerTail(),
                }
                ) },

        // Stamina
        {"stamina",
            new AbilityData(
                ID: "stamina",
                abilityName: "Stamina",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Justified(
                        statStageMod: new Effects.General.StatStageMod(DEFMod: 1)
                        ),
                }
                ) },

        // Stance Change
        {"stancechange",
            new AbilityData(
                ID: "stancechange",
                abilityName: "Stance Change",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.StanceChange(
                        transformations: new Effects.Abilities.StanceChange.Transformation[]
                        {
                            // Blade
                            new Effects.Abilities.StanceChange.Transformation(
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "aegislash", toForm: "aegislash-blade"),
                                moveCheck: new Effects.Filter.MoveCheck(
                                    moveCategories: new MoveCategory[]
                                    {
                                        MoveCategory.Physical, MoveCategory.Special
                                    })
                                ),
                            // Shield
                            new Effects.Abilities.StanceChange.Transformation(
                                transformation: new Effects.General.FormTransformation(
                                    preForm: "aegislash-blade", toForm: "aegislash"),
                                moveCheck: new Effects.Filter.MoveCheck(
                                    specificMoveIDs: new string[] { "kingsshield" })
                                )
                        }
                        )
                }
                ) },

        // Static
        {"static",
            new AbilityData(
                ID: "static",
                abilityName: "Static",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlameBody(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "paralysis"
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        },
                        chance: 1f
                        ),
                }
                ) },

        // Steam Engine
        {"steamengine",
            new AbilityData(
                ID: "steamengine",
                abilityName: "Steam Engine",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Justified(
                        statStageMod: new Effects.General.StatStageMod(SPEMod: 6),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "water" }
                                ),
                        }
                        ),
                }
                ) },

        // Steelworker
        {"steelworker",
            new AbilityData(
                ID: "steelworker",
                abilityName: "Steelworker",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.5f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "steel" }
                                )
                        }
                        ),
                }
                ) },

        // Steely Spirit
        {"steelyspirit",
            new AbilityData(
                ID: "steelyspirit",
                abilityName: "Steely Spirit",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.5f,
                        steelySpirit: true,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "steel" }
                                )
                        }
                        ),
                }
                ) },

        // Stench
        {"stench",
            new AbilityData(
                ID: "stench",
                abilityName: "Stench",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.PoisonTouch(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        }
                        ),
                }
                ) },

        // Sticky Hold
        {"stickyhold",
            new AbilityData(
                ID: "stickyhold",
                abilityName: "Sticky Hold",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.StickyHold(),
                }
                ) },

        // Storm Drain
        {"stormdrain",
            new AbilityData(
                ID: "stormdrain",
                abilityName: "Storm Drain",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.LightningRod(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "water" }
                                ),
                        }),
                    new Effects.Abilities.VoltAbsorb(
                        conditions: new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "water" },
                                motorDrive: new Effects.General.StatStageMod(SPAMod: 1)
                                ),
                        }),
                }
                ) },

        // Strong Jaw
        {"strongjaw",
            new AbilityData(
                ID: "strongjaw",
                abilityName: "Strong Jaw",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.5f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.BiteMove }
                                )
                        }
                        ),
                }
                ) },

        // Sturdy
        {"sturdy",
            new AbilityData(
                ID: "sturdy",
                abilityName: "Sturdy",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Sturdy(),
                }
                ) },

        // Suction Cups
        {"suctioncups",
            new AbilityData(
                ID: "suctioncups",
                abilityName: "Suction Cups",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SuctionCups(),
                }
                ) },

        // Super Luck
        {"superluck",
            new AbilityData(
                ID: "superluck",
                abilityName: "Super Luck",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SuperLuck(),
                }
                ) },

        // Surge Surfer
        {"surgesurfer",
            new AbilityData(
                ID: "surgesurfer",
                abilityName: "Surge Surfer",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SwiftSwim(
                        conditions: new List<Effects.Abilities.SwiftSwim.SwiftSwimCondition>
                        {
                            new Effects.Abilities.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "electricterrain" },
                                statScale: new Effects.General.StatScale(
                                    scaleMap: new Dictionary<PokemonStats, float>
                                    {
                                        { PokemonStats.Speed, 2f }
                                    })
                                )
                        })
                }
                ) },

        // Swarm
        {"swarm",
            new AbilityData(
                ID: "swarm",
                abilityName: "Swarm",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.5f,
                        blazeThreshold: 1f/3,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "bug" }
                                )
                        }
                        ),
                }
                ) },

        // Sweet Veil
        {"sweetveil",
            new AbilityData(
                ID: "sweetveil",
                abilityName: "Sweet Veil",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        pastelVeil: true,
                        conditions: new string[] { "sleep" }
                        ),
                }
                ) },

        // Swift Swim
        {"swiftswim",
            new AbilityData(
                ID: "swiftswim",
                abilityName: "Swift Swim",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.SwiftSwim(
                        conditions: new List<Effects.Abilities.SwiftSwim.SwiftSwimCondition>
                        {
                            new Effects.Abilities.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "rain" },
                                statScale: new Effects.General.StatScale(
                                    scaleMap: new Dictionary<PokemonStats, float>
                                    {
                                        { PokemonStats.Speed, 2f }
                                    })
                                )
                        })
                }
                ) },

        // Symbiosis
        {"symbiosis",
            new AbilityData(
                ID: "symbiosis",
                abilityName: "Symbiosis",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Symbiosis(),
                }
                ) },

        // Synchronize
        {"synchronize",
            new AbilityData(
                ID: "synchronize",
                abilityName: "Synchronize",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Synchronize(
                        conditionCheck: new Effects.Filter.Harvest(
                            conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn", "paralysis", "poison" }
                            )),
                }
                ) },

        // Tangled Feet
        {"tangledfeet",
            new AbilityData(
                ID: "tangledfeet",
                abilityName: "Tangled Feet",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Guts(
                        conditionCheck: new Effects.Filter.Harvest(
                            conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                            statusPKTypes: new PokemonSEType[] { PokemonSEType.Confusion }
                            ),
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Evasion, 2f },
                            })
                    )
                }
                ) },

        // Tangling Hair
        {"tanglinghair",
            new AbilityData(
                ID: "tanglinghair",
                abilityName: "Tangling Hair",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Gooey(
                        statStageMod: new Effects.General.StatStageMod(SPEMod: -1),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact, }
                        ),
                }
                ) },

        // Technician
        {"technician",
            new AbilityData(
                ID: "technician",
                abilityName: "Technician",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Technician(),
                }
                ) },

        // Telepathy
        {"telepathy",
            new AbilityData(
                ID: "telepathy",
                abilityName: "Telepathy",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Telepathy(),
                }
                ) },

        // Teravolt
        {"teravolt",
            new AbilityData(
                ID: "teravolt",
                abilityName: "Teravolt",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.MoldBreaker(displayText: "ability-teravolt"),
                }
                ) },

        // Thick Fat
        {"thickfat",
            new AbilityData(
                ID: "thickfat",
                abilityName: "Thick Fat",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IceScales(
                        damageModifier: 0.5f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fire", "ice", })
                        }
                    )
                }
                ) },

        // Tinted Lens
        {"tintedlens",
            new AbilityData(
                ID: "tintedlens",
                abilityName: "Tinted Lens",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.TintedLens(
                        notVeryEffectiveModifier: 2f),
                }
                ) },

        // Torrent
        {"torrent",
            new AbilityData(
                ID: "torrent",
                abilityName: "Torrent",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.5f,
                        blazeThreshold: 1f/3,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "water" }
                                )
                        }
                        ),
                }
                ) },

        // Toxic Boost
        {"toxicboost",
            new AbilityData(
                ID: "toxicboost",
                abilityName: "Toxic Boost",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.FlareBoost(
                        powerMultiplier: 1.5f,
                        conditionCheck: new Effects.Filter.Harvest(
                            conditionType: Effects.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "poison" }
                            ),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Physical })
                        }
                    )
                }
                ) },

        // Trace
        {"trace",
            new AbilityData(
                ID: "trace",
                abilityName: "Trace",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Trace(),
                }
                ) },

        // Tough Claws
        {"toughclaws",
            new AbilityData(
                ID: "toughclaws",
                abilityName: "Tough Claws",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 1.3f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MakesContact }
                                )
                        }
                        ),
                }
                ) },

        // Triage
        {"triage",
            new AbilityData(
                ID: "triage",
                abilityName: "Triage",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.GaleWings(
                        priority: 3,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(healingOnly: true),
                        }
                        )
                }
                ) },

        // Truant
        {"truant",
            new AbilityData(
                ID: "truant",
                abilityName: "Truant",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Truant(),
                }
                ) },

        // Turboblaze
        {"turboblaze",
            new AbilityData(
                ID: "turboblaze",
                abilityName: "Turboblaze",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.MoldBreaker(displayText: "ability-turboblaze"),
                }
                ) },

        // Unaware
        {"unaware",
            new AbilityData(
                ID: "unaware",
                abilityName: "Unaware",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Unaware(),
                }
                ) },

        // Unburden
        {"unburden",
            new AbilityData(
                ID: "unburden",
                abilityName: "Unburden",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Unburden(
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Speed, 2f }
                            })
                    )
                }
                ) },

        // Unseen Fist
        {"unseenfist",
            new AbilityData(
                ID: "unseenfist",
                abilityName: "Unseen Fist",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.UnseenFist(),
                }
                ) },

        // Victory Star
        {"victorystar",
            new AbilityData(
                ID: "victorystar",
                abilityName: "Victory Star",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.CompoundEyes(
                        victoryStar: true,
                        statScale: new Effects.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Accuracy, 1.1f }
                            })
                    )
                }
                ) },

        // Vital Spirit
        {"vitalspirit",
            new AbilityData(
                ID: "vitalspirit",
                abilityName: "Vital Spirit",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        conditions: new string[] { "sleep" }
                        ),
                }
                ) },

        // Volt Absorb
        {"voltabsorb",
            new AbilityData(
                ID: "voltabsorb",
                abilityName: "Volt Absorb",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.VoltAbsorb(
                        conditions: new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "electric" },
                                absorbPercent: 0.25f
                                ),
                        }),
                }
                ) },

        // Wandering Spirit
        {"wanderingspirit",
            new AbilityData(
                ID: "wanderingspirit",
                abilityName: "Wandering Spirit",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Mummy(
                        wanderingSpirit: false,
                        displayText: "ability-wanderingspirit"
                        ),
                }
                ) },

        // Water Absorb
        {"waterabsorb",
            new AbilityData(
                ID: "waterabsorb",
                abilityName: "Water Absorb",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.VoltAbsorb(
                        conditions: new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new Effects.Abilities.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "water" },
                                absorbPercent: 0.25f
                                ),
                        }),
                }
                ) },

        // Water Bubble
        {"waterbubble",
            new AbilityData(
                ID: "waterbubble",
                abilityName: "Water Bubble",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.IronFist(
                        powerMultiplier: 2f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "water" }
                                )
                        }
                        ),
                    new Effects.Abilities.IceScales(
                        damageModifier: 0.5f,
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "fire", })
                        }
                        )
                }
                ) },

        // Water Compaction
        {"watercompaction",
            new AbilityData(
                ID: "watercompaction",
                abilityName: "Water Compaction",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Justified(
                        statStageMod: new Effects.General.StatStageMod(DEFMod: 2),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.TypeList(
                                targetType: Effects.Filter.TypeList.TargetType.Move,
                                types: new string[] { "water" }
                                ),
                        }
                        ),
                }
                ) },

        // Water Veil
        {"waterveil",
            new AbilityData(
                ID: "waterveil",
                abilityName: "Water Veil",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Limber(
                        conditions: new string[] { "burn" }
                        ),
                }
                ) },

        // Weak Armor
        {"weakarmor",
            new AbilityData(
                ID: "weakarmor",
                abilityName: "Weak Armor",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.Justified(
                        statStageMod: new Effects.General.StatStageMod(DEFMod: -1, SPEMod: 2),
                        mustMatchCategory: true,
                        category: MoveCategory.Physical
                        ),
                }
                ) },

        // White Smoke
        {"whitesmoke",
            new AbilityData(
                ID: "whitesmoke",
                abilityName: "White Smoke",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.HyperCutter(
                        clearBody: true,
                        displayText: "ability-clearbody"),
                }
                ) },

        // Wimp Out
        {"wimpout",
            new AbilityData(
                ID: "wimpout",
                abilityName: "Wimp Out",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.WimpOut(hpThreshold: 0.5f),
                }
                ) },

        // Wonder Guard
        {"wonderguard",
            new AbilityData(
                ID: "wonderguard",
                abilityName: "Wonder Guard",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.WonderGuard(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(damagingOnly: true),
                        }),
                }
                ) },

        // Wonder Skin
        {"wonderskin",
            new AbilityData(
                ID: "wonderskin",
                abilityName: "Wonder Skin",
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.WonderSkin(
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Status })
                        }
                        ),
                }
                ) },

        // Zen Mode
        {"zenmode",
            new AbilityData(
                ID: "zenmode",
                abilityName: "Zen Mode",
                tags: new AbilityTag[]
                {
                    AbilityTag.CannotRolePlay,
                    AbilityTag.CannotSkillSwap, AbilityTag.CannotSkillSwapUser,
                    AbilityTag.CannotSuppress,
                    AbilityTag.CannotWorrySeed,
                },
                effectsNew: new Effects.Abilities.AbilityEffect[]
                {
                    new Effects.Abilities.ZenMode(
                        hpThreshold: 0.5f, checkBelow: true,
                        transformation: new Effects.General.FormTransformation(
                            preForm: "darmanitan", toForm: "darmanitan-zen"
                            )
                        ),
                    new Effects.Abilities.ZenMode(
                        hpThreshold: 0.5f, checkBelow: true,
                        transformation: new Effects.General.FormTransformation(
                            preForm: "darmanitan-galar", toForm: "darmanitan-zen-galar"
                            )
                        ),
                }
                ) },


        // ---ACCURACY---

        

        // ---DAMAGE MULTIPLIERS---
        

        // ---FORM CHANGES---

        

        

        // ---ITEMS---

        {"klutz",
            new AbilityData(
                ID: "klutz",
                abilityName: "Klutz",
                tags: new AbilityTag[]
                {

                },
                effects: new AbilityEffect[]
                {
                    new AbilityEffect(effectType: AbilityEffectType.Klutz)
                }
                ) },



        {"unnerve",
            new AbilityData(
                ID: "unnerve",
                abilityName: "Unnerve",
                tags: new AbilityTag[]
                {

                },
                effects: new AbilityEffect[]
                {
                    new AbilityEffect(effectType: AbilityEffectType.Unnerve)
                }
                ) },

        // ---PROTECTION---

        
        

        // ---STAT STAGES---

        


        // ---TYPES---

        
        

        // ---MISC---

        

        

    };

        // Methods
        public AbilityData GetAbilityData(string ID)
        {
            if (database.ContainsKey(ID))
            {
                return database[ID];
            }
            Debug.LogWarning("Could not find ability with ID: " + ID);
            return database[""];
        }
    }
}
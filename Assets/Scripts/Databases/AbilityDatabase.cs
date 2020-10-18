using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDatabase
{
    //create an object of SingleObject
    private static AbilityDatabase singleton = new AbilityDatabase();

    //make the constructor private so that this class cannot be
    //instantiated
    private AbilityDatabase() { }

    //Get the only object available
    public static AbilityDatabase instance
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Adaptability(),
                }
                ) },

        // Aerilate
        {"aerilate",
            new AbilityData(
                ID: "aerilate",
                abilityName: "Aerilate",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Aerilate(
                        toMoveType: "flying",
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Aftermath(
                        damage: new EffectDatabase.General.Damage(
                            mode: EffectDatabase.General.Damage.DamageMode.MaxHPPercent,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.AirLock(),
                }
                ) },

        // Analytic
        {"analytic",
            new AbilityData(
                ID: "analytic",
                abilityName: "Analytic",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Analytic(),
                }
                ) },

        // Anger Point
        {"angerpoint",
            new AbilityData(
                ID: "angerpoint",
                abilityName: "Anger Point",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.AngerPoint(
                        statStageMod: new EffectDatabase.General.StatStageMod(maxATK: true)
                        ),
                }
                ) },

        // Anticipation
        {"anticipation",
            new AbilityData(
                ID: "anticipation",
                abilityName: "Anticipation",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Anticipation(),
                }
                ) },

        // Arena Trap
        {"arenatrap",
            new AbilityData(
                ID: "arenatrap",
                abilityName: "Arena Trap",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ShadowTag(
                        arenaTrap: true,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(types: new string[]{ "ghost" }, invert: true),
                        }
                        ),
                }
                ) },

        // Aroma Veil
        {"aromaveil",
            new AbilityData(
                ID: "aromaveil",
                abilityName: "Aroma Veil",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Oblivious(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.AuraBreak(),
                }
                ) },

        // Bad Dreams
        {"baddreams",
            new AbilityData(
                ID: "baddreams",
                abilityName: "Bad Dreams",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.BadDreams(),
                }
                ) },

        // Ball Fetch
        {"ballfetch",
            new AbilityData(
                ID: "ballfetch",
                abilityName: "Ball Fetch",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.BallFetch(),
                }
                ) },

        // Battery
        {"battery",
            new AbilityData(
                ID: "battery",
                abilityName: "Battery",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Battery(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.BattleArmor(),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.BattleBond(
                        transformations: new EffectDatabase.AbilityEff.BattleBond.BattleBondTransformation[]
                        {
                            new EffectDatabase.AbilityEff.BattleBond.BattleBondTransformation(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.BeastBoost(statMod: 1),
                }
                ) },

        // Berserk
        {"berserk",
            new AbilityData(
                ID: "berserk",
                abilityName: "Berserk",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Berserk(
                        hpThreshold: 0.5f,
                        statStageMod: new EffectDatabase.General.StatStageMod(SPAMod: 1)
                        ),
                }
                ) },

        // Big Pecks
        {"bigpecks",
            new AbilityData(
                ID: "bigpecks",
                abilityName: "Big Pecks",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HyperCutter(
                        affectedStats: new PokemonStats[] { PokemonStats.Defense, }
                        )
                }
                ) },

        // Blaze
        {"blaze",
            new AbilityData(
                ID: "blaze",
                abilityName: "Blaze",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.5f,
                        blazeThreshold: 1f/3,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Cacophony(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Cacophony(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.CheekPouch(),
                }
                ) },

        // Chlorophyll
        {"chlorophyll",
            new AbilityData(
                ID: "chlorophyll",
                abilityName: "Chlorophyll",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SwiftSwim(
                        conditions: new List<EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition>
                        {
                            new EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "harshsunlight" },
                                statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HyperCutter(
                        clearBody: true,
                        displayText: "ability-clearbody"),
                }
                ) },

        // Color Change
        {"colorchange",
            new AbilityData(
                ID: "colorchange",
                abilityName: "Color Change",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ColorChange(),
                }
                ) },

        // Comatose
        {"comatose",
            new AbilityData(
                ID: "comatose",
                abilityName: "Comatose",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Comatose(),
                }
                ) },

        // Competitive
        {"competitive",
            new AbilityData(
                ID: "competitive",
                abilityName: "Competitive",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Defiant(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPAMod: 2),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Contrary(),
                }
                ) },

        // Corrosion
        {"corrosion",
            new AbilityData(
                ID: "corrosion",
                abilityName: "Corrosion",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Corrosion(
                        statuses: new string[] { "poison" }
                        ),
                }
                ) },

        // Cotton Down
        {"cottondown",
            new AbilityData(
                ID: "cottondown",
                abilityName: "Cotton Down",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Gooey(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPEMod: -1),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlameBody(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "disable"
                            ),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact },
                        chance: 0.3f
                        ),
                }
                ) },

        // Cute Charm
        {"cutecharm",
            new AbilityData(
                ID: "cutecharm",
                abilityName: "Cute Charm",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlameBody(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "infatuation"
                            ),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact },
                        chance: 0.3f
                        ),
                }
                ) },

        // Damp
        {"damp",
            new AbilityData(
                ID: "damp",
                abilityName: "Damp",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Damp(moveTags: new MoveTag[] { MoveTag.ExplosiveMove }),
                }
                ) },

        // Dancer
        {"dancer",
            new AbilityData(
                ID: "dancer",
                abilityName: "Dancer",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Dancer(moveTags: new MoveTag[] { MoveTag.DanceMove }),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.DarkAura(
                        damageMultiplier: 4f/3,
                        moveTypes: new string[]{ "dark" }),
                }
                ) },

        // Dauntless Shield
        {"dauntlessshield",
            new AbilityData(
                ID: "dauntlessshield",
                abilityName: "Dauntless Shield",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IntrepidSword(
                        statStageMod: new EffectDatabase.General.StatStageMod(DEFMod: 1)
                        ),
                }
                ) },

        // Dazzling
        {"dazzling",
            new AbilityData(
                ID: "dazzling",
                abilityName: "Dazzling",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.QueenlyMajesty(),
                }
                ) },

        // Defeatist
        {"defeatist",
            new AbilityData(
                ID: "defeatist",
                abilityName: "Defeatist",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        defeatistThreshold: 0.5f,
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Defiant(
                        statStageMod: new EffectDatabase.General.StatStageMod(ATKMod: 2),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        desolateLand: true,
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        desolateLand: true,
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Disguise(
                        disguiseForms: new EffectDatabase.General.FormTransformation[]
                        {
                            new EffectDatabase.General.FormTransformation(
                                preForm: "mimikyu", toForm: "mimikyu-busted"),
                        },
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Download(
                        downloadComparisons: new EffectDatabase.AbilityEff.Download.DownloadCompare[]
                        {
                            new EffectDatabase.AbilityEff.Download.DownloadCompare(
                                statStageMod1: new EffectDatabase.General.StatStageMod(ATKMod: 1),
                                statStageMod2: new EffectDatabase.General.StatStageMod(SPAMod: 1),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.DrySkin(
                        conditions: new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition[]
                        {
                            new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition(
                                conditions: new string[] { "rain" },
                                hpGainPercent: 1f/8
                                ),
                            new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition(
                                conditions: new string[] { "harshsunlight" },
                                hpLosePercent: 1f/8
                                ),
                        }
                        ),

                    new EffectDatabase.AbilityEff.VoltAbsorb(
                        conditions: new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "water" },
                                absorbPercent: 0.25f
                                ),
                        }),
                    new EffectDatabase.AbilityEff.IceScales(
                        damageModifier: 1.25f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.EarlyBird(
                        conditions: new string[] { "sleep" },
                        turnModifier: 0.5f),
                }
                ) },

        // Effect Spore
        {"effectspore",
            new AbilityData(
                ID: "effectspore",
                abilityName: "Effect Spore",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlameBody(
                        effectSpores: new EffectDatabase.AbilityEff.FlameBody.EffectSporeCondition[]
                        {
                            new EffectDatabase.AbilityEff.FlameBody.EffectSporeCondition(
                                inflictStatus: new EffectDatabase.General.InflictStatus(
                                    statusType: StatusType.Pokemon,
                                    statusID: "poison"
                                    ),
                                chance: 1f / 3
                                ),
                            new EffectDatabase.AbilityEff.FlameBody.EffectSporeCondition(
                                inflictStatus: new EffectDatabase.General.InflictStatus(
                                    statusType: StatusType.Pokemon,
                                    statusID: "paralysis"
                                    ),
                                chance: 1f / 3
                                ),
                            new EffectDatabase.AbilityEff.FlameBody.EffectSporeCondition(
                                inflictStatus: new EffectDatabase.General.InflictStatus(
                                    statusType: StatusType.Pokemon,
                                    statusID: "sleep"
                                    ),
                                chance: 1f / 3
                                ),
                        },
                        triggerTags: new MoveTag[] { MoveTag.MakesContact },
                        chance: 0.3f
                        ),
                }
                ) },

        // Electric Surge
        {"electricsurge",
            new AbilityData(
                ID: "electricsurge",
                abilityName: "Electric Surge",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.WimpOut(hpThreshold: 0.5f),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.DarkAura(
                        damageMultiplier: 4f/3,
                        moveTypes: new string[]{ "fairy" }),
                }
                ) },

        // Filter
        {"filter",
            new AbilityData(
                ID: "filter",
                abilityName: "Filter",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SolidRock(
                        superEffectiveModifier: 0.75f),
                }
                ) },

        // Flame Body
        {"flamebody",
            new AbilityData(
                ID: "flamebody",
                abilityName: "Flame Body",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlameBody(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "burn"
                            ),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact },
                        chance: 0.3f
                        ),
                }
                ) },

        // Flare Boost
        {"flareboost",
            new AbilityData(
                ID: "flareboost",
                abilityName: "Flare Boost",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlareBoost(
                        powerMultiplier: 1.5f,
                        conditionCheck: new EffectDatabase.Filter.Harvest(
                            conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn" }
                            ),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.VoltAbsorb(
                        conditions: new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Forecast(
                        transformations: new EffectDatabase.AbilityEff.Forecast.ForecastTransformation[]
                        {
                            // Sunshine Cherrim
                            new EffectDatabase.AbilityEff.Forecast.ForecastTransformation(
                                conditions: new string[] { "harshsunlight" },
                                transformation: new EffectDatabase.General.FormTransformation(
                                    preForm: "cherrim",
                                    toForm: "cherrim-sunshine"
                                    )
                                ),
                        },
                        defaultTransformation: new EffectDatabase.General.FormTransformation(
                            preForm: "cherrim-sunshine",
                            toForm: "cherrim"
                            )
                        ),
                    new EffectDatabase.AbilityEff.SwiftSwim(
                        conditions: new List<EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition>
                        {
                            new EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "harshsunlight" },
                                flowerGift: true,
                                statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IceScales(
                        damageModifier: 0.5f,
                        tags: new MoveTag[] { MoveTag.MakesContact, }
                    ),
                    new EffectDatabase.AbilityEff.IceScales(
                        damageModifier: 2f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    // Castform
                    new EffectDatabase.AbilityEff.Forecast(
                        transformations: new EffectDatabase.AbilityEff.Forecast.ForecastTransformation[]
                        {
                            // Sunny
                            new EffectDatabase.AbilityEff.Forecast.ForecastTransformation(
                                conditions: new string[] { "harshsunlight" },
                                transformation: new EffectDatabase.General.FormTransformation(
                                    preForm: "castform", toForm: "castform-sunny"
                                    )
                                ),
                            // Rainy
                            new EffectDatabase.AbilityEff.Forecast.ForecastTransformation(
                                conditions: new string[] { "rain" },
                                transformation: new EffectDatabase.General.FormTransformation(
                                    preForm: "castform", toForm: "castform-rainy"
                                    )
                                ),
                            // Snowy
                            new EffectDatabase.AbilityEff.Forecast.ForecastTransformation(
                                conditions: new string[] { "hail" },
                                transformation: new EffectDatabase.General.FormTransformation(
                                    preForm: "castform", toForm: "castform-snowy"
                                    )
                                ),
                        },
                        defaultTransformation: new EffectDatabase.General.FormTransformation(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Forewarn(),
                }
                ) },

        // Friend Guard
        {"friendguard",
            new AbilityData(
                ID: "friendguard",
                abilityName: "Friend Guard",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FriendGuard(
                        damageModifier: 0.75f),
                }
                ) },

        // Frisk
        {"frisk",
            new AbilityData(
                ID: "frisk",
                abilityName: "Frisk",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Frisk(),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HyperCutter(
                        clearBody: true,
                        displayText: "ability-clearbody"),
                }
                ) },

        // Fur Coat
        {"furcoat",
            new AbilityData(
                ID: "furcoat",
                abilityName: "Fur Coat",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.GaleWings(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Aerilate(
                        toMoveType: "electric",
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Gluttony(),
                }
                ) },

        // Gooey
        {"gooey",
            new AbilityData(
                ID: "gooey",
                abilityName: "Gooey",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Gooey(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPEMod: -1),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact, }
                        ),
                }
                ) },

        // Gorilla Tactics
        {"gorillatactics",
            new AbilityData(
                ID: "gorillatactics",
                abilityName: "Gorilla Tactics",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.GorillaTactics(),
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SwiftSwim(
                        conditions: new List<EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition>
                        {
                            new EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "grassyterrain" },
                                statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.GulpMissile(
                        gulpTransformations: new EffectDatabase.AbilityEff.GulpMissile.GulpTransformation[]
                        {
                            // Gulping (Arrokuda)
                            new EffectDatabase.AbilityEff.GulpMissile.GulpTransformation(
                                moves: new string[] { "surf", "dive" },
                                missiles: new EffectDatabase.AbilityEff.GulpMissile.Missile[]
                                {
                                    new EffectDatabase.AbilityEff.GulpMissile.Missile(
                                        hpThreshold: 1f, hpLossPercent: 0.25f,
                                        statStageMod: new EffectDatabase.General.StatStageMod(DEFMod: -1)
                                        )
                                },
                                transformation: new EffectDatabase.General.FormTransformation(
                                    preForm: "cramorant", toForm: "cramorant-gulping"
                                    )
                                ),
                            // Gorging (Pikachu)
                            new EffectDatabase.AbilityEff.GulpMissile.GulpTransformation(
                                moves: new string[] { "surf", "dive" },
                                missiles: new EffectDatabase.AbilityEff.GulpMissile.Missile[]
                                {
                                    new EffectDatabase.AbilityEff.GulpMissile.Missile(
                                        hpThreshold: 0.5f, hpLossPercent: 0.25f,
                                        inflictStatus: new EffectDatabase.General.InflictStatus(
                                            statusType: StatusType.Pokemon,
                                            statusID: "paralysis"
                                            )
                                        )
                                },
                                transformation: new EffectDatabase.General.FormTransformation(
                                    preForm: "cramorant", toForm: "cramorant-gorging"
                                    )
                                ),
                        },
                        spitUpTransformations: new EffectDatabase.General.FormTransformation[]
                        {
                            new EffectDatabase.General.FormTransformation(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Guts(
                        conditionCheck: new EffectDatabase.Filter.Harvest(
                            conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn", "paralysis", "poison", "sleep" }
                            ),
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    // 50% base chance
                    new EffectDatabase.AbilityEff.Harvest(
                        chance: 0.5f,
                        pockets: new ItemPocket[] { ItemPocket.Berries }
                        ),
                    // Always works during sunlight
                    new EffectDatabase.AbilityEff.Harvest(
                        chance: 1f,
                        pockets: new ItemPocket[] { ItemPocket.Berries },
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.Harvest(
                                conditionType: EffectDatabase.Filter.Harvest.ConditionType.Battle,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Hydration(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IceScales(
                        damageModifier: 0.5f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HeavyMetal(),
                }
                ) },

        // Huge Power
        {"hugepower",
            new AbilityData(
                ID: "hugepower",
                abilityName: "Huge Power",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HungerSwitch(
                        pokemonID1: "morpeko", pokemonID2: "morpeko-hangry"
                        ),
                }
                ) },

        // Hustle
        {"hustle",
            new AbilityData(
                ID: "hustle",
                abilityName: "Hustle",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Attack, 1.5f }
                            })
                    ),
                    new EffectDatabase.AbilityEff.Hustle(
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Hydration(
                        chance: 1f,
                        statusTypes: new PokemonSEType[] { PokemonSEType.NonVolatile },
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.Harvest(
                                conditionType: EffectDatabase.Filter.Harvest.ConditionType.Battle,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HyperCutter(
                        affectedStats: new PokemonStats[] { PokemonStats.Attack, }
                        )
                }
                ) },

        // Ice Body
        {"icebody",
            new AbilityData(
                ID: "icebody",
                abilityName: "Ice Body",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.DrySkin(
                        conditions: new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition[]
                        {
                            new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Disguise(
                        disguiseForms: new EffectDatabase.General.FormTransformation[]
                        {
                            new EffectDatabase.General.FormTransformation(
                                preForm: "eiscue", toForm: "eiscue-noice"),
                        },
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Physical }
                                )
                        }
                        ),
                    new EffectDatabase.AbilityEff.Forecast(
                        transformations: new EffectDatabase.AbilityEff.Forecast.ForecastTransformation[]
                        {
                            new EffectDatabase.AbilityEff.Forecast.ForecastTransformation(
                                conditions: new string[] { "hail" },
                                transformation: new EffectDatabase.General.FormTransformation(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IceScales(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Illusion(),
                }
                ) },

        // Immunity
        {"immunity",
            new AbilityData(
                ID: "immunity",
                abilityName: "Immunity",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
                        conditions: new string[] { "poison" }
                        ),
                }
                ) },

        // Infiltrator
        {"infiltrator",
            new AbilityData(
                ID: "infiltrator",
                abilityName: "Infiltrator",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Infiltrator(),
                }
                ) },

        // Innards Out
        {"innardsout",
            new AbilityData(
                ID: "innardsout",
                abilityName: "Innards Out",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Aftermath(
                        damage: new EffectDatabase.General.Damage(
                            mode: EffectDatabase.General.Damage.DamageMode.InnardsOut,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Oblivious(
                        effectsBlocked: new PokemonSEType[]{ PokemonSEType.Flinch }
                        ),
                    new EffectDatabase.AbilityEff.IntimidateBlock(
                        abilitiesBlocked: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Insomnia
        {"insomnia",
            new AbilityData(
                ID: "insomnia",
                abilityName: "Insomnia",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
                        conditions: new string[] { "sleep" }
                        ),
                }
                ) },

        // Intimidate
        {"intimidate",
            new AbilityData(
                ID: "intimidate",
                abilityName: "Intimidate",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Intimidate(
                        statStageMod: new EffectDatabase.General.StatStageMod(ATKMod: -1)
                        ),
                }
                ) },

        // Intrepid Sword
        {"intrepidsword",
            new AbilityData(
                ID: "intrepidsword",
                abilityName: "Intrepid Sword",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IntrepidSword(
                        statStageMod: new EffectDatabase.General.StatStageMod(ATKMod: 1)
                        ),
                }
                ) },

        // Iron Barbs
        {"ironbarbs",
            new AbilityData(
                ID: "ironbarbs",
                abilityName: "Iron Barbs",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.RoughSkin(
                        damage: new EffectDatabase.General.Damage(
                            mode: EffectDatabase.General.Damage.DamageMode.MaxHPPercent,
                            value: 1f/8
                            )
                        ),
                }
                ) },

        // Iron Fist
        {"ironfist",
            new AbilityData(
                ID: "ironfist",
                abilityName: "Iron Fist",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.2f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Justified(
                        statStageMod: new EffectDatabase.General.StatStageMod(ATKMod: 1),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HyperCutter(
                        affectedStats: new PokemonStats[] { PokemonStats.Accuracy, }
                        ),
                    new EffectDatabase.AbilityEff.Unaware(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
                        statusTypes: new PokemonSEType[] { PokemonSEType.NonVolatile }
                        ),
                }
                ) },

        // Levitate
        {"levitate",
            new AbilityData(
                ID: "levitate",
                abilityName: "Levitate",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Levitate(),
                }
                ) },

        // Libero
        {"libero",
            new AbilityData(
                ID: "libero",
                abilityName: "Libero",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Protean(),
                }
                ) },

        // Light Metal
        {"lightmetal",
            new AbilityData(
                ID: "lightmetal",
                abilityName: "Light Metal",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HeavyMetal(weightMultiplier: 0.5f),
                }
                ) },

        // Lightning Rod
        {"lightningrod",
            new AbilityData(
                ID: "lightningrod",
                abilityName: "Lightning Rod",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.LightningRod(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
                                types: new string[] { "electric" }
                                ),
                        }),
                    new EffectDatabase.AbilityEff.VoltAbsorb(
                        conditions: new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "electric" },
                                motorDrive: new EffectDatabase.General.StatStageMod(SPAMod: 1)
                                ),
                        }),
                }
                ) },

        // Limber
        {"limber",
            new AbilityData(
                ID: "limber",
                abilityName: "Limber",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
                        conditions: new string[] { "paralysis" }
                        ),
                }
                ) },

        // Liquid Ooze
        {"liquidooze",
            new AbilityData(
                ID: "liquidooze",
                abilityName: "Liquid Ooze",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.LiquidOoze(),
                }
                ) },

        // Liquid Voice
        {"liquidvoice",
            new AbilityData(
                ID: "liquidvoice",
                abilityName: "Liquid Voice",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Aerilate(
                        toMoveType: "water",
                        powerMultiplier: 1f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.MagicBounce(
                        magicCoat: new EffectDatabase.General.MagicCoat(),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Overcoat(allWeather: true),
                    new EffectDatabase.AbilityEff.MagicGuard(),
                }
                ) },

        // Magician
        {"magician",
            new AbilityData(
                ID: "magician",
                abilityName: "Magician",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Magician(),
                }
                ) },

        // Magma Armor
        {"magmaarmor",
            new AbilityData(
                ID: "magmaarmor",
                abilityName: "Magma Armor",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.MagnetPull(
                        types: new string[] {"steel"},
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(types: new string[]{ "ghost" }, invert: true),
                        }
                        ),
                }
                ) },

        // Marvel Scale
        {"marvelscale",
            new AbilityData(
                ID: "marvelscale",
                abilityName: "Marvel Scale",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Guts(
                        conditionCheck: new EffectDatabase.Filter.Harvest(
                            conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn", "freeze", "paralysis", "poison", "sleep" }
                            ),
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.5f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SuperLuck(
                        alwaysCritical: true,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.Harvest(
                                conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Mimicry(
                        conditions: new EffectDatabase.AbilityEff.Mimicry.MimicryCondition[]
                        {
                            new EffectDatabase.AbilityEff.Mimicry.MimicryCondition(
                                conditions: new string[] { "electricterrain" },
                                types: new string[] { "electric" }
                                ),
                            new EffectDatabase.AbilityEff.Mimicry.MimicryCondition(
                                conditions: new string[] { "grassyterrain" },
                                types: new string[] { "grass" }
                                ),
                            new EffectDatabase.AbilityEff.Mimicry.MimicryCondition(
                                conditions: new string[] { "mistyterrain" },
                                types: new string[] { "fairy" }
                                ),
                            new EffectDatabase.AbilityEff.Mimicry.MimicryCondition(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Minus(
                        allyAbilities: new string[] { "minus", "plus" },
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.MirrorArmor(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.MoldBreaker(),
                }
                ) },

        // Moody
        {"moody",
            new AbilityData(
                ID: "moody",
                abilityName: "Moody",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Moody(
                        statStageMods1: new List<EffectDatabase.General.StatStageMod>
                        {
                            new EffectDatabase.General.StatStageMod(ATKMod: 2),
                            new EffectDatabase.General.StatStageMod(DEFMod: 2),
                            new EffectDatabase.General.StatStageMod(SPAMod: 2),
                            new EffectDatabase.General.StatStageMod(SPDMod: 2),
                            new EffectDatabase.General.StatStageMod(SPEMod: 2),
                        },
                        statStageMods2: new List<EffectDatabase.General.StatStageMod>
                        {
                            new EffectDatabase.General.StatStageMod(ATKMod: -1),
                            new EffectDatabase.General.StatStageMod(DEFMod: -1),
                            new EffectDatabase.General.StatStageMod(SPAMod: -1),
                            new EffectDatabase.General.StatStageMod(SPDMod: -1),
                            new EffectDatabase.General.StatStageMod(SPEMod: -1),
                        }
                        ),
                }
                ) },

        // Motor Drive
        {"motordrive",
            new AbilityData(
                ID: "motordrive",
                abilityName: "Motor Drive",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.VoltAbsorb(
                        conditions: new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "electric" },
                                motorDrive: new EffectDatabase.General.StatStageMod(SPEMod: 1)
                                ),
                        }),
                }
                ) },

        // Moxie
        {"moxie",
            new AbilityData(
                ID: "moxie",
                abilityName: "Moxie",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Moxie(
                        statStageMod: new EffectDatabase.General.StatStageMod(ATKMod: 1)
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Multitype(),
                }
                ) },

        // Multiscale
        {"multiscale",
            new AbilityData(
                ID: "multiscale",
                abilityName: "Multiscale",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Multiscale(),
                }
                ) },

        // Mummy
        {"mummy",
            new AbilityData(
                ID: "mummy",
                abilityName: "Mummy",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Mummy(),
                }
                ) },

        // Natural Cure
        {"naturalcure",
            new AbilityData(
                ID: "naturalcure",
                abilityName: "Natural Cure",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.NaturalCure(
                        conditions: new EffectDatabase.Filter.Harvest[]
                        {
                            new EffectDatabase.Filter.Harvest(
                                conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.TintedLens(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.NeutralizingGas(),
                }
                ) },

        // No Guard
        {"noguard",
            new AbilityData(
                ID: "noguard",
                abilityName: "No Guard",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.NoGuard(),
                }
                ) },

        // Normalize
        {"normalize",
            new AbilityData(
                ID: "normalize",
                abilityName: "Normalize",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Aerilate(
                        toMoveType: "normal"
                        ),
                }
                ) },

        // Oblivious
        {"oblivious",
            new AbilityData(
                ID: "oblivious",
                abilityName: "Oblivious",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Oblivious(
                        effectsBlocked: new PokemonSEType[]{ PokemonSEType.Infatuation, PokemonSEType.Taunt }
                        ),
                    new EffectDatabase.AbilityEff.IntimidateBlock(
                        abilitiesBlocked: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Overcoat
        {"overcoat",
            new AbilityData(
                ID: "overcoat",
                abilityName: "Overcoat",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Overcoat(allWeather: true),
                    new EffectDatabase.AbilityEff.Cacophony(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.5f,
                        blazeThreshold: 1f/3,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Oblivious(
                        effectsBlocked: new PokemonSEType[]{ PokemonSEType.Confusion }
                        ),
                    new EffectDatabase.AbilityEff.IntimidateBlock(
                        abilitiesBlocked: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Parental Bond
        {"parentalbond",
            new AbilityData(
                ID: "parentalbond",
                abilityName: "Parental Bond",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ParentalBond(
                        bondedHits: new EffectDatabase.AbilityEff.ParentalBond.BondedHit[]
                        {
                            new EffectDatabase.AbilityEff.ParentalBond.BondedHit(damageModifier: 1f),
                            new EffectDatabase.AbilityEff.ParentalBond.BondedHit(damageModifier: 0.25f),
                        }
                        ),
                }
                ) },

        // Pastel Veil
        {"pastelveil",
            new AbilityData(
                ID: "pastelveil",
                abilityName: "Pastel Veil",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlameBody(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "perishsong"
                            ),
                        perishBody: true,
                        triggerTags: new MoveTag[] { MoveTag.MakesContact }
                        ),
                }
                ) },

        // Pickpocket
        {"pickpocket",
            new AbilityData(
                ID: "pickpocket",
                abilityName: "Pickpocket",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Pickpocket(),
                }
                ) },

        // Pixilate
        {"pixilate",
            new AbilityData(
                ID: "pixilate",
                abilityName: "Pixilate",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Aerilate(
                        toMoveType: "fairy",
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Minus(
                        allyAbilities: new string[] { "minus", "plus" },
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.PoisonHeal(
                        conditions: new EffectDatabase.AbilityEff.PoisonHeal.HealCondition[]
                        {
                            new EffectDatabase.AbilityEff.PoisonHeal.HealCondition(
                                heal: new EffectDatabase.General.HealHP(
                                    healMode: EffectDatabase.General.HealHP.HealMode.MaxHPPercent,
                                    healValue: 1f/8
                                    ),
                                conditions: new EffectDatabase.Filter.Harvest[]
                                {
                                    new EffectDatabase.Filter.Harvest(
                                        conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlameBody(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "poison"
                            ),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact },
                        chance: 0.3f
                        ),
                }
                ) },

        // Poison Touch
        {"poisontouch",
            new AbilityData(
                ID: "poisontouch",
                abilityName: "Poison Touch",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.PoisonTouch(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "poison"
                            ),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ZenMode(
                        hpThreshold: 0.5f, checkBelow: true,
                        transformation: new EffectDatabase.General.FormTransformation(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.PowerOfAlchemy(),
                }
                ) },

        // Power Spot
        {"powerspot",
            new AbilityData(
                ID: "powerspot",
                abilityName: "Power Spot",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Battery(),
                }
                ) },

        // Prankster
        {"prankster",
            new AbilityData(
                ID: "prankster",
                abilityName: "Prankster",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.GaleWings(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
                                moveCategories: new MoveCategory[] { MoveCategory.Status }
                                )
                        }
                        ),
                    new EffectDatabase.AbilityEff.Prankster(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Pokemon,
                                types: new string[] { "dark" }
                                ),
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Pressure()
                }
                ) },

        // Primordial Sea
        {"primordialsea",
            new AbilityData(
                ID: "primordialsea",
                abilityName: "Primordial Sea",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        desolateLand: true,
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SolidRock(
                        superEffectiveModifier: 0.75f),
                }
                ) },

        // Propeller Tail
        {"propellertail",
            new AbilityData(
                ID: "propellertail",
                abilityName: "Propeller Tail",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.PropellerTail(),
                }
                ) },

        // Protean
        {"protean",
            new AbilityData(
                ID: "protean",
                abilityName: "Protean",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Protean(),
                }
                ) },

        // Psychic Surge
        {"psychicsurge",
            new AbilityData(
                ID: "psychicsurge",
                abilityName: "Psychic Surge",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.3f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.SoundMove }
                                )
                        }
                        ),
                    new EffectDatabase.AbilityEff.IceScales(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.QueenlyMajesty(),
                }
                ) },

        // Quick Draw
        {"quickdraw",
            new AbilityData(
                ID: "quickdraw",
                abilityName: "Quick Draw",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.QuickDraw(),
                }
                ) },

        // Quick Feet
        {"quickfeet",
            new AbilityData(
                ID: "quickfeet",
                abilityName: "Quick Feet",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Guts(
                        conditionCheck: new EffectDatabase.Filter.Harvest(
                            conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn", "freeze", "paralysis", "poison", "sleep" }
                            ),
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.DrySkin(
                        conditions: new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition[]
                        {
                            new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Justified(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPEMod: 1),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
                                types: new string[] { "bug", "dark", "ghost" }
                                ),
                        }
                        ),
                    new EffectDatabase.AbilityEff.IntimidateTrigger(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPEMod: 1),
                        abilityTriggers: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Receiver
        {"receiver",
            new AbilityData(
                ID: "receiver",
                abilityName: "Receiver",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.PowerOfAlchemy(),
                }
                ) },

        // Reckless
        {"reckless",
            new AbilityData(
                ID: "reckless",
                abilityName: "Reckless",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.2f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Aerilate(
                        toMoveType: "ice",
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.NaturalCure(
                        regenerator: new EffectDatabase.General.HealHP(
                            healMode: EffectDatabase.General.HealHP.HealMode.MaxHPPercent,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Ripen(
                        effectMultiplier: 2f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.ItemCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Rivalry(),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.RKSSystem(),
                }
                ) },

        // Rock Head
        {"rockhead",
            new AbilityData(
                ID: "rockhead",
                abilityName: "Rock Head",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.RockHead(),
                }
                ) },

        // Rough Skin
        {"roughskin",
            new AbilityData(
                ID: "roughskin",
                abilityName: "Rough Skin",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.RoughSkin(
                        damage: new EffectDatabase.General.Damage(
                            mode: EffectDatabase.General.Damage.DamageMode.MaxHPPercent,
                            value: 1f/8
                            )
                        ),
                }
                ) },

        // Run Away
        {"runaway",
            new AbilityData(
                ID: "runaway",
                abilityName: "Run Away",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.RunAway(),
                }
                ) },

        // Sand Force
        {"sandforce",
            new AbilityData(
                ID: "sandforce",
                abilityName: "Sand Force",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Overcoat(
                        conditions: new string[] { "sandstorm" }
                        ),
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.3f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Overcoat(
                        conditions: new string[] { "sandstorm" }
                        ),
                    new EffectDatabase.AbilityEff.SwiftSwim(
                        conditions: new List<EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition>
                        {
                            new EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "sandstorm" },
                                statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlameBody(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Overcoat(
                        conditions: new string[] { "sandstorm" }
                        ),
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Evasion, 1.2f },
                            }),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.Harvest(
                                conditionType: EffectDatabase.Filter.Harvest.ConditionType.Battle,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.VoltAbsorb(
                        conditions: new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "grass" },
                                motorDrive: new EffectDatabase.General.StatStageMod(ATKMod: 1)
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ZenMode(
                        hpThreshold: 0.25f, checkBelow: false,
                        transformation: new EffectDatabase.General.FormTransformation(
                            preForm: "wishiwashi", toForm: "wishiwashi-school"
                            )
                        ),
                    new EffectDatabase.AbilityEff.ZenMode(
                        hpThreshold: 0.25f, checkBelow: true,
                        transformation: new EffectDatabase.General.FormTransformation(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Scrappy(
                        bypassImmunities: new string[] { "ghost" }
                        ),
                    new EffectDatabase.AbilityEff.IntimidateBlock(
                        abilitiesBlocked: new string[] { "intimidate" }
                        ),
                }
                ) },

        // Screen Cleaner
        {"screencleaner",
            new AbilityData(
                ID: "screencleaner",
                abilityName: "Screen Cleaner",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ScreenCleaner(),
                }
                ) },

        // Serene Grace
        {"serenegrace",
            new AbilityData(
                ID: "serenegrace",
                abilityName: "Serene Grace",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SereneGrace(),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Multiscale(),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ShadowTag(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(types: new string[]{ "ghost" }, invert: true),
                        }
                        ),
                }
                ) },

        // Shed Skin
        {"shedskin",
            new AbilityData(
                ID: "shedskin",
                abilityName: "Shed Skin",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Hydration(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SheerForce(),
                }
                ) },

        // Shell Armor
        {"shellarmor",
            new AbilityData(
                ID: "shellarmor",
                abilityName: "Shell Armor",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.BattleArmor(),
                }
                ) },

        // Shield Dust
        {"shielddust",
            new AbilityData(
                ID: "shielddust",
                abilityName: "Shield Dust",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ShieldDust(),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ZenMode(
                        hpThreshold: 0.5f, checkBelow: true,
                        transformation: new EffectDatabase.General.FormTransformation(
                            preForm: "minior", toForm: "minior-core"
                            )
                        ),
                    new EffectDatabase.AbilityEff.ZenMode(
                        hpThreshold: 0.5f, checkBelow: false,
                        transformation: new EffectDatabase.General.FormTransformation(
                            preForm: "minior-core", toForm: "minior"
                            )
                        ),
                    new EffectDatabase.AbilityEff.ShieldsDown(
                        meteorForms: new EffectDatabase.AbilityEff.ShieldsDown.MeteorForm[]
                        {
                            new EffectDatabase.AbilityEff.ShieldsDown.MeteorForm(
                                forms: new string[] { "minior" },
                                blockedStatuses: new EffectDatabase.Filter.Harvest[]
                                {
                                    new EffectDatabase.Filter.Harvest(
                                        conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Simple(),
                }
                ) },

        // Skill Link
        {"skilllink",
            new AbilityData(
                ID: "skilllink",
                abilityName: "Skill Link",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SkillLink(),
                }
                ) },

        // Slow Start
        {"slowstart",
            new AbilityData(
                ID: "slowstart",
                abilityName: "Slow Start",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SlowStart(
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SwiftSwim(
                        conditions: new List<EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition>
                        {
                            new EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "hail" },
                                statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Sniper(),
                }
                ) },

        // Snow Cloak
        {"snowcloak",
            new AbilityData(
                ID: "snowcloak",
                abilityName: "Snow Cloak",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Overcoat(
                        conditions: new string[] { "hail" }
                        ),
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.Evasion, 1.2f },
                            }),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.Harvest(
                                conditionType: EffectDatabase.Filter.Harvest.ConditionType.Battle,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Drought(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.DrySkin(
                        conditions: new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition[]
                        {
                            new EffectDatabase.AbilityEff.DrySkin.DrySkinCondition(
                                conditions: new string[] { "harshsunlight" },
                                hpLosePercent: 1f/8
                                ),
                        }
                        ),
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        statScale: new EffectDatabase.General.StatScale(
                            scaleMap: new Dictionary<PokemonStats, float>
                            {
                                { PokemonStats.SpecialAttack, 1.5f },
                            }),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.Harvest(
                                conditionType: EffectDatabase.Filter.Harvest.ConditionType.Battle,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SolidRock(
                        superEffectiveModifier: 0.75f),
                }
                ) },

        // Soundproof
        {"soundproof",
            new AbilityData(
                ID: "soundproof",
                abilityName: "Soundproof",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Cacophony(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SoulHeart(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPAMod: 1)
                        ),
                }
                ) },

        // Speed Boost
        {"speedboost",
            new AbilityData(
                ID: "speedboost",
                abilityName: "Speed Boost",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SpeedBoost(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPEMod: 1)
                        ),
                }
                ) },

        // Stakeout
        {"stakeout",
            new AbilityData(
                ID: "stakeout",
                abilityName: "Stakeout",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Stakeout(),
                }
                ) },

        // Stall
        {"stall",
            new AbilityData(
                ID: "stall",
                abilityName: "Stall",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Stall(),
                }
                ) },

        // Stalwart
        {"stalwart",
            new AbilityData(
                ID: "stalwart",
                abilityName: "Stalwart",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.PropellerTail(),
                }
                ) },

        // Stamina
        {"stamina",
            new AbilityData(
                ID: "stamina",
                abilityName: "Stamina",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Justified(
                        statStageMod: new EffectDatabase.General.StatStageMod(DEFMod: 1)
                        ),
                }
                ) },

        // Stance Change
        {"stancechange",
            new AbilityData(
                ID: "stancechange",
                abilityName: "Stance Change",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.StanceChange(
                        transformations: new EffectDatabase.AbilityEff.StanceChange.Transformation[]
                        {
                            // Blade
                            new EffectDatabase.AbilityEff.StanceChange.Transformation(
                                transformation: new EffectDatabase.General.FormTransformation(
                                    preForm: "aegislash", toForm: "aegislash-blade"),
                                moveCheck: new EffectDatabase.Filter.MoveCheck(
                                    moveCategories: new MoveCategory[]
                                    {
                                        MoveCategory.Physical, MoveCategory.Special
                                    })
                                ),
                            // Shield
                            new EffectDatabase.AbilityEff.StanceChange.Transformation(
                                transformation: new EffectDatabase.General.FormTransformation(
                                    preForm: "aegislash-blade", toForm: "aegislash"),
                                moveCheck: new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlameBody(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "paralysis"
                            ),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact },
                        chance: 0.3f
                        ),
                }
                ) },

        // Steam Engine
        {"steamengine",
            new AbilityData(
                ID: "steamengine",
                abilityName: "Steam Engine",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Justified(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPEMod: 6),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.5f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.5f,
                        steelySpirit: true,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.PoisonTouch(
                        inflictStatus: new EffectDatabase.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.StickyHold(),
                }
                ) },

        // Storm Drain
        {"stormdrain",
            new AbilityData(
                ID: "stormdrain",
                abilityName: "Storm Drain",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.LightningRod(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
                                types: new string[] { "water" }
                                ),
                        }),
                    new EffectDatabase.AbilityEff.VoltAbsorb(
                        conditions: new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition(
                                moveTypes: new string[] { "water" },
                                motorDrive: new EffectDatabase.General.StatStageMod(SPAMod: 1)
                                ),
                        }),
                }
                ) },

        // Strong Jaw
        {"strongjaw",
            new AbilityData(
                ID: "strongjaw",
                abilityName: "Strong Jaw",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.5f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Sturdy(),
                }
                ) },

        // Suction Cups
        {"suctioncups",
            new AbilityData(
                ID: "suctioncups",
                abilityName: "Suction Cups",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SuctionCups(),
                }
                ) },

        // Super Luck
        {"superluck",
            new AbilityData(
                ID: "superluck",
                abilityName: "Super Luck",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SuperLuck(),
                }
                ) },

        // Surge Surfer
        {"surgesurfer",
            new AbilityData(
                ID: "surgesurfer",
                abilityName: "Surge Surfer",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SwiftSwim(
                        conditions: new List<EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition>
                        {
                            new EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "electricterrain" },
                                statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.5f,
                        blazeThreshold: 1f/3,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.SwiftSwim(
                        conditions: new List<EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition>
                        {
                            new EffectDatabase.AbilityEff.SwiftSwim.SwiftSwimCondition(
                                conditions: new string[] { "rain" },
                                statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Symbiosis(),
                }
                ) },

        // Synchronize
        {"synchronize",
            new AbilityData(
                ID: "synchronize",
                abilityName: "Synchronize",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Synchronize(
                        conditionCheck: new EffectDatabase.Filter.Harvest(
                            conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "burn", "paralysis", "poison" }
                            )),
                }
                ) },

        // Tangled Feet
        {"tangledfeet",
            new AbilityData(
                ID: "tangledfeet",
                abilityName: "Tangled Feet",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Guts(
                        conditionCheck: new EffectDatabase.Filter.Harvest(
                            conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
                            statusPKTypes: new PokemonSEType[] { PokemonSEType.Confusion }
                            ),
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Gooey(
                        statStageMod: new EffectDatabase.General.StatStageMod(SPEMod: -1),
                        triggerTags: new MoveTag[] { MoveTag.MakesContact, }
                        ),
                }
                ) },

        // Technician
        {"technician",
            new AbilityData(
                ID: "technician",
                abilityName: "Technician",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Technician(),
                }
                ) },

        // Telepathy
        {"telepathy",
            new AbilityData(
                ID: "telepathy",
                abilityName: "Telepathy",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Telepathy(),
                }
                ) },

        // Teravolt
        {"teravolt",
            new AbilityData(
                ID: "teravolt",
                abilityName: "Teravolt",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.MoldBreaker(displayText: "ability-teravolt"),
                }
                ) },

        // Thick Fat
        {"thickfat",
            new AbilityData(
                ID: "thickfat",
                abilityName: "Thick Fat",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IceScales(
                        damageModifier: 0.5f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.TintedLens(
                        notVeryEffectiveModifier: 2f),
                }
                ) },

        // Torrent
        {"torrent",
            new AbilityData(
                ID: "torrent",
                abilityName: "Torrent",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.5f,
                        blazeThreshold: 1f/3,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.FlareBoost(
                        powerMultiplier: 1.5f,
                        conditionCheck: new EffectDatabase.Filter.Harvest(
                            conditionType: EffectDatabase.Filter.Harvest.ConditionType.Pokemon,
                            conditions: new string[] { "poison" }
                            ),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Trace(),
                }
                ) },

        // Tough Claws
        {"toughclaws",
            new AbilityData(
                ID: "toughclaws",
                abilityName: "Tough Claws",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 1.3f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.GaleWings(
                        priority: 3,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(healingOnly: true),
                        }
                        )
                }
                ) },

        // Truant
        {"truant",
            new AbilityData(
                ID: "truant",
                abilityName: "Truant",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Truant(),
                }
                ) },

        // Turboblaze
        {"turboblaze",
            new AbilityData(
                ID: "turboblaze",
                abilityName: "Turboblaze",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.MoldBreaker(displayText: "ability-turboblaze"),
                }
                ) },

        // Unaware
        {"unaware",
            new AbilityData(
                ID: "unaware",
                abilityName: "Unaware",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Unaware(),
                }
                ) },

        // Unburden
        {"unburden",
            new AbilityData(
                ID: "unburden",
                abilityName: "Unburden",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Unburden(
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.UnseenFist(),
                }
                ) },

        // Victory Star
        {"victorystar",
            new AbilityData(
                ID: "victorystar",
                abilityName: "Victory Star",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.CompoundEyes(
                        victoryStar: true,
                        statScale: new EffectDatabase.General.StatScale(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
                        conditions: new string[] { "sleep" }
                        ),
                }
                ) },

        // Volt Absorb
        {"voltabsorb",
            new AbilityData(
                ID: "voltabsorb",
                abilityName: "Volt Absorb",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.VoltAbsorb(
                        conditions: new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Mummy(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.VoltAbsorb(
                        conditions: new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition[]
                        {
                            new EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.IronFist(
                        powerMultiplier: 2f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
                                types: new string[] { "water" }
                                )
                        }
                        ),
                    new EffectDatabase.AbilityEff.IceScales(
                        damageModifier: 0.5f,
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Justified(
                        statStageMod: new EffectDatabase.General.StatStageMod(DEFMod: 2),
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.TypeList(
                                targetType: EffectDatabase.Filter.TypeList.TargetType.Move,
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Limber(
                        conditions: new string[] { "burn" }
                        ),
                }
                ) },

        // Weak Armor
        {"weakarmor",
            new AbilityData(
                ID: "weakarmor",
                abilityName: "Weak Armor",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.Justified(
                        statStageMod: new EffectDatabase.General.StatStageMod(DEFMod: -1, SPEMod: 2),
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.HyperCutter(
                        clearBody: true,
                        displayText: "ability-clearbody"),
                }
                ) },

        // Wimp Out
        {"wimpout",
            new AbilityData(
                ID: "wimpout",
                abilityName: "Wimp Out",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.WimpOut(hpThreshold: 0.5f),
                }
                ) },

        // Wonder Guard
        {"wonderguard",
            new AbilityData(
                ID: "wonderguard",
                abilityName: "Wonder Guard",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.WonderGuard(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(damagingOnly: true),
                        }),
                }
                ) },

        // Wonder Skin
        {"wonderskin",
            new AbilityData(
                ID: "wonderskin",
                abilityName: "Wonder Skin",
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.WonderSkin(
                        filters: new EffectDatabase.Filter.FilterEffect[]
                        {
                            new EffectDatabase.Filter.MoveCheck(
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
                effectsNew: new EffectDatabase.AbilityEff.AbilityEffect[]
                {
                    new EffectDatabase.AbilityEff.ZenMode(
                        hpThreshold: 0.5f, checkBelow: true,
                        transformation: new EffectDatabase.General.FormTransformation(
                            preForm: "darmanitan", toForm: "darmanitan-zen"
                            )
                        ),
                    new EffectDatabase.AbilityEff.ZenMode(
                        hpThreshold: 0.5f, checkBelow: true,
                        transformation: new EffectDatabase.General.FormTransformation(
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

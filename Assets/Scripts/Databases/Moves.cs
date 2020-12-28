using PBS.Data;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Databases
{
    public class Moves
    {
        //create an object of SingleObject
        private static Moves singleton = new Moves();

        //make the constructor private so that this class cannot be
        //instantiated
        private Moves() { }

        //Get the only object available
        public static Moves instance
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
        private Dictionary<string, MoveData> database = new Dictionary<string, MoveData>
    {
        // Null / Placeholder
        {"",
            new MoveData(
                ID: ""
                ) },

        // Struggle
        {"struggle",
            new MoveData(
                ID: "struggle",
                moveName: "Struggle",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Self,

                basePower: 50, PP: 1,
                moveTags: new MoveTag[]
                {
                    MoveTag.CannotDisable,
                    MoveTag.CannotEncore,
                    MoveTag.CannotInstruct,
                    MoveTag.CannotMimic,
                    MoveTag.CannotSketch,
                    MoveTag.MakesContact,
                    MoveTag.UncallableCommon,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMeFirst,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableByMirrorMove,
                    MoveTag.UncallableBySleepTalk,

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Struggle),
                    new MoveEffect(MoveEffectType.TypelessDamage),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.DoubleEdge(
                        hpLossPercent: 0.25f
                        )
                }
                ) },

        // Growl2
        {"growl2",
            new MoveData(
                ID: "growl2",
                moveName: "Growl2",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllAdjacentOpponents,

                accuracy: 1.0f, PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.SoundMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "atk" }
                        ),
                    new MoveEffect(
                        MoveEffectType.StatStageMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 1 },
                        stringParams: new string[] { "spa" }
                        ),
                }
                ) },

        // Heavy Rain Dance
        {"raindance2",
            new MoveData(
                ID: "raindance2",
                moveName: "Heavy Rain Dance",
                moveType: "water",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 5,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 5, -1 },
                        stringParams: new string[] { "heavyrain" }
                        ),
                }
                ) },

        // Delta Stream
        {"deltastream",
            new MoveData(
                ID: "deltastream",
                moveName: "Delta Stream",
                moveType: "flying",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 5,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 2, -1 },
                        stringParams: new string[] { "strongwinds" }
                        ),
                }
                ) },

        // Hyperspace Hole 2
        {"hyperspacehole2",
            new MoveData(
                ID: "hyperspacehole2",
                moveName: "Minispace Hole",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 10, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Feint(),
                }
                ) },








        // ---REGULAR MOVES---

        // A

        // Absorb
        {"absorb",
            new MoveData(
                ID: "absorb",
                moveName: "Absorb",
                moveType: "grass",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 20, accuracy: 1.0f, PP: 25,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Absorb(),
                }
                ) },

        // Acid
        {"acid",
            new MoveData(
                ID: "acid",
                moveName: "Acid",
                moveType: "poison",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 40, accuracy: 1.0f, PP: 30,

                moveTags: new MoveTag[]
                {
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.StatStageMod,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        effectChance: 0.1f,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "spd" }
                        ),
                }
                ) },

        // After You
        {"afteryou",
            new MoveData(
                ID: "afteryou",
                moveName: "After You",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByMetronome
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.AfterYou,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Ally Switch
        {"allyswitch",
            new MoveData(
                ID: "allyswitch",
                moveName: "Ally Switch",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AdjacentAlly,
                PP: 15, priority: 2,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.AllySwitch,
                        stringParams: new string[] { "move-allyswitch" }
                        ),
                }
                ) },

        // Anchor Shot
        {"anchorshot",
            new MoveData(
                ID: "anchorshot",
                moveName: "Anchor Shot",
                moveType: "steel",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 80, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Block,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT", "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Aqua Ring
        {"aquaring",
            new MoveData(
                ID: "aquaring",
                moveName: "Aqua Ring",
                moveType: "water",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Ingrain,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { 1f / 16 },
                        stringParams: new string[] { "move-aquaring", "move-aquaring-heal" }
                        ),
                }
                ) },

        // Aromatherapy
        {"aromatherapy",
            new MoveData(
                ID: "aromatherapy",
                moveName: "Aromatherapy",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Aromatherapy,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Team,
                        boolParams: new bool[] { true }
                        ),
                    new MoveEffect(
                        MoveEffectType.Celebrate,
                        stringParams: new string[] { "move-aromatherapy" }
                        ),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Refresh(
                        statusEffectTypes: new PokemonSEType[] { PokemonSEType.NonVolatile }
                        ),
                }
                ) },

        // Assist
        {"assist",
            new MoveData(
                ID: "assist",
                moveName: "Assist",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Assist),
                }
                ) },

        // Attract
        {"attract",
            new MoveData(
                ID: "attract",
                moveName: "Attract",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                    MoveTag.MagicCoatSusceptible,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "infatuation"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Aura Wheel
        {"aurawheel",
            new MoveData(
                ID: "aurawheel",
                moveName: "Aura Wheel",
                moveType: "electric",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 110, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.AuraWheel(
                        type: "electric",
                        pokemonIDs: new string[] { "morpeko" }
                        ),
                    new Effects.Moves.AuraWheel(
                        type: "dark",
                        pokemonIDs: new string[] { "morpeko-hangry" }
                        ),
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            SPEMod: 1
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Self
                        ),
                    new Effects.Moves.FailNotPokemon(
                        pokemonIDs: new string[] { "morpeko", "morpeko-hangry" },
                        allowTransform: true
                        ),
                }
                ) },

        // Aurora Beam
        {"aurorabeam",
            new MoveData(
                ID: "aurorabeam",
                moveName: "Aurora Beam",
                moveType: "ice",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                basePower: 65, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        chance: 0.1f,
                        statStageMod: new Effects.General.StatStageMod(
                            ATKMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Aurora Veil
        {"auroraveil",
            new MoveData(
                ID: "auroraveil",
                moveName: "Aurora Veil",
                moveType: "ice",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.FailWeather,
                        boolParams: new bool[] { false, true, false, false },
                        stringParams: new string[] { "hail" }
                        ),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Team,
                            statusID: "auroraveil"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true
                        ),
                }
                ) },


        // B

        // Baneful Bunker
        {"banefulbunker",
            new MoveData(
                ID: "banefulbunker",
                moveName: "Baneful Bunker",
                moveType: "poison",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10, priority: 4,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Protect,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Self,
                        boolParams: new bool[] { true },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.BanefulBunker,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "poison" }
                        ),
                }
                ) },

        // Baton Pass
        {"batonpass",
            new MoveData(
                ID: "batonpass",
                moveName: "Baton Pass",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 40,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.BatonPass,
                        boolParams: new bool[] { true }
                        ),
                }
                ) },

        // Beak Blast
        {"beakblast",
            new MoveData(
                ID: "beakblast",
                moveName: "Beak Blast",
                moveType: "flying",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 100, accuracy: 1.0f, PP: 15, priority: -3,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMeFirst,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.BeakBlast,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "burn", "DEFAULT" }
                        ),
                }
                ) },

        // Beat Up
        {"beatup",
            new MoveData(
                ID: "beatup",
                moveName: "Beat Up",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.BeatUp)
                }
                ) },

        // Belch
        {"belch",
            new MoveData(
                ID: "belch",
                moveName: "Belch",
                moveType: "poison",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 120, accuracy: 0.9f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByMeFirst,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Belch)
                }
                ) },

        // Bestow
        {"bestow",
            new MoveData(
                ID: "bestow",
                moveName: "Bestow",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Bestow,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Bide
        {"bide",
            new MoveData(
                ID: "bide",
                moveName: "Bide",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Self,

                accuracy: 1.0f, PP: 10, priority: 1,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Bide,
                        floatParams: new float[] { 2f, 2 }
                    ),
                }
                ) },

        // Bind
        {"bind",
            new MoveData(
                ID: "bind",
                moveName: "Bind",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 15, accuracy: 0.85f, PP: 20,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Bind,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        floatParams: new float[] { 1f/8, 4 },
                        stringParams: new string[] {
                            "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.BindTurnRange,
                        floatParams: new float[] { 4, 5 }
                        ),
                }
                ) },

        // Bite
        {"bite",
            new MoveData(
                ID: "bite",
                moveName: "Bite",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 60, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FakeOut(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.OnceForEachTarget,
                    targetType: MoveEffectTargetType.Target,
                    chance: 0.3f
                    ),
                }
                ) },

        // Blizzard
        {"blizzard",
            new MoveData(
                ID: "blizzard",
                moveName: "Blizzard",
                moveType: "ice",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 110, accuracy: 0.7f, PP: 5,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.InflictPokemonSC,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        effectChance: 0.1f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "freeze" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.AccuracyInWeather,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "hail" }
                        ),
                }
                ) },

        // Block
        {"block",
            new MoveData(
                ID: "block",
                moveName: "Block",
                moveType: "steel",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Block,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT", "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Body Slam
        {"bodyslam",
            new MoveData(
                ID: "bodyslam",
                moveName: "Body Slam",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 85, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.3f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { 1, -1 },
                        stringParams: new string[] { "paralysis" }
                        ),
                    new MoveEffect(
                        MoveEffectType.DmgMinimizeState,
                        floatParams: new float[] { 2 }
                        ),
                }
                ) },

        // Bounce
        {"bounce",
            new MoveData(
                ID: "bounce",
                moveName: "Bounce",
                moveType: "flying",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Any,

                basePower: 85, accuracy: 0.85f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseInGravity,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "move-charge-bounce" }
                        ),

                    new MoveEffect(MoveEffectType.MultiTurnFly),
                }
                ) },

        // Bubblebeam
        {"bubblebeam",
            new MoveData(
                ID: "bubblebeam",
                moveName: "Bubble Beam",
                moveType: "water",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 65, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            SPEMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: 0.1f
                        ),
                }
                ) },

        // Bug Bite
        {"bugbite",
            new MoveData(
                ID: "bugbite",
                moveName: "Bug Bite",
                moveType: "bug",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.BugBite,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Burn Up
        {"burnup",
            new MoveData(
                ID: "burnup",
                moveName: "Burn Up",
                moveType: "fire",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 130, accuracy: 1.0f, PP: 5,

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.BurnUp,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Self,
                        stringParams: new string[] { "move-burnup", "fire" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.FailUserType,
                        boolParams: new bool[] { false },
                        stringParams: new string[] { "fire" }
                        ),
                }
                ) },

        // Burning Jealousy
        {"burningjealousy",
            new MoveData(
                ID: "burningjealousy",
                moveName: "Burning Jealousy",
                moveType: "fire",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 70, accuracy: 1.0f, PP: 5,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.BurningJealousy(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "burn"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },


        // C

        // Celebrate
        {"celebrate",
            new MoveData(
                ID: "celebrate",
                moveName: "Celebrate",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Celebrate,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Chatter
        {"chatter",
            new MoveData(
                ID: "chatter",
                moveName: "Chatter",
                moveType: "flying",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 65, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.SoundMove,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMeFirst,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { 1, -1 },
                        stringParams: new string[] { "confusion" }
                        ),
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSCTR,
                        floatParams: new float[] { 2, 5 },
                        stringParams: new string[] { "confusion" }
                        ),
                }
                ) },

        // Chip Away
        {"chipaway",
            new MoveData(
                ID: "chipaway",
                moveName: "Chip Away",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 70, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.ChipAway,
                        stringParams: new string[] { "def", "eva" }
                        ),
                }
                ) },

        // Circle Throw
        {"circlethrow",
            new MoveData(
                ID: "circlethrow",
                moveName: "Circle Throw",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 0.9f, PP: 10, priority: -6,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Whirlwind(),
                }
                ) },

        // Close Combat
        {"closecombat",
            new MoveData(
                ID: "closecombat",
                moveName: "Close Combat",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 120, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            DEFMod: -1, SPDMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Self
                        ),
                }
                ) },

        // Comet Punch
        {"cometpunch",
            new MoveData(
                ID: "cometpunch",
                moveName: "Comet Punch",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 18, accuracy: 0.85f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.PunchMove
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FuryAttack(),
                }
                ) },

        // Confuse Ray
        {"confuseray",
            new MoveData(
                ID: "confuseray",
                moveName: "Confuse Ray",
                moveType: "ghost",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { 1, -1 },
                        stringParams: new string[] { "confusion" }
                        ),
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSCTR,
                        floatParams: new float[] { 2, 5 },
                        stringParams: new string[] { "confusion" }
                        ),
                }
                ) },

        // Copycat
        {"copycat",
            new MoveData(
                ID: "copycat",
                moveName: "Copycat",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Copycat),
                }
                ) },

        // Core Enforcer
        {"coreenforcer",
            new MoveData(
                ID: "coreenforcer",
                moveName: "Core Enforcer",
                moveType: "dragon",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 100, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.CoreEnforcer(),
                },
                ZBasePower: 140
                ) },

        // Corrosive Gas
        {"corrosivegas",
            new MoveData(
                ID: "corrosivegas",
                moveName: "Corrosive Gas",
                moveType: "poison",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllAdjacent,
                accuracy: 1.0f, PP: 40,

                moveTags: new MoveTag[]
                {

                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.CorrosiveGas(),
                }
                ) },

        // Counter
        {"counter",
            new MoveData(
                ID: "counter",
                moveName: "Counter",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Self,
                accuracy: 1.0f, PP: 20, priority: -5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMeFirst,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Counter,
                        boolParams: new bool[]{ false, true },
                        floatParams: new float[] { 2f }
                    ),
                }
                ) },

        // Covet
        {"covet",
            new MoveData(
                ID: "covet",
                moveName: "Covet",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Covet(),
                }
                ) },

        // Crafty Shield
        {"craftyshield",
            new MoveData(
                ID: "craftyshield",
                moveName: "Crafty Shield",
                moveType: "fairy",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 10, priority: 3,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MatBlock,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Team,
                        boolParams: new bool[] { false },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.CraftyShield,
                        boolParams: new bool[] { true }
                        ),
                }
                ) },

        // Cut
        {"cut",
            new MoveData(
                ID: "cut",
                moveName: "Cut",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 50, accuracy: 0.95f, PP: 35,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                }
                ) },


        // D

        // Defense Curl
        {"defensecurl",
            new MoveData(
                ID: "defensecurl",
                moveName: "Defense Curl",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            DEFMod: 1
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "defensecurl"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Target
                        )
                }
                ) },

        // Destiny Bond
        {"destinybond",
            new MoveData(
                ID: "destinybond",
                moveName: "Destiny Bond",
                moveType: "ghost",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.DestinyBond,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Self,
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Detect
        {"detect",
            new MoveData(
                ID: "detect",
                moveName: "Detect",
                moveType: "fighting",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 5, priority: 4,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Protect(
                        protect: new Effects.General.Protect()
                        ),
                }
                ) },

        // Diamond Storm
        {"diamondstorm",
            new MoveData(
                ID: "diamondstorm",
                moveName: "Diamond Storm",
                moveType: "rock",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 100, accuracy: 0.95f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            DEFMod: 2
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Self,
                        chance: 0.5f
                        ),
                }
                ) },

        // Dig
        {"dig",
            new MoveData(
                ID: "dig",
                moveName: "Dig",
                moveType: "ground",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 80, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "move-charge-dig" }
                        ),

                    new MoveEffect(MoveEffectType.MultiTurnDig),
                }
                ) },

        // Disable
        {"disable",
            new MoveData(
                ID: "disable",
                moveName: "Disable",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                    MoveTag.MagicCoatSusceptible,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "disable"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Dive
        {"dive",
            new MoveData(
                ID: "dive",
                moveName: "Dive",
                moveType: "water",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 80, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "move-charge-dive" }
                        ),

                    new MoveEffect(MoveEffectType.MultiTurnDive),
                },
                MaxPower: 130
                ) },

        // Doom Desire
        {"doomdesire",
            new MoveData(
                ID: "doomdesire",
                moveName: "Doom Desire",
                moveType: "steel",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 140, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.FutureSight,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 1 },
                        stringParams: new string[] { "move-futuresight-start-doomdesire", "DEFAULT" }
                        ),
                }
                ) },

        // Double-Edge
        {"doubleedge",
            new MoveData(
                ID: "doubleedge",
                moveName: "Double-Edge",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 120, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.DoubleEdge(
                        hpLossPercent: 1f/3
                        )
                }
                ) },

        // Double Kick
        {"doublekick",
            new MoveData(
                ID: "doublekick",
                moveName: "Double Kick",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 30, accuracy: 1.0f, PP: 30,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },


                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.DoubleKick(),
                }
                ) },

        // Double Slap
        {"doubleslap",
            new MoveData(
                ID: "doubleslap",
                moveName: "Double Slap",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 15, accuracy: 0.85f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FuryAttack(),
                }
                ) },

        // Dragon Dance
        {"dragondance",
            new MoveData(
                ID: "dragondance",
                moveName: "Dragon Dance",
                moveType: "dragon",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.DanceMove,
                    MoveTag.Snatchable,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            ATKMod: 1, SPEMod: 1
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Self,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Dragon Rage
        {"dragonrage",
            new MoveData(
                ID: "dragonrage",
                moveName: "Dragon Rage",
                moveType: "dragon",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.DragonRage(damage: 40),
                }
                ) },

        // Dragon Tail
        {"dragontail",
            new MoveData(
                ID: "dragontail",
                moveName: "Dragon Tail",
                moveType: "dragon",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 0.9f, PP: 10, priority: -6,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Whirlwind(),
                }
                ) },

        // Dynamax Cannon
        {"dynamaxcannon",
            new MoveData(
                ID: "dynamaxcannon",
                moveName: "Dynamax Cannon",
                moveType: "dragon",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 100, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.DynamaxCannon,
                        floatParams: new float[] { 2 }
                        ),
                }
                ) },


        // E

        // Earthquake
        {"earthquake",
            new MoveData(
                ID: "earthquake",
                moveName: "Earthquake",
                moveType: "ground",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.AllAdjacent,
                basePower: 100, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                }
                ) },

        // Electric Terrain
        {"electricterrain",
            new MoveData(
                ID: "electricterrain",
                moveName: "Electric Terrain",
                moveType: "electric",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "electricterrain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Embargo
        {"embargo",
            new MoveData(
                ID: "embargo",
                moveName: "Embargo",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "embargo"
                            ),
                        timing: MoveEffectTiming.Unique,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Ember
        {"ember",
            new MoveData(
                ID: "ember",
                moveName: "Ember",
                moveType: "fire",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 40, accuracy: 1.0f, PP: 25,

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.1f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "burn" }
                        )
                }
                ) },

        // Encore
        {"encore",
            new MoveData(
                ID: "encore",
                moveName: "Encore",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                    MoveTag.MagicCoatSusceptible,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "encore"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Endeavor
        {"endeavor",
            new MoveData(
                ID: "endeavor",
                moveName: "Endeavor",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Endeavor),
                }
                ) },

        // Endure
        {"endure",
            new MoveData(
                ID: "endure",
                moveName: "Endure",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10, priority: 4,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Endure(),
                }
                ) },

        // Energy Ball
        {"energyball",
            new MoveData(
                ID: "energyball",
                moveName: "Energy Ball",
                moveType: "grass",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.BallMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        chance: 0.1f,
                        statStageMod: new Effects.General.StatStageMod(
                            SPDMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Expanding Force
        {"expandingforce",
            new MoveData(
                ID: "expandingforce",
                moveName: "Expanding Force",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 80, accuracy: 1.0f, PP: 20,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.ExpandingForce(
                        newTargetType: MoveTargetType.AllAdjacentOpponents,
                        conditions: new string[] { "psychicterrain" }
                        ),
                    new Effects.Moves.ExpandingForcePower(
                        damageScale: 1.5f,
                        conditions: new string[] { "psychicterrain" }
                        ),
                }
                ) },

        // Explosion
        {"explosion",
            new MoveData(
                ID: "explosion",
                moveName: "Explosion",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.AllAdjacent,
                basePower: 250, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.ExplosiveMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.FaintUser,
                        boolParams: new bool[] { true, false }
                        ),
                }
                ) },


        // F

        // Fake Out
        {"fakeout",
            new MoveData(
                ID: "fakeout",
                moveName: "Fake Out",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 40, accuracy: 1.0f, PP: 10, priority: 3,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FakeOut(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.OnceForEachTarget,
                    targetType: MoveEffectTargetType.Target
                    ),
                }
                ) },

        // False Swipe
        {"falseswipe",
            new MoveData(
                ID: "falseswipe",
                moveName: "False Swipe",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 40, accuracy: 1.0f, PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.FalseSwipe),
                }
                ) },

        // Feint
        {"feint",
            new MoveData(
                ID: "feint",
                moveName: "Feint",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 30, accuracy: 1.0f, PP: 10, priority: 2,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Feint(),
                }
                ) },

        // Fire Punch
        {"firepunch",
            new MoveData(
                ID: "firepunch",
                moveName: "Fire Punch",
                moveType: "fire",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 75, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.PunchMove
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "burn"
                            ),
                        chance: 0.1f
                        ),
                }
                ) },

        // Fire Spin
        {"firespin",
            new MoveData(
                ID: "firespin",
                moveName: "Fire Spin",
                moveType: "fire",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 35, accuracy: 0.85f, PP: 15,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Bind,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        floatParams: new float[] { 1f/8, 4 },
                        stringParams: new string[] {
                            "move-bind-start-firespin", "move-bind-damage-firespin", "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.BindTurnRange,
                        floatParams: new float[] { 4, 5 }
                        ),
                }
                ) },

        // First Impression
        {"firstimpression",
            new MoveData(
                ID: "firstimpression",
                moveName: "First Impression",
                moveType: "bug",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 10, priority: 2,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FakeOut(),
                }
                ) },

        // Flamethrower
        {"flamethrower",
            new MoveData(
                ID: "flamethrower",
                moveName: "Flamethrower",
                moveType: "fire",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 15,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "burn"
                            ),
                        chance: 0.1f
                        ),
                }
                ) },

        // Fleur Cannon
        {"fleurcannon",
            new MoveData(
                ID: "fleurcannon",
                moveName: "Fleur Cannon",
                moveType: "fairy",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 130, accuracy: 0.9f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageSelfMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Self,
                        floatParams: new float[] { -2 },
                        stringParams: new string[] { "spa" }
                        ),
                }
                ) },

        // Fling
        {"fling",
            new MoveData(
                ID: "fling",
                moveName: "Fling",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Fling,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Fly
        {"fly",
            new MoveData(
                ID: "fly",
                moveName: "Fly",
                moveType: "flying",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Any,

                basePower: 90, accuracy: 0.95f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseInGravity,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "move-charge-fly" }
                        ),
                    new MoveEffect(MoveEffectType.MultiTurnFly),
                }
                ) },

        // Flying Press
        {"flyingpress",
            new MoveData(
                ID: "flyingpress",
                moveName: "Flying Press",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Any,
                basePower: 100, accuracy: 0.95f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseInGravity,
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.FlyingPress,
                        boolParams: new bool[] { false, false, false },
                        stringParams: new string[] { "flying" }
                        ),
                    new MoveEffect(
                        MoveEffectType.DmgMinimizeState,
                        floatParams: new float[] { 2 }
                        ),
                },

                ZBasePower: 170
                ) },

        // Focus Punch
        {"focuspunch",
            new MoveData(
                ID: "focuspunch",
                moveName: "Focus Punch",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 150, accuracy: 1.0f, PP: 15, priority: -3,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMeFirst,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.FocusPunch,
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Follow Me
        {"followme",
            new MoveData(
                ID: "followme",
                moveName: "Follow Me",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20, priority: 2,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.FollowMe,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Foresight
        {"foresight",
            new MoveData(
                ID: "foresight",
                moveName: "Foresight",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "identification"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Forest's Curse
        {"forestscurse",
            new MoveData(
                ID: "forestscurse",
                moveName: "Forest's Curse",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.ForestsCurse,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "move-forestscurse", "grass" }
                        ),
                }
                ) },

        // Freeze Shock
        {"freezeshock",
            new MoveData(
                ID: "freezeshock",
                moveName: "Freeze Shock",
                moveType: "ice",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 140, accuracy: 0.9f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[]{ 2 },
                        stringParams: new string[] { "move-charge-freezeshock" }
                        ),
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.3f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "paralysis" }
                        )
                }
                ) },

        // Freeze-Dry
        {"freezedry",
            new MoveData(
                ID: "freezedry",
                moveName: "Freeze-Dry",
                moveType: "ice",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Any,
                basePower: 70, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.FreezeDry,
                        boolParams: new bool[] { false, true, false },
                        stringParams: new string[] { "water" }
                        ),
                }
                ) },

        // Frost Breath
        {"frostbreath",
            new MoveData(
                ID: "frostbreath",
                moveName: "Frost Breath",
                moveType: "ice",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 0.9f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StormThrow(),
                }
                ) },

        // Fury Attack
        {"furyattack",
            new MoveData(
                ID: "furyattack",
                moveName: "Fury Attack",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 15, accuracy: 0.85f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FuryAttack(),
                }
                ) },

        // Future Sight
        {"futuresight",
            new MoveData(
                ID: "futuresight",
                moveName: "Future Sight",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 120, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.FutureSight,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },


        // G

        // Gastro Acid
        {"gastroacid",
            new MoveData(
                ID: "gastroacid",
                moveName: "Gastro Acid",
                moveType: "poison",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.CoreEnforcer(),
                }
                ) },

        // Geomancy
        {"geomancy",
            new MoveData(
                ID: "geomancy",
                moveName: "Geomancy",
                moveType: "fairy",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "move-charge-geomancy" }
                        ),
                    new MoveEffect(
                        MoveEffectType.StatStageSelfMod,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Self,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "spa", "spd", "spe" }
                        ),
                }
                ) },

        // Giga Drain
        {"gigadrain",
            new MoveData(
                ID: "gigadrain",
                moveName: "Giga Drain",
                moveType: "grass",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 75, accuracy: 1.0f, PP: 10,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Absorb(),
                }
                ) },

        // Grass Knot
        {"grassknot",
            new MoveData(
                ID: "grassknot",
                moveName: "Grass Knot",
                moveType: "grass",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,

                accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.LowKick(),
                }
                ) },

        // Grassy Glide
        {"grassyglide",
            new MoveData(
                ID: "grassyglide",
                moveName: "Grassy Glide",
                moveType: "grass",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 70, accuracy: 1.0f, PP: 20,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.GrassyGlide(
                        mode: Effects.Moves.GrassyGlide.PriorityMode.Add, priority: 1,
                        conditions: new string[] { "grassyterrain" }
                        ),
                }
                ) },

        // Grassy Terrain
        {"grassyterrain",
            new MoveData(
                ID: "grassyterrain",
                moveName: "Grassy Terrain",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "grassyterrain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Gravity
        {"gravity",
            new MoveData(
                ID: "gravity",
                moveName: "Gravity",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 5,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "gravity" }
                        )
                }
                ) },

        // Growl
        {"growl",
            new MoveData(
                ID: "growl",
                moveName: "Growl",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllAdjacentOpponents,

                accuracy: 1.0f, PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.SoundMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            ATKMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Growth
        {"growth",
            new MoveData(
                ID: "growth",
                moveName: "Growth",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageSelfMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Self,
                        boolParams: new bool[]{ true },
                        floatParams: new float[] { 1 },
                        stringParams: new string[] { "atk","spa" }
                        ),
                    new MoveEffect(
                        MoveEffectType.StatStageGrowth,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "harshsunlight", "extremelyharshsunlight" }
                        ),
                }
                ) },

        // Guard Split
        {"guardsplit",
            new MoveData(
                ID: "guardsplit",
                moveName: "Guard Split",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.PowerSplit,
                        stringParams: new string[] { "def", "spd" }
                        ),
                    new MoveEffect(
                        MoveEffectType.PowerSplitText,
                        stringParams: new string[] { "move-guardsplit" }
                        ),
                }
                ) },

        // Guard Swap
        {"guardswap",
            new MoveData(
                ID: "guardswap",
                moveName: "Guard Swap",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.PowerSwap,
                        stringParams: new string[] { "def", "spd" }
                        ),
                    new MoveEffect(
                        MoveEffectType.PowerSwapText,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Guillotine
        {"guillotine",
            new MoveData(
                ID: "guillotine",
                moveName: "Guillotine",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                accuracy: 0.3f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Guillotine,
                        boolParams: new bool[]{ false }),

                    new MoveEffect(MoveEffectType.GuillotineAccuracy),
                }
                ) },

        // Gust
        {"gust",
            new MoveData(
                ID: "gust",
                moveName: "Gust",
                moveType: "flying",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Any,

                basePower: 40, accuracy: 1.0f, PP: 35,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.DmgFlyState,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { 2 }
                        ),
                }
                ) },

        // H

        // Hail
        {"hail",
            new MoveData(
                ID: "hail",
                moveName: "Hail",
                moveType: "ice",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 5, -1 },
                        stringParams: new string[] { "hail" }
                        ),
                }
                ) },

        // Haze
        {"haze",
            new MoveData(
                ID: "haze",
                moveName: "Haze",
                moveType: "ice",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 30,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Haze,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        stringParams: new string[] { "ALL" }
                        ),
                }
                ) },

        // Headbutt
        {"headbutt",
            new MoveData(
                ID: "headbutt",
                moveName: "Headbutt",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 70, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FakeOut(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.OnceForEachTarget,
                    targetType: MoveEffectTargetType.Target,
                    chance: 0.3f
                    ),
                }
                ) },

        // Heal Bell
        {"healbell",
            new MoveData(
                ID: "healbell",
                moveName: "Heal Bell",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                    MoveTag.SoundMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Aromatherapy,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Team,
                        boolParams: new bool[] { true }
                        ),
                    new MoveEffect(
                        MoveEffectType.Celebrate,
                        stringParams: new string[] { "move-healbell" }
                        ),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Refresh(
                        statusEffectTypes: new PokemonSEType[] { PokemonSEType.NonVolatile }
                        ),
                }
                ) },

        // Heal Block
        {"healblock",
            new MoveData(
                ID: "healblock",
                moveName: "Heal Block",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllAdjacentOpponents,
                accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "healblock"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Heal Order
        {"healorder",
            new MoveData(
                ID: "healorder",
                moveName: "Heal Order",
                moveType: "bug",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Recover,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Heal Pulse
        {"healpulse",
            new MoveData(
                ID: "healpulse",
                moveName: "Heal Pulse",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Any,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Recover,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Healing Wish
        {"healingwish",
            new MoveData(
                ID: "healingwish",
                moveName: "Healing Wish",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Wish,
                        boolParams: new bool[] { true, true },
                        floatParams: new float[] { 1f, 0 },
                        stringParams: new string[] { null, "move-wish-heal-healingwish" }
                        ),
                    new MoveEffect(
                        MoveEffectType.FaintUser,
                        boolParams: new bool[] { false, false }
                        ),
                }
                ) },

        // Heat Crash
        {"heatcrash",
            new MoveData(
                ID: "heatcrash",
                moveName: "Heat Crash",
                moveType: "fire",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseOnDynamax,
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.DmgMinimizeState,
                        floatParams: new float[] { 2 }
                        ),
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.HeavySlam(),
                }
                ) },

        // Heavy Slam
        {"heavyslam",
            new MoveData(
                ID: "heavyslam",
                moveName: "Heavy Slam",
                moveType: "steel",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseOnDynamax,
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.DmgMinimizeState,
                        floatParams: new float[] { 2 }
                        ),
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.HeavySlam(),
                }
                ) },

        // Helping Hand
        {"helpinghand",
            new MoveData(
                ID: "helpinghand",
                moveName: "Helping Hand",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AdjacentAlly,
                PP: 20, priority: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.HelpingHand,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        floatParams: new float[] { 1.5f },
                        stringParams: new string[] { "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.DmgDigState,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { 0 }
                        ),
                    new MoveEffect(
                        MoveEffectType.DmgDiveState,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { 0 }
                        ),
                    new MoveEffect(
                        MoveEffectType.DmgFlyState,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { 0 }
                        ),
                    new MoveEffect(
                        MoveEffectType.DmgShadowForceState,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { 0 }
                        ),
                }
                ) },

        // Hidden Power
        {"hiddenpower",
            new MoveData(
                ID: "hiddenpower",
                moveName: "Hidden Power",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {

                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.HiddenPower(),
                }
                ) },

        // High Jump Kick
        {"highjumpkick",
            new MoveData(
                ID: "highjumpkick",
                moveName: "High Jump Kick",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 130, accuracy: 0.05f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseInGravity,
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.JumpKick,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "move-jumpkick-fail-jumpkick" }
                        ),
                }
                ) },

        // Hold Hands
        {"holdhands",
            new MoveData(
                ID: "holdhands",
                moveName: "Hold Hands",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AdjacentAlly,
                PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.HoldHands,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        boolParams: new bool[] { true },
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Horn Attack
        {"hornattack",
            new MoveData(
                ID: "hornattack",
                moveName: "Horn Attack",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 65, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {

                }
                ) },

        // Horn Drill
        {"horndrill",
            new MoveData(
                ID: "horndrill",
                moveName: "Horn Drill",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 0.3f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Guillotine,
                        boolParams: new bool[]{ false }
                        ),

                    new MoveEffect(MoveEffectType.GuillotineAccuracy),
                }
                ) },

        // Hydro Pump
        {"hydropump",
            new MoveData(
                ID: "hydropump",
                moveName: "Hydro Pump",
                moveType: "water",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 110, accuracy: 0.8f, PP: 5,

                moveTags: new MoveTag[]
                {

                },

                MaxPower: 140
                ) },

        // Hyper Beam
        {"hyperbeam",
            new MoveData(
                ID: "hyperbeam",
                moveName: "Hyper Beam",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,

                basePower: 150, accuracy: 0.9f, PP: 5,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.RechargeTurn,
                        floatParams: new float[]{ 1 }
                        ),
                }
                ) },

        // Hyperspace Fury
        {"hyperspacefury",
            new MoveData(
                ID: "hyperspacefury",
                moveName: "Hyperspace Fury",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 100, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Feint(),
                    new Effects.Moves.FailNotPokemon(pokemonIDs: new string[] { "hoopa-unbound" }),
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            DEFMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Self
                        ),
                }
                ) },

        // Hyperspace Hole
        {"hyperspacehole",
            new MoveData(
                ID: "hyperspacehole",
                moveName: "Hyperspace Hole",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 80, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Feint(),
                }
                ) },

        // I

        // Ice Ball
        {"iceball",
            new MoveData(
                ID: "iceball",
                moveName: "Ice Ball",
                moveType: "ice",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 30, accuracy: 0.9f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.RollingMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Rollout(),
                }
                ) },

        // Ice Beam
        {"icebeam",
            new MoveData(
                ID: "icebeam",
                moveName: "Ice Beam",
                moveType: "ice",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "freeze"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: 0.1f
                        ),
                }
                ) },

        // Ice Burn
        {"iceburn",
            new MoveData(
                ID: "iceburn",
                moveName: "Ice Burn",
                moveType: "ice",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 140, accuracy: 0.9f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[]{ 2 },
                        stringParams: new string[] { "move-charge-burn" }
                        ),
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.3f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "burn" }
                        )
                }
                ) },

        // Ice Punch
        {"icepunch",
            new MoveData(
                ID: "icepunch",
                moveName: "Ice Punch",
                moveType: "ice",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 75, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.PunchMove
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.1f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "freeze" }
                        )
                }
                ) },

        // Imprison
        {"imprison",
            new MoveData(
                ID: "imprison",
                moveName: "Imprison",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "imprison"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Incinerate
        {"incinerate",
            new MoveData(
                ID: "incinerate",
                moveName: "Incinerate",
                moveType: "fire",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Incinerate,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Ingrain
        {"ingrain",
            new MoveData(
                ID: "ingrain",
                moveName: "Ingrain",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Ingrain,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 1f / 16 },
                        stringParams: new string[] { "move-ingrain", "move-ingrain-heal" }
                        ),
                }
                ) },

        // Instruct
        {"instruct",
            new MoveData(
                ID: "instruct",
                moveName: "Instruct",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.IgnoreProtect,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByMetronome
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Instruct),
                }
                ) },

        // Ion Deluge
        {"iondeluge",
            new MoveData(
                ID: "iondeluge",
                moveName: "Ion Deluge",
                moveType: "electric",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 25, priority: 1,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "iondeluge"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true
                        ),
                }
                ) },


        // J

        // Judgment
        {"judgment",
            new MoveData(
                ID: "judgment",
                moveName: "Judgment",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 100, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Judgment(),
                }
                ) },

        // Jump Kick
        {"jumpkick",
            new MoveData(
                ID: "jumpkick",
                moveName: "Jump Kick",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 100, accuracy: 0.95f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseInGravity,
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.JumpKick,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "move-jumpkick-fail-jumpkick" }
                        ),
                }
                ) },


        // K

        // Karate Chop
        {"karatechop",
            new MoveData(
                ID: "karatechop",
                moveName: "Karate Chop",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 50, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.KarateChop(),
                }
                ) },

        // King's Shield
        {"kingsshield",
            new MoveData(
                ID: "kingsshield",
                moveName: "King's Shield",
                moveType: "steel",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10, priority: 4,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Protect,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Self,
                        boolParams: new bool[] { true },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.KingsShield,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "atk" }
                        ),
                }
                ) },
        
        // Knock Off
        {"knockoff",
            new MoveData(
                ID: "knockoff",
                moveName: "Knock Off",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 65, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.KnockOff(
                        damageScale: 1.5f
                        ),
                }
                ) },


        // L

        // Lash Out
        {"lashout",
            new MoveData(
                ID: "lashout",
                moveName: "Lash Out",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 75, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.LashOut(),
                }
                ) },

        // Leech Seed
        {"leechseed",
            new MoveData(
                ID: "leechseed",
                moveName: "Leech Seed",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,

                accuracy: 0.9f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.LeechSeed,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 1f/8 },
                        stringParams: new string[] { "DEFAULT", "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.LeechSeedTypeImmunity,
                        stringParams: new string[] { "grass" }
                        ),
                }
                ) },

        // Leer
        {"leer",
            new MoveData(
                ID: "leer",
                moveName: "Leer",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllAdjacentOpponents,
                accuracy: 1.0f, PP: 30,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "def" }
                        ),
                }
                ) },

        // Light of Iruin
        {"lightofiruin",
            new MoveData(
                ID: "lightofiruin",
                moveName: "Light of Iruin",
                moveType: "fairy",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 140, accuracy: 0.9f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.DoubleEdge(
                        hpLossPercent: 0.5f
                        )
                }
                ) },

        // Light Screen
        {"lightscreen",
            new MoveData(
                ID: "lightscreen",
                moveName: "Light Screen",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Team,
                            statusID: "lightscreen"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Liquidation
        {"liquidation",
            new MoveData(
                ID: "liquidation",
                moveName: "Liquidation",
                moveType: "water",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 85, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            DEFMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: 0.2f
                        ),
                }
                ) },

        // Lock On
        {"lockon",
            new MoveData(
                ID: "lockon",
                moveName: "Lock On",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.LockOn,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 1 },
                        stringParams: new string[] { "DEFAULT", null, "DEFAULT" }
                        ),
                }
                ) },

        // Low Kick
        {"lowkick",
            new MoveData(
                ID: "lowkick",
                moveName: "Low Kick",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.LowKick(),
                },

                MaxPower: 100
                ) },

        // Lucky Chant
        {"luckychant",
            new MoveData(
                ID: "luckychant",
                moveName: "Lucky Chant",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 30,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Safeguard,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { 5 },
                        stringParams: new string[] { "move-luckychant", "move-luckychant-fail", "move-luckychant-remove" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.SafeguardLuckyChant
                        ),
                }
                ) },

        // Lunar Dance
        {"lunardance",
            new MoveData(
                ID: "lunardance",
                moveName: "Lunar Dance",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Wish,
                        boolParams: new bool[] { true, true },
                        floatParams: new float[] { 1f, 0 },
                        stringParams: new string[] { null, "move-wish-heal-lunardance" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.WishLunarDance),
                    new MoveEffect(
                        MoveEffectType.FaintUser,
                        boolParams: new bool[] { false, false }
                        ),
                }
                ) },


        // M

        // Magic Coat
        {"magiccoat",
            new MoveData(
                ID: "magiccoat",
                moveName: "Magic Coat",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 15, priority: 4,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MagicCoat,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Self,
                        stringParams: new string[] { "move-magiccoat-magiccoat" }
                        ),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.MagicCoat(
                        magicCoat: new Effects.General.MagicCoat(),
                        filters: new Effects.Filter.FilterEffect[]
                        {
                            new Effects.Filter.MoveCheck(
                                moveTags: new MoveTag[] { MoveTag.MagicCoatSusceptible }
                                )
                        }
                        )
                }
                ) },

        // Magic Powder
        {"magicpowder",
            new MoveData(
                ID: "magicpowder",
                moveName: "Magic Powder",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.PowderMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Soak,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        boolParams: new bool[] { false },
                        stringParams: new string[] { "move-soak", "psychic" }
                        ),
                }
                ) },

        // Magic Room
        {"magicroom",
            new MoveData(
                ID: "magicroom",
                moveName: "Magic Room",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 5, -1 },
                        stringParams: new string[] { "magicroom" }
                        )
                }
                ) },

        // Magnitude
        {"magnitude",
            new MoveData(
                ID: "magnitude",
                moveName: "Magnitude",
                moveType: "ground",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.AllAdjacent,
                accuracy: 1.0f, PP: 30,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Magnitude(),
                }
                ) },

        // Mat Block
        {"matblock",
            new MoveData(
                ID: "matblock",
                moveName: "Mat Block",
                moveType: "fighting",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.MatBlock(
                        protect: new Effects.General.Protect()
                        ),
                    new Effects.Moves.FakeOut(),
                }
                ) },

        // Me First
        {"mefirst",
            new MoveData(
                ID: "mefirst",
                moveName: "Me First",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AdjacentOpponent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.MeFirst),
                }
                ) },

        // Mega Drain
        {"megadrain",
            new MoveData(
                ID: "megadrain",
                moveName: "Mega Drain",
                moveType: "grass",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 40, accuracy: 1.0f, PP: 15,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Absorb(),
                },

                ZBasePower: 120
                ) },

        // Mega Kick
        {"megakick",
            new MoveData(
                ID: "megakick",
                moveName: "Mega Kick",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 120, accuracy: 0.75f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {

                }
                ) },

        // Mega Punch
        {"megapunch",
            new MoveData(
                ID: "megapunch",
                moveName: "Mega Punch",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 80, accuracy: 0.85f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.PunchMove
                }
                ) },

        // Memento
        {"memento",
            new MoveData(
                ID: "memento",
                moveName: "Memento",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.FaintUser,
                        boolParams: new bool[] { true, true }
                        ),
                    new MoveEffect(
                        MoveEffectType.StatStageMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { -2 },
                        stringParams: new string[] { "atk", "spa" }
                        ),
                }
                ) },

        // Metal Burst
        {"metalburst",
            new MoveData(
                ID: "metalburst",
                moveName: "Metal Burst",
                moveType: "steel",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Self,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseOnSubstitute,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByMeFirst,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Counter,
                        boolParams: new bool[]{ true, true },
                        floatParams: new float[] { 1.5f }
                    ),
                }
                ) },

        // Metronome
        {"metronome",
            new MoveData(
                ID: "metronome",
                moveName: "Metronome",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Metronome),
                }
                ) },

        // Milk Drink
        {"milkdrink",
            new MoveData(
                ID: "milkdrink",
                moveName: "Milk Drink",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Recover,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Mimic
        {"mimic",
            new MoveData(
                ID: "mimic",
                moveName: "Mimic",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.CannotSketch,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Mimic),
                }
                ) },

        // Mind Blown
        {"mindblown",
            new MoveData(
                ID: "mindblown",
                moveName: "Mind Blown",
                moveType: "fire",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacent,
                basePower: 150, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.ExplosiveMove,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.HPLoss,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Mind Reader
        {"mindreader",
            new MoveData(
                ID: "mindreader",
                moveName: "Mind Reader",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.LockOn,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 1 },
                        stringParams: new string[] { "DEFAULT", null, "DEFAULT" }
                        ),
                }
                ) },

        // Minimize
        {"minimize",
            new MoveData(
                ID: "minimize",
                moveName: "Minimize",
                moveType: "minimize",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Self,
                        floatParams: new float[] { -2 },
                        stringParams: new string[] { "eva" }
                        ),
                    new MoveEffect(
                        MoveEffectType.Minimize,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Self
                        ),
                }
                ) },

        // Miracle Eye
        {"miracleeye",
            new MoveData(
                ID: "miracleeye",
                moveName: "Miracle Eye",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "identification",
                            effectMode: Effects.General.InflictStatus.EffectMode.Replace,
                            customPokemonEffects: new Effects.PokemonStatuses.PokemonSE[]
                            {
                                new Effects.PokemonStatuses.Identification(
                                    types: new string[] { "dark" }
                                    ),
                            }
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Mirror Coat
        {"mirrorcoat",
            new MoveData(
                ID: "mirrorcoat",
                moveName: "Mirror Coat",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Self,
                accuracy: 1.0f, PP: 20, priority: -5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMeFirst,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.Counter,
                        boolParams: new bool[]{ false, false },
                        floatParams: new float[] { 2f }
                    ),
                }
                ) },

        // Mirror Move
        {"mirrormove",
            new MoveData(
                ID: "mirrormove",
                moveName: "Mirror Move",
                moveType: "flying",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableByMirrorMove,
                    MoveTag.UncallableBySleepTalk
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.MirrorMove),
                }
                ) },

        // Mist
        {"mist",
            new MoveData(
                ID: "mist",
                moveName: "Mist",
                moveType: "ice",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 30,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Safeguard,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 5 },
                        stringParams: new string[] { "move-mist", "move-mist-fail", "move-mist-remove" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.SafeguardMist,
                        stringParams: new string[] { "move-mist-protect", "ALL" }
                        ),
                }
                ) },

        // Misty Terrain
        {"mistyterrain",
            new MoveData(
                ID: "mistyterrain",
                moveName: "Misty Terrain",
                moveType: "fairy",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "mistyterrain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Moongeist Beam
        {"moongeistbeam",
            new MoveData(
                ID: "moongeistbeam",
                moveName: "Moongeist Beam",
                moveType: "ghost",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,

                basePower: 100, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.SunsteelStrike(),
                }
                ) },


        // N

        // Natural Gift
        {"naturalgift",
            new MoveData(
                ID: "naturalgift",
                moveName: "Natural Gift",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.NaturalGift(),
                }
                ) },

        // Nature Power
        {"naturepower",
            new MoveData(
                ID: "naturepower",
                moveName: "Nature Power",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.NaturePower),
                }
                ) },

        // Nature's Madness
        {"naturesmadness",
            new MoveData(
                ID: "naturesmadness",
                moveName: "Nature's Madness",
                moveType: "fairy",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                accuracy: 0.9f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.SuperFang(damagePercent: 0.5f),
                }
                ) },

        // Night Shade
        {"nightshade",
            new MoveData(
                ID: "nightshade",
                moveName: "Night Shade",
                moveType: "ghost",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,

                accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.SeismicToss),
                }
                ) },


        // O

        // Obstruct
        {"obstruct",
            new MoveData(
                ID: "obstruct",
                moveName: "Obstruct",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10, priority: 4,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Protect,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Self,
                        boolParams: new bool[] { true },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.KingsShield,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -2 },
                        stringParams: new string[] { "def" }
                        ),
                }
                ) },

        // Odor Sleuth
        {"odorsleuth",
            new MoveData(
                ID: "odorsleuth",
                moveName: "Odor Sleuth",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "identification"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Outrage
        {"outrage",
            new MoveData(
                ID: "outrage",
                moveName: "Outrage",
                moveType: "dragon",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Self,

                basePower: 120, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Thrash,
                        floatParams: new float[] { -1, 1 },
                        stringParams: new string[] { "confusion", "move-thrash-default" }
                        ),
                    new MoveEffect(
                        MoveEffectType.ThrashTurnRange,
                        floatParams: new float[] { 2, 3 }
                        ),
                    new MoveEffect(
                        MoveEffectType.ThrashStatusTurnRange,
                        floatParams: new float[] { 2, 5 }
                        ),
                }
                ) },


        // P

        // Parting Shot
        {"partingshot",
            new MoveData(
                ID: "partingshot",
                moveName: "Parting Shot",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.SoundMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "atk","spa" }
                        ),
                    new MoveEffect(MoveEffectType.BatonPass,
                        boolParams: new bool[] { false }
                        ),
                    new MoveEffect(MoveEffectType.FailIfEffectsFail),
                }
                ) },

        // Pay Day
        {"payday",
            new MoveData(
                ID: "payday",
                moveName: "Pay Day",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 40, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.PayDay,
                        MoveEffectTiming.AfterSuccessfulHit,
                        floatParams: new float[]{ 5 }),
                }
                ) },

        // Perish Song
        {"perishsong",
            new MoveData(
                ID: "perishsong",
                moveName: "Perish Song",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllPokemon,
                PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FakeOut(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "perishsong"
                            ),
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.OnceForEachTarget,
                    targetType: MoveEffectTargetType.Target
                    ),
                }
                ) },

        // Petal Dance
        {"petaldance",
            new MoveData(
                ID: "petaldance",
                moveName: "Petal Dance",
                moveType: "grass",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Self,

                basePower: 120, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.DanceMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Thrash,
                        floatParams: new float[] { -1, 1 },
                        stringParams: new string[] { "confusion", "move-thrash-default" }
                        ),
                    new MoveEffect(
                        MoveEffectType.ThrashTurnRange,
                        floatParams: new float[] { 2, 3 }
                        ),
                    new MoveEffect(
                        MoveEffectType.ThrashStatusTurnRange,
                        floatParams: new float[] { 2, 5 }
                        ),
                }
                ) },

        // Phantom Force
        {"phantomforce",
            new MoveData(
                ID: "phantomforce",
                moveName: "Phantom Force",
                moveType: "ghost",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "move-charge-phantomforce" }
                        ),
                    new MoveEffect(MoveEffectType.MultiTurnShadowForce),
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Feint(),
                }
                ) },

        // Photon Geyser
        {"photongeyser",
            new MoveData(
                ID: "photongeyser",
                moveName: "Photon Geyser",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 100, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.PhotonGeyser(),
                    new Effects.Moves.SunsteelStrike(),
                }
                ) },

        // Pin Missile
        {"pinmissile",
            new MoveData(
                ID: "pinmissile",
                moveName: "Pin Missile",
                moveType: "bug",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 25, accuracy: 0.95f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FuryAttack(),
                }
                ) },

        // Plasma Fist
        {"plasmafist",
            new MoveData(
                ID: "plasmafist",
                moveName: "Plasma Fist",
                moveType: "electric",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 0, -1 },
                        stringParams: new string[] { "iondeluge" }
                        )
                }
                ) },

        // Pluck
        {"pluck",
            new MoveData(
                ID: "pluck",
                moveName: "Pluck",
                moveType: "flying",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Any,
                basePower: 60, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.BugBite,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Poison Powder
        {"poisonpowder",
            new MoveData(
                ID: "poisonpowder",
                moveName: "Poison Powder",
                moveType: "poison",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 0.75f, PP: 35,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.PowderMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "poison" }
                        ),
                }
                ) },

        // Poison Sting
        {"poisonsting",
            new MoveData(
                ID: "poisonsting",
                moveName: "Poison Sting",
                moveType: "poison",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 15, accuracy: 1.0f, PP: 35,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.3f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "poison" }
                        ),
                }
                ) },

        // Poison Tail
        {"poisontail",
            new MoveData(
                ID: "poisontail",
                moveName: "Poison Tail",
                moveType: "poison",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 50, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.1f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "poison" }
                        ),
                    new MoveEffect(
                        MoveEffectType.CriticalBoost,
                        floatParams: new float[]{ 1 }
                        ),
                }
                ) },

        // Pollen Puff
        {"pollenpuff",
            new MoveData(
                ID: "pollenpuff",
                moveName: "Pollen Puff",
                moveType: "bug",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.BombMove,
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Recover,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        effectFilters: new MoveEffectFilter[]
                        {
                            MoveEffectFilter.AlliesOnly
                        },
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(MoveEffectType.PollenPuff),
                }
                ) },

        // Poltergeist
        {"poltergeist",
            new MoveData(
                ID: "poltergeist",
                moveName: "Poltergeist",
                moveType: "ghost",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 110, accuracy: 0.9f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Poltergeist(),
                }
                ) },

        // Pound
        {"pound",
            new MoveData(
                ID: "pound",
                moveName: "Pound",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 40, accuracy: 1.0f, PP: 35,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                }
                ) },

        // Powder
        {"powder",
            new MoveData(
                ID: "powder",
                moveName: "Powder",
                moveType: "bug",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 20, priority: 1,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.PowderMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Powder,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        floatParams: new float[] { 0.25f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT", "fire" }
                        ),
                }
                ) },

        // Power Split
        {"powersplit",
            new MoveData(
                ID: "powersplit",
                moveName: "Power Split",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.PowerSplit,
                        stringParams: new string[] { "atk", "spa" }
                        ),
                    new MoveEffect(
                        MoveEffectType.PowerSplitText,
                        stringParams: new string[] { "move-powersplit" }
                        ),
                }
                ) },

        // Power Swap
        {"powerswap",
            new MoveData(
                ID: "powerswap",
                moveName: "Power Swap",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.PowerSwap,
                        stringParams: new string[] { "atk", "spa" }
                        ),
                    new MoveEffect(
                        MoveEffectType.PowerSwapText,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Power Trick
        {"powertrick",
            new MoveData(
                ID: "powertrick",
                moveName: "Power Trick",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.PowerTrick,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT", "atk", "def" }
                        ),
                }
                ) },

        // Power Trip
        {"powertrip",
            new MoveData(
                ID: "powertip",
                moveName: "Power Trip",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 20, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StoredPower(),
                }
                ) },

        // Protect
        {"protect",
            new MoveData(
                ID: "protect",
                moveName: "Protect",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10, priority: 4,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Protect(
                        protect: new Effects.General.Protect()
                        ),
                }
                ) },

        // Psybeam
        {"psybeam",
            new MoveData(
                ID: "psybeam",
                moveName: "Psybeam",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 65, accuracy: 1.0f, PP: 20,

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.1f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "confusion" }
                        ),
                }
                ) },

        // Psychic
        {"psychic",
            new MoveData(
                ID: "psychic",
                moveName: "Psychic",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        chance: 0.1f,
                        statStageMod: new Effects.General.StatStageMod(
                            SPDMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Psychic Terrain
        {"psychicterrain",
            new MoveData(
                ID: "psychicterrain",
                moveName: "Psychic Terrain",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "psychicterrain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Psyshock
        {"psyshock",
            new MoveData(
                ID: "psyshock",
                moveName: "Psyshock",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 80, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Psyshock,
                        stringParams: new string[] { "def" }
                        ),
                }
                ) },

        // Psystrike
        {"psystrike",
            new MoveData(
                ID: "psystrike",
                moveName: "Psystrike",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 100, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Psyshock,
                        stringParams: new string[] { "def" }
                        ),
                }
                ) },

        // Psywave
        {"psywave",
            new MoveData(
                ID: "psywave",
                moveName: "Psywave",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Psywave(),
                }
                ) },

        // Punishment
        {"punishment",
            new MoveData(
                ID: "punishment",
                moveName: "Punishment",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 20, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Punishment(),
                }
                ) },

        // Pursuit
        {"pursuit",
            new MoveData(
                ID: "pursuit",
                moveName: "Pursuit",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 40, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Pursuit(),
                }
                ) },


        // Q

        // Quash
        {"quash",
            new MoveData(
                ID: "quash",
                moveName: "Quash",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Quash,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Quick Attack
        {"quickattack",
            new MoveData(
                ID: "quickattack",
                moveName: "Quick Attack",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 40, accuracy: 1.0f, PP: 30, priority: 1,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                }
                ) },

        // Quick Guard
        {"quickguard",
            new MoveData(
                ID: "quickguard",
                moveName: "Quick Guard",
                moveType: "fighting",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 15, priority: 3,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MatBlock,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Team,
                        boolParams: new bool[] { true },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(MoveEffectType.QuickGuard),
                }
                ) },


        // R

        // Rage
        {"rage",
            new MoveData(
                ID: "rage",
                moveName: "Rage",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 20, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Rage,
                        floatParams: new float[] { 1 },
                        stringParams: new string[] {"atk" }
                        ),
                }
                ) },

        // Rage Powder
        {"ragepowder",
            new MoveData(
                ID: "ragepowder",
                moveName: "Rage Powder",
                moveType: "bug",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20, priority: 2,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.FollowMe,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Rain Dance
        {"raindance",
            new MoveData(
                ID: "raindance",
                moveName: "Rain Dance",
                moveType: "water",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 5,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 5, -1 },
                        stringParams: new string[] { "rain" }
                        ),
                }
                ) },

        // Razor Leaf
        {"razorleaf",
            new MoveData(
                ID: "razorleaf",
                moveName: "Razor Leaf",
                moveType: "grass",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.AllAdjacentOpponents,

                basePower: 55, accuracy: 0.95f, PP: 25,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.CriticalBoost,
                        floatParams: new float[]{ 1 }
                        ),
                }
                ) },

        // Razor Wind
        {"razorwind",
            new MoveData(
                ID: "razorwind",
                moveName: "Razor Wind",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,

                basePower: 80, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[]{ 2 },
                        stringParams: new string[] { "move-charge-razorwind" }
                        ),

                    new MoveEffect(
                        MoveEffectType.CriticalBoost,
                        floatParams: new float[]{ 1 }
                        ),
                }
                ) },

        // Recover
        {"recover",
            new MoveData(
                ID: "recover",
                moveName: "Recover",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Recover,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Recycle
        {"recycle",
            new MoveData(
                ID: "recycle",
                moveName: "Recycle",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Recycle,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Reflect
        {"reflect",
            new MoveData(
                ID: "reflect",
                moveName: "Reflect",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        inflictStatus: new Effects.General.InflictStatus(
                            statusType: StatusType.Team,
                            statusID: "reflect"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Refresh
        {"refresh",
            new MoveData(
                ID: "refresh",
                moveName: "Refresh",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Refresh(
                        statuses: new string[] { "burn", "paralysis", "poison", },
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Relic Song
        {"relicsong",
            new MoveData(
                ID: "relicsong",
                moveName: "Relic Song",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 75, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.SoundMove,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.1f,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "sleep" }
                        ),
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.RelicSong(
                        form1: "meloetta-aria", form2: "meloetta-pirouette"
                        ),
                }
                ) },

        // Rest
        {"rest",
            new MoveData(
                ID: "rest",
                moveName: "Rest",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Rest,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Self,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Rising Voltage
        {"risingvoltage",
            new MoveData(
                ID: "risingvoltage",
                moveName: "Rising Voltage",
                moveType: "electric",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 70, accuracy: 1.0f, PP: 20,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.RisingVoltage(
                        damageScale: 2f,
                        conditions: new string[] { "electricterrain" }
                        )
                }
                ) },

        // Roar
        {"roar",
            new MoveData(
                ID: "roar",
                moveName: "Roar",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,

                PP: 20, priority: -6,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.SoundMove,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Whirlwind(),
                }
                ) },

        // Role Play
        {"roleplay",
            new MoveData(
                ID: "roleplay",
                moveName: "Role Play",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.RolePlay,
                        boolParams: new bool[] { true, false },
                        stringParams: new string[] { "move-roleplay" }
                        ),
                }
                ) },

        // Rolling Kick
        {"rollingkick",
            new MoveData(
                ID: "rollingkick",
                moveName: "Rolling Kick",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 65, accuracy: 0.85f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FakeOut(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.OnceForEachTarget,
                    targetType: MoveEffectTargetType.Target,
                    chance: 0.3f
                    ),
                }
                ) },

        // Rollout
        {"rollout",
            new MoveData(
                ID: "rollout",
                moveName: "Rollout",
                moveType: "rock",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 30, accuracy: 0.9f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.RollingMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Rollout(),
                }
                ) },

        // Roost
        {"roost",
            new MoveData(
                ID: "roost",
                moveName: "Roost",
                moveType: "flying",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Recover,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.Roost,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Self
                        ),
                    new MoveEffect(
                        MoveEffectType.RoostTypeLoss,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Self,
                        stringParams: new string[] { "flying" }
                        ),
                }
                ) },


        // S

        // Sacred Sword
        {"sacredsword",
            new MoveData(
                ID: "sacredsword",
                moveName: "Sacred Sword",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.ChipAway,
                        stringParams: new string[] { "def", "eva" }
                        ),
                }
                ) },

        // Safeguard
        {"safeguard",
            new MoveData(
                ID: "safeguard",
                moveName: "Safeguard",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Safeguard,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 5 },
                        stringParams: new string[] { "move-safeguard", "DEFAULT", "move-safeguard-remove" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.SafeguardStatus,
                        boolParams: new bool[] { false, true },
                        stringParams: new string[] { "move-safeguard-protect", "burn", "confusion", "freeze", "paralysis", "poison", "poison2", "sleep" }
                        ),
                }
                ) },

        // Sand Attack
        {"sandattack",
            new MoveData(
                ID: "sandattack",
                moveName: "Sand Attack",
                moveType: "ground",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,

                accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "acc" }
                        ),
                }
                ) },

        // Sandstorm
        {"sandstorm",
            new MoveData(
                ID: "sandstorm",
                moveName: "Sandstorm",
                moveType: "rock",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 5,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 5, -1 },
                        stringParams: new string[] { "sandstorm" }
                        ),
                }
                ) },

        // Scratch
        {"scratch",
            new MoveData(
                ID: "scratch",
                moveName: "Scratch",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 40, accuracy: 1.0f, PP: 35,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {

                }
                ) },

        // Secret Power
        {"secretpower",
            new MoveData(
                ID: "secretpower",
                moveName: "Secret Power",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 70, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.SecretPower(secondaryEffectChance: 0.3f),
                }
                ) },

        // Secret Sword
        {"secretsword",
            new MoveData(
                ID: "secretsword",
                moveName: "Secret Sword",
                moveType: "fighting",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 85, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Psyshock,
                        stringParams: new string[] { "def" }
                        ),
                }
                ) },

        // Seismic Toss
        {"seismictoss",
            new MoveData(
                ID: "seismictoss",
                moveName: "Seismic Toss",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.SeismicToss),
                }
                ) },

        // Shadow Force
        {"shadowforce",
            new MoveData(
                ID: "shadowforce",
                moveName: "Shadow Force",
                moveType: "ghost",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 120, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "move-charge-phantomforce" }
                        ),
                    new MoveEffect(MoveEffectType.MultiTurnShadowForce),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Feint(),
                }
                ) },

        // Shadow Sneak
        {"shadowsneak",
            new MoveData(
                ID: "shadowsneak",
                moveName: "Shadow Sneak",
                moveType: "ghost",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 40, accuracy: 1.0f, PP: 30, priority: 1,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                }
                ) },

        // Shell Side Arm
        {"shellsidearm",
            new MoveData(
                ID: "shellsidearm",
                moveName: "Shell Side Arm",
                moveType: "poison",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 10,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.ShellSideArm(),
                }
                ) },

        // Shell Smash
        {"shellsmash",
            new MoveData(
                ID: "shellsmash",
                moveName: "Shell Smash",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            ATKMod: 2, SPAMod: 2, SPEMod: 2, DEFMod: -1, SPDMod: -1
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Self,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Shell Trap
        {"shelltrap",
            new MoveData(
                ID: "shelltrap",
                moveName: "Shell Trap",
                moveType: "fire",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 150, accuracy: 1.0f, PP: 5, priority: -3,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMeFirst,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.ShellTrap,
                        boolParams: new bool[] { true, true },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Sing
        {"sing",
            new MoveData(
                ID: "sing",
                moveName: "Sing",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,

                accuracy: 0.55f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.SoundMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "sleep" }
                        ),
                }
                ) },

        // Sketch
        {"sketch",
            new MoveData(
                ID: "sketch",
                moveName: "Sketch",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 1,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.CannotSketch,
                    MoveTag.IgnoreProtect,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Sketch,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Skill Swap
        {"skillswap",
            new MoveData(
                ID: "skillswap",
                moveName: "Skill Swap",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.SkillSwap,
                        boolParams: new bool[] { true, true },
                        stringParams: new string[] { "move-skillswap" }
                        ),
                }
                ) },

        // Skull Bash
        {"skullbash",
            new MoveData(
                ID: "skullbash",
                moveName: "Skull Bash",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 130, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[]{ 2 },
                        stringParams: new string[] { "move-charge-skullbash" }
                        ),
                    new MoveEffect(
                        MoveEffectType.StatStageSelfMod,
                        MoveEffectTiming.OnChargeTurn,
                        MoveEffectTargetType.Self,
                        floatParams: new float[] { 1 },
                        stringParams: new string[] { "def" }
                        ),
                }
                ) },

        // Sky Attack
        {"skyattack",
            new MoveData(
                ID: "skyattack",
                moveName: "Sky Attack",
                moveType: "flying",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 140, accuracy: 0.9f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[]{ 2 },
                        stringParams: new string[] { "move-charge-skyattack" }
                        ),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FakeOut(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.OnceForEachTarget,
                    targetType: MoveEffectTargetType.Target,
                    chance: 0.3f
                    ),
                }
                ) },

        // Sky Drop
        {"skydrop",
            new MoveData(
                ID: "skydrop",
                moveName: "Sky Drop",
                moveType: "flying",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Any,
                basePower: 60, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotUseInGravity,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.SkyDrop,
                        stringParams: new string[] { "DEFAULT", "DEFAULT", "DEFAULT" }
                        ),

                    new MoveEffect(MoveEffectType.MultiTurnFly),
                }
                ) },

        // Slack Off
        {"slackoff",
            new MoveData(
                ID: "slackoff",
                moveName: "Slack Off",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Recover,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Slam
        {"slam",
            new MoveData(
                ID: "slam",
                moveName: "Slam",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 80, accuracy: 0.75f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                }
                ) },

        // Sleep Talk
        {"sleeptalk",
            new MoveData(
                ID: "sleeptalk",
                moveName: "Sleep Talk",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                    MoveTag.UncallableBySleepTalk
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.SleepTalk),
                }
                ) },

        // Smack Down
        {"smackdown",
            new MoveData(
                ID: "smackdown",
                moveName: "Smack Down",
                moveType: "rock",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 50, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.ThousandArrows,
                        boolParams: new bool[] { true, false, false, false },
                        floatParams: new float[] { 1 },
                        stringParams: new string[] { "flying" }
                        ),
                    new MoveEffect(
                        MoveEffectType.DmgFlyState,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 1 }
                        ),
                    new MoveEffect(
                        MoveEffectType.SmackDown,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Snarl
        {"snarl",
            new MoveData(
                ID: "snarl",
                moveName: "Snarl",
                moveType: "dark",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 55, accuracy: 0.95f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.SoundMove,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.StatStageMod,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "acc" }
                        ),
                }
                ) },

        // Snatch
        {"snatch",
            new MoveData(
                ID: "snatch",
                moveName: "Snatch",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10, priority: 4,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Snatch,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }),
                }
                ) },

        // Snore
        {"snore",
            new MoveData(
                ID: "snore",
                moveName: "Snore",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 50, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.SoundMove,
                    MoveTag.UncallableByMetronome,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Snore(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.OnceForEachTarget,
                    targetType: MoveEffectTargetType.Target,
                    chance: 0.3f
                    ),
                }
                ) },

        // Soak
        {"soak",
            new MoveData(
                ID: "soak",
                moveName: "Soak",
                moveType: "water",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Soak,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        boolParams: new bool[] { false },
                        stringParams: new string[] { "move-soak", "water" }
                        ),
                }
                ) },

        // Soft-Boiled
        {"softboiled",
            new MoveData(
                ID: "softboiled",
                moveName: "Soft-Boiled",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Recover,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.5f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Solar Beam
        {"solarbeam",
            new MoveData(
                ID: "solarbeam",
                moveName: "Solar Beam",
                moveType: "grass",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,

                basePower: 120, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[]{ 2 },
                        stringParams: new string[] { "move-charge-solarbeam" }
                        ),
                    new MoveEffect(
                        MoveEffectType.InstantMultiTurnWeather,
                        stringParams: new string[]{ "harshsunlight" }
                        ),
                }
                ) },

        // Solar Blade
        {"solarblade",
            new MoveData(
                ID: "solarblade",
                moveName: "Solar Blade",
                moveType: "grass",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 125, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableBySleepTalk,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MultiTurnAttack,
                        floatParams: new float[]{ 2 },
                        stringParams: new string[] { "move-charge-solarbeam" }
                        ),
                    new MoveEffect(
                        MoveEffectType.InstantMultiTurnWeather,
                        stringParams: new string[]{ "harshsunlight" }
                        ),
                }
                ) },

        // Sonic Boom
        {"sonicboom",
            new MoveData(
                ID: "sonicboom",
                moveName: "Sonic Boom",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                accuracy: 0.9f, PP: 20,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.DragonRage(damage: 20),
                }
                ) },

        // Spectral Thief
        {"spectralthief",
            new MoveData(
                ID: "spectralthief",
                moveName: "Spectral Thief",
                moveType: "ghost",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                    MoveTag.MakesContact,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.SpectralThief,
                        effectTiming: MoveEffectTiming.BeforeTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        boolParams: new bool[] { true },
                        stringParams: new string[] { "all" }
                        ),
                }
                ) },

        // Speed Swap
        {"speedswap",
            new MoveData(
                ID: "speedswap",
                moveName: "Speed Swap",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.PowerSwap,
                        stringParams: new string[] { "spe" }
                        ),
                    new MoveEffect(
                        MoveEffectType.PowerSwapText,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Spikes
        {"spikes",
            new MoveData(
                ID: "spikes",
                moveName: "Spikes",
                moveType: "ground",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamOpponent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazard,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1, 3 },
                        stringParams: new string[] { "move-spikes", "move-spikes-fail", "move-spikes-remove" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazardDamage,
                        floatParams: new float[] { 1f/8, 1f/16 },
                        stringParams: new string[] { "move-spikes-damage" }
                        ),
                }
                ) },

        // Spiky Shield
        {"spikyshield",
            new MoveData(
                ID: "spikyshield",
                moveName: "Spiky Shield",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10, priority: 4,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Protect,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Self,
                        boolParams: new bool[] { true },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.SpikyShield,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 1f/8 },
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Spore
        {"spore",
            new MoveData(
                ID: "spore",
                moveName: "Spore",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.PowderMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "sleep"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Spotlight
        {"spotlight",
            new MoveData(
                ID: "spotlight",
                moveName: "Spotlight",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15, priority: 3,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.FollowMe,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Stealth Rock
        {"stealthrock",
            new MoveData(
                ID: "stealthrock",
                moveName: "Stealth Rock",
                moveType: "rock",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamOpponent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazard,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { -1, 1 },
                        stringParams: new string[] { "move-stealthrock", "move-stealthrock-fail", "move-stealthrock-remove" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazardDamage,
                        floatParams: new float[] { 1f/8, 0 },
                        stringParams: new string[] { "move-stealthrock-damage" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazardStealthRock,
                        stringParams: new string[] { "rock" }
                        ),
                }
                ) },
        
        // Steam Eruption
        {"steameruption",
            new MoveData(
                ID: "steameruption",
                moveName: "Steam Eruption",
                moveType: "water",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 110, accuracy: 0.95f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "burn"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: 0.3f
                        ),
                    new Effects.Moves.HealBeforeUse(
                        statuses: new string[] { "burn" }
                        ),
                    new Effects.Moves.Refresh(
                        statuses: new string[] { "burn" }
                        ),
                }
                ) },

        // Steel Roller
        {"steelroller",
            new MoveData(
                ID: "steelroller",
                moveName: "Steel Roller",
                moveType: "steel",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 130, accuracy: 1f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.SteelRoller(),
                }
                ) },

        // Sticky Web
        {"stickyweb",
            new MoveData(
                ID: "stickyweb",
                moveName: "Sticky Web",
                moveType: "bug",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamOpponent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazard,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1, 1 },
                        stringParams: new string[] { "move-stickyweb", "move-stickyweb-fail", "move-stickyweb-remove" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazardStickyWeb,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "spe" }
                        ),
                }
                ) },

        // Stomp
        {"stomp",
            new MoveData(
                ID: "stomp",
                moveName: "Stomp",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 65, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.DmgMinimizeState,
                        floatParams: new float[] { 2 }
                        ),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.FakeOut(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "flinch"
                            ),
                    timing: MoveEffectTiming.AfterTargetImpact,
                    occurrence: MoveEffectOccurrence.OnceForEachTarget,
                    targetType: MoveEffectTargetType.Target,
                    chance: 0.3f
                    ),
                }
                ) },

        // Stored Power
        {"storedpower",
            new MoveData(
                ID: "storedpower",
                moveName: "Stored Power",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 20, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StoredPower(),
                }
                ) },

        // Storm Throw
        {"stormthrow",
            new MoveData(
                ID: "stormthrow",
                moveName: "Storm Throw",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StormThrow(),
                }
                ) },

        // Stuff Cheeks
        {"stuffcheeks",
            new MoveData(
                ID: "stuffcheeks",
                moveName: "Stuff Cheeks",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StuffCheeks,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        floatParams: new float[] { 2 },
                        stringParams: new string[] { "def" }
                        ),
                }
                ) },

        // Stun Spore
        {"stunspore",
            new MoveData(
                ID: "stunspore",
                moveName: "Stun Spore",
                moveType: "grass",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1f, PP: 30,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.PowderMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "paralysis" }
                        ),
                }
                ) },

        // Substitute
        {"substitute",
            new MoveData(
                ID: "substitute",
                moveName: "Substitute",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Substitute,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Self,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 0.25f, 0.25f },
                        stringParams: new string[] { "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Sucker Punch
        {"suckerpunch",
            new MoveData(
                ID: "suckerpunch",
                moveName: "Sucker Punch",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 70, accuracy: 1.0f, PP: 5, priority: 1,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.SuckerPunch(),
                }
                ) },

        // Superpower
        {"superpower",
            new MoveData(
                ID: "superpower",
                moveName: "Superpower",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 120, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            ATKMod: -1, DEFMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Self
                        ),
                }
                ) },

        // Supersonic
        {"supersonic",
            new MoveData(
                ID: "supersonic",
                moveName: "Supersonic",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,

                accuracy: 0.55f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.SoundMove
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true, false },
                        floatParams: new float[] { -1, -1 },
                        stringParams: new string[] { "confusion" }
                        ),
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSCTR,
                        floatParams: new float[] { 2, 5 }
                        ),
                }
                ) },

        // Sunny Day
        {"sunnyday",
            new MoveData(
                ID: "sunnyday",
                moveName: "Sunny Day",
                moveType: "fire",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 5,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 5 },
                        stringParams: new string[] { "harshsunlight" }
                        ),
                }
                ) },

        // Sunsteel Strike
        {"sunsteelstrike",
            new MoveData(
                ID: "sunsteelstrike",
                moveName: "Sunsteel Strike",
                moveType: "steel",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 100, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.SunsteelStrike(),
                }
                ) },

        // Super Fang
        {"superfang",
            new MoveData(
                ID: "superfang",
                moveName: "Super Fang",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                accuracy: 0.9f, PP: 10,
                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.SuperFang(damagePercent: 0.5f),
                }
                ) },

        // Surf
        {"surf",
            new MoveData(
                ID: "surf",
                moveName: "Surf",
                moveType: "water",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacent,
                basePower: 90, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.DmgDiveState,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { 2 }
                        ),
                }
                ) },

        // Switcheroo
        {"switcheroo",
            new MoveData(
                ID: "switcheroo",
                moveName: "Switcheroo",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Trick,
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Swords Dance
        {"swordsdance",
            new MoveData(
                ID: "swordsdance",
                moveName: "Swords Dance",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.DanceMove,
                    MoveTag.Snatchable,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            ATKMod: 2
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Self,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Synchronoise
        {"synchronoise",
            new MoveData(
                ID: "synchronoise",
                moveName: "Synchronoise",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.AllAdjacent,
                basePower: 120, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {

                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Synchronoise(),
                }
                ) },


        // T

        // Tackle
        {"tackle",
            new MoveData(
                ID: "tackle",
                moveName: "Tackle",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 50, accuracy: 1.0f, PP: 35,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {

                }
                ) },

        // Tail Whip
        {"tailwhip",
            new MoveData(
                ID: "tailwhip",
                moveName: "Tail Whip",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllAdjacentOpponents,

                accuracy: 1.0f, PP: 30,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageMod,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "def" }
                        ),
                }
                ) },

        // Take Down
        {"takedown",
            new MoveData(
                ID: "takedown",
                moveName: "Take Down",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 90, accuracy: 0.85f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.DoubleEdge(
                        hpLossPercent: 0.25f
                        )
                }
                ) },

        // Tar Shot
        {"tarshot",
            new MoveData(
                ID: "tarshot",
                moveName: "Tar Shot",
                moveType: "rock",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            SPEMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "tarshot"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Taunt
        {"taunt",
            new MoveData(
                ID: "taunt",
                moveName: "Taunt",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                    MoveTag.MagicCoatSusceptible,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "taunt"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Teatime
        {"teatime",
            new MoveData(
                ID: "teatime",
                moveName: "Teatime",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.AllPokemon,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Teatime,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Techno Blast
        {"technoblast",
            new MoveData(
                ID: "technoblast",
                moveName: "Techno Blast",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 120, accuracy: 1.0f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.TechnoBlast),
                }
                ) },

        // Teleport
        {"teleport",
            new MoveData(
                ID: "teleport",
                moveName: "Teleport",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 20, priority: -6,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.BatonPass,
                        boolParams: new bool[] { false }
                        ),
                }
                ) },

        // Terrain Pulse
        {"terrainpulse",
            new MoveData(
                ID: "terrainpulse",
                moveName: "Terrain Pulse",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 50, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.PulseMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.TerrainPulse(),
                }
                ) },
        
        // Thief
        {"thief",
            new MoveData(
                ID: "thief",
                moveName: "Thief",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 60, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMeFirst,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Covet(),
                }
                ) },

        // Thousand Arrows
        {"thousandarrows",
            new MoveData(
                ID: "thousandarrows",
                moveName: "Thousand Arrows",
                moveType: "ground",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 90, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.ThousandArrows,
                        boolParams: new bool[] { true, false, false, false },
                        floatParams: new float[] { 1 },
                        stringParams: new string[] { "flying" }
                        ),
                    new MoveEffect(
                        MoveEffectType.DmgFlyState,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { 1 }
                        ),
                    new MoveEffect(
                        MoveEffectType.SmackDown,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Thousand Waves
        {"thousandwaves",
            new MoveData(
                ID: "thousandwaves",
                moveName: "Thousand Waves",
                moveType: "ground",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.AllAdjacentOpponents,
                basePower: 90, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByMetronome
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Block,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        stringParams: new string[] { "DEFAULT", "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Thrash
        {"thrash",
            new MoveData(
                ID: "thrash",
                moveName: "Thrash",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Self,

                basePower: 120, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Thrash,
                        floatParams: new float[] { -1, 1 },
                        stringParams: new string[] { "confusion", "move-thrash-default" }
                        ),
                    new MoveEffect(
                        MoveEffectType.ThrashTurnRange,
                        floatParams: new float[] { 2, 3 }
                        ),
                    new MoveEffect(
                        MoveEffectType.ThrashStatusTurnRange,
                        floatParams: new float[] { 2, 5 }
                        ),
                }
                ) },

        // Thunder Punch
        {"thunderpunch",
            new MoveData(
                ID: "thunderpunch",
                moveName: "Thunder Punch",
                moveType: "electric",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 75, accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.PunchMove
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "paralysis"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: 0.1f
                        ),
                }
                ) },

        // Thunder Wave
        {"thunderwave",
            new MoveData(
                ID: "thunderwave",
                moveName: "Thunder Wave",
                moveType: "electric",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 0.9f, PP: 20,
                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "paralysis"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Thunderbolt
        {"thunderbolt",
            new MoveData(
                ID: "thunderbolt",
                moveName: "Thunderbolt",
                moveType: "electric",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 90, accuracy: 1.0f, PP: 15,

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "paralysis"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: 0.1f
                        ),
                }
                ) },

        // Torment
        {"torment",
            new MoveData(
                ID: "torment",
                moveName: "Torment",
                moveType: "dark",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreSubstitute,
                    MoveTag.MagicCoatSusceptible,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "torment"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target
                        ),
                }
                ) },

        // Toxic Spikes
        {"toxicspikes",
            new MoveData(
                ID: "toxicspikes",
                moveName: "Toxic Spikes",
                moveType: "poison",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamOpponent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazard,
                        effectTiming: MoveEffectTiming.AfterSuccessfulMoveUse,
                        effectTargetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1, 2 },
                        stringParams: new string[] { "move-toxicspikes", "move-toxicspikes-fail", "move-toxicspikes-remove" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazardToxicSpikes,
                        boolParams: new bool[] { false },
                        floatParams: new float[] { -1, 1 },
                        stringParams: new string[] { "poison" }
                        ),
                    new MoveEffect(
                        effectType: MoveEffectType.EntryHazardToxicSpikes,
                        boolParams: new bool[] { true },
                        floatParams: new float[] { -1, 2 },
                        stringParams: new string[] { "poison2" }
                        ),
                }
                ) },

        // Transform
        {"transform",
            new MoveData(
                ID: "transform",
                moveName: "Transform",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.CannotInstruct,
                    MoveTag.IgnoreSubstitute,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Transform,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        stringParams: new string[] { "DEFAULT" }
                        ),
                }
                ) },

        // Trick
        {"trick",
            new MoveData(
                ID: "trick",
                moveName: "Trick",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Trick,
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // Trick Room
        {"trickroom",
            new MoveData(
                ID: "trickroom",
                moveName: "Trick Room",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 5, priority: -7,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 5 },
                        stringParams: new string[] { "trickroom" }
                        )
                }
                ) },

        // Trick-or-Treat
        {"trickortreat",
            new MoveData(
                ID: "trickortreat",
                moveName: "Trick-or-Treat",
                moveType: "ghost",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.ForestsCurse,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 3 },
                        stringParams: new string[] { "move-forestscurse", "ghost" }
                        ),
                }
                ) },

        // Triple Axel
        {"tripleaxel",
            new MoveData(
                ID: "tripleaxel",
                moveName: "Triple Axel",
                moveType: "ice",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 20, accuracy: 0.9f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.AccuracyCheckEveryHit,
                    MoveTag.MakesContact,
                    MoveTag.PunchMove
                },


                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.TripleKick(),
                }
                ) },

        // Triple Kick
        {"triplekick",
            new MoveData(
                ID: "triplekick",
                moveName: "Triple Kick",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 10, accuracy: 0.9f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.AccuracyCheckEveryHit,
                    MoveTag.MakesContact,
                    MoveTag.PunchMove
                },


                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.TripleKick(),
                }
                ) },

        // Twineedle
        {"twineedle",
            new MoveData(
                ID: "twineedle",
                moveName: "Twineedle",
                moveType: "bug",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 25, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictPokemonSC,
                        MoveEffectTiming.AfterTargetImpact,
                        MoveEffectTargetType.Target,
                        effectChance: 0.2f,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "poison" }
                        ),
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.DoubleKick(),
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "poison"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        chance: 0.2f
                        ),

                }
                ) },


        // U

        // Uproar
        {"uproar",
            new MoveData(
                ID: "uproar",
                moveName: "Uproar",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Self,
                basePower: 90, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.SoundMove,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Uproar,
                        floatParams: new float[] { 3 },
                        stringParams: new string[] { "DEFAULT", "DEFAULT", "DEFAULT" }
                        ),
                }
                ) },

        // U-Turn
        {"uturn",
            new MoveData(
                ID: "uturn",
                moveName: "U-Turn",
                moveType: "bug",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 70, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.BatonPass,
                        boolParams: new bool[] { false }
                        ),
                }
                ) },


        // V

        // V-Create
        {"vcreate",
            new MoveData(
                ID: "vcreate",
                moveName: "V-Create",
                moveType: "fire",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 180, accuracy: 0.95f, PP: 5,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.StatStageSelfMod,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Self,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "def", "spd", "spe" }
                        ),
                }
                ) },

        // Vine Whip
        {"vinewhip",
            new MoveData(
                ID: "vinewhip",
                moveName: "Vine Whip",
                moveType: "grass",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 45, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                }
                ) },

        // Vise Grip
        {"visegrip",
            new MoveData(
                ID: "visegrip",
                moveName: "Vise Grip",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                basePower: 55, accuracy: 1.0f, PP: 30,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                }
                ) },

        // Volt Switch
        {"voltswitch",
            new MoveData(
                ID: "voltswitch",
                moveName: "Volt Switch",
                moveType: "electric",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 70, accuracy: 1.0f, PP: 20,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(MoveEffectType.BatonPass,
                        boolParams: new bool[] { false }
                        ),
                }
                ) },


        // W

        // Water Gun
        {"watergun",
            new MoveData(
                ID: "watergun",
                moveName: "Water Gun",
                moveType: "water",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 40, accuracy: 1.0f, PP: 25,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {

                }
                ) },

        // Weather Ball
        {"weatherball",
            new MoveData(
                ID: "weatherball",
                moveName: "Weather Ball",
                moveType: "normal",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 50, accuracy: 1.0f, PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.BallMove
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.WeatherBall(),
                },

                ZBasePower: 160
                ) },

        // Whirlwind
        {"whirlwind",
            new MoveData(
                ID: "whirlwind",
                moveName: "Whirlwind",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,

                PP: 20, priority: -6,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.UncallableByAssist,
                    MoveTag.UncallableByCopycat,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Whirlwind(),
                }
                ) },

        // Wide Guard
        {"wideguard",
            new MoveData(
                ID: "wideguard",
                moveName: "Wide Guard",
                moveType: "rock",
                category: MoveCategory.Status,
                targetType: MoveTargetType.TeamAlly,
                PP: 10, priority: 3,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                    MoveTag.UncallableByMetronome,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.MatBlock,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Team,
                        boolParams: new bool[] { true },
                        stringParams: new string[] { "DEFAULT", "DEFAULT" }
                        ),
                    new MoveEffect(MoveEffectType.WideGuard),
                }
                ) },

        // Will-O-Wisp
        {"willowisp",
            new MoveData(
                ID: "willowisp",
                moveName: "Will-O-Wisp",
                moveType: "fire",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 0.85f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.InflictPokemonSC,
                        effectTiming: MoveEffectTiming.AfterTargetImpact,
                        effectTargetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true,
                        floatParams: new float[] { -1 },
                        stringParams: new string[] { "burn" }
                        ),
                }
                ) },

        // Wing Attack
        {"wingattack",
            new MoveData(
                ID: "wingattack",
                moveName: "Wing Attack",
                moveType: "flying",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Any,

                basePower: 60, accuracy: 1.0f, PP: 35,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {

                }
                ) },

        // Wish
        {"wish",
            new MoveData(
                ID: "wish",
                moveName: "Wish",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,
                PP: 10,

                moveTags: new MoveTag[]
                {
                    MoveTag.IgnoreProtect,
                    MoveTag.Snatchable,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        effectType: MoveEffectType.Wish,
                        boolParams: new bool[] { false, false },
                        floatParams: new float[] { 0.5f, 1 },
                        stringParams: new string[] { null, "DEFAULT" }
                        ),
                }
                ) },

        // Withdraw
        {"withdraw",
            new MoveData(
                ID: "withdraw",
                moveName: "Withdraw",
                moveType: "water",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                PP: 40,

                moveTags: new MoveTag[]
                {
                    MoveTag.Snatchable,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            DEFMod: 1
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Wonder Room
        {"wonderroom",
            new MoveData(
                ID: "wonderroom",
                moveName: "Wonder Room",
                moveType: "psychic",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Battlefield,
                PP: 10,

                moveTags: new MoveTag[]
                {

                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.InflictBattleSC,
                        MoveEffectTiming.AfterSuccessfulMoveUse,
                        MoveEffectTargetType.Battlefield,
                        forceEffectDisplay: true,
                        floatParams: new float[] { 5 },
                        stringParams: new string[] { "wonderroom" }
                        )
                }
                ) },

        // Wrap
        {"wrap",
            new MoveData(
                ID: "wrap",
                moveName: "Wrap",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 15, accuracy: 0.9f, PP: 20,

                moveTags: new MoveTag[]
                {
                    MoveTag.MakesContact,
                },

                moveEffects: new MoveEffect[]
                {
                    new MoveEffect(
                        MoveEffectType.Bind,
                        MoveEffectTiming.AfterTargetImpact,
                        floatParams: new float[] { 1f/8, 4 },
                        stringParams: new string[] {
                            "move-bind-start-wrap", "DEFAULT", "move-bind-end-wrap", "DEFAULT" }
                        ),
                    new MoveEffect(
                        MoveEffectType.BindTurnRange,
                        floatParams: new float[] { 4, 5 }
                        ),
                }
                ) },


        // X

        

        // Y

        // Yawn
        {"yawn",
            new MoveData(
                ID: "yawn",
                moveName: "Yawn",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Adjacent,
                accuracy: 1.0f, PP: 15,

                moveTags: new MoveTag[]
                {
                    MoveTag.MagicCoatSusceptible,
                    MoveTag.PowderMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Pokemon,
                            statusID: "yawn"
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTarget,
                        targetType: MoveEffectTargetType.Target,
                        forceEffectDisplay: true
                        ),
                }
                ) },


        // Z



        // ---Z MOVES---
        
        // --Regular

        // Breakneck Blitz
        {"breakneckblitz",
            new MoveData(
                ID: "breakneckblitz",
                moveName: "Breakneck Blitz",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 1,

                moveTags: new MoveTag[]
                {
                    MoveTag.ZMove
                }
                ) },

        // Shattered Psyche
        {"shatteredpsyche",
            new MoveData(
                ID: "shatteredpsyche",
                moveName: "Shattered Psyche",
                moveType: "psychic",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,
                basePower: 1,

                moveTags: new MoveTag[]
                {
                    MoveTag.ZMove
                }
                ) },

        // --Signature

        // Guardian Of Alola
        {"guardianofalola",
            new MoveData(
                ID: "guardianofalola",
                moveName: "Guardian Of Alola",
                moveType: "fairy",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.ZMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.SuperFang(damagePercent: 0.75f),
                }
                ) },

        // Light That Burns the Sky
        {"lightthatburnsthesky",
            new MoveData(
                ID: "lightthatburnsthesky",
                moveName: "Light That Burns the Sky",
                moveType: "psychic",
                category: MoveCategory.Special,
                targetType: MoveTargetType.Adjacent,
                basePower: 200,

                moveTags: new MoveTag[]
                {
                    MoveTag.ZMove
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.PhotonGeyser(),
                    new Effects.Moves.SunsteelStrike(),
                }
                ) },


        // ---MAX MOVES---
        
        // --Regular

        // Max Guard
        {"maxguard",
            new MoveData(
                ID: "maxguard",
                moveName: "Max Guard",
                moveType: "normal",
                category: MoveCategory.Status,
                targetType: MoveTargetType.Self,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },

                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.Protect(
                        protect: new Effects.General.Protect(
                            maxGuard: true
                            )
                        ),
                }
                ) },

        // Max Airstream
        {"maxairstream",
            new MoveData(
                ID: "maxairstream",
                moveName: "Max Airstream",
                moveType: "flying",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                           SPEMod: 1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.SelfTeam,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Max Darkness
        {"maxdarkness",
            new MoveData(
                ID: "maxdarkness",
                moveName: "Max Darkness",
                moveType: "dark",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            SPDMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true
                        ),
                }
                ) },
                
        // Max Flare
        {"maxflare",
            new MoveData(
                ID: "maxflare",
                moveName: "Max Flare",
                moveType: "fire",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "harshsunlight"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield
                        ),
                }
                ) },

        // Max Flutterby
        {"maxflutterby",
            new MoveData(
                ID: "maxflutterby",
                moveName: "Max Flutterby",
                moveType: "bug",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            SPAMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true
                        ),
                }
                ) },
                
        // Max Geyser
        {"maxgeyser",
            new MoveData(
                ID: "maxgeyser",
                moveName: "Max Geyser",
                moveType: "water",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "rain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield
                        ),
                }
                ) },
                
        // Max Hailstorm
        {"maxhailstorm",
            new MoveData(
                ID: "maxhailstorm",
                moveName: "Max Hailstorm",
                moveType: "ice",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "hail"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield
                        ),
                }
                ) },

        // Max Knuckle
        {"maxknuckle",
            new MoveData(
                ID: "maxknuckle",
                moveName: "Max Knuckle",
                moveType: "fighting",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            ATKMod: 1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.SelfTeam,
                        forceEffectDisplay: true
                        ),
                }
                ) },
                
        // Max Lightning
        {"maxlightning",
            new MoveData(
                ID: "maxlightning",
                moveName: "Max Lightning",
                moveType: "electric",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "electricterrain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield
                        ),
                }
                ) },
                
        // Max Mindstorm
        {"maxmindstorm",
            new MoveData(
                ID: "maxmindstorm",
                moveName: "Max Mindstorm",
                moveType: "psychic",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "psychicterrain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield
                        ),
                }
                ) },
                
        // Max Ooze
        {"maxooze",
            new MoveData(
                ID: "maxooze",
                moveName: "Max Ooze",
                moveType: "poison",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            SPAMod: 1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.SelfTeam,
                        forceEffectDisplay: true
                        ),
                }
                ) },
                
        // Max Overgrowth
        {"maxovergrowth",
            new MoveData(
                ID: "maxovergrowth",
                moveName: "Max Overgrowth",
                moveType: "grass",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "grassyterrain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield
                        ),
                }
                ) },
                
        // Max Quake
        {"maxquake",
            new MoveData(
                ID: "maxquake",
                moveName: "Max Quake",
                moveType: "ground",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            SPDMod: 1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.SelfTeam,
                        forceEffectDisplay: true
                        ),
                }
                ) },
                
        // Max Phantasm
        {"maxphantasm",
            new MoveData(
                ID: "maxphantasm",
                moveName: "Max Phantasm",
                moveType: "ghost",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            DEFMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true
                        ),
                }
                ) },
                
        // Max Rockfall
        {"maxrockfall",
            new MoveData(
                ID: "maxrockfall",
                moveName: "Max Rockfall",
                moveType: "rock",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "sandstorm"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield
                        ),
                }
                ) },
                
        // Max Starfall
        {"maxstarfall",
            new MoveData(
                ID: "maxstarfall",
                moveName: "Max Starfall",
                moveType: "fairy",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Battle,
                            statusID: "mistyterrain"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.Battlefield
                        ),
                }
                ) },
                
        // Max Steelspike
        {"maxsteelspike",
            new MoveData(
                ID: "maxsteelspike",
                moveName: "Max Steelspike",
                moveType: "steel",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            DEFMod: 1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.Once,
                        targetType: MoveEffectTargetType.SelfTeam,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Max Strike
        {"maxstrike",
            new MoveData(
                ID: "maxstrike",
                moveName: "Max Strike",
                moveType: "normal",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            SPEMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // Max Wyrmwind
        {"maxwyrmwind",
            new MoveData(
                ID: "maxwyrmwind",
                moveName: "Max Wyrmwind",
                moveType: "dragon",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.StatStageMod(
                        statStageMod: new Effects.General.StatStageMod(
                            ATKMod: -1
                            ),
                        timing: MoveEffectTiming.AfterTargetImpact,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team,
                        forceEffectDisplay: true
                        ),
                }
                ) },

        // --Signature

        // G-Max Cannonade
        {"gmaxcannonade",
            new MoveData(
                ID: "gmaxcannonade",
                moveName: "G-Max Cannonade",
                moveType: "water",
                category: MoveCategory.Physical,
                targetType: MoveTargetType.Adjacent,

                moveTags: new MoveTag[]
                {
                    MoveTag.DynamaxMove,
                },
                effectsNew: new Effects.Moves.MoveEffect[]
                {
                    new Effects.Moves.InflictStatus(
                        new Effects.General.InflictStatus(
                            statusType: StatusType.Team,
                            statusID: "gmaxcannonade"
                            ),
                        timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                        occurrence: MoveEffectOccurrence.OnceForEachTeam,
                        targetType: MoveEffectTargetType.Team
                        ),
                }
                ) },


    };

        // Methods
        public MoveData GetMoveData(string ID)
        {
            if (database.ContainsKey(ID))
            {
                return database[ID];
            }
            Debug.LogWarning("Could not find move with ID: " + ID);
            return database[""];
        }

        public List<string> GetMetronomeMoves()
        {
            List<string> moves = new List<string>(database.Keys);
            List<string> validMoves = new List<string>();
            for (int i = 0; i < moves.Count; i++)
            {
                MoveData moveData = GetMoveData(moves[i]);
                if (!moveData.HasTag(MoveTag.UncallableCommon)
                    && !moveData.HasTag(MoveTag.UncallableByMetronome)
                    && !moveData.HasTag(MoveTag.ZMove))
                {
                    validMoves.Add(moves[i]);
                }
            }
            return validMoves;
        }

    }
}


// Game Settings
public enum GameLanguages
{
    English
}

// Overworld
public enum OverworldState
{
    Idle,
    Walk,
    Run,
    Unique
}
public enum OverworldDirection
{
    Down,
    Right,
    Up,
    Left
}


// Pokemon
public enum PokemonGender
{
    Male,
    Female,
    Genderless,
}
public enum PokemonStats
{
    None,

    HitPoints,
    Attack,
    Defense,
    SpecialAttack,
    SpecialDefense,
    Speed,

    Accuracy,
    Evasion
}
public enum PokemonTag
{
    // Special Pokemon
    Legendary,
    Mythical,
    PseudoLegendary,
    Starter,
    UltraBeast,

    // Legendary Sub-Classes
    GuardianDeity,
    LunarDuo,

    /// <summary>
    /// This is a Gigantamax pokemon.
    /// </summary>
    IsGigantamax,
    
    /// <summary>
    /// This is a Mega Pokemon.
    /// </summary>
    IsMega,

    /// <summary>
    /// If this pokemon entered battle under a different form, it will revert back to its base form once 
    /// battle is over.
    /// </summary>
    RevertOnBattleEnd,

    /// <summary>
    /// If this pokemon entered battle under a different form, it will revert back to its base form if it 
    /// faints.
    /// </summary>
    RevertOnFaint,

    /// <summary>
    /// If this pokemon entered battle under a different form, it will revert back to its base form if it switches out.
    /// </summary>
    RevertOnSwitchOut,
}
public enum PokemonEggGroup
{
    Dragon,
    Field,
    Monster,
    Water1,
    Water2,
    Water3,
    Undiscovered,
}

// Effect Filters
public enum FilterEffectType
{
    BurningJealousy,
    Harvest,
    ItemCheck,
    MoveCheck,
    PollenPuff,
    TypeList,
}

// Types
public enum TypeTag
{
    /// <summary>
    /// This type is not grounded, and so pokemon of this type are unaffected by grounded mechanics.
    /// </summary>
    Airborne,

    /// <summary>
    /// This type is grounded, and all of its moves are affected by grounded mechanics.
    /// </summary>
    Grounded,

    /// <summary>
    /// Prevents this type from being trapped by moves or abilities.
    /// </summary>
    CannotTrap,


}
public enum TypeEffectType
{
    None,

    /// <summary>
    /// Pokemon with this type instantly remove specific entry hazards when switching into them.
    /// </summary>
    RemoveEntryHazard,
    /// stringx:
    ///     range:  entry hazard IDs, or ALL (for all entry hazards)
    ///     desc:   list of entry hazards that are removed by this type upon switch-in
    
}
public enum TypeEffectiveness
{
    Neutral,
    SuperEffective,
    NotVeryEffective,
    Immune
}

// Items
public enum ItemPocket
{
    None,
    Medicine,
    Pokeballs,
    BattleItems,
    Berries,
    OtherItems,
    TMs,
    Treasures,
    Ingredients,
    KeyItems
}
public enum ItemBattlePocket
{
    None,
    HPRestore,
    StatusRestore,
    Pokeballs,
    BattleItems
}
public enum ItemTag
{
    /// <summary>
    /// This item can still be used regardless of whether or not the holder is under Embargo.
    /// </summary>
    BypassEmbargo,
    /// <summary>
    /// This item can still be used regarless of whether or not the holder has Klutz.
    /// </summary>
    BypassKlutz,

    CannotBestow,
    Consumable,

    OnlyUseableInBattle,
    StaysOnConsume,
}
public enum ItemEffectType
{
    ChoiceBand,
    ChoiceBandStats,
    GriseousOrb,
    Leftovers,
    LiechiBerry,
    LifeOrb,
    MegaStone,
    PokeBall,
    QuickClaw,
    RKSMemory,
    ShedShell,
    ShellBell,
    SmokeBall,
    ZCrystal,
    ZCrystalSignature,

    // BATTLE ITEMS

    /// <summary>
    /// Adds / Subtracts stat stages of the target pokemon.
    /// </summary>
    XAttack,
    /// float0:
    ///     range: any
    ///     desc: the stage to add to existing stat stage changes
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    /// <summary>
    /// Maximizes / Minimizes stat stages of the target pokemon.
    /// </summary>
    StatStageMax,
    /// bool0:
    ///     desc: set to true to maximize the stats of the pokemon, false to minimize
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify


    // BERRIES

    /// <summary>
    /// Scales the damage taken by moves of the listed types.
    /// </summary>
    YacheBerry,
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier to apply to moves of the listed types
    /// string0:
    ///     desc: ID for text to display when the item is activated
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1+:
    ///     range:  type IDs
    ///     desc:   types that have their damage modified

    /// <summary>
    /// Scales the damage taken by super-effective moves of the listed types. The item is consumed
    /// (if possible) afterward.
    /// </summary>
    TypeBerrySuperEffective,
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier to apply to super-effective moves of the listed types
    /// string0:
    ///     desc: ID for text to display when the item is activated
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1+:
    ///     range:  type IDs
    ///     desc:   types that have their damage modified


    // MEDICINE

    /// <summary>
    /// Restores up to an exact amount of hit points.
    /// </summary>
    Potion,
    /// float0:
    ///     range:  1-any
    ///     desc:   the exact hit points recovered
    /// string0:
    ///     desc:   ID for text to display when the pokemon heals HP
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Restores a % of the pokemon's HP.
    /// </summary>
    HealPercent,
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the % of HP recovered
    /// string0:
    ///     desc:   ID for text to display when the pokemon heals HP
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Removes the listed statuses for a pokemon.
    /// </summary>
    LumBerry,
    /// stringx:
    ///     range:  status IDs
    ///     desc:   statuses with the given IDs are cured

    /// <summary>
    /// Heals all non-volatile status conditions.
    /// </summary>
    HealStatusNonVolatile,

    /// <summary>
    /// Ensures that this item works, but only works on fainted Pokemon.
    /// </summary>
    Revive,
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the % of HP recovered upon revival
    /// string0:
    ///     desc:   ID for text to display when the pokemon revives
    ///             set to "DEFAULT" for default text
    ///             set to null for no text


    // MOVES

    /// <summary>
    /// Scales the power of moves with the listed types.
    /// </summary>
    Charcoal,
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier
    /// stringx:
    ///     range:  type IDs
    ///     desc:   the move type that this effect applies to

    /// <summary>
    /// Enables this item to be used for Fling, and defines the power of Fling.
    /// </summary>
    Fling,

    /// <summary>
    /// Enables the holder to survive fatal hits of moves occasionally.
    /// </summary>
    FocusBand,
    /// float0:
    ///     range:  -1, 0-1.0
    ///     desc:   the chance of the item activating. Set to -1 to guarantee activation.
    /// string0:
    ///     desc: ID for text to display when the item is activated
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// To be used alongside FocusBand. Restricts Focus Band activation to only if the holder is at maximum HP.
    /// </summary>
    FocusSash,

    /// <summary>
    /// If using a move with the effect Judgment, the move's type is changed to the given type.
    /// </summary>
    Judgment,
    /// string0:
    ///     range:  type IDs
    ///     desc:   type that the move is changed to

    /// <summary>
    /// This item can be used by Natural Gift and transforms its type and power.
    /// </summary>
    NaturalGift,
    /// float0:
    ///     range:  1-any
    ///     desc:   the power of natural gift
    /// string0:
    ///     range:  type IDs
    ///     desc:   the type that Natural Gift becomes


    // ---POKEMON---

    /// <summary>
    /// To be used alongside FormChange. Will only enable FormChange if the user has an ability tagged with Multitype.
    /// </summary>
    ArceusPlate,

    /// <summary>
    /// If held by a pokemon who has a specified baseID, or is the specified base pokemonID, their form is changed to
    /// the specified pokemonID.
    /// </summary>
    FormChange,
    /// string0:
    ///     range:  pokemonIDs
    ///     desc:   base pokemonID
    /// string1:
    ///     range:  pokemonIDs
    ///     desc:   form pokemonID


    // PROTECTION

    HeavyDutyBootsDamage,


    // TRIGGERS

    /// <summary>
    /// This item can have its effects triggered instantly when the user's HP drops below a certain threshold.
    /// </summary>
    TriggerOnHPLoss,
    /// bool0:
    ///     desc:   set to true to include activation of the trigger at exactly the threshold
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the % of the holder's maximum HP needed to be below to trigger

    /// <summary>
    /// This item can have its effects triggered instantly when inflicted with one of the given status conditions.
    /// </summary>
    LumBerryTrigger,
    /// bool0:
    ///     desc:   set to true to automatically trigger with a paired "HealStatus" and its listed status IDs
    ///             stringx is ignored if true
    /// stringx:
    ///     range:  status IDs
    ///     desc:   statuses with the given IDs trigger this item.



}


// Moves
public enum MoveCategory
{
    Physical,
    Special,
    Status,
    None
}
public enum MoveTargetType
{
    None,
    Self,
    Any,
    Adjacent,
    AdjacentOpponent,
    AdjacentAlly,
    SelfOrAdjacentAlly,
    AllAdjacent,
    AllAdjacentOpponents,
    AllAllies,
    AllAlliesButUser,
    AllOpponents,
    AllButUser,
    AllPokemon,
    TeamAlly,
    TeamOpponent,
    Battlefield
}
public enum MoveTag
{
    /// <summary>
    /// Checks accuracy every hit, rather than just once.
    /// </summary>
    AccuracyCheckEveryHit,

    CannotInstruct,
    CannotMimic,
    CannotSketch,
    CannotUseInGravity,
    CannotUseOnDynamax,
    CannotUseOnSubstitute,
    MakesContact,

    IgnoreChoiceLock,
    IgnoreCraftyShield,
    CannotDisable,
    CannotEncore,
    IgnoreMaxGuard,
    IgnoreProtect,
    IgnoreRedirection,
    IgnoreSubstitute,       // Note that SoundMove also ignores Substitute

    IgnoreKingsRock,
          
    Snatchable,
    MagicCoatSusceptible,
    
    BallMove,
    BiteMove,
    BombMove,
    DanceMove,
    ExplosiveMove,
    PowderMove,
    /// <summary>
    /// Gains boost from Mega Launcher.
    /// </summary>
    PulseMove,
    PunchMove,
    /// <summary>
    /// Gains boost from using Defense Curl.
    /// </summary>
    RollingMove,
    SoundMove,
    
    ZMove,
    DynamaxMove,

    UncallableCommon,
    UncallableByAssist,
    UncallableByCopycat,
    UncallableByMeFirst,
    UncallableByMetronome,
    UncallableByMirrorMove,
    UncallableBySleepTalk,


}
public enum MoveEffectType
{
    None,

    AuraWheel,
    BasePowerMultiplier,
    BattleConditionBoost,
    CorrosiveGas,
    DamageMultiplier,
    DoubleKick,
    DragonRage,
    Eruption,
    ExpandingForce,
    ExpandingForcePower,
    FinalGambit,
    FuryAttack,
    GrassyGlide,
    HiddenPower,
    InflictStatus,
    KarateChop,
    KnockOff,
    Magnitude,
    Poltergeist,
    Psywave,
    Punishment,
    Pursuit,
    Reversal,
    RisingVoltage,
    Rollout,
    SecretPower,
    SteelRoller,
    StoredPower,
    SuckerPunch,
    SuperFang,
    Synchronoise,
    ThunderWave,
    TripleKick,
    WorrySeed,




    // ABILITY-RELATED

    /// <summary>
    /// Suppresses the target's ability as long as it remains in battle (if possible).
    /// </summary>
    CoreEnforcer,
    /// bool0:
    ///     desc:   set to true to only cancel if the target has already moved this turn
    /// string0:
    ///     desc:   ID for text to display when the ability is successfully suppressed
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when the ability can't be suppressed
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Attempts to copy the target's ability.
    /// </summary>
    RolePlay,
    /// bool0:
    ///     desc:   set to true to make this move not affect targets if their abilities can't be copied
    /// bool1:
    ///     desc:   set to true to activate the ability upon copying it
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Attempts to swap abilities with the target.
    /// </summary>
    SkillSwap,
    /// bool0:
    ///     desc:   set to true to make this move not affect targets if their abilities can't be swapped
    ///             or the user's ability can't be swapped
    /// bool1:
    ///     desc:   set to true to activate the abilities upon swapping them
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Ignores abilities when in use.
    /// </summary>
    SunteelStrike,


    // ACCURACY

    AccuracyInWeather,
    /// bool0:
    ///     desc:   set to true for float0 to be a flat accuracy. set to false for a multiplier.
    /// float0:
    ///     range:  0-any
    ///     desc:   factor to change accuracy value
    /// stringx:
    ///     desc:   weather ID's that the accuracy change applies to 

    /// <summary>
    /// If successful, ensures that the next move the user makes against the target does not miss.
    /// </summary>
    LockOn,
    /// float0:
    ///     range:  -1,1-any
    ///     desc:   the number of turns this effect lasts for. Set to -1 for indefinitely
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when no longer locking on
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string2:
    ///     desc:   ID for text to display when the target is already locked on
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    GuillotineAccuracy,
    /// Calculated as ((user.level - target.level) + baseAccuracy)
    /// Independent of accuracy and evasion stats


    // CALL OTHER MOVES

    /// <summary>
    /// Causes the user to use a random move from its party or ally trainers' parties.
    /// </summary>
    Assist,

    /// <summary>
    /// Causes the user to use the last move that was used by a pokemon in battle.
    /// </summary>
    Copycat,

    /// <summary>
    /// Causes the target to use its last used move instantly.
    /// </summary>
    Instruct,

    /// <summary>
    /// If used before the target makes a move, the user will execute the target's move.
    /// </summary>
    MeFirst,

    /// <summary>
    /// Causes the user to use a completely random move.
    /// </summary>
    Metronome,

    /// <summary>
    /// Causes the user to use the last move that it was targeted by.
    /// </summary>
    MirrorMove,

    /// <summary>
    /// Changes to a different move depending on the environment or terrain.
    /// </summary>
    NaturePower,

    /// <summary>
    /// Causes the user to use a random move from its moveset while asleep.
    /// </summary>
    SleepTalk,


    // CRITICAL

    AlwaysCritical,
    /// ex. Frost Breath
    /// apply to guarantee critical hits (does not bypass Shell Armor, etc.)

    CriticalBoost,
    /// ex. Karate Chop, Stone Edge
    /// float0: 
    ///     range: 0-3 
    ///     desc: base critical hit ratio


    // DAMAGE MANIPULATION

    /// <summary>
    /// Calculates damage using the higher stat of either Attack or Special Attack of the user. If Attack is
    /// higher, this becomes a physical move. If Special Attack is higher, it becomes a special move.
    /// </summary>
    PhotonGeyser,

    /// <summary>
    /// Calculates damage using the specified stat as the defensive stat for the target.
    /// </summary>
    Psyshock,
    /// string0:
    ///     range:  atk,def,spa,spd,spe
    ///     desc:   the stat to use as the defensive stat for the target

    /// <summary>
    /// Calculates damage using the specified stat as the offensive stat for the user.
    /// </summary>
    PsyshockOffense,
    /// string0:
    ///     range:  atk,def,spa,spd,spe
    ///     desc:   the stat to use as the offensive stat for the user

    /// <summary>
    /// Calculates damage using the lower stat of either Defense or Special Defense of the target.
    /// </summary>
    ShellSideArm,


    // DAMAGE MULTIPLIERS

    /// <summary>
    /// Allows for this move to hit targets in the MultiTurnDig state (and bypasses accuracy checks).
    /// </summary>
    DmgDigState,
    /// bool0:
    ///     desc:   set to true to cancel the target's MultiTurnDig move if possible
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier to apply to pokemon using Dig

    /// <summary>
    /// Allows for this move to hit targets in the MultiTurnDive state (and bypasses accuracy checks).
    /// </summary>
    DmgDiveState,
    /// bool0:
    ///     desc:   set to true to cancel the target's MultiTurnDive move if possible
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier to apply to pokemon using Dive

    /// <summary>
    /// Allows for this move to hit targets in the MultiTurnFly state (and bypasses accuracy checks).
    /// </summary>
    DmgFlyState,
    /// bool0:
    ///     desc:   set to true to cancel the target's MultiTurnFly move if possible
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier to apply to pokemon using Fly, Bounce, Sky Drop, etc.

    /// <summary>
    /// Allows for this move to hit targets in the MultiTurnShadowForce state (and bypasses accuracy checks).
    /// </summary>
    DmgShadowForceState,
    /// bool0:
    ///     desc:   set to true to cancel the target's MultiTurnShadowForce move if possible
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier to apply to pokemon using Shadow Force, Phantom Force, etc.

    /// <summary>
    /// Allows for this move to hit targets in the Minimize state (and bypasses accuracy checks)/
    /// </summary>
    DmgMinimizeState,
    /// float0:
    ///     range: 0-any
    ///     desc: damage multiplier to apply to pokemon who previously used Minimize

    /// <summary>
    /// Scales damage depending on if the target is dynamaxed.
    /// </summary>
    DynamaxCannon,
    /// float0:
    ///     range: 0-any
    ///     desc: damage multiplier

    /// <summary>
    /// Allows for damage boosts as long as this pokemon hasn't moved yet.
    /// </summary>
    HelpingHand,
    /// float0:
    ///     range: 0-any
    ///     desc: damage multiplier to apply when hitting a target
    /// string0:
    ///     desc: ID for text to display when the pokemon is getting a helping hand boost
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Scales damage depending on if this move is used during a weather that activates Terrain Pulse.
    /// </summary>
    TerrainPulse,
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier

    /// <summary>
    /// Scales damage depending on if this move is used during a weather that activates Weather Ball.
    /// </summary>
    WeatherBall,
    /// float0:
    ///     range:  0-any
    ///     desc:   damage multiplier


    // DAMAGE OVERRIDE

    /// <summary>
    /// Deals multiplied damage taken to the attacker over a set amount of turns. Fails if no damage is taken.
    /// </summary>
    Bide,
    /// float0:
    ///     range: 0-any
    ///     desc: factor by which to multiply damage taken
    /// float1:
    ///     range: 1-any
    ///     desc: the number of turns to wait

    /// <summary>
    /// Deals multiplied damage taken to the attacker. Fails if no damage is taken.
    /// </summary>
    Counter,
    /// bool0:
    ///     desc:   if true, this move counters regardless of the type of damage taken. Overrides bool1.
    /// bool1:
    ///     desc:   if true, this move counters physical attacks. Set to false to counter special attacks
    /// float0:
    ///     range:  0-any
    ///     desc:   factor by which to multiply damage taken

    /// <summary>
    /// Causes the target's HP to match the user's own HP. Deals no damage if the user has higher HP than the target.
    /// </summary>
    Endeavor,

    /// <summary>
    /// Move power is based on the target's weight compared to the user's. Calculated like the games.
    /// </summary>
    HeavySlam,

    /// <summary>
    /// Move power is based on target's weight. Calculated like the games.
    /// </summary>
    LowKick,

    Guillotine,
    /// bool0:
    ///     desc:   if true, can affect targets of a higher level than the user 

    /// <summary>
    /// Deals damage equal to the user's level.
    /// </summary>
    SeismicToss,

    SetDamage,
    /// float0:
    ///     range: 0-any
    ///     desc: the set amount of damage dealt by this move


    // FORMS

    /// <summary>
    /// Alternates the user's forms after successful use, if it is one of the listed forms.
    /// </summary>
    RelicSong,
    /// string0:
    ///     range:  pokemonIDs
    ///     desc:   one pokemon ID
    /// string1:
    ///     range:  pokemonIDs
    ///     desc:   second pokemon ID
    /// string2:
    ///     desc:   ID for text to display when the pokemon switches form
    ///             set to "DEFAULT" for default text
    ///             set to null for no text


    // HEALTH-RELATED

    /// <summary>
    /// Heals the user's party of all non-volatile status conditions.
    /// </summary>
    Aromatherapy,
    /// bool0:
    ///     desc:   set to true to also heal ally trainers' parties

    /// <summary>
    /// The user instantly faints after using this move.
    /// </summary>
    FaintUser,
    /// bool0:
    ///     desc:   set to true to prevent HP loss when there are no targets (i.e. Mind Blown)
    /// bool1:
    ///     desc:   set to true to prevent HP loss when no targets are affected (i.e. Memento)

    /// <summary>
    /// Recovers a % of damage dealt as HP.
    /// </summary>
    HPDrain,
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the % of the damage dealt that is recovered.
    /// string0:
    ///     desc:   ID for text to display when the pokemon gains HP
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Induces HP loss on the user after the move is used (only on the 1st hit), proportional to user's maximum HP.
    /// </summary>
    HPLoss,
    /// bool0:
    ///     desc:   set to true to prevent HP loss when there are no targets (i.e. Mind Blown)
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the % of the HP lost.
    /// string0:
    ///     desc:   ID for text to display when the pokemon loses HP
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Gradually recovers the target's HP by a % each turn.
    /// </summary>
    Ingrain,
    /// bool0:
    ///     desc:   set to true to enable the ingrain effect (grounds pokemon, can't switch out)
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the % of the the target's HP that is recovered each turn
    /// string0:
    ///     desc:   ID for text to display upon success
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when the pokemon gains HP
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Plants leech seed onto the hit target to drain a % of its HP.
    /// </summary>
    LeechSeed,
    /// float0:
    ///     range: 0-1.0
    ///     desc: the % of HP drained
    /// string0:
    ///     desc: ID for text to display when the pokemon is seeded
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the pokemon's HP is drained
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when the pokemon can't be seeded
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Prevents leech seed effect based on type.
    /// </summary>
    LeechSeedTypeImmunity,
    /// Must be used alongside "LeechSeed".
    /// stringx:
    ///     range:  type IDs
    ///     desc:   types that cannot be leech seeded

    /// <summary>
    /// Induces HP loss when this move explicitly fails.
    /// </summary>
    JumpKick,
    /// The user will lose HP if their move is blocked or misses all of their targets.
    /// float0:
    ///     range: 0-1.0
    ///     desc: the % of the user's max HP that will be lost
    /// string0:
    ///     desc: ID for text to display when the pokemon loses HP
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Induces HP loss proportional to damage dealt.
    /// </summary>
    Recoil,
    /// The user will lose HP if their move damaged their targets successfully.
    /// bool0:
    ///     desc: set to true to use damage dealt as basis for recoil. set to false to use the user's maximum HP.
    /// float0:
    ///     range: 0-1.0
    ///     desc: the % of (total damage dealt / user's maximum HP) that the user takes as recoil
    /// string0:
    ///     desc: ID for text to display when the pokemon loses HP
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Restores a % of the target's HP.
    /// </summary>
    Recover,
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the % of HP recovered
    /// string0:
    ///     desc:   ID for text to display when the pokemon gains HP
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when the pokemon fails to recover HP
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Alleviates the target of non-volatile status conditions.
    /// </summary>
    Refresh,
    /// bool0:
    ///     desc:   set to true to invert the status filter - i.e. only the listed statuses can't be healed
    /// bool1:
    ///     desc:   set to true to limit to non-volatile statuses only
    /// stringx:
    ///     range:  statusID's, "ALL"
    ///     desc:   status IDs to heal. Set to "ALL" to heal all non-volatile conditions

    /// <summary>
    /// Fully heals and cures the user, but puts them to sleep afterward for 2 turns. 
    /// Fails if used while the user has full HP.
    /// </summary>
    Rest,
    /// string0:
    ///     desc: ID for text to display when the pokemon rests
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// The user makes a wish, and whoever is in its position x turns later will recover some HP.
    /// </summary>
    Wish,
    /// bool0:
    ///     desc:   set to true to enable healing non-volatile status effects as well
    /// bool1:
    ///     desc:   set to true to fully heal the target's HP
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the % of the user's HP recovered
    /// float1:
    ///     range:  0-any
    ///     desc:   the number of turns to wait
    /// string0:
    ///     desc:   ID for text to display when the pokemon makes a wish
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when the pokemon's wish comes true
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// To be used alongside Wish. Restores PP of the target Pokemon.
    /// </summary>
    WishLunarDance,

    /// <summary>
    /// To be used alongside Wish. Overrides wish targets to be the user's position.
    /// </summary>
    WishUserPosition,


    // ITEM-RELATED

    /// <summary>
    /// Only works if the user has consumed a berry sometime in battle. Then this move may be
    /// used for the rest of the battle.
    /// </summary>
    Belch,

    /// <summary>
    /// Transfers the user's held item to the target. Fails if the user has no held item,
    /// the target already has a held item, or the user's item cannot be bestowed.
    /// </summary>
    Bestow,
    /// string0:
    ///     desc: ID for text to display when the item is transferred
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Eats and gains the effects of the targets' berries when attacking.
    /// </summary>
    BugBite,
    /// string0:
    ///     desc: ID for text to display when the item is stolen and consumed
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Steals the target's held item (if possible) if the user doens't have one.
    /// </summary>
    Covet,
    /// string0:
    ///     desc: ID for text to display when the item is stolen
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Prevents the target from using items, and negates their held item's effect.
    /// </summary>
    Embargo,
    /// float0:
    ///     range: 1-any
    ///     desc: the amount of turns that this affliction lasts for
    /// string0:
    ///     desc: ID for text to display when embargo begins
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when embargo ends
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when a pokemon attempts to use an item while under embargo
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string3:
    ///     desc: ID for text to display when the target cannot be afflicted with embargo
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Changes power and forces the target to consume the item.
    /// Fails if the user has no item, or the item cannot be flung.
    /// </summary>
    Fling,
    /// string0:
    ///     desc: ID for text to display when the item is flung
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Destroys the target's berry item.
    /// </summary>
    Incinerate,
    /// string0:
    ///     desc: ID for text to display when the item is destroyed
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Changes the move's type and power based on the berry held. Fails if the user is not
    /// holding a berry, or can't use its berry.
    /// </summary>
    NaturalGift,

    /// <summary>
    /// Forces the target to consume its held berry, and the target receives a stat stage change at the same time.
    /// </summary>
    StuffCheeks,
    /// float0:
    ///     range: any
    ///     desc: the stage to add to existing stat stage changes
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    /// <summary>
    /// Works similarly to StuffCheeks, but maximizes or minimizes the stats.
    /// </summary>
    StuffCheeksMax,
    /// bool0:
    ///     desc: set to true to maximize the stats of the pokemon, false to minimize
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    /// <summary>
    /// Forces all pokemon to consume their held berries. Fails if no pokemon has a berry to consume.
    /// </summary>
    Teatime,
    /// string0:
    ///     desc: ID for text to display when teatime fails
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Swaps held items with the target (if possible).
    /// </summary>
    Trick,
    /// string0:
    ///     desc: ID for text to display when trick initiates
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when a pokemon obtains a swapped item
    ///           set to "DEFAULT" for default text
    ///           set to null for no text


    // MULTI-HIT

    /// <summary>
    /// Hits for as many able party members there are. The base power per hit is dependent upon the 
    /// matching party member's Attack.
    /// </summary>
    BeatUp,

    Multihit,
    /// ex. Double Kick, Dragon Darts
    /// float0:
    ///     range: 0-any
    ///     desc: the move hits float0 amount of times

    Multihit2To5,
    /// ex. Bullet Seed, Rock Blast
    /// simulates 2-5 hits and their chances from the games


    // MULTI-TURN

    /// <summary>
    /// Sets up a move that will execute turns later at the end of that turn.
    /// </summary>
    FutureSight,
    /// bool0:
    ///     desc:   set to true to bypass redirection when the move is hit
    /// float0:
    ///     range:  0-any
    ///     desc:   the amount of turns to wait before attacking (after move is set up)
    /// string0:
    ///     desc:   ID for text to display when the move is set up
    ///             set to "DEFAULT" to use the default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when the move executes
    ///             set to "DEFAULT" to use the default text
    ///             set to null for no text

    /// <summary>
    /// Skips charge turns if one of the listed weather conditions is present. Takes effect alongside other 
    /// MultiTurn effects.
    /// </summary>
    InstantMultiTurnWeather,
    /// stringx:
    ///     range: weather ID's
    ///     desc:  weather conditions that skip the charge turns

    MultiTurnAttack,
    /// float0:
    ///     range: 2-any
    ///     desc: the amount of turns needed to attack
    /// stringx:
    ///     range: game text ID's
    ///     desc: ID for text to display for turn x
    ///           set to "DEFAULT" to use the default text
    ///           set to null for no text

    MultiTurnDig,
    /// During charge turns, the user is put into a semi-invulnerable state such as Dig.
    /// During this state, the user can't be hit by most moves.

    MultiTurnDive,
    /// During charge turns, the user is put into a semi-invulnerable state such as Dive.
    /// During this state, the user can't be hit by most moves.

    MultiTurnFly,
    /// During charge turns, the user is put into a semi-invulnerable state such as Fly, Bounce or Sky Drop.
    /// During this state, the user can't be hit by most moves.

    MultiTurnShadowForce,
    /// During charge turns, the user is put into a semi-invulnerable state such as Shadow Force or Phantom Force.
    /// During this state, the user can't be hit by most moves.

    RechargeTurn,
    /// float0:
    ///     range: 1-any
    ///     desc: the amount of turns needed to recharge after a successful attack

    /// <summary>
    /// Grabs the target on the first turn, preventing them from executing moves or switching. Damage is dealt on the
    /// second turn, and the target becomes able to act again.
    /// </summary>
    SkyDrop,
    /// string0:
    ///     desc: ID for text to display for the grab turn
    ///           set to "DEFAULT" to use the default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when a target is freed from the move
    ///           set to "DEFAULT" to use the default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when the pokemon attempts to switch out while being sky dropped
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    
    // PROTECTION

    /// <summary>
    /// To be used alongside Protect or Mat Block. Induces a status condition on attackers upon protection.
    /// </summary>
    BanefulBunker,
    /// bool0:
    ///     desc:   set to true to only induce effect on attackers making contact
    /// float0:
    ///     desc:   -1, 0-any
    ///     desc:   the number of turns to inflict status. If set to -1, the status lasts indefinitely.
    /// string0:
    ///     range:  statusID
    ///     desc:   the ailment with ID string0 will attempt to be inflicted

    /// <summary>
    /// To be used alongside Protect or Mat Block. Limits protection to status moves only.
    /// </summary>
    CraftyShield,
    /// bool0:
    ///     desc: set to true to block all status moves except those tagged with "IgnoreCraftyShield".
    ///           moves only tagged with "IgnoreProtect" are protected against in this case

    /// <summary>
    /// The user survives direct attacks with 1 HP for the rest of the turn.
    /// </summary>
    Endure,
    /// string0:
    ///     desc: ID for text to display upon endure initiation
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display upon successfully enduring a hit
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Lifts the effects of protection moves for the rest of the turn if it hits.
    /// </summary>
    Feint,
    /// string0:
    ///     desc: ID for text to display when protection moves are lifted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// To be used alongside Protect or Mat Block. Induces stat stage changes on attackers upon protection.
    /// </summary>
    KingsShield,
    /// bool0:
    ///     desc: set to true to only induce effect on attackers making contact
    /// float0:
    ///     range: any
    ///     desc: the stage to add to existing stat stage changes
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    /// <summary>
    /// Protects the user's team from most moves.
    /// </summary>
    MatBlock,
    /// bool0:
    ///     desc: set to true to only protect against damaging moves
    /// string0:
    ///     desc: ID for text to display upon protect initiation
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display upon successful protection
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Protects the user from most moves. Less likely to succeed with consecutive uses.
    /// </summary>
    Protect,
    /// bool0:
    ///     desc: set to true to only protect against damaging moves
    /// string0:
    ///     desc: ID for text to display upon protect initiation
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display upon successful protection
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// To be used alongside Protect or Mat Block. Adds protection for the user against Team-Targeting attacks.
    /// (Ex. Entry Hazards)
    /// </summary>
    ProtectField,

    /// <summary>
    /// To be used alongside Protect or Mat Block. Limits protection to heightened priority moves only.
    /// </summary>
    QuickGuard,

    /// <summary>
    /// To be used alongside Protect or Mat Block. Deals damage to attackers upon protection.
    /// </summary>
    SpikyShield,
    /// bool0:
    ///     desc: set to true to only induce effect on attackers making contact
    /// float0:
    ///     range: 0-1.0
    ///     desc: the % of the attacker's max HP that will be lost
    /// string0:
    ///     desc: ID for text to display upon dealing damage
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// To be used alongside Protect or Mat Block. Limits protection to multi-targeting moves only.
    /// </summary>
    WideGuard,


    // STATUS

    /// <summary>
    /// Charges at the beginning of the turn. Any Pokemon who attack the user before it executes the move
    /// is inflicted with the given status condition.
    /// </summary>
    BeakBlast,
    /// bool0:
    ///     desc: set to true to only induce effect on attackers making contact
    /// float0:
    ///     desc: -1, 0-any
    ///     desc: the number of turns to inflict status. If set to -1, the status lasts indefinitely.
    /// string0:
    ///     range: statusID
    ///     desc: the ailment with ID string0 will attempt to be inflicted
    /// string1:
    ///     desc: ID for text to display when charging up
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Traps the target for a set amount of turns. The effect can be lifted early if the user leaves battle. 
    /// The target also takes gradual damage every turn.
    /// </summary>
    Bind,
    /// float0:
    ///     range: 0-1
    ///     desc: the HP % lost each turn for pokemon afflicted with bind
    /// float1:
    ///     range: 1-any
    ///     desc: the amount of turns that this affliction lasts for
    /// string0:
    ///     desc: ID for text to display when bind is inflicted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when damage is inflicted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when bind is lifted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string3:
    ///     desc: ID for text to display when the pokemon attempts to switch out when bound
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Modifies <seealso cref="Bind"/> to have a range of turns to last for, by overriding float1.
    /// </summary>
    BindTurnRange,
    /// float0:
    ///     range: 0-any
    ///     desc: the lowest amount of turns that bind will last for
    /// float1:
    ///     range: higher than float0
    ///     desc: the highest amount of turns that bind will last for

    /// <summary>
    /// Traps the target in battle as long as the user is still in battle.
    /// </summary>
    Block,
    /// string0:
    ///     desc: ID for text to display when the target is trapped
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the target fails to be trapped
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when the target attempts to switch out while trapped
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Prevents the target from using its last move for a certain amount of turns.
    /// </summary>
    Disable,
    /// float0:
    ///     range: -1, 1-any
    ///     desc: the amount of turns that this affliction lasts for. If set to -1, the status lasts indefinitely.
    /// string0:
    ///     desc: ID for text to display when the target is successfully disabled
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the target is free from disable
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when the target attempts to use the disabled move
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string3:
    ///     desc: ID for text to display when the target cannot be disabled
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string4:
    ///     desc: ID for text to display when the target is already disabled
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Prevents the target from using any other move than its last move for a certain amount of turns.
    /// </summary>
    Encore,
    /// float0:
    ///     range: -1, 1-any
    ///     desc: the amount of turns that this affliction lasts for. If set to -1, the status lasts indefinitely.
    /// string0:
    ///     desc: ID for text to display when the target is successfully encored
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the target is free from encore
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when the target attempts to use a move while encored
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string3:
    ///     desc: ID for text to display when the target cannot be encored
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string4:
    ///     desc: ID for text to display when the target is already encored
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Prevents the target from using a move during the current turn.
    /// </summary>
    Flinch,

    /// <summary>
    /// Prevents the target from healing for a certain amount of turns.
    /// </summary>
    HealBlock,
    /// float0:
    ///     range: -1, 1-any
    ///     desc: the amount of turns that this affliction lasts for. If set to -1, the status lasts indefinitely.
    /// string0:
    ///     desc: ID for text to display when the target is successfully heal blocked
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the target is free from heal block
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when the target attempts to use a move while heal blocked
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string3:
    ///     desc: ID for text to display when the target cannot be heal blocked
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Prevents the target from using status moves for a certain amount of turns.
    /// </summary>
    Taunt,
    /// float0:
    ///     range: -1, 1-any
    ///     desc: the amount of turns that this affliction lasts for. If set to -1, the status lasts indefinitely.
    /// string0:
    ///     desc: ID for text to display when the target is successfully taunted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the target is free from taunt
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when the target attempts to use a move while taunted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string3:
    ///     desc: ID for text to display when the target cannot be taunted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Prevents the target from using the same move back-to-back for a certain amount of turns.
    /// </summary>
    Torment,
    /// float0:
    ///     range: -1, 1-any
    ///     desc: the amount of turns that this affliction lasts for. If set to -1, the status lasts indefinitely.
    /// string0:
    ///     desc: ID for text to display when the target is successfully tormented
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the target is free from torment
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when the target attempts to use a move while tormented
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string3:
    ///     desc: ID for text to display when the target cannot be tormented
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Induces a pokemon-based condition on the target team (bound, poison, sleep, etc.)
    /// </summary>
    InflictPokemonSC,
    /// bool0:
    ///     desc:   set to true to force this condition to use its default turns, if it has any. Overwrites bool1.
    /// bool1:
    ///     desc:   set to true to set a turn range for this status, between float0 and float1
    /// float0:
    ///     range:  -1, 0-any
    ///     desc:   the number of turns to inflict status. If set to -1, the status lasts indefinitely.
    ///             If bool1 is true, this is the lowest amount of turns for this status
    /// float1:
    ///     range:  0-any, at least as high as float0
    ///     desc:   the maximum number of turns to inflict status if bool1 is true
    /// string0:
    ///     range:  statusID's
    ///     desc:   the status with ID string0 will attempt to be inflicted.

    InflictPokemonSCOverwrite,

    InflictPokemonSCTR,
    /// Must be used alongside "InflictStatusCondition".
    /// Randomly selects the amount of turns left for a status between float0 and float1 inclusive.
    /// Overrides float1 in "InflictStatusCondition".
    /// float0:
    ///     range: 0-any
    ///     desc: the lowest amount of turns that status will be inflicted for
    /// float1:
    ///     range: higher than float0
    ///     desc: the highest amount of turns that status will be inflicted for
    /// stringx:
    ///     desc: the status IDs that this applies to
    ///           leave empty to apply to all status

    /// <summary>
    /// Induces a team-based status condition on the target.
    /// </summary>
    InflictTeamSC,
    /// bool0:
    ///     desc:   set to true to force this condition to use its default turns, if it has any. Overwrites bool1.
    /// bool1:
    ///     desc:   set to true to set a turn range for this status, between float0 and float1
    /// float0:
    ///     range:  -1, 0-any
    ///     desc:   the number of turns to inflict status. If set to -1, the status lasts indefinitely.
    ///             If bool1 is true, this is the lowest amount of turns for this status
    /// float1:
    ///     range:  0-any, at least as high as float0
    ///     desc:   the maximum number of turns to inflict status if bool1 is true
    /// string0:
    ///     range: statusID's
    ///     desc: the condition with ID string0 will attempt to be inflicted.

    InflictTeamSCTR,
    /// Must be used alongside "InflictTeamStatusCondition".
    /// Randomly selects the amount of turns left for a status between float0 and float1 inclusive.
    /// Overrides float1 in "InflictTeamStatusCondition".
    /// float0:
    ///     range: 0-any
    ///     desc: the lowest amount of turns that status will be inflicted for
    /// float1:
    ///     range: higher than float0
    ///     desc: the highest amount of turns that status will be inflicted for
    /// stringx:
    ///     desc: the status IDs that this applies to
    ///           leave empty to apply to all status

    /// <summary>
    /// Induces a battle-based condition (weather, terrain, rooms, etc.)
    /// </summary>
    InflictBattleSC,
    /// bool0:
    ///     desc:   set to true to force this condition to use its default turns, if it has any. Overwrites bool1.
    /// bool1:
    ///     desc:   set to true to set a turn range for this status, between float0 and float1
    /// float0:
    ///     range:  -1, 0-any
    ///     desc:   the number of turns to inflict status. If set to -1, the status lasts indefinitely.
    ///             If bool1 is true, this is the lowest amount of turns for this status
    /// float1:
    ///     range:  0-any, at least as high as float0
    ///     desc:   the maximum number of turns to inflict status if bool1 is true
    /// string0:
    ///     range: statusID's
    ///     desc: the condition with ID string0 will attempt to be inflicted.

    InflictBattleSCTR,
    /// Must be used alongside "InflictBattleStatusCondition".
    /// Randomly selects the amount of turns left for a status between float0 and float1 inclusive.
    /// Overrides float1 in "InflictBattleStatusCondition".
    /// float0:
    ///     range: 0-any
    ///     desc: the lowest amount of turns that status will be inflicted for
    /// float1:
    ///     range: higher than float0
    ///     desc: the highest amount of turns that status will be inflicted for
    /// stringx:
    ///     desc: the status IDs that this applies to
    ///           leave empty to apply to all status

    /// <summary>
    /// Thaws the target if they are frozen.
    /// </summary>
    ThawTarget,

    /// <summary>
    /// Thaws the user before attacking.
    /// </summary>
    HealBeforeUse,


    // STAT STAGE

    /// <summary>
    /// Enables this move to ignore stat stage changes of the target when attacking.
    /// </summary>
    ChipAway,
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to ignore the changes of

    /// <summary>
    /// Eliminates stat changes. Can be applied to targets, a team, or the battlefield.
    /// </summary>
    Haze,
    /// stringx:
    ///     range:  atk,def,spa,spd,spe,acc,eva,all
    ///     desc:   the stats affected

    /// <summary>
    /// To be used alongside Haze. Determines the text displayed when this move eliminates stat changes.
    /// </summary>
    HazeText,
    /// string0:
    ///     desc:   ID for text to display when upon success
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Initiates a counter where as long as this move is used, the counter and the listed stats are increased
    /// when hit directly.
    /// </summary>
    Rage,
    /// float0:
    ///     range: any
    ///     desc: the stage to add to the listed stats
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    RageText,
    /// string0:
    ///     desc: ID for text to display when rage is building
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Steals stat stage changes from the target.
    /// </summary>
    SpectralThief,
    /// bool0:
    ///     desc:   set to true to only steal stat stage boosts
    /// stringx:
    ///     range:  atk,def,spa,spd,spe,acc,eva,all
    ///     desc:   the stats to steal changes from

    /// <summary>
    /// Adds / Subtracts stat stages of the user pokemon.
    /// </summary>
    StatStageSelfMod,
    /// float0:
    ///     range: any
    ///     desc: the stage to add to existing stat stage changes
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    /// <summary>
    /// Maximizes / Minimizes stat stages of the user pokemon.
    /// </summary>
    StatStageSelfMax,
    /// bool0:
    ///     desc: set to true to maximize the stats of the pokemon, false to minimize
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    /// <summary>
    /// Adds / Subtracts stat stages of the target pokemon.
    /// </summary>
    StatStageMod,
    /// float0:
    ///     range: any
    ///     desc: the stage to add to existing stat stage changes
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    /// <summary>
    /// Maximizes / Minimizes stat stages of the target pokemon.
    /// </summary>
    StatStageMax,
    /// bool0:
    ///     desc: set to true to maximize the stats of the pokemon, false to minimize
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to modify

    /// <summary>
    /// Scales accompanying stat stage boosts during specified weather. Takes effect alongside <seealso cref="StatStageMod"/>.
    /// </summary>
    StatStageGrowth,
    /// float0:
    ///     range: any int
    ///     desc:  the factor by which to multiply to the stat stage boosts
    /// stringx:
    ///     range: weather ID's
    ///     desc:  the weather that this effect applies to


    // STATS

    /// <summary>
    /// Averages the given stats with the target pokemon.
    /// </summary>
    PowerSplit,
    /// stringx:
    ///     range:  atk,def,spa,spd,spe
    ///     desc:   the stats to average

    /// <summary>
    /// To be used alongside PowerSplit. Determines the text that displays upon successful use.
    /// </summary>
    PowerSplitText,
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Swaps the given stat stages with the target pokemon.
    /// </summary>
    PowerSwap,
    /// stringx:
    ///     range:  atk,def,spa,spd,spe,acc,eva,all
    ///     desc:   the stat stages to swap

    /// <summary>
    /// To be used alongside PowerSwap. Determines the text that displays upon successful use.
    /// </summary>
    PowerSwapText,
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Swaps the given raw stats of the target pokemon.
    /// </summary>
    PowerTrick,
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     range:  atk,def,spa,spd,spe
    ///     desc:   first stat to swap
    /// string2:
    ///     range:  atk,def,spa,spd,spe
    ///     desc:   second stat to swap


    // TEAM-RELATED

    /// <summary>
    /// Lays entry hazards near the target's team.
    /// </summary>
    EntryHazard,
    /// bool0: 
    ///     desc:   set to true to make the entry hazard only affect grounded pokemon
    /// float0:
    ///     range:  -1,0-any
    ///     desc:   the number of turns this hazard lasts for. Set to -1 to last indefinitely
    /// float1:
    ///     range:  1-any
    ///     desc:   the maximum amount of layers for this entry hazard
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display upon failure
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string2:
    ///     desc:   ID for text to display upon removal
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// To be used alongside EntryHazard. Induces damage whenever a pokemon switches into the entry hazard.
    /// </summary>
    EntryHazardDamage,
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the base HP % lost by the pokemon upon switch-in
    /// float1:
    ///     range:  0-1.0
    ///     desc:   an additional HP % lost per extra layer of the hazard
    /// string0:
    ///     desc:   ID for text to display when the pokemon takes damage
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// To be used alongside EntryHazardDamage. Scales damage taken based on the type relationships between
    /// the pokemon's types, and the hazard's type(s).
    /// </summary>
    EntryHazardStealthRock,
    /// stringx:
    ///     range:  type IDs
    ///     desc:   the associated type(s) of this hazard

    /// <summary>
    /// To be used alongside EntryHazard. Induces a stat stage change whenever a pokemon switches into the entry 
    /// hazard.
    /// </summary>
    EntryHazardStickyWeb,
    /// bool0: 
    ///     desc:   set to true to enable the effect only at the exact layer (otherwise also past this layer)
    /// float0:
    ///     range:  any
    ///     desc:   the stage to add to existing stat stage changes
    /// float1:
    ///     range:  -1,1-any
    ///     desc:   the layer to apply this effect on. Set to -1 to apply on all layers
    /// stringx:
    ///     range:  atk,def,spa,spd,spe,acc,eva,ALL
    ///     desc:   the stats to modify

    /// <summary>
    /// To be used alongside EntryHazard. Induces a status condition whenever a pokemon switches into the entry hazard.
    /// </summary>
    EntryHazardToxicSpikes,
    /// bool0: 
    ///     desc:   set to true to enable the effect only at the exact layer (otherwise also past this layer)
    /// float0:
    ///     range:  -1, 0-any
    ///     desc:   the number of turns to inflict status. If set to -1, the status lasts indefinitely.
    /// float1:
    ///     range:  -1,1-any
    ///     desc:   the layer to apply this effect on. Set to -1 to apply on all layers
    /// string0:
    ///     range:  statusID's
    ///     desc:   the ailment with ID string0 will attempt to be inflicted.

    /// <summary>
    /// Scales damage taken by team members for a set amount of turns.
    /// </summary>
    Reflect,
    /// bool0: 
    ///     desc:   set to true to allow bypassing via Infiltrator mechanics
    /// float0:
    ///     range:  -1,0-any
    ///     desc:   the number of turns Reflect lasts for. Set to -1 to last indefinitely
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display upon failure
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string2:
    ///     desc:   ID for text to display upon removal
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// To be used alongside Reflect. Scales damage taken based on move category.
    /// </summary>
    ReflectMoveCategory,
    /// bool0: 
    ///     desc:   set to true to allow stacking with other stackable ReflectMoveCategory effects
    /// float0:
    ///     range:  0-any
    ///     desc:   damage scaling applied to physical moves
    /// float1:
    ///     range:  0-any
    ///     desc:   damage scaling applied to special moves
    /// float2:
    ///     range:  0-any
    ///     desc:   damage scaling applied to physical moves in multi-battles
    /// float3:
    ///     range:  0-any
    ///     desc:   damage scaling applied to special moves in multi-battles

    /// <summary>
    /// Protects the target's team from certain effects.
    /// </summary>
    Safeguard,
    /// bool0: 
    ///     desc:   set to true to allow bypassing via Infiltrator mechanics
    /// float0:
    ///     range:  -1,0-any
    ///     desc:   the number of turns Safeguard lasts for. Set to -1 to last indefinitely
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display upon failure
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string2:
    ///     desc:   ID for text to display upon removal
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// To be used alongside Safeguard. Provides protection against critical hits.
    /// </summary>
    SafeguardLuckyChant,

    /// <summary>
    /// To be used alongside Safeguard. Provides protection against lowering stat stages.
    /// </summary>
    SafeguardMist,
    /// string0:
    ///     desc:   ID for text to display when protecting against a stat stage decrease
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1+:
    ///     range:  atk,def,spa,spd,spe,acc,eva,ALL
    ///     desc:   the stats to protect

    /// <summary>
    /// To be used alongside Safeguard. Provides protection against certain status conditions.
    /// </summary>
    SafeguardStatus,
    /// bool0:
    ///     desc:   set to true to invert the status filter - i.e. only the listed statuses can't be protected against
    /// bool1:
    ///     desc:   set to true to limit to non-volatile statuses only
    /// string0:
    ///     desc:   ID for text to display when protecting against a status
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1+:
    ///     range:  statusIDs, "ALL"
    ///     desc:   the status conditions that are protected against. Set to "ALL" for all statuses


    // TYPE-RELATED

    /// <summary>
    /// To be used with status moves. Prevents their effects from running if the target is immune to the move type.
    /// </summary>
    

    /// <summary>
    /// Removes types from the target upon success.
    /// </summary>
    BurnUp,
    /// string0:
    ///     desc:   ID for text to display when the types are successfully lost
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1+:
    ///     range:  typeIDs, ALL
    ///     desc:   the types that are lost. Set to ALL to lose all types.

    /// <summary>
    /// Adds additional types to this move when calculating type effectiveness.
    /// </summary>
    FlyingPress,
    /// bool0:
    ///     desc:   set to true for additional types to bypass type resistances
    /// bool1:
    ///     desc:   set to true for additional types to bypass type weaknesses
    /// bool2:
    ///     desc:   set to true for additional types to bypass type immunities
    /// stringx:
    ///     range:  typeIDs, ALL
    ///     desc:   the types that are added. Set to ALL to add all types.

    /// <summary>
    /// Adds additional types to the target.
    /// </summary>
    ForestsCurse,
    /// bool0:
    ///     desc:   set to true enable stacking with other ForestsCurse effects. If false, these types will
    ///             replace any existing ForestsCurse effects.
    /// bool1:
    ///     desc:   set to true enable effect if the target already naturally has the type (still will not stack its
    ///             own ForestsCurse effects with itself)
    /// float0:
    ///     range:  -1, 0-any
    ///     desc:   the number of turns this effect lasts for. Set to -1 to last indefinitely
    /// string0:
    ///     desc:   ID for text to display when the types are successfully added
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1+:
    ///     range:  typeIDs, ALL
    ///     desc:   the types that are added

    /// <summary>
    /// Changes this move's type effectiveness against certain types
    /// </summary>
    FreezeDry,
    /// bool0:
    ///     desc:   set to true make the listed types resist this move
    /// bool1:
    ///     desc:   set to true make the listed types weak to this move
    /// bool2:
    ///     desc:   set to true make the listed types immune to this move
    /// stringx:
    ///     range:  typeIDs, ALL
    ///     desc:   the types that have changed effectiveness. Set to ALL to add all types.

    /// <summary>
    /// Changes the move's type if the user's held item has the effect Judgment.
    /// </summary>
    Judgment,

    /// <summary>
    /// Covers the target in powder, where if the target uses moves of certain types, the move fails and
    /// they take damage.
    /// </summary>
    Powder,
    /// float0:
    ///     range:  0-1.0
    ///     desc:   the HP % lost by the afflicted pokemon
    /// string0:
    ///     desc: ID for text to display when powder is put onto the pokemon
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when powder causes a failure
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2+:
    ///     range:  type IDs
    ///     desc:   types that are blocked by powder

    /// <summary>
    /// Grounds the target if possible, making it susceptible to Ground-type moves, entry hazards,
    /// terrain, etc.
    /// </summary>
    SmackDown,
    /// string0:
    ///     desc: ID for text to display when the target is successfully grounded
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Sets the target's types.
    /// </summary>
    Soak,
    /// bool0:
    ///     desc:   set to true enable stacking with ForestsCurse effects. If false, these types will also 
    ///             overwrite any existing ForestsCurse effects.
    /// string0:
    ///     desc:   ID for text to display when the types are successfully set
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1+:
    ///     range:  typeIDs, ALL
    ///     desc:   the types that are set

    /// <summary>
    /// This move's type is altered depending on the Drive that the user holds.
    /// </summary>
    TechnoBlast,

    /// <summary>
    /// Forces a given effective hit if the target has one of the listed types, regardless of its other types.
    /// </summary>
    ThousandArrows,
    /// bool0:
    ///     desc:   set to true to force neutral effectiveness against these types
    /// bool1:
    ///     desc:   set to true to force super-effectiveness against these types
    /// bool2:
    ///     desc:   set to true to force not very effectiveness against these types
    /// bool3:
    ///     desc:   set to true to force immunity for these types
    /// float0:
    ///     range:  1-any
    ///     desc:   the number of times to apply the effectiveness
    /// stringx:
    ///     range:  type ID's
    ///     desc:   the types that this effect applies to


    // FAIL

    /// <summary>
    /// Automatically fails against Dynamaxed targets.
    /// </summary>
    FailAgainstDynamax,

    /// <summary>
    /// Automatically fails if the move effects failed.
    /// </summary>
    FailIfEffectsFail,

    /// <summary>
    /// Automatically fails if used as the last move in the turn.
    /// </summary>
    FailLastCommand,

    /// <summary>
    /// Automatically fails if the pokemon is not one of the listed pokemon. Forms of the same pokemon are valid.
    /// </summary>
    FailNotPokemon,
    /// bool0:
    ///     desc:   set to true to invert this effect - i.e. only the listed pokemon can't use this move.
    /// bool1:
    ///     desc:   set to true to allow derivative forms of these pokemon to use the move as well
    /// bool2:
    ///     desc:   set to true to allow ancestor forms of these pokemon to use the move as well
    /// bool3:
    ///     desc:   set to true to allow forms with the same ancestor as these pokemon to use the move as well
    /// stringx:
    ///     range:  pokemon ID's
    ///     desc:   the pokemon that are able to use this move

    /// <summary>
    /// Automatically fails if the pokemon is not one of the valid forms listed, but it is a form of
    /// the pokemon.
    /// </summary>
    FailNotPokemonText,
    /// string0:
    ///     desc:   ID for text to display when failed because of pokemon
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when failed because of not correct form
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Automatically fails if the current terrain is not one of those listed.
    /// </summary>
    FailTerrain,
    /// bool0:
    ///     desc:   inverts this effect - i.e. fails if the current terrain is one of those listed
    /// bool1:
    ///     desc:   set to true to allow derivatives of these terrain to use the move as well
    /// bool2:
    ///     desc:   set to true to allow ancestors of these terrain to use the move as well
    /// bool3:
    ///     desc:   set to true to allow terrain with the same ancestors as these terrain to use the move as well
    /// stringx:
    ///     desc:   terrain IDs that this effect applies to

    /// <summary>
    /// Automatically fails if the user does not have one of the listed types.
    /// </summary>
    FailUserType,
    /// bool0:
    ///     desc:   set to true to invert this filter
    /// bool1:
    ///     desc:   set to true to allow derivatives of these weathers to use the move as well
    /// bool2:
    ///     desc:   set to true to allow ancestors of these weathers to use the move as well
    /// bool3:
    ///     desc:   set to true to allow weathers with the same ancestors as these weathers to use the move as well
    /// stringx:
    ///     range:  typeIDs
    ///     desc:   types that the user must be to use the move

    /// <summary>
    /// Automatically fails if the current weather is not one of those listed.
    /// </summary>
    FailWeather,
    /// bool0:
    ///     desc:   inverts this effect - i.e. fails if the current weather is one of those listed
    /// bool1:
    ///     desc:   set to true to allow derivatives of these weathers to use the move as well
    /// bool2:
    ///     desc:   set to true to allow ancestors of these weathers to use the move as well
    /// bool3:
    ///     desc:   set to true to allow weathers with the same ancestors as these weathers to use the move as well
    /// stringx:
    ///     desc:   weather IDs that this effect applies to

    /// <summary>
    /// Fails if used after X amount of turns the user is in battle.
    /// </summary>
    FakeOut,
    /// float0:
    ///     range: 1-any
    ///     desc: if the user attempts to use this move after the X'th turn, the move automatically fails.


    // MISC

    /// <summary>
    /// Causes the target to execute its command right after this move has finished executing.
    /// </summary>
    AfterYou,
    /// string0:
    ///     desc:   ID for text to display when successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Causes the user to switch positions with one of its allies.
    /// </summary>
    AllySwitch,
    /// string0:
    ///     desc: ID for text to display when successful
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Switches the user out after the move is used successfully (if the trainer has pokemon to switch into).
    /// </summary>
    BatonPass,
    /// bool0: 
    ///     desc: set to true to pass properties to switch-in teammates like Baton Pass

    /// <summary>
    /// Displays certain text in battle.
    /// </summary>
    Celebrate,
    /// string0:
    ///     desc: ID for text to display
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    Chance,
    /// bool0: 
    ///     desc: set to true for all the next effects to fail, should the chance fail
    /// float0: 
    ///     range: 0-1
    ///     desc: the chance to succeed
    /// float1: 
    ///     range: 0-any
    ///     desc: the next float1 effects to skip should the chance fail (if bool0 is false)

    /// <summary>
    /// Sets a status upon the user where if they faint due to a direct attack, their attacker faints alongside
    /// them.
    /// </summary>
    DestinyBond,
    /// string0:
    ///     desc: ID for text to display when destiny bond is activated
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when destiny bond causes an attacker to faint
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    DisableTurnRange,
    /// Must be used alongside "Disable".
    /// Randomly selects the amount of turns disabled between float0 and float1 inclusive.
    /// Overrides float0 in "Disable".
    /// float0:
    ///     range: 0-any
    ///     desc: the lowest amount of turns that the move will be disabled
    /// float1:
    ///     range: higher than float0
    ///     desc: the highest amount of turns that the move will be disabled

    /// <summary>
    /// Prevents this move from fatally damaging a target. They are always left with at least 1 HP.
    /// </summary>
    FalseSwipe,

    /// <summary>
    /// Charges at the beginning of the turn. If the user takes damage before it attacks, the user loses focus
    /// and does not attack.
    /// </summary>
    FocusPunch,
    /// string0:
    ///     desc: ID for text to display when charging up
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when losing focus
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Makes the user the center of attention, re-directing opponent single-targeting moves to itself
    /// for the remainder of the turn.
    /// </summary>
    FollowMe,
    /// string0:
    ///     desc: ID for text to display when the pokemon successfully becomes the center of attention.
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Displays certain text if used successfully on a target.
    /// </summary>
    HoldHands,
    /// bool0: 
    ///     desc:   set to true to automatically fail in a singles battle
    /// string0:
    ///     desc:   ID for text to display
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    MagicCoat,
    /// Applies magic coat to the target for the turn 
    /// reflecting all moves used on it that are tagged with "MagicCoatSusceptible"
    /// string0:
    ///     desc: ID for text to display when the move is reflected
    ///           set to null for default text

    /// <summary>
    /// Copies the last move the target used into its own moveslot. Fails if the user already knows the move.
    /// </summary>
    Mimic,

    /// <summary>
    /// Applies the minimize effect to the target pokemon until they switch out.
    /// </summary>
    Minimize,

    PayDay,
    /// float0:
    ///     range: 0-any
    ///     desc: float0 multiplied by the user's level determines the payout for each use

    /// <summary>
    /// This move will not deal damage to ally Pokemon.
    /// </summary>
    PollenPuff,

    /// <summary>
    /// Causes the target to execute its command last during the turn, if they haven't acted already.
    /// </summary>
    Quash,
    /// string0:
    ///     desc: ID for text to display when successful
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Restores a consumed item.
    /// </summary>
    Recycle,
    /// string0:
    ///     desc:   ID for text to display when the pokemon successfully recycles an item
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Removes entry hazards from the target's team.
    /// </summary>
    RemoveEntryHazard,

    /// <summary>
    /// Removes reflect screens from the target's team.
    /// </summary>
    RemoveReflect,

    /// <summary>
    /// Grounds the user for the rest of the turn.
    /// </summary>
    Roost,

    /// <summary>
    /// Removes types from the user for the rest of the turn. 
    /// </summary>
    RoostTypeLoss,
    /// stringx:
    ///     range:  type ID's
    ///     desc:   the types that are lost for the rest of the turn

    /// <summary>
    /// Charges at the beginning of the turn. If the user does not take damage before it attacks, the move fails.
    /// </summary>
    ShellTrap,
    /// bool0:
    ///     desc:   set to true to only induce effect on physical attacks
    /// bool1:
    ///     desc:   set to true to only induce effect when enemies attack
    /// string0:
    ///     desc:   ID for text to display when charging up
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when the move fails
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Allows the user to learn the target's last used move permanently, replacing this move in the process.
    /// Fails if the user already knows the move.
    /// </summary>
    Sketch,
    /// string0:
    ///     desc:   ID for text to display when sketch succeeds
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Allows the user to hijack moves tagged with "Snatchable".
    /// </summary>
    Snatch,
    /// string0:
    ///     desc: ID for text to display when the user is attempting to snatch a move
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the user succeeded in snatching a move
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// This move can be used even while a pokemon is sleeping.
    /// </summary>
    Snore,

    Struggle,
    /// Bypasses checks to always be successful.
    /// Ignores ability effects "Reckless" and "RockHead".
    /// Redirects targets to "Adjacent".

    /// <summary>
    /// This move can be used even while a pokemon is sleeping.
    /// </summary>
    Substitute,
    /// float0:
    ///     range:  0-1
    ///     desc:   HP % that the substitute has
    /// float1:
    ///     range:  0-1
    ///     desc:   HP % needed (and lost) to create the substitute
    /// string0:
    ///     desc:   ID for text to display when the subsititute is created
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string1:
    ///     desc:   ID for text to display when the subsititute is destroyed
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string2:
    ///     desc:   ID for text to display when the subsititute is already created (fails to create)
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string3:
    ///     desc:   ID for text to display when the user doesn't have enough HP for a substitute
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string4:
    ///     desc:   ID for text to display when the substitute blocks an attack
    ///             set to "DEFAULT" for default text
    ///             set to null for no text
    /// string5:
    ///     desc:   ID for text to display when the substitute takes damage
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Locks the user into a move that randomly targets opponents for a set amount of turns.
    /// </summary>
    Thrash,
    /// float0:
    ///     range: 0-any
    ///     desc: the amount of turns that the move will be locked into
    /// float1:
    ///     range: -1, 0-any
    ///     desc: the amount of turns that the status will be inflicted for
    /// string0:
    ///     range: statusID
    ///     desc: the status to inflict on the user once all thrash turns have passed
    /// string1:
    ///     desc: the text to display once the status is inflicted on the user

    ThrashTurnRange,
    /// Must be used alongside "Thrash" or "Uproar".
    /// Randomly selects the amount of turns locked between float0 and float1 inclusive.
    /// Overrides float0 in "Thrash".
    /// float0:
    ///     range: 0-any
    ///     desc: the lowest amount of turns that the move will be locked into
    /// float1:
    ///     range: higher than float0
    ///     desc: the highest amount of turns that the move will be locked into

    ThrashStatusTurnRange,
    /// Must be used alongside "Thrash".
    /// Randomly selects the amount of turns that the thrash status lasts between float0 and float1 inclusive.
    /// Overrides float1 in "Thrash".
    /// float0:
    ///     range: 0-any
    ///     desc: the lowest amount of turns that status will be inflicted for
    /// float1:
    ///     range: higher than float0
    ///     desc: the highest amount of turns that status will be inflicted for

    /// <summary>
    /// The user transforms into the target, changing the user's type, species, stats, stat modifications,
    /// and moveset to match.
    /// </summary>
    Transform,
    /// string0:
    ///     desc:   ID for text to display when the transformation is successful
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Does not take into account the move's type when dealing damage.
    /// </summary>
    TypelessDamage,

    /// <summary>
    /// User continuously uses this move for a set amount of turns. Pokemon without soundproofing
    /// cannot fall asleep or are immediately woken up while Uproar is in effect.
    /// </summary>
    Uproar,
    /// float0:
    ///     range: 0-any
    ///     desc: the amount of turns that the move will be locked into
    /// string0:
    ///     desc: the text to display once the user starts an uproar
    /// string1:
    ///     desc: the text to display once the user stops the uproar
    /// string2:
    ///     desc: the text to display whenever uproar prevents a pokemon from sleeping

    /// <summary>
    /// Forces out the target into a random pokemon from their trainer's party.
    /// </summary>
    Whirlwind,
    /// string0:
    ///     desc: ID for text to display when the target is blown away
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when a pokemon is forced into battle
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

}
public enum MoveEffectTiming
{
    /// <summary>
    /// Handled elsewhere in script. The default timing specification.
    /// </summary>
    Unique,

    /// <summary>
    /// Occurs for any timing specification except for Unique.
    /// </summary>
    Any,

    /// <summary>
    /// Specifically for multi-hit moves, occurs on the charging turns.
    /// </summary>
    OnChargeTurn,

    /// <summary>
    /// Occurs before any hits are calculated.
    /// </summary>
    BeforeMoveUse,

    /// <summary>
    /// Occurs for each target before each hit.
    /// </summary>
    BeforeTargetImpact,
    
    /// <summary>
    /// Occurs for each target after each successful hit.
    /// </summary>
    AfterTargetImpact,

    /// <summary>
    /// Occurs after each successful move hit (after all <seealso cref="AfterTargetImpact"/>).
    /// </summary>
    AfterSuccessfulHit,

    /// <summary>
    /// Occurs after a failed move hit (after all <seealso cref="AfterTargetImpact"/>).
    /// </summary>
    AfterFailureHit,

    /// <summary>
    /// Occurs after full move execution (all hits) and success.
    /// </summary>
    AfterSuccessfulMoveUse,

    /// <summary>
    /// Occurs after full move execution (all hits), but the move failed.
    /// </summary>
    AfterFailureMoveUse,

    /// <summary>
    /// Occurs after full move execution (all hits).
    /// </summary>
    AfterMoveUse,

}
public enum MoveEffectTargetType
{
    Unique,
    Self,
    Target,
    SelfTeam,
    Team,
    Battlefield
}
public enum MoveEffectOccurrence
{
    None,
    OnceForEachTarget,
    OnceForEachTeam,
    Once
}
public enum MoveEffectFilter
{
    /// <summary>
    /// This effect will only be applied to allies.
    /// </summary>
    AlliesOnly,

    /// <summary>
    /// This effect will only be applied to enemies.
    /// </summary>
    EnemiesOnly,
}


// Abilities
public enum AbilityTag
{
    /// <summary>
    /// This ability cannot be bypassed by abilities or moves that would usually ignore this ability 
    /// (ex. Mold Breaker, Sunsteel Strike)
    /// </summary>
    BypassMoldBreaker,

    /// <summary>
    /// This ability cannot be copied via mechanics such as Role Play.
    /// </summary>
    CannotRolePlay,

    /// <summary>
    /// This ability cannot be replaced via mechanics such as Role Play.
    /// </summary>
    CannotRolePlayUser,

    /// <summary>
    /// This ability cannot be swapped via mechanics such as Skill Swap or Mummy.
    /// </summary>
    CannotSkillSwap,

    /// <summary>
    /// This ability cannot be replaced via mechanics such as Skill Swap or Mummy.
    /// </summary>
    CannotSkillSwapUser,

    /// <summary>
    /// This ability cannot be replaced via mechanics such as Worry Seed or Entrainment.
    /// </summary>
    CannotWorrySeed,

    /// <summary>
    /// This ability cannot be suppressed by moves such as Gastro Acid or Core Enforcer.
    /// </summary>
    CannotSuppress,

    /// <summary>
    /// This ability cannot be neutralized by mechanics such as Neutralizing Gas.
    /// </summary>
    CannotNeutralize,
}
public enum AbilityEffectType
{
    Adaptability,
    Aerilate,
    Aftermath,
    AirLock,
    Analytic,
    AngerPoint,
    Anticipation,
    AromaVeil,
    AuraBreak,
    BadDreams,
    BallFetch,
    Battery,
    Berserk,
    BattleArmor,
    BattleBond,
    BeastBoost,
    Bulletproof,
    Cacophony,
    CheekPouch,
    Chlorophyll,
    ColorChange,
    Comatose,
    CompoundEyes,
    Damp,
    Dancer,
    DarkAura,
    Defiant,
    Disguise,
    Download,
    Drought,
    DrySkin,
    EarlyBird,
    EffectSpore,
    FlameBody,
    FlareBoost,
    FlowerGift,
    Forecast,
    Forewarn,
    FriendGuard,
    Frisk,
    GaleWings,
    Gluttony,
    Gooey,
    GorillaTactics,
    GulpMissile,
    Guts,
    Harvest,
    Healer,
    HeavyMetal,
    HoneyGather,
    HungerSwitch,
    Hustle,
    Hydration,
    HyperCutter,
    IceScales,
    Illusion,
    InnerFocus,
    Intimidate,
    IntimidateBlock,
    IntimidateTrigger,
    IntrepidSword,
    Justified,
    Levitate,
    Limber,
    LongReach,
    MagicBounce,
    MagicGuard,
    MegaLauncher,
    Merciless,
    Mimicry,
    Minus,
    MirrorArmor,
    MoldBreaker,
    Moody,
    Moxie,
    Multiscale,
    Mummy,
    NaturalCure,
    Neuroforce,
    NeutralizingGas,
    Oblivious,
    ParentalBond,
    Pickpocket,
    Pickup,
    PoisonHeal,
    PoisonPoint,
    PoisonTouch,
    PowerOfAlchemy,
    Pressure,
    Prankster,
    PropellerTail,
    Protean,
    QueenlyMajesty,
    QuickDraw,
    Regenerator,
    Ripen,
    Rivalry,
    RKSSystem,
    RoughSkin,
    RunAway,
    Scrappy,
    ScreenCleaner,
    ShadowTag,
    SheerForce,
    ShieldDust,
    ShieldsDown,
    SkillLink,
    SlowStart,
    SolidRock,
    SoulHeart,
    SpeedBoost,
    Stall,
    Steadfast,
    Stakeout,
    StanceChange,
    Steelworker,
    Stench,
    StickyHold,
    SuctionCups,
    SuperLuck,
    Symbiosis,
    SwiftSwim,
    Synchronize,
    Technician,
    Telepathy,
    ThickFat,
    TintedLens,
    Trace,
    Truant,
    ToxicBoost,
    Unaware,
    Unburden,
    UnseenFist,
    VoltAbsorb,
    WimpOut,
    WonderGuard,
    WonderSkin,
    ZenMode,


    // ACCURACY

    /// <summary>
    /// Bypasses accuracy checks when attacking with this Pokemon, or targeting this Pokemon.
    /// </summary>
    NoGuard,


    // DAMAGE MULTIPLIERS

    /// <summary>
    /// Scales damage based on the user's HP threshold, and applies to specific types.
    /// </summary>
    Blaze,
    /// bool0:
    ///     desc:   set to true to invert the HP threshold - i.e. HP at or above the threshold triggers effect
    /// bool1:
    ///     desc:   set to true to invert the type list - i.e. types not in the list are affected instead
    /// float0: 
    ///     range:  0-any
    ///     desc:   damage multiplier for affected moves
    /// float1:
    ///     range:  0-1
    ///     desc:   User's HP % threshold for effect to occur. Must be at or below threshold
    /// stringx:
    ///     range:  typeIDs
    ///     desc:   list of types that this effect applies to

    /// <summary>
    /// Scales damage if this move is tagged with "PunchMove".
    /// </summary>
    IronFist,
    /// float0: 
    ///     range:  0-any
    ///     desc:   damage multiplier

    /// <summary>
    /// Scales damage if this move has the effects Recoil or JumpKick.
    /// </summary>
    Reckless,
    /// float0: 
    ///     range:  0-any
    ///     desc:   damage multiplier

    /// <summary>
    /// Changes the critical hit multiplier.
    /// </summary>
    Sniper,
    /// float0: 
    ///     range:  0-any
    ///     desc:   new critical hit multiplier

    /// <summary>
    /// Scales damage if this move is tagged with "BiteMove".
    /// </summary>
    StrongJaw,
    /// float0: 
    ///     range:  0-any
    ///     desc:   damage multiplier


    // ITEMS

    /// <summary>
    /// Prevents the user's held items from having any effect.
    /// </summary>
    Klutz,
    
    /// <summary>
    /// Steals the target's held item upon attacking it with a move.
    /// </summary>
    Magician,
    /// bool0:
    ///     desc:   set to true limit to only damaging moves
    /// string0:
    ///     desc:   ID for text to display when the item is stolen
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Prevents the opposing pokemon from eating berries.
    /// </summary>
    Unnerve,
    /// string0:
    ///     desc:   ID for text to display when the pokemon's ability is activated
    ///             set to "DEFAULT" for default text
    ///             set to null for no text


    // HEALTH

    /// <summary>
    /// Prevents the pokemon from being inflicted with the listed status conditions.
    /// </summary>
    Immunity,
    /// stringx:
    ///     range: status ID's
    ///     desc:  the status conditions that this effect applies to

    /// <summary>
    /// Inverts the effects of HP-draining moves against this Pokemon.
    /// </summary>
    LiquidOoze,

    /// <summary>
    /// Prevents recoil damage from moves. Does not work if "Struggle" effect is paired along with recoil.
    /// </summary>
    RockHead,


    // PROTECTION

    /// <summary>
    /// Prevents the pokemon from being hurt by weather conditions.
    /// </summary>
    Overcoat,

    /// <summary>
    /// Prevents the pokemon from being inflicted with moves tagged with "Powder".
    /// </summary>
    Overcoat2,

    /// <summary>
    /// Prevents the pokemon from being affected by sound moves.
    /// </summary>
    Soundproof,

    /// <summary>
    /// Prevents this pokemon from fainting from a direct hit at maximum HP.
    /// </summary>
    Sturdy,
    /// string0:
    ///     desc:   ID for text to display when the pokemon's ability is activated
    ///             set to "DEFAULT" for default text
    ///             set to null for no text


    // STATUS

    /// <summary>
    /// Allows this pokemon to inflict the listed status regardless of type.
    /// </summary>
    Corrosion,
    /// stringx:
    ///     range:  status ID's
    ///     desc:   the status conditions that this effect applies to
    


    // STAT STAGES

    /// <summary>
    /// Reverses all stat stage changes applied to this Pokemon.
    /// </summary>
    Contrary,

    /// <summary>
    /// Scales all stat stage changes applied to this Pokemon.
    /// </summary>
    Simple,
    /// float0:
    ///     range:  1-any
    ///     desc:   factor to scale stat stage changes

    /// <summary>
    /// Enables this pokemon to ignore stat stage changes of attackers.
    /// </summary>
    UnawareDefense,
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to ignore the changes of

    /// <summary>
    /// Enables this pokemon to ignore stat stage changes of targets when attacking.
    /// </summary>
    UnawareOffense,
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc: the stats to ignore the changes of


    // TYPE-RELATED

    /// <summary>
    /// Redirects moves of the given types to this pokemon.
    /// </summary>
    LightningRod,
    /// stringx:
    ///     range:  type ID's
    ///     desc:   the move types that are redirected to this pokemon

    /// <summary>
    /// Changes the user's type depending on the Plate it is holding.
    /// </summary>
    Multitype,


    // MISC

    /// <summary>
    /// Ignores substitutes and screens when attacking.
    /// </summary>
    Infiltrator,

    /// <summary>
    /// Scales move effect chance.
    /// </summary>
    SereneGrace,
    /// float0:
    ///     range:  0-any
    ///     desc:   factor to scale stat stage changes

}
public enum AbilityEffectTargetType
{
    Unique,
    Target,
    Team
}


// Status Conditions
public enum StatusType
{
    Pokemon,
    Team,
    Battle,
}
public enum PokemonSTag
{
    /// <summary>
    /// Given to default status conditions.
    /// </summary>
    IsDefault,

    /// <summary>
    /// This condition doesn't stay on its target, but specified effects still apply.
    /// </summary>
    NonStick,

    NonVolatile,

    RequiresPokemonCauseInBattle,
    /// This condition is removed immediately if the pokemon who caused the ailment is not in battle

    /// <summary>
    /// This condition reduces its turns left at the end of the turn.
    /// </summary>
    TurnsDecreaseOnEnd,

    /// <summary>
    /// This condition reduces its turns left when the pokemon is attempting to use a move.
    /// </summary>
    TurnsDecreaseOnMove,

}
public enum PokemonSEType
{
    Burn,
    DefenseCurl,
    Electrify,
    Embargo,
    Flinch,
    HealBlock,
    Identified,
    Imprison,
    Infatuation,
    NonVolatile,
    Octolock,
    PerishSong,
    TarShot,
    Taunt,
    Torment,
    Yawn,


    // DAMAGE

    /// <summary>
    /// Non-Stick Effect. Traps the target while dealing gradual damage every turn for a set amount of turns. 
    /// The effect can be lifted early if the user leaves battle.
    /// </summary>
    Bound,
    /// float0:
    ///     range: 0-1
    ///     desc: the HP % lost each turn for pokemon afflicted with bind
    /// float1:
    ///     range: 1-any
    ///     desc: the amount of turns that this affliction lasts for
    /// string0:
    ///     desc: ID for text to display when bound is inflicted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when damage is inflicted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string2:
    ///     desc: ID for text to display when bound is lifted
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string3:
    ///     desc: ID for text to display when the pokemon attempts to switch out when bound
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// Recovers a % of the Pokemon's HP.
    /// </summary>
    HPGain,
    /// float0: 
    ///     range:  0-1
    ///     desc:   HP % lost
    /// string0:
    ///     desc:   ID for text to display when HP is gained
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// Loses a % of the Pokemon's HP.
    /// </summary>
    HPLoss,
    /// float0: 
    ///     range:  0-1
    ///     desc:   HP % lost
    /// string0:
    ///     desc:   ID for text to display when damage is inflicted
    ///             set to "DEFAULT" for default text
    ///             set to null for no text

    /// <summary>
    /// To be used alongside HPLoss. Increases damage inflicted with each passing active turn.
    /// </summary>
    ToxicStack,
    /// float0:
    ///     range:  0-any
    ///     desc:   HP % to add to HPLoss every turn


    // TYPES

    /// <summary>
    /// Prevents pokemon of the listed types from being affected by this condition.
    /// </summary>
    TypeImmunity,
    /// stringx:
    ///     range: type ID's
    ///     desc:  the types that this effect applies to


    // STATS

    /// <summary>
    /// Scales certain stats of this pokemon while this status is acquired.
    /// </summary>
    StatScale,
    /// float0:
    ///     range:  0-any
    ///     desc:   the value that the given stats will be scaled by
    /// stringx:
    ///     range: atk,def,spa,spd,spe,acc,eva,all
    ///     desc:  the stats that this effect applies to


    // MISC
    Confusion,
    /// Cannot stack with other confusion statuses.
    /// bool0:
    ///     desc: set to true for confusion damage to use physical attack, false for special attack
    /// float0: 
    ///     range: 0-1
    ///     desc: chance of pokemon hitting itself
    /// float1: 
    ///     range: 1-any
    ///     desc: the base power used to calculate confusion damage
    /// string0:
    ///     desc: ID for text to display to indicate the pokemon is confused
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the pokemon hits itself in confusion
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    Disable,
    /// Prevents the pokemon from using its last move.
    /// The whole effect cannot be inflicted if the target hasn't used a move.
    /// string0:
    ///     desc: ID for text to display when the pokemon attempts to use the disabled move
    ///           set to default for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when the player tries to choose disabled moves
    ///           set to default for default text
    ///           set to null for no text

    /// <summary>
    /// Prevents the target from using any other move than its last move for a certain amount of turns.
    /// </summary>
    Encore,
    /// string0:
    ///     desc: ID for text to display when the target attempts to use a move while encored
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    /// <summary>
    /// The default amount of turns left for this condition if left unspecified.
    /// </summary>
    DefaultTurnsLeft,
    /// bool0:
    ///     desc:   set to true to set a turn range for this status, between float0 and float1
    /// float0:
    ///     range:  -1, 0-any
    ///     desc:   the number of turns to inflict status. If set to -1, the status lasts indefinitely.
    ///             If bool1 is true, this is the lowest amount of turns for this status
    /// float1:
    ///     range:  0-any, at least as high as float0
    ///     desc:   the maximum number of turns to inflict status if bool1 is true

    Freeze,
    /// Prevents the pokemon from attacking.
    /// Cannot stack with other freeze statuses.
    /// float0: 
    ///     range: 0-1
    ///     desc: chance of pokemon thawing out when attacking
    /// string0:
    ///     desc: ID for text to display to indicate the pokemon is frozen
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    FreezeThaw,
    /// Must be used alongside "Freeze".
    /// Damaging moves used by the listed types can instantly thaw out a pokemon.
    /// stringx:
    ///     range: type ID's
    ///     desc: the types that can instantly thaw out a pokemon

    /// <summary>
    /// Grounds this Pokemon, regardless of ability or typing.
    /// </summary>
    Grounded,

    Paralysis,
    /// Cannot stack with other paralysis statuses.
    /// float0: 
    ///     range: 0-1
    ///     desc: chance of pokemon not attacking
    /// string0:
    ///     desc: ID for text to display when the pokemon fails to attack due to paralysis
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    Sleep,
    /// Prevents the pokemon from attacking.
    /// Cannot stack with other sleep statuses.
    /// string0:
    ///     desc: ID for text to display to indicate the pokemon is sleeping
    ///           set to "DEFAULT" for default text
    ///           set to null for no text

    Trap,
    /// Prevents the pokemon from switching out of battle.
    /// string0:
    ///     desc: ID for text to display when attempting to escape when trapped
    ///           set to default for default text
    ///           set to null for no text

}
public enum PokemonSETiming
{
    Unique,
    /// handled uniquely in script

    Any,
    /// Occurs at any of the below

    EndOfTurn,
    /// Occurs at the end of the turn

    OnHeal,
    /// Occurs when the status is about to be healed

}


// Team Status Conditions
public enum TeamSTag
{
    /// <summary>
    /// This condition doesn't stick to its team if successfully inflicted, but specified effects still apply.
    /// </summary>
    NonStick,

    /// <summary>
    /// This condition reduces its turns left at the end of the turn.
    /// </summary>
    TurnsDecreaseOnEnd,
}
public enum TeamSEType
{
    EntryHazard,
    GMaxWildfirePriority,
    HPLoss,
    LightScreen,

    // ENTRY HAZARDS




    // PROTECTION




    // STATS




    // MISC

    /// <summary>
    /// The default amount of turns left for this condition if left unspecified.
    /// </summary>
    DefaultTurnsLeft,
    /// bool0:
    ///     desc:   set to true to set a turn range for this status, between float0 and float1
    /// float0:
    ///     range:  -1, 0-any
    ///     desc:   the number of turns to inflict status. If set to -1, the status lasts indefinitely.
    ///             If bool1 is true, this is the lowest amount of turns for this status
    /// float1:
    ///     range:  0-any, at least as high as float0
    ///     desc:   the maximum number of turns to inflict status if bool1 is true



}
public enum TeamSETiming
{
    /// <summary>
    /// Handled uniquely in script.
    /// </summary>
    Unique,

    /// <summary>
    /// Occurs at any of the below.
    /// </summary>
    Any,

    /// <summary>
    /// Occurs immediately when status starts.
    /// </summary>
    OnStart,

    /// <summary>
    /// Occurs at the end of the turn.
    /// </summary>
    EndOfTurn,

    /// <summary>
    /// Occurs when the status is healed.
    /// </summary>
    OnHeal,

}


// Battlefield Conditions
public enum BattleSTag
{
    /// <summary>
    /// Set for the default battle environments (weather, terrain, etc.)
    /// </summary>
    Default,

    /// <summary>
    /// Most of the effects of this condition will only apply to non-grounded Pokémon.
    /// </summary>
    IsAerial,

    /// <summary>
    /// Most of the effects of this condition will only apply to grounded Pokémon.
    /// </summary>
    IsGrounded,

    /// <summary>
    /// This condition doesn't stick to the battle if successfully inflicted, but specified effects still apply.
    /// </summary>
    NonStick,

    TurnsDecreaseOnEnd,
    // This condition reduces its turns left at the end of the turn

    UndoesSelf,
    // This means that the status will undo itself if it is initiated while already in battle
}
public enum BattleSEType
{
    BattleEnvironment,
    BlockMoves,
    BlockStatus,
    DesolateLand,
    HPGain,
    HPLoss,
    MoveDamageModifier,
    StatScale,
    StrongWinds,
    TypeDamageModifier,

    // GRAVITY

    /// <summary>
    /// Grounds all pokemon for a set amount of turns.
    /// </summary>
    Gravity,
    /// float0: 
    ///     range:  -1, 0-any
    ///     desc:   Priority. It can only replace other gravity conditions of the same priority or lower.
    ///             Set to -1 to override all other conditions.

    /// <summary>
    /// Instantly grounds all pokemon, and cancels all "flying" moves in-progress (ex. Fly, Jump Kick, Sky Drop).
    /// </summary>
    IntensifyGravity,
    /// string0:
    ///     desc: ID for text to display when pokemon become grounded
    ///           set to "DEFAULT" for default text
    ///           set to null for no text
    /// string1:
    ///     desc: ID for text to display when a pokemon attempts to use a flying move
    ///           set to "DEFAULT" for default text
    ///           set to null for no text


    // MOVES

    /// <summary>
    /// Changes the types of moves while this status is active.
    /// </summary>
    IonDeluge,
    /// string0:
    ///     range:  type IDs
    ///     desc:   the type of moves to change into the type string1
    /// string1:
    ///     range:  type IDs
    ///     desc:   the type that moves of type string0 change into

    /// <summary>
    /// This condition will help create Secret Power's secondary effects. 
    /// </summary>
    SecretPower,
    /// floatx:
    ///     range:  -1, 0-1
    ///     desc:   the effect chance floatx for the move effect stringx
    /// stringx:
    ///     range:  MoveEffectTypes - "inflictstatus,flinch,statstagemod,statstageselfmod,etc."
    ///     desc:   for each effect listed, there should be a corresponding SecretPowerMoveEffect,
    ///             and its parameter configuration should match the move effect. The SecretPowerMoveEffects
    ///             should be listed in the same order as these effects

    /// <summary>
    /// To be used alongside SecretPower. Defines the parameters for a specified move effect in SecretPower's stringx.
    /// </summary>
    SecretPowerMoveEffect,
    /// boolx: varies
    /// floatx: varies
    /// stringx: varies


    


    // ROOM

    /// <summary>
    /// Suppresses the effects of held items.
    /// </summary>
    MagicRoom,
    /// float0: 
    ///     range:  -1, 0-any
    ///     desc:   Priority. It can only replace other magic room conditions of the same priority or lower.
    ///             Set to -1 to override all other conditions.

    /// <summary>
    /// Determines turn order by a specified stat. Does not override priority.
    /// </summary>
    TrickRoom,
    /// bool0: 
    ///     desc:   Reverse this effect.
    /// float0: 
    ///     range:  -1, 0-any
    ///     desc:   Priority. It can only replace other trick room conditions of the same priority or lower.
    ///             Set to -1 to override all other conditions.
    /// string0:
    ///     range:  atk,def,spa,spd,spe
    ///     desc:   stat used to determine speed

    /// <summary>
    /// Swaps 2 specified stats for every pokemon in battle.
    /// </summary>
    WonderRoom,
    /// float0: 
    ///     range:  -1, 0-any
    ///     desc:   Priority. It can only replace other wonder room conditions of the same priority or lower.
    ///             Set to -1 to override all other conditions.
    /// string0:
    ///     range:  atk,def,spa,spd,spe
    ///     desc:   stat to swap with string1
    /// string1:
    ///     range:  atk,def,spa,spd,spe
    ///     desc:   stat to swap with string0


    // TERRAIN

    /// <summary>
    /// Indicates that this is a terrain condition.
    /// </summary>
    Terrain,
    /// float0: 
    ///     range:  -1, 0-any
    ///     desc:   Priority. It can only replace other terrain conditions of the same priority or lower.
    ///             Set to -1 to override all other conditions.


    // WEATHER

    /// <summary>
    /// Indicates that this is a weather condition.
    /// </summary>
    Weather,
    /// float0: 
    ///     range:  -1, 0-any
    ///     desc:   Priority. It can only replace other weather conditions of the same priority or lower.
    ///             Set to -1 to override all other conditions.


    // MISC

    /// <summary>
    /// Displays text when a lower priority condition fails to overwrite this condition.
    /// </summary>
    BlockLowerPriorityCondition,
    /// string0:
    ///     desc:   ID for text to display when the the lower priority condition is blocked
    ///             set to "DEFAULT" for default text
    ///             set to null for no text


}
public enum BattleSETiming
{
    /// <summary>
    /// Handled uniquely in script.
    /// </summary>
    Unique,

    /// <summary>
    /// Occurs at any of the below.
    /// </summary>
    Any,

    /// <summary>
    /// Occurs at the end of the turn.
    /// </summary>
    EndOfTurn,

    /// <summary>
    /// Occurs when the status is about to be healed.
    /// </summary>
    OnHeal,

    /// <summary>
    /// Occurs when the status is initialized.
    /// </summary>
    OnStart,
}


// Battle
public enum BattleType
{
    Single,
    Double,
    Triple
}
public enum BattleOrder
{
    First,
    Last,
    SpeedTie
}
public enum BattleCommandType
{
    None,

    Fight,
    Party,
    PartyReplace, // Replace pokemon
    Bag,
    Run, GiveUp,

    Recharge,     // Hyper Beam

    Back
}
public enum BattleExtraCommand
{
    Summary,
    Switch,
    Moves,
    Cancel
}

public enum BattleEnvironmentType
{
    Building,
    Cave,
    Field,
    Sand,
    Snow,
    Swamp,
    Water,
    Volcano,
    UltraSpace,
}

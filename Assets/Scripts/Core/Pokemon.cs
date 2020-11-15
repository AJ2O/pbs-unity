using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Core.Pokemon
{

}

public class Pokemon
{
    public class Ability
    {
        public string abilityID;
        public bool activated;
        public bool isSuppressed;
        public int turnsActive;
        public AbilityData data
        {
            get
            {
                return AbilityDatabase.instance.GetAbilityData(abilityID);
            }
        }

        public Ability(string abilityID, bool activated = false, bool isSuppressed = false, int turnsActive = 0)
        {
            this.abilityID = abilityID;
            this.activated = activated;
            this.isSuppressed = isSuppressed;
            this.turnsActive = turnsActive;
        }
        public Ability Clone()
        {
            return new Ability(
                abilityID: abilityID, 
                activated: activated,
                isSuppressed: isSuppressed,
                turnsActive: turnsActive);
        }
        public Ability TransformClone()
        {
            return new Ability(abilityID: abilityID);
        }

    }
    public class AbilityEffectPair
    {
        public Ability ability;
        public EffectDatabase.AbilityEff.AbilityEffect effect;
        public AbilityEffectPair(
            Ability ability,
            EffectDatabase.AbilityEff.AbilityEffect effect)
        {
            this.ability = ability;
            this.effect = effect;
        }
        public AbilityEffectPair Clone()
        {
            return new AbilityEffectPair(
                ability: ability.Clone(),
                effect: effect.Clone()
                );
        }
    }
    public class Moveslot
    {
        public string moveID;
        public int PP;
        public int PPUps;
        private int p_maxPP;
        public int maxPP
        {
            get
            {
                if (p_maxPP >= 0)
                {
                    return p_maxPP;
                }

                int basePP = MoveDatabase.instance.GetMoveData(moveID).PP;
                int bonusPP = (basePP / 5) * PPUps;
                return basePP + bonusPP;
            }
        }
        public MoveData data
        {
            get
            {
                return MoveDatabase.instance.GetMoveData(moveID);
            }
        }

        // Constructor
        public Moveslot(string moveID, int PPUps = 0, int PP = -1, int p_maxPP = -1)
        {
            this.p_maxPP = p_maxPP;

            this.moveID = moveID;
            this.PPUps = PPUps;

            if (PP < 0)
            {
                this.PP = maxPP;
            }
            else
            {
                this.PP = PP;
            }
        }

        // Clone
        public static Moveslot Clone(Moveslot original)
        {
            Moveslot cloneSlot = new Moveslot(
                moveID: original.moveID,
                PPUps: original.PPUps,
                PP: original.PP,
                p_maxPP: original.p_maxPP
                );
            return cloneSlot;
        }
    }

    public class BattleProperties
    {
        public List<Ability> abilities;

        public class Bound
        {
            public string boundCauser;
            public int turnsLeft;
            StatusPKData statusData;

            public Bound(
                string boundCauser,
                StatusPKData statusData,
                int turnsLeft
                )
            {
                this.boundCauser = boundCauser;
                this.statusData = statusData;
                this.turnsLeft = turnsLeft;
            }
            public Bound Clone()
            {
                return new Bound(boundCauser, statusData, turnsLeft);
            }
        }
        public Bound bound;

        public EffectDatabase.StatusPKEff.DefenseCurl defenseCurl;

        public class Disable
        {
            public string disableMove;
            public int turnsLeft;
            StatusPKData statusData;

            public Disable(
                string disableMove,
                int turnsLeft,
                StatusPKData statusData
                )
            {
                this.disableMove = disableMove;
                this.turnsLeft = turnsLeft;
                this.statusData = statusData;
            }
            public Disable Clone()
            {
                return new Disable(disableMove, turnsLeft, statusData);
            }
        }
        public Disable disable;

        public EffectDatabase.StatusPKEff.Electrify electrify;

        public class Embargo
        {
            public EffectDatabase.StatusPKEff.Embargo effect;
            public int turnsLeft;
            public bool justInitialized;

            public Embargo(
                EffectDatabase.StatusPKEff.Embargo effect,
                int turnsLeft = -1,
                bool justInitialized = true
                )
            {
                this.effect = effect.Clone();
                this.turnsLeft = turnsLeft;
                this.justInitialized = justInitialized;
            }
            public Embargo Clone()
            {
                return new Embargo(
                    effect: effect,
                    turnsLeft: turnsLeft,
                    justInitialized: justInitialized
                    );
            }
        }
        public Embargo embargo;

        public class Encore
        {
            public string encoreMove;
            public int turnsLeft;
            StatusPKData statusData;

            public Encore(
                string encoreMove,
                int turnsLeft,
                StatusPKData statusData
                )
            {
                this.encoreMove = encoreMove;
                this.turnsLeft = turnsLeft;
                this.statusData = statusData;
            }
            public Encore Clone()
            {
                return new Encore(encoreMove, turnsLeft, statusData);
            }
        }
        public Encore encore;

        public EffectDatabase.MoveEff.Endure endure;

        public class FlashFireBoost
        {
            public string moveType;
            public float boost;
            public FlashFireBoost(string moveType = "fire", float boost = 1.5f)
            {
                this.moveType = moveType;
                this.boost = boost;
            }
            public FlashFireBoost Clone()
            {
                return new FlashFireBoost(moveType, boost);
            }
        }
        public List<FlashFireBoost> flashFireBoosts;

        public EffectDatabase.StatusPKEff.Flinch flinch;

        public class ForestsCurse
        {
            public string moveID;
            public string typeID;
            public int turnsLeft;

            public ForestsCurse(string moveID, string typeID, int turnsLeft = -1)
            {
                this.moveID = moveID;
                this.typeID = typeID;
                this.turnsLeft = turnsLeft;
            }
            public static ForestsCurse Clone(ForestsCurse other)
            {
                return new ForestsCurse(other.moveID, other.typeID, other.turnsLeft);
            }
        }
        public List<ForestsCurse> forestsCurses;

        public bool friskIdentified;

        public EffectDatabase.AbilityEff.GulpMissile.Missile gulpMissile;

        public class HealBlock
        {
            public int turnsLeft;
            StatusPKData statusData;

            public HealBlock(
                int turnsLeft,
                StatusPKData statusData
                )
            {
                this.turnsLeft = turnsLeft;
                this.statusData = statusData;
            }
            public HealBlock Clone()
            {
                return new HealBlock(turnsLeft, statusData);
            }
        }
        public HealBlock healBlock;

        public List<EffectDatabase.StatusPKEff.Identification> identifieds;
        
        public class Illusion
        {
            public string pokemonID;
            public string nickname;
            public PokemonGender gender;
            public Illusion(Pokemon pokemon)
            {
                pokemonID = pokemon.pokemonID;
                nickname = pokemon.nickname;
                gender = pokemon.gender;
            }
            public Illusion(string pokemonID, string nickname, PokemonGender gender)
            {
                this.pokemonID = pokemonID;
                this.nickname = nickname;
                this.gender = gender;
            }

            public Illusion Clone()
            {
                Illusion clone = new Illusion(
                    pokemonID: pokemonID,
                    nickname: nickname,
                    gender: gender
                    );
                return clone;
            }
        }
        public Illusion illusion;

        public EffectDatabase.StatusPKEff.Imprison imprison;
        public EffectDatabase.StatusPKEff.Infatuation infatuation;

        public class LockOn
        {
            public string pokemonUniqueID;
            public string moveID;
            public int turnsLeft;

            public LockOn(string pokemonUniqueID, string moveID, int turnsLeft)
            {
                this.pokemonUniqueID = pokemonUniqueID;
                this.moveID = moveID;
                this.turnsLeft = turnsLeft;
            }
            public static LockOn Clone(LockOn other)
            {
                return new LockOn(other.pokemonUniqueID, other.moveID, other.turnsLeft);
            }
        }
        public List<LockOn> lockOnTargets;

        public class MoveLimiter
        {
            public EffectDatabase.StatusPKEff.MoveLimiting effect;
            public int turnsLeft;
            public bool justInitialized;
            public HashSet<string> affectedMoves;
            
            public MoveLimiter(
                EffectDatabase.StatusPKEff.MoveLimiting effect,
                int turnsLeft = -1,
                bool justInitialized = true,
                IEnumerable<string> affectedMoves = null
                )
            {
                this.effect = effect.Clone();
                this.turnsLeft = turnsLeft;
                this.justInitialized = justInitialized;
                this.affectedMoves = (affectedMoves == null) ? new HashSet<string>()
                    : new HashSet<string>(affectedMoves);
            }
            public MoveLimiter Clone()
            {
                return new MoveLimiter(
                    effect: effect,
                    turnsLeft: turnsLeft,
                    justInitialized: justInitialized,
                    affectedMoves: affectedMoves
                    );
            }

            public void SetMove(string move)
            {
                this.SetMoves(new string[] { move });
            }
            public void SetMoves(IEnumerable<string> moves)
            {
                this.affectedMoves = new HashSet<string>(moves);
            }
            public string GetMove()
            {
                List<string> moves = new List<string>(affectedMoves);
                if (moves.Count > 0)
                {
                    return moves[0];
                }
                return null;
            }
        }
        public List<MoveLimiter> moveLimiters;

        public EffectDatabase.StatusPKEff.PerishSong perishSong;
        public EffectDatabase.General.Protect protect;
        public List<EffectDatabase.StatusPKEff.TarShot> tarShots;

        public class Taunt
        {
            public int turnsLeft;
            StatusPKData statusData;

            public Taunt(
                int turnsLeft,
                StatusPKData statusData
                )
            {
                this.turnsLeft = turnsLeft;
                this.statusData = statusData;
            }
            public Taunt Clone()
            {
                return new Taunt(turnsLeft, statusData);
            }
        }
        public Taunt taunt;

        public class Torment
        {
            public string tormentMove;
            public int turnsLeft;
            StatusPKData statusData;

            public Torment(
                string tormentMove,
                int turnsLeft,
                StatusPKData statusData
                )
            {
                this.tormentMove = tormentMove;
                this.turnsLeft = turnsLeft;
                this.statusData = statusData;
            }
            public Torment Clone()
            {
                return new Torment(tormentMove, turnsLeft, statusData);
            }
        }
        public Torment torment;

        public TransformProperties tProps;
        public int truantTurns = 0;

        public string unburdenItem;

        public EffectDatabase.StatusPKEff.Yawn yawn;

        public int turnsActive;

        public int ATKStage;
        public int DEFStage;
        public int SPAStage;
        public int SPDStage;
        public int SPEStage;
        public int ACCStage;
        public int EVAStage;

        public bool ATKRaised;
        public bool ATKLowered;
        public bool DEFRaised;
        public bool DEFLowered;
        public bool SPARaised;
        public bool SPALowered;
        public bool SPDRaised;
        public bool SPDLowered;
        public bool SPERaised;
        public bool SPELowered;
        public bool ACCRaised;
        public bool ACCLowered;
        public bool EVARaised;
        public bool EVALowered;

        public int ATKOverride;
        public int DEFOverride;
        public int SPAOverride;
        public int SPDOverride;
        public int SPEOverride;

        public PokemonStats ATKMappedStat;
        public PokemonStats DEFMappedStat;
        public PokemonStats SPAMappedStat;
        public PokemonStats SPDMappedStat;
        public PokemonStats SPEMappedStat;

        public List<string> types;

        public List<StatusCondition> statusConditions;

        public PokemonSavedCommand nextCommand;

        public bool wasStruckForDamage;
        public bool wasHitByOpponent;
        public bool wasHitByAlly;
        public string lastMove;
        public string lastMoveHitBy;
        public string lastMoveTargetedBy;
        public int lastMoveTargetedTurns;
        public string lastTargeterPokemon;
        public BattlePosition lastTargeterPosition;
        public BattlePosition lastDamagerPosition;

        public bool actedThisTurn;
        public bool switchedIn;
        public bool isSwitchingOut;

        public int turnPhysicalDamageTaken;
        public int turnSpecialDamageTaken;
        public int turnTotalDamageTaken
        {
            get
            {
                return turnPhysicalDamageTaken + turnSpecialDamageTaken;
            }
        }
        public string choiceMove;

        public float preHPPercent;

        public string consumedItem;
        public string consumedBerry;

        public string lostItem;

        public bool isAbilitySuppressed;

        public int bideDamageTaken;
        public int bideTurnsLeft;

        public int rechargeTurns;

        public string thrashMove;
        public int thrashTurns;

        public string uproarMove;
        public int uproarTurns;

        public bool inDigState;
        public bool inDiveState;
        public bool inFlyState;
        public bool inShadowForceState;

        public bool attemptingToSkyDrop;
        public string skyDropMove;
        public string skyDropUser;
        public List<string> skyDropTargets;

        public bool isMagicCoatActive;
        public string magicCoatMove;
        public bool isMinimizeActive;
        public string snatchMove;

        public bool isSmackedDown;

        public string leechSeedMove;
        public BattlePosition leechSeedPosition;

        public string substituteMove;
        public int substituteHP;

        public string blockMove;
        public string blockPokemon;

        public string bindMove;
        public string bindPokemon;
        public int bindTurns;

        public string rageMove;
        public int rageCounter;

        public string confusionMove;
        public int confusionTurns;

        public List<string> ingrainMoves;

        public string mimicBaseMove;
        public Moveslot mimicMoveslot;
        public string sketchBaseMove;
        public Moveslot sketchMoveslot;

        public bool usingMeFirst;

        public string roostMove;


        public string endureMove;
        public string protectMove;
        public int protectCounter;

        public string beakBlastMove;
        public string focusPunchMove;
        public string shellTrapMove;

        public bool isFlinching;

        public string powderMove;

        public string destinyBondMove;

        public bool isCenterOfAttention;

        public List<string> helpingHandMoves;

        // Constructor
        public BattleProperties(Pokemon pokemon)
        {
            Reset(pokemon);

            // Non-resettable properties
            consumedItem = null;
            consumedBerry = null;
            lostItem = null;

            sketchBaseMove = null;
            sketchMoveslot = null;
        }

        // Clone
        public static BattleProperties Clone(BattleProperties original, Pokemon pokemon)
        {
            BattleProperties cloneProps = new BattleProperties(pokemon);

            cloneProps.abilities = new List<Ability>();
            for (int i = 0; i < original.abilities.Count; i++)
            {
                cloneProps.abilities.Add(original.abilities[i].Clone());
            }

            cloneProps.bound = (original.bound == null) ? null : original.bound.Clone();
            cloneProps.defenseCurl = (original.defenseCurl == null) ? null : original.defenseCurl.Clone();
            cloneProps.electrify = (original.electrify == null) ? null : original.electrify.Clone();
            cloneProps.embargo = (original.embargo == null) ? null : original.embargo.Clone();
            cloneProps.endure = (original.endure == null) ? null : original.endure.Clone();
            for (int i = 0; i < original.flashFireBoosts.Count; i++)
            {
                cloneProps.flashFireBoosts.Add(original.flashFireBoosts[i].Clone());
            }
            cloneProps.flinch = (original.flinch == null) ? null : original.flinch.Clone();

            for (int i = 0; i < original.forestsCurses.Count; i++)
            {
                cloneProps.forestsCurses.Add(ForestsCurse.Clone(original.forestsCurses[i]));
            }
            cloneProps.friskIdentified = original.friskIdentified;
            cloneProps.gulpMissile = (original.gulpMissile == null) ? null : original.gulpMissile.Clone();
            for (int i = 0; i < original.identifieds.Count; i++)
            {
                cloneProps.identifieds.Add(original.identifieds[i].Clone());
            }
            cloneProps.illusion = (original.illusion == null) ? null : original.illusion.Clone();
            cloneProps.imprison = (original.imprison == null) ? null : original.imprison.Clone();
            cloneProps.infatuation = (original.infatuation == null) ? null : original.infatuation.Clone();
            for (int i = 0; i < original.lockOnTargets.Count; i++)
            {
                cloneProps.lockOnTargets.Add(LockOn.Clone(original.lockOnTargets[i]));
            }
            for (int i = 0; i < original.moveLimiters.Count; i++)
            {
                cloneProps.moveLimiters.Add(original.moveLimiters[i].Clone());
            }
            cloneProps.perishSong = (original.perishSong == null) ? null : original.perishSong.Clone();
            for (int i = 0; i < original.tarShots.Count; i++)
            {
                cloneProps.tarShots.Add(original.tarShots[i].Clone());
            }
            cloneProps.protect = (original.protect == null) ? null : original.protect.Clone();
            cloneProps.tProps = (original.tProps == null) ? null
                : TransformProperties.Clone(original.tProps);
            cloneProps.yawn = (original.yawn == null) ? null : original.yawn.Clone();

            cloneProps.turnsActive = original.turnsActive;

            cloneProps.ATKStage = original.ATKStage;
            cloneProps.DEFStage = original.DEFStage;
            cloneProps.SPAStage = original.SPAStage;
            cloneProps.SPDStage = original.SPDStage;
            cloneProps.SPEStage = original.SPEStage;
            cloneProps.ACCStage = original.ACCStage;
            cloneProps.EVAStage = original.EVAStage;

            cloneProps.ATKRaised = original.ATKRaised;
            cloneProps.ATKLowered = original.ATKLowered;
            cloneProps.DEFRaised = original.DEFRaised;
            cloneProps.DEFLowered = original.DEFLowered;
            cloneProps.SPARaised = original.SPARaised;
            cloneProps.SPALowered = original.SPALowered;
            cloneProps.SPDRaised = original.SPDRaised;
            cloneProps.SPDLowered = original.SPDLowered;
            cloneProps.SPERaised = original.SPERaised;
            cloneProps.SPELowered = original.SPELowered;
            cloneProps.ACCRaised = original.ACCRaised;
            cloneProps.ACCLowered = original.ACCLowered;
            cloneProps.EVARaised = original.EVARaised;
            cloneProps.EVALowered = original.EVALowered;

            cloneProps.ATKOverride = original.ATKOverride;
            cloneProps.DEFOverride = original.DEFOverride;
            cloneProps.SPAOverride = original.SPAOverride;
            cloneProps.SPDOverride = original.SPDOverride;
            cloneProps.SPEOverride = original.SPEOverride;

            cloneProps.ATKMappedStat = original.ATKMappedStat;
            cloneProps.DEFMappedStat = original.DEFMappedStat;
            cloneProps.SPAMappedStat = original.SPAMappedStat;
            cloneProps.SPDMappedStat = original.SPDMappedStat;
            cloneProps.SPEMappedStat = original.SPEMappedStat;

            cloneProps.types = new List<string>(original.types);

            cloneProps.nextCommand = (original.nextCommand == null) ? null
                : PokemonSavedCommand.Clone(original.nextCommand);

            cloneProps.inDigState = original.inDigState;
            cloneProps.inDiveState = original.inDiveState;
            cloneProps.inFlyState = original.inFlyState;
            cloneProps.inShadowForceState = original.inShadowForceState;

            cloneProps.attemptingToSkyDrop = original.attemptingToSkyDrop;
            cloneProps.skyDropMove = original.skyDropMove;
            cloneProps.skyDropUser = original.skyDropUser;
            cloneProps.skyDropTargets = new List<string>(original.skyDropTargets);

            cloneProps.isMagicCoatActive = original.isMagicCoatActive;
            cloneProps.magicCoatMove = original.magicCoatMove;

            cloneProps.isMinimizeActive = original.isMinimizeActive;

            cloneProps.thrashMove = original.thrashMove;
            cloneProps.thrashTurns = original.thrashTurns;

            cloneProps.uproarMove = original.uproarMove;
            cloneProps.uproarTurns = original.uproarTurns;

            cloneProps.snatchMove = original.snatchMove;

            cloneProps.isSmackedDown = original.isSmackedDown;

            cloneProps.rechargeTurns = original.rechargeTurns;

            cloneProps.wasStruckForDamage = original.wasStruckForDamage;
            cloneProps.wasHitByAlly = original.wasHitByAlly;
            cloneProps.wasHitByOpponent = original.wasHitByOpponent;
            cloneProps.lastMove = original.lastMove;
            cloneProps.lastMoveHitBy = original.lastMoveHitBy;
            cloneProps.lastMoveTargetedBy = original.lastMoveTargetedBy;
            cloneProps.lastMoveTargetedTurns = original.lastMoveTargetedTurns;
            cloneProps.lastTargeterPokemon = original.lastTargeterPokemon;
            cloneProps.lastDamagerPosition = original.lastDamagerPosition;
            cloneProps.lastTargeterPosition = BattlePosition.Clone(original.lastTargeterPosition);

            cloneProps.actedThisTurn = original.actedThisTurn;
            cloneProps.switchedIn = original.switchedIn;
            cloneProps.isSwitchingOut = original.isSwitchingOut;

            cloneProps.preHPPercent = original.preHPPercent;
            cloneProps.turnPhysicalDamageTaken = original.turnPhysicalDamageTaken;
            cloneProps.turnSpecialDamageTaken = original.turnSpecialDamageTaken;
            cloneProps.choiceMove = original.choiceMove;

            cloneProps.consumedItem = original.consumedItem;
            cloneProps.consumedBerry = original.consumedBerry;
            cloneProps.lostItem = original.lostItem;

            cloneProps.isAbilitySuppressed = original.isAbilitySuppressed;

            cloneProps.bideDamageTaken = original.bideDamageTaken;
            cloneProps.bideTurnsLeft = original.bideTurnsLeft;

            cloneProps.leechSeedMove = original.leechSeedMove;
            cloneProps.leechSeedPosition = BattlePosition.Clone(original.leechSeedPosition);

            cloneProps.substituteMove = original.substituteMove;
            cloneProps.substituteHP = original.substituteHP;

            cloneProps.blockMove = original.blockMove;
            cloneProps.blockPokemon = original.blockPokemon;

            cloneProps.bindMove = original.bindMove;
            cloneProps.bindPokemon = original.bindPokemon;
            cloneProps.bindTurns = original.bindTurns;

            cloneProps.rageMove = original.rageMove;
            cloneProps.rageCounter = original.rageCounter;

            cloneProps.confusionMove = original.confusionMove;
            cloneProps.confusionTurns = original.confusionTurns;

            cloneProps.ingrainMoves = new List<string>(original.ingrainMoves);

            cloneProps.mimicBaseMove = original.mimicBaseMove;
            cloneProps.mimicMoveslot = (original.mimicMoveslot == null) ? null
                : Moveslot.Clone(original.mimicMoveslot);
            cloneProps.sketchBaseMove = original.sketchBaseMove;
            cloneProps.sketchMoveslot = (original.sketchMoveslot == null) ? null
                : Moveslot.Clone(original.sketchMoveslot);

            cloneProps.usingMeFirst = original.usingMeFirst;

            cloneProps.roostMove = original.roostMove;

            cloneProps.endureMove = original.endureMove;
            cloneProps.protectMove = original.protectMove;
            cloneProps.protectCounter = original.protectCounter;

            cloneProps.beakBlastMove = original.beakBlastMove;
            cloneProps.focusPunchMove = original.focusPunchMove;
            cloneProps.shellTrapMove = original.shellTrapMove;

            cloneProps.isFlinching = original.isFlinching;

            cloneProps.powderMove = original.powderMove;

            cloneProps.destinyBondMove = original.destinyBondMove;

            cloneProps.isCenterOfAttention = original.isCenterOfAttention;

            cloneProps.helpingHandMoves = new List<string>(original.helpingHandMoves);

            if (original.statusConditions != null)
            {
                for (int i = 0; i < original.statusConditions.Count; i++)
                {
                    cloneProps.statusConditions.Add(StatusCondition.Clone(original.statusConditions[i]));
                }
            }

            return cloneProps;
        }

        public void Reset(Pokemon pokemon)
        {
            bound = null;
            defenseCurl = null;
            electrify = null;
            embargo = null;
            endure = null;
            flashFireBoosts = new List<FlashFireBoost>();
            flinch = null;
            forestsCurses = new List<ForestsCurse>();
            friskIdentified = false;
            gulpMissile = null;
            identifieds = new List<EffectDatabase.StatusPKEff.Identification>();
            illusion = null;
            imprison = null;
            infatuation = null;
            lockOnTargets = new List<LockOn>();
            moveLimiters = new List<MoveLimiter>();
            perishSong = null;
            protect = null;
            tarShots = new List<EffectDatabase.StatusPKEff.TarShot>();
            tProps = null;
            yawn = null;

            turnsActive = 0;

            ATKStage = 0;
            DEFStage = 0;
            SPAStage = 0;
            SPDStage = 0;
            SPEStage = 0;
            ACCStage = 0;
            EVAStage = 0;

            ATKRaised = false;
            ATKLowered = false;
            DEFRaised = false;
            DEFLowered = false;
            SPARaised = false;
            SPALowered = false;
            SPDRaised = false;
            SPDLowered = false;
            SPERaised = false;
            SPELowered = false;
            ACCRaised = false;
            ACCLowered = false;
            EVARaised = false;
            EVALowered = false;

            ResetOverrides(pokemon);

            nextCommand = null;
            rechargeTurns = 0;

            wasStruckForDamage = false;
            wasHitByOpponent = false;
            wasHitByAlly = false;
            lastMove = null;
            lastMoveHitBy = null;
            lastMoveTargetedBy = null;
            lastMoveTargetedTurns = 0;
            lastTargeterPokemon = null;
            lastTargeterPosition = null;
            lastDamagerPosition = null;

            actedThisTurn = false;
            switchedIn = false;
            isSwitchingOut = false;

            isAbilitySuppressed = false;

            preHPPercent = pokemon.HPPercent;
            turnPhysicalDamageTaken = 0;
            turnSpecialDamageTaken = 0;
            choiceMove = null;

            bideDamageTaken = 0;
            bideTurnsLeft = -1;

            isSmackedDown = false;

            thrashMove = null;
            thrashTurns = -1;

            uproarMove = null;
            uproarTurns = -1;

            inDigState = false;
            inDiveState = false;
            inFlyState = false;
            inShadowForceState = false;

            attemptingToSkyDrop = false;
            skyDropMove = null;
            skyDropUser = null;
            skyDropTargets = new List<string>();

            isMagicCoatActive = false;
            magicCoatMove = null;
            isMinimizeActive = false;
            snatchMove = null;

            leechSeedMove = null;
            leechSeedPosition = null;

            substituteMove = null;
            substituteHP = 0;

            blockMove = null;
            blockPokemon = null;

            bindMove = null;
            bindPokemon = null;
            bindTurns = 0;

            rageMove = null;
            rageCounter = 0;

            confusionMove = null;
            confusionTurns = 0;

            ingrainMoves = new List<string>();

            mimicBaseMove = null;
            mimicMoveslot = null;

            usingMeFirst = false;

            roostMove = null;

            endureMove = null;
            protectMove = null;
            protectCounter = 0;

            beakBlastMove = null;
            focusPunchMove = null;
            shellTrapMove = null;

            isFlinching = false;

            powderMove = null;

            destinyBondMove = null;

            isCenterOfAttention = false;

            helpingHandMoves = new List<string>();

            statusConditions = new List<StatusCondition>();
        }

        public void BatonPassFrom(BattleProperties other)
        {
            ATKStage = other.ATKStage;
            DEFStage = other.DEFStage;
            SPAStage = other.SPAStage;
            SPDStage = other.SPDStage;
            SPEStage = other.SPEStage;
            ACCStage = other.ACCStage;
            EVAStage = other.EVAStage;

            ATKMappedStat = other.ATKMappedStat;
            DEFMappedStat = other.DEFMappedStat;
            SPAMappedStat = other.SPAMappedStat;
            SPDMappedStat = other.SPDMappedStat;
            SPEMappedStat = other.SPEMappedStat;

            confusionMove = other.confusionMove;
            confusionTurns = other.confusionTurns;

            leechSeedMove = other.leechSeedMove;
            leechSeedPosition = BattlePosition.Clone(other.leechSeedPosition);

            lockOnTargets = new List<LockOn>(other.lockOnTargets);

            ingrainMoves = new List<string>(other.ingrainMoves);

            perishSong = (other.perishSong == null) ? null : other.perishSong.Clone();
            substituteMove = other.substituteMove;
            substituteHP = other.substituteHP;

        }
        public void ResetOverrides(Pokemon pokemon)
        {
            abilities = new List<Ability>();
            if (!string.IsNullOrEmpty(pokemon.GetAbility()))
            {
                abilities.Add(new Ability(pokemon.GetAbility()));
            }

            tProps = null;

            ATKOverride = -1;
            DEFOverride = -1;
            SPAOverride = -1;
            SPDOverride = -1;
            SPEOverride = -1;

            ATKMappedStat = PokemonStats.Attack;
            DEFMappedStat = PokemonStats.Defense;
            SPAMappedStat = PokemonStats.SpecialAttack;
            SPDMappedStat = PokemonStats.SpecialDefense;
            SPEMappedStat = PokemonStats.Speed;

            types = new List<string>(pokemon.data.types);
        }

        public FlashFireBoost GetFlashFireBoost(string moveType)
        {
            for (int i = 0; i < flashFireBoosts.Count; i++)
            {
                if (flashFireBoosts[i].moveType == moveType)
                {
                    return flashFireBoosts[i];
                }
            }
            return null;
        }
    }
    public class DynamaxProperties
    {
        public int dynamaxLevel;
        public string GMaxForm;
        public string GMaxMove;
        public string moveType;
        public int turnsLeft;

        public DynamaxProperties(
            int dynamaxLevel = 0,
            string GMaxForm = null,
            string GMaxMove = null,
            string moveType = null,
            int turnsLeft = 0
            )
        {
            this.dynamaxLevel = dynamaxLevel;
            this.GMaxForm = GMaxForm;
            this.moveType = moveType;
            this.GMaxMove = GMaxMove;
            this.turnsLeft = turnsLeft;
        }
        public DynamaxProperties Clone()
        {
            return new DynamaxProperties(
                dynamaxLevel: dynamaxLevel,
                GMaxForm: GMaxForm,
                moveType: moveType,
                GMaxMove: GMaxMove,
                turnsLeft: turnsLeft
                );
        }
    }
    public class TransformProperties
    {
        public string pokemonID;
        public Moveslot[] moveslots;

        public int ATK;
        public int DEF;
        public int SPA;
        public int SPD;
        public int SPE;

        // Constructor
        private TransformProperties()
        {
            moveslots = new Moveslot[4];
        }
        public TransformProperties(Pokemon pokemon)
        {
            pokemonID = pokemon.pokemonID;

            moveslots = new Moveslot[4];
            for (int i = 0; i < pokemon.moveslots.Length; i++)
            {
                Moveslot slot = pokemon.moveslots[i];
                if (slot != null)
                {
                    moveslots[i] = new Moveslot(moveID: slot.moveID, p_maxPP: Mathf.Min(slot.maxPP, 5));
                }
            }

            ATK = pokemon.ATK;
            DEF = pokemon.DEF;
            SPA = pokemon.SPA;
            SPD = pokemon.SPD;
            SPE = pokemon.SPE;
        }

        // Clone
        public static TransformProperties Clone(TransformProperties original)
        {
            TransformProperties clone = new TransformProperties();

            clone.moveslots = new Moveslot[4];
            for (int i = 0; i < original.moveslots.Length; i++)
            {
                Moveslot slot = original.moveslots[i];
                if (slot != null)
                {
                    clone.moveslots[i] = Moveslot.Clone(original.moveslots[i]);
                }
            }

            clone.ATK = original.ATK;
            clone.DEF = original.DEF;
            clone.SPA = original.SPA;
            clone.SPD = original.SPD;
            clone.SPE = original.SPE;

            return clone;
        }
    }

    public enum MegaState
    {
        None,
        Mega,
        Primal
    }
    public enum DynamaxState
    {
        None,
        Dynamax,
        Gigantamax
    }

    // General
    public string uniqueID;
    public string uniqueIDTrunc { get
        {
            return uniqueID.Substring(0, 8);
        }
    }
    public string basePokemonID;
    public string pokemonID;
    private string p_nickname;
    public string nickname {
        get {
            if (string.IsNullOrEmpty(p_nickname))
            {
                return PokemonDatabase.instance.GetPokemonData(pokemonID).speciesName;
            }
            return p_nickname;
        }
        set
        {
            p_nickname = value;
        }
    }
    public int level;
    public string natureID;
    public PokemonGender gender;
    public PokemonData data
    {
        get
        {
            return PokemonDatabase.instance.GetPokemonData(pokemonID);
        }
    }

    // Stats
    public int ivHP;
    public int ivATK;
    public int ivDEF;
    public int ivSPA;
    public int ivSPD;
    public int ivSPE;

    public int evHP;
    public int evATK;
    public int evDEF;
    public int evSPA;
    public int evSPD;
    public int evSPE;

    public int currentHP;
    public int maxHP {
        get
        {
            PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemonID);

            float numerator = ((2 * pokemonData.baseHP) + ivHP + (evHP / 4)) * level;
            float result = ((numerator / 100) + level + 10) * NatureDatabase.instance.GetNatureData(natureID).HPMod;

            if (dynamaxState != DynamaxState.None)
            {
                result = result * (1.5f + (dynamaxProps.dynamaxLevel * GameSettings.pkmnDynamaxHPLevelBoost));
            }
            return Mathf.FloorToInt(result);
        }
    }
    public int ATK
    {
        get
        {
            if (bProps.ATKOverride > 0)
            {
                return bProps.ATKOverride;
            }
            if (bProps.tProps != null)
            {
                return bProps.tProps.ATK;
            }

            float numerator = ((2 * data.baseATK) + ivATK + (evATK / 4)) * level;
            float result = ((numerator / 100) + 5) * NatureDatabase.instance.GetNatureData(natureID).ATKMod;
            return Mathf.FloorToInt(result);
        }
    }
    public int DEF
    {
        get
        {
            if (bProps.DEFOverride > 0)
            {
                return bProps.DEFOverride;
            }
            if (bProps.tProps != null)
            {
                return bProps.tProps.DEF;
            }

            float numerator = ((2 * data.baseDEF) + ivDEF + (evDEF / 4)) * level;
            float result = ((numerator / 100) + 5) * NatureDatabase.instance.GetNatureData(natureID).DEFMod;
            return Mathf.FloorToInt(result);
        }
    }
    public int SPA
    {
        get
        {
            if (bProps.SPAOverride > 0)
            {
                return bProps.SPAOverride;
            }
            if (bProps.tProps != null)
            {
                return bProps.tProps.SPA;
            }

            float numerator = ((2 * data.baseSPA) + ivSPA + (evSPA / 4)) * level;
            float result = ((numerator / 100) + 5) * NatureDatabase.instance.GetNatureData(natureID).SPAMod;
            return Mathf.FloorToInt(result);
        }
    }
    public int SPD
    {
        get
        {
            if (bProps.SPDOverride > 0)
            {
                return bProps.SPDOverride;
            }
            if (bProps.tProps != null)
            {
                return bProps.tProps.SPD;
            }

            float numerator = ((2 * data.baseSPD) + ivSPD + (evSPD / 4)) * level;
            float result = ((numerator / 100) + 5) * NatureDatabase.instance.GetNatureData(natureID).SPDMod;
            return Mathf.FloorToInt(result);
        }
    }
    public int SPE
    {
        get
        {
            if (bProps.SPEOverride > 0)
            {
                return bProps.SPEOverride;
            }
            if (bProps.tProps != null)
            {
                return bProps.tProps.SPE;
            }

            float numerator = ((2 * data.baseSPE) + ivSPE + (evSPE / 4)) * level;
            float result = ((numerator / 100) + 5) * NatureDatabase.instance.GetNatureData(natureID).SPEMod;
            return Mathf.FloorToInt(result);
        }
    }

    public int HPdifference {
        get
        {
            return maxHP - currentHP;
        }
    }
    public float HPPercent { 
        get 
        {
            return (float)currentHP / maxHP;
        } 
    }

    // Moves
    public Moveslot[] moveslots;

    // Ability
    public int abilityNo;
    public bool isHiddenAbility;

    // Status
    public StatusCondition nonVolatileStatus;

    // Items
    public Item item { 
        get 
        {
            return (items.Count > 0) ? items[0] : null;
        } 
    }
    public List<Item> items;

    // Mega Properties
    public MegaState megaState;

    // Dynamax Properties
    public DynamaxState dynamaxState;
    public DynamaxProperties dynamaxProps;
    public void ResetDynamaxProps()
    {
        dynamaxState = DynamaxState.None;
        dynamaxProps.turnsLeft = 0;
    }

    // Battle-only Properties
    public bool isInBattleMode;
    public int teamPos;
    public int battlePos;
    public int faintPos;
    public BattleProperties bProps;

    // Constructor
    public Pokemon(
        string pokemonID,
        string basePokemonID = null,
        string uniqueID = "",
        string nickname = null,
        int level = 1,
        string natureID = null,
        string genderID = null,
        float hpPercent = -1f,
        int currentHP = -1,
        int ivHP = -1, int ivATK = -1, int ivDEF = -1, int ivSPA = -1, int ivSPD = -1, int ivSPE = -1,
        int evHP = 0, int evATK = 0, int evDEF = 0, int evSPA = 0, int evSPD = 0, int evSPE = 0,
        Moveslot[] moveslots = null,
        int abilityNo = -1, bool isHiddenAbility = false,
        StatusCondition nonVolatileStatus = null,
        Item item = null,
        MegaState megaState = MegaState.None,
        DynamaxState dynamaxState = DynamaxState.None,
        DynamaxProperties dynamaxProps = null,
        bool checkForm = false
        )
    {
        // general
        if (string.IsNullOrEmpty(uniqueID))
        {
            this.uniqueID = System.Guid.NewGuid().ToString();
        }
        else
        {
            this.uniqueID = uniqueID;
        }
        this.pokemonID = pokemonID;
        this.basePokemonID = string.IsNullOrEmpty(basePokemonID) ? pokemonID : basePokemonID;
        this.nickname = nickname;
        this.level = Mathf.Max(1, level);

        if (natureID == null)
        {
            this.natureID = NatureDatabase.instance.GetRandomNature().ID;
        }
        else
        {
            this.natureID = natureID;
        }

        if (string.IsNullOrEmpty(genderID))
        {
            gender = data.GetRandomGender();
        }
        else
        {
            gender = GameTextDatabase.ConvertToGender(genderID);
        }

        // stats
        this.ivHP = ivHP < 0 ? GameSettings.pkmnMaxIVValue : ivHP;
        this.ivATK = ivATK < 0 ? GameSettings.pkmnMaxIVValue : ivATK;
        this.ivDEF = ivDEF < 0 ? GameSettings.pkmnMaxIVValue : ivDEF;
        this.ivSPA = ivSPA < 0 ? GameSettings.pkmnMaxIVValue : ivSPA;
        this.ivSPD = ivSPD < 0 ? GameSettings.pkmnMaxIVValue : ivSPD;
        this.ivSPE = ivSPE < 0 ? GameSettings.pkmnMaxIVValue : ivSPE;

        this.evHP = evHP;
        this.evATK = evATK;
        this.evDEF = evDEF;
        this.evSPA = evSPA;
        this.evSPD = evSPD;
        this.evSPE = evSPE;

        if (currentHP < 0)
        {
            this.currentHP = this.maxHP;
        }
        else
        {
            this.currentHP = currentHP;
        }

        if (hpPercent >= 0)
        {
            this.currentHP = GetHPByPercent(hpPercent);
        }

        this.moveslots = new Moveslot[4];
        if (moveslots != null) {
            for (int i = 0; i < 4; i++)
            {
                if (moveslots.Length > i)
                {
                    if (moveslots[i] != null)
                    {
                        this.moveslots[i] = Moveslot.Clone(moveslots[i]);
                    }
                }
            }
        }

        this.isHiddenAbility = isHiddenAbility;
        this.abilityNo = abilityNo;
        if (abilityNo < 0)
        {
            this.abilityNo = data.GetRandomAbilityIndex(this.isHiddenAbility);
        }

        this.nonVolatileStatus = (nonVolatileStatus == null) ? new StatusCondition("healthy")
            : StatusCondition.Clone(nonVolatileStatus);

        this.items = new List<Item>();
        if (item != null)
        {
            this.items.Add(item.Clone());
        }

        // battle
        isInBattleMode = false;
        faintPos = -1;
        battlePos = -1;
        teamPos = -1;
        bProps = new BattleProperties(this);

        // mega / dynamax
        this.megaState = megaState;
        this.dynamaxState = dynamaxState;
        this.dynamaxProps = (dynamaxProps == null) ? new DynamaxProperties() : dynamaxProps.Clone();
        
        if (checkForm)
        {
            CheckForm();
        }
    }

    // Clone
    public static Pokemon Clone(Pokemon original)
    {
        Pokemon clonePokemon = new Pokemon(
            pokemonID: original.pokemonID,
            basePokemonID: original.pokemonID,
            uniqueID: original.uniqueID,
            nickname: original.nickname,
            level: original.level,
            natureID: original.natureID,
            genderID: GameTextDatabase.ConvertGenderToString(original.gender),

            currentHP: original.currentHP,
            ivHP: original.ivHP, ivATK: original.ivATK, ivDEF: original.ivDEF,
            ivSPA: original.ivSPA, ivSPD: original.ivSPD, ivSPE: original.ivSPE,

            moveslots: original.moveslots,
            abilityNo: original.abilityNo, isHiddenAbility: original.isHiddenAbility,
            
            nonVolatileStatus: original.nonVolatileStatus,

            item: original.item,
            megaState: original.megaState,
            dynamaxState: original.dynamaxState,
            dynamaxProps: original.dynamaxProps,
            checkForm: false
            );

        // Battle-Only Properties
        clonePokemon.isInBattleMode = original.isInBattleMode;
        clonePokemon.battlePos = original.battlePos;
        clonePokemon.teamPos = original.teamPos;
        clonePokemon.faintPos = original.faintPos;
        clonePokemon.bProps = BattleProperties.Clone(original.bProps, clonePokemon);

        return clonePokemon;
    }

    // Forms
    public void CheckForm()
    {
        AbilityData abilityData = AbilityDatabase.instance.GetAbilityData(GetAbility());
        // Items
        if (item != null)
        {
            List<EffectDatabase.ItemEff.ItemEffect> griseousOrbs_ = item.data.GetEffectsNew(ItemEffectType.GriseousOrb);
            for (int i = 0; i < griseousOrbs_.Count; i++)
            {
                EffectDatabase.ItemEff.GriseousOrb griseousOrb = griseousOrbs_[i] as EffectDatabase.ItemEff.GriseousOrb;
                PokemonData basePokemonData = PokemonDatabase.instance.GetPokemonData(griseousOrb.baseFormID);
                PokemonData toPokemonData = PokemonDatabase.instance.GetPokemonData(griseousOrb.formID);

                // Validate if the pokemon is contained
                if (pokemonID == basePokemonData.ID
                    || pokemonID == toPokemonData.ID
                    || data.IsABaseID(basePokemonData.ID))
                {
                    bool canTransform = true;

                    // Arceus Plate = Multitype Check
                    if (griseousOrb is EffectDatabase.ItemEff.ArceusPlate)
                    {
                        if (abilityData.GetEffectNew(AbilityEffectType.Multitype) == null)
                        {
                            canTransform = false;
                        }
                    }

                    // RKS Memory = RKS System Check
                    if (griseousOrb is EffectDatabase.ItemEff.RKSMemory)
                    {
                        if (abilityData.GetEffectNew(AbilityEffectType.RKSSystem) != null)
                        {
                            canTransform = false;
                        }
                    }

                    // Mega Stone Requires in-battle activation (not automatic)
                    if (griseousOrb is EffectDatabase.ItemEff.MegaStone)
                    {
                        canTransform = false;
                    }

                    if (canTransform)
                    {
                        if (pokemonID != toPokemonData.ID)
                        {
                            basePokemonID = toPokemonData.ID;
                            pokemonID = toPokemonData.ID;
                            bProps.ResetOverrides(this);
                        }
                    }
                }
            }
        }

    }

    // Characteristics
    public static bool IsGenderOpposite(PokemonGender gender1, PokemonGender gender2)
    {
        return (gender1 == PokemonGender.Male && gender2 == PokemonGender.Female)
            || (gender1 == PokemonGender.Female && gender2 == PokemonGender.Male);
    }

    // Battle
    public void DisruptBide()
    {
        bProps.bideDamageTaken = 0;
        bProps.bideTurnsLeft = -1;
    }

    // General Methods
    public bool IsTheSameAs(Pokemon other)
    {
        if (other == null) return false;
        return uniqueID == other.uniqueID;
    }
    public bool IsTheSameAs(string uniqueID)
    {
        return this.uniqueID == uniqueID;
    }

    // Health Methods
    public bool IsAbleToBattle()
    {
        return currentHP > 0;
    }
    public void RestoreEverything()
    {
        this.currentHP = this.maxHP;
        for (int i = 0; i < this.moveslots.Length; i++)
        {
            this.moveslots[i].PP = this.moveslots[i].maxPP;
        }
    }
    public int GetHPByPercent(float percent)
    {
        if (percent >= 1f)
        {
            return maxHP;
        }
        else if (percent <= 0)
        {
            return 0;
        }
        return Mathf.RoundToInt(maxHP * percent);
    }

    // Stats Methods
    public string GetStatSpreadString()
    {
        return maxHP + "/" + ATK + "/" + DEF + "/" + SPA + "/" + SPD + "/" + SPE;
    }

    // Moveset Methods
    public int GetNumberOfMoves()
    {
        int count = 0;
        for (int i = 0; i < moveslots.Length; i++)
        {
            if (moveslots[i] != null)
            {
                count++;
            }
        }
        return count;
    }
    public List<Moveslot> GetMoveset()
    {
        List<Moveslot> moveset = new List<Moveslot>();
        for (int i = 0; i < moveslots.Length; i++)
        {
            if (moveslots[i] != null)
            {
                moveset.Add(moveslots[i]);
            }
        }
        return moveset;
    }
    public int ConsumePP(string moveID, int PPLost)
    {
        for (int i = 0; i < moveslots.Length; i++)
        {
            if (moveslots[i] != null)
            {
                if (moveslots[i].moveID == moveID)
                {
                    return ConsumePPFromSlot(moveslots[i], PPLost);
                }
            }
        }
        return 0;
    }
    public int ConsumePPFromSlot(Moveslot moveslot, int PPLost)
    {
        int realPPLost = PPLost;
        if (PPLost >= moveslot.PP)
        {
            realPPLost = moveslot.PP;
            moveslot.PP = 0;
        }
        else
        {
            moveslot.PP -= PPLost;
        }
        return realPPLost;
    }
    public bool HasMove(string moveID)
    {
        for (int i = 0; i < moveslots.Length; i++)
        {
            if (moveslots[i] != null)
            {
                if (moveslots[i].moveID == moveID)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Ability Methods
    public string GetAbility()
    {
        return data.GetAbilityAtIndex(abilityNo, isHiddenAbility);
    }

    // Status Condition Methods
    public void ApplyStatusCondition(StatusCondition condition)
    {
        if (condition.data.HasTag(PokemonSTag.NonVolatile))
        {
            SetNonVolatileStatus(condition);
        }
        else
        {
            AddStatusCondition(condition);
        }
    }
    public void SetNonVolatileStatus(StatusCondition condition)
    {
        nonVolatileStatus = condition;
    }
    public void UnsetNonVolatileStatus()
    {
        nonVolatileStatus = new StatusCondition("healthy");
    }
    public bool AddStatusCondition(StatusCondition condition)
    {
        if (!HasStatusCondition(condition.statusID))
        {
            bProps.statusConditions.Add(condition);
            return true;
        }
        return false;
    }
    public bool HasStatusCondition(string statusID)
    {
        if (nonVolatileStatus.statusID == statusID)
        {
            return true;
        }
        for (int i = 0; i < bProps.statusConditions.Count; i++)
        {
            if (bProps.statusConditions[i].statusID == statusID)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveStatusCondition(string statusID)
    {
        List<StatusCondition> newStatusConditions = new List<StatusCondition>();
        for (int i = 0; i < bProps.statusConditions.Count; i++)
        {
            if (bProps.statusConditions[i].statusID != statusID)
            {
                newStatusConditions.Add(bProps.statusConditions[i]);
            }
        }
        bProps.statusConditions.Clear();
        bProps.statusConditions.AddRange(newStatusConditions);
    }
    public void UnsetVolatileStatuses()
    {
        bProps.statusConditions.Clear();
    }
    public List<StatusCondition> GetAllStatusConditions()
    {
        List<StatusCondition> conditions = new List<StatusCondition>();
        conditions.Add(nonVolatileStatus);
        conditions.AddRange(bProps.statusConditions);
        return conditions;
    }

    // Item Methods
    public void SetItem(Item item)
    {
        items.Clear();
        if (item != null)
        {
            items.Add(item);
            bProps.unburdenItem = null;
        }
    }
    public void UnsetItem(Item item)
    {
        items.Remove(item);
        if (item == null)
        {
            bProps.unburdenItem = item.itemID;
        }
    }

    // Next Command Properties
    public void SetNextCommand(BattleCommand command)
    {
        bProps.nextCommand = PokemonSavedCommand.CreateSavedCommand(command);
    }
    public void UnsetNextCommand()
    {
        bProps.nextCommand = null;
    }
    public BattleCommand ConvertNextCommand()
    {
       if (bProps.nextCommand != null)
        {
            BattleCommand command = BattleCommand.CreateMoveCommand(
                this,
                bProps.nextCommand.moveID,
                bProps.nextCommand.targetPositions,
                bProps.nextCommand.isPlayerCommand
                );
            command.iteration = bProps.nextCommand.iteration;
            command.consumePP = bProps.nextCommand.consumePP;
            command.displayMove = bProps.nextCommand.displayMove;
            return command;
        }
        return null;
    }

    


}



public class PokemonSavedCommand
{
    public string moveID;
    public List<BattlePosition> targetPositions;
    public bool isPlayerCommand;
    public int iteration;
    public bool consumePP;
    public bool displayMove;

    // Constructor
    public PokemonSavedCommand(
        string moveID,
        List<BattlePosition> targetPositions,
        bool isPlayerCommand = false,
        int iteration = 1,
        bool consumePP = true,
        bool displayMove = true
        )
    {
        this.moveID = moveID;
        this.targetPositions = new List<BattlePosition>();
        for (int i = 0; i < targetPositions.Count; i++)
        {
            this.targetPositions.Add(BattlePosition.Clone(targetPositions[i]));
        }
        this.isPlayerCommand = isPlayerCommand;
        this.iteration = iteration;
        this.consumePP = consumePP;
        this.displayMove = displayMove;
    }
    public static PokemonSavedCommand CreateSavedCommand(BattleCommand command)
    {
        List<BattlePosition> targetPositions = new List<BattlePosition>(command.targetPositions);
        PokemonSavedCommand savedCommand = new PokemonSavedCommand(
            moveID: command.moveID,
            targetPositions: targetPositions,
            isPlayerCommand: command.isExplicitlySelected,
            iteration: command.iteration,
            consumePP: command.consumePP,
            displayMove: command.displayMove
            );
        return savedCommand;
    }

    // Clone
    public static PokemonSavedCommand Clone(PokemonSavedCommand original)
    {
        PokemonSavedCommand cloneCommand = new PokemonSavedCommand(
            moveID: original.moveID,
            targetPositions: original.targetPositions,
            isPlayerCommand: original.isPlayerCommand,
            iteration: original.iteration,
            consumePP: original.consumePP
            );
        return cloneCommand;
    }
}
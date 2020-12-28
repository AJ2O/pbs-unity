using System.Collections.Generic;

namespace PBS.Main.Pokemon
{
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
                this.affectedMoves = affectedMoves == null ? new HashSet<string>()
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
                SetMoves(new string[] { move });
            }
            public void SetMoves(IEnumerable<string> moves)
            {
                affectedMoves = new HashSet<string>(moves);
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

            cloneProps.bound = original.bound == null ? null : original.bound.Clone();
            cloneProps.defenseCurl = original.defenseCurl == null ? null : original.defenseCurl.Clone();
            cloneProps.electrify = original.electrify == null ? null : original.electrify.Clone();
            cloneProps.embargo = original.embargo == null ? null : original.embargo.Clone();
            cloneProps.endure = original.endure == null ? null : original.endure.Clone();
            for (int i = 0; i < original.flashFireBoosts.Count; i++)
            {
                cloneProps.flashFireBoosts.Add(original.flashFireBoosts[i].Clone());
            }
            cloneProps.flinch = original.flinch == null ? null : original.flinch.Clone();

            for (int i = 0; i < original.forestsCurses.Count; i++)
            {
                cloneProps.forestsCurses.Add(ForestsCurse.Clone(original.forestsCurses[i]));
            }
            cloneProps.friskIdentified = original.friskIdentified;
            cloneProps.gulpMissile = original.gulpMissile == null ? null : original.gulpMissile.Clone();
            for (int i = 0; i < original.identifieds.Count; i++)
            {
                cloneProps.identifieds.Add(original.identifieds[i].Clone());
            }
            cloneProps.illusion = original.illusion == null ? null : original.illusion.Clone();
            cloneProps.imprison = original.imprison == null ? null : original.imprison.Clone();
            cloneProps.infatuation = original.infatuation == null ? null : original.infatuation.Clone();
            for (int i = 0; i < original.lockOnTargets.Count; i++)
            {
                cloneProps.lockOnTargets.Add(LockOn.Clone(original.lockOnTargets[i]));
            }
            for (int i = 0; i < original.moveLimiters.Count; i++)
            {
                cloneProps.moveLimiters.Add(original.moveLimiters[i].Clone());
            }
            cloneProps.perishSong = original.perishSong == null ? null : original.perishSong.Clone();
            for (int i = 0; i < original.tarShots.Count; i++)
            {
                cloneProps.tarShots.Add(original.tarShots[i].Clone());
            }
            cloneProps.protect = original.protect == null ? null : original.protect.Clone();
            cloneProps.tProps = original.tProps == null ? null
                : TransformProperties.Clone(original.tProps);
            cloneProps.yawn = original.yawn == null ? null : original.yawn.Clone();

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

            cloneProps.nextCommand = original.nextCommand == null ? null
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
            cloneProps.mimicMoveslot = original.mimicMoveslot == null ? null
                : Moveslot.Clone(original.mimicMoveslot);
            cloneProps.sketchBaseMove = original.sketchBaseMove;
            cloneProps.sketchMoveslot = original.sketchMoveslot == null ? null
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

            perishSong = other.perishSong == null ? null : other.perishSong.Clone();
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
}
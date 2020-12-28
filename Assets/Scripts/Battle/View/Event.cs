using PBS.Enums.Battle;
using PBS.Main.Team;
using PBS.Main.Trainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Events
{
    /// <summary>
    /// Displays events to the player's view.
    /// </summary>
    public class Base { 

    }

    // Battle Phases

    public class StartBattle : Base
    {

    }
    public class EndBattle : Base
    {
        public int winningTeam;
    }


    // Messages

    /// <summary>
    /// Displays a message to the player's dialog box.
    /// </summary>
    public class Message : Base
    {
        public string message;
    }
    public class MessageParameterized : Base
    {
        public string messageCode;
        public bool isQueryResponse = false;
        public bool isQuerySuccessful = true;
        public int playerPerspectiveID = 0;
        public int teamPerspectiveID = 0;

        private string p_pokemonID = "";
        public string pokemonID
        {
            get
            {
                return p_pokemonID;
            }
            set
            {
                p_pokemonID = (string.IsNullOrEmpty(value))? "" : value;
            }
        }
        private string p_pokemonUserID = "";
        public string pokemonUserID
        {
            get
            {
                return p_pokemonUserID;
            }
            set
            {
                p_pokemonUserID = (string.IsNullOrEmpty(value))? "" : value;
            }
        }
        private string p_pokemonTargetID = "";
        public string pokemonTargetID
        {
            get
            {
                return p_pokemonTargetID;
            }
            set
            {
                p_pokemonTargetID = (string.IsNullOrEmpty(value))? "" : value;
            }
        }
        public List<string> pokemonListIDs = new List<string>();

        public int trainerID = 0;

        public int teamID = 0;

        public string typeID = "";
        public List<string> typeIDs = new List<string>();

        public string moveID = "";
        public List<string> moveIDs = new List<string>();

        public string abilityID = "";
        public List<string> abilityIDs = new List<string>();

        public string itemID = "";
        public List<string> itemIDs = new List<string>();

        public string statusID = "";
        public string statusTeamID = "";
        public string statusEnvironmentID = "";

        public List<int> intArgs = new List<int>();
        public List<PokemonStats> statList = new List<PokemonStats>();
    }


    // Backend
    public class ModelUpdate : Base
    {
        public bool loadAssets = false;
    }
    public class ModelUpdateLoadAssets : Base
    {

    }
    public class ModelUpdatePokemon : Base
    {
        public bool loadAsset = false;
        public View.WifiFriendly.Pokemon pokemon;
    }
    public class ModelUpdateTrainer : Base
    {
        public bool loadAsset = false;
        public string name = "";
        public int playerID = 0;
        public int teamID = 0;
        public List<string> party = new List<string>();
        public List<string> items = new List<string>();
        public List<int> controlPos = new List<int>();

        public ModelUpdateTrainer() { }
        public ModelUpdateTrainer(Trainer trainer, bool loadAsset = false)
        {
            this.loadAsset = loadAsset;
            name = trainer.name;
            playerID = trainer.playerID;
            teamID = trainer.teamID;

            for (int i = 0; i < trainer.party.Count; i++)
            {
                party.Add(trainer.party[i].uniqueID);
            }
            for (int i = 0; i < trainer.items.Count; i++)
            {
                items.Add(trainer.items[i].itemID);
            }
            for (int i = 0; i < trainer.controlPos.Length; i++)
            {
                controlPos.Add(trainer.controlPos[i]);
            }
        }
    }
    public class ModelUpdateTeam : Base
    {
        public bool loadAsset = false;
        public int teamID = 0;
        public TeamMode teamMode = TeamMode.Single;
        public List<int> trainers = new List<int>();

        public ModelUpdateTeam() { }
        public ModelUpdateTeam(Team obj, bool loadAsset = false)
        {
            this.loadAsset = loadAsset;
            teamID = obj.teamID;
            teamMode = (obj.teamMode == Team.TeamMode.Single)? TeamMode.Single
                : (obj.teamMode == Team.TeamMode.Double)? TeamMode.Double
                : TeamMode.Triple;
            for (int i = 0; i < obj.trainers.Count; i++)
            {
                trainers.Add(obj.trainers[i].playerID);
            }
        }
    }


    // Command Prompts
    public class CommandAgent
    {
        public class Moveslot
        {
            public string moveID = "";
            public int PP = 0;
            public int maxPP = 0;

            public int basePower = 0;
            public float accuracy = 0;

            public bool useable = true;
            public bool hide = false;
            public string failMessageCode = "";

            public List<List<BattlePosition>> possibleTargets = new List<List<BattlePosition>>();

            public Moveslot() { }
            public Moveslot(string moveID)
            {
                MoveData moveData = MoveDatabase.instance.GetMoveData(moveID);
                this.moveID = moveID;
                this.PP = moveData.PP;
                this.maxPP = moveData.PP;
                this.basePower = moveData.basePower;
                this.accuracy = moveData.accuracy;
            }
        }

        public string pokemonUniqueID;
        public bool canMegaEvolve = false;
        public bool canZMove = false;
        public bool canDynamax = false;
        public bool isDynamaxed = false;

        public List<BattleCommandType> commandTypes;
        public List<Moveslot> moveslots;
        public List<Moveslot> zMoveSlots;
        public List<Moveslot> dynamaxMoveSlots;
    }
    public class CommandGeneralPrompt : Base
    {
        public int playerID;
        public bool multiTargetSelection;
        public bool canMegaEvolve;
        public bool canZMove;
        public bool canDynamax;

        public List<string> items;
        public List<CommandAgent> pokemonToCommand;
    }
    public class CommandReplacementPrompt : Base
    {
        public int playerID;
        public int[] fillPositions;
    }


    // Trainer Interactions
    public class TrainerSendOut : Base
    {
        public int playerID;
        public List<string> pokemonUniqueIDs;
    }
    public class TrainerMultiSendOut : Base
    {
        public List<TrainerSendOut> sendEvents;
    }
    public class TrainerWithdraw : Base
    {
        public int playerID;
        public List<string> pokemonUniqueIDs;
    }
    public class TrainerItemUse : Base
    {
        public int playerID;
        public string itemID;
    }

    // Weather / Environmental Conditions


    // --- Pokemon Interactions ---

    // General
    public class PokemonChangeForm : Base
    {
        public string pokemonUniqueID;
        public string preForm;
        public string postForm;
    }
    public class PokemonSwitchPosition : Base
    {
        public string pokemonUniqueID1;
        public string pokemonUniqueID2;
    }

    // Health
    public class PokemonHealthDamage : Base
    {
        public string pokemonUniqueID;
        public int preHP;
        public int postHP;
        public int maxHP;
        public int damageDealt
        {
            get
            {
                return preHP - postHP;
            }
        }
    }
    public class PokemonHealthHeal : Base
    {
        public string pokemonUniqueID;
        public int preHP;
        public int postHP;
        public int maxHP;
        public int hpHealed
        {
            get
            {
                return postHP - preHP;
            }
        }
    }
    public class PokemonHealthFaint : Base
    {
        public string pokemonUniqueID;
    }
    public class PokemonHealthRevive : Base
    {
        public string pokemonUniqueID;
    }

    // Abilities
    public class PokemonAbilityActivate : Base
    {
        public string pokemonUniqueID;
        public string abilityID;
    }

    // Moves
    public class PokemonMoveUse : Base
    {
        public string pokemonUniqueID;
        public string moveID;
    }
    
    public class PokemonMoveHitTarget
    {
        public string pokemonUniqueID;
        public bool affectedByMove = false;
        public bool missed = false;
        public bool criticalHit = false;
        public int preHP;
        public int postHP;
        public int maxHP;
        public int damageDealt;
        public float effectiveness = 1f;

        public PokemonMoveHitTarget() { }
        public PokemonMoveHitTarget(BattleHitTarget hitTarget)
        {
            pokemonUniqueID = hitTarget.pokemon.uniqueID;
            affectedByMove = hitTarget.affectedByMove;
            missed = hitTarget.missed;
            criticalHit = hitTarget.criticalHit;
            preHP = hitTarget.preHP;
            postHP = hitTarget.postHP;
            maxHP = hitTarget.pokemon.maxHP;
            damageDealt = hitTarget.damageDealt;
            effectiveness = hitTarget.effectiveness.GetTotalEffectiveness();
        }
    }
    public class PokemonMoveHit : Base
    {
        public string pokemonUniqueID;
        public string moveID;
        public int currentHit = 1;
        public List<PokemonMoveHitTarget> hitTargets;
    }

    // Stats
    public class PokemonStatChange : Base
    {
        public string pokemonUniqueID;
        public int modValue;
        public bool runAnim = false;
        public bool maximize = false;
        public bool minimize = false;
        public List<PokemonStats> statsToMod;
    }
    public class PokemonStatUnchangeable : Base
    {
        public string pokemonUniqueID;
        public bool tooHigh;
        public List<PokemonStats> statsToMod;
    }

    // Items

    // Status


}


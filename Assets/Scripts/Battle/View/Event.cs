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
        public int playerPerspectiveID = 0;
        public int teamPerspectiveID = 0;

        public string pokemonID = "";
        public string pokemonUserID = "";
        public string pokemonTargetID = "";
        public List<string> pokemonListIDs = new List<string>();

        public int trainerID = -1;

        public int teamID = -1;

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
    public class MessagePokemon : Base
    {
        public string preMessage = "";
        public string postMessage = "";
        public List<string> pokemonUniqueIDs;
    }
    public class MessageTrainer : Base
    {
        public string preMessage = "";
        public string postMessage = "";
        public List<int> playerIDs;
    }
    public class MessageTeam : Base
    {
        public string preMessage = "";
        public string postMessage = "";
        public int teamID;
    }

    // Backend
    public class ModelUpdate : Base
    {
        public enum UpdateType
        {
            None,
            LoadAssets
        }
        public UpdateType updateType;
        public bool synchronize = true;
        public Battle.View.Model model;
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
    public class EnvironmentalConditionStart : Base
    {
        public string conditionID;
    }
    public class EnvironmentalConditionEnd : Base
    {
        public string conditionID;
    }


    // --- Pokemon Interactions ---

    // General

    // Health
    public class PokemonHealthDamage : Base
    {
        public string pokemonUniqueID;
        public int preHP;
        public int postHP;
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
    public class PokemonAbilityQuickDraw : PokemonAbilityActivate { }

    // Moves
    public class PokemonMoveUse : Base
    {
        public string pokemonUniqueID;
        public string moveID;
    }
    public class PokemonMoveCelebrate : Base { }

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
    public class PokemonItemQuickClaw : Base
    {
        public string pokemonUniqueID;
        public string itemID;
    }

    // Status

    // Misc Status
    public class PokemonMiscProtect : Base
    {
        public string pokemonUniqueID;
    }
    public class PokemonMiscMatBlock : Base
    {
        public int teamID;
    }


}


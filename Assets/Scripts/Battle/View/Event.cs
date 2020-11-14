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
        public string[] parameters;
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

    // Stats

    // Items
    public class PokemonItemQuickClaw : Base
    {
        public string pokemonUniqueID;
        public string itemID;
    }


}


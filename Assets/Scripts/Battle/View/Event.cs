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

}


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

    // Battle Phases (0 - 99)

    public class StartBattle : Base
    {

    }

    public class EndBattle : Base
    {

    }

    // Messages (100 - 199)

    /// <summary>
    /// Displays a message to the player's dialog box.
    /// </summary>
    public class Message : Base
    {
        public string message;
    }

    // Backend (200 - 299)

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


    // Trainer Interactions (1001 - 1099)

    public class TrainerSendOut : Base
    {
        public int playerID;
        public List<string> pokemonUniqueIDs;
    }

    public class TrainerMultiSendOut : Base
    {
        public List<TrainerSendOut> sendEvents;
    }

}


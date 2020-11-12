using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Events
{
    /// <summary>
    /// Displays events to the player's view.
    /// </summary>
    public class Base { 
        public Base() { }
    }

    // Messages

    /// <summary>
    /// Displays a message to the player's dialog box.
    /// </summary>
    public class Message : Base
    {
        public string message;
        public Message(string message)
        {
            this.message = message;
        }
    }

    // Backend

    public class ModelUpdate : Base
    {
        public enum UpdateType : byte
        {
            LoadAssets
        }

        public Battle.Core.Model model;
        public UpdateType updateType;
        public ModelUpdate(Battle.Core.Model model, UpdateType updateType = UpdateType.LoadAssets)
        {
            this.model = model;
            this.updateType = updateType;
        }
    }

    // Phases

    public class StartBattle : Base
    {
        public StartBattle(Battle.Core.Model model)
        {

        }
    }

    public class EndBattle : Base
    {
        public EndBattle(Battle.Core.Model model)
        {

        }
    }

}


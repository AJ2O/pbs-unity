using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking { 
    
    public class Manager : NetworkManager
    {
        /// <summary>
        /// Mediates interactions between players and the battle instance.
        /// </summary>
        public Core battleCore;
        /// <summary>
        /// The amount of players needed to start the battle.
        /// </summary>
        public int requiredPlayers = 1;
        /// <summary>
        /// The settings that will be applied to the initiated battle.
        /// </summary>
        public BattleSettings battleSettings;
        
        /// <summary>
        /// Contains a mapping of connections to player objects in the scene.
        /// </summary>
        public Dictionary<int, PBS.Networking.Player> playerConnections;
        /// <summary>
        /// Contains a mapping of connections for the players and their corresponding trainers.
        /// </summary>
        Dictionary<int, Trainer> trainerConnections;

        // 1.
        public override void Start()
        {
            base.Start();

            playerConnections = new Dictionary<int, Player>();
            trainerConnections = new Dictionary<int, Trainer>();
        }

        // 2.
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            // Associate player with new connection
            GameObject player = Instantiate(playerPrefab);
            NetworkServer.AddPlayerForConnection(conn, player);

            Debug.Log("A player is joining the server.");
        }

        // TODO: OnServerDisconnect

        public void AddPlayer(NetworkConnection conn, PBS.Networking.Player player)
        {
            playerConnections.Add(conn.connectionId, player);
        }
        public void AddTrainer(NetworkConnection conn)
        {
            Trainer trainer = Testing.CreateTrainerUsingTeamNo();
            if (trainerConnections.ContainsKey(conn.connectionId))
            {
                Debug.LogWarning("A player with ID " + conn.connectionId + " is trying to join, but is already in the battle!");
            }
            else if (trainerConnections.Count < requiredPlayers)
            {
                trainer.playerID = conn.connectionId;
                trainerConnections.Add(conn.connectionId, trainer);
                Debug.Log("Added player " + trainer.name + "!");

                // 5.
                // Start battle if we have enough trainers
                if (trainerConnections.Count == requiredPlayers)
                {
                    StartBattle();
                }
            }
            else
            {
                Debug.Log("A spectator (" + trainer.name + ") is watching...");
            }
        }

        public Trainer GetTrainer(int connID)
        {
            try
            {
                Trainer trainer = trainerConnections[connID];
                return trainer;
            }
            catch
            {
                Debug.LogError("Could not find a trainer with ID: " + connID);
                return null;
            }
        }

        public PBS.Networking.Player GetPlayer(int connID)
        {
            try
            {
                PBS.Networking.Player player = playerConnections[connID];
                return player;
            }
            catch
            {
                Debug.LogError("Could not find a player with ID: " + connID);
                return null;
            }
        }
        public List<PBS.Networking.Player> GetAllPlayers()
        {
            return new List<Player>(playerConnections.Values);
        }

        // 6.
        public void StartBattle()
        {
            // Create teams using trainerConnections
            List<BattleTeam> teams = new List<BattleTeam>();
            List<Trainer> allTrainers = new List<Trainer>(trainerConnections.Values);

            // TODO: More battle configurations than single or double
            BattleTeam.TeamMode teamMode = (battleSettings.battleType == BattleType.Single) ? BattleTeam.TeamMode.Single
                : BattleTeam.TeamMode.Double;

            // TODO: More than 2 trainers
            for (int i = 0; i < allTrainers.Count && i < 2; i++)
            {
                Trainer trainer = allTrainers[i];
                BattleTeam team = new BattleTeam(
                    teamMode: teamMode,
                    trainers: new List<Trainer> { trainer }
                    );

                // Synchronize trainer to player
                PBS.Networking.Player player = GetPlayer(trainer.playerID);
                player.RpcSyncTrainerToClient(new Battle.View.Compact.Trainer(trainer));
                player.RpcSyncTeamPerspectiveToClient(new Battle.View.Compact.Team(team));

                teams.Add(team);
            }

            // Start Battle Core
            Debug.Log("Starting Battle! (From Network Manager)");
            StartCoroutine(battleCore.StartBattle(battleSettings: battleSettings, teams: teams));
        }


    }

}


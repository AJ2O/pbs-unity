using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking { 
    
    public class Manager : NetworkManager
    {
        /// <summary>
        /// Mediates interactions between players and the battle instance.
        /// </summary>
        [Tooltip("Mediates interactions between players and the battle.")]
        public Core battleCore;
        /// <summary>
        /// The amount of human players needed to start the battle.
        /// </summary>
        [Tooltip("The amount of human players needed to start the battle.")]
        public int humanTrainers = 1;
        /// <summary>
        /// The difference between this and "Human Trainers" is how many AI trainers there will be.
        /// </summary>
        [Tooltip("The difference between this and \"Human Trainers\" is how many AI trainers there will be.")]
        public int totalTrainers = 2;
        /// <summary>
        /// The settings that will be applied to the battle.
        /// </summary>
        [Tooltip("The settings that will be applied to the battle.")]
        public BattleSettings battleSettings;
        
        /// <summary>
        /// Contains a mapping of connections to player objects in the scene.
        /// </summary>
        public Dictionary<int, PBS.Networking.Player> playerConnections;
        /// <summary>
        /// Contains a mapping of connections for the players and their corresponding trainers.
        /// </summary>
        public Dictionary<int, Trainer> trainerConnections;

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
            playerConnections.Add(conn.connectionId + 1, player);
        }
        public void AddTrainer(NetworkConnection conn)
        {
            int debugMod = trainerConnections.Count % 2;

            Trainer trainer = Testing.CreateTrainerUsingTeamNo(
                trainerName: (debugMod == 0)? "Red" : "Blue",
                teamNo: (debugMod == 0)? 1 : 2
                );
            if (trainerConnections.ContainsKey(conn.connectionId + 1))
            {
                Debug.LogWarning("A player with ID " + conn.connectionId + " is trying to join, but is already in the battle!");
            }
            else if (trainerConnections.Count < humanTrainers)
            {
                trainer.playerID = conn.connectionId + 1;
                trainerConnections.Add(trainer.playerID, trainer);
                Debug.Log("Added player " + trainer.name + "!");

                // 5.
                // Start battle if we have enough trainers
                if (trainerConnections.Count == humanTrainers)
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
            List<Trainer> humanTrainers = new List<Trainer>(trainerConnections.Values);

            // Configure AI Trainers
            List<Trainer> aiTrainers = new List<Trainer>();
            int aiTrainerCount = Mathf.Max(totalTrainers - this.humanTrainers, 0);
            Debug.Log($"Adding {aiTrainerCount} AI trainers...");
            for (int i = 0; i < aiTrainerCount; i++)
            {
                Trainer trainer = Testing.CreateTrainer2("AI " + i);
                trainer.playerID = -1 - i;
                aiTrainers.Add(trainer);
            }

            // TODO: More battle configurations than single or double
            BattleTeam.TeamMode teamMode = (battleSettings.battleType == BattleType.Single) ? BattleTeam.TeamMode.Single
                : BattleTeam.TeamMode.Double;
            
            List<Trainer> allTrainers = new List<Trainer>();
            allTrainers.AddRange(humanTrainers);
            allTrainers.AddRange(aiTrainers);
            int midpoint = allTrainers.Count / 2;

            BattleTeam team1 = new BattleTeam(
                teamID: 1,
                trainers: allTrainers.GetRange(0, midpoint),
                teamMode: teamMode
                );
            BattleTeam team2 = new BattleTeam(
                teamID: 2,
                trainers: allTrainers.GetRange(midpoint, allTrainers.Count - midpoint),
                teamMode: teamMode
                );
            teams.Add(team1);
            teams.Add(team2);

            // TODO: More than 2 trainers
            for (int i = 0; i < humanTrainers.Count; i++)
            {
                Trainer trainer = humanTrainers[i];
                BattleTeam team = (trainer.teamID == team1.teamID)? team1 : team2;

                // Synchronize trainer & team to player
                PBS.Networking.Player player = GetPlayer(trainer.playerID);
                NetworkIdentity networkIdentity = player.GetComponent<NetworkIdentity>();

                player.playerID = trainer.playerID;
                player.TargetSyncTrainerToClient(networkIdentity.connectionToClient, new Battle.View.WifiFriendly.Trainer(trainer));
                player.TargetSyncTeamPerspectiveToClient(networkIdentity.connectionToClient, new Battle.View.WifiFriendly.Team(team));
            }

            // Start Battle Core
            Debug.Log("Starting Battle! (From Network Manager)");
            Debug.Log($"{team1.trainers.Count} vs. {team2.trainers.Count}");
            StartCoroutine(battleCore.StartBattle(battleSettings: battleSettings, teams: teams));
        }


    }

}


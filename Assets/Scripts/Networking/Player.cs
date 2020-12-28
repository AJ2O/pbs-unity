using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PBS.Databases;

namespace PBS.Networking
{

    /// <summary>
    /// This class handles interaction between the client and server.
    /// On the server, this handles sending commands and queries to the server.
    /// On the client, this handles receiving events and displaying them for the player.
    /// </summary>
    public class Player : NetworkBehaviour
    {
        #region Attributes
        /// <summary>
        /// The ID of this player. If the player is spectating, this value is 0.
        /// </summary>
        public int playerID = 0;
        /// <summary>
        /// The in-battle representation of this player's trainer.
        /// </summary>
        public Battle.View.WifiFriendly.Trainer myTrainer;
        /// <summary>
        /// Battle events will be displayed to this player according to this team's perspective.
        /// </summary>
        public Battle.View.WifiFriendly.Team myTeamPerspective;
        /// <summary>
        /// The model of the battle that the player sees.
        /// </summary>
        public Battle.View.Model myModel;
        /// <summary>
        /// UI display.
        /// </summary>
        public Battle.View.UI.Canvas battleUI;
        /// <summary>
        /// Scene display.
        /// </summary>
        public Battle.View.Scene.Canvas sceneCanvas;

        /// <summary>
        /// The controls component for the player.
        /// </summary>
        public PBS.Player.BattleControls controls;

        // Events
        /// <summary>
        /// If true, this player is currently executing battle events. Battle events run by one at a time.
        /// </summary>
        bool isExecutingEvents = true;
        /// <summary>
        /// The coroutine handling the execution of events.
        /// </summary>
        Coroutine eventExecutor;
        /// <summary>
        /// The queue of events the player needs to run.
        /// </summary>
        List<PBS.Battle.View.Events.Base> eventQueue;
        /// <summary>
        /// If true, the player is waiting for a response from the server.
        /// </summary>
        bool isWaitingForResponse = false;
        /// <summary>
        /// A potential response message from the server.
        /// </summary>
        public PBS.Battle.View.Events.MessageParameterized queryMessageResponse;
        /// <summary>
        /// A potential query response from the server.
        /// </summary>
        public PBS.Player.Query.QueryResponseBase queryResponse;
        #endregion

        private void Awake()
        {
            myTrainer = null;
            myTeamPerspective = null;
            myModel = null;
            queryMessageResponse = null;
            queryResponse = null;
        }

        #region Player Convenience Checks
        /// <summary>
        /// Returns true if the given playerID matches this player's.
        /// </summary>
        /// <param name="playerID">The playerID to check.</param>
        /// <returns></returns>
        public bool IsTrainerPlayer(int playerID)
        {
            if (myTrainer != null)
            {
                return playerID == myTrainer.playerID;
            }
            return false;
        }
        /// <summary>
        /// Returns true if the given trainer represents this player.
        /// </summary>
        /// <param name="trainer">The trainer to check.</param>
        /// <returns></returns>
        public bool IsTrainerPlayer(Battle.View.WifiFriendly.Trainer trainer)
        {
            return IsTrainerPlayer(trainer.playerID);
        }
        /// <summary>
        /// Returns true if the given pokemon is owned by the player.
        /// </summary>
        /// <param name="pokemon">The pokemon to check.</param>
        /// <returns></returns>
        public bool IsPokemonOwnedByPlayer(Battle.View.WifiFriendly.Pokemon pokemon)
        {
            if (myTrainer != null)
            {
                PBS.Battle.View.WifiFriendly.Trainer ownerTrainer = myModel.GetTrainer(pokemon);
                if (ownerTrainer != null)
                {
                    return ownerTrainer.playerID == myTrainer.playerID;
                }
            }
            return false;
        }
        #endregion

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            Debug.Log("Joined the server!");

            isExecutingEvents = true;
            eventQueue = new List<Battle.View.Events.Base>();

            SetComponents();

            // Synchronize trainer to server
            //Trainer trainer = Testing.CreateTrainerUsingTeamNo();
            CmdSyncTrainerToServer();

            // Wait for server events
            StartCoroutine(RunEventPollingSystem());
        }

        #region Server/Client Interaction
        // 3.
        /// <summary>
        /// Initializes the components of this object.
        /// </summary>
        [Client]
        public void SetComponents()
        {
            UpdateModel(new Battle.View.Model());

            // UI
            this.battleUI = Static.Master.instance.ui;
            this.controls.battleUI = this.battleUI;
            this.controls.EnableQuickAdvanceDialog();

            // Scene
            this.sceneCanvas =  Static.Master.instance.scene;
            this.controls.sceneCanvas = this.sceneCanvas;
        }

        // 4.
        /// <summary>
        /// Synchronizes this player's trainer to the server.
        /// </summary>
        [Command]
        public void CmdSyncTrainerToServer()
        {
            Static.Master.instance.networkManager.AddPlayer(this.connectionToClient, this);
            Static.Master.instance.networkManager.AddTrainer(this.connectionToClient);
        }
        /// <summary>
        /// Synchronizes the server's trainer representation to the client.
        /// </summary>
        /// <param name="target">The targeted player to send this command to.</param>
        /// <param name="trainer">The server's version of the trainer sent to the client.</param>
        [TargetRpc]
        public void TargetSyncTrainerToClient(NetworkConnection target, Battle.View.WifiFriendly.Trainer trainer)
        {
            myTrainer = trainer;
            playerID = myTrainer.playerID;
        }
        /// <summary>
        /// Synchronizes the server's team representation to the client.
        /// </summary>
        /// <param name="target">The player to send this command to.</param>
        /// <param name="perspective">The server's version of the team sent to the client.</param>
        [TargetRpc]
        public void TargetSyncTeamPerspectiveToClient(NetworkConnection target, Battle.View.WifiFriendly.Team perspective)
        {
            myTeamPerspective = perspective;
        }

        // 7.
        /// <summary>
        /// Sends an event to the client for the player to enqueue.
        /// </summary>
        /// <param name="target">The player to send this event to.</param>
        /// <param name="bEvent">The event to be queued.</param>
        [TargetRpc]
        public void TargetReceiveEvent(NetworkConnection target, Battle.View.Events.Base bEvent)
        {
            eventQueue.Add(bEvent);
        }
        /// <summary>
        /// Sends a message response to the client to be displayed.
        /// </summary>
        /// <param name="target">The player to send this event to.</param>
        /// <param name="bEvent">The message event.</param>
        [TargetRpc]
        public void TargetReceiveQueryMessageResponse(NetworkConnection target, Battle.View.Events.MessageParameterized bEvent)
        {
            queryMessageResponse = bEvent;
        }
        /// <summary>
        /// Sends a query response to the client to be displayed.
        /// </summary>
        /// <param name="target">The player to send this event to.</param>
        /// <param name="response">The query response.</param>
        [TargetRpc]
        public void TargetReceiveQueryResponse(NetworkConnection target, PBS.Player.Query.QueryResponseBase response)
        {
            queryResponse = response;
        }

        // 8.
        /// <summary>
        /// Sends player commands to the server.
        /// </summary>
        /// <param name="commands"></param>
        [Command]
        public void CmdSendCommands(List<PBS.Player.Command> commands)
        {
            Static.Master.instance.networkManager.battleCore.ReceiveCommands(playerID, commands, false);
        }
        /// <summary>
        /// Sends player replacement commands to the server.
        /// </summary>
        /// <param name="commands"></param>
        [Command]
        public void CmdSendCommandsReplace(List<PBS.Player.Command> commands)
        {
            Static.Master.instance.networkManager.battleCore.ReceiveCommands(playerID, commands, true);
        }
        /// <summary>
        /// Sends a player query to the server.
        /// </summary>
        /// <param name="query"></param>
        [Command]
        public void CmdSendQuery(PBS.Player.Query.QueryBase query)
        {
            Static.Master.instance.networkManager.battleCore.ReceiveQuery(query, playerID);
        }
        /// <summary>
        /// Sends a player's command query to the server.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="committedCommands"></param>
        [Command]
        public void CmdSendCommmandQuery(PBS.Player.Command command, List<PBS.Player.Command> committedCommands)
        {
            Static.Master.instance.networkManager.battleCore.ReceiveCommandQuery(
                command: command,
                committedCommands: committedCommands);
        }
        #endregion

        #region Event Polling and Execution
        /// <summary>
        /// A continuous system that polls the server for events, adds them to the queue, and runs them for the player.
        /// </summary>
        /// <returns></returns>
        public IEnumerator RunEventPollingSystem()
        {
            while (true)
            {
                if (eventQueue.Count > 0 && isExecutingEvents)
                {
                    // Run all events in queue
                    PBS.Battle.View.Events.Base bEvent = eventQueue[0];
                    eventExecutor = StartCoroutine(ExecuteEvent(bEvent));
                    yield return eventExecutor;

                    eventQueue.RemoveAt(0);
                }
                yield return null;
            }
        }
        /// <summary>
        /// Sends a query to the server and waits for a response in <seealso cref="queryResponse"/>.
        /// </summary>
        /// <param name="query">The query to send.</param>
        /// <returns></returns>
        public IEnumerator RunQueryResponsePollingSystem(PBS.Player.Query.QueryBase query)
        {
            isWaitingForResponse = true;
            queryResponse = null;
            CmdSendQuery(query);
            while (isWaitingForResponse)
            {
                if (queryResponse != null)
                {
                    isWaitingForResponse = false;
                }
                yield return null;
            }
        }
        /// <summary>
        /// Sends a potential command to the server and waits for a validity response in 
        /// <seealso cref="queryMessageResponse"/>.
        /// </summary>
        /// <param name="command">The command to check for validity.</param>
        /// <param name="committedCommands">The commands the player has already committed to.</param>
        /// <returns></returns>
        public IEnumerator RunCommandQueryResponsePollingSystem(
            PBS.Player.Command command,
            List<PBS.Player.Command> committedCommands)
        {
            isWaitingForResponse = true;
            queryMessageResponse = null;
            CmdSendCommmandQuery(command, committedCommands);
            while (isWaitingForResponse)
            {
                if (queryMessageResponse != null)
                {
                    isWaitingForResponse = false;
                }
                yield return null;
            }
        }

        /// <summary>
        /// Runs a battle event from this player's perspective.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent(PBS.Battle.View.Events.Base bEvent)
        {
            yield return StartCoroutine(

                // Battle Phases
                (bEvent is PBS.Battle.View.Events.StartBattle)? 
                ExecuteEvent_StartBattle(bEvent as PBS.Battle.View.Events.StartBattle)
                : (bEvent is PBS.Battle.View.Events.EndBattle)? 
                ExecuteEvent_EndBattle(bEvent as PBS.Battle.View.Events.EndBattle)


                // Messages
                : (bEvent is PBS.Battle.View.Events.Message)? 
                ExecuteEvent_Message(bEvent as PBS.Battle.View.Events.Message)
                : (bEvent is PBS.Battle.View.Events.MessageParameterized)? 
                ExecuteEvent_MessageParameterized(bEvent as PBS.Battle.View.Events.MessageParameterized)


                // Backend
                : (bEvent is PBS.Battle.View.Events.ModelUpdate)? 
                ExecuteEvent_ModelUpdate(bEvent as PBS.Battle.View.Events.ModelUpdate)
                : (bEvent is PBS.Battle.View.Events.ModelUpdatePokemon)? 
                ExecuteEvent_ModelUpdatePokemon(bEvent as PBS.Battle.View.Events.ModelUpdatePokemon)
                : (bEvent is PBS.Battle.View.Events.ModelUpdateTrainer)? 
                ExecuteEvent_ModelUpdateTrainer(bEvent as PBS.Battle.View.Events.ModelUpdateTrainer)
                : (bEvent is PBS.Battle.View.Events.ModelUpdateTeam)? 
                ExecuteEvent_ModelUpdateTeam(bEvent as PBS.Battle.View.Events.ModelUpdateTeam)


                // Command Prompts
                : (bEvent is PBS.Battle.View.Events.CommandGeneralPrompt)? 
                ExecuteEvent_CommandGeneralPrompt(bEvent as PBS.Battle.View.Events.CommandGeneralPrompt)
                : (bEvent is PBS.Battle.View.Events.CommandReplacementPrompt)? 
                ExecuteEvent_CommandReplacementPrompt(bEvent as PBS.Battle.View.Events.CommandReplacementPrompt)


                // Trainer Interactions
                : (bEvent is PBS.Battle.View.Events.TrainerSendOut)? 
                ExecuteEvent_TrainerSendOut(bEvent as PBS.Battle.View.Events.TrainerSendOut)
                : (bEvent is PBS.Battle.View.Events.TrainerMultiSendOut)? 
                ExecuteEvent_TrainerMultiSendOut(bEvent as PBS.Battle.View.Events.TrainerMultiSendOut)
                : (bEvent is PBS.Battle.View.Events.TrainerWithdraw)? 
                ExecuteEvent_TrainerWithdraw(bEvent as PBS.Battle.View.Events.TrainerWithdraw)
                : (bEvent is PBS.Battle.View.Events.TrainerItemUse)? 
                ExecuteEvent_TrainerItemUse(bEvent as PBS.Battle.View.Events.TrainerItemUse)


                // Environmental Interactions


                // --- Pokemon Interactions ---

                // General
                : (bEvent is PBS.Battle.View.Events.PokemonChangeForm)? 
                ExecuteEvent_PokemonChangeForm(bEvent as PBS.Battle.View.Events.PokemonChangeForm)
                : (bEvent is PBS.Battle.View.Events.PokemonSwitchPosition)? 
                ExecuteEvent_PokemonSwitchPosition(bEvent as PBS.Battle.View.Events.PokemonSwitchPosition)

                // Damage / Health
                : (bEvent is PBS.Battle.View.Events.PokemonHealthDamage)? 
                ExecuteEvent_PokemonHealthDamage(bEvent as PBS.Battle.View.Events.PokemonHealthDamage)
                : (bEvent is PBS.Battle.View.Events.PokemonHealthHeal)? 
                ExecuteEvent_PokemonHealthHeal(bEvent as PBS.Battle.View.Events.PokemonHealthHeal)
                : (bEvent is PBS.Battle.View.Events.PokemonHealthFaint)? 
                ExecuteEvent_PokemonHealthFaint(bEvent as PBS.Battle.View.Events.PokemonHealthFaint)
                : (bEvent is PBS.Battle.View.Events.PokemonHealthRevive)? 
                ExecuteEvent_PokemonHealthRevive(bEvent as PBS.Battle.View.Events.PokemonHealthRevive)

                // Abilities
                : (bEvent is PBS.Battle.View.Events.PokemonAbilityActivate)? 
                ExecuteEvent_PokemonAbilityActivate(bEvent as PBS.Battle.View.Events.PokemonAbilityActivate)

                // Moves
                : (bEvent is PBS.Battle.View.Events.PokemonMoveUse)? 
                ExecuteEvent_PokemonMoveUse(bEvent as PBS.Battle.View.Events.PokemonMoveUse)
                : (bEvent is PBS.Battle.View.Events.PokemonMoveHit)? 
                ExecuteEvent_PokemonMoveHit(bEvent as PBS.Battle.View.Events.PokemonMoveHit)

                // Stats
                : (bEvent is PBS.Battle.View.Events.PokemonStatChange)? 
                ExecuteEvent_PokemonStatChange(bEvent as PBS.Battle.View.Events.PokemonStatChange)
                : (bEvent is PBS.Battle.View.Events.PokemonStatUnchangeable)? 
                ExecuteEvent_PokemonStatUnchangeable(bEvent as PBS.Battle.View.Events.PokemonStatUnchangeable)


                : ExecuteEvent_Unhandled(bEvent)
                );

            yield return null;
        }
        #endregion

        // Unhandled Events
        public IEnumerator ExecuteEvent_Unhandled(Battle.View.Events.Base bEvent)
        {
            Debug.LogWarning("Received unknown event type");
            yield return null;
        }

        #region Battle Phases
        /// <summary>
        /// Displays the event for starting the battle.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_StartBattle(Battle.View.Events.StartBattle bEvent)
        {
            // Get Ally Trainers
            string allyString = "";
            if (myTrainer != null)
            {
                // Not a spectator
                allyString += "You";
                List<PBS.Battle.View.WifiFriendly.Trainer> allyTrainers = myModel.GetTrainerAllies(myTrainer);
                for (int i = 0; i < allyTrainers.Count; i++)
                {
                    allyString += " and " + allyTrainers[i].name;
                }
            }

            // Get Enemy Trainers
            string enemyString = "";
            List<PBS.Battle.View.WifiFriendly.Trainer> enemyTrainers = myModel.GetTrainerEnemies(myTeamPerspective);
            enemyString += GetTrainerNames(enemyTrainers);

            // Challenge Statement
            string challengeString = " were challenged by ";

            // End Statement
            string endString = "!";
            yield return StartCoroutine(battleUI.DrawText($"{allyString}{challengeString}{enemyString}{endString}"));
        }
        /// <summary>
        /// Displays the event for finishing the battle.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_EndBattle(Battle.View.Events.EndBattle bEvent)
        {
            string text = "";
            if (bEvent.winningTeam < 0)
            {
                text = "The battle ended in a draw!";
            }
            else
            {
                // spectator
                string allyString = "";
                string enemyString = GetTrainerNames(myModel.GetTrainerEnemies(myTeamPerspective));
                string resultString = " defeated ";
                string endString = "!";
                if (myTrainer == null)
                {
                    allyString = GetTrainerNames(myTeamPerspective.trainers);
                }
                else
                {
                    allyString = "You";
                }

                if (myTeamPerspective.teamID != bEvent.winningTeam)
                {
                    resultString = " lost to ";
                    endString = "...";
                }

                text = $"{allyString}{resultString}{enemyString}{endString}";
            }
            yield return StartCoroutine(battleUI.DrawText(text));
        }
        #endregion

        #region Messages
        /// <summary>
        /// Displays the given message to the player.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_Message(PBS.Battle.View.Events.Message bEvent)
        {
            yield return StartCoroutine(battleUI.DrawText(bEvent.message));
        }
        /// <summary>
        /// Parses the given event message and displays it to the player.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_MessageParameterized(Battle.View.Events.MessageParameterized bEvent)
        {
            yield return StartCoroutine(battleUI.DrawText(
                Battle.View.UI.Canvas.RenderMessage(
                    message: bEvent,
                    myModel: myModel,
                    myPlayerID: playerID,
                    myTrainer: myTrainer,
                    myTeamPerspective: myTeamPerspective
                    )
                ));
        }
        #endregion

        #region Model/Backend Updates
        /// <summary>
        /// Sets <seealso cref="myModel"/> to this value, and does the same for attached components such as 
        /// for <seealso cref="controls"/>.
        /// </summary>
        /// <param name="model"></param>
        public void UpdateModel(Battle.View.Model model)
        {
            myModel = model;
            controls.battleModel = model;
        }
        /// <summary>
        /// Updates references and loads all assets defined in the event, related to the whole battle.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_ModelUpdate(PBS.Battle.View.Events.ModelUpdate bEvent)
        {
            // Update references in the model
            yield return StartCoroutine(BattleAssetLoader.instance.LoadBattleAssets(myModel));
            UpdateModel(myModel);
        }
        /// <summary>
        /// Updates references and loads all assets defined in the event, related to pokemon.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_ModelUpdatePokemon(PBS.Battle.View.Events.ModelUpdatePokemon bEvent)
        {
            // Update references in the model
            bool isInModel = false;

            for (int i = 0; i < myModel.teams.Count && !isInModel; i++)
            {
                Battle.View.WifiFriendly.Team team = myModel.teams[i];
                for (int k = 0; k < team.trainers.Count && !isInModel; k++)
                {
                    Battle.View.WifiFriendly.Trainer trainer = team.trainers[k];
                    if (trainer.partyIDs.Contains(bEvent.pokemon.uniqueID))
                    {
                        Battle.View.WifiFriendly.Pokemon pokemon = trainer.GetPokemon(bEvent.pokemon.uniqueID);
                        if (pokemon != null)
                        {
                            isInModel = true;
                            pokemon.Update(bEvent.pokemon);
                        }

                        if (!isInModel)
                        {
                            pokemon = bEvent.pokemon;
                            trainer.party.Add(bEvent.pokemon);
                        }
                        yield return StartCoroutine(BattleAssetLoader.instance.LoadPokemon(pokemon));
                    }
                }
            }
            UpdateModel(myModel);
        }
        /// <summary>
        /// Updates references and loads all assets defined in the event, related to trainers.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_ModelUpdateTrainer(PBS.Battle.View.Events.ModelUpdateTrainer bEvent)
        {
            // Update references in the model
            bool isInModel = false;

            for (int i = 0; i < myModel.teams.Count && !isInModel; i++)
            {
                Battle.View.WifiFriendly.Team team = myModel.teams[i];
                if (team.teamID == bEvent.teamID)
                {
                    Battle.View.WifiFriendly.Trainer trainer = team.GetTrainer(bEvent.playerID);
                    if (trainer != null)
                    {
                        isInModel = true;
                        trainer.partyIDs = new List<string>(bEvent.party);
                    }

                    if (!isInModel)
                    {
                        isInModel = true;
                        trainer = new Battle.View.WifiFriendly.Trainer
                        {
                            name = bEvent.name,
                            playerID = bEvent.playerID,
                            teamPos = bEvent.teamID,
                            party = new List<Battle.View.WifiFriendly.Pokemon>(),
                            partyIDs = new List<string>(bEvent.party),
                            items = new List<string>(bEvent.items),
                            controlPos = new List<int>(bEvent.controlPos)
                        };
                        team.trainers.Add(trainer);
                    }
                    yield return StartCoroutine(BattleAssetLoader.instance.LoadTrainer(trainer));
                }
            }
            UpdateModel(myModel);
        }
        /// <summary>
        /// Updates references and loads all assets defined in the event, related to teams.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_ModelUpdateTeam(PBS.Battle.View.Events.ModelUpdateTeam bEvent)
        {
            // Update references in the model
            bool isInModel = false;

            for (int i = 0; i < myModel.teams.Count && !isInModel; i++)
            {
                Battle.View.WifiFriendly.Team team = myModel.teams[i];
                if (team.teamID == bEvent.teamID)
                {
                    isInModel = true;
                    team.playerIDs = new List<int>(bEvent.trainers);
                }
            }

            if (!isInModel)
            {
                Battle.View.WifiFriendly.Team team = new Battle.View.WifiFriendly.Team
                {
                    teamID = bEvent.teamID,
                    teamMode = bEvent.teamMode,
                    trainers = new List<Battle.View.WifiFriendly.Trainer>(),
                    playerIDs = new List<int>(bEvent.trainers)
                };
                myModel.teams.Add(team);
            }
            UpdateModel(myModel);
            yield return null;
        }
        #endregion

        #region Command Prompts
        /// <summary>
        /// Prompts the player to select commands according to the event definition.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_CommandGeneralPrompt(Battle.View.Events.CommandGeneralPrompt bEvent)
        {
            List<PBS.Player.Command> commands = new List<PBS.Player.Command>();
            yield return StartCoroutine(controls.HandlePromptCommands(
                bEvent: bEvent,
                (result) =>
                {
                    commands = new List<PBS.Player.Command>(result);
                }));
            CmdSendCommands(commands);
        }
        /// <summary>
        /// Prompts the player to replace pokemon according to the event definition.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_CommandReplacementPrompt(Battle.View.Events.CommandReplacementPrompt bEvent)
        {
            List<PBS.Player.Command> commands = new List<PBS.Player.Command>();
            yield return StartCoroutine(controls.HandlePromptReplace(
                bEvent: bEvent,
                (result) =>
                {
                    commands = new List<PBS.Player.Command>(result);
                }));
            CmdSendCommandsReplace(commands);
        }
        #endregion

        #region Trainer Interactions
        /// <summary>
        /// Displays an event for a trainer sending out pokemon to the field.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_TrainerSendOut(Battle.View.Events.TrainerSendOut bEvent)
        {
            string text = "";
            string pokemonNames = "";
            List<Battle.View.WifiFriendly.Pokemon> pokemon = new List<Battle.View.WifiFriendly.Pokemon>();

            for (int i = 0; i < bEvent.pokemonUniqueIDs.Count; i++)
            {
                Battle.View.WifiFriendly.Pokemon curPokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueIDs[i]);
                pokemon.Add(curPokemon);
            }
            pokemonNames = GetPokemonNames(pokemon);

            Battle.View.WifiFriendly.Trainer trainer = myModel.GetMatchingTrainer(bEvent.playerID);
            if (IsTrainerPlayer(bEvent.playerID))
            {
                text = "You sent out " + pokemonNames + "!";
            }
            else
            {
                text = trainer.name + " sent out " + pokemonNames + "!";
            }

            // Draw
            for (int i = 0; i < pokemon.Count; i++)
            {
                Battle.View.WifiFriendly.Pokemon curPokemon = pokemon[i];
                battleUI.DrawPokemonHUD(curPokemon, myTeamPerspective.teamMode, myTeamPerspective.teamID == trainer.teamPos);
                sceneCanvas.DrawPokemon(curPokemon, myTeamPerspective.teamMode, myTeamPerspective.teamID == trainer.teamPos);
            }
            yield return StartCoroutine(battleUI.DrawText(text));
        }
        /// <summary>
        /// Displays an event for trainers sending out their pokemon at the same time.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_TrainerMultiSendOut(Battle.View.Events.TrainerMultiSendOut bEvent)
        {
            List<Battle.View.Events.TrainerSendOut> enemySendEvents 
                = new List<Battle.View.Events.TrainerSendOut>();
            List<Battle.View.Events.TrainerSendOut> allySendEvents 
                = new List<Battle.View.Events.TrainerSendOut>();
            List<Battle.View.Events.TrainerSendOut> spectatorSendEvents 
                = new List<Battle.View.Events.TrainerSendOut>();
            Battle.View.Events.TrainerSendOut playerSendEvent = null;

            for (int i = 0; i < bEvent.sendEvents.Count; i++)
            {
                Battle.View.Events.TrainerSendOut sendEvent = bEvent.sendEvents[i];
                Battle.View.WifiFriendly.Trainer trainer = myModel.GetMatchingTrainer(sendEvent.playerID);
                Battle.View.WifiFriendly.Team perspective = myModel.GetTeamOfTrainer(trainer);

                if (myTrainer == null)
                {
                    spectatorSendEvents.Add(sendEvent);
                }
                else
                {
                    if (trainer.teamPos != myTrainer.teamPos)
                    {
                        enemySendEvents.Add(sendEvent);
                    }
                    else if (trainer.playerID != myTrainer.playerID)
                    {
                        allySendEvents.Add(sendEvent);
                    }
                    else
                    {
                        playerSendEvent = sendEvent;
                    }
                }
            }

            // run enemy send in
            for (int i = 0; i < enemySendEvents.Count; i++)
            {
                yield return StartCoroutine(ExecuteEvent_TrainerSendOut(enemySendEvents[i]));
            }

            // run ally send in
            for (int i = 0; i < allySendEvents.Count; i++)
            {
                yield return StartCoroutine(ExecuteEvent_TrainerSendOut(allySendEvents[i]));
            }

            // run send in events from spectator POV
            for (int i = 0; i < allySendEvents.Count; i++)
            {
                yield return StartCoroutine(ExecuteEvent_TrainerSendOut(spectatorSendEvents[i]));
            }

            // run player send in
            if (playerSendEvent != null)
            {
                yield return StartCoroutine(ExecuteEvent_TrainerSendOut(playerSendEvent));
            }

            yield return null;
        }
        /// <summary>
        /// Displays an event for the trainer withdrawing an on-field pokemon.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_TrainerWithdraw(Battle.View.Events.TrainerWithdraw bEvent)
        {
            string text = "";
            string pokemonNames = "";

            List<Battle.View.WifiFriendly.Pokemon> pokemon = new List<Battle.View.WifiFriendly.Pokemon>();
            for (int i = 0; i < bEvent.pokemonUniqueIDs.Count; i++)
            {
                Battle.View.WifiFriendly.Pokemon curPokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueIDs[i]);
                pokemon.Add(curPokemon);
            }
            pokemonNames = GetPokemonNames(pokemon);

            if (IsTrainerPlayer(bEvent.playerID))
            {
                text = "Come back, " + pokemonNames + "!";
            }
            else
            {
                Battle.View.WifiFriendly.Trainer trainer = myModel.GetMatchingTrainer(bEvent.playerID);
                text = trainer.name + " withdrew " + pokemonNames + "!";
            }

            // Undraw
            for (int i = 0; i < pokemon.Count; i++)
            {
                Battle.View.WifiFriendly.Pokemon curPokemon = pokemon[i];
                battleUI.UndrawPokemonHUD(curPokemon);
                sceneCanvas.UndrawPokemon(curPokemon.uniqueID);
            }
            yield return StartCoroutine(battleUI.DrawText(text));
        }
        /// <summary>
        /// Displays an event for the trainer using an item.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_TrainerItemUse(Battle.View.Events.TrainerItemUse bEvent)
        {
            string text = "";
            ItemData itemData = Items.instance.GetItemData(bEvent.itemID);
            if (IsTrainerPlayer(bEvent.playerID))
            {
                text = "You used one " + itemData.itemName + ".";
            }
            else
            {
                Battle.View.WifiFriendly.Trainer trainer = myModel.GetMatchingTrainer(bEvent.playerID);
                text = trainer.name + " used one " + itemData.itemName + ".";
            }
            yield return StartCoroutine(battleUI.DrawText(text));
        }
        #endregion

        #region Pokemon Interactions

        // General
        /// <summary>
        /// Displays an event for a Pokemon changing form. This also includes Transform, Mega-Evolution 
        /// and Gigantamaxing.
        /// </summary>
        /// <param name="bEvent">The form change event.</param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonChangeForm(Battle.View.Events.PokemonChangeForm bEvent)
        {
            Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            Battle.View.WifiFriendly.Trainer trainer = myModel.GetTrainer(pokemon);

            PokemonData preFormData = Pokemon.instance.GetPokemonData(bEvent.preForm);
            PokemonData postFormData = Pokemon.instance.GetPokemonData(bEvent.postForm);
            Debug.Log("DEBUG - " + pokemon.nickname + " changed from "
                + preFormData.speciesName + " (" + preFormData.formName + ") to "
                + postFormData.speciesName + " (" + postFormData.formName + ") ");

            // Undraw
            battleUI.UndrawPokemonHUD(pokemon);
            sceneCanvas.UndrawPokemon(pokemon.uniqueID);

            // Re-draw
            battleUI.DrawPokemonHUD(pokemon, myTeamPerspective.teamMode, myTeamPerspective.teamID == trainer.teamPos);
            sceneCanvas.DrawPokemon(pokemon, myTeamPerspective.teamMode, myTeamPerspective.teamID == trainer.teamPos);

            yield return null;
        }
        /// <summary>
        /// Displays an event for two pokemon switching their positions in battle (ex. Ally Switch).
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonSwitchPosition(Battle.View.Events.PokemonSwitchPosition bEvent)
        {
            Battle.View.WifiFriendly.Pokemon pokemon1 = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID1);
            Battle.View.WifiFriendly.Pokemon pokemon2 = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID2);
            Debug.Log($"{pokemon1.nickname} and {pokemon2.nickname} switched places!");
            yield return null;
        }


        // Damage / Health
        /// <summary>
        /// Animates an HP change for a pokemon.
        /// </summary>
        /// <param name="pokemon">The pokemon for which HP should be changed.</param>
        /// <param name="preHP">The amount of HP before modification.</param>
        /// <param name="postHP">The amount of HP after modification.</param>
        /// <param name="maxHP">The pokemon's maximum HP.</param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_Helper_PokemonHealthChange(
            Battle.View.WifiFriendly.Pokemon pokemon,
            int preHP,
            int postHP,
            int maxHP)
        {
            battleUI.SetPokemonHUDActive(pokemon, true);
            yield return new WaitForSeconds(0.25f);
            yield return StartCoroutine(battleUI.AnimatePokemonHUDHPChange(
                pokemon,
                preHP,
                postHP,
                maxHP
                ));
            yield return new WaitForSeconds(0.25f);
        }
        /// <summary>
        /// Displays an event for a pokemon taking damage.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonHealthDamage(Battle.View.Events.PokemonHealthDamage bEvent)
        {
            Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            int preHP = bEvent.preHP;
            int postHP = bEvent.postHP;

            string text = "";
            if (IsPokemonOwnedByPlayer(pokemon))
            {
                text = pokemon.nickname + " lost " + bEvent.damageDealt + " HP!";
            }
            else
            {
                text = pokemon.nickname + " lost HP!";
            }
            Debug.Log(text);

            yield return StartCoroutine(ExecuteEvent_Helper_PokemonHealthChange(
                pokemon: pokemon,
                preHP: bEvent.preHP,
                postHP: bEvent.postHP,
                maxHP: bEvent.maxHP
                ));
        }
        /// <summary>
        /// Displays an event for a pokemon being healed.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonHealthHeal(Battle.View.Events.PokemonHealthHeal bEvent)
        {
            PBS.Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            int preHP = bEvent.preHP;
            int postHP = bEvent.postHP;

            string text = "";
            if (IsPokemonOwnedByPlayer(pokemon))
            {
                text = pokemon.nickname + " recovered " + bEvent.hpHealed + " HP!";
            }
            else
            {
                text = pokemon.nickname + " recovered HP!";
            }
            Debug.Log(text);

            yield return StartCoroutine(ExecuteEvent_Helper_PokemonHealthChange(
                pokemon: pokemon,
                preHP: bEvent.preHP,
                postHP: bEvent.postHP,
                maxHP: bEvent.maxHP
                ));
        }
        /// <summary>
        /// Displays an event for a pokemon fainting.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonHealthFaint(Battle.View.Events.PokemonHealthFaint bEvent)
        {
            PBS.Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            yield return StartCoroutine(battleUI.DrawText(PBS.Battle.View.UI.Canvas.RenderMessage(
                new Battle.View.Events.MessageParameterized
                {
                    messageCode = "pokemon-faint",
                    pokemonTargetID = pokemon.uniqueID
                },
                myModel: myModel,
                myPlayerID: playerID,
                myTrainer: myTrainer,
                myTeamPerspective: myTeamPerspective
                )));

            // Undraw
            battleUI.UndrawPokemonHUD(pokemon);
            sceneCanvas.UndrawPokemon(pokemon.uniqueID);
        }
        /// <summary>
        /// Displays an event for a pokemon being revived.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonHealthRevive(Battle.View.Events.PokemonHealthRevive bEvent)
        {
            PBS.Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            yield return StartCoroutine(battleUI.DrawText($"{pokemon.nickname} was revived!"));
        }

        // Abilities
        /// <summary>
        /// Displays an ability being activated or used.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonAbilityActivate(PBS.Battle.View.Events.PokemonAbilityActivate bEvent)
        {
            PBS.Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            AbilityData abilityData = Abilities.instance.GetAbilityData(bEvent.abilityID);

            string prefix = PBS.Battle.View.UI.Canvas.GetPrefix(
                pokemon: pokemon, 
                myModel: myModel, 
                teamPerspectiveID: myTeamPerspective.teamID,
                myPlayerID: playerID);
            string pokemonName = PBS.Battle.View.UI.Canvas.GetPokemonName(pokemon, myModel);
            string text = $"({prefix}{pokemon.nickname}'s \\cyellow\\{abilityData.abilityName}\\c.\\)";
            yield return StartCoroutine(battleUI.DrawText(text));
        }

        // Moves
        /// <summary>
        /// Displays a move being used.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonMoveUse(PBS.Battle.View.Events.PokemonMoveUse bEvent)
        {
            PBS.Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            MoveData moveData = Moves.instance.GetMoveData(bEvent.moveID);

            string prefix = PBS.Battle.View.UI.Canvas.GetPrefix(
                pokemon: pokemon, 
                myModel: myModel, 
                teamPerspectiveID: myTeamPerspective.teamID,
                myPlayerID: playerID);
            string pokemonName = PBS.Battle.View.UI.Canvas.GetPokemonName(pokemon, myModel);
            yield return StartCoroutine(battleUI.DrawText($"{prefix}{pokemon.nickname} used {moveData.moveName}!"));
        }
        /// <summary>
        /// Displays a single hit of a move.
        /// </summary>
        /// <param name="bEvent">The event defining how targets were struck during this hit.</param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonMoveHit(Battle.View.Events.PokemonMoveHit bEvent)
        {
            Battle.View.WifiFriendly.Pokemon userPokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            MoveData moveData = Moves.instance.GetMoveData(bEvent.moveID);
            List<Battle.View.Events.PokemonMoveHitTarget> hitTargets = bEvent.hitTargets;

            List<Battle.View.WifiFriendly.Pokemon> missedPokemon = new List<Battle.View.WifiFriendly.Pokemon>();
            for (int i = 0; i < hitTargets.Count; i++)
            {
                if (hitTargets[i].missed)
                {
                    missedPokemon.Add(myModel.GetMatchingPokemon(hitTargets[i].pokemonUniqueID));
                }
            }

            // display missed pokemon
            if (missedPokemon.Count > 0)
            {
                if (myModel.settings.battleType == BattleType.Single)
                {
                    string missText = "It missed!";
                    yield return StartCoroutine(battleUI.DrawText(missText));
                }
                else
                {
                    List<Battle.View.WifiFriendly.Pokemon> enemyDodgers = FilterPokemonByPerspective(missedPokemon, Battle.View.Enums.ViewPerspective.Enemy);
                    if (enemyDodgers.Count > 0) 
                    {
                        string text = GetPrefix(Battle.View.Enums.ViewPerspective.Enemy) 
                            + GetPokemonNames(enemyDodgers) 
                            + " avoided the " 
                            + ((bEvent.currentHit == 1) ? "attack" : "hit") + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> allyDodgers = FilterPokemonByPerspective(missedPokemon, Battle.View.Enums.ViewPerspective.Ally);
                    if (allyDodgers.Count > 0)
                    {
                        string text = GetPrefix(Battle.View.Enums.ViewPerspective.Ally)
                            + GetPokemonNames(allyDodgers)
                            + " avoided the "
                            + ((bEvent.currentHit == 1) ? "attack" : "hit") + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> playerDodgers = FilterPokemonByPerspective(missedPokemon, Battle.View.Enums.ViewPerspective.Player);
                    if (playerDodgers.Count > 0)
                    {
                        string text = GetPrefix(Battle.View.Enums.ViewPerspective.Player)
                            + GetPokemonNames(playerDodgers)
                            + " avoided the "
                            + ((bEvent.currentHit == 1) ? "attack" : "hit") + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }
                }
            }
            else if (bEvent.currentHit == 1
                && hitTargets.Count == 0
                && missedPokemon.Count == 0)
            {
                string text = "But there was no target...";
                yield return StartCoroutine(battleUI.DrawText(text));
            }

            // display immune pokemon
            List<Battle.View.WifiFriendly.Pokemon> immunePokemon = new List<Battle.View.WifiFriendly.Pokemon>();
            for (int i = 0; i < hitTargets.Count; i++)
            {
                if (hitTargets[i].affectedByMove && hitTargets[i].effectiveness == 0)
                {
                    immunePokemon.Add(myModel.GetMatchingPokemon(hitTargets[i].pokemonUniqueID));
                }
            }
            if (immunePokemon.Count > 0)
            {
                if (myModel.settings.battleType == BattleType.Single)
                {
                    string text = "It had no effect...";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
                else
                {
                    string prefixTxt = "It had no effect on ";

                    List<Battle.View.WifiFriendly.Pokemon> enemyImmune = FilterPokemonByPerspective(immunePokemon, Battle.View.Enums.ViewPerspective.Enemy);
                    if (enemyImmune.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Enemy, true)
                            + GetPokemonNames(enemyImmune, true)
                            + "...";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> allyImmune = FilterPokemonByPerspective(immunePokemon, Battle.View.Enums.ViewPerspective.Ally);
                    if (allyImmune.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Ally, true)
                            + GetPokemonNames(allyImmune, true)
                            + "...";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> playerImmune = FilterPokemonByPerspective(immunePokemon, Battle.View.Enums.ViewPerspective.Player);
                    if (playerImmune.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Player, true)
                            + GetPokemonNames(playerImmune, true)
                            + "...";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }
                }
            }

            // Animate HP Loss
            List<Coroutine> hpRoutines = new List<Coroutine>();
            // TODO: Come back to edit HP Bars
            for (int i = 0; i < hitTargets.Count; i++)
            {
                Battle.View.Events.PokemonMoveHitTarget curTarget = hitTargets[i];
                Battle.View.WifiFriendly.Pokemon curPokemon = myModel.GetMatchingPokemon(curTarget.pokemonUniqueID);
                if (curTarget.affectedByMove && curTarget.damageDealt >= 0)
                {
                    Coroutine cr = StartCoroutine(ExecuteEvent_Helper_PokemonHealthChange(
                        pokemon: curPokemon,
                        preHP: curTarget.preHP,
                        postHP: curTarget.postHP,
                        maxHP: curTarget.maxHP
                        ));
                    hpRoutines.Add(cr);
                }
            }
            for (int i = 0; i < hpRoutines.Count; i++)
            {
                yield return hpRoutines[i];
            }

            // Critical Hits / Effectiveness
            List<Battle.View.WifiFriendly.Pokemon> criticalTargets = new List<Battle.View.WifiFriendly.Pokemon>();
            List<Battle.View.WifiFriendly.Pokemon> superEffTargets = new List<Battle.View.WifiFriendly.Pokemon>();
            List<Battle.View.WifiFriendly.Pokemon> nveEffTargets = new List<Battle.View.WifiFriendly.Pokemon>();
    
            for (int i = 0; i < hitTargets.Count; i++)
            {
                Battle.View.Events.PokemonMoveHitTarget curTarget = hitTargets[i];
                Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(curTarget.pokemonUniqueID);
                if (curTarget.affectedByMove)
                {
                    if (curTarget.criticalHit)
                    {
                        criticalTargets.Add(pokemon);
                    }
                    if (curTarget.effectiveness > 1)
                    {
                        superEffTargets.Add(pokemon);
                    }
                    else if (curTarget.effectiveness < 1)
                    {
                        nveEffTargets.Add(pokemon);
                    }
                }
            }
            if (criticalTargets.Count > 0)
            {
                if (myModel.settings.battleType == BattleType.Single)
                {
                    string text = "A critical hit!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
                else
                {
                    string prefixTxt = "It was a critical hit on ";
                    List<Battle.View.WifiFriendly.Pokemon> enemyPokemon = FilterPokemonByPerspective(criticalTargets, Battle.View.Enums.ViewPerspective.Enemy);
                    if (enemyPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(PBS.Battle.View.Enums.ViewPerspective.Enemy, true)
                            + GetPokemonNames(enemyPokemon)
                            + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> allyPokemon = FilterPokemonByPerspective(criticalTargets, Battle.View.Enums.ViewPerspective.Ally);
                    if (allyPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Ally, true)
                            + GetPokemonNames(allyPokemon)
                            + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> playerPokemon = FilterPokemonByPerspective(criticalTargets, Battle.View.Enums.ViewPerspective.Player);
                    if (playerPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Player, true)
                            + GetPokemonNames(playerPokemon)
                            + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }
                }
            }
            if (superEffTargets.Count > 0)
            {
                if (myModel.settings.battleType == BattleType.Single)
                {
                    string text = "It was super-effective!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
                else
                {
                    string prefixTxt = "It was super-effective on ";
                    List<Battle.View.WifiFriendly.Pokemon> enemyPokemon = FilterPokemonByPerspective(superEffTargets, Battle.View.Enums.ViewPerspective.Enemy);
                    if (enemyPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Enemy, true)
                            + GetPokemonNames(enemyPokemon)
                            + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> allyPokemon = FilterPokemonByPerspective(superEffTargets, Battle.View.Enums.ViewPerspective.Ally);
                    if (allyPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Ally, true)
                            + GetPokemonNames(allyPokemon)
                            + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> playerPokemon = FilterPokemonByPerspective(superEffTargets, Battle.View.Enums.ViewPerspective.Player);
                    if (playerPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Player, true)
                            + GetPokemonNames(playerPokemon)
                            + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }
                }
            }
            if (nveEffTargets.Count > 0)
            {
                if (myModel.settings.battleType == BattleType.Single)
                {
                    string text = "It was not very effective.";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
                else
                {
                    string prefixTxt = "It was not very effective on ";
                    List<Battle.View.WifiFriendly.Pokemon> enemyPokemon = FilterPokemonByPerspective(nveEffTargets, Battle.View.Enums.ViewPerspective.Enemy);
                    if (enemyPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Enemy, true)
                            + GetPokemonNames(enemyPokemon)
                            + ".";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> allyPokemon = FilterPokemonByPerspective(nveEffTargets, Battle.View.Enums.ViewPerspective.Ally);
                    if (allyPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Ally, true)
                            + GetPokemonNames(allyPokemon)
                            + ".";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }

                    List<Battle.View.WifiFriendly.Pokemon> playerPokemon = FilterPokemonByPerspective(nveEffTargets, Battle.View.Enums.ViewPerspective.Player);
                    if (playerPokemon.Count > 0)
                    {
                        string text = prefixTxt
                            + GetPrefix(Battle.View.Enums.ViewPerspective.Player, true)
                            + GetPokemonNames(playerPokemon)
                            + ".";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }
                }
            }
        }

        // Stats
        /// <summary>
        /// Displays a stat stage modification event.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonStatChange(Battle.View.Events.PokemonStatChange bEvent)
        {
            Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            string statString = Battle.View.UI.Canvas.ConvertStatsToString(bEvent.statsToMod.ToArray());
            string modString = (bEvent.maximize)? "was maximized!"
                : (bEvent.minimize)? "was minimized!"
                : (bEvent.modValue == 1) ? "rose!"
                : (bEvent.modValue == 2) ? "harshly rose!"
                : (bEvent.modValue >= 3) ? "drastically rose!"
                : (bEvent.modValue == -1) ? "fell!"
                : (bEvent.modValue == -2) ? "harshly fell!"
                : (bEvent.modValue <= -3) ? "drastically fell!"
                : "";
            string prefix = Battle.View.UI.Canvas.GetPrefix(
                pokemon: pokemon, 
                myModel: myModel, 
                teamPerspectiveID: myTeamPerspective.teamID,
                myPlayerID: playerID);
            string pokemonName = Battle.View.UI.Canvas.GetPokemonName(pokemon, myModel);
            yield return StartCoroutine(battleUI.DrawText($"{prefix}{pokemon.nickname}'s {statString} {modString}"));
        }
        /// <summary>
        /// Displays a stat stage modification failure event.
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_PokemonStatUnchangeable(Battle.View.Events.PokemonStatUnchangeable bEvent)
        {
            Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            string statString = ConvertStatsToString(bEvent.statsToMod.ToArray());
            string modString = (bEvent.tooHigh)? "cannot go any higher!" : " cannot go any lower!";
            yield return StartCoroutine(battleUI.DrawText($"{pokemon.nickname}'s {statString} {modString}"));
        }

        // Items

        // Status
        #endregion

        #region To Be Legacy
        public PBS.Battle.View.Enums.ViewPerspective GetPerspective(Battle.View.WifiFriendly.Pokemon pokemon)
        {
            PBS.Battle.View.WifiFriendly.Trainer trainer = myModel.GetTrainer(pokemon);
            PBS.Battle.View.WifiFriendly.Team team = myModel.GetTeamOfTrainer(trainer);
            if (team.teamID != myTeamPerspective.teamID)
            {
                return PBS.Battle.View.Enums.ViewPerspective.Enemy;
            }
            else
            {
                if (myTrainer == null)
                {
                    return PBS.Battle.View.Enums.ViewPerspective.Ally;
                }
                return PBS.Battle.View.Enums.ViewPerspective.Player;
            }
        }
        public List<PBS.Battle.View.WifiFriendly.Pokemon> FilterPokemonByPerspective(List<PBS.Battle.View.WifiFriendly.Pokemon> pokemon, PBS.Battle.View.Enums.ViewPerspective viewPerspective)
        {
            List<PBS.Battle.View.WifiFriendly.Pokemon> filteredPokemon = new List<PBS.Battle.View.WifiFriendly.Pokemon>();
            for (int i = 0; i < pokemon.Count; i++)
            {
                if (GetPerspective(pokemon[i]) == viewPerspective)
                {
                    filteredPokemon.Add(pokemon[i]);
                }
            }
            return filteredPokemon;
        }
        public string GetPrefix(PBS.Battle.View.Enums.ViewPerspective viewPerspective, bool lowercase = false)
        {
            string prefix = (viewPerspective == PBS.Battle.View.Enums.ViewPerspective.Ally) ? "The ally "
                : (viewPerspective == PBS.Battle.View.Enums.ViewPerspective.Enemy) ? 
                    (myModel.settings.isWildBattle? "The wild " : "The foe's ")
                : "";
            if (lowercase)
            {
                prefix = prefix.ToLower();
            }

            return prefix;
        }
        public string GetPrefix(PBS.Battle.View.WifiFriendly.Pokemon pokemon, bool capitalize = true)
        {
            string text = "";
            PBS.Battle.View.WifiFriendly.Trainer trainer = myModel.GetTrainer(pokemon);
            if (pokemon.teamPos != myTeamPerspective.teamID)
            {
                text = "The opposing ";
            }
            else
            {
                if (myTrainer != null)
                {
                    if (trainer.playerID != myTrainer.playerID)
                    {
                        text = "The ally ";
                    }
                }
            }
            if (!capitalize)
            {
                text = text.ToLower();
                text = " " + text;
            }
            return text;
        }
        public string GetPokemonName(PBS.Battle.View.WifiFriendly.Pokemon pokemon)
        {
            return GetPokemonNames(new List<PBS.Battle.View.WifiFriendly.Pokemon> { pokemon });
        }
        public string GetPokemonNames(List<PBS.Battle.View.WifiFriendly.Pokemon> pokemonList, bool orConjunct = false)
        {
            string conjunct = (orConjunct) ? "or" : "and";

            string names = "";
            if (pokemonList.Count == 1)
            {
                return pokemonList[0].nickname;
            }
            else if (pokemonList.Count == 2)
            {
                return pokemonList[0].nickname + " " + conjunct + " " + pokemonList[1].nickname;
            }
            else
            {
                for (int i = 0; i < pokemonList.Count; i++)
                {
                    names += (i == pokemonList.Count - 1) ?
                        conjunct + " " + pokemonList[i].nickname :
                        pokemonList[i].nickname + ", ";
                }
            }
            return names;
        }
        private string GetTrainerNames(List<PBS.Battle.View.WifiFriendly.Trainer> trainers)
        {
            string text = "";
            for (int i = 0; i < trainers.Count; i++)
            {
                text += (i == 0)? trainers[i].name : " and " + trainers[i].name;
            }
            return text;
        }

        private static string ConvertStatToString(PokemonStats stat, bool capitalize = true)
        {
            return (stat == PokemonStats.Attack) ? "Attack"
                : (stat == PokemonStats.Defense) ? "Defense"
                : (stat == PokemonStats.SpecialAttack) ? "Special Attack"
                : (stat == PokemonStats.SpecialDefense) ? "Special Defense"
                : (stat == PokemonStats.Speed) ? "Speed"
                : (stat == PokemonStats.Accuracy) ? "Accuracy"
                : (stat == PokemonStats.Evasion) ? "Evasion"
                : "HP";
        }
        private static string ConvertStatsToString(PokemonStats[] statList, bool capitalize = true)
        {
            if (statList.Length == 7)
            {
                string s = "Stats";
                s = (capitalize) ? s : s.ToLower();
                return s;
            }

            string text = "";
            if (statList.Length == 1)
            {
                return ConvertStatToString(statList[0], capitalize);
            }
            else if (statList.Length == 2)
            {
                return ConvertStatToString(statList[0], capitalize) 
                    + " and " 
                    + ConvertStatToString(statList[1], capitalize);
            }
            else
            {
                for (int i = 0; i < statList.Length; i++)
                {
                    text += (i == statList.Length - 1) ? "and " + ConvertStatToString(statList[i], capitalize) 
                        : ConvertStatToString(statList[i], capitalize) + ", ";
                }
            }
            return text;
        }

        private string RenderMessageTrainer(int playerID, int teamPerspectiveID = -1, string baseString = "")
        {
            if (teamPerspectiveID == -1)
            {
                teamPerspectiveID = myTeamPerspective.teamID;
            }
            PBS.Battle.View.WifiFriendly.Trainer trainer = myModel.GetMatchingTrainer(playerID);
            GameTextData textData = 
                (trainer.teamPos != teamPerspectiveID)? GameTextDatabase.instance.GetGameTextData("trainer-perspective-opposing")
                : (myTrainer == null)? GameTextDatabase.instance.GetGameTextData("trainer-perspective-ally")
                : GameTextDatabase.instance.GetGameTextData("trainer-perspective-player");

            string replaceString = textData.languageDict[GameSettings.language];
            string replaceStringPoss = replaceString;
            if (!string.IsNullOrEmpty(baseString))
            {
                if (GameSettings.language == GameLanguages.English && IsTrainerPlayer(trainer))
                {
                    if (!baseString.StartsWith("{{-trainer-"))
                    {
                        replaceString = replaceString.ToLower();
                        replaceStringPoss = replaceStringPoss.ToLower();
                    }
                }
            }
           
            string newString = baseString;
            newString = newString.Replace("{{-trainer-}}", replaceString);
            newString = newString.Replace("{{-trainer-poss-}}", replaceStringPoss);

            return newString;
        }
        private string RenderMessageTeam(int teamID, int teamPerspectiveID = -1, string baseString = "")
        {
            if (teamPerspectiveID == -1)
            {
                teamPerspectiveID = myTeamPerspective.teamID;
            }
            PBS.Battle.View.WifiFriendly.Team team = myModel.GetMatchingTeam(teamID);
            GameTextData textData = 
                (team.teamID != teamPerspectiveID)? GameTextDatabase.instance.GetGameTextData("team-perspective-opposing")
                : (myTrainer == null)? GameTextDatabase.instance.GetGameTextData("team-perspective-ally")
                : GameTextDatabase.instance.GetGameTextData("team-perspective-player");

            string teamString = textData.languageDict[GameSettings.language];
            if (!string.IsNullOrEmpty(baseString))
            {
                if (GameSettings.language == GameLanguages.English)
                {
                    if (!baseString.StartsWith("{{-target-team-"))
                    {
                        teamString = teamString.ToLower();
                    }
                }
            }
            string newString = baseString;
            newString = newString.Replace("{{-target-team-}}", teamString);
            newString = newString.Replace("{{-target-team-poss-}}", teamString
                + (teamString.EndsWith("s") ? "'" : "'s")
                );

            return newString;
        }
        public string RenderMessage(PBS.Battle.View.Events.MessageParameterized message)
        {
            GameTextData textData = GameTextDatabase.instance.GetGameTextData(message.messageCode);
            if (textData == null)
            {
                return "";
            }
            string baseString = textData.languageDict[GameSettings.language];
            string newString = baseString;

            PBS.Battle.View.WifiFriendly.Trainer trainerPerspective = 
                (myTrainer == null)? myModel.GetMatchingTrainer(message.playerPerspectiveID)
                : myTrainer;
            PBS.Battle.View.WifiFriendly.Team teamPerspective = 
                (myTeamPerspective == null)? myModel.GetMatchingTeam(message.teamPerspectiveID)
                : myTeamPerspective;

            // player
            newString = newString.Replace("{{-player-name-}}", PlayerSave.instance.name);

            if (!string.IsNullOrEmpty(message.pokemonID))
            {
                PBS.Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonID);
                newString = newString.Replace("{{-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonUserID))
            {
                PBS.Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonUserID);
                newString = newString.Replace("{{-user-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-user-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonTargetID))
            {
                PBS.Battle.View.WifiFriendly.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonTargetID);
                newString = newString.Replace("{{-target-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-target-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (message.pokemonListIDs.Count > 0)
            {
                List<PBS.Battle.View.WifiFriendly.Pokemon> pokemonList = new List<Battle.View.WifiFriendly.Pokemon>();
                for (int i = 0; i < message.pokemonListIDs.Count; i++)
                {
                    pokemonList.Add(myModel.GetMatchingPokemon(message.pokemonListIDs[i]));
                }
                string pokemonNameList = GetPokemonNames(pokemonList);
                newString = newString.Replace("{{-pokemon-list-}}", pokemonNameList);
            }

            if (message.trainerID != 0)
            {
                newString = RenderMessageTrainer(message.trainerID, teamPerspective.teamID, newString);
            }
            
            if (message.teamID != 0)
            {
                newString = RenderMessageTeam(message.teamID, teamPerspective.teamID, newString);
            }

            if (!string.IsNullOrEmpty(message.typeID))
            {
                TypeData typeData = ElementalTypes.instance.GetTypeData(message.typeID);
                newString = newString.Replace("{{-type-name-}}", typeData.typeName + "-type");
            }
            if (message.typeIDs.Count > 0)
            {
                newString = newString.Replace("{{-type-list-}}", GameTextDatabase.ConvertTypesToString(message.typeIDs.ToArray()));
            }

            if (!string.IsNullOrEmpty(message.moveID))
            {
                MoveData moveData = Moves.instance.GetMoveData(message.moveID);
                newString = newString.Replace("{{-move-name-}}", moveData.moveName);
            }
            if (message.moveIDs.Count > 0)
            {
                for (int i = 0; i < message.moveIDs.Count; i++)
                {
                    MoveData moveXData = Moves.instance.GetMoveData(message.moveIDs[i]);
                    string partToReplace = "{{-move-name-" + i + "-}}";
                    newString = newString.Replace(partToReplace, moveXData.moveName);
                }
            }

            if (!string.IsNullOrEmpty(message.abilityID))
            {
                AbilityData abilityData = Abilities.instance.GetAbilityData(message.abilityID);
                newString = newString.Replace("{{-ability-name-}}", abilityData.abilityName);
            }
            if (message.abilityIDs.Count > 0)
            {
                for (int i = 0; i < message.abilityIDs.Count; i++)
                {
                    AbilityData abilityXData = Abilities.instance.GetAbilityData(message.abilityIDs[i]);
                    string partToReplace = "{{-ability-name-" + i + "-}}";
                    newString = newString.Replace(partToReplace, abilityXData.abilityName);
                }
            }

            if (!string.IsNullOrEmpty(message.itemID))
            {
                ItemData itemData = Items.instance.GetItemData(message.itemID);
                newString = newString.Replace("{{-item-name-}}", itemData.itemName);
            }

            if (!string.IsNullOrEmpty(message.statusID))
            {
                StatusPKData statusData = StatusPKDatabase.instance.GetStatusData(message.statusID);
                newString = newString.Replace("{{-status-name-}}", statusData.conditionName);
            }
            if (!string.IsNullOrEmpty(message.statusTeamID))
            {
                StatusTEData statusData = StatusTEDatabase.instance.GetStatusData(message.statusTeamID);
                newString = newString.Replace("{{-status-name-}}", statusData.conditionName);
            }
            if (!string.IsNullOrEmpty(message.statusEnvironmentID))
            {
                StatusBTLData statusData = StatusBTLDatabase.instance.GetStatusData(message.statusEnvironmentID);
                newString = newString.Replace("{{-status-name-}}", statusData.conditionName);
            }

            // swapping substrings
            for (int i = 0; i < message.intArgs.Count; i++)
            {
                string partToReplace = "{{-int-" + i + "-}}";
                newString = newString.Replace(partToReplace, message.intArgs[i].ToString());
            }



            return newString;
        }
        #endregion
    }
}



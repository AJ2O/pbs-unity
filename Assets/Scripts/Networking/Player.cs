using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking { 

    public class Player : NetworkBehaviour
    {
        // Player View
        public PBS.Battle.View.Compact.Trainer myTrainer;
        public PBS.Battle.View.Compact.Team myTeamPerspective;
        public PBS.Battle.View.Model myModel;

        // Events
        bool isExecutingEvents = true;
        Coroutine eventExecutor;
        List<PBS.Battle.View.Events.Base> eventQueue;

        private void Awake()
        {
            myTrainer = null;
            myTeamPerspective = null;
            myModel = null;
        }

        /// <summary>
        /// TODO: Initialize view perspective here
        /// </summary>
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

        // 3.
        /// <summary>
        /// TODO: Initialize this player's view here
        /// </summary>
        public void SetComponents()
        {
            // UI
            // Scene
        }

        public bool IsTrainerPlayer(int playerID)
        {
            if (myTrainer != null)
            {
                return playerID == myTrainer.playerID;
            }
            return false;
        }
        public bool IsTrainerPlayer(PBS.Battle.View.Compact.Trainer trainer)
        {
            return IsTrainerPlayer(trainer.playerID);
        }
        public bool IsPokemonOwnedByPlayer(PBS.Battle.View.Compact.Pokemon pokemon)
        {
            if (myTrainer != null)
            {
                PBS.Battle.View.Compact.Trainer ownerTrainer = myModel.GetTrainer(pokemon);
                if (ownerTrainer != null)
                {
                    return ownerTrainer.playerID == myTrainer.playerID;
                }
            }
            return false;
        }

        // 4.
        [Command]
        public void CmdSyncTrainerToServer()
        {
            PBS.Static.Master.instance.networkManager.AddPlayer(this.connectionToClient, this);
            PBS.Static.Master.instance.networkManager.AddTrainer(this.connectionToClient);
        }
        [ClientRpc]
        public void RpcSyncTrainerToClient(PBS.Battle.View.Compact.Trainer trainer)
        {
            myTrainer = trainer;
        }
        [ClientRpc]
        public void RpcSyncTeamPerspectiveToClient(PBS.Battle.View.Compact.Team perspective)
        {
            myTeamPerspective = perspective;
        }

        // 7.
        [TargetRpc]
        public void TargetReceiveEvent(PBS.Battle.View.Events.Base bEvent)
        {
            eventQueue.Add(bEvent);
        }

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
                : (bEvent is PBS.Battle.View.Events.MessagePokemon)? 
                ExecuteEvent_MessagePokemon(bEvent as PBS.Battle.View.Events.MessagePokemon)
                : (bEvent is PBS.Battle.View.Events.MessageTrainer)? 
                ExecuteEvent_MessageTrainer(bEvent as PBS.Battle.View.Events.MessageTrainer)
                : (bEvent is PBS.Battle.View.Events.MessageTeam)? 
                ExecuteEvent_MessageTeam(bEvent as PBS.Battle.View.Events.MessageTeam)


                // Backend
                : (bEvent is PBS.Battle.View.Events.ModelUpdate)? 
                ExecuteEvent_ModelUpdate(bEvent as PBS.Battle.View.Events.ModelUpdate)


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
                : (bEvent is PBS.Battle.View.Events.EnvironmentalConditionStart)? 
                ExecuteEvent_EnvironmentalConditionStart(bEvent as PBS.Battle.View.Events.EnvironmentalConditionStart)
                : (bEvent is PBS.Battle.View.Events.EnvironmentalConditionEnd)? 
                ExecuteEvent_EnvironmentalConditionEnd(bEvent as PBS.Battle.View.Events.EnvironmentalConditionEnd)


                // --- Pokemon Interactions ---

                // General

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
                : (bEvent is PBS.Battle.View.Events.PokemonAbilityQuickDraw)? 
                ExecuteEvent_PokemonAbilityQuickDraw(bEvent as PBS.Battle.View.Events.PokemonAbilityQuickDraw)
                : (bEvent is PBS.Battle.View.Events.PokemonAbilityActivate)? 
                ExecuteEvent_PokemonAbilityActivate(bEvent as PBS.Battle.View.Events.PokemonAbilityActivate)

                // Moves
                : (bEvent is PBS.Battle.View.Events.PokemonMoveUse)? 
                ExecuteEvent_PokemonMoveUse(bEvent as PBS.Battle.View.Events.PokemonMoveUse)

                // Stats
                : (bEvent is PBS.Battle.View.Events.PokemonStatChange)? 
                ExecuteEvent_PokemonStatChange(bEvent as PBS.Battle.View.Events.PokemonStatChange)
                : (bEvent is PBS.Battle.View.Events.PokemonStatUnchangeable)? 
                ExecuteEvent_PokemonStatUnchangeable(bEvent as PBS.Battle.View.Events.PokemonStatUnchangeable)

                // Items
                : (bEvent is PBS.Battle.View.Events.PokemonItemQuickClaw)? 
                ExecuteEvent_PokemonItemQuickClaw(bEvent as PBS.Battle.View.Events.PokemonItemQuickClaw)

                // Status

                // Misc
                : (bEvent is PBS.Battle.View.Events.PokemonMiscProtect)? 
                ExecuteEvent_PokemonMiscProtect(bEvent as PBS.Battle.View.Events.PokemonMiscProtect)
                : (bEvent is PBS.Battle.View.Events.PokemonMiscMatBlock)? 
                ExecuteEvent_PokemonMiscMatBlock(bEvent as PBS.Battle.View.Events.PokemonMiscMatBlock)

                // Unhandled

                : ExecuteEvent_Unhandled(bEvent)
                );

            yield return null;
        }

        // Unhandled Events
        public IEnumerator ExecuteEvent_Unhandled(PBS.Battle.View.Events.Base bEvent)
        {
            Debug.LogWarning("Received unknown event type");
            yield return null;
        }


        // Battle Phases
        /// <summary>
        /// TODO: Description
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_StartBattle(PBS.Battle.View.Events.StartBattle bEvent)
        {
            // Get Ally Trainers
            string allyString = "";
            if (myTrainer != null)
            {
                // Not a spectator
                allyString += "You";
                List<PBS.Battle.View.Compact.Trainer> allyTrainers = myModel.GetTrainerAllies(myTrainer);
                for (int i = 0; i < allyTrainers.Count; i++)
                {
                    allyString += " and " + allyTrainers[i].name;
                }
            }

            // Get Enemy Trainers
            string enemyString = "";
            List<PBS.Battle.View.Compact.Trainer> enemyTrainers = myModel.GetTrainerEnemies(myTeamPerspective);
            for (int i = 0; i < enemyTrainers.Count; i++)
            {
                enemyString += (i == 0)? enemyTrainers[i].name : " and " + enemyTrainers[i].name;
            }

            // Challenge Statement
            string challengeString = " were challenged by ";

            // End Statement
            string endString = "!";

            Debug.Log($"{allyString}{challengeString}{enemyString}{endString}");
            yield return null;
        }

        /// <summary>
        /// TODO: Description
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_EndBattle(PBS.Battle.View.Events.EndBattle bEvent)
        {
            if (bEvent.winningTeam < 0)
            {
                Debug.Log("The battle ended in a draw!");
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

                if (myTeamPerspective.teamPos != bEvent.winningTeam)
                {
                    resultString = " lost to ";
                    endString = "...";
                }
                Debug.Log($"{allyString}{resultString}{enemyString}{endString}");
            }

            yield return null;
        }


        // Messages
        /// <summary>
        /// TODO: Use dialog box
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_Message(PBS.Battle.View.Events.Message bEvent)
        {
            Debug.Log(bEvent.message);
            yield return null;
        }
        /// <summary>
        /// TODO: Use dialog box
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_MessageParameterized(PBS.Battle.View.Events.MessageParameterized bEvent)
        {
            Debug.Log(RenderMessage(bEvent));
            yield return null;
        }
        /// <summary>
        /// TODO: Use dialog box
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_MessagePokemon(PBS.Battle.View.Events.MessagePokemon bEvent)
        {
            List<PBS.Battle.View.Compact.Pokemon> pokemon = new List<Battle.View.Compact.Pokemon>();
            for (int i = 0; i < bEvent.pokemonUniqueIDs.Count; i++)
            {
                pokemon.Add(myModel.GetMatchingPokemon(bEvent.pokemonUniqueIDs[i]));
            }
            string pokemonNames = GetPokemonNames(pokemon);

            Debug.Log($"{bEvent.preMessage}{pokemonNames}{bEvent.postMessage}");
            yield return null;
        }
        /// <summary>
        /// TODO: Use dialog box
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_MessageTrainer(PBS.Battle.View.Events.MessageTrainer bEvent)
        {
            List<PBS.Battle.View.Compact.Trainer> trainers = new List<Battle.View.Compact.Trainer>();
            for (int i = 0; i < bEvent.playerIDs.Count; i++)
            {
                trainers.Add(myModel.GetMatchingTrainer(bEvent.playerIDs[i]));
            }
            string trainerString = GetTrainerNames(trainers);

            Debug.Log($"{bEvent.preMessage}{trainerString}{bEvent.postMessage}");
            yield return null;
        }
        /// <summary>
        /// TODO: Use dialog box
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_MessageTeam(PBS.Battle.View.Events.MessageTeam bEvent)
        {
            PBS.Battle.View.Compact.Team team = myModel.GetMatchingTeam(bEvent.teamID);
            string teamString = (team.teamPos == myTeamPerspective.teamPos)? "The ally" : "The opposing";
            if (myTrainer != null)
            {
                teamString = "Your";
            };
            if (string.IsNullOrEmpty(bEvent.preMessage))
            {
                teamString = teamString.ToLower();
            }

            Debug.Log($"{bEvent.preMessage}{teamString}{bEvent.postMessage}");
            yield return null;
        }

        // Backend
        /// <summary>
        /// TODO: Description
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_ModelUpdate(PBS.Battle.View.Events.ModelUpdate bEvent)
        {
            // Update references in the model
            myModel = bEvent.model;

            switch (bEvent.updateType)
            {
                case Battle.View.Events.ModelUpdate.UpdateType.LoadAssets:
                    Debug.Log("Loading battle assets...");
                    yield return StartCoroutine(BattleAssetLoader.instance.LoadBattleAssets(bEvent.model));
                    break;

                default:
                    break;
            }
        }


        // Trainer Interactions
        /// <summary>
        /// TODO: Description
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_TrainerSendOut(PBS.Battle.View.Events.TrainerSendOut bEvent)
        {
            string text = "";
            string pokemonNames = "";

            for (int i = 0; i < bEvent.pokemonUniqueIDs.Count; i++)
            {
                PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueIDs[i]);
                pokemonNames += (i == 0)? pokemon.nickname : " and " + pokemon.nickname;
            }

            if (IsTrainerPlayer(bEvent.playerID))
            {
                text = "You sent out " + pokemonNames + "!";
            }
            else
            {
                PBS.Battle.View.Compact.Trainer trainer = myModel.GetMatchingTrainer(bEvent.playerID);
                text = trainer.name + " sent out " + pokemonNames + "!";
            }

            Debug.Log(text);
            yield return null;
        }
        /// <summary>
        /// TODO: Description
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_TrainerMultiSendOut(PBS.Battle.View.Events.TrainerMultiSendOut bEvent)
        {
            List<PBS.Battle.View.Events.TrainerSendOut> enemySendEvents 
                = new List<PBS.Battle.View.Events.TrainerSendOut>();
            List<PBS.Battle.View.Events.TrainerSendOut> allySendEvents 
                = new List<PBS.Battle.View.Events.TrainerSendOut>();
            List<PBS.Battle.View.Events.TrainerSendOut> spectatorSendEvents 
                = new List<PBS.Battle.View.Events.TrainerSendOut>();
            PBS.Battle.View.Events.TrainerSendOut playerSendEvent = null;

            for (int i = 0; i < bEvent.sendEvents.Count; i++)
            {
                PBS.Battle.View.Events.TrainerSendOut sendEvent = bEvent.sendEvents[i];
                PBS.Battle.View.Compact.Trainer trainer = myModel.GetMatchingTrainer(sendEvent.playerID);
                PBS.Battle.View.Compact.Team perspective = myModel.GetTeamOfTrainer(trainer);

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
        /// TODO: Description
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_TrainerWithdraw(PBS.Battle.View.Events.TrainerWithdraw bEvent)
        {
            string text = "";
            string pokemonNames = "";

            for (int i = 0; i < bEvent.pokemonUniqueIDs.Count; i++)
            {
                PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueIDs[i]);
                pokemonNames += (i == 0)? pokemon.nickname : " and " + pokemon.nickname;
            }

            if (IsTrainerPlayer(bEvent.playerID))
            {
                text = "Come back, " + pokemonNames + "!";
            }
            else
            {
                PBS.Battle.View.Compact.Trainer trainer = myModel.GetMatchingTrainer(bEvent.playerID);
                text = trainer.name + " withdrew " + pokemonNames + "!";
            }

            Debug.Log(text);
            yield return null;
        }
        /// <summary>
        /// TODO: Description
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_TrainerItemUse(PBS.Battle.View.Events.TrainerItemUse bEvent)
        {
            string text = "";
            ItemData itemData = ItemDatabase.instance.GetItemData(bEvent.itemID);
            if (IsTrainerPlayer(bEvent.playerID))
            {
                text = "You used one " + itemData.itemName + ".";
            }
            else
            {
                PBS.Battle.View.Compact.Trainer trainer = myModel.GetMatchingTrainer(bEvent.playerID);
                text = trainer.name + " used one " + itemData.itemName + ".";
            }

            Debug.Log(text);
            yield return null;
        }

        // Environmental Interactions
        /// <summary>
        /// TODO: Use dialog box
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_EnvironmentalConditionStart(PBS.Battle.View.Events.EnvironmentalConditionStart bEvent)
        {
            StatusBTLData data = StatusBTLDatabase.instance.GetStatusData(bEvent.conditionID);

            Debug.Log($"{data.conditionName} started!");
            yield return null;
        }

        /// <summary>
        /// TODO: Use dialog box
        /// </summary>
        /// <param name="bEvent"></param>
        /// <returns></returns>
        public IEnumerator ExecuteEvent_EnvironmentalConditionEnd(PBS.Battle.View.Events.EnvironmentalConditionEnd bEvent)
        {
            StatusBTLData data = StatusBTLDatabase.instance.GetStatusData(bEvent.conditionID);

            Debug.Log($"{data.conditionName} ended!");
            yield return null;
        }


        // --- Pokemon Interactions ---

        // General

        // Damage / Health
        public IEnumerator ExecuteEvent_PokemonHealthDamage(PBS.Battle.View.Events.PokemonHealthDamage bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            int preHP = bEvent.preHP;
            int postHP = bEvent.postHP;

            string text = "";
            if (IsPokemonOwnedByPlayer(pokemon))
            {
                text = pokemon.nickname + " lost " + bEvent.damageDealt + " HP!";
            }
            else
            {
                text = pokemon.nickname = " lost HP!";
            }

            Debug.Log(text);
            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonHealthHeal(PBS.Battle.View.Events.PokemonHealthHeal bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            int preHP = bEvent.preHP;
            int postHP = bEvent.postHP;

            string text = "";
            if (IsPokemonOwnedByPlayer(pokemon))
            {
                text = pokemon.nickname + " recovered " + bEvent.hpHealed + " HP!";
            }
            else
            {
                text = pokemon.nickname = " recovered HP!";
            }

            Debug.Log(text);
            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonHealthFaint(PBS.Battle.View.Events.PokemonHealthFaint bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            Debug.Log($"{pokemon.nickname} fainted!");
            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonHealthRevive(PBS.Battle.View.Events.PokemonHealthRevive bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            Debug.Log($"{pokemon.nickname} was revived!");
            yield return null;
        }

        // Abilities
        public IEnumerator ExecuteEvent_PokemonAbilityActivate(PBS.Battle.View.Events.PokemonAbilityActivate bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            AbilityData abilityData = AbilityDatabase.instance.GetAbilityData(bEvent.abilityID);
            Debug.Log($"{pokemon.nickname}'s {abilityData.abilityName}");

            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonAbilityQuickDraw(PBS.Battle.View.Events.PokemonAbilityQuickDraw bEvent)
        {
            yield return StartCoroutine(ExecuteEvent_PokemonAbilityActivate(bEvent));
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            Debug.Log($"{pokemon.nickname} moved first!");

            yield return null;
        }

        // Moves
        public IEnumerator ExecuteEvent_PokemonMoveUse(PBS.Battle.View.Events.PokemonMoveUse bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            MoveData moveData = MoveDatabase.instance.GetMoveData(bEvent.moveID);
            Debug.Log($"{pokemon.nickname} used {moveData.moveName}!");

            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonMoveCelebrate(PBS.Battle.View.Events.PokemonMoveCelebrate bEvent)
        {
            PBS.Battle.View.Compact.Trainer trainerToCelebrate = (myTrainer != null)? myTrainer
                : myModel.GetTrainers()[0];

            Debug.Log($"Congratulations {trainerToCelebrate.name}!");

            yield return null;
        }

        // Stats
        public IEnumerator ExecuteEvent_PokemonStatChange(PBS.Battle.View.Events.PokemonStatChange bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            string statString = ConvertStatsToString(bEvent.statsToMod.ToArray());
            string modString = (bEvent.maximize)? "was maximized!"
                : (bEvent.minimize)? "was minimized!"
                : (bEvent.modValue == 1) ? "rose!"
                : (bEvent.modValue == 2) ? "harshly rose!"
                : (bEvent.modValue >= 3) ? "drastically rose!"
                : (bEvent.modValue == -1) ? "fell!"
                : (bEvent.modValue == -2) ? "harshly fell!"
                : (bEvent.modValue <= -3) ? "drastically fell!"
                : "";
            Debug.Log($"{pokemon.nickname}'s {statString} {modString}");
            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonStatUnchangeable(PBS.Battle.View.Events.PokemonStatUnchangeable bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            string statString = ConvertStatsToString(bEvent.statsToMod.ToArray());
            string modString = (bEvent.tooHigh)? "cannot go any higher!" : " cannot go any lower!";
            Debug.Log($"{pokemon.nickname}'s {statString} {modString}");
            yield return null;
        }

        // Items
        public IEnumerator ExecuteEvent_PokemonItemQuickClaw(PBS.Battle.View.Events.PokemonItemQuickClaw bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            ItemData itemData = ItemDatabase.instance.GetItemData(bEvent.itemID);
            Debug.Log($"{pokemon.nickname}'s {itemData.itemName} activated!");

            yield return null;
        }

        // Status

        // Misc
        public IEnumerator ExecuteEvent_PokemonMiscProtect(PBS.Battle.View.Events.PokemonMiscProtect bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            Debug.Log($"{pokemon.nickname} protected itself!");

            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonMiscMatBlock(PBS.Battle.View.Events.PokemonMiscMatBlock bEvent)
        {
            PBS.Battle.View.Compact.Team team = myModel.GetMatchingTeam(bEvent.teamID);
            string teamString = (team.teamPos == myTeamPerspective.teamPos)? "The ally" : "The opposing";
            if (myTrainer != null)
            {
                teamString = "Your";
            };

            Debug.Log($"{teamString} team is being protected!");

            yield return null;
        }











        // Helpers
        private string GetPokemonNames(List<PBS.Battle.View.Compact.Pokemon> pokemon)
        {
            string text = "";
            for (int i = 0; i < pokemon.Count; i++)
            {
                text += (i == 0)? pokemon[i].nickname : " and " + pokemon[i].nickname;
            }
            return text;
        }
        private string GetTrainerNames(List<PBS.Battle.View.Compact.Trainer> trainers)
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
                teamPerspectiveID = myTeamPerspective.teamPos;
            }
            PBS.Battle.View.Compact.Trainer trainer = myModel.GetMatchingTrainer(playerID);
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
                teamPerspectiveID = myTeamPerspective.teamPos;
            }
            PBS.Battle.View.Compact.Team team = myModel.GetMatchingTeam(teamID);
            GameTextData textData = 
                (team.teamPos != teamPerspectiveID)? GameTextDatabase.instance.GetGameTextData("team-perspective-opposing")
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
        private string RenderMessage(PBS.Battle.View.Events.MessageParameterized message)
        {
            GameTextData textData = GameTextDatabase.instance.GetGameTextData(message.messageCode);
            if (textData == null)
            {
                return "";
            }
            string baseString = textData.languageDict[GameSettings.language];
            string newString = baseString;

            PBS.Battle.View.Compact.Trainer trainerPerspective = 
                (myTrainer == null)? myModel.GetMatchingTrainer(message.playerPerspectiveID)
                : myTrainer;
            PBS.Battle.View.Compact.Team teamPerspective = 
                (myTeamPerspective == null)? myModel.GetMatchingTeam(message.teamPerspectiveID)
                : myTeamPerspective;

            // player
            newString = newString.Replace("{{-player-name-}}", PlayerSave.instance.name);

            if (!string.IsNullOrEmpty(message.pokemonID))
            {
                PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonID);
                newString = newString.Replace("{{-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonUserID))
            {
                PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonUserID);
                newString = newString.Replace("{{-user-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-user-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (!string.IsNullOrEmpty(message.pokemonTargetID))
            {
                PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(message.pokemonTargetID);
                newString = newString.Replace("{{-target-pokemon-}}", pokemon.nickname);
                newString = newString.Replace("{{-target-pokemon-poss-}}", pokemon.nickname
                    + ((pokemon.nickname.EndsWith("s")) ? "'" : "'s")
                    );
            }
            if (message.pokemonListIDs.Count > 0)
            {
                List<PBS.Battle.View.Compact.Pokemon> pokemonList = new List<Battle.View.Compact.Pokemon>();
                for (int i = 0; i < message.pokemonListIDs.Count; i++)
                {
                    pokemonList.Add(myModel.GetMatchingPokemon(message.pokemonListIDs[i]));
                }
                string pokemonNameList = GetPokemonNames(pokemonList);
                newString = newString.Replace("{{-pokemon-list-}}", pokemonNameList);
            }

            if (message.trainerID != 0)
            {
                newString = RenderMessageTrainer(message.trainerID, teamPerspective.teamPos, newString);
            }
            
            if (message.teamID != 0)
            {
                newString = RenderMessageTeam(message.teamID, teamPerspective.teamPos, newString);
            }

            if (!string.IsNullOrEmpty(message.typeID))
            {
                TypeData typeData = TypeDatabase.instance.GetTypeData(message.typeID);
                newString = newString.Replace("{{-type-name-}}", typeData.typeName + "-type");
            }
            if (message.typeIDs.Count > 0)
            {
                newString = newString.Replace("{{-type-list-}}", GameTextDatabase.ConvertTypesToString(message.typeIDs.ToArray()));
            }

            if (!string.IsNullOrEmpty(message.moveID))
            {
                MoveData moveData = MoveDatabase.instance.GetMoveData(message.moveID);
                newString = newString.Replace("{{-move-name-}}", moveData.moveName);
            }
            if (message.moveIDs.Count > 0)
            {
                for (int i = 0; i < message.moveIDs.Count; i++)
                {
                    MoveData moveXData = MoveDatabase.instance.GetMoveData(message.moveIDs[i]);
                    string partToReplace = "{{-move-name-" + i + "-}}";
                    newString = newString.Replace(partToReplace, moveXData.moveName);
                }
            }

            if (!string.IsNullOrEmpty(message.abilityID))
            {
                AbilityData abilityData = AbilityDatabase.instance.GetAbilityData(message.abilityID);
                newString = newString.Replace("{{-ability-name-}}", abilityData.abilityName);
            }
            if (message.abilityIDs.Count > 0)
            {
                for (int i = 0; i < message.abilityIDs.Count; i++)
                {
                    AbilityData abilityXData = AbilityDatabase.instance.GetAbilityData(message.abilityIDs[i]);
                    string partToReplace = "{{-ability-name-" + i + "-}}";
                    newString = newString.Replace(partToReplace, abilityXData.abilityName);
                }
            }

            if (!string.IsNullOrEmpty(message.itemID))
            {
                ItemData itemData = ItemDatabase.instance.GetItemData(message.itemID);
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
    }
}



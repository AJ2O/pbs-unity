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


                // Backend
                : (bEvent is PBS.Battle.View.Events.ModelUpdate)? 
                ExecuteEvent_ModelUpdate(bEvent as PBS.Battle.View.Events.ModelUpdate)


                // Trainer Interactions
                : (bEvent is PBS.Battle.View.Events.TrainerSendOut)? 
                ExecuteEvent_TrainerSendOut(bEvent as PBS.Battle.View.Events.TrainerSendOut)
                : (bEvent is PBS.Battle.View.Events.TrainerMultiSendOut)? 
                ExecuteEvent_TrainerMultiSendOut(bEvent as PBS.Battle.View.Events.TrainerMultiSendOut)


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

                // Stats

                // Items


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
                pokemonNames += (i == 0)? pokemon.nickame : " and " + pokemon.nickame;
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
                text = pokemon.nickame + " lost " + bEvent.damageDealt + " HP!";
            }
            else
            {
                text = pokemon.nickame = " lost HP!";
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
                text = pokemon.nickame + " recovered " + bEvent.hpHealed + " HP!";
            }
            else
            {
                text = pokemon.nickame = " recovered HP!";
            }

            Debug.Log(text);
            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonHealthFaint(PBS.Battle.View.Events.PokemonHealthFaint bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            Debug.Log($"{pokemon.nickame} fainted!");
            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonHealthRevive(PBS.Battle.View.Events.PokemonHealthRevive bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            Debug.Log($"{pokemon.nickame} was revived!");
            yield return null;
        }

        // Abilities
        public IEnumerator ExecuteEvent_PokemonAbilityActivate(PBS.Battle.View.Events.PokemonAbilityActivate bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            AbilityData abilityData = AbilityDatabase.instance.GetAbilityData(bEvent.abilityID);
            Debug.Log($"{pokemon.nickame}'s {abilityData.abilityName}");

            yield return null;
        }
        public IEnumerator ExecuteEvent_PokemonAbilityQuickDraw(PBS.Battle.View.Events.PokemonAbilityQuickDraw bEvent)
        {
            yield return StartCoroutine(ExecuteEvent_PokemonAbilityActivate(bEvent));
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            Debug.Log($"{pokemon.nickame} moved first!");

            yield return null;
        }

        // Moves

        // Stats

        // Items
        public IEnumerator ExecuteEvent_PokemonItemQuickClaw(PBS.Battle.View.Events.PokemonItemQuickClaw bEvent)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = myModel.GetMatchingPokemon(bEvent.pokemonUniqueID);
            ItemData itemData = ItemDatabase.instance.GetItemData(bEvent.itemID);
            Debug.Log($"{pokemon.nickame}'s {itemData.itemName} activated!");

            yield return null;
        }












        // Helpers
        private string GetPokemonNames(List<PBS.Battle.View.Compact.Pokemon> pokemon)
        {
            string text = "";
            for (int i = 0; i < pokemon.Count; i++)
            {
                text += (i == 0)? pokemon[i].nickame : " and " + pokemon[i].nickame;
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

    }
}



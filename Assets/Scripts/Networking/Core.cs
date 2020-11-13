using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking {

    public class Core : NetworkBehaviour
    {
        [HideInInspector] public Battle.Core.Model battle;
        public bool isRunningBattle;
        Coroutine battleCoroutine;
        Coroutine waitCoroutine;
        List<BattleCommand> allCommands = new List<BattleCommand>();
        List<BattleCommand> replaceCommands = new List<BattleCommand>();
        bool executedFormTransformations = false;

        public void InitializeComponents()
        {
            allCommands = new List<BattleCommand>();
            isRunningBattle = true;
        }
        public void FinishComponents()
        {
            allCommands.Clear();
            battle = null;
            isRunningBattle = false;
        }

        // Player Events
        public void SendEvent(PBS.Battle.View.Events.Base bEvent)
        {
            List<PBS.Networking.Player> playerObjs = PBS.Static.Master.instance.networkManager.GetAllPlayers();
            for (int i = 0; i < playerObjs.Count; i++)
            {
                playerObjs[i].TargetReceiveEvent(bEvent);
            }
        }
        public void SendEventToPlayer(PBS.Battle.View.Events.Base bEvent, int playerID)
        {
            PBS.Networking.Player playerObj = PBS.Static.Master.instance.networkManager.GetPlayer(playerID);
            if (playerObj != null)
            {
                playerObj.TargetReceiveEvent(bEvent);
            }
        }
        public void SendEvents(List<PBS.Battle.View.Events.Base> bEvents)
        {
            for (int i = 0; i < bEvents.Count; i++)
            {
                SendEvent(bEvents[i]);
            }
        }
        public void UpdateClients()
        {
            SendEvent(new PBS.Battle.View.Events.ModelUpdate 
            { 
                model = new PBS.Battle.View.Model(battle), 
                updateType = PBS.Battle.View.Events.ModelUpdate.UpdateType.None 
            });
        }

        // 6.
        public IEnumerator StartBattle(
            BattleSettings battleSettings, 
            List<BattleTeam> teams)
        {
            InitializeComponents();
            battle = new Battle.Core.Model(battleSettings: battleSettings, turns: 0, teams: teams);
            
            // load assets on player clients
            SendEvent(new PBS.Battle.View.Events.ModelUpdate 
            { 
                model = new PBS.Battle.View.Model(battle), 
                updateType = PBS.Battle.View.Events.ModelUpdate.UpdateType.LoadAssets 
            });

            // send start message
            SendEvent(new PBS.Battle.View.Events.StartBattle());

            // force out pokemon
            Dictionary<Trainer, List<Pokemon>> sentOutMap = battle.ForceSendAllPokemon();
            List<PBS.Battle.View.Events.TrainerSendOut> sendEvents = new List<PBS.Battle.View.Events.TrainerSendOut>();
            List<Trainer> trainers = new List<Trainer>(sentOutMap.Keys);
            for (int i = 0; i < trainers.Count; i++)
            {
                Trainer trainer = trainers[i];
                PBS.Battle.View.Events.TrainerSendOut sendEvent = new Battle.View.Events.TrainerSendOut
                {
                    playerID = trainer.playerID,
                    pokemonUniqueIDs = new List<string>()
                };
                List<Pokemon> sentPokemon = sentOutMap[trainer];
                for (int j = 0; j < sentPokemon.Count; j++)
                {
                    sendEvent.pokemonUniqueIDs.Add(sentPokemon[j].uniqueID);
                }
                sendEvents.Add(sendEvent);
                SendEvent(sendEvent);
            }
            
            PBS.Battle.View.Events.TrainerMultiSendOut multiSendEvent = new Battle.View.Events.TrainerMultiSendOut
            {
                sendEvents = sendEvents
            };
            
            UpdateClients();
            // TODO: Come back to multi-send event
            //SendEvent(multiSendEvent);

            // Run Starting notifications

            /*/ Initial Weather / Terrain / etc.
            List<BattleCondition> initialBConditions = battle.BBPGetSCs();
            for (int i = 0; i < initialBConditions.Count; i++)
            {
                string natureText = initialBConditions[i].data.natureTextID;
                if (!string.IsNullOrEmpty(natureText))
                {
                    BTLEvent_GameText textEvent = new BTLEvent_GameText();
                    textEvent.Create(
                        textID: initialBConditions[i].data.natureTextID
                        );
                    SendEvent(textEvent);
                }
            }*/


            // Initial Team Effects


            // Initial Abilities
            //yield return StartCoroutine(PBPRunEnterAbilities(battle.pokemonOnField));

            // enter battle loop
            //yield return StartCoroutine(BattleLoop());

            // end battle
            //yield return StartCoroutine(EndBattle());
            yield return null;
        }



    }
}


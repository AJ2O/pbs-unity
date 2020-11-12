using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace PBS.Networking {

    public class Core : NetworkBehaviour
    {
        [HideInInspector] public Battle.Core.Model model;
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
            model = null;
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



        // 6.
        public IEnumerator StartBattle(
            BattleSettings battleSettings, 
            List<BattleTeam> teams)
        {
            InitializeComponents();
            model = new Battle.Core.Model(battleSettings: battleSettings, turns: 0, teams: teams);
            
            // load assets on player clients
            SendEvent(new PBS.Battle.View.Events.ModelUpdate(
                model: model, 
                updateType: PBS.Battle.View.Events.ModelUpdate.UpdateType.LoadAssets));

            // send start message
            SendEvent(new PBS.Battle.View.Events.StartBattle(model));

            // force out pokemon
            /*Dictionary<Trainer, List<Pokemon>> sentOutMap = battle.ForceSendAllPokemon();
            Battle.Core.Model startModel = Battle.CloneModel(battle);

            BTLEvent_SendOut[] sendEvents = new BTLEvent_SendOut[sentOutMap.Count];
            List<Trainer> trainers = new List<Trainer>(sentOutMap.Keys);
            for (int i = 0; i < trainers.Count; i++)
            {
                sendEvents[i] = new BTLEvent_SendOut();
                sendEvents[i].SetBattleModel(startModel);
                sendEvents[i].Create(trainers[i], sentOutMap[trainers[i]]);
            }

            BTLEvent_ForceOut forceOutEvent = new BTLEvent_ForceOut();
            forceOutEvent.SetBattleModel(startModel);
            forceOutEvent.Create(sendEvents);
            yield return StartCoroutine(SendEventAndWait(forceOutEvent));

            // Run Starting notifications

            // Initial Weather / Terrain / etc.
            List<BattleCondition> initialBConditions = model.BBPGetSCs();
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
            }


            // Initial Team Effects


            // Initial Abilities
            yield return StartCoroutine(PBPRunEnterAbilities(battle.pokemonOnField));

            // enter battle loop
            yield return StartCoroutine(BattleLoop());

            // end battle
            yield return StartCoroutine(EndBattle());*/
            yield return null;
        }



    }
}


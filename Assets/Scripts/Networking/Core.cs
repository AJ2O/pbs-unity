using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using PBS.Battle;

namespace PBS.Networking
{

    public class Core : NetworkBehaviour
    {
        [HideInInspector] public Model battle;
        [HideInInspector] public PBS.AI.Battle ai;
        public bool isRunningBattle;
        Coroutine battleCoroutine;
        Coroutine waitCoroutine;

        HashSet<int> waitConnections = new HashSet<int>();
        List<BattleCommand> allCommands = new List<BattleCommand>();
        List<BattleCommand> replaceCommands = new List<BattleCommand>();
        List<BattleCommand> batonPassReplaceCommands = new List<BattleCommand>();
        bool executingBatonPass = false;
        bool executedFormTransformations = false;

        public void InitializeComponents()
        {
            ai = new AI.Battle();
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
                SendEventToPlayer(bEvent, playerObjs[i].playerID);
            }
        }
        public void SendEventToPlayer(PBS.Battle.View.Events.Base bEvent, int playerID)
        {
            PBS.Networking.Player playerObj = PBS.Static.Master.instance.networkManager.GetPlayer(playerID);
            if (playerObj != null)
            {
                NetworkIdentity opponentIdentity = playerObj.GetComponent<NetworkIdentity>();
                playerObj.TargetReceiveEvent(opponentIdentity.connectionToClient, bEvent);
            }
        }
        public void SendEvents(List<PBS.Battle.View.Events.Base> bEvents)
        {
            for (int i = 0; i < bEvents.Count; i++)
            {
                SendEvent(bEvents[i]);
            }
        }
        public void InitializeClients(bool loadAssets = true)
        {
            UpdateClients(true, true, true, true);
            if (loadAssets)
            {
                SendEvent(new Battle.View.Events.ModelUpdate());
            }
        }
        public void UpdateClients()
        {
            UpdateClients(updateTeam: true, updateTrainer: true, updatePokemon: true);
        }
        public void UpdateClients(
            bool updateTeam = false,
            bool updateTrainer = false,
            bool updatePokemon = false,
            bool loadAsset = false
            )
        {
            // Send all teams
            if (updateTeam)
            {
                List<BattleTeam> teams = new List<BattleTeam>(battle.teams);
                for (int i = 0; i < teams.Count; i++)
                {
                    SendEvent(new Battle.View.Events.ModelUpdateTeam(teams[i], loadAsset));
                }
            }

            // Send all trainers
            if (updateTrainer)
            {
                List<Trainer> trainers = battle.GetTrainers();
                for (int i = 0; i < trainers.Count; i++)
                {
                    SendEvent(new Battle.View.Events.ModelUpdateTrainer(trainers[i], loadAsset));
                }
            }

            // Send all pokemon
            if (updatePokemon)
            {
                List<Pokemon> pokemon = battle.GetPokemonFromAllTrainers();
                for (int i = 0; i < pokemon.Count; i++)
                {
                    SendEvent(new Battle.View.Events.ModelUpdatePokemon
                    {
                        pokemon = new Battle.View.WifiFriendly.Pokemon(pokemon[i]),
                        loadAsset = loadAsset
                    });
                }
            }
        }

        // Player Queries
        public void ReceiveQuery(PBS.Player.Query.QueryBase query, int playerID)
        {
            PBS.Player.Query.QueryResponseBase response;

            if (query is PBS.Player.Query.MoveTarget moveTarget)
            {
                Pokemon pokemon = battle.GetBattleInstanceOfPokemon(moveTarget.pokemonUniqueID);
                response = new PBS.Player.Query.MoveTargetResponse
                {
                    possibleTargets = battle.GetMovePossibleTargets(pokemon, MoveDatabase.instance.GetMoveData(moveTarget.moveID))
                };
            }
            // default
            else
            {
                response = new PBS.Player.Query.QueryResponseBase
                {

                };
            }

            SendQueryResponse(response, playerID);
        }
        public void SendQueryResponse(PBS.Player.Query.QueryResponseBase response, int playerID)
        {
            PBS.Networking.Player playerObj = PBS.Static.Master.instance.networkManager.GetPlayer(playerID);
            if (playerObj != null)
            {
                NetworkIdentity opponentIdentity = playerObj.GetComponent<NetworkIdentity>();
                playerObj.TargetReceiveQueryResponse(opponentIdentity.connectionToClient, response);
            }
        }

        public void ReceiveCommandQuery(PBS.Player.Command command, List<PBS.Player.Command> committedCommands)
        {
            int playerID = command.commandTrainer;
            PBS.Battle.View.Events.MessageParameterized response = battle.CanChooseCommand(
                attemptedCommand: battle.SanitizeCommand(command),
                committedCommands: battle.SanitizeCommands(committedCommands));
            SendCommandQueryResponse(response, playerID);
        }
        public void SendCommandQueryResponse(PBS.Battle.View.Events.MessageParameterized response, int playerID)
        {
            PBS.Networking.Player playerObj = PBS.Static.Master.instance.networkManager.GetPlayer(playerID);
            if (playerObj != null)
            {
                NetworkIdentity opponentIdentity = playerObj.GetComponent<NetworkIdentity>();
                playerObj.TargetReceiveQueryMessageResponse(opponentIdentity.connectionToClient, response);
            }
        }

        // 6.
        public IEnumerator StartBattle(
            BattleSettings battleSettings, 
            List<BattleTeam> teams)
        {
            InitializeComponents();
            battle = new PBS.Battle.Model(battleSettings: battleSettings, turns: 0, teams: teams);

            // load assets on player clients
            InitializeClients();

            // send start message
            SendEvent(new PBS.Battle.View.Events.StartBattle());

            // force out pokemon
            Dictionary<Trainer, List<Pokemon>> sentOutMap = battle.ForceSendAllPokemon();
            List<PBS.Battle.View.Events.TrainerSendOut> sendEvents = new List<PBS.Battle.View.Events.TrainerSendOut>();
            List<Trainer> trainers = new List<Trainer>(sentOutMap.Keys);
            UpdateClients();
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
            // TODO: Come back to multi-send event
            //SendEvent(multiSendEvent);

            // Run Starting notifications

            // Initial Weather / Terrain / etc.
            List<BattleCondition> initialBConditions = battle.BBPGetSCs();
            for (int i = 0; i < initialBConditions.Count; i++)
            {
                string natureText = initialBConditions[i].data.natureTextID;
                if (!string.IsNullOrEmpty(natureText))
                {
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = natureText,
                        statusEnvironmentID = initialBConditions[i].statusID
                    });
                }
            }


            // TODO: Initial Team Effects


            // Initial Abilities
            yield return StartCoroutine(PBPRunEnterAbilities(battle.pokemonOnField));

            // enter battle loop
            yield return StartCoroutine(BattleLoop());

            // end battle
            yield return StartCoroutine(EndBattle());
        }

        // 11.
        private IEnumerator EndBattle()
        {
            Debug.Log("Battle Finished (Server)");

            // Revert all forms
            List<Pokemon> allPokemon = battle.GetPokemonFromAllTrainers();
            for (int i = 0; i < allPokemon.Count; i++)
            {
                if (allPokemon[i].data.HasTag(PokemonTag.RevertOnBattleEnd))
                {
                    battle.PBPRevertForm(allPokemon[i]);
                }
            }
            UpdateClients();

            BattleTeam winningTeam = battle.GetWinningTeam();
            int winningTeamNo = (winningTeam == null) ? -1 : winningTeam.teamID;
            SendEvent(new Battle.View.Events.EndBattle
            {
                winningTeam = winningTeamNo
            });
            
            FinishComponents();
            yield return null;
        }

        private IEnumerator BattleLoop()
        {
            // loop while battle isn't finished
            while (!battle.IsFinished())
            {
                // advance turn
                yield return StartCoroutine(BattleAdvanceTurn());

                // run commmand segment
                yield return StartCoroutine(BattleCommandCycle());

                // run end of turn events
                yield return StartCoroutine(BattleEndTurn());
            
                // replacement step
                yield return StartCoroutine(BattleReplacement());

                // clear command list at the end of the turn
                allCommands.Clear();

                yield return null;
            }
        }
        private IEnumerator BattleAdvanceTurn()
        {
            // advance pokemon turns
            List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
            for (int i = 0; i < allPokemon.Count; i++)
            {
                battle.AdvanceTurnPokemon(allPokemon[i]);
            }

            // advance team status turns
            for (int i = 0; i < battle.teams.Count; i++)
            {
                battle.AdvanceTurnTeam(battle.teams[i]);
            }

            // advance battle status turns
            battle.AdvanceTurn();
            yield return null;
        }
        private IEnumerator BattleCommandCycle()
        {
            allCommands = new List<BattleCommand>();
            UpdateClients();
            
            // send command prompts to all player trainers
            List<int> playerIDs = new List<int>(PBS.Static.Master.instance.networkManager.trainerConnections.Keys);
            for (int i = 0; i < playerIDs.Count; i++)
            {
                int playerID = playerIDs[i];
                Trainer trainer = battle.GetTrainerWithID(playerID);
                if (trainer != null)
                {
                    List<PBS.Battle.View.Events.CommandAgent> pokemonAgents = battle.GetTrainerCommandableAgents(trainer);
                    
                    // only send to trainers with commandable pokemon
                    if (pokemonAgents.Count > 0)
                    {
                        List<string> items = new List<string>();
                        for (int k = 0; k < trainer.items.Count; k++)
                        {
                            items.Add(trainer.items[k].itemID);
                        }

                        PBS.Battle.View.Events.CommandGeneralPrompt commandPrompt = new Battle.View.Events.CommandGeneralPrompt
                        {
                            playerID = playerID,
                            multiTargetSelection = !battle.IsSinglesBattle(),
                            canMegaEvolve = !trainer.bProps.usedMegaEvolution,
                            canZMove = !trainer.bProps.usedZMove,
                            canDynamax = !trainer.bProps.usedDynamax,
                            items = items,
                            pokemonToCommand = pokemonAgents
                        };
                        waitConnections.Add(playerID);
                        SendEventToPlayer(commandPrompt, playerID);
                    }
                }
            }

            // wait for players to confirm commands
            Debug.Log("Waiting for players...");
            yield return StartCoroutine(WaitForPlayer());

            // TODO: AI Commands
            ai.UpdateModel(Model.CloneModel(battle));
            SelectAICommands();

            // select all preset commands
            yield return StartCoroutine(SelectPresetCommands());

            // resort all commands
            allCommands = battle.ReorderCommands(allCommands);

            // run pre-command events (ex. Beak Blast, Focus Punch)
            executedFormTransformations = false;
            List<BattleCommand> quickClawCommands = new List<BattleCommand>();
            yield return StartCoroutine(ExecuteQuickClawCommands(
                callback: (result) => { quickClawCommands.AddRange(result); }
                ));
            yield return StartCoroutine(ExecutePreCommands());

            // execute all commands

            // Party
            List<BattleCommand> partyCommands = battle.GetFilteredCommands(BattleCommandType.Party, allCommands);
            yield return StartCoroutine(ExecuteAllCommands(partyCommands));

            // Bag
            List<BattleCommand> bagCommands = battle.GetFilteredCommands(BattleCommandType.Bag, allCommands);
            yield return StartCoroutine(ExecuteAllCommands(bagCommands));

            // Run
            List<BattleCommand> runCommands = battle.GetFilteredCommands(BattleCommandType.Run, allCommands);
            runCommands.AddRange(battle.GetFilteredCommands(BattleCommandType.GiveUp, allCommands));
            runCommands = battle.ReorderCommands(runCommands);
            yield return StartCoroutine(ExecuteAllCommands(runCommands));

            // Moves
            yield return StartCoroutine(ExecuteAllCommands(quickClawCommands));
            List<BattleCommand> moveCommands = battle.GetFilteredCommands(BattleCommandType.Fight, allCommands);
            moveCommands.AddRange(battle.GetFilteredCommands(BattleCommandType.Recharge, allCommands));
            moveCommands = battle.ReorderCommands(moveCommands);
            yield return StartCoroutine(ExecuteAllCommands(moveCommands));
        }
        private IEnumerator SelectPresetCommands()
        {
            allCommands.AddRange(battle.GetPresetCommands());
            yield return null;
        }
        private IEnumerator ExecuteQuickClawCommands(System.Action<List<BattleCommand>> callback)
        {
            List<BattleCommand> quickCommands = new List<BattleCommand>();

            for (int i = 0; i < allCommands.Count; i++)
            {
                BattleCommand command = allCommands[i];
                Pokemon userPokemon = command.commandUser;
                if (command.commandType == BattleCommandType.Fight)
                {
                    if (battle.IsPokemonOnFieldAndAble(userPokemon))
                    {
                        MoveData pkMoveData = battle.GetPokemonMoveData(
                            moveID: command.moveID,
                            userPokemon: userPokemon,
                            command: command);


                        bool activatedQuickClaw = false;
                        // Quick Draw
                        List<Pokemon.Ability> pbAbilities = battle.PBPGetAbilitiesWithEffect(
                            pokemon: userPokemon,
                            effectType: AbilityEffectType.QuickDraw);
                        for (int k = 0; k < pbAbilities.Count && !activatedQuickClaw; k++)
                        {
                            Pokemon.Ability ability = pbAbilities[k];
                            EffectDatabase.AbilityEff.AbilityEffect quickDraw_ =
                                ability.data.GetEffectNew(AbilityEffectType.QuickDraw);
                            if (quickDraw_ != null)
                            {
                                EffectDatabase.AbilityEff.QuickDraw quickDraw =
                                    quickDraw_ as EffectDatabase.AbilityEff.QuickDraw;
                                if (battle.DoEffectFiltersPass(
                                    filters: quickDraw.filters,
                                    targetPokemon: userPokemon,
                                    moveData: pkMoveData
                                    ))
                                {
                                    if (Random.value <= quickDraw.chance)
                                    {
                                        activatedQuickClaw = true;
                                        PBPShowAbility(userPokemon, ability);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = quickDraw.displayText,
                                            pokemonUserID = userPokemon.uniqueID,
                                            abilityID = ability.abilityID
                                        });
                                    }
                                }
                            }
                        }

                        // Quick Claw
                        List<Item> pbItems = battle.PBPGetItemsWithEffect(
                            pokemon: userPokemon,
                            effectType: ItemEffectType.QuickClaw);
                        for (int k = 0; k < pbItems.Count && !activatedQuickClaw; k++)
                        {
                            Item item = pbItems[k];
                            EffectDatabase.ItemEff.ItemEffect quickClaw_ =
                                item.data.GetEffectNew(ItemEffectType.QuickClaw);
                            if (quickClaw_ != null)
                            {
                                EffectDatabase.ItemEff.QuickClaw quickClaw =
                                    quickClaw_ as EffectDatabase.ItemEff.QuickClaw;
                                if (battle.DoEffectFiltersPass(
                                    filters: quickClaw.filters,
                                    targetPokemon: userPokemon,
                                    moveData: pkMoveData
                                    ))
                                {
                                    if (Random.value <= quickClaw.chance)
                                    {
                                        activatedQuickClaw = true;
                                        yield return StartCoroutine(ConsumeItem(
                                            pokemon: userPokemon, 
                                            holderPokemon: userPokemon,
                                            item: item,
                                            consumeText: quickClaw.displayText,
                                            callback: (result) => { }
                                            ));
                                    }
                                }
                            }
                        }

                        if (activatedQuickClaw)
                        {
                            quickCommands.Add(command);
                        }
                    }
                }
            }
            callback(quickCommands);
            yield return null;
        }
        private IEnumerator ExecutePreCommands()
        {
            for (int i = 0; i < allCommands.Count; i++)
            {
                BattleCommand command = allCommands[i];
                Pokemon userPokemon = command.commandUser;
                if (command.commandType == BattleCommandType.Fight)
                {
                    if (battle.IsPokemonOnFieldAndAble(userPokemon))
                    {
                        yield return StartCoroutine(PBPFormTransformation(userPokemon, command));
                    
                        MoveData moveData = MoveDatabase.instance.GetMoveData(command.moveID);

                        // Beak Blast
                        MoveEffect beakBlast = moveData.GetEffect(MoveEffectType.BeakBlast);
                        if (beakBlast != null)
                        {
                            userPokemon.bProps.beakBlastMove = moveData.ID;
                            SendEvent(new Battle.View.Events.Message
                            {
                                message = userPokemon.nickname + " started heating up its beak!"
                            });
                        }

                        // Focus Punch
                        MoveEffect focusPunch = moveData.GetEffect(MoveEffectType.FocusPunch);
                        if (focusPunch != null)
                        {
                            userPokemon.bProps.focusPunchMove = moveData.ID;
                            SendEvent(new Battle.View.Events.Message
                            {
                                message = userPokemon.nickname + " is tightening its focus!"
                            });
                        }

                        // Shell Trap
                        MoveEffect shellTrap = moveData.GetEffect(MoveEffectType.ShellTrap);
                        if (shellTrap != null)
                        {
                            userPokemon.bProps.shellTrapMove = moveData.ID;
                            SendEvent(new Battle.View.Events.Message
                            {
                                message = userPokemon.nickname + " set a shell trap!"
                            });
                        }
                    }
                }
            }
            yield return null;
        }
        public IEnumerator PBPFormTransformationAll(List<BattleCommand> commands)
        {
            for (int i = 0; i < commands.Count; i++)
            {
                Pokemon userPokemon = commands[i].commandUser;
                if (battle.IsPokemonOnFieldAndAble(userPokemon))
                {
                    yield return StartCoroutine(PBPFormTransformation(userPokemon, commands[i]));
                }
            }
        }
        public IEnumerator PBPFormTransformation(Pokemon pokemon, BattleCommand command)
        {
            // Cannot form change if not on field, or being sky-dropped
            if (string.IsNullOrEmpty(pokemon.bProps.skyDropMove)
                && battle.IsPokemonOnFieldAndAble(pokemon))
            {
                Trainer trainer = battle.GetPokemonOwner(pokemon);
            
                // Mega-Evolution
                if (command.isMegaEvolving 
                    && pokemon.megaState != Pokemon.MegaState.Mega
                    && trainer.megaRing != null)
                {
                    // Special Cases (Rayquaza) 

                    Item item = pokemon.item;
                    if (item != null)
                    {
                        EffectDatabase.ItemEff.ItemEffect formChangeItemEffect =
                            battle.PBPGetItemFormChangeEffect(pokemon, item);
                        if (formChangeItemEffect != null)
                        {
                            // Mega-Evolution
                            if (formChangeItemEffect is EffectDatabase.ItemEff.MegaStone)
                            {
                                EffectDatabase.ItemEff.MegaStone megaStone =
                                    formChangeItemEffect as EffectDatabase.ItemEff.MegaStone;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = "pokemon-megaevolve",
                                    pokemonUserID = pokemon.uniqueID,
                                    trainerID = trainer.playerID,
                                    itemID = trainer.megaRing.itemID
                                });

                                trainer.bProps.usedMegaEvolution = true;
                                pokemon.megaState = Pokemon.MegaState.Mega;
                                yield return StartCoroutine(PBPChangeForm(pokemon: pokemon, toForm: megaStone.formID));
                                UpdateClients();
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = "pokemon-megaevolve-form",
                                    pokemonUserID = pokemon.uniqueID,
                                    trainerID = trainer.playerID,
                                    itemID = trainer.megaRing.itemID
                                });
                            }
                        }
                    }
                }

                // Dynamax
                if (command.isDynamaxing 
                    && pokemon.dynamaxState == Pokemon.DynamaxState.None
                    && trainer.dynamaxBand != null)
                {
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = "pokemon-dynamax",
                        pokemonUserID = pokemon.uniqueID,
                        trainerID = trainer.playerID,
                        itemID = trainer.dynamaxBand.itemID
                    });

                    trainer.bProps.usedDynamax = true;

                    battle.PBPDynamax(pokemon);
                    if (!string.IsNullOrEmpty(pokemon.dynamaxProps.GMaxForm))
                    {
                        yield return StartCoroutine(PBPChangeForm(
                            pokemon: pokemon,
                            toForm: pokemon.dynamaxProps.GMaxForm,
                            checkAbility: false));
                    }
                    UpdateClients();

                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = (pokemon.dynamaxState == Pokemon.DynamaxState.Gigantamax)? "pokemon-gigantamax" : "pokemon-dynamax-form",
                        pokemonUserID = pokemon.uniqueID
                    });
                }
            }
            yield return null;
        }
        public IEnumerator PBPUnDynamax(Pokemon pokemon)
        {
            if (pokemon.dynamaxState != Pokemon.DynamaxState.None)
            {
                string pokemonPreForm = pokemon.pokemonID;
                battle.PBPUndynamax(pokemon);
                UpdateClients();

                SendEvent(new Battle.View.Events.PokemonChangeForm
                {
                    pokemonUniqueID = pokemon.uniqueID,
                    preForm = pokemonPreForm,
                    postForm = pokemon.pokemonID
                });
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = "pokemon-dynamax-end",
                    pokemonUserID = pokemon.uniqueID
                });
                yield return null;
            }
        }

        private IEnumerator BattleEndTurn()
        {
            // Keep track of things we've done
            HashSet<PokemonSEType> pastStatus = new HashSet<PokemonSEType>();
            HashSet<TeamSEType> pastTeamStatus = new HashSet<TeamSEType>();
            HashSet<BattleSEType> pastBattleStatus = new HashSet<BattleSEType>();

            // ex. weather buffet, status damage, speed boost, items, etc.

            // Battle HP Loss (ex. Hail, Sandstorm)
            pastBattleStatus.Add(BattleSEType.HPLoss);
            if (!battle.IsFinished())
            {
                List<BattleCondition> conditions = battle.BBPGetSCs();
                for (int i = 0; i < conditions.Count && !battle.IsFinished(); i++)
                {
                    EffectDatabase.StatusBTLEff.BattleSE effect_ = conditions[i].data.GetEffectNew(BattleSEType.HPLoss);
                    if (effect_ != null)
                    {
                        if (effect_.timing == BattleSETiming.EndOfTurn)
                        {
                            // heal when no turns left
                            if (conditions[i].turnsLeft == 0)
                            {
                                yield return StartCoroutine(HealBattleSC(conditions[i]));
                            }
                            else
                            {
                                yield return StartCoroutine(ExecuteBattleSE(effect_: effect_, statusData: conditions[i].data));
                            }
                        }
                    }
                }
            }

            // Team HP Loss (G-Max Wildfire)
            if (!battle.IsFinished())
            {
                for (int i = 0; i < battle.teams.Count; i++)
                {
                    BattleTeam team = battle.teams[i];
                    BattleTeamProperties.GMaxWildfire GMaxWildfire = team.bProps.GMaxWildfireStatus;
                    if (GMaxWildfire != null)
                    {
                        StatusTEData statusData = StatusTEDatabase.instance.GetStatusData(GMaxWildfire.statusID); 
                        EffectDatabase.StatusTEEff.TeamSE effect_ = statusData.GetEffectNew(TeamSEType.HPLoss);
                        if (effect_ != null)
                        {
                            if (effect_.timing == TeamSETiming.EndOfTurn)
                            {
                                // Heal on last turn
                                if (GMaxWildfire.turnsLeft == 0)
                                {
                                    yield return StartCoroutine(EndTeamSC(
                                        statusData: statusData,
                                        targetTeam: team,
                                        callback: (result) => { }
                                        ));
                                }
                                // Deal damage
                                else
                                {
                                    yield return StartCoroutine(ExecuteTeamSE(
                                        effect_: effect_,
                                        team: team,
                                        statusData: statusData
                                        ));
                                }
                            }
                        }
                    }
                }
            }

            // Status HP Loss (ex. Bind, Poison)
            pastStatus.Add(PokemonSEType.HPLoss);
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon pokemon = allPokemon[i];
                    List<StatusCondition> conditions = battle.GetPokemonStatusConditions(pokemon);
                    for (int k = 0; k < conditions.Count; k++)
                    {
                        EffectDatabase.StatusPKEff.PokemonSE effect_ = conditions[k].data.GetEffectNew(PokemonSEType.HPLoss);
                        if (effect_ != null)
                        {
                            if (effect_.timing == PokemonSETiming.EndOfTurn)
                            {
                                // heal when no turns left
                                if (conditions[k].turnsLeft == 0)
                                {
                                    yield return StartCoroutine(HealPokemonSC(
                                        targetPokemon: pokemon,
                                        condition: conditions[k]
                                        ));
                                }
                                // deal damage
                                else
                                {
                                    yield return StartCoroutine(ExecutePokemonSE(
                                        effect_: effect_,
                                        pokemon: pokemon,
                                        condition: conditions[k]
                                        ));
                                }
                            }
                        }
                    }
                }
            }

            // Leech Seed
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon leeched = allPokemon[i];
                    if (leeched.bProps.leechSeedPosition != null)
                    {
                        Pokemon pokemon = battle.GetPokemonAtPosition(leeched.bProps.leechSeedPosition);
                        if (pokemon != null)
                        {
                            AbilityData liquidOoze = 
                                battle.PBPLegacyGetAbilityDataWithEffect(leeched, AbilityEffectType.LiquidOoze);

                            // Move-based
                            if (!string.IsNullOrEmpty(leeched.bProps.leechSeedMove))
                            {
                                MoveData moveData = MoveDatabase.instance.GetMoveData(leeched.bProps.leechSeedMove);
                                MoveEffect effect = moveData.GetEffect(MoveEffectType.LeechSeed);
                            
                                // Damage
                                float healPercent = effect.GetFloat(0);
                                int preHP = leeched.currentHP;
                                int damage = battle.GetPokemonHPByPercent(leeched, healPercent);
                                damage = Mathf.Max(1, damage);
                                int damageDealt = battle.SubtractPokemonHP(leeched, damage);
                                int postHP = leeched.currentHP;

                                SendEvent(new Battle.View.Events.Message
                                {
                                    message = leeched.nickname + " had its health sapped by " + moveData.moveName + "!"
                                });

                                yield return StartCoroutine(PBPChangePokemonHP(
                                    pokemon: leeched,
                                    preHP: preHP,
                                    hpChange: damageDealt,
                                    postHP: postHP
                                    ));
                                UpdateClients();

                                // Liquid Ooze inverts effect
                                if (liquidOoze != null)
                                {
                                    PBPShowAbility(leeched, liquidOoze);

                                    // Damage
                                    preHP = pokemon.currentHP;
                                    damageDealt = battle.SubtractPokemonHP(pokemon, damageDealt);
                                    postHP = pokemon.currentHP;

                                    yield return StartCoroutine(PBPChangePokemonHP(
                                        pokemon: pokemon,
                                        preHP: preHP,
                                        hpChange: damageDealt,
                                        postHP: postHP
                                    ));
                                }
                                else
                                {
                                    // Heal
                                    preHP = pokemon.currentHP;
                                    damageDealt = battle.AddPokemonHP(pokemon, damageDealt);
                                    postHP = pokemon.currentHP;

                                    yield return StartCoroutine(PBPChangePokemonHP(
                                        pokemon: pokemon,
                                        preHP: preHP,
                                        hpChange: damageDealt,
                                        postHP: postHP,
                                        heal: true
                                    ));
                                }
                            }
                    
                        }
                    }
                    yield return StartCoroutine(BattleFaintCheck(leeched));
                }
            }

            // Bind / Magma Storm / Whirlpool
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon currentTarget = allPokemon[i];
                    if (!string.IsNullOrEmpty(currentTarget.bProps.bindMove))
                    {
                        Pokemon bindUser = battle.GetFieldPokemonByID(currentTarget.bProps.bindPokemon);
                        MoveData moveData = MoveDatabase.instance.GetMoveData(currentTarget.bProps.bindMove);
                        MoveEffect effect = moveData.GetEffect(MoveEffectType.Bind);
                        // free from bind
                        if (currentTarget.bProps.bindTurns == 0)
                        {
                            string bindMove = currentTarget.bProps.bindMove;
                            battle.UnsetPokemonBindMove(currentTarget);
                            SendEvent(new Battle.View.Events.Message
                            {
                                message = currentTarget + " was freed from " + moveData.moveName + "!"
                            });
                        }
                        // take bind damage
                        else
                        {
                            float hpPercent = effect.GetFloat(0);
                            int preHP = currentTarget.currentHP;
                            int damage = battle.GetPokemonHPByPercent(currentTarget, hpPercent);
                            damage = Mathf.Max(1, damage);
                            int damageDealt = battle.SubtractPokemonHP(currentTarget, damage);
                            int postHP = currentTarget.currentHP;
                            SendEvent(new Battle.View.Events.Message
                            {
                                message = currentTarget + " was hurt by " + moveData.moveName + "!"
                            });

                            yield return StartCoroutine(PBPChangePokemonHP(
                                pokemon: currentTarget,
                                preHP: preHP,
                                hpChange: damageDealt,
                                postHP: postHP
                                ));
                        }
                    }
                    yield return StartCoroutine(BattleFaintCheck(currentTarget));
                }
            }

            // Battle HP Gain (ex. Grassy Terrain)
            pastBattleStatus.Add(BattleSEType.HPGain);
            if (!battle.IsFinished())
            {
                List<BattleCondition> conditions = battle.BBPGetSCs();
                for (int i = 0; i < conditions.Count && !battle.IsFinished(); i++)
                {
                    EffectDatabase.StatusBTLEff.BattleSE effect_ = conditions[i].data.GetEffectNew(BattleSEType.HPGain);
                    if (effect_ != null)
                    {
                        if (effect_.timing == BattleSETiming.EndOfTurn)
                        {
                            // heal when no turns left
                            if (conditions[i].turnsLeft == 0)
                            {
                                yield return StartCoroutine(HealBattleSC(conditions[i]));
                            }
                            else
                            {
                                yield return StartCoroutine(ExecuteBattleSE(effect_: effect_, statusData: conditions[i].data));
                            }
                        }
                    }
                }
            }

            // Ingrain
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon currentTarget = allPokemon[i];
                    bool isHealBlocked = false;
                    for (int k = 0; k < currentTarget.bProps.moveLimiters.Count && !isHealBlocked; k++)
                    {
                        if (currentTarget.bProps.moveLimiters[k].effect is EffectDatabase.StatusPKEff.HealBlock)
                        {
                            isHealBlocked = true;
                        }
                    }

                    if (!isHealBlocked)
                    {
                        for (int k = 0; k < currentTarget.bProps.ingrainMoves.Count; k++)
                        {
                            MoveData moveData = MoveDatabase.instance.GetMoveData(currentTarget.bProps.ingrainMoves[k]);
                            MoveEffect effect = moveData.GetEffect(MoveEffectType.Ingrain);

                            float healPercent = effect.GetFloat(0);
                            int preHP = currentTarget.currentHP;
                            int healAmount = battle.GetPokemonHPByPercent(currentTarget, healPercent);
                            healAmount = Mathf.Max(1, healAmount);
                            int hpHealed = battle.AddPokemonHP(currentTarget, healAmount);
                            int postHP = currentTarget.currentHP;

                            if (hpHealed > 0)
                            {
                                string textID = effect.GetString(1);
                                textID = (textID == "DEFAULT") ? "move-ingrain-heal" : textID;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = textID,
                                    pokemonTargetID = currentTarget.uniqueID,
                                    moveID = moveData.ID
                                });

                                yield return StartCoroutine(PBPChangePokemonHP(
                                    pokemon: currentTarget,
                                    preHP: preHP,
                                    hpChange: hpHealed,
                                    postHP: postHP,
                                    heal: true
                                    ));
                            }
                        }
                    }
                }
            }

            // Leftovers

            // Check end of turn statuses
        
            // Move-Limiters (Disable, Encore, Taunt, etc.)
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon currentTarget = allPokemon[i];
                    List<Pokemon.BattleProperties.MoveLimiter> limiters =
                        new List<Pokemon.BattleProperties.MoveLimiter>(currentTarget.bProps.moveLimiters);
                    for (int k = 0; k < limiters.Count; k++)
                    {
                        if (limiters[k].turnsLeft == 0)
                        {
                            currentTarget.bProps.moveLimiters.Remove(limiters[k]);
                            string moveID = limiters[k].GetMove();
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = limiters[k].effect.endText,
                                pokemonTargetID = currentTarget.uniqueID,
                                moveID = moveID
                            });
                        }
                    }

                    // Embargo
                    if (currentTarget.bProps.embargo != null)
                    {
                        Pokemon.BattleProperties.Embargo embargo = currentTarget.bProps.embargo;
                        if (embargo.turnsLeft == 0)
                        {
                            currentTarget.bProps.embargo = null;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = embargo.effect.endText,
                                pokemonTargetID = currentTarget.uniqueID
                            });
                        }
                    }
                }
            }

            // Yawn
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon currentTarget = allPokemon[i];
                    if (currentTarget.bProps.yawn != null)
                    {
                        // Inflict status
                        if (currentTarget.bProps.yawn.turnsLeft == 0)
                        {
                            StatusPKData yawnStatusData = 
                                StatusPKDatabase.instance.GetStatusData(currentTarget.bProps.yawn.statusID);
                            currentTarget.bProps.yawn = null;
                            yield return StartCoroutine(ApplyPokemonSC(
                                statusData: yawnStatusData,
                                targetPokemon: currentTarget,
                                callback: (result) => { }
                                ));
                        }
                        else
                        {
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = currentTarget.bProps.yawn.waitText,
                                pokemonTargetID = currentTarget.uniqueID
                            });
                        }
                    }
                }
            }

            // Forest's Curse
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon curPokemon = allPokemon[i];
                    List<Pokemon.BattleProperties.ForestsCurse> forestsCurses
                        = new List<Pokemon.BattleProperties.ForestsCurse>(curPokemon.bProps.forestsCurses);

                    for (int k = 0; k < forestsCurses.Count; k++)
                    {
                        if (forestsCurses[k].turnsLeft == 0)
                        {
                            yield return StartCoroutine(PBPRemoveForestsCurse(curPokemon, forestsCurses[k]));
                        }
                    }
                }
            }

            // Lock-On
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon curPokemon = allPokemon[i];
                    List<Pokemon.BattleProperties.LockOn> lockOnTargets 
                        = new List<Pokemon.BattleProperties.LockOn>(curPokemon.bProps.lockOnTargets);

                    for (int k = 0; k < lockOnTargets.Count; k++)
                    {
                        if (lockOnTargets[k].turnsLeft == 0)
                        {
                            curPokemon.bProps.lockOnTargets.Remove(lockOnTargets[k]);

                            Pokemon lockOnPokemon = battle.GetPokemonByID(lockOnTargets[k].pokemonUniqueID);
                            MoveData moveData = MoveDatabase.instance.GetMoveData(lockOnTargets[k].moveID);
                            MoveEffect effect = moveData.GetEffect(MoveEffectType.LockOn);

                            string textID = effect.GetString(1);
                            textID = (textID == "DEFAULT") ? "move-lockon-end" : textID;

                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = curPokemon.uniqueID,
                                pokemonTargetID = lockOnPokemon.uniqueID,
                                moveID = moveData.ID
                            });
                        }
                    }
                }
            }

            // Template
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon curPokemon = allPokemon[i];
                }
            }

            // Entry Hazards 
            if (!battle.IsFinished())
            {
                for (int i = 0; i < battle.teams.Count; i++)
                {
                    BattleTeam team = battle.teams[i];
                    List<BattleTeamProperties.EntryHazard> entryHazards 
                        = new List<BattleTeamProperties.EntryHazard>(team.bProps.entryHazards);
                    for (int k = 0; k < entryHazards.Count; k++)
                    {
                        if (entryHazards[k].turnsLeft == 0)
                        {
                            yield return StartCoroutine(TBPRemoveEntryHazard(team, entryHazards[k]));
                        }
                    }
                }
            }

            // Reflect / Light Screen 
            if (!battle.IsFinished())
            {
                for (int i = 0; i < battle.teams.Count; i++)
                {
                    BattleTeam team = battle.teams[i];
                    List<BattleTeamProperties.ReflectScreen> reflectScreens
                        = new List<BattleTeamProperties.ReflectScreen>(team.bProps.reflectScreens);
                    for (int k = 0; k < reflectScreens.Count; k++)
                    {
                        if (reflectScreens[k].turnsLeft == 0)
                        {
                            yield return StartCoroutine(TBPRemoveReflectScreen(team, reflectScreens[k]));
                        }
                    }
                }
            }

            // Safeguard
            if (!battle.IsFinished())
            {
                for (int i = 0; i < battle.teams.Count; i++)
                {
                    BattleTeam team = battle.teams[i];
                    List<BattleTeamProperties.Safeguard> safeguards
                        = new List<BattleTeamProperties.Safeguard>(team.bProps.safeguards);
                    for (int k = 0; k < safeguards.Count; k++)
                    {
                        if (safeguards[k].turnsLeft == 0)
                        {
                            yield return StartCoroutine(TBPRemoveSafeguard(team, safeguards[k]));
                        }
                    }
                }
            }

            // Future Sight
            if (!battle.IsFinished())
            {
                List<BattleFutureSightCommand> futureSightCommands =
                    new List<BattleFutureSightCommand>(battle.futureSightCommands);
                for (int i = 0; i < futureSightCommands.Count; i++)
                {
                    // Execute Future Sight
                    if (futureSightCommands[i].turnsLeft == 0)
                    {
                        BattleCommand moveCommand = battle.CreateFutureSightMoveCommand(futureSightCommands[i]);
                        yield return StartCoroutine(ExecuteCommand(moveCommand));
                        battle.futureSightCommands.Remove(futureSightCommands[i]);
                    }
                }
            }

            // Perish Song
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon pokemon = allPokemon[i];
                    if (battle.IsPokemonOnFieldAndAble(pokemon) && pokemon.bProps.perishSong != null)
                    {
                        EffectDatabase.StatusPKEff.PerishSong perishSong = pokemon.bProps.perishSong;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = perishSong.countText,
                            pokemonTargetID = pokemon.uniqueID,
                            intArgs = new List<int> { perishSong.turnsLeft }
                        });

                        if (perishSong.turnsLeft == 0)
                        {
                            yield return PBPFaintPokemon(pokemon);
                        }
                    }
                }
            }

            // Pokemon Abilities
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon pokemon = allPokemon[i];
                    yield return StartCoroutine(PBPRunEndTurnAbilities(pokemon));
                }
            }

            // Dynamax End
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon pokemon = allPokemon[i];
                    if (battle.IsPokemonOnFieldAndAble(pokemon))
                    {
                        if (pokemon.dynamaxState != Pokemon.DynamaxState.None
                            && pokemon.dynamaxProps.turnsLeft == 0)
                        {
                            yield return StartCoroutine(PBPUnDynamax(pokemon));
                        }
                    }
                }
            }

            // Run any end of turn effects that haven't been captured yet
            // Battle
            if (!battle.IsFinished())
            {
                List<BattleCondition> conditions = battle.BBPGetSCs();
                for (int i = 0; i < conditions.Count && !battle.IsFinished(); i++)
                {
                    if (conditions[i].data.HasTag(BattleSTag.TurnsDecreaseOnEnd))
                    {
                        // heal when no turns left
                        if (conditions[i].turnsLeft == 0)
                        {
                            yield return StartCoroutine(HealBattleSC(conditions[i]));
                        }
                        else
                        {
                            //yield return StartCoroutine(ApplyBattleStatusEffect(effect, conditions[i]));
                        }
                        yield return StartCoroutine(BattleFaintCheck());
                    }
                }
            }

            // Team
            if (!battle.IsFinished())
            {
                for (int i = 0; i < battle.teams.Count; i++)
                {
                    BattleTeam team = battle.teams[i];
                    List<TeamCondition> conditions = battle.GetTeamSCs(team);
                    for (int k = 0; k < conditions.Count; k++)
                    {
                        if (conditions[k].data.HasTag(TeamSTag.TurnsDecreaseOnEnd))
                        {
                            // heal when no turns left
                            if (conditions[k].turnsLeft == 0)
                            {
                                yield return StartCoroutine(HealTeamSC(
                                    targetTeam: team,
                                    condition: conditions[k]
                                    ));
                            }
                            else
                            {
                                //yield return StartCoroutine(ApplyStatusEffect(
                                // effect,
                                // conditions[k],
                                //allPokemon[i]
                                //));
                            }
                        }
                    }
                }
            }

            // Pokemon
            if (!battle.IsFinished())
            {
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    List<StatusCondition> conditions = battle.GetPokemonStatusConditions(allPokemon[i]);
                    for (int k = 0; k < conditions.Count; k++)
                    {
                        if (conditions[k].data.HasTag(PokemonSTag.TurnsDecreaseOnEnd))
                        {
                            // heal when no turns left
                            if (conditions[k].turnsLeft == 0)
                            {
                                yield return StartCoroutine(HealPokemonSC(
                                    targetPokemon: allPokemon[i],
                                    condition: conditions[k]
                                    ));
                            }
                            else
                            {
                                //yield return StartCoroutine(ApplyStatusEffect(
                                   // effect,
                                   // conditions[k],
                                    //allPokemon[i]
                                    //));
                            }
                        }
                    }
                }
                yield return StartCoroutine(BattleFaintCheck());
            }

            // All Pokemon on the field should be unfainted at this point

            yield return null;
        }
        private IEnumerator BattleReplacement()
        {
            bool stillReplace = true;
            int iterations = 0;
            while (stillReplace && !battle.IsFinished())
            {
                iterations++;
                stillReplace = false;
                replaceCommands = new List<BattleCommand>();
                Model replaceModel = Model.CloneModel(battle);
                UpdateClients();

                // query player for replacements
                List<int> playerIDs = new List<int>(PBS.Static.Master.instance.networkManager.playerConnections.Keys);
                for (int i = 0; i < playerIDs.Count; i++)
                {
                    int playerID = playerIDs[i];
                    Trainer trainer = battle.GetTrainerWithID(playerID);
                    if (trainer != null)
                    {
                        if (battle.IsTrainerAbleToBattle(trainer) && !trainer.IsAIControlled())
                        {
                            List<int> emptyPositions = battle.GetEmptyTrainerControlPositions(trainer);
                            if (emptyPositions.Count > 0)
                            {
                                List<Pokemon> availablePokemon =
                                    battle.GetTrainerFirstXAvailablePokemon(trainer, emptyPositions.Count);
                                // prompt player
                                if (availablePokemon.Count > 0)
                                {
                                    stillReplace = true;
                                    PBS.Battle.View.Events.CommandReplacementPrompt replacePrompt = new Battle.View.Events.CommandReplacementPrompt
                                    {
                                        playerID = trainer.playerID,
                                        fillPositions = emptyPositions.ToArray()
                                    };
                                    waitConnections.Add(playerID);
                                    SendEventToPlayer(replacePrompt, trainer.playerID);
                                }
                            }
                        }
                    }
                }

                // wait for player to confirm choices
                yield return StartCoroutine(WaitForPlayer());

                // set ai replacements
                ai.UpdateModel(replaceModel);
                SelectAIReplacements();

                // reorder replacement commands
                replaceCommands = battle.ReorderCommands(replaceCommands);
                List<Pokemon> replacePokemon = new List<Pokemon>();

                // run replacement commands
                for (int i = 0; i < replaceCommands.Count; i++)
                {
                    yield return StartCoroutine(ExecuteCommandReplace(replaceCommands[i]));
                    replacePokemon.Add(replaceCommands[i].switchInPokemon);
                }

                // TODO: check entry hazards and fainting for replacement pokemon
                for (int i = 0; i < replacePokemon.Count; i++)
                {
                    yield return StartCoroutine(ApplyToPokemonSwitchInEvents(replacePokemon[i]));
                }

                // check fainted pokemon
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                List<Pokemon> faintedPokemon = new List<Pokemon>();
                for (int k = 0; k < allPokemon.Count; k++)
                {
                    if (battle.IsPokemonFainted(allPokemon[k]))
                    {
                        faintedPokemon.Add(allPokemon[k]);
                        battle.FaintPokemon(allPokemon[k]);
                    }
                }
                if (faintedPokemon.Count > 0)
                {
                    for (int i = 0; i < faintedPokemon.Count; i++)
                    {
                        SendEvent(new Battle.View.Events.PokemonHealthFaint
                        {
                            pokemonUniqueID = faintedPokemon[i].uniqueID
                        });
                    }
                    yield return StartCoroutine(UntieListedPokemon(faintedPokemon));
                }

                // Run Wish Effects on first attempt
                if (iterations <= 1)
                {
                    // Wish
                    if (!battle.IsFinished())
                    {
                        List<BattleWishCommand> wishCommands =
                            new List<BattleWishCommand>(battle.wishCommands);
                        for (int i = 0; i < wishCommands.Count; i++)
                        {
                            // Execute Wish
                            if (wishCommands[i].turnsLeft == 0)
                            {
                                MoveData wishMoveData = MoveDatabase.instance.GetMoveData(wishCommands[i].moveID);

                                List<Pokemon> wishRecipients = new List<Pokemon>();
                                for (int k = 0; k < wishCommands[i].wishPositions.Count; k++)
                                {
                                    Pokemon pokemon = battle.GetPokemonAtPosition(wishCommands[i].wishPositions[k]);
                                    if (pokemon != null)
                                    {
                                        wishRecipients.Add(pokemon);
                                    }
                                }

                                if (wishRecipients.Count > 0)
                                {
                                    MoveEffect effect = wishMoveData.GetEffect(MoveEffectType.Wish);
                                    string textID = effect.GetString(1);
                                    textID = (textID == "DEFAULT") ? "move-wish-heal" : textID;

                                    List<string> wishRecipientIDs = new List<string>();
                                    for (int k = 0; k < wishRecipients.Count; k++)
                                    {
                                        wishRecipientIDs.Add(wishRecipients[k].uniqueID);
                                    }
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = battle.GetPokemonByID(wishCommands[i].pokemonID).uniqueID,
                                        pokemonListIDs = wishRecipientIDs,
                                        moveID = wishMoveData.ID
                                    });

                                    for (int k = 0; k < wishRecipients.Count; k++)
                                    {
                                        // Heal HP
                                        Pokemon pokemon = wishRecipients[k];
                                        int preHP = pokemon.currentHP;
                                        int hpRecovered = effect.GetBool(1) ? pokemon.maxHP : wishCommands[i].hpRecovered;
                                        int hpHealed = battle.AddPokemonHP(pokemon, hpRecovered);
                                        int postHP = pokemon.currentHP;

                                        yield return StartCoroutine(PBPChangePokemonHP(
                                            pokemon: pokemon,
                                            preHP: preHP,
                                            hpChange: hpHealed,
                                            postHP: postHP,
                                            heal: true
                                            ));

                                        // Heal PP
                                        if (wishMoveData.GetEffect(MoveEffectType.WishLunarDance) != null)
                                        {
                                            battle.ReplenishPokemonPP(pokemon);
                                        }

                                        // Heal Status
                                        if (effect.GetBool(0))
                                        {
                                            if (!pokemon.nonVolatileStatus.data.HasTag(PokemonSTag.IsDefault))
                                            {
                                                yield return StartCoroutine(HealPokemonSC(
                                                    targetPokemon: pokemon,
                                                    condition: pokemon.nonVolatileStatus
                                                    ));
                                            }
                                        }
                                    }

                                }
                                battle.wishCommands.Remove(wishCommands[i]);
                            }
                        }
                    }
                }


                yield return null;
            }
        }
        private IEnumerator BattleFaintCheck(Pokemon pokemon)
        {
            yield return StartCoroutine(BattleFaintCheck(
                pokemonToCheck: new List<Pokemon> { pokemon }
                ));
        }
        private IEnumerator BattleFaintCheck()
        {
            yield return StartCoroutine(BattleFaintCheck(battle.pokemonOnField));
        }
        private IEnumerator BattleFaintCheck(
            List<Pokemon> pokemonToCheck,
            Pokemon attackerPokemon = null)
        {
            List<Pokemon> alivePokemon = battle.GetPokemonUnfaintedFrom(battle.pokemonOnField);
            alivePokemon = battle.GetPokemonBySpeed(alivePokemon);

            Dictionary<Pokemon, List<Pokemon>> allyPokemonMap = new Dictionary<Pokemon, List<Pokemon>>();
            Dictionary<Pokemon, List<Pokemon>> opposingPokemonMap = new Dictionary<Pokemon, List<Pokemon>>();

            List<Pokemon> faintedPokemon = battle.GetPokemonFaintedFrom(pokemonToCheck);
            for (int i = 0; i < faintedPokemon.Count; i++)
            {
                if (battle.IsPokemonOnField(faintedPokemon[i]))
                {
                    for (int k = 0; k < alivePokemon.Count; k++)
                    {
                        if (battle.ArePokemonAllies(alivePokemon[k], faintedPokemon[i])
                            && alivePokemon[k] != faintedPokemon[i])
                        {
                            if (!allyPokemonMap.ContainsKey(alivePokemon[k]))
                            {
                                allyPokemonMap.Add(alivePokemon[k], new List<Pokemon>());
                            }
                            allyPokemonMap[alivePokemon[k]].Add(faintedPokemon[i]);
                        }
                        else if (battle.ArePokemonEnemies(alivePokemon[k], faintedPokemon[i]))
                        {
                            if (!opposingPokemonMap.ContainsKey(alivePokemon[k]))
                            {
                                opposingPokemonMap.Add(alivePokemon[k], new List<Pokemon>());
                            }
                            opposingPokemonMap[alivePokemon[k]].Add(faintedPokemon[i]);
                        }
                    }

                    battle.FaintPokemon(faintedPokemon[i]);
                }
            }
            if (faintedPokemon.Count > 0)
            {
                for (int i = 0; i < faintedPokemon.Count; i++)
                {
                    SendEvent(new Battle.View.Events.PokemonHealthFaint
                    {
                        pokemonUniqueID = faintedPokemon[i].uniqueID
                    });
                }
                yield return StartCoroutine(UntieListedPokemon(faintedPokemon));
            }

            // Run Abilities
            for (int i = 0; i < alivePokemon.Count; i++)
            {
                Pokemon pokemon = alivePokemon[i];
                if (battle.IsPokemonOnFieldAndAble(pokemon))
                {
                    List<Pokemon.Ability> abilities = battle.PBPGetAbilities(pokemon);

                    // Battle Bond
                    if (attackerPokemon != null && pokemon == attackerPokemon)
                    {
                        bool transformed = false;
                        for (int k = 0; k < abilities.Count && !transformed; k++)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect battleBond_ =
                                abilities[k].data.GetEffectNew(AbilityEffectType.BattleBond);
                            if (battleBond_ != null)
                            {
                                EffectDatabase.AbilityEff.BattleBond battleBond =
                                    battleBond_ as EffectDatabase.AbilityEff.BattleBond;

                                if (opposingPokemonMap.ContainsKey(pokemon))
                                {
                                    for (int j = 0; j < battleBond.transformations.Count && !transformed; j++)
                                    {
                                        if (pokemon.pokemonID == battleBond.transformations[j].preForm)
                                        {
                                            transformed = true;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = battleBond.beforeText,
                                                pokemonUserID = pokemon.uniqueID,
                                                abilityID = abilities[k].data.ID
                                            });

                                            PBPShowAbility(pokemon, abilities[k]);
                                            yield return StartCoroutine(PBPChangeForm(
                                                pokemon: pokemon,
                                                toForm: battleBond.transformations[j].toForm
                                                ));

                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = battleBond.afterText,
                                                pokemonUserID = pokemon.uniqueID,
                                                abilityID = abilities[k].data.ID
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Beast Boost
                    if (attackerPokemon != null && pokemon == attackerPokemon)
                    {
                        for (int k = 0; k < abilities.Count; k++)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect beastBoost_ =
                                abilities[k].data.GetEffectNew(AbilityEffectType.BeastBoost);
                            if (beastBoost_ != null)
                            {
                                EffectDatabase.AbilityEff.BeastBoost beastBoost =
                                    beastBoost_ as EffectDatabase.AbilityEff.BeastBoost;

                                List<Pokemon> knockedOutPokemon = new List<Pokemon>();
                                if (allyPokemonMap.ContainsKey(pokemon))
                                {
                                    knockedOutPokemon.AddRange(allyPokemonMap[pokemon]);
                                }
                                if (opposingPokemonMap.ContainsKey(pokemon))
                                {
                                    knockedOutPokemon.AddRange(opposingPokemonMap[pokemon]);
                                }

                                if (knockedOutPokemon.Count > 0)
                                {
                                    int KOCount = knockedOutPokemon.Count;

                                    PokemonStats highestStat = battle.GetPokemonHighestStat(pokemon);
                                    EffectDatabase.General.StatStageMod beastBoostStatMod =
                                        new EffectDatabase.General.StatStageMod(
                                            ATKMod: (highestStat == PokemonStats.Attack)
                                                ? beastBoost.statMod * KOCount : 0,
                                            DEFMod: (highestStat == PokemonStats.Defense)
                                                ? beastBoost.statMod * KOCount : 0,
                                            SPAMod: (highestStat == PokemonStats.SpecialAttack)
                                                ? beastBoost.statMod * KOCount : 0,
                                            SPDMod: (highestStat == PokemonStats.SpecialDefense)
                                                ? beastBoost.statMod * KOCount : 0,
                                            SPEMod: (highestStat == PokemonStats.Speed)
                                                ? beastBoost.statMod * KOCount : 0
                                        );

                                    PBPShowAbility(pokemon, abilities[k]);
                                    yield return StartCoroutine(ApplyStatStageMod(
                                        userPokemon: pokemon,
                                        targetPokemon: pokemon,
                                        statStageMod: beastBoostStatMod,
                                        forceFailureMessage: true,
                                        callback: (result) => { }
                                        ));
                                }
                            }
                        }
                    }

                    // Moxie
                    if (attackerPokemon != null && pokemon == attackerPokemon)
                    {
                        for (int k = 0; k < abilities.Count; k++)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect moxie_ =
                                abilities[k].data.GetEffectNew(AbilityEffectType.Moxie);
                            if (moxie_ != null)
                            {
                                EffectDatabase.AbilityEff.Moxie moxie =
                                    moxie_ as EffectDatabase.AbilityEff.Moxie;

                                List<Pokemon> knockedOutPokemon = new List<Pokemon>();
                                if (allyPokemonMap.ContainsKey(pokemon))
                                {
                                    knockedOutPokemon.AddRange(allyPokemonMap[pokemon]);
                                }
                                if (opposingPokemonMap.ContainsKey(pokemon))
                                {
                                    knockedOutPokemon.AddRange(opposingPokemonMap[pokemon]);
                                }

                                EffectDatabase.General.StatStageMod moxieStatMod = null;
                                for (int j = 0; j < knockedOutPokemon.Count; j++)
                                {
                                    if (moxieStatMod == null)
                                    {
                                        moxieStatMod = moxie.statStageMod.Clone();
                                    }
                                    else
                                    {
                                        moxieStatMod.AddOther(moxie.statStageMod);
                                    }
                                }

                                if (moxieStatMod != null)
                                {
                                    PBPShowAbility(pokemon, abilities[k]);
                                    yield return StartCoroutine(ApplyStatStageMod(
                                        userPokemon: pokemon,
                                        targetPokemon: pokemon,
                                        statStageMod: moxieStatMod,
                                        forceFailureMessage: true,
                                        callback: (result) => { }
                                        ));
                                }
                            }
                        }
                    }

                    // Soul Heart
                    for (int k = 0; k < abilities.Count; k++)
                    {
                        EffectDatabase.AbilityEff.AbilityEffect soulHeart_ =
                            abilities[k].data.GetEffectNew(AbilityEffectType.SoulHeart);
                        if (soulHeart_ != null)
                        {
                            EffectDatabase.AbilityEff.SoulHeart soulHeart =
                                soulHeart_ as EffectDatabase.AbilityEff.SoulHeart;

                            List<Pokemon> knockedOutPokemon = new List<Pokemon>();
                            if (allyPokemonMap.ContainsKey(pokemon))
                            {
                                knockedOutPokemon.AddRange(allyPokemonMap[pokemon]);
                            }
                            if (opposingPokemonMap.ContainsKey(pokemon))
                            {
                                knockedOutPokemon.AddRange(opposingPokemonMap[pokemon]);
                            }

                            EffectDatabase.General.StatStageMod soulHeartStatMod = null;
                            for (int j = 0; j < knockedOutPokemon.Count; j++)
                            {
                                if (soulHeartStatMod == null)
                                {
                                    soulHeartStatMod = soulHeart.statStageMod.Clone();
                                }
                                else
                                {
                                    soulHeartStatMod.AddOther(soulHeart.statStageMod);
                                }
                            }

                            if (soulHeartStatMod != null)
                            {
                                PBPShowAbility(pokemon, abilities[k]);
                                yield return StartCoroutine(ApplyStatStageMod(
                                    userPokemon: pokemon,
                                    targetPokemon: pokemon,
                                    statStageMod: soulHeartStatMod,
                                    forceFailureMessage: true,
                                    callback: (result) => { }
                                    ));
                            }
                        }
                    }

                    // Power of Alchemy / Receiver
                    for (int k = 0; k < abilities.Count; k++)
                    {
                        Pokemon.Ability powerOfAlchemyAbility = abilities[k];
                        EffectDatabase.AbilityEff.AbilityEffect powerOfAlchemy_ =
                            powerOfAlchemyAbility.data.GetEffectNew(AbilityEffectType.PowerOfAlchemy);
                        if (powerOfAlchemy_ != null)
                        {
                            EffectDatabase.AbilityEff.PowerOfAlchemy powerOfAlchemy =
                                powerOfAlchemy_ as EffectDatabase.AbilityEff.PowerOfAlchemy;
                            if (battle.DoEffectFiltersPass(
                                filters: powerOfAlchemy_.filters,
                                targetPokemon: pokemon
                                )
                                && allyPokemonMap.ContainsKey(pokemon))
                            {
                                bool triggeredReceiver = false;
                                List<Pokemon> faintedAllies = allyPokemonMap[pokemon];

                                for (int j = 0; j < faintedAllies.Count && !triggeredReceiver; j++)
                                {
                                    List<Pokemon.Ability> allyAbilities =
                                        battle.PBPGetAbilitiesGainable(faintedPokemon[j]);
                                    if (allyAbilities.Count > 0
                                        && battle.PBPGetAbilitiesReplaceable(pokemon, allyAbilities).Count > 0)
                                    {
                                        triggeredReceiver = true;
                                        PBPShowAbility(pokemon, powerOfAlchemyAbility);

                                        // remove ability
                                        yield return StartCoroutine(PBPRemoveAbility(
                                            pokemon: pokemon,
                                            ability: powerOfAlchemyAbility
                                            ));

                                        // replace with new abilities
                                        yield return StartCoroutine(PBPAddAbilities(
                                            pokemon: pokemon,
                                            abilities: allyAbilities
                                            ));
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }

        // Common Battle Interactions
        private IEnumerator PBPChangePokemonHP(
            Pokemon pokemon, 
            int preHP, 
            int hpChange, 
            int postHP,
            bool heal = false,
            bool checkFaint = false)
        {
            if (heal)
            {
                SendEvent(new Battle.View.Events.PokemonHealthHeal
                {
                    pokemonUniqueID = pokemon.uniqueID,
                    preHP = preHP,
                    postHP = postHP,
                    maxHP = pokemon.maxHP
                });
            }
            else
            {
                SendEvent(new Battle.View.Events.PokemonHealthDamage
                {
                    pokemonUniqueID = pokemon.uniqueID,
                    preHP = preHP,
                    postHP = postHP,
                    maxHP = pokemon.maxHP
                });
                yield return null;

                if (checkFaint)
                {
                    // run faint event if it hasn't been done already for the user
                    if (battle.IsPokemonOnField(pokemon) && battle.IsPokemonFainted(pokemon))
                    {
                        battle.FaintPokemon(pokemon);
                        SendEvent(new Battle.View.Events.PokemonHealthFaint
                        {
                            pokemonUniqueID = pokemon.uniqueID
                        });
                        yield return StartCoroutine(UntiePokemon(pokemon));
                    }
                }
            }
        }
        private IEnumerator PBPDamagePokemon(
            Pokemon pokemon,
            int HPToLose,
            bool checkFaint = true,
            bool checkHPLossTriggers = true,
            bool ignoreMagicGuard = false
            )
        {
            // Magic Guard Check
            bool ignoreDamage = false;
            if (!ignoreMagicGuard)
            {
                List<EffectDatabase.AbilityEff.AbilityEffect> magicGuard_ =
                    battle.PBPGetAbilityEffects(pokemon, AbilityEffectType.MagicGuard);
                for (int i = 0; i < magicGuard_.Count && !ignoreDamage; i++)
                {
                    EffectDatabase.AbilityEff.MagicGuard magicGuard =
                        magicGuard_[i] as EffectDatabase.AbilityEff.MagicGuard;
                    if (battle.DoEffectFiltersPass(
                        filters: magicGuard.filters,
                        targetPokemon: pokemon
                        ))
                    {
                        ignoreDamage = true;
                    }
                }
            }

            if (!ignoreDamage)
            {
                float preHPPercent = battle.GetPokemonHPAsPercentage(pokemon);
                int preHP = pokemon.currentHP;
                int hpRecovered = battle.SubtractPokemonHP(pokemon, HPToLose);
                int postHP = pokemon.currentHP;
                float postHPPercent = battle.GetPokemonHPAsPercentage(pokemon);

                yield return StartCoroutine(PBPChangePokemonHP(
                    pokemon: pokemon,
                    preHP: preHP,
                    hpChange: hpRecovered,
                    postHP: postHP,
                    checkFaint: checkFaint
                    ));

                // HP-Loss triggers
                if (checkHPLossTriggers)
                {
                    if (battle.IsPokemonOnFieldAndAble(pokemon))
                    {
                        // HP Trigger Items (Oran Berry, Sitrus Berry)
                        // Check HP-Trigger Berry
                        List<Item> sitrusItems = battle.PBPGetItemsWithEffect(pokemon, ItemEffectType.TriggerOnHPLoss);
                        for (int i = 0; i < sitrusItems.Count; i++)
                        {
                            Item curItem = sitrusItems[i];

                            EffectDatabase.ItemEff.TriggerSitrusBerry sitrusBerry =
                                curItem.data.GetEffectNew(ItemEffectType.TriggerOnHPLoss)
                                as EffectDatabase.ItemEff.TriggerSitrusBerry;
                            float triggerThreshold = sitrusBerry.hpThreshold;

                            // Gluttony
                            List<EffectDatabase.AbilityEff.AbilityEffect> gluttony_ =
                                battle.PBPGetAbilityEffects(
                                    pokemon: pokemon,
                                    effectType: AbilityEffectType.Gluttony);
                            for (int k = 0; k < gluttony_.Count; k++)
                            {
                                EffectDatabase.AbilityEff.Gluttony gluttony =
                                    gluttony_[k] as EffectDatabase.AbilityEff.Gluttony;
                                if (triggerThreshold >= gluttony.minItemHPThreshold
                                    && triggerThreshold <= gluttony.maxItemHPThreshold)
                                {
                                    if (curItem.data.pocket == ItemPocket.Berries)
                                    {
                                        triggerThreshold *= gluttony.thresholdScale;
                                    }
                                }
                            }

                            triggerThreshold = Mathf.Min(triggerThreshold, 1f);
                            if (preHPPercent > triggerThreshold
                                && postHPPercent <= triggerThreshold)
                            {
                                yield return StartCoroutine(TryToConsumeItem(
                                    pokemon: pokemon,
                                    holderPokemon: pokemon,
                                    item: curItem,
                                    (result) => { }
                                    ));
                            }
                        }
                    }
                }
            }
        }
        private IEnumerator PBPHealPokemon(
            Pokemon pokemon,
            int HPToAdd
            )
        {
            int preHP = pokemon.currentHP;
            int hpRecovered = battle.AddPokemonHP(pokemon, HPToAdd);
            int postHP = pokemon.currentHP;

            SendEvent(new Battle.View.Events.MessageParameterized
            {
                messageCode = battle.IsPokemonFainted(pokemon) ? "pokemon-revive" : "pokemon-heal", 
                pokemonTargetID = pokemon.uniqueID,
                intArgs = new List<int> { hpRecovered }
            });

            yield return StartCoroutine(PBPChangePokemonHP(
                pokemon: pokemon,
                preHP: preHP,
                hpChange: hpRecovered,
                postHP: postHP,
                heal: true
                ));
        }
        private IEnumerator PBPFaintPokemon(Pokemon pokemon)
        {
            int preHP = pokemon.currentHP;
            int damage = pokemon.maxHP;
            int damageDealt = battle.SubtractPokemonHP(pokemon, damage);
            int postHP = pokemon.currentHP;

            yield return StartCoroutine(PBPChangePokemonHP(
                pokemon: pokemon,
                preHP: preHP,
                hpChange: damageDealt,
                postHP: postHP,
                checkFaint: true
                ));
        }

        public IEnumerator ApplyHealHP(
            EffectDatabase.General.HealHP healHP,
            System.Action<bool> callback,
            Pokemon targetPokemon = null,
            Pokemon healerPokemon = null,
            float scaleAmount = 1f,
            bool forceFailMessage = false,
            bool apply = true
            )
        {
            bool success = false;

            bool canApply = true;
            // Revive?
            if (!healHP.revive && battle.IsPokemonFainted(targetPokemon))
            {
                canApply = false;
            }

            // Can recover HP?
            if (canApply && battle.GetPokemonHPAsPercentage(targetPokemon) == 1f)
            {
                canApply = false;
                if (forceFailMessage)
                {
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = "pokemon-heal-fail", 
                        pokemonTargetID = targetPokemon.uniqueID
                    });
                }
            }

            if (canApply)
            {
                success = true;
                int HPToHeal = battle.GetHeal(heal: healHP, targetPokemon: targetPokemon,  healerPokemon: healerPokemon);
                
                if (battle.IsPokemonFainted(targetPokemon))
                {
                    SendEvent(new Battle.View.Events.PokemonHealthRevive
                    {
                        pokemonUniqueID = targetPokemon.uniqueID
                    });
                }

                yield return StartCoroutine(PBPHealPokemon(
                    pokemon: targetPokemon,
                    HPToAdd: Mathf.FloorToInt(HPToHeal * scaleAmount)
                    ));
            }
            callback(success);
        }

        // Executing Commands
        private IEnumerator ExecuteAllCommands(List<BattleCommand> commands)
        {
            // Execute all switch commands
            for (int i = 0; i < commands.Count && !battle.IsFinished(); i++)
            {
                if (!commands[i].completed && commands[i].commandType == BattleCommandType.Party)
                {
                    yield return StartCoroutine(ExecuteCommand(commands[i]));
                }
            }

            // Execute all form changes if they already haven't been already
            if (!executedFormTransformations && !battle.IsFinished())
            {
                yield return StartCoroutine(PBPFormTransformationAll(commands));
                executedFormTransformations = true;
                commands = battle.ReorderCommands(commands, true);
            }

            // Execute all bag commands
            for (int i = 0; i < commands.Count && !battle.IsFinished(); i++)
            {
                if (!commands[i].completed && commands[i].commandType == BattleCommandType.Bag)
                {
                    yield return StartCoroutine(ExecuteCommand(commands[i]));
                }
            }

            // Execute all other commands
            for (int i = 0; i < commands.Count && !battle.IsFinished(); i++)
            {
                if (!commands[i].completed)
                {
                    yield return StartCoroutine(ExecuteCommand(commands[i]));
                }
            }
        }
        private IEnumerator ExecuteCommand(BattleCommand command)
        {
            Pokemon commandUser = command.commandUser;

            // only execute if the pokemon is still able to
            // Ex. may have fainted, so can't execute command
            if (battle.CanPokemonStillExecuteCommand(commandUser, command))
            {
                command.inProgress = true;
                switch (command.commandType)
                {
                    // execute as move
                    case BattleCommandType.Fight:
                        yield return StartCoroutine(ExecuteCommandMove(command));
                        break;

                    case BattleCommandType.Recharge:
                        yield return StartCoroutine(ExecuteCommandRecharge(command));
                        break;

                    // switch
                    case BattleCommandType.Party:
                        yield return StartCoroutine(ExecuteCommandParty(command));
                        break;

                    // replace
                    case BattleCommandType.PartyReplace:
                        yield return StartCoroutine(ExecuteCommandReplace(command));
                        break;

                    // bag
                    case BattleCommandType.Bag:
                        yield return StartCoroutine(ExecuteCommandBag(command));
                        break;

                    // run attempt
                    case BattleCommandType.Run:
                        yield return StartCoroutine(ExecuteCommandRun(command));
                        break;
                }
                command.inProgress = false;
            }
            command.completed = true;
        }
        public BattleCommand GetPokemonCommand(Pokemon pokemon)
        {
            for (int j = 0; j < allCommands.Count; j++)
            {
                if (pokemon.IsTheSameAs(allCommands[j].commandUser))
                {
                    return allCommands[j];
                }
            }
            return null;
        }

        private IEnumerator ExecuteCommandMove(BattleCommand command)
        {
            // essential variables
            Pokemon userPokemon = command.commandUser;

            // Form change if it hasn't been done already
            yield return StartCoroutine(PBPFormTransformation(userPokemon, command));

            BattleTeam userTeam = battle.GetTeam(userPokemon);
            List<BattleTeam> targetTeams = new List<BattleTeam>();

            // Move Data
            MoveData masterMoveData = battle.GetPokemonMoveData(
                userPokemon: userPokemon,
                moveID: command.moveID,
                command: command,
                hit: 1);

            // Magnitude
            EffectDatabase.MoveEff.Magnitude.MagnitudeLevel magnitudeLevel = null;
            EffectDatabase.MoveEff.MoveEffect magnitudeEffect_ = masterMoveData.GetEffectNew(MoveEffectType.Magnitude);
            if (magnitudeEffect_ != null)
            {
                EffectDatabase.MoveEff.Magnitude magnitudeEffect = magnitudeEffect_ as EffectDatabase.MoveEff.Magnitude;
                magnitudeLevel = magnitudeEffect.GetAMagnitudeLevel();
            }

            string baseMoveID = command.moveID;
            bool usedZMove = command.isZMove;

            // encore to force another move
            if (!command.isFutureSightMove && !usedZMove)
            {
                MoveData encoreData = null;
                for (int i = 0; i < userPokemon.bProps.moveLimiters.Count && encoreData == null; i++)
                {
                    Pokemon.BattleProperties.MoveLimiter limiter =
                        userPokemon.bProps.moveLimiters[i];
                    if (limiter.effect is EffectDatabase.StatusPKEff.Encore)
                    {
                        string encoreID = limiter.GetMove();
                        if (encoreID != null)
                        {
                            encoreData = MoveDatabase.instance.GetMoveData(encoreID);
                        }
                    }
                }

                if (encoreData != null)
                {
                    if (masterMoveData.ID != encoreData.ID)
                    {
                        // change move
                        MoveCategory oldCategory = masterMoveData.category;

                        masterMoveData = battle.GetPokemonMoveData(
                            userPokemon: userPokemon,
                            moveID: encoreData.ID,
                            command: command,
                            hit: 1);
                        baseMoveID = masterMoveData.ID;
                        if (oldCategory != masterMoveData.category)
                        {
                            command.targetPositions = battle.GetMoveAutoTargets(userPokemon, masterMoveData).ToArray();
                        }
                    }
                }
            
            }

            // run checks to see if move will work
            bool moveSuccess = true;
            bool moveUsedSuccessfully = false;
            bool effectsRanSuccessfully = false;
            bool moveExplicitlyFailed = false;
            bool displayMove = true;
            bool tryToConsumePP = false;
            bool moveWasDisplayed = false;
            MoveData baseMoveData = battle.GetPokemonMoveData(
                userPokemon: userPokemon,
                moveID: baseMoveID,
                command: command,
                overrideZMove: true,
                overrideMaxMove: true,
                hit: 1);

            // some cases where we don't want the move to be displayed

            // can't execute if the user fainted
            if (moveSuccess && !command.isFutureSightMove)
            {
                if (battle.IsPokemonFainted(userPokemon))
                {
                    moveSuccess = false;
                }
            }

            if (moveSuccess && !command.isFutureSightMove)
            {
                if (masterMoveData.GetEffect(MoveEffectType.DestinyBond) == null)
                {
                    userPokemon.bProps.destinyBondMove = null;
                }
            }

            // check statuses
            if (moveSuccess 
                && !command.bypassStatusInterrupt
                && !command.isFutureSightMove)
            {
                // check freeze
                if (moveSuccess)
                {
                    bool isComatose = false;
                    StatusCondition freezeCondition = null;
                    StatusPKData freezeStatusData = null;
                    List<EffectDatabase.AbilityEff.AbilityEffect> effects_ =
                        battle.PBPGetAbilityEffects(userPokemon, AbilityEffectType.Comatose);

                    // Comatose
                    for (int i = 0; i < effects_.Count && freezeStatusData == null; i++)
                    {
                        EffectDatabase.AbilityEff.Comatose comatose =
                            effects_[i] as EffectDatabase.AbilityEff.Comatose;
                        StatusPKData comatoseStatusData = StatusPKDatabase.instance.GetStatusData(comatose.statusID);
                        if (comatoseStatusData.GetEffectNew(PokemonSEType.Freeze) != null)
                        {
                            isComatose = true;
                            freezeStatusData = comatoseStatusData;
                        }
                    }

                    // Status Conditions
                    List<StatusCondition> bConds = battle.PBPGetSCs(userPokemon);
                    for (int i = 0; i < bConds.Count && freezeStatusData == null; i++)
                    {
                        EffectDatabase.StatusPKEff.PokemonSE sleep_ =
                            bConds[i].data.GetEffectNew(PokemonSEType.Freeze);
                        if (sleep_ != null)
                        {
                            EffectDatabase.StatusPKEff.Sleep sleep = sleep_ as EffectDatabase.StatusPKEff.Sleep;
                            if (bConds[i].data.HasTag(PokemonSTag.TurnsDecreaseOnMove))
                            {
                                bConds[i].AdvanceSelf();
                            }
                            if (bConds[i].turnsLeft == 0)
                            {
                                yield return StartCoroutine(HealPokemonSC(
                                    targetPokemon: userPokemon,
                                    condition: bConds[i]
                                    ));
                            }
                            else
                            {
                                freezeCondition = bConds[i];
                                freezeStatusData = bConds[i].data;
                            }
                        }
                    }

                    if (freezeStatusData != null)
                    {
                        if (!isComatose)
                        {
                            bool willThaw = false;

                            EffectDatabase.StatusPKEff.Freeze freeze =
                                freezeStatusData.GetEffectNew(PokemonSEType.Freeze) as EffectDatabase.StatusPKEff.Freeze;

                            // Thaw based on type
                            if (!willThaw
                                && battle.AreTypesContained(
                                    containerTypes: freeze.thawMoveTypes, 
                                    checkType: masterMoveData.moveType)
                                    )
                            {
                                willThaw = true;
                            }

                            // Thaw based on move
                            if (!willThaw)
                            {
                                List<EffectDatabase.MoveEff.MoveEffect> healBeforeUse_ =
                                    masterMoveData.GetEffectsNew(MoveEffectType.HealBeforeUse);
                                for (int i = 0; i < healBeforeUse_.Count && !willThaw; i++)
                                {
                                    EffectDatabase.MoveEff.HealBeforeUse healBeforeUse =
                                        healBeforeUse_[i] as EffectDatabase.MoveEff.HealBeforeUse;
                                    for (int k = 0; k < healBeforeUse.statuses.Count && !willThaw; k++)
                                    {
                                        StatusPKData statusData = 
                                            StatusPKDatabase.instance.GetStatusData(healBeforeUse.statuses[k]);
                                        if (freezeStatusData.ID == statusData.ID
                                            || freezeStatusData.IsABaseID(statusData.ID))
                                        {
                                            willThaw = true;
                                        }
                                    }
                                }
                            }

                            // Thaw based on chance
                            if (!willThaw &&
                                Random.value <= freeze.thawChance)
                            {
                                willThaw = true;
                            }

                            if (!willThaw)
                            {
                                moveSuccess = false;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = freeze.displayText,
                                    pokemonTargetID = userPokemon.uniqueID,
                                    statusID = freezeStatusData.ID
                                });
                            }
                            else
                            {
                                if (freezeCondition != null)
                                {
                                    yield return StartCoroutine(HealPokemonSC(
                                        targetPokemon: userPokemon,
                                        condition: freezeCondition
                                        ));
                                }
                            }
                        
                        }
                    }
                }

                // check sleep
                if (moveSuccess)
                {
                    bool isComatose = false;
                    StatusPKData sleepStatusData = null;
                    List<EffectDatabase.AbilityEff.AbilityEffect> effects_ =
                        battle.PBPGetAbilityEffects(userPokemon, AbilityEffectType.Comatose);

                    // Comatose
                    for (int i = 0; i < effects_.Count && sleepStatusData == null; i++)
                    {
                        EffectDatabase.AbilityEff.Comatose comatose =
                            effects_[i] as EffectDatabase.AbilityEff.Comatose;
                        StatusPKData comatoseStatusData = StatusPKDatabase.instance.GetStatusData(comatose.statusID);
                        if (comatoseStatusData.GetEffectNew(PokemonSEType.Sleep) != null)
                        {
                            isComatose = true;
                            sleepStatusData = comatoseStatusData;
                        }
                    }
                
                    // Status Conditions
                    List<StatusCondition> bConds = battle.PBPGetSCs(userPokemon);
                    for (int i = 0; i < bConds.Count && sleepStatusData == null; i++)
                    {
                        EffectDatabase.StatusPKEff.PokemonSE sleep_ =
                            bConds[i].data.GetEffectNew(PokemonSEType.Sleep);
                        if (sleep_ != null)
                        {
                            EffectDatabase.StatusPKEff.Sleep sleep = sleep_ as EffectDatabase.StatusPKEff.Sleep;
                            if (bConds[i].data.HasTag(PokemonSTag.TurnsDecreaseOnMove))
                            {
                                bConds[i].AdvanceSelf();
                            }
                            if (bConds[i].turnsLeft == 0)
                            {
                                yield return StartCoroutine(HealPokemonSC(
                                    targetPokemon: userPokemon,
                                    condition: bConds[i]
                                    ));
                            }
                            else
                            {
                                sleepStatusData = bConds[i].data;
                            }
                        }
                    }
                
                    if (sleepStatusData != null)
                    {
                        if (!isComatose)
                        {
                            EffectDatabase.StatusPKEff.Sleep sleep =
                                sleepStatusData.GetEffectNew(PokemonSEType.Sleep) as EffectDatabase.StatusPKEff.Sleep;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = sleep.displayText,
                                pokemonTargetID = userPokemon.uniqueID,
                                statusID = sleepStatusData.ID
                            });
                        }

                        // Comatose ignores sleep
                        bool bypassSleep = !isComatose;
                    
                        // Snore ignores sleep
                        if (masterMoveData.GetEffectNew(MoveEffectType.Snore) != null
                            || masterMoveData.GetEffectNew(MoveEffectType.SleepTalk) != null)
                        {
                            bypassSleep = true;
                        }

                        if (!bypassSleep)
                        {
                            moveSuccess = false;
                        }
                    }
                }

                // check flinching
                if (moveSuccess)
                {
                    if (userPokemon.bProps.flinch != null)
                    {
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = userPokemon.bProps.flinch.flinchText,
                            pokemonTargetID = userPokemon.uniqueID
                        });

                        // Steadfast
                        List<Pokemon.Ability> pbAbilities = 
                            battle.PBPGetAbilitiesWithEffect(userPokemon, AbilityEffectType.Steadfast);
                        for (int i = 0; i < pbAbilities.Count; i++)
                        {
                            Pokemon.Ability ability = pbAbilities[i];
                            EffectDatabase.AbilityEff.AbilityEffect steadfast_ =
                                ability.data.GetEffectNew(AbilityEffectType.Steadfast);
                            if (steadfast_ != null)
                            {
                                EffectDatabase.AbilityEff.Steadfast steadfast =
                                    steadfast_ as EffectDatabase.AbilityEff.Steadfast;
                                yield return StartCoroutine(PBPRunAbilityEffect(
                                    pokemon: userPokemon,
                                    ability: ability,
                                    effect_: steadfast,
                                    callback: (result) => { }));
                            }
                        }

                        moveSuccess = false;
                    }
                }

                // check confusion
                if (moveSuccess)
                {
                    StatusCondition condition = battle.GetPokemonFilteredStatus(userPokemon, PokemonSEType.Confusion);
                    if (condition != null)
                    {
                        if (condition.data.HasTag(PokemonSTag.TurnsDecreaseOnMove))
                        {
                            condition.AdvanceSelf();
                        }
                        if (condition.turnsLeft == 0)
                        {
                            yield return StartCoroutine(HealPokemonSC(
                                targetPokemon: userPokemon,
                                condition: condition
                                ));
                        }
                        else
                        {
                            PokemonCEff effect = condition.data.GetEffect(PokemonSEType.Confusion);
                            // send idle message
                            string idleTextID = effect.GetString(0);
                            idleTextID = (idleTextID == "DEFAULT") ? "status-confusion-idle" : idleTextID;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = idleTextID,
                                pokemonTargetID = userPokemon.uniqueID
                            });

                            if (battle.DoesStatusEffectPassChance(
                                effect,
                                condition,
                                targetPokemon: userPokemon
                                ))
                            {
                                // send hit message
                                string hitTextID = effect.GetString(1);
                                hitTextID = (hitTextID == "DEFAULT") ? "status-confusion-hit" : hitTextID;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = hitTextID,
                                    pokemonTargetID = userPokemon.uniqueID
                                });

                                // deal damage
                                int basePower = Mathf.FloorToInt(effect.GetFloat(1));
                                string category = effect.GetBool(0) ? "physical" : "special";
                                int damage = Mathf.FloorToInt(battle.GetMoveBaseDamage(
                                    userPokemon: userPokemon,
                                    targetPokemon: userPokemon,
                                    moveData: null,
                                    overwriteCategory: MoveCategory.Physical,
                                    overwriteBasePower: basePower
                                    ));
                                damage = Mathf.Max(1, damage);
                                int preHP = userPokemon.currentHP;
                                int damageDealt = battle.SubtractPokemonHP(userPokemon, damage);
                                int postHP = userPokemon.currentHP;

                                yield return StartCoroutine(PBPChangePokemonHP(
                                    pokemon: userPokemon,
                                    preHP: preHP,
                                    hpChange: damageDealt,
                                    postHP: postHP
                                    ));

                                // run faint event if it hasn't been done already for the user
                                if (battle.IsPokemonFainted(userPokemon))
                                {
                                    moveSuccess = false;
                                    battle.FaintPokemon(userPokemon);
                                    SendEvent(new Battle.View.Events.PokemonHealthFaint
                                    {
                                        pokemonUniqueID = userPokemon.uniqueID
                                    });
                                    yield return StartCoroutine(UntiePokemon(userPokemon));
                                }
                                moveSuccess = false;
                            }
                        }
                    }
                }

                // check paralysis
                if (moveSuccess)
                {
                    StatusCondition condition = battle.GetPokemonFilteredStatus(userPokemon, PokemonSEType.Paralysis);
                    if (condition != null)
                    {
                        if (condition.data.HasTag(PokemonSTag.TurnsDecreaseOnMove))
                        {
                            condition.AdvanceSelf();
                        }
                        if (condition.turnsLeft == 0)
                        {
                            yield return StartCoroutine(HealPokemonSC(
                                targetPokemon: userPokemon,
                                condition: condition
                                ));
                        }
                        else
                        {
                            PokemonCEff effect = condition.data.GetEffect(PokemonSEType.Paralysis);
                            if (battle.DoesStatusEffectPassChance(
                                effect,
                                condition,
                                targetPokemon: userPokemon
                                ))
                            {
                                // send paralysis message
                                string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "status-paralysis"
                                    : textID;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = textID,
                                    pokemonTargetID = userPokemon.uniqueID
                                });
                                moveSuccess = false;
                            }
                        }
                    }
                }

                // check Infatuation
                if (moveSuccess)
                {
                    EffectDatabase.StatusPKEff.Infatuation infatuation = userPokemon.bProps.infatuation;
                    if (infatuation != null)
                    {
                        Pokemon infatuator = battle.GetPokemonByID(infatuation.infatuator);
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = infatuation.moveText,
                            pokemonUserID = infatuator.uniqueID,
                            pokemonTargetID = userPokemon.uniqueID
                        });

                        // move fail
                        if (infatuation.moveFailChance >= Random.value)
                        {
                            moveSuccess = false;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = infatuation.moveFailText,
                                pokemonUserID = infatuator.uniqueID,
                                pokemonTargetID = userPokemon.uniqueID
                            });
                        }
                    }
                }

                // check Imprison
                if (moveSuccess && !usedZMove)
                {
                    Pokemon imprisonPokemon = battle.PBPGetImprison(userPokemon, masterMoveData);
                    if (imprisonPokemon != null)
                    {
                        moveSuccess = false;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = imprisonPokemon.bProps.imprison.negateText,
                            pokemonUserID = userPokemon.uniqueID,
                            moveID = masterMoveData.ID
                        });
                    }
                }

                // Move-Limiter (Disable, Heal Block, Taunt, Torment)
                if (moveSuccess)
                {
                    for (int i = 0; i < userPokemon.bProps.moveLimiters.Count && moveSuccess; i++)
                    {
                        Pokemon.BattleProperties.MoveLimiter limiter =
                            userPokemon.bProps.moveLimiters[i];
                        if (!limiter.justInitialized || !limiter.effect.canUseMiddleOfTurn)
                        {
                            moveSuccess = false;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = limiter.effect.attemptText,
                                pokemonUserID = userPokemon.uniqueID,
                                moveID = masterMoveData.ID
                            });
                        }
                    }
                }
            }

            // Truant
            if (moveSuccess
                && !command.isFutureSightMove
                && userPokemon.bProps.truantTurns > 0)
            {
                Pokemon.AbilityEffectPair truantPair = 
                    battle.PBPGetAbilityEffectPair(userPokemon, AbilityEffectType.Truant);
                if (truantPair != null)
                {
                    moveSuccess = false;
                    userPokemon.bProps.truantTurns--;
                    PBPShowAbility(userPokemon, truantPair.ability);

                    EffectDatabase.AbilityEff.Truant truant = truantPair.effect as EffectDatabase.AbilityEff.Truant;
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = truant.displayText,
                        pokemonUserID = userPokemon.uniqueID
                    });
                }
            }

            // gravity preventing moves from working
            if (moveSuccess)
            {
                if (battle.BBPIsPokemonAffectedByBSC(userPokemon, battle.gravity))
                {
                    EffectDatabase.StatusBTLEff.BattleSE gravity_ = battle.gravity.data.GetEffectNew(BattleSEType.Gravity);
                    if (gravity_ != null)
                    {
                        EffectDatabase.StatusBTLEff.Gravity gravity = gravity_ as EffectDatabase.StatusBTLEff.Gravity;
                        bool moveCancelled = false;
                        if (gravity.intensifyGravity)
                        {
                            if (masterMoveData.HasTag(MoveTag.CannotUseInGravity))
                            {
                                moveCancelled = true;
                            }
                        }
                
                        if (moveCancelled)
                        {
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = gravity.moveFailText,
                                pokemonUserID = userPokemon.uniqueID,
                                moveID = masterMoveData.ID
                            });
                        }
                        moveSuccess = !moveCancelled;
                    }
                }
            }

            // Focus Punch
            if (moveSuccess
                && !command.isFutureSightMove
                && !string.IsNullOrEmpty(userPokemon.bProps.focusPunchMove)
                && userPokemon.bProps.turnTotalDamageTaken >= 0)
            {
                MoveData focusPunchData = MoveDatabase.instance.GetMoveData(userPokemon.bProps.focusPunchMove);
                MoveEffect focusPunch = focusPunchData.GetEffect(MoveEffectType.FocusPunch);
                SendEvent(new Battle.View.Events.Message
                {
                    message = userPokemon.nickname + " lost its focus and couldn't move!"
                });

                moveSuccess = false;
                moveExplicitlyFailed = true;
            }

            // move will be executed (not including immunities, protect, etc.)
            // display Struggle
            if (moveSuccess 
                && masterMoveData.GetEffect(MoveEffectType.Struggle) != null
                && !command.isFutureSightMove)
            {
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = "move-struggle",
                    pokemonUserID = userPokemon.uniqueID,
                    moveID = masterMoveData.ID
                });
            }

            // display Z-Move
            if (moveSuccess && usedZMove && !command.isFutureSightMove)
            {
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = "zmove-start",
                    pokemonUserID = userPokemon.uniqueID,
                    moveID = masterMoveData.ID
                });
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = "zmove-use",
                    pokemonUserID = userPokemon.uniqueID,
                    moveID = masterMoveData.ID
                });

                // Set Z-Move used for trainer
                Trainer trainer = battle.GetPokemonOwner(userPokemon);
                trainer.bProps.usedZMove = true;
            }

            // display "Pokemon used Move!"
            if (moveSuccess && !command.isFutureSightMove)
            {
                // display move being used
                if (displayMove && command.displayMove)
                {
                    // Protean
                    AbilityData proteanData = battle.PBPGetAbilityDataWithEffect(userPokemon, AbilityEffectType.Protean);
                    if (proteanData != null)
                    {
                        string moveType = masterMoveData.moveType;
                        if (!battle.AreTypesContained(
                            containerType: moveType,
                            checkTypes: battle.PBPGetTypes(userPokemon)))
                        {
                            PBPShowAbility(userPokemon, proteanData);
                        
                            battle.SetPokemonType(userPokemon, moveType);
                            EffectDatabase.AbilityEff.Protean protean =
                                proteanData.GetEffectNew(AbilityEffectType.Protean) as EffectDatabase.AbilityEff.Protean;

                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = protean.displayText,
                                pokemonUserID = userPokemon.uniqueID,
                                typeID = moveType
                            });
                        }
                    }

                    // Stance Change
                    bool isStanceChanged = false;
                    List<Pokemon.Ability> stanceChangeAbilities = 
                        battle.PBPGetAbilitiesWithEffect(userPokemon, AbilityEffectType.StanceChange);
                    for (int i = 0; i < stanceChangeAbilities.Count && !isStanceChanged; i++)
                    {
                        Pokemon.Ability ability = stanceChangeAbilities[i];
                        List<EffectDatabase.AbilityEff.AbilityEffect> stanceChange_ =
                            ability.data.GetEffectsNew(AbilityEffectType.StanceChange);
                        for (int k = 0; k < stanceChange_.Count && !isStanceChanged; k++)
                        {
                            EffectDatabase.AbilityEff.StanceChange stanceChange =
                                stanceChange_[k] as EffectDatabase.AbilityEff.StanceChange;
                            for (int j = 0; j < stanceChange.transformations.Count && !isStanceChanged; j++)
                            {
                                EffectDatabase.AbilityEff.StanceChange.Transformation trns =
                                    stanceChange.transformations[j];
                                if (battle.DoesEffectFilterPass(
                                    effect_: trns.moveCheck,
                                    userPokemon: userPokemon,
                                    targetPokemon: userPokemon,
                                    moveData: masterMoveData
                                    )
                                    && trns.transformation.IsPokemonAPreForm(userPokemon))
                                {
                                    isStanceChanged = true;

                                    PBPShowAbility(userPokemon, ability);
                                    yield return StartCoroutine(PBPChangeForm(
                                        pokemon: userPokemon,
                                        toForm: trns.transformation.toForm,
                                        changeText: trns.displayText,
                                        checkAbility: false
                                        ));
                                }
                            }
                        }
                    }

                    SendEvent(new Battle.View.Events.PokemonMoveUse
                    {
                        pokemonUniqueID = userPokemon.uniqueID,
                        moveID = masterMoveData.ID
                    });
                    moveWasDisplayed = true;
                    tryToConsumePP = true;

                    // Magnitude X!
                    if (magnitudeEffect_ != null && magnitudeLevel != null)
                    {
                        EffectDatabase.MoveEff.Magnitude magnitude = magnitudeEffect_ as EffectDatabase.MoveEff.Magnitude;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = magnitude.displayText,
                            pokemonUserID = userPokemon.uniqueID,
                            moveID = masterMoveData.ID,
                            intArgs = new List<int> { magnitudeLevel.level }
                        });
                    }
                }

                // set the last move used by the pokemon
                if (command.displayMove)
                {
                    battle.SetPokemonLastMove(userPokemon, masterMoveData.ID);
                    // Set choiced move
                    if (command.isExplicitlySelected 
                        && battle.IsPokemonChoiced(userPokemon)
                        && battle.CanMoveBeChoiceLocked(userPokemon, masterMoveData))
                    {
                        userPokemon.bProps.choiceMove = masterMoveData.ID;
                    }
                    if (masterMoveData.GetEffect(MoveEffectType.Copycat) == null)
                    {
                        battle.lastUsedMove = masterMoveData.ID;
                    }

                    // Set Truant
                    if (userPokemon.bProps.truantTurns == 0)
                    {
                        EffectDatabase.AbilityEff.AbilityEffect truant_ =
                            battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.Truant);
                        if (truant_ != null)
                        {
                            EffectDatabase.AbilityEff.Truant truant = truant_ as EffectDatabase.AbilityEff.Truant;
                            userPokemon.bProps.truantTurns = truant.turnsWaiting;
                        }
                    }
                }

                // Unset Protect and Mat Block
                if (masterMoveData.GetEffectNew(MoveEffectType.Protect) == null
                    && masterMoveData.GetEffectNew(MoveEffectType.Endure) == null)
                {
                    battle.ResetPokemonProtect(userPokemon);
                }
            }

            // Z-Effects
            if (moveSuccess && usedZMove)
            {
                List<EffectDatabase.MoveEff.MoveEffect> ZPowerEffects = masterMoveData.GetZEffects();
                BattleHitTarget userHitTarget = new BattleHitTarget(userPokemon);
                userHitTarget.affectedByMove = true;

                yield return StartCoroutine(ExecuteMoveEffects(
                    userPokemon: userPokemon,
                    moveData: masterMoveData,
                    effects: ZPowerEffects,
                    battleHitTargets: new List<BattleHitTarget> { userHitTarget },
                    targetTeams: new List<BattleTeam> { userTeam },
                    callback: (result) =>
                    {

                    }
                    ));
            }

            // Unset Beak Blast / Focus Punch / Shell Trap
            if (battle.IsPokemonOnField(userPokemon) 
                && !battle.IsPokemonFainted(userPokemon)
                && !command.isFutureSightMove)
            {
                userPokemon.bProps.beakBlastMove = null;
                userPokemon.bProps.focusPunchMove = null;
                userPokemon.bProps.shellTrapMove = null;
            }

            // get target positions of move
            BattlePosition[] targetPositions = command.targetPositions;
            List<Pokemon> targetPokemon = battle.GetTargetsLive(targetPositions);
            // try to auto-target alive pokemon if there are no target pokemon the first pass
            if (targetPokemon.Count == 0
                && !command.bypassRedirection)
            {
                targetPositions = battle.GetMoveAutoTargets(
                    pokemon: userPokemon,
                    moveData: masterMoveData,
                    filterAlive: true,
                    useFallBack: false
                    ).ToArray();
                targetPokemon = battle.GetTargetsLive(targetPositions);
            }

            // redirection (ex. Thrash, Uproar)
            MoveTargetType overwriteMoveTarget = MoveTargetType.None;
            if (!command.isFutureSightMove)
            {
                if (masterMoveData.GetEffect(MoveEffectType.Thrash) != null
                || masterMoveData.GetEffect(MoveEffectType.Uproar) != null)
                {
                    overwriteMoveTarget = MoveTargetType.AdjacentOpponent;
                    targetPositions = battle.GetMoveAutoTargets(
                        pokemon: userPokemon,
                        moveData: masterMoveData,
                        overwriteTarget: overwriteMoveTarget,
                        filterAlive: true,
                        useFallBack: false
                        ).ToArray();
                }
                if (masterMoveData.GetEffect(MoveEffectType.Struggle) != null)
                {
                    overwriteMoveTarget = MoveTargetType.AdjacentOpponent;
                    targetPositions = battle.GetMoveAutoTargets(
                        pokemon: userPokemon,
                        moveData: masterMoveData,
                        overwriteTarget: overwriteMoveTarget,
                        filterAlive: true
                        ).ToArray();
                }
                if (masterMoveData.GetEffect(MoveEffectType.Bide) != null)
                {
                    if (userPokemon.bProps.bideTurnsLeft == 0)
                    {
                        BattlePosition userPos = battle.GetPokemonPosition(userPokemon);
                        BattlePosition lastPos = userPokemon.bProps.lastDamagerPosition;
                        if (lastPos != null)
                        {
                            if (battle.ArePositionsAllies(userPos, lastPos))
                            {
                                targetPositions = new BattlePosition[] { lastPos };
                            }
                            else
                            {
                                overwriteMoveTarget = MoveTargetType.AdjacentOpponent;
                                targetPositions = battle.GetMoveAutoTargets(
                                    pokemon: userPokemon,
                                    moveData: masterMoveData,
                                    overwriteTarget: overwriteMoveTarget,
                                    biasPosition: lastPos,
                                    filterAlive: true,
                                    useFallBack: true
                                    ).ToArray();
                            }
                        }
                        else
                        {
                            targetPositions = new BattlePosition[0];
                        }
                    }
                }
                if (masterMoveData.GetEffect(MoveEffectType.Counter) != null)
                {
                    BattlePosition userPos = battle.GetPokemonPosition(userPokemon);
                    BattlePosition lastPos = userPokemon.bProps.lastDamagerPosition;
                    if (lastPos != null)
                    {
                        if (battle.ArePositionsAllies(userPos, lastPos))
                        {
                            targetPositions = new BattlePosition[] { lastPos };
                        }
                        else
                        {
                            overwriteMoveTarget = MoveTargetType.AdjacentOpponent;
                            targetPositions = battle.GetMoveAutoTargets(
                                pokemon: userPokemon,
                                moveData: masterMoveData,
                                overwriteTarget: overwriteMoveTarget,
                                biasPosition: lastPos,
                                filterAlive: true,
                                useFallBack: true
                                ).ToArray();
                        }
                    }
                    else
                    {
                        targetPositions = new BattlePosition[0];
                    }
                }
                EffectDatabase.MoveEff.MoveEffect expandingForce_ = masterMoveData.GetEffectNew(MoveEffectType.ExpandingForce);
                if (expandingForce_ != null)
                {
                    EffectDatabase.MoveEff.ExpandingForce expandingForce = expandingForce_ as EffectDatabase.MoveEff.ExpandingForce;
                    overwriteMoveTarget = expandingForce.newTargetType;
                    targetPositions = battle.GetMoveAutoTargets(
                        pokemon: userPokemon,
                        moveData: masterMoveData,
                        overwriteTarget: overwriteMoveTarget,
                        filterAlive: true
                        ).ToArray();
                }
            }

            // get final targets after redirection
            targetPokemon = battle.GetTargetsLive(targetPositions);

            // consume PP
            if (moveSuccess)
            {
                if (command.consumePP
                    && tryToConsumePP
                    && masterMoveData.GetEffect(MoveEffectType.Struggle) == null)
                {
                    int PPLost = battle.GetPPConsumed(
                        userPokemon: userPokemon,
                        targetPokemon: targetPokemon,
                        moveData: baseMoveData
                        );
                }
            }

            // set values here that always work regardless of failure afterward
            if (moveSuccess
                && !command.isFutureSightMove)
            {
                // Set / Unset Rage
                MoveEffect rageEffect = masterMoveData.GetEffect(MoveEffectType.Rage);
                if (rageEffect != null)
                {
                    if (masterMoveData.ID != userPokemon.bProps.rageMove)
                    {
                        battle.UnsetPokemonRageMove(userPokemon);
                    }
                    if (string.IsNullOrEmpty(userPokemon.bProps.rageMove))
                    {
                        userPokemon.bProps.rageMove = masterMoveData.ID;
                        userPokemon.bProps.rageCounter = 1;
                    }
                }
                else
                {
                    battle.UnsetPokemonRageMove(userPokemon);
                }
            }

            // check if move can be executed
            if (moveSuccess)
            {
                yield return StartCoroutine(TryToFailMove(
                    userPokemon: userPokemon,
                    userTeam: userTeam,
                    moveData: masterMoveData,
                    callback: (result) =>
                    {
                        moveSuccess = !result;
                    }
                    ));
                if (!moveSuccess)
                {
                    moveExplicitlyFailed = true;
                }
            }

            // Powder
            if (moveSuccess 
                && !string.IsNullOrEmpty(userPokemon.bProps.powderMove)
                && !command.isFutureSightMove)
            {
                MoveData powderData = MoveDatabase.instance.GetMoveData(userPokemon.bProps.powderMove);
                MoveEffect effect = powderData.GetEffect(MoveEffectType.Powder);

                List<string> powderTypes = new List<string>();
                for (int i = 2; i < effect.stringParams.Length; i++)
                {
                    powderTypes.Add(effect.stringParams[i]);
                }

                if (powderTypes.Contains(masterMoveData.moveType))
                {
                    float hpPercent = effect.GetFloat(0);
                    int preHP = userPokemon.currentHP;
                    int damage = battle.GetPokemonHPByPercent(userPokemon, hpPercent);
                    damage = Mathf.Max(1, damage);
                    int damageDealt = battle.SubtractPokemonHP(userPokemon, damage);
                    int postHP = userPokemon.currentHP;

                    string textID = effect.GetString(1);
                    textID = (textID == "DEFAULT") ? "move-powder-damage-default" : textID;

                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = textID,
                        pokemonUserID = userPokemon.uniqueID
                    });
                    yield return StartCoroutine(PBPChangePokemonHP(
                        pokemon: userPokemon,
                        preHP: preHP,
                        hpChange: damageDealt,
                        postHP: postHP,
                        checkFaint: true
                        ));

                    moveSuccess = false;
                }
            }

            // check 2-turn attack
            bool isCharging = false;
            if (moveSuccess
                && !command.isFutureSightMove)
            {
                MoveEffect multiTurnEffect = masterMoveData.GetEffect(MoveEffectType.MultiTurnAttack);
                if (multiTurnEffect != null)
                {
                    int attackTurns = Mathf.FloorToInt(multiTurnEffect.GetFloat(0));
                    bool needsToCharge = true;

                    // TODO: Power Herb, Solarbeam (Sunny Day), etc.
                    if (command.iteration < attackTurns)
                    {
                        // send event
                        string textID = multiTurnEffect.GetString(command.iteration - 1);
                        textID = (textID == "DEFAULT") ? "move-default-charge"
                            : textID;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = textID,
                            pokemonUserID = userPokemon.uniqueID,
                            moveID = masterMoveData.ID
                        });

                        // Run Charge effects here
                        yield return StartCoroutine(ExecuteMoveEffects(
                            userPokemon: userPokemon,
                            battleHitTargets: new List<BattleHitTarget>(),
                            targetTeams: new List<BattleTeam>(),
                            moveData: masterMoveData,
                            timing: MoveEffectTiming.OnChargeTurn,
                            callback: (result) =>
                            {

                            }
                            ));
                        yield return StartCoroutine(ExecuteMoveEffectsByTiming(
                            userPokemon: userPokemon,
                            battleHitTargets: new List<BattleHitTarget>(),
                            targetTeams: new List<BattleTeam>(),
                            moveData: masterMoveData,
                            timing: MoveEffectTiming.OnChargeTurn,
                            callback: (result) =>
                            {

                            }
                            ));

                        // put in semi-invulnerable states if possible
                        if (masterMoveData.GetEffect(MoveEffectType.MultiTurnDig) != null)
                        {
                            userPokemon.bProps.inDigState = true;
                        }
                        if (masterMoveData.GetEffect(MoveEffectType.MultiTurnDive) != null)
                        {
                            userPokemon.bProps.inDiveState = true;
                        }
                        if (masterMoveData.GetEffect(MoveEffectType.MultiTurnFly) != null)
                        {
                            userPokemon.bProps.inFlyState = true;
                        }
                        if (masterMoveData.GetEffect(MoveEffectType.MultiTurnShadowForce) != null)
                        {
                            userPokemon.bProps.inShadowForceState = true;
                        }
                    }

                    // Weather Influence (ex. Solar Beam)
                    MoveEffect weatherEffect = masterMoveData.GetEffect(MoveEffectType.InstantMultiTurnWeather);
                    if (needsToCharge && weatherEffect != null)
                    {
                        List<string> weatherList = new List<string>(weatherEffect.stringParams);
                        if (weatherList.Contains(battle.weather.statusID))
                        {
                            needsToCharge = false;
                        }
                    }

                    // If we've charged up enough turns, don't need to charge anymore
                    if (command.iteration >= attackTurns)
                    {
                        needsToCharge = false;
                    }

                    // still charging
                    if (needsToCharge)
                    {
                        command.iteration++;
                        command.consumePP = false;
                        userPokemon.SetNextCommand(command);
                        moveSuccess = false;
                    }
                    // no charging needed
                    else
                    {
                        if (masterMoveData.GetEffect(MoveEffectType.MultiTurnDig) != null)
                        {
                            userPokemon.bProps.inDigState = false;
                        }
                        if (masterMoveData.GetEffect(MoveEffectType.MultiTurnDive) != null)
                        {
                            userPokemon.bProps.inDiveState = false;
                        }
                        if (masterMoveData.GetEffect(MoveEffectType.MultiTurnFly) != null)
                        {
                            userPokemon.bProps.inFlyState = false;
                        }
                        if (masterMoveData.GetEffect(MoveEffectType.MultiTurnShadowForce) != null)
                        {
                            userPokemon.bProps.inShadowForceState = false;
                        }
                    }
                    isCharging = needsToCharge;
                    moveSuccess = !needsToCharge;
                }
            }

            // Future Sight
            if (moveSuccess
                && !command.isFutureSightMove)
            {
                MoveEffect effect = masterMoveData.GetEffect(MoveEffectType.FutureSight);
                if (effect != null)
                {
                    if (battle.CanFutureSightTargets(command.targetPositions))
                    {
                        BattleFutureSightCommand futureCommand = new BattleFutureSightCommand(
                            pokemonID: userPokemon.uniqueID,
                            moveID: masterMoveData.ID,
                            targetPositions: command.targetPositions,
                            Mathf.FloorToInt(effect.GetFloat(0))
                            );
                        battle.AddFutureSightCommand(futureCommand);

                        // TODO: More descriptive text
                        string textID = effect.GetString(0);
                        textID = (textID == "DEFAULT") ? "move-futuresight-start-default" : textID;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = textID,
                            pokemonUserID = userPokemon.uniqueID,
                            moveID = masterMoveData.ID
                        });
                    }
                    else
                    {
                        moveExplicitlyFailed = true;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = "move-FAIL-default",
                            pokemonUserID = userPokemon.uniqueID,
                            moveID = masterMoveData.ID
                        });
                    }
                    moveSuccess = false;
                }
            }

            // de-activate two turn state on failure
            if (!moveSuccess
                && battle.IsPokemonOnField(userPokemon)
                && !battle.IsPokemonFainted(userPokemon)
                && masterMoveData.GetEffect(MoveEffectType.MultiTurnAttack) != null
                && !command.isFutureSightMove)
            {
                // If the two turn move was supposed to hit this turn, deactivate invulnerable states
                if (userPokemon.bProps.nextCommand == null)
                {
                    if (masterMoveData.GetEffect(MoveEffectType.MultiTurnDig) != null)
                    {
                        userPokemon.bProps.inDigState = false;
                    }
                    if (masterMoveData.GetEffect(MoveEffectType.MultiTurnDive) != null)
                    {
                        userPokemon.bProps.inDiveState = false;
                    }
                    if (masterMoveData.GetEffect(MoveEffectType.MultiTurnFly) != null)
                    {
                        userPokemon.bProps.inFlyState = false;
                    }
                    if (masterMoveData.GetEffect(MoveEffectType.MultiTurnShadowForce) != null)
                    {
                        userPokemon.bProps.inShadowForceState = false;
                    }
                }
            }

            // by this point, the move should be properly disrupted
            if (!moveSuccess)
            {
                battle.DisruptPokemon(userPokemon, masterMoveData);
            }

            // Bide
            if (moveSuccess
                && !command.isFutureSightMove)
            {
                MoveEffect effect = masterMoveData.GetEffect(MoveEffectType.Bide);
                if (effect != null)
                {
                    // start biding
                    if (userPokemon.bProps.bideTurnsLeft == -1)
                    {
                        userPokemon.bProps.bideTurnsLeft = Mathf.FloorToInt(effect.GetFloat(1));

                        command.iteration++;
                        command.displayMove = false;
                        command.consumePP = false;
                        userPokemon.SetNextCommand(command);

                        moveSuccess = false;
                    }
                    // continue biding
                    else if (userPokemon.bProps.bideTurnsLeft > 0)
                    {
                        // send event
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = "move-bide-store-default",
                            pokemonUserID = userPokemon.uniqueID,
                            moveID = masterMoveData.ID
                        });

                        command.iteration++;
                        command.consumePP = false;
                        command.displayMove = false;
                        userPokemon.SetNextCommand(command);

                        moveSuccess = false;
                    }
                    // unleash bide
                    else
                    {
                        // send event
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = "move-bide-end-default",
                            pokemonUserID = userPokemon.uniqueID,
                            moveID = masterMoveData.ID
                        });
                    }
                }
            }

            // Check if move will be hijacked (ex. Snatch)
            if (moveSuccess
                && !command.isFutureSightMove)
            {
                yield return StartCoroutine(TryToHijackMove(
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: masterMoveData,
                    originalCommand: command,
                    (result) =>
                    {
                        moveSuccess = !result;
                    }
                    ));
            }

            // Check if move will be reflected (ex. Magic Coat)
            if (moveSuccess)
            {
                yield return StartCoroutine(TryToReflectMove(
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: masterMoveData,
                    originalCommand: command,
                    (magicCoat) =>
                    {
                        moveSuccess = magicCoat == null;
                    }
                    ));

                if (masterMoveData.targetType == MoveTargetType.TeamOpponent)
                {
                    if (battle.IsMoveReflected(userPokemon, targetPokemon, masterMoveData))
                    {
                        moveSuccess = false;
                        yield return StartCoroutine(ExecuteReflectedMove(
                            userPokemon,
                            targetPokemon,
                            masterMoveData,
                            command
                            ));
                    }
                }
            }

            // check if move calls other moves
            bool calledAnotherMove = false;
            BattleCommand callCommand = null;
            BattleCommand meFirstCommand = null;
            List<BattleCommand> instructCommands = new List<BattleCommand>();
            List<BattleCommand> afterYouCommands = new List<BattleCommand>();
            List<BattleCommand> shellTrapCommands = new List<BattleCommand>();
            if (moveExplicitlyFailed
                && command.displayMove
                && masterMoveData.GetEffect(MoveEffectType.Copycat) != null)
            {
                battle.lastUsedMove = masterMoveData.ID;
            }
            if (moveSuccess)
            {
                // Assist
                if (!calledAnotherMove && masterMoveData.GetEffect(MoveEffectType.Assist) != null)
                {
                    calledAnotherMove = true;
                    List<string> assistMoves = battle.GetPokemonAssistMoves(userPokemon);
                    string callMove = assistMoves[Random.Range(0, assistMoves.Count)];
                    callCommand = BattleCommand.CreateMoveCommand(
                        userPokemon,
                        callMove,
                        battle.GetMoveAutoTargets(userPokemon, MoveDatabase.instance.GetMoveData(callMove))
                        );
                }

                // Copycat
                if (!calledAnotherMove && masterMoveData.GetEffect(MoveEffectType.Copycat) != null)
                {
                    calledAnotherMove = true;
                    MoveData callData = MoveDatabase.instance.GetMoveData(battle.lastUsedMove);
                    callCommand = BattleCommand.CreateMoveCommand(
                        userPokemon,
                        callData.ID,
                        battle.GetMoveAutoTargets(userPokemon, callData)
                        );
                }

                // Metronome
                if (!calledAnotherMove && masterMoveData.GetEffect(MoveEffectType.Metronome) != null)
                {
                    calledAnotherMove = true;
                    List<string> metronomeMoves = battle.GetMetronomeMoves(userPokemon);
                    string callMove = metronomeMoves[Random.Range(0, metronomeMoves.Count)];
                    callCommand = BattleCommand.CreateMoveCommand(
                        userPokemon,
                        callMove,
                        battle.GetMoveAutoTargets(userPokemon, MoveDatabase.instance.GetMoveData(callMove))
                        );
                }

                // Mirror Move
                if (!calledAnotherMove && masterMoveData.GetEffect(MoveEffectType.MirrorMove) != null)
                {
                    calledAnotherMove = true;
                    string callMove = userPokemon.bProps.lastMoveTargetedBy;
                    callCommand = BattleCommand.CreateMoveCommand(
                        userPokemon,
                        callMove,
                        battle.GetMoveAutoTargets(userPokemon, MoveDatabase.instance.GetMoveData(callMove))
                        );
                }

                // Nature Power
                if (!calledAnotherMove && masterMoveData.GetEffect(MoveEffectType.NaturePower) != null)
                {
                    calledAnotherMove = true;
                    string callMove = battle.GetNaturePowerMove(userPokemon);
                    callCommand = BattleCommand.CreateMoveCommand(
                        userPokemon,
                        callMove,
                        battle.GetMoveAutoTargets(userPokemon, MoveDatabase.instance.GetMoveData(callMove))
                        );
                }

                // Sleep Talk
                if (!calledAnotherMove && masterMoveData.GetEffect(MoveEffectType.SleepTalk) != null)
                {
                    calledAnotherMove = true;
                    List<string> sleepTalkMoves = battle.GetPokemonSleepTalkMoves(userPokemon);
                    string callMove = sleepTalkMoves[Random.Range(0, sleepTalkMoves.Count)];
                    callCommand = BattleCommand.CreateMoveCommand(
                        userPokemon,
                        callMove,
                        battle.GetMoveAutoTargets(userPokemon, MoveDatabase.instance.GetMoveData(callMove))
                        );
                }
            }

            // check if move is redirected
            if (moveSuccess
                && !command.bypassRedirection
                && !masterMoveData.HasTag(MoveTag.IgnoreRedirection)
                && battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.PropellerTail) == null)
            {
                // Lightning Rod / Storm Drain
                List<BattlePosition> redirectionTargets =
                    battle.GetRedirectionTargets(userPokemon, masterMoveData, overwriteMoveTarget);
                if (redirectionTargets.Count > 0)
                {
                    targetPositions = redirectionTargets.ToArray();
                }

                // Follow Me / Rage Powder
                List<BattlePosition> followMeTargets =
                    battle.GetFollowMeTargets(userPokemon, masterMoveData, overwriteMoveTarget);
                if (followMeTargets.Count > 0)
                {
                    targetPositions = followMeTargets.ToArray();
                }

                // get final targets after redirection
                targetPokemon = battle.GetTargetsLive(targetPositions);
            }

            // Try To Fail With Targets selected
            if (moveSuccess)
            {
                yield return StartCoroutine(TryToFailMoveWithTargets(
                    userPokemon: userPokemon,
                    userTeam: userTeam,
                    moveData: masterMoveData,
                    targetPokemon: targetPokemon,
                    callback: (result) =>
                    {
                        moveSuccess = !result;
                    }
                    ));
                if (!moveSuccess)
                {
                    moveExplicitlyFailed = true;
                }
            }

            // set team targets
            if (moveSuccess)
            {
                for (int i = 0; i < targetPositions.Length; i++)
                {
                    BattleTeam team = battle.GetTeamFromBattlePosition(targetPositions[i]);
                    if (!targetTeams.Contains(team))
                    {
                        targetTeams.Add(team);
                    }
                }
            }

            // Set the last move that targeted these pokemon
            if (moveWasDisplayed)
            {
                for (int i = 0; i < targetPokemon.Count; i++)
                {
                    battle.SetPokemonLastTargeted(targetPokemon[i], userPokemon, masterMoveData.ID);
                }
            }

            // actually try to use the move now on targets
            // establish major variables
            int totalDamageDealt = 0;
            List<BattleHitTarget> lastHitTargets = new List<BattleHitTarget>();
            List<BattleTeam> allHitTeams = new List<BattleTeam>();
            if (moveSuccess)
            {

                // Pre-move effects
                if (!command.isFutureSightMove)
                {
                    // Fling
                    if (masterMoveData.GetEffect(MoveEffectType.Fling) != null)
                    {
                        Item item = battle.GetPokemonItemFiltered(userPokemon, ItemEffectType.Fling);
                        if (item != null)
                        {
                            MoveEffect effect = masterMoveData.GetEffect(MoveEffectType.Fling);
                            string textID = effect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-fling-default" : textID;

                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = "move-bide-store-default",
                                pokemonUserID = userPokemon.uniqueID,
                                itemID = item.itemID
                            });
                        }
                    }

                    // Sky Drop
                    if (masterMoveData.GetEffect(MoveEffectType.SkyDrop) != null)
                    {
                        // if "true", grab step
                        // if "false", execute move
                        userPokemon.bProps.attemptingToSkyDrop = !userPokemon.bProps.attemptingToSkyDrop;
                    }
                    else
                    {
                        userPokemon.bProps.attemptingToSkyDrop = false;
                    }
                }
            
                // Future Sight
                if (command.isFutureSightMove && targetPokemon.Count > 0)
                {
                    MoveEffect effect = masterMoveData.GetEffect(MoveEffectType.FutureSight);
                    if (effect != null)
                    {
                        // TODO: More descriptive
                        List<string> targetPokemonIDs = new List<string>();
                        for (int i = 0; i < targetPokemon.Count; i++)
                        {
                            targetPokemonIDs.Add(targetPokemon[i].uniqueID);
                        }

                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = "move-futuresight-attack-default",
                            moveID = masterMoveData.ID,
                            pokemonListIDs = targetPokemonIDs
                        });
                    }
                }

                // get move hits
                int moveHits = (command.forceOneHit) ? 1 : battle.GetMoveHits(userPokemon, masterMoveData);

                // Parental Bond
                List<EffectDatabase.AbilityEff.ParentalBond.BondedHit> parentalBondHits =
                    new List<EffectDatabase.AbilityEff.ParentalBond.BondedHit>();
                Pokemon.Ability parentalBondAbility = null;
                if (!command.forceOneHit)
                {
                    List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(userPokemon);
                    for (int i = 0; i < userAbilities.Count && parentalBondAbility == null; i++)
                    {
                        Pokemon.Ability ability = userAbilities[i];
                        EffectDatabase.AbilityEff.AbilityEffect parentalBond_ =
                            ability.data.GetEffectNew(AbilityEffectType.ParentalBond);
                        if (parentalBond_ != null)
                        {
                            EffectDatabase.AbilityEff.ParentalBond parentalBond =
                                parentalBond_ as EffectDatabase.AbilityEff.ParentalBond;
                            if (battle.DoEffectFiltersPass(
                                filters: parentalBond.filters,
                                userPokemon: userPokemon,
                                moveData: masterMoveData
                                ))
                            {
                                bool applyParentalBond = true;

                                // certain moves cannot be used multiple times
                                if (applyParentalBond)
                                {
                                    if (masterMoveData.GetEffectNew(MoveEffectType.FaintUser) != null
                                        || masterMoveData.GetEffectNew(MoveEffectType.Fling) != null
                                        || masterMoveData.GetEffectNew(MoveEffectType.Recover) != null
                                        || masterMoveData.GetEffectNew(MoveEffectType.Rollout) != null
                                        || masterMoveData.GetEffectNew(MoveEffectType.Uproar) != null
                                    
                                        || masterMoveData.HasTag(MoveTag.DynamaxMove)
                                        || masterMoveData.HasTag(MoveTag.ZMove))
                                    {
                                        applyParentalBond = false;
                                    }
                                }
                            
                                if (applyParentalBond)
                                {
                                    // set extra hits
                                    parentalBondAbility = ability;
                                    parentalBondHits = parentalBond.bondedHits;
                                    moveHits *= parentalBond.bondedHits.Count;
                                }
                            }
                        }
                    }
                }

                bool isMultiHit = (command.forceOneHit) ? false : battle.IsMultiHitMove(userPokemon, masterMoveData);
                BattlePosition userPosition = battle.GetPokemonPosition(userPokemon);

                List<Pokemon> allHitPokemon = new List<Pokemon>();
                List<Pokemon> alivePokemon = battle.GetPokemonUnfaintedFrom(targetPokemon);
                alivePokemon = battle.GetPokemonOnFieldFrom(alivePokemon);
                alivePokemon = battle.GetPokemonBySpeed(alivePokemon);

                // loop through total hits
                for (int i = 0; i < moveHits; i++)
                {
                    // essential variables
                    int curHit = i + 1;
                    int hitDamageDealt = 0;
                    bool successfulHit = false;
                    bool effectHitSuccessful = false;

                    // Parental Bond Hit
                    EffectDatabase.AbilityEff.ParentalBond.BondedHit parentalBondHit =
                        (parentalBondHits.Count == 0) ? null : parentalBondHits[moveHits % parentalBondHits.Count];
                    if (parentalBondAbility != null)
                    {
                        if (moveHits % parentalBondHits.Count > 0)
                        {
                            PBPShowAbility(userPokemon, parentalBondAbility);
                        }
                    }

                    // Set move data for this specific hit, may change (ex. Colour Change)
                    MoveData moveHitData = battle.GetPokemonMoveData(
                        userPokemon: userPokemon,
                        moveID: masterMoveData.ID,
                        targetPokemon: targetPokemon,
                        command: command,
                        hit: curHit,
                        magnitudeLevel: magnitudeLevel,
                        parentalBondHit: parentalBondHit);

                    // Consume Natural Gift Berry if possible
                    if (masterMoveData.GetEffectNew(MoveEffectType.NaturalGift) != null)
                    {
                        Item naturalGiftItem = battle.PBPGetItemWithEffect(userPokemon, ItemEffectType.NaturalGift);
                        if (naturalGiftItem != null)
                        {
                            if (naturalGiftItem.data.HasTag(ItemTag.Consumable))
                            {
                                userPokemon.bProps.consumedItem = naturalGiftItem.itemID;
                                userPokemon.UnsetItem(naturalGiftItem);
                            }
                        }
                    }

                    // Set ability bypass if possible
                    bool bypassAbility = false;
                    // Mold Breaker bypasses ability immunities
                    if (!bypassAbility)
                    {
                        EffectDatabase.AbilityEff.AbilityEffect moldBreakerEffect =
                            battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                        if (moldBreakerEffect != null)
                        {
                            bypassAbility = true;
                        }

                        // Sunsteel Strike bypasses ability immunities
                        EffectDatabase.MoveEff.MoveEffect effect = moveHitData.GetEffectNew(MoveEffectType.SunteelStrike);
                        if (effect != null)
                        {
                            bypassAbility = true;
                        }
                    }

                    // Set substitute bypass if applicable
                    bool bypassSubstitute = false;

                    // Infiltrator
                    List<EffectDatabase.AbilityEff.AbilityEffect> infiltrators_ =
                        battle.PBPGetAbilityEffects(userPokemon, AbilityEffectType.Infiltrator);
                    for (int k = 0; k < infiltrators_.Count && !bypassSubstitute; k++)
                    {
                        EffectDatabase.AbilityEff.Infiltrator infiltrator =
                            infiltrators_[k] as EffectDatabase.AbilityEff.Infiltrator;
                        if (infiltrator.bypassSubstitute)
                        {
                            bypassSubstitute = true;
                        }
                    }

                    // force bypass or sound move
                    if (moveHitData.HasTag(MoveTag.IgnoreSubstitute)
                        || moveHitData.HasTag(MoveTag.SoundMove))
                    {
                        bypassSubstitute = true;
                    }

                    //  ---Pre-calculation things...---
                
                    List<BattleHitTarget> preBattleHitTargets = new List<BattleHitTarget>();
                
                    // Strong Winds Pre-calc
                    for (int k = 0; k < alivePokemon.Count; k++)
                    {
                        BattleHitTarget curTarget = new BattleHitTarget(alivePokemon[k]);
                        curTarget.effectiveness = battle.GetMoveEffectiveness(userPokemon, moveHitData, curTarget.pokemon);
                        preBattleHitTargets.Add(curTarget);
                    }
                    List<EffectDatabase.StatusBTLEff.StrongWinds> preStrongWinds = 
                        battle.BBPGetStrongWindsEffects(preBattleHitTargets);
                    HashSet<EffectDatabase.StatusBTLEff.StrongWinds> strongWindsUsed = 
                        new HashSet<EffectDatabase.StatusBTLEff.StrongWinds>();
                
                    // ---Finished Pre-calculation---

                    List<BattleTeam> hitTeams = new List<BattleTeam>();
                    List<bool> teamBlocked = new List<bool>();
                    List<BTLEvent_MoveHitTarget> hitTargetEvents = new List<BTLEvent_MoveHitTarget>();

                    // loop through each team
                    List<BattleHitTeam> battleHitTeams = new List<BattleHitTeam>();
                
                    // Add teams based on target type
                    for (int k = 0; k < battle.teams.Count; k++)
                    {
                        BattleTeam curTeam = battle.teams[k];
                        BattleHitTeam hitTeam = new BattleHitTeam(curTeam);
                        if (moveHitData.targetType == MoveTargetType.TeamAlly)
                        {
                            if (hitTeam.team == userTeam)
                            {
                                battleHitTeams.Add(hitTeam);
                            }
                        }
                        else if (moveHitData.targetType == MoveTargetType.TeamOpponent)
                        {
                            if (hitTeam.team != userTeam)
                            {
                                battleHitTeams.Add(hitTeam);
                            }
                        }
                    }
                    // Add teams based on target pokemon
                    for (int k = 0; k < alivePokemon.Count; k++)
                    {
                        BattleTeam curTeam = battle.GetTeam(alivePokemon[k]);
                        bool foundTeam = false;
                        for (int j = 0; j < battleHitTeams.Count && !foundTeam; j++)
                        {
                            if (curTeam == battleHitTeams[j].team)
                            {
                                foundTeam = false;
                            }
                        }
                        if (!foundTeam)
                        {
                            BattleHitTeam hitTeam = new BattleHitTeam(curTeam);
                            battleHitTeams.Add(hitTeam);
                        }
                    }


                    // Check team reflection
                    bool skipHitChecks = false;
                    for (int k = 0; k < battleHitTeams.Count; k++)
                    {
                        BattleHitTeam curHitTeam = battleHitTeams[k];
                        BattleTeam currentTeam = battleHitTeams[k].team;
                        yield return StartCoroutine(TryToTeamProtectMoveHit(
                            userPokemon: userPokemon,
                            moveData: moveHitData,
                            userTeam: userTeam,
                            targetTeam: currentTeam,
                            (protect) =>
                            {
                                if (protect != null)
                                {
                                    skipHitChecks = true;
                                    curHitTeam.protection = protect.Clone();
                                    if (!moveHitData.HasTag(MoveTag.ZMove)
                                        && !moveHitData.HasTag(MoveTag.DynamaxMove)
                                        && !(protect.maxGuard && moveHitData.HasTag(MoveTag.IgnoreMaxGuard))
                                        )
                                    {
                                        curHitTeam.affectedByMove = false;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = protect.protectText,
                                            pokemonUserID = userPokemon.uniqueID,
                                            teamID = currentTeam.teamID
                                        });
                                    }
                                }
                            }
                            ));
                    }

                    // Check team protection
                    if (!skipHitChecks)
                    {
                        for (int k = 0; k < battleHitTeams.Count; k++)
                        {
                            BattleHitTeam curHitTeam = battleHitTeams[k];
                            BattleTeam currentTeam = battleHitTeams[k].team;
                            yield return StartCoroutine(TryToTeamProtectMoveHit(
                                userPokemon: userPokemon,
                                moveData: moveHitData,
                                userTeam: userTeam,
                                targetTeam: currentTeam,
                                (protect) =>
                                {
                                    if (protect != null)
                                    {
                                        skipHitChecks = true;
                                        curHitTeam.protection = protect.Clone();
                                        if (!moveHitData.HasTag(MoveTag.ZMove)
                                            && !moveHitData.HasTag(MoveTag.DynamaxMove)
                                            && !(protect.maxGuard && moveHitData.HasTag(MoveTag.IgnoreMaxGuard))
                                            )
                                        {
                                            curHitTeam.affectedByMove = false;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = protect.protectText,
                                                pokemonUserID = userPokemon.uniqueID,
                                                teamID = currentTeam.teamID
                                            });
                                        }
                                    }
                                }
                                ));
                        }
                    }

                    // loop through each hit pokemon
                    List<BattleHitTarget> battleHitTargets = new List<BattleHitTarget>();
                    for (int k = 0; k < alivePokemon.Count && !skipHitChecks; k++)
                    {
                        Pokemon currentTarget = alivePokemon[k];
                        BattleTeam currentTeam = battle.GetTeam(currentTarget);
                        BattleHitTarget hitTarget = new BattleHitTarget(currentTarget);
                        MoveData moveImpactData = battle.GetPokemonMoveData(
                            userPokemon: userPokemon,
                            moveID: moveHitData.ID,
                            targetPokemon: currentTarget,
                            command: command,
                            hit: curHit,
                            magnitudeLevel: magnitudeLevel,
                            parentalBondHit: parentalBondHit);
                        List<Pokemon.Ability> targetAbilities =
                            battle.PBPGetAbilities(currentTarget, bypassAbility);

                        // Check if team association already blocked move
                        BattleHitTeam curHitTeam = null;
                        for (int j = 0; j < battleHitTeams.Count; j++)
                        {
                            if (currentTeam == battleHitTeams[j].team)
                            {
                                curHitTeam = battleHitTeams[j];
                            }
                        }
                        if (curHitTeam == null)
                        {
                            curHitTeam = new BattleHitTeam(currentTeam);
                            battleHitTeams.Add(curHitTeam);
                        }
                        if (curHitTeam.protection != null)
                        {
                            hitTarget.teamProtection = curHitTeam.protection.Clone();
                        }
                        if (!curHitTeam.affectedByMove)
                        {
                            hitTarget.affectedByMove = false;
                        }

                        hitTarget.effectiveness = battle.GetMoveEffectiveness(userPokemon, moveImpactData, currentTarget);
                    
                        // Check to see if damage will be dealt
                        bool shouldDealDamage = moveImpactData.category == MoveCategory.Physical
                            || moveImpactData.category == MoveCategory.Special;

                        // Pollen Puff avoids teammate damage
                        if (shouldDealDamage
                            && moveImpactData.GetEffect(MoveEffectType.PollenPuff) != null
                            && battle.ArePokemonAllies(userPokemon, currentTarget))
                        {
                            shouldDealDamage = false;
                        }

                        // Status moves override effectiveness
                        if (!shouldDealDamage)
                        {
                            // Status moves have neutral effectiveness, unless explicitly stated
                            if (moveImpactData.GetEffectNew(MoveEffectType.ThunderWave) == null)
                            {
                                hitTarget.effectiveness.rawEffectiveness = 1f;
                            }
                        }

                        // checks to see if the move is reflected
                        if (hitTarget.affectedByMove)
                        {
                            hitTarget.isMoveReflected = battle.IsMoveReflected(userPokemon, currentTarget, moveImpactData);
                            hitTarget.affectedByMove = !hitTarget.isMoveReflected;
                        }

                        // checks to see if the move affects the target beyond reflection

                        // Individual Protection
                        if (hitTarget.affectedByMove)
                        {
                            yield return StartCoroutine(TryToProtectMoveHit(
                                userPokemon: userPokemon,
                                targetPokemon: currentTarget,
                                moveData: moveImpactData,
                                userTeam: userTeam,
                                targetTeam: currentTeam,
                                (protect) =>
                                {
                                    if (protect != null)
                                    {
                                        hitTarget.protection = protect;
                                        if (!moveImpactData.HasTag(MoveTag.ZMove)
                                            && !moveImpactData.HasTag(MoveTag.DynamaxMove)
                                            && !(protect.maxGuard && moveImpactData.HasTag(MoveTag.IgnoreMaxGuard))
                                            )
                                        {
                                            hitTarget.affectedByMove = false;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = protect.protectText,
                                                pokemonUserID = userPokemon.uniqueID,
                                                pokemonTargetID = currentTarget.uniqueID,
                                                teamID = currentTeam.teamID
                                            });
                                        }
                                    }
                                }
                                ));
                        }

                        // Ability / Item Protection
                        if (hitTarget.affectedByMove)
                        {
                            yield return StartCoroutine(TryToFailMoveHit(
                                userPokemon: userPokemon,
                                targetPokemon: currentTarget,
                                moveData: moveImpactData,
                                userTeam: userTeam,
                                targetTeam: currentTeam,
                                (failed) =>
                                {
                                    hitTarget.affectedByMove = !failed;
                                },
                                effectiveness: hitTarget.effectiveness
                                ));
                        }

                        // Check Type Protection
                        if (hitTarget.affectedByMove)
                        {
                            if (hitTarget.effectiveness.GetTotalEffectiveness() == 0)
                            {
                                hitTarget.affectedByMove = false;
                                string textID = (battle.IsSinglesBattle()) ? "move-noeffect-default" 
                                    : "move-noeffect-multi-default";
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = textID,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = currentTarget.uniqueID
                                });
                            }
                        }

                        // Check accuracy
                        if (hitTarget.affectedByMove)
                        {
                            // If we've already hit before, don't check accuracy again
                            bool bypassTraditionalCheck = curHit > 1;

                            if (masterMoveData.HasTag(MoveTag.AccuracyCheckEveryHit))
                            {
                                bypassTraditionalCheck = false;
                            }

                            float accuracy = battle.GetMoveAccuracy(
                                userPokemon: userPokemon, 
                                targetPokemon: alivePokemon[k], 
                                moveData: masterMoveData,
                                bypassTraditionalCheck: bypassTraditionalCheck);
                            // hit the target
                            if ((accuracy >= Random.value && accuracy > 0)
                                || accuracy == -1)
                            {

                            }
                            // missed the target
                            else
                            {
                                hitTarget.missed = true;
                                hitTarget.affectedByMove = false;
                            }
                        }

                        // Execute damage step if affected
                        if (hitTarget.affectedByMove)
                        {
                            // Poltergeist
                            EffectDatabase.MoveEff.MoveEffect poltergeist_ = moveImpactData.GetEffectNew(MoveEffectType.Poltergeist);
                            if (poltergeist_ != null)
                            {
                                EffectDatabase.MoveEff.Poltergeist poltergeist = poltergeist_ as EffectDatabase.MoveEff.Poltergeist;
                                Item targetItem = battle.PBPGetHeldItem(currentTarget);
                                if (targetItem != null)
                                {
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = poltergeist.displayText,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = currentTarget.uniqueID,
                                        itemID = targetItem.itemID
                                    });
                                }
                            }

                            // Spectral Thief
                            MoveEffect spectralThief = moveImpactData.GetEffect(MoveEffectType.SpectralThief);
                            if (spectralThief != null)
                            {
                                if (spectralThief.effectTiming == MoveEffectTiming.BeforeTargetImpact
                                    && spectralThief.effectTargetType == MoveEffectTargetType.Target)
                                {
                                    yield return StartCoroutine(ExecuteMoveEffect(
                                        effect: spectralThief,
                                        moveData: moveHitData,
                                        userPokemon: userPokemon,
                                        targetPokemon: currentTarget,
                                        targetTeam: currentTeam,
                                        callback: (result) =>
                                        {

                                        }
                                        ));
                                }
                            }

                            // Damaging categories
                            if (shouldDealDamage)
                            {
                                // only run damage for pokemon not immune to move, and on the non-sky drop step
                                if (!userPokemon.bProps.attemptingToSkyDrop)
                                {
                                    bool directHit = true;
                                    int damage;
                                    // Damage override
                                    if (battle.DoesMoveOverrideDamage(userPokemon, moveImpactData))
                                    {
                                        damage = battle.GetMoveOverrideDamage(userPokemon, currentTarget, moveImpactData);
                                        hitTarget.effectiveness.rawEffectiveness = 1f;
                                    }
                                    else
                                    {
                                        // Core damage calculation
                                        float presetMultipliers = 1f;

                                        // Critical Hit
                                        hitTarget.criticalHit =
                                            battle.CalculateCriticalHit(userPokemon, currentTarget, moveImpactData);

                                        // Delta Stream / Strong Winds Check here
                                        for (int j = 0; j < preStrongWinds.Count; j++)
                                        {
                                            EffectDatabase.StatusBTLEff.StrongWinds effect = preStrongWinds[j];
                                            List<string> affectedTypes = battle.BBPGetTargetStrongWindTypes(
                                                target: hitTarget,
                                                effect: effect
                                                );

                                            // Force effectiveness if the target will be affected
                                            if (affectedTypes.Count > 0)
                                            {
                                                for (int l = 0; l < affectedTypes.Count; l++)
                                                {
                                                    hitTarget.effectiveness.SetType(
                                                        affectedTypes[l],
                                                        effect.GetEffectiveness()
                                                        );
                                                }

                                                if (!strongWindsUsed.Contains(effect))
                                                {
                                                    strongWindsUsed.Add(effect);
                                                    SendEvent(new Battle.View.Events.MessageParameterized
                                                    {
                                                        messageCode = effect.changeText,
                                                        pokemonUserID = userPokemon.uniqueID,
                                                        pokemonTargetID = currentTarget.uniqueID,
                                                        typeIDs = affectedTypes
                                                    });
                                                }

                                                break;
                                            }
                                        }

                                        // move-type items here
                                        Item moveTypeItem = null;
                                        ItemEffect moveTypeEffect = null;

                                        // Only activate berry on direct contact
                                        if (bypassSubstitute || string.IsNullOrEmpty(currentTarget.bProps.substituteMove))
                                        {
                                            if (hitTarget.effectiveness.GetTotalEffectiveness() == 1f)
                                            {
                                                moveTypeItem = battle.GetPokemonItemFiltered(
                                                    currentTarget,
                                                    ItemEffectType.TypeBerry);
                                                if (moveTypeItem != null)
                                                {
                                                    moveTypeEffect =
                                                        moveTypeItem.data.GetEffect(ItemEffectType.TypeBerry);
                                                }
                                            }
                                            else if (hitTarget.effectiveness.GetTotalEffectiveness() > 1f)
                                            {
                                                moveTypeItem = battle.GetPokemonItemFiltered(
                                                    currentTarget,
                                                    ItemEffectType.TypeBerrySuperEffective);
                                                if (moveTypeItem != null)
                                                {
                                                    moveTypeEffect =
                                                        moveTypeItem.data.GetEffect(ItemEffectType.TypeBerrySuperEffective);
                                                }
                                            }
                                        }

                                        // Try to activate item
                                        if (moveTypeItem != null)
                                        {
                                            List<string> typesAffected = new List<string>();
                                            for (int j = 1; j < moveTypeEffect.stringParams.Length; j++)
                                            {
                                                typesAffected.Add(moveTypeEffect.stringParams[j]);
                                            }

                                            if (typesAffected.Contains(moveImpactData.moveType))
                                            {
                                                // Test if item can be consumed
                                                bool canConsumeItem = false;
                                                yield return StartCoroutine(TryToConsumeItem(
                                                    pokemon: currentTarget,
                                                    holderPokemon: currentTarget,
                                                    item: moveTypeItem,
                                                    (result) =>
                                                    {
                                                        canConsumeItem = result;
                                                    },
                                                    apply: false
                                                    ));

                                                // Consume the item, modify the damage
                                                if (canConsumeItem)
                                                {
                                                    presetMultipliers *= moveTypeEffect.GetFloat(0);

                                                    string textID = moveTypeEffect.GetString(0);
                                                    textID = (textID == "DEFAULT") ? "item-typeberry-default"
                                                        : textID;

                                                    yield return StartCoroutine(ConsumeItem(
                                                        pokemon: currentTarget,
                                                        holderPokemon: currentTarget,
                                                        item: moveTypeItem,
                                                        consumeText: textID,
                                                        typeID: moveImpactData.moveType,
                                                        callback: (result) =>
                                                        {

                                                        }
                                                        ));
                                                }
                                            }
                                        }

                                        // Run main damage calculation
                                        damage = battle.GetMoveDamage(
                                            userPokemon: userPokemon,
                                            targetPokemon: currentTarget,
                                            moveData: moveImpactData,
                                            criticalHit: hitTarget.criticalHit,
                                            bypassAbility: bypassAbility,
                                            typeEffectiveness: hitTarget.effectiveness,
                                            presetMultipliers: presetMultipliers);
                                    }

                                    // Hit a protection move, but still burst through
                                    if (hitTarget.teamProtection != null
                                        || hitTarget.protection != null)
                                    {
                                        damage = Mathf.FloorToInt(damage * 0.25f);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = "zmove-protect",
                                            pokemonTargetID = currentTarget.uniqueID
                                        });
                                    }

                                    // Substitute / Disguise / Ice Face Check
                                    if (!bypassSubstitute)
                                    {
                                        // Substitute
                                        if (directHit && !string.IsNullOrEmpty(currentTarget.bProps.substituteMove))
                                        {
                                            directHit = false;
                                            hitTarget.damageDealt = battle.SubtractSubstituteHP(currentTarget, damage);
                                            hitTarget.subDamage = hitTarget.damageDealt;
                                            hitTarget.affectedByMove = false;
                                        }

                                        // Disguise / Ice Face Check
                                        for (int j = 0; j < targetAbilities.Count && directHit; j++)
                                        {
                                            EffectDatabase.AbilityEff.AbilityEffect disguise_ =
                                                targetAbilities[j].data.GetEffectNew(AbilityEffectType.Disguise);
                                            if (disguise_ != null)
                                            {
                                                EffectDatabase.AbilityEff.Disguise disguise =
                                                    disguise_ as EffectDatabase.AbilityEff.Disguise;
                                                if (battle.DoEffectFiltersPass(
                                                    filters: disguise.filters,
                                                    userPokemon: userPokemon,
                                                    targetPokemon: currentTarget,
                                                    moveData: moveHitData
                                                    ))
                                                {
                                                    if (disguise.IsPokemonDisguised(currentTarget))
                                                    {
                                                        directHit = false;
                                                        hitTarget.disguise = targetAbilities[j];
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    // Only reduce pokemon HP on a direct hit
                                    if (directHit)
                                    {
                                        hitTarget.preHP = currentTarget.currentHP;

                                        // Check if the damage would faint the pokemon usually
                                        // TODO: Endure, False Swipe, Focus Sash, Sturdy
                                        if (damage >= currentTarget.currentHP)
                                        {
                                            bool faintAvoided = false;

                                            // run checks here

                                            // False Swipe
                                            if (!faintAvoided && moveImpactData.GetEffect(MoveEffectType.FalseSwipe) != null)
                                            {
                                                faintAvoided = true;
                                            }

                                            // Endure
                                            if (!faintAvoided && currentTarget.bProps.endure != null)
                                            {
                                                faintAvoided = true;
                                                hitTarget.endure = currentTarget.bProps.endure.Clone();
                                            }

                                            // Sturdy
                                            if (!faintAvoided)
                                            {
                                                List<Pokemon.AbilityEffectPair> sturdyAbilities =
                                                    battle.PBPGetAbilityEffectPairs(
                                                        currentTarget, 
                                                        AbilityEffectType.Sturdy, 
                                                        bypassAbility);
                                                for (int j = 0; j < sturdyAbilities.Count && !faintAvoided; j++)
                                                {
                                                    Pokemon.AbilityEffectPair abilityPair = sturdyAbilities[j];
                                                    EffectDatabase.AbilityEff.Sturdy sturdy =
                                                        abilityPair.effect as EffectDatabase.AbilityEff.Sturdy;
                                                    if (battle.DoEffectFiltersPass(
                                                        filters: sturdy.filters,
                                                        userPokemon: userPokemon,
                                                        targetPokemon: currentTarget,
                                                        moveData: moveImpactData
                                                        ))
                                                    {
                                                        if (battle.GetPokemonHPAsPercentage(currentTarget) >= sturdy.hpThreshold
                                                            && Random.value <= sturdy.chance)
                                                        {
                                                            faintAvoided = true;
                                                            hitTarget.sturdyPair = abilityPair;
                                                        }
                                                    }
                                                }
                                            }

                                            // Focus Band / Focus Sash
                                            if (!faintAvoided)
                                            {
                                                List<Item> focusBandItems =
                                                    battle.PBPGetItemsWithEffect(currentTarget, ItemEffectType.FocusBand);
                                                for (int j = 0; j < focusBandItems.Count && !faintAvoided; j++)
                                                {
                                                    Item focusBandItem = focusBandItems[j];
                                                    EffectDatabase.ItemEff.FocusBand focusBand =
                                                        focusBandItem.data.GetEffectNew(ItemEffectType.FocusBand)
                                                        as EffectDatabase.ItemEff.FocusBand;
                                                    if (battle.DoEffectFiltersPass(
                                                        filters: focusBand.filters,
                                                        userPokemon: userPokemon,
                                                        targetPokemon: currentTarget,
                                                        moveData: moveImpactData
                                                        ))
                                                    {
                                                        if (battle.GetPokemonHPAsPercentage(currentTarget) >= focusBand.hpThreshold
                                                            && Random.value <= focusBand.chance)
                                                        {
                                                            faintAvoided = true;
                                                            hitTarget.focusBand = focusBandItem;
                                                        }
                                                    }
                                                }
                                            }

                                            if (faintAvoided)
                                            {
                                                damage = currentTarget.currentHP - 1;
                                            }
                                        }

                                        hitTarget.damageDealt = battle.SubtractPokemonHP(currentTarget, damage);
                                        hitTarget.postHP = currentTarget.currentHP;
                                        hitTarget.postHPPercent = battle.GetPokemonHPAsPercentage(currentTarget);
                                        hitTarget.fainted = hitTarget.postHP == 0;
                                    }
                                    hitDamageDealt += hitTarget.damageDealt;
                                }
                            }
                        }

                        if (!hitTarget.affectedByMove)
                        {
                            hitTarget.effectiveness.rawEffectiveness = 1f;
                        }
                        else
                        {
                            if (!allHitPokemon.Contains(currentTarget))
                            {
                                allHitPokemon.Add(currentTarget);
                            }
                        }

                        // add team
                        if (hitTarget.affectedByMove || hitTarget.subDamage >= 0)
                        {
                            moveUsedSuccessfully = true;
                            successfulHit = true;
                            if (!hitTeams.Contains(currentTeam))
                            {
                                hitTeams.Add(currentTeam);
                            }
                            if (!allHitTeams.Contains(currentTeam))
                            {
                                allHitTeams.Add(currentTeam);
                            }
                        }

                        // check faint status
                        hitTarget.fainted = battle.IsPokemonFainted(currentTarget);

                        hitTarget.destinyBondMove = currentTarget.bProps.destinyBondMove;
                        battleHitTargets.Add(hitTarget);
                    }

                    // send the move hit event
                    // TODO: Come back to handle moves
                    PBS.Battle.View.Events.PokemonMoveHit moveHitEventNew = new Battle.View.Events.PokemonMoveHit
                    {
                        pokemonUniqueID = userPokemon.uniqueID,
                        moveID = moveHitData.ID,
                        currentHit = curHit
                    };
                    List<PBS.Battle.View.Events.PokemonMoveHitTarget> hitTargetsNew = new List<Battle.View.Events.PokemonMoveHitTarget>();
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        hitTargetsNew.Add(new Battle.View.Events.PokemonMoveHitTarget(battleHitTargets[k]));
                    }
                    moveHitEventNew.hitTargets = new List<Battle.View.Events.PokemonMoveHitTarget>(hitTargetsNew);
                    SendEvent(moveHitEventNew);

                    // Only run some effects on the first hit
                    if (curHit == 1)
                    {
                        // Celebrate
                        if (moveHitData.GetEffect(MoveEffectType.Celebrate) != null)
                        {
                            MoveEffect effect = moveHitData.GetEffect(MoveEffectType.Celebrate);
                            string textID = effect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-celebrate" : textID;
                            
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                trainerID = battle.GetPokemonOwner(userPokemon).playerID
                            });
                        }

                        // Wish
                        if (moveHitData.GetEffect(MoveEffectType.Wish) != null
                            && !command.isFutureSightMove)
                        {
                            MoveEffect effect = moveHitData.GetEffect(MoveEffectType.Wish);
                            BattlePosition[] wishPositions;
                            if (moveHitData.GetEffect(MoveEffectType.WishUserPosition) != null)
                            {
                                wishPositions = new BattlePosition[] { userPosition };
                            }
                            else
                            {
                                wishPositions = command.targetPositions;
                            }

                            BattleWishCommand wishCommand = new BattleWishCommand(
                                pokemonID: userPokemon.uniqueID,
                                moveID: masterMoveData.ID,
                                wishPositions: wishPositions,
                                Mathf.FloorToInt(effect.GetFloat(1)),
                                battle.GetPokemonHPByPercent(userPokemon, effect.GetFloat(0))
                                );

                            battle.AddWishCommand(wishCommand);
                            string textID = effect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-wish-start" : textID;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                moveID = masterMoveData.ID
                            });
                        }

                        // Lose HP after use of move (Mind Blown)
                        if (moveHitData.GetEffect(MoveEffectType.HPLoss) != null
                            && battle.PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.MagicGuard) == null
                            && !command.isFutureSightMove)
                        {
                            MoveEffect effect = moveHitData.GetEffect(MoveEffectType.HPLoss);
                            bool loseHP = true;

                            // do not lose HP on no targets available (ex. Mind Blown)
                            if (effect.GetBool(0))
                            {
                                if (battleHitTargets.Count == 0)
                                {
                                    loseHP = false;
                                }
                            }

                            // Lose HP
                            if (loseHP)
                            {
                                int preHP = userPokemon.currentHP;
                                int damage = battle.GetPokemonHPByPercent(
                                    userPokemon,
                                    effect.GetFloat(0),
                                    roundDown: false,
                                    roundUp: true);
                                int damageDealt = battle.SubtractPokemonHP(userPokemon, damage);
                                int postHP = userPokemon.currentHP;

                                yield return StartCoroutine(PBPChangePokemonHP(
                                    pokemon: userPokemon,
                                    preHP: preHP,
                                    hpChange: damageDealt,
                                    postHP: postHP
                                    ));
                            }
                        }

                        // Explosion (self-faint)
                        // Lose HP after use of move
                        else if (moveHitData.GetEffect(MoveEffectType.FaintUser) != null
                            && !command.isFutureSightMove)
                        {
                            MoveEffect effect = moveHitData.GetEffect(MoveEffectType.FaintUser);
                            bool loseHP = true;

                            // do not lose HP on no targets available (ex. Explosion)
                            if (effect.GetBool(0))
                            {
                                if (battleHitTargets.Count == 0)
                                {
                                    loseHP = false;
                                }
                            }

                            // Lose HP
                            if (loseHP)
                            {
                                int preHP = userPokemon.currentHP;
                                int damage = userPokemon.maxHP;
                                int damageDealt = battle.SubtractPokemonHP(userPokemon, damage);
                                int postHP = userPokemon.currentHP;

                                yield return StartCoroutine(PBPChangePokemonHP(
                                    pokemon: userPokemon,
                                    preHP: preHP,
                                    hpChange: damageDealt,
                                    postHP: postHP
                                    ));
                            }
                        }
                
                    }

                    // record hit variables for each target
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        int damage = currentTarget.damageDealt;
                        int subDamage = currentTarget.subDamage;
                        if (currentTarget.affectedByMove)
                        {
                            if (!battle.IsPokemonFainted(currentTarget.pokemon))
                            {
                                if (moveHitData.category == MoveCategory.Physical)
                                {
                                    currentTarget.pokemon.bProps.turnPhysicalDamageTaken += damage;
                                }
                                else if (moveHitData.category == MoveCategory.Special)
                                {
                                    currentTarget.pokemon.bProps.turnSpecialDamageTaken += damage;
                                }
                                if (currentTarget.pokemon.bProps.bideTurnsLeft > 0)
                                {
                                    currentTarget.pokemon.bProps.bideDamageTaken += damage;
                                }
                                if (damage >= 0)
                                {
                                    currentTarget.pokemon.bProps.wasStruckForDamage = true;
                                    currentTarget.pokemon.bProps.lastDamagerPosition = userPosition;
                                }
                                currentTarget.pokemon.bProps.lastTargeterPosition = userPosition;

                                if (battle.ArePokemonEnemies(userPokemon, currentTarget.pokemon))
                                {
                                    currentTarget.pokemon.bProps.wasHitByOpponent = true;
                                }
                                if (battle.ArePokemonAllies(userPokemon, currentTarget.pokemon))
                                {
                                    currentTarget.pokemon.bProps.wasHitByAlly = true;
                                }
                            }

                        }
                    }

                    // run survival checks (Endure, Focus Sash, Sturdy, etc.)
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove)
                        {
                            // Endure
                            if (currentTarget.endure != null)
                            {
                                EffectDatabase.MoveEff.Endure endure = currentTarget.endure;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = endure.protectText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = currentTarget.pokemon.uniqueID
                                });
                            }
                            // Sturdy
                            else if (currentTarget.sturdyPair != null)
                            {
                                PBPShowAbility(currentTarget.pokemon, currentTarget.sturdyPair.ability);
                                EffectDatabase.AbilityEff.Sturdy sturdy =
                                    currentTarget.sturdyPair.effect as EffectDatabase.AbilityEff.Sturdy;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = sturdy.displayText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = currentTarget.pokemon.uniqueID
                                });
                            }
                            // Focus Band / Focus Sash
                            else if (currentTarget.focusBand != null)
                            {
                                EffectDatabase.ItemEff.FocusBand focusBand =
                                    currentTarget.focusBand.data.GetEffectNew(ItemEffectType.FocusBand)
                                    as EffectDatabase.ItemEff.FocusBand;

                                yield return StartCoroutine(ConsumeItem(
                                    pokemon: currentTarget.pokemon,
                                    item: currentTarget.focusBand,
                                    holderPokemon: currentTarget.pokemon,
                                    consumeText: focusBand.displayText,
                                    callback: (result) => { }));
                            }
                        }
                    }

                    // run HPDrain checks
                    EffectDatabase.MoveEff.MoveEffect absorb_ = moveHitData.GetEffectNew(MoveEffectType.HPDrain);
                    MoveEffect HPDrainEffect = moveHitData.GetEffect(MoveEffectType.HPDrain);
                    if (absorb_ != null)
                    {
                        EffectDatabase.MoveEff.Absorb absorb = absorb_ as EffectDatabase.MoveEff.Absorb;
                        for (int k = 0; k < battleHitTargets.Count; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            int damage = currentTarget.damageDealt;
                            if (damage > 0)
                            {
                                if (battle.IsPokemonOnFieldAndAble(userPokemon))
                                {
                                    // TODO: Big Root increases drainedHP
                                    int drainedHP = Mathf.FloorToInt(absorb.healPercent * damage);

                                    bool ignoreDrain = false;
                                    List<Pokemon.Ability> targetAbilities = 
                                        battle.PBPGetAbilities(currentTarget.pokemon);
                                    if (battle.IsPokemonOnFieldAndAble(userPokemon))
                                    {
                                        for (int j = 0; j < targetAbilities.Count && !ignoreDrain; j++)
                                        {
                                            Pokemon.Ability ability = targetAbilities[j];
                                            EffectDatabase.AbilityEff.AbilityEffect liquidOoze_ =
                                                ability.data.GetEffectNew(AbilityEffectType.LiquidOoze);

                                            if (liquidOoze_ != null)
                                            {
                                                EffectDatabase.AbilityEff.LiquidOoze liquidOoze =
                                                    liquidOoze_ as EffectDatabase.AbilityEff.LiquidOoze;
                                                if (battle.DoEffectFiltersPass(
                                                    filters: liquidOoze.filters,
                                                    userPokemon: currentTarget.pokemon,
                                                    targetPokemon: userPokemon
                                                    ))
                                                {
                                                    bool applyLiquidOoze = true;

                                                    if (applyLiquidOoze)
                                                    {
                                                        ignoreDrain = true;

                                                        PBPShowAbility(currentTarget.pokemon, ability);
                                                        int liquidOozeDamage = 
                                                            Mathf.FloorToInt(drainedHP * liquidOoze.damagePercent);
                                                        yield return StartCoroutine(PBPDamagePokemon(
                                                            pokemon: userPokemon,
                                                            HPToLose: liquidOozeDamage
                                                            ));
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    // is user prevented from healing?
                                    if (!ignoreDrain)
                                    {
                                        ignoreDrain = battle.IsPokemonPreventedFromHealing(userPokemon);
                                    }

                                    // heal HP here
                                    if (userPokemon.currentHP < userPokemon.maxHP && !ignoreDrain)
                                    {
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = absorb.displayText,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = currentTarget.pokemon.uniqueID
                                        });

                                        yield return StartCoroutine(PBPHealPokemon(
                                            pokemon: userPokemon,
                                            HPToAdd: drainedHP
                                            ));
                                    }
                                }
                            }
                        }
                    }

                    // run mimic
                    MoveEffect mimicEffect = moveHitData.GetEffect(MoveEffectType.Mimic);
                    if (mimicEffect != null
                        && !battle.IsPokemonFainted(userPokemon)
                        && !command.isFutureSightMove)
                    {
                        // attempt to mimic the first pokemon available
                        Pokemon enablerPokemon = null;
                        for (int k = 0; k < battleHitTargets.Count; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            if (currentTarget.affectedByMove)
                            {
                                enablerPokemon = currentTarget.pokemon;
                                break;
                            }
                        }

                        // Set up mimic move
                        if (enablerPokemon != null)
                        {
                            userPokemon.bProps.mimicBaseMove = moveHitData.ID;
                            userPokemon.bProps.mimicMoveslot = new Pokemon.Moveslot(enablerPokemon.bProps.lastMove);
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = "move-mimic-success-default",
                                pokemonUserID = userPokemon.uniqueID,
                                moveID = enablerPokemon.bProps.lastMove
                            });
                        }
                        else
                        {
                            moveExplicitlyFailed = true;
                        }
                    }

                    // run sketch
                    MoveEffect sketchEffect = moveHitData.GetEffect(MoveEffectType.Sketch);
                    if (sketchEffect != null
                        && !battle.IsPokemonFainted(userPokemon)
                        && !command.isFutureSightMove)
                    {
                        // attempt to mimic the first pokemon available
                        Pokemon enablerPokemon = null;
                        for (int k = 0; k < battleHitTargets.Count; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            if (currentTarget.affectedByMove)
                            {
                                enablerPokemon = currentTarget.pokemon;
                                break;
                            }
                        }

                        // Set up sketch move
                        if (enablerPokemon != null)
                        {
                            string textID = sketchEffect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-sketch-success-default" : textID;

                            userPokemon.bProps.sketchBaseMove = moveHitData.ID;
                            userPokemon.bProps.sketchMoveslot = new Pokemon.Moveslot(enablerPokemon.bProps.lastMove);
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                moveID = enablerPokemon.bProps.lastMove
                            });
                        }
                        else
                        {
                            moveExplicitlyFailed = true;
                        }
                    }

                    // run transform
                    MoveEffect transformEffect = moveHitData.GetEffect(MoveEffectType.Transform);
                    if (transformEffect != null
                        && !battle.IsPokemonFainted(userPokemon)
                        && !command.isFutureSightMove)
                    {
                        // attempt to mimic the first pokemon available
                        Pokemon enablerPokemon = null;
                        for (int k = 0; k < battleHitTargets.Count; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            if (currentTarget.affectedByMove)
                            {
                                enablerPokemon = currentTarget.pokemon;
                                break;
                            }
                        }

                        // Set up transform
                        if (enablerPokemon != null)
                        {
                            string textID = transformEffect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-transform-default" : textID;

                            string prePokemon = userPokemon.pokemonID;
                            string postPokemon = enablerPokemon.pokemonID;
                            battle.PBPTransformIntoPokemon(userPokemon, enablerPokemon);
                            UpdateClients();
                            SendEvent(new Battle.View.Events.PokemonChangeForm
                            {
                                pokemonUniqueID = userPokemon.uniqueID,
                                preForm = prePokemon,
                                postForm = postPokemon
                            });

                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                pokemonTargetID = enablerPokemon.uniqueID
                            });
                        }
                        else
                        {
                            moveExplicitlyFailed = true;
                        }
                    }

                    // run me first
                    MoveEffect meFirstEffect = moveHitData.GetEffect(MoveEffectType.MeFirst);
                    if (meFirstEffect != null
                        && !battle.IsPokemonFainted(userPokemon)
                        && !command.isFutureSightMove)
                    {
                        // attempt to use me first on the first pokemon available
                        for (int k = 0; k < battleHitTargets.Count; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            if (currentTarget.affectedByMove)
                            {
                                for (int j = 0; j < allCommands.Count; j++)
                                {
                                    if (currentTarget.pokemon.IsTheSameAs(allCommands[j].commandUser)
                                        && allCommands[j].commandType == BattleCommandType.Fight)
                                    {
                                        MoveData callData = MoveDatabase.instance.GetMoveData(allCommands[j].moveID);
                                        meFirstCommand = BattleCommand.CreateMoveCommand(
                                            userPokemon,
                                            allCommands[j].moveID,
                                            battle.GetMoveAutoTargets(userPokemon, callData)
                                            );
                                        meFirstCommand.consumePP = false;
                                        meFirstCommand.isMoveCalled = true;
                                        meFirstCommand.bypassStatusInterrupt = true;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }

                    // run move effects for each target (that was hit), on non-sky drop turn
                    if (successfulHit && !userPokemon.bProps.attemptingToSkyDrop)
                    {
                        yield return StartCoroutine(ExecuteMoveEffects(
                            userPokemon: userPokemon,
                            battleHitTargets: battleHitTargets,
                            targetTeams: hitTeams,
                            moveData: moveHitData,
                            (result) =>
                            {
                                if (result)
                                {
                                    effectsRanSuccessfully = true;
                                }
                            },
                            timing: MoveEffectTiming.AfterTargetImpact
                            ));
                        yield return StartCoroutine(ExecuteMoveEffectsByTiming(
                            userPokemon: userPokemon,
                            battleHitTargets: battleHitTargets,
                            targetTeams: hitTeams,
                            moveData: moveHitData,
                            timing: MoveEffectTiming.AfterTargetImpact,
                            callback: (result) =>
                            {
                                if (result)
                                {
                                    effectsRanSuccessfully = true;
                                }
                            }
                            ));
                    }

                    // Execute after events (Protection indicators, contact abilities, items, etc.)
                    yield return StartCoroutine(ExecuteAfterMoveEvents(userPokemon, battleHitTargets, command, moveHitData));

                    // Check HP-Trigger Berries
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget curTarget = battleHitTargets[k];
                        if (battle.IsPokemonOnFieldAndAble(curTarget.pokemon))
                        {
                            // Check HP-Trigger Berry
                            List<Item> sitrusItems = 
                                battle.PBPGetItemsWithEffect(curTarget.pokemon, ItemEffectType.TriggerOnHPLoss);
                            for (int j = 0; j < sitrusItems.Count; j++)
                            {
                                Item curItem = sitrusItems[j];

                                EffectDatabase.ItemEff.TriggerSitrusBerry sitrusBerry =
                                    curItem.data.GetEffectNew(ItemEffectType.TriggerOnHPLoss)
                                    as EffectDatabase.ItemEff.TriggerSitrusBerry;
                                float triggerThreshold = sitrusBerry.hpThreshold;

                                // Gluttony
                                List<EffectDatabase.AbilityEff.AbilityEffect> gluttony_ =
                                    battle.PBPGetAbilityEffects(
                                        pokemon: curTarget.pokemon, 
                                        effectType: AbilityEffectType.Gluttony);
                                for (int l = 0; l < gluttony_.Count; l++)
                                {
                                    EffectDatabase.AbilityEff.Gluttony gluttony =
                                        gluttony_[l] as EffectDatabase.AbilityEff.Gluttony;
                                    if (triggerThreshold >= gluttony.minItemHPThreshold
                                        && triggerThreshold <= gluttony.maxItemHPThreshold)
                                    {
                                        if (curItem.data.pocket == ItemPocket.Berries)
                                        {
                                            triggerThreshold *= gluttony.thresholdScale;
                                        }
                                    }
                                }

                                triggerThreshold = Mathf.Min(triggerThreshold, 1f);
                                if (curTarget.preHPPercent > triggerThreshold
                                    && curTarget.postHPPercent <= triggerThreshold)
                                {
                                    yield return StartCoroutine(TryToConsumeItem(
                                        pokemon: curTarget.pokemon,
                                        holderPokemon: curTarget.pokemon,
                                        item: curItem,
                                        (result) => { }
                                        ));
                                }
                            }
                        }
                    }

                    // check substitute status
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        int damage = currentTarget.damageDealt;
                        int subDamage = currentTarget.subDamage;
                        if (!battle.IsPokemonFainted(currentTarget.pokemon))
                        {
                            // Disguise / Ice Face
                            if (currentTarget.disguise != null)
                            {
                                EffectDatabase.AbilityEff.Disguise disguise =
                                    currentTarget.disguise.data.GetEffectNew(AbilityEffectType.Disguise)
                                    as EffectDatabase.AbilityEff.Disguise;

                                EffectDatabase.General.FormTransformation disguiseForm =
                                    disguise.GetDisguiseForm(currentTarget.pokemon);

                                if (disguiseForm != null)
                                {
                                    PBPShowAbility(currentTarget.pokemon, currentTarget.disguise.data);
                                    yield return StartCoroutine(PBPChangeForm(
                                        pokemon: currentTarget.pokemon,
                                        toForm: disguiseForm.toForm,
                                        changeText: disguise.displayText,
                                        checkAbility: false
                                        ));

                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = disguise.displayText,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID
                                    });

                                    // Lose health
                                    if (disguise.hpLossPercent > 0)
                                    {
                                        yield return StartCoroutine(PBPDamagePokemon(
                                            pokemon: currentTarget.pokemon,
                                            HPToLose: battle.GetPokemonHPByPercent(currentTarget.pokemon, disguise.hpLossPercent)
                                            ));
                                    }
                                }
                            }

                            // Substitute
                            if (!string.IsNullOrEmpty(currentTarget.pokemon.bProps.substituteMove)
                                && battle.IsPokemonOnFieldAndAble(currentTarget.pokemon))
                            {
                                MoveData subMoveData =
                                    MoveDatabase.instance.GetMoveData(currentTarget.pokemon.bProps.substituteMove);
                                MoveEffect effect = subMoveData.GetEffect(MoveEffectType.Substitute);

                                // substitute took damage and still alive
                                if (subDamage >= 0 && damage == 0 && currentTarget.pokemon.bProps.substituteHP > 0)
                                {
                                    string textID = effect.GetString(5);
                                    textID = (textID == "DEFAULT") ? "move-substitute-damage-default" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID,
                                        moveID = moveHitData.ID
                                    });
                                }
                                // break substitute
                                else if (currentTarget.pokemon.bProps.substituteHP == 0)
                                {
                                    currentTarget.pokemon.bProps.substituteMove = null;
                                    string textID = effect.GetString(1);
                                    textID = (textID == "DEFAULT") ? "move-substitute-destroy-default" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID,
                                        moveID = moveHitData.ID
                                    });
                                }
                            }
                        }
                    }

                    // check fainted pokemon
                    List<Pokemon> faintedPokemon = new List<Pokemon>();
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (battle.IsPokemonFainted(currentTarget.pokemon))
                        {
                            // Aftermath check
                            if (currentTarget.fainted)
                            {
                                EffectDatabase.AbilityEff.AbilityEffect aftermath_ =
                                    battle.PBPGetAbilityEffect(currentTarget.pokemon, AbilityEffectType.Aftermath);
                                if (aftermath_ != null 
                                    && battle.IsPokemonOnFieldAndAble(userPokemon)
                                    && !userPokemon.IsTheSameAs(currentTarget.pokemon))
                                {
                                    AbilityData afterMathData 
                                        = battle.PBPGetAbilityDataWithEffect(currentTarget.pokemon, AbilityEffectType.Aftermath);
                                    EffectDatabase.AbilityEff.Aftermath aftermath =
                                        aftermath_ as EffectDatabase.AbilityEff.Aftermath;

                                    if (battle.DoEffectFiltersPass(
                                        filters: aftermath.filters,
                                        userPokemon: currentTarget.pokemon,
                                        targetPokemon: userPokemon
                                        ))
                                    {
                                        bool applyAftermath = true;

                                        // Contact requirement
                                        if (applyAftermath 
                                            && aftermath.onlyContact
                                            && !battle.DoesMoveMakeContact(
                                                userPokemon: userPokemon,
                                                targetPokemon: currentTarget.pokemon,
                                                moveData: moveHitData))
                                        {
                                            applyAftermath = false;
                                        }

                                        // Damp requirement
                                        if (applyAftermath && aftermath.blockedByDamp)
                                        {
                                            bool blocked = false;

                                            List<Pokemon> dampUsers = battle.GetPokemonUnfaintedFrom(battle.pokemonOnField);
                                            for (int j = 0; j < dampUsers.Count && !blocked; j++)
                                            {
                                                List<Pokemon.Ability> abilities = battle.PBPGetAbilities(dampUsers[j]);
                                                for (int l = 0; l < abilities.Count && !blocked; l++)
                                                {
                                                    AbilityData dampData = abilities[l].data;
                                                    EffectDatabase.AbilityEff.AbilityEffect damp_ =
                                                        dampData.GetEffectNew(AbilityEffectType.Damp);

                                                    if (damp_ != null)
                                                    {
                                                        EffectDatabase.AbilityEff.Damp damp =
                                                            damp_ as EffectDatabase.AbilityEff.Damp;

                                                        if (damp.moveTags.Contains(MoveTag.ExplosiveMove))
                                                        {
                                                            blocked = true;
                                                        }
                                                    }
                                                }
                                            }

                                            if (blocked)
                                            {
                                                applyAftermath = false;
                                            }
                                        }

                                        if (applyAftermath)
                                        {
                                            PBPShowAbility(currentTarget.pokemon, afterMathData);

                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = aftermath.damage.displayText,
                                                pokemonUserID = currentTarget.pokemon.uniqueID,
                                                pokemonTargetID = userPokemon.uniqueID
                                            });
                                            int damage = battle.GetDamage(
                                                damage: aftermath.damage,
                                                targetPokemon: userPokemon,
                                                attackerPokemon: currentTarget.pokemon
                                                );
                                            yield return StartCoroutine(PBPDamagePokemon(
                                                pokemon: userPokemon,
                                                HPToLose: damage
                                                ));
                                        }
                                    }
                                }
                            }

                            faintedPokemon.Add(currentTarget.pokemon);
                        }
                    }
                    yield return StartCoroutine(BattleFaintCheck(
                        pokemonToCheck: faintedPokemon,
                        attackerPokemon: userPokemon));

                    // check Destiny Bond
                    if (!command.isFutureSightMove)
                    {
                        bool foundDestinyBondUser = false;
                        for (int k = 0; k < battleHitTargets.Count && !foundDestinyBondUser; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            if (currentTarget.fainted
                                && battle.IsPokemonOnFieldAndAble(userPokemon)
                                && !string.IsNullOrEmpty(currentTarget.destinyBondMove))
                            {
                                foundDestinyBondUser = true;

                                MoveData destinyBond = MoveDatabase.instance.GetMoveData(currentTarget.destinyBondMove);
                                MoveEffect effect = destinyBond.GetEffect(MoveEffectType.DestinyBond);
                                string textID = effect.GetString(1);
                                textID = (textID == "DEFAULT") ? "move-destinybond-success-default" : textID;

                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = textID,
                                    pokemonUserID = currentTarget.pokemon.uniqueID,
                                    pokemonTargetID = userPokemon.uniqueID
                                });

                                int preHP = userPokemon.currentHP;
                                int damage = userPokemon.maxHP;
                                int damageDealt = battle.SubtractPokemonHP(userPokemon, damage);
                                int postHP = userPokemon.currentHP;
                                yield return StartCoroutine(PBPChangePokemonHP(
                                    pokemon: userPokemon,
                                    preHP: preHP,
                                    hpChange: damageDealt,
                                    postHP: postHP,
                                    checkFaint: true
                                    ));
                            }
                        }
                    }

                    // run reflection hits
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.isMoveReflected && battle.IsPokemonOnFieldAndAble(userPokemon))
                        {
                            yield return StartCoroutine(ExecuteReflectedMove(
                                userPokemon,
                                new List<Pokemon> { currentTarget.pokemon },
                                moveHitData,
                                command,
                                forceOneHit: true
                                ));
                        }
                    }

                    // add instruct commands
                    if (moveHitData.GetEffect(MoveEffectType.Instruct) != null)
                    {
                        for (int k = 0; k < battleHitTargets.Count; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            if (currentTarget.affectedByMove)
                            {
                                if (battle.IsPokemonOnField(currentTarget.pokemon)
                                    && !battle.IsPokemonFainted(currentTarget.pokemon))
                                {
                                    string lastMove = currentTarget.pokemon.bProps.lastMove;
                                    if (lastMove != null)
                                    {
                                        MoveData instructData = MoveDatabase.instance.GetMoveData(lastMove);
                                        if (battle.DoesPokemonHaveMove(currentTarget.pokemon, lastMove)
                                            && battle.GetPokemonMovePP(currentTarget.pokemon, lastMove) > 0
                                            && !instructData.HasTag(MoveTag.CannotInstruct)
                                            && instructData.GetEffect(MoveEffectType.RechargeTurn) == null
                                            && instructData.GetEffect(MoveEffectType.MultiTurnAttack) == null
                                            && instructData.GetEffect(MoveEffectType.SkyDrop) == null
                                            && string.IsNullOrEmpty(currentTarget.pokemon.bProps.beakBlastMove)
                                            && string.IsNullOrEmpty(currentTarget.pokemon.bProps.focusPunchMove)
                                            && string.IsNullOrEmpty(currentTarget.pokemon.bProps.shellTrapMove)
                                            && currentTarget.pokemon.bProps.nextCommand == null)
                                        {
                                            // Set up instruct command
                                            BattleCommand newCommand = BattleCommand.CreateMoveCommand(
                                                currentTarget.pokemon,
                                                lastMove,
                                                battle.GetMoveAutoTargets(
                                                    currentTarget.pokemon, 
                                                    MoveDatabase.instance.GetMoveData(lastMove)));
                                            newCommand.consumePP = true;
                                            instructCommands.Add(newCommand);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // add after you commands
                    if (moveHitData.GetEffect(MoveEffectType.AfterYou) != null)
                    {
                        for (int k = 0; k < battleHitTargets.Count; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            if (currentTarget.affectedByMove)
                            {
                                if (battle.IsPokemonOnField(currentTarget.pokemon)
                                    && !battle.IsPokemonFainted(currentTarget.pokemon)
                                    && !currentTarget.pokemon.bProps.actedThisTurn)
                                {
                                    for (int j = 0; j < allCommands.Count; j++)
                                    {
                                        if (!afterYouCommands.Contains(allCommands[j]))
                                        {
                                            if (currentTarget.pokemon.IsTheSameAs(allCommands[j].commandUser))
                                            {
                                                afterYouCommands.Add(allCommands[j]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // quash commands
                    if (moveHitData.GetEffect(MoveEffectType.Quash) != null)
                    {
                        List<BattleCommand> accountedCommands = new List<BattleCommand>();
                        for (int k = 0; k < battleHitTargets.Count; k++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[k];
                            if (currentTarget.affectedByMove)
                            {
                                if (battle.IsPokemonOnField(currentTarget.pokemon)
                                    && !battle.IsPokemonFainted(currentTarget.pokemon)
                                    && !currentTarget.pokemon.bProps.actedThisTurn)
                                {
                                    for (int j = 0; j < allCommands.Count; j++)
                                    {
                                        BattleCommand curCommand = allCommands[j];
                                        if (!accountedCommands.Contains(curCommand))
                                        {
                                            if (currentTarget.pokemon.IsTheSameAs(curCommand.commandUser))
                                            {
                                                MoveEffect effect = moveHitData.GetEffect(MoveEffectType.Quash);
                                                string textID = effect.GetString(0);
                                                textID = (textID == "DEFAULT") ? "move-quash-default" : textID;
                                                SendEvent(new Battle.View.Events.MessageParameterized
                                                {
                                                    messageCode = textID,
                                                    pokemonTargetID = currentTarget.pokemon.uniqueID
                                                });

                                                allCommands.Remove(curCommand);
                                                allCommands.Add(curCommand);
                                                accountedCommands.Add(curCommand);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // add shell trap commands
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove)
                        {
                            if (battle.IsPokemonOnField(currentTarget.pokemon)
                                && !battle.IsPokemonFainted(currentTarget.pokemon)
                                && !string.IsNullOrEmpty(currentTarget.pokemon.bProps.shellTrapMove))
                            {
                                MoveData shellTrapData =
                                    MoveDatabase.instance.GetMoveData(currentTarget.pokemon.bProps.shellTrapMove);
                                MoveEffect shellTrap = shellTrapData.GetEffect(MoveEffectType.ShellTrap);

                                for (int j = 0; j < allCommands.Count; j++)
                                {
                                    if (!shellTrapCommands.Contains(allCommands[j]))
                                    {
                                        if (currentTarget.pokemon.IsTheSameAs(allCommands[j].commandUser)
                                        && allCommands[j].commandType == BattleCommandType.Fight
                                        && !allCommands[j].completed
                                        && !allCommands[j].inProgress)
                                        {
                                            if (allCommands[j].moveID == shellTrapData.ID)
                                            {
                                                bool canShellTrap = true;

                                                if (canShellTrap
                                                    && currentTarget.damageDealt < 0)
                                                {
                                                    canShellTrap = false;
                                                }

                                                if (canShellTrap
                                                    && shellTrap.GetBool(0)
                                                    && currentTarget.pokemon.bProps.turnPhysicalDamageTaken < 0)
                                                {
                                                    canShellTrap = false;
                                                }

                                                if (canShellTrap
                                                    && shellTrap.GetBool(1)
                                                    && !currentTarget.pokemon.bProps.wasHitByOpponent)
                                                {
                                                    canShellTrap = false;
                                                }

                                                if (canShellTrap)
                                                {
                                                    shellTrapCommands.Add(allCommands[j]);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // effects successful?
                    if (effectHitSuccessful)
                    {
                        effectsRanSuccessfully = true;
                    }

                    // no target was affected, and it's our first hit
                    totalDamageDealt += hitDamageDealt;
                    if (!moveUsedSuccessfully)
                    {
                        moveExplicitlyFailed = true;
                    }

                    // set alive pokemon from the field
                    List<Pokemon> endOfHitPokemon = new List<Pokemon>();
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (!endOfHitPokemon.Contains(currentTarget.pokemon))
                        {
                            if (currentTarget.affectedByMove)
                            {
                                endOfHitPokemon.Add(currentTarget.pokemon);
                            }
                        }
                    }

                    // set last hit targets
                    lastHitTargets.Clear();
                    lastHitTargets = new List<BattleHitTarget>(battleHitTargets);

                    alivePokemon = battle.GetPokemonOnFieldFrom(alivePokemon);
                    alivePokemon = battle.GetPokemonUnfaintedFrom(endOfHitPokemon);

                    if ((battle.IsPokemonFainted(userPokemon) && !command.isFutureSightMove)
                       || alivePokemon.Count == 0
                        || !successfulHit)
                    {
                        break;
                    }
                } // End of hit loop
            }

            if (battle.IsPokemonOnField(userPokemon)
                && !battle.IsPokemonFainted(userPokemon)
                && !command.isFutureSightMove)
            {
                userPokemon.bProps.actedThisTurn = true;
            }

            // set move automatically successful if they targeted the field and weren't blocked or interrupted
            if (moveSuccess)
            {
                if (battle.IsMoveFieldTargeting(userPokemon, masterMoveData))
                {
                    moveUsedSuccessfully = true;
                }
            }

            // some moves fail if their effects fail
            if (!effectsRanSuccessfully)
            {
                if (masterMoveData.GetEffect(MoveEffectType.FailIfEffectsFail) != null)
                {
                    moveUsedSuccessfully = false;
                }
            }

            // move was used successfully
            if (moveUsedSuccessfully)
            {
                // apply success effects, including team effects
                yield return StartCoroutine(ExecuteMoveEffects(
                    userPokemon: userPokemon,
                    battleHitTargets: lastHitTargets,
                    targetTeams: allHitTeams,
                    moveData: masterMoveData,
                    timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                    callback: (success) =>
                    {

                    }
                    ));
                yield return StartCoroutine(ExecuteMoveEffectsByTiming(
                    userPokemon: userPokemon,
                    battleHitTargets: lastHitTargets,
                    targetTeams: allHitTeams,
                    moveData: masterMoveData,
                    timing: MoveEffectTiming.AfterSuccessfulMoveUse,
                    callback: (success) =>
                    {

                    }
                    ));

                // Emergency Exit
                for (int k = 0; k < lastHitTargets.Count; k++)
                {
                    BattleHitTarget currentTarget = lastHitTargets[k];
                    if (currentTarget.affectedByMove
                        && battle.IsPokemonOnFieldAndAble(currentTarget.pokemon))
                    {
                        Trainer trainer = battle.GetPokemonOwner(currentTarget.pokemon);
                        Pokemon availablePokemon = battle.GetTrainerFirstAvailablePokemon(trainer);
                        if (availablePokemon != null)
                        {
                            List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(userPokemon);
                            bool wimpedOut = false;
                            for (int i = 0; i < userAbilities.Count && !wimpedOut; i++)
                            {
                                EffectDatabase.AbilityEff.AbilityEffect wimpOut_ =
                                    userAbilities[i].data.GetEffectNew(AbilityEffectType.WimpOut);
                                if (wimpOut_ != null)
                                {
                                    EffectDatabase.AbilityEff.WimpOut wimpOut = wimpOut_ as EffectDatabase.AbilityEff.WimpOut;
                                    if (currentTarget.preHPPercent > wimpOut.hpThreshold
                                        && currentTarget.postHPPercent <= wimpOut.hpThreshold)
                                    {
                                        wimpedOut = true;
                                        PBPShowAbility(userPokemon, userAbilities[i].data);
                                    }
                                }
                            }

                            if (wimpedOut)
                            {
                                yield return StartCoroutine(TryToBatonPassOut(
                                    withdrawPokemon: currentTarget.pokemon,
                                    trainer: trainer,
                                    isBatonPassing: false,
                                    bypassPursuitCheck: true,
                                    callback: (result) => { }
                                    ));
                            }
                        }
                    }
                }

                // check fainted pokemon
                List<Pokemon> faintedPokemon = new List<Pokemon>();
                for (int k = 0; k < lastHitTargets.Count; k++)
                {
                    BattleHitTarget currentTarget = lastHitTargets[k];
                    if (battle.IsPokemonFainted(currentTarget.pokemon)
                        && battle.IsPokemonOnField(currentTarget.pokemon))
                    {
                        faintedPokemon.Add(currentTarget.pokemon);
                        battle.FaintPokemon(currentTarget.pokemon);
                    }
                }
                if (faintedPokemon.Count > 0)
                {
                    for (int i = 0; i < faintedPokemon.Count; i++)
                    {
                        SendEvent(new Battle.View.Events.PokemonHealthFaint
                        {
                            pokemonUniqueID = faintedPokemon[i].uniqueID
                        });
                    }
                }

                // check for user
                if (battle.IsPokemonOnField(userPokemon) 
                    && !command.isFutureSightMove)
                {
                    // Sky Drop
                    if (!battle.IsPokemonFainted(userPokemon)
                        && masterMoveData.GetEffect(MoveEffectType.SkyDrop) != null
                        && userPokemon.bProps.attemptingToSkyDrop)
                    {
                        // put in semi-invulnerable states if possible
                        bool digState = (masterMoveData.GetEffect(MoveEffectType.MultiTurnDig) != null);
                        bool diveState = (masterMoveData.GetEffect(MoveEffectType.MultiTurnDive) != null);
                        bool flyState = (masterMoveData.GetEffect(MoveEffectType.MultiTurnFly) != null);
                        bool shadowForceState = (masterMoveData.GetEffect(MoveEffectType.MultiTurnShadowForce) != null);

                        MoveEffect effect = masterMoveData.GetEffect(MoveEffectType.SkyDrop);
                        string textID = effect.GetString(0);
                        textID = (textID == "DEFAULT") ? "move-skydrop-grab-default" : textID;

                        // Lock Sky Drop Targets if in grab step
                        if (userPokemon.bProps.attemptingToSkyDrop)
                        {
                            for (int i = 0; i < lastHitTargets.Count; i++)
                            {
                                // Only lock affected, unfainted targets that aren't already committed to a Sky Drop
                                BattleHitTarget currentTarget = lastHitTargets[i];
                                if (currentTarget.affectedByMove)
                                {
                                    if (battle.IsPokemonOnField(currentTarget.pokemon)
                                        && !battle.IsPokemonFainted(currentTarget.pokemon)
                                        && currentTarget.pokemon.bProps.skyDropUser == null)
                                    {
                                        currentTarget.pokemon.bProps.skyDropUser = userPokemon.uniqueID;
                                        currentTarget.pokemon.bProps.skyDropMove = masterMoveData.ID;
                                        userPokemon.bProps.skyDropTargets.Add(currentTarget.pokemon.uniqueID);

                                        currentTarget.pokemon.bProps.inDigState = digState;
                                        currentTarget.pokemon.bProps.inDiveState = diveState;
                                        currentTarget.pokemon.bProps.inFlyState = flyState;
                                        currentTarget.pokemon.bProps.inShadowForceState = shadowForceState;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = currentTarget.pokemon.uniqueID,
                                            moveID = masterMoveData.ID
                                        });
                                    }
                                }
                            }
                        }

                        if (userPokemon.bProps.skyDropTargets.Count > 0)
                        {
                            userPokemon.bProps.inDigState = digState;
                            userPokemon.bProps.inDiveState = diveState;
                            userPokemon.bProps.inFlyState = flyState;
                            userPokemon.bProps.inShadowForceState = shadowForceState;

                            command.iteration++;
                            command.consumePP = false;
                            command.bypassRedirection = true;
                            userPokemon.SetNextCommand(command);
                        }
                    }

                    // Ally Switch
                    if (battle.IsPokemonOnFieldAndAble(userPokemon)
                        && masterMoveData.GetEffect(MoveEffectType.AllySwitch) != null)
                    {
                        Pokemon allySwitchPokemon = null;

                        for (int i = 0; i < lastHitTargets.Count; i++)
                        {
                            if (lastHitTargets[i].affectedByMove)
                            {
                                Pokemon allyCandidate = lastHitTargets[i].pokemon;
                                if (battle.IsPokemonOnFieldAndAble(allyCandidate))
                                {
                                    allySwitchPokemon = allyCandidate;
                                    break;
                                }
                            }
                        }

                        if (allySwitchPokemon != null)
                        {
                            // Backend update
                            BattlePosition userPosition = battle.GetPokemonPosition(userPokemon);
                            BattlePosition allyPosition = battle.GetPokemonPosition(allySwitchPokemon);
                            battle.SetPokemonPosition(userPokemon, allyPosition);
                            battle.SetPokemonPosition(allySwitchPokemon, userPosition);

                            // Send visual update
                            SendEvent(new Battle.View.Events.PokemonSwitchPosition
                            {
                                pokemonUniqueID1 = userPokemon.uniqueID,
                                pokemonUniqueID2 = allySwitchPokemon.uniqueID
                            });

                            // Send text event
                            MoveEffect effect = masterMoveData.GetEffect(MoveEffectType.AllySwitch);
                            string textID = effect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-allyswitch" : textID;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                pokemonTargetID = allySwitchPokemon.uniqueID,
                                moveID = masterMoveData.ID
                            });
                        }
                    }

                    float preHPPercent = battle.GetPokemonHPAsPercentage(userPokemon);
                    bool recoilActivated = false;
                    // Recoil
                    if (battle.IsPokemonOnFieldAndAble(userPokemon)
                        && totalDamageDealt > 0
                        && !userPokemon.bProps.attemptingToSkyDrop)
                    {
                        EffectDatabase.MoveEff.MoveEffect doubleEdge_ = masterMoveData.GetEffectNew(MoveEffectType.Recoil);

                        // Rock Head Check
                        EffectDatabase.AbilityEff.AbilityEffect rockHead_ = 
                            battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.RockHead);

                        if (doubleEdge_ != null)
                        {
                            EffectDatabase.MoveEff.DoubleEdge doubleEdge = 
                                doubleEdge_ as EffectDatabase.MoveEff.DoubleEdge;

                            bool applyRecoil = true;
                            if (rockHead_ != null || doubleEdge.bypassRockHead)
                            {
                                applyRecoil = false;
                            }

                            // Execute Recoil
                            if (applyRecoil)
                            {
                                int recoilDamage =
                                    (doubleEdge.recoilMode == EffectDatabase.MoveEff.DoubleEdge.RecoilMode.Damage) ?
                                    Mathf.FloorToInt(totalDamageDealt * doubleEdge.hpLossPercent)
                                    : (doubleEdge.recoilMode == EffectDatabase.MoveEff.DoubleEdge.RecoilMode.MaxHP) ?
                                    battle.GetPokemonHPByPercent(userPokemon, doubleEdge.hpLossPercent)
                                    : 1;
                                recoilDamage = Mathf.Max(1, recoilDamage);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = doubleEdge.displayText,
                                    pokemonUserID = userPokemon.uniqueID
                                });

                                recoilActivated = true;
                                yield return StartCoroutine(PBPDamagePokemon(
                                    pokemon: userPokemon,
                                    HPToLose: recoilDamage,
                                    checkFaint: false
                                    ));
                            }
                        }
                    }

                    // Lose HP
                    // TODO: Life Orb

                    // Emergency Exit / Wimp Out
                    float postHPPercent = battle.GetPokemonHPAsPercentage(userPokemon);
                    if (recoilActivated
                        && battle.IsPokemonOnFieldAndAble(userPokemon))
                    {
                        Trainer trainer = battle.GetPokemonOwner(userPokemon);
                        Pokemon availablePokemon = battle.GetTrainerFirstAvailablePokemon(trainer);
                        if (availablePokemon != null)
                        {
                            List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(userPokemon);
                            bool wimpedOut = false;
                            for (int i = 0; i < userAbilities.Count && !wimpedOut; i++)
                            {
                                EffectDatabase.AbilityEff.AbilityEffect wimpOut_ =
                                    userAbilities[i].data.GetEffectNew(AbilityEffectType.WimpOut);
                                if (wimpOut_ != null)
                                {
                                    EffectDatabase.AbilityEff.WimpOut wimpOut = wimpOut_ as EffectDatabase.AbilityEff.WimpOut;
                                    if (preHPPercent > wimpOut.hpThreshold
                                        && postHPPercent <= wimpOut.hpThreshold)
                                    {
                                        wimpedOut = true;
                                        PBPShowAbility(userPokemon, userAbilities[i].data);
                                    }
                                }
                            }

                            if (wimpedOut)
                            {
                                yield return StartCoroutine(TryToBatonPassOut(
                                    withdrawPokemon: userPokemon,
                                    trainer: trainer,
                                    isBatonPassing: false,
                                    bypassPursuitCheck: true,
                                    callback: (result) => { }
                                    ));
                            }
                        }
                    }

                    // Recharge Turn
                    if (battle.IsPokemonOnFieldAndAble(userPokemon)
                        && !userPokemon.bProps.attemptingToSkyDrop)
                    {
                        MoveEffect effect = masterMoveData.GetEffect(MoveEffectType.RechargeTurn);
                        if (effect != null)
                        {
                            userPokemon.bProps.rechargeTurns += Mathf.FloorToInt(effect.GetFloat(0));
                        }
                    }

                    // Change Form
                    if (battle.IsPokemonOnFieldAndAble(userPokemon))
                    {
                        float userHPPercent = battle.GetPokemonHPAsPercentage(userPokemon);

                        // Relic Song
                        EffectDatabase.MoveEff.MoveEffect relicSong_ =
                            masterMoveData.GetEffectNew(MoveEffectType.RelicSong);
                        if (relicSong_ != null)
                        {
                            EffectDatabase.MoveEff.RelicSong relicSong =
                                (relicSong_ as EffectDatabase.MoveEff.RelicSong);

                            bool changedForm = false;
                            string prevForm = userPokemon.pokemonID;
                            string toForm = null;

                            if (userPokemon.pokemonID == relicSong.form1)
                            {
                                toForm = relicSong.form2;
                                changedForm = true;
                            }
                            else if (userPokemon.pokemonID == relicSong.form2)
                            {
                                toForm = relicSong.form1;
                                changedForm = true;
                            }

                            if (changedForm)
                            {
                                yield return StartCoroutine(PBPChangeForm(
                                    pokemon: userPokemon,
                                    toForm: toForm,
                                    changeText: relicSong.afterText
                                    ));
                            }
                        }

                        // Gulp Missile
                        List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(userPokemon);
                        for (int i = 0; i < userAbilities.Count; i++)
                        {
                            Pokemon.Ability ability = userAbilities[i];
                            List<EffectDatabase.AbilityEff.AbilityEffect> gulpMissiles_ =
                                ability.data.GetEffectsNew(AbilityEffectType.GulpMissile);
                            bool changedForm = false;

                            for (int k = 0; k < gulpMissiles_.Count && !changedForm; k++)
                            {
                                EffectDatabase.AbilityEff.GulpMissile gulpMissile = 
                                    gulpMissiles_[k] as EffectDatabase.AbilityEff.GulpMissile;
                                for (int j = 0; j < gulpMissile.gulpTransformations.Count && !changedForm; j++)
                                {
                                    EffectDatabase.AbilityEff.GulpMissile.GulpTransformation gulpTrns
                                        = gulpMissile.gulpTransformations[j];

                                    bool canTransform = gulpTrns.transformation.IsPokemonAPreForm(userPokemon)
                                        && !gulpTrns.transformation.IsPokemonAToForm(userPokemon);

                                    // Move requirement
                                    if (canTransform && gulpTrns.moves.Count > 0)
                                    {
                                        canTransform = false;
                                        if (gulpTrns.moves.Contains(masterMoveData.ID))
                                        {
                                            canTransform = true;
                                        }
                                    }

                                    // Check missiles
                                    if (canTransform && gulpTrns.missiles.Count > 0)
                                    {
                                        canTransform = false;
                                        for (int l = 0; l < gulpTrns.missiles.Count && !canTransform; l++)
                                        {
                                            if (gulpTrns.missiles[l].hpThreshold >= userHPPercent)
                                            {
                                                // Change Form
                                                canTransform = true;
                                                changedForm = true;
                                                SendEvent(new Battle.View.Events.MessageParameterized
                                                {
                                                    messageCode = gulpMissile.gulpText,
                                                    pokemonUserID = userPokemon.uniqueID
                                                });

                                                PBPShowAbility(pokemon: userPokemon, ability: ability);
                                                yield return StartCoroutine(PBPChangeForm(
                                                    pokemon: userPokemon,
                                                    toForm: gulpTrns.transformation.toForm
                                                    ));

                                                // Add Missile to User
                                                userPokemon.bProps.gulpMissile = gulpTrns.missiles[l].Clone();
                                            }
                                        }
                                    }
                                }

                                /*yield return StartCoroutine(PBPRunAbilityEffect(
                                    pokemon: userPokemon,
                                    ability: ability,
                                    effect_: gulpMissile,
                                    callback: (result) =>
                                    {
                                        if (result)
                                        {
                                            changedForm = true;
                                        }
                                    }
                                    ));*/
                            }


                        }
                    }

                    // Run called moves here at the end
                    if (battle.IsPokemonOnField(userPokemon)
                        && !battle.IsPokemonFainted(userPokemon)
                        && callCommand != null
                        && !userPokemon.bProps.attemptingToSkyDrop)
                    {
                        // Mirror Move?
                        if (masterMoveData.GetEffect(MoveEffectType.MirrorMove) != null)
                        {
                            battle.UnsetPokemonLastTargeted(userPokemon);
                        }

                        // Nature Power?
                        if (masterMoveData.GetEffect(MoveEffectType.NaturePower) != null)
                        {
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = "move-naturepower-default",
                                pokemonUserID = userPokemon.uniqueID,
                                moveIDs = new List<string> { masterMoveData.ID, callCommand.moveID }
                            });
                        }


                        callCommand.consumePP = false;
                        callCommand.isMoveCalled = true;
                        callCommand.bypassStatusInterrupt = true;
                        yield return StartCoroutine(ExecuteCommand(callCommand));
                    }

                    // Use Me First if it was available
                    if (!battle.IsPokemonFainted(userPokemon) && meFirstCommand != null
                        && !userPokemon.bProps.attemptingToSkyDrop)
                    {
                        userPokemon.bProps.usingMeFirst = true;
                        yield return StartCoroutine(ExecuteCommand(meFirstCommand));
                        userPokemon.bProps.usingMeFirst = false;
                    }

                    // Baton Pass / U-Turn / Teleport (if not being committed to Sky Drop)
                    MoveEffect BPEffect = masterMoveData.GetEffect(MoveEffectType.BatonPass);
                    if (battle.IsPokemonOnField(userPokemon)
                        && !battle.IsPokemonFainted(userPokemon)
                        && BPEffect != null
                        && userPokemon.bProps.skyDropUser == null
                        && !userPokemon.bProps.attemptingToSkyDrop)
                    {
                        Trainer trainer = battle.GetPokemonOwner(userPokemon);
                        Pokemon availablePokemon = battle.GetTrainerFirstAvailablePokemon(trainer);
                        if (availablePokemon != null)
                        {
                            yield return StartCoroutine(TryToBatonPassOut(
                                withdrawPokemon: userPokemon,
                                trainer: trainer,
                                uTurnData: masterMoveData,
                                isBatonPassing: BPEffect.GetBool(0),
                                callback: (result) =>
                                {

                                }
                                ));
                        }
                    }

                    // run faint event if it hasn't been done already for the user
                    if (battle.IsPokemonOnField(userPokemon)
                        && battle.IsPokemonFainted(userPokemon))
                    {
                        battle.FaintPokemon(userPokemon);
                        SendEvent(new Battle.View.Events.PokemonHealthFaint
                        {
                            pokemonUniqueID = userPokemon.uniqueID
                        });
                        yield return StartCoroutine(UntiePokemon(userPokemon));
                    }
                }
            }

            // Move totally failed (failed, missed or blocked by all targets)
            if (moveExplicitlyFailed)
            {
                if (battle.IsPokemonOnField(userPokemon)
                    && !command.isFutureSightMove)
                {
                    // Destiny Bond
                    if (!battle.IsPokemonFainted(userPokemon)
                        && masterMoveData.GetEffect(MoveEffectType.DestinyBond) != null)
                    {
                        userPokemon.bProps.destinyBondMove = null;
                    }

                    // Jump Kick, lose HP on failure
                    if (!battle.IsPokemonFainted(userPokemon))
                    {
                        MoveEffect jumpKickEffect = masterMoveData.GetEffect(MoveEffectType.JumpKick);
                        if (jumpKickEffect != null)
                        {
                            int preHP = userPokemon.currentHP;
                            int damage = battle.GetPokemonHPByPercent(userPokemon, jumpKickEffect.GetFloat(0));
                            damage = Mathf.Max(1, damage);
                            int damageDealt = battle.SubtractPokemonHP(userPokemon, damage);
                            int postHP = userPokemon.currentHP;
                            string textCodeID = jumpKickEffect.GetString(0);
                            textCodeID = (textCodeID == "DEFAULT") ? "move-jumpkick-fail-default" : textCodeID;

                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textCodeID,
                                pokemonUserID = userPokemon.uniqueID,
                                moveID = masterMoveData.ID
                            });
                            yield return StartCoroutine(PBPChangePokemonHP(
                                pokemon: userPokemon,
                                preHP: preHP,
                                hpChange: damageDealt,
                                postHP: postHP
                                ));

                        }
                    }

                    // unset thrash move
                    battle.UnsetPokemonThrashMove(userPokemon);

                    // unset uproar move
                    battle.UnsetPokemonUproarMove(userPokemon);

                    // TODO: Stomping Tantrum Boost
                    if (!battle.IsPokemonFainted(userPokemon))
                    {

                    }

                    // run faint event if it hasn't been done already for the user
                    if (battle.IsPokemonFainted(userPokemon))
                    {
                        battle.FaintPokemon(userPokemon);
                        SendEvent(new Battle.View.Events.PokemonHealthFaint
                        {
                            pokemonUniqueID = userPokemon.uniqueID
                        });
                        yield return StartCoroutine(UntiePokemon(userPokemon));
                    }
                }
            }

            // free sky drop targets on failure
            if (!moveSuccess || moveExplicitlyFailed)
            {
                // Sky Drop
                if (battle.IsPokemonOnField(userPokemon)
                    && !battle.IsPokemonFainted(userPokemon)
                    && masterMoveData.GetEffect(MoveEffectType.SkyDrop) != null)
                {
                    userPokemon.bProps.attemptingToSkyDrop = false;
                    for (int i = 0; i < userPokemon.bProps.skyDropTargets.Count; i++)
                    {
                        Pokemon target = battle.GetFieldPokemonByID(userPokemon.bProps.skyDropTargets[i]);
                        if (target != null)
                        {
                            if (battle.IsPokemonOnField(target)
                                && !battle.IsPokemonFainted(target))
                            {
                                yield return StartCoroutine(FreePokemonFromSkyDrop(target));
                            }
                        }
                    }
                    userPokemon.bProps.skyDropTargets.Clear();
                }
            }

            // end of move regardless
            bool thrashEnded = false;
            bool uproarEnded = false;
            if (battle.IsPokemonOnField(userPokemon) && !command.isFutureSightMove)
            {
                // Thrash check
                if (!battle.IsPokemonFainted(userPokemon))
                {
                    MoveEffect thrashEffect = masterMoveData.GetEffect(MoveEffectType.Thrash);
                    if (thrashEffect != null)
                    {
                        // check if thrash finished
                        if (!string.IsNullOrEmpty(userPokemon.bProps.thrashMove))
                        {
                            // end thrash
                            if (userPokemon.bProps.thrashTurns == 0)
                            {
                                thrashEnded = true;

                                battle.UnsetPokemonThrashMove(userPokemon);
                                string statusID = thrashEffect.GetString(0);
                                string displayText = thrashEffect.GetString(1);
                                int statusTurns = Mathf.FloorToInt(thrashEffect.GetFloat(1));
                                MoveEffect statusTurnEffect =
                                    masterMoveData.GetEffect(MoveEffectType.ThrashStatusTurnRange);
                                if (statusTurnEffect != null)
                                {
                                    int minTurns = Mathf.FloorToInt(statusTurnEffect.GetFloat(0));
                                    int maxTurns = Mathf.FloorToInt(statusTurnEffect.GetFloat(1));
                                    statusTurns = Random.Range(minTurns, maxTurns);
                                }

                                // try to inflict status condition
                                yield return StartCoroutine(TryToInflictPokemonSC(
                                    statusID: statusID,
                                    targetPokemon: userPokemon,
                                    turnsLeft: statusTurns,
                                    userPokemon: userPokemon,
                                    inflictOverwrite: displayText,
                                    callback: (result) =>
                                    {

                                    }
                                    ));
                            }
                        }
                    }
                }

                // Uproar check
                if (!battle.IsPokemonFainted(userPokemon))
                {
                    MoveEffect uproarEffect = masterMoveData.GetEffect(MoveEffectType.Uproar);
                    if (uproarEffect != null)
                    {
                        // check if thrash finished
                        if (!string.IsNullOrEmpty(userPokemon.bProps.uproarMove))
                        {
                            // end thrash
                            if (userPokemon.bProps.uproarTurns == 0)
                            {
                                uproarEnded = true;

                                string textID = uproarEffect.GetString(1);
                                textID = (textID == "DEFAULT") ? "move-uproar-end-default" : textID;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = textID,
                                    pokemonUserID = userPokemon.uniqueID,
                                    moveID = masterMoveData.ID
                                });
                                battle.UnsetPokemonUproarMove(userPokemon);
                            }
                        }
                    }
                }
            }

            if (!command.isFutureSightMove)
            {
                if (battle.IsPokemonOnFieldAndAble(userPokemon))
                {
                    // Rollout
                    EffectDatabase.MoveEff.MoveEffect rollout_ = masterMoveData.GetEffectNew(MoveEffectType.Rollout);
                    if (rollout_ != null && !isCharging)
                    {
                        EffectDatabase.MoveEff.Rollout rollout = rollout_ as EffectDatabase.MoveEff.Rollout;
                        bool endRollout = false;

                        if (rollout.endOnFail && !moveUsedSuccessfully)
                        {
                            endRollout = true;
                        }
                        if (rollout.endOnMaxHits && command.iteration >= rollout.maxExecutions
                            && rollout.maxExecutions != -1)
                        {
                            endRollout = true;
                        }

                        if (!endRollout)
                        {
                            // Continue
                            if (command.iteration >= rollout.maxExecutions
                                && rollout.maxExecutions != -1)
                            {
                                command.iteration = 1;
                            }
                            // Restart
                            else if (rollout.maxExecutions != -1)
                            {
                                command.iteration++;
                            }
                            command.consumePP = false;
                            userPokemon.SetNextCommand(command);
                        }
                    }
                }
            }

            // last success checks
            if (moveSuccess && !command.isFutureSightMove)
            {
                if (battle.IsPokemonOnField(userPokemon))
                {
                    // Bide check
                    if (!battle.IsPokemonFainted(userPokemon))
                    {
                        if (userPokemon.bProps.bideTurnsLeft == 0)
                        {
                            userPokemon.DisruptBide();
                        }
                    }

                    // Thrash check
                    if (!battle.IsPokemonFainted(userPokemon) && !thrashEnded)
                    {
                        MoveEffect thrashEffect = masterMoveData.GetEffect(MoveEffectType.Thrash);
                        if (thrashEffect != null)
                        {
                            // check if thrashing currently
                            if (!string.IsNullOrEmpty(userPokemon.bProps.thrashMove))
                            {
                                if (userPokemon.bProps.thrashTurns > 0)
                                {
                                    userPokemon.bProps.thrashTurns--;
                                }
                                // keep thrashing
                                userPokemon.SetNextCommand(command);
                            }
                            // induce thrash
                            else
                            {
                                int thrashTurns = Mathf.FloorToInt(thrashEffect.GetFloat(0));
                                MoveEffect thrashTurnEffect = masterMoveData.GetEffect(MoveEffectType.ThrashTurnRange);
                                if (thrashTurnEffect != null)
                                {
                                    int minTurns = Mathf.FloorToInt(thrashTurnEffect.GetFloat(0));
                                    int maxTurns = Mathf.FloorToInt(thrashTurnEffect.GetFloat(1));
                                    thrashTurns = Random.Range(minTurns, maxTurns);
                                }
                                battle.SetPokemonThrashMove(
                                    userPokemon,
                                    command,
                                    thrashTurns - 1
                                    );
                            }
                        }
                    }

                    // Uproar check
                    if (!battle.IsPokemonFainted(userPokemon) && !uproarEnded)
                    {
                        MoveEffect uproarEffect = masterMoveData.GetEffect(MoveEffectType.Uproar);
                        if (uproarEffect != null)
                        {
                            // check if uproaring currently
                            if (!string.IsNullOrEmpty(userPokemon.bProps.uproarMove))
                            {
                                if (userPokemon.bProps.uproarTurns > 0)
                                {
                                    userPokemon.bProps.uproarTurns--;
                                }
                                // keep uproaring
                                userPokemon.SetNextCommand(command);
                            }
                            // induce uproar
                            else
                            {
                                string textID = uproarEffect.GetString(0);
                                textID = (textID == "DEFAULT") ? "move-uproar-start-default" : textID;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = textID,
                                    pokemonUserID = userPokemon.uniqueID,
                                    moveID = masterMoveData.ID
                                });

                                // Wake other pokemon up
                                List<Pokemon> ablePokemon = battle.GetPokemonUnfaintedFrom(battle.pokemonOnField);
                                for (int i = 0; i < ablePokemon.Count; i++)
                                {
                                    Pokemon pokemon = ablePokemon[i];
                                    StatusCondition sleepCondition = 
                                        battle.GetPokemonFilteredStatus(pokemon, PokemonSEType.Sleep);
                                    if (sleepCondition != null)
                                    {
                                        yield return StartCoroutine(HealPokemonSC(
                                            targetPokemon: pokemon,
                                            condition: sleepCondition
                                            ));
                                    }
                                }

                                int uproarTurns = Mathf.FloorToInt(uproarEffect.GetFloat(0));
                                MoveEffect thrashTurnEffect = masterMoveData.GetEffect(MoveEffectType.ThrashTurnRange);
                                if (thrashTurnEffect != null)
                                {
                                    int minTurns = Mathf.FloorToInt(thrashTurnEffect.GetFloat(0));
                                    int maxTurns = Mathf.FloorToInt(thrashTurnEffect.GetFloat(1));
                                    uproarTurns = Random.Range(minTurns, maxTurns);
                                }
                                battle.SetPokemonUproarMove(
                                    userPokemon,
                                    command,
                                    uproarTurns - 1
                                    );
                            }
                        }
                    }

                }
            }

            // run instruct commands
            for (int k = 0; k < instructCommands.Count; k++)
            {
                if (!instructCommands[k].completed
                    && !instructCommands[k].inProgress)
                {
                    yield return StartCoroutine(ExecuteCommand(instructCommands[k]));
                }
            }

            // run dancer commands
            if (moveUsedSuccessfully && !command.isDanceMove)
            {
                List<Pokemon> dancerPokemon = battle.GetPokemonUnfainted();
                dancerPokemon.Remove(userPokemon);

                for (int i = 0; i < dancerPokemon.Count; i++)
                {
                    if (battle.IsPokemonOnFieldAndAble(dancerPokemon[i]))
                    {
                        List<Pokemon.Ability> abilities = battle.PBPGetAbilities(dancerPokemon[i]);
                        for (int k = 0; k < abilities.Count; k++)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect dancer_ = 
                                abilities[k].data.GetEffectNew(AbilityEffectType.Dancer);
                            if (dancer_ != null
                                && battle.IsPokemonOnFieldAndAble(dancerPokemon[i]))
                            {
                                EffectDatabase.AbilityEff.Dancer dancer = dancer_ as EffectDatabase.AbilityEff.Dancer;
                                if (battle.DoEffectFiltersPass(
                                    filters: dancer.filters,
                                    userPokemon: dancerPokemon[i],
                                    moveData: masterMoveData
                                    ))
                                {
                                    bool dance = true;

                                    if (dancer.moveTags.Count > 0)
                                    {
                                        dance = false;
                                        List<MoveTag> moveTags = new List<MoveTag>(masterMoveData.moveTags);
                                        for (int j = 0; j < moveTags.Count && !dance; j++)
                                        {
                                            if (dancer.moveTags.Contains(moveTags[j]))
                                            {
                                                dance = true;
                                            }
                                        }
                                    }

                                    if (dance)
                                    {
                                        PBPShowAbility(dancerPokemon[i], abilities[k].data);

                                        string danceMove = masterMoveData.ID;

                                        // Set up dance command
                                        BattleCommand newCommand = BattleCommand.CreateMoveCommand(
                                            commandUser: dancerPokemon[i],
                                            moveID: danceMove,
                                            targetPositions: battle.GetMoveAutoTargets(
                                                dancerPokemon[i],
                                                MoveDatabase.instance.GetMoveData(danceMove)));
                                        newCommand.consumePP = true;
                                        newCommand.isDanceMove = true;
                                        yield return StartCoroutine(ExecuteCommand(newCommand));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // run after you commands
            for (int k = 0; k < afterYouCommands.Count; k++)
            {
                if (!afterYouCommands[k].completed
                    && !afterYouCommands[k].inProgress)
                {
                    Pokemon afterYouPokemon = afterYouCommands[k].commandUser;
                    if (battle.IsPokemonOnField(afterYouPokemon)
                        && !battle.IsPokemonFainted(afterYouPokemon))
                    {
                        MoveEffect effect = masterMoveData.GetEffect(MoveEffectType.AfterYou);
                        if (effect != null)
                        {
                            string textID = effect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-afteryou-default" : textID;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                pokemonTargetID = afterYouPokemon.uniqueID
                            });
                        }
                        yield return StartCoroutine(ExecuteCommand(afterYouCommands[k]));
                    }
                }
            }

            // run shell trap commands
            for (int k = 0; k < shellTrapCommands.Count; k++)
            {
                if (!shellTrapCommands[k].completed
                    && !shellTrapCommands[k].inProgress)
                {
                    yield return StartCoroutine(ExecuteCommand(shellTrapCommands[k]));
                }
            }
        }
        public IEnumerator ExecuteMoveEffects(
            Pokemon userPokemon,
            List<BattleHitTarget> battleHitTargets,
            List<BattleTeam> targetTeams,
            MoveData moveData,
            System.Action<bool> callback,
            MoveEffectTiming timing = MoveEffectTiming.Unique,
            bool bypassChecks = false,
            bool apply = true
            )
        {
            List<MoveEffect> effects = moveData.GetEffectsFiltered(timing);
            bool effectSuccess = false;

            for (int i = 0; i < effects.Count; i++)
            {
                MoveEffect effect = effects[i];

                // target effects
                if (effect.effectTargetType == MoveEffectTargetType.Target)
                {
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        BattleTeam currentTeam = battle.GetTeam(currentTarget.pokemon);
                        if (currentTarget.affectedByMove || bypassChecks)
                        {
                            if (battle.CanApplyMoveEffect(userPokemon, currentTarget.pokemon, moveData, effect))
                            {
                                yield return StartCoroutine(ExecuteMoveEffect(
                                    effect: effect,
                                    moveData: moveData,
                                    userPokemon: userPokemon,
                                    targetPokemon: currentTarget.pokemon,
                                    targetTeam: currentTeam,
                                    apply: apply,
                                    callback: (success) =>
                                    {
                                        if (success)
                                        {
                                            effectSuccess = true;
                                        }
                                    }
                                    ));
                            }
                        }
                    }
                }

                // user-only effects
                if (effect.effectTargetType == MoveEffectTargetType.Self)
                {
                    yield return StartCoroutine(ExecuteMoveEffect(
                        effect: effect,
                        moveData: moveData,
                        userPokemon: userPokemon,
                        targetPokemon: null,
                        targetTeam: null,
                        apply: apply,
                        callback: (success) =>
                        {
                            if (success)
                            {
                                effectSuccess = true;
                            }
                        }
                        ));
                }

                // self team effects
                if (effect.effectTargetType == MoveEffectTargetType.SelfTeam)
                {
                    BattleTeam userTeam = battle.GetTeam(userPokemon);
                    yield return StartCoroutine(ExecuteMoveEffect(
                        effect: effect,
                        moveData: moveData,
                        userPokemon: userPokemon,
                        targetPokemon: null,
                        targetTeam: userTeam,
                        apply: apply,
                        callback: (success) =>
                        {
                            if (success)
                            {
                                effectSuccess = true;
                            }
                        }
                        ));
                }

                // team effects
                if (effect.effectTargetType == MoveEffectTargetType.Team)
                {
                    for (int k = 0; k < targetTeams.Count; k++)
                    {
                        yield return StartCoroutine(ExecuteMoveEffect(
                            effect: effect,
                            moveData: moveData,
                            userPokemon: userPokemon,
                            targetPokemon: null,
                            targetTeam: targetTeams[k],
                            apply: apply,
                            callback: (success) =>
                            {
                                if (success)
                                {
                                    effectSuccess = true;
                                }
                            }
                            ));
                    }
                }

                // battle effects
                if (effect.effectTargetType == MoveEffectTargetType.Battlefield)
                {
                    yield return StartCoroutine(ExecuteMoveEffect(
                        effect: effect,
                        moveData: moveData,
                        userPokemon: userPokemon,
                        targetPokemon: null,
                        targetTeam: null,
                        apply: apply,
                        callback: (success) =>
                        {
                            if (success)
                            {
                                effectSuccess = true;
                            }
                        }
                        ));
                }

            }
            callback(effectSuccess);
            yield return null;
        }
        public IEnumerator ExecuteMoveEffect(
            MoveEffect effect,
            MoveData moveData,
            Pokemon userPokemon,
            Pokemon targetPokemon,
            BattleTeam targetTeam,
            System.Action<bool> callback,
            bool apply = true)
        {
            bool effectSuccess = false;
            bool forceEffectDisplay = apply && effect.forceEffectDisplay;

            if (battle.DoesMoveEffectPassChance(
                effect: effect,
                moveData: moveData,
                userPokemon: userPokemon,
                targetPokemon: targetPokemon,
                targetTeam: targetTeam
                ))
            {
                Item userItem = battle.PBPGetHeldItem(userPokemon);

                // target effects
                if (targetPokemon != null)
                {
                    if (battle.IsPokemonOnField(targetPokemon))
                    {
                        if (!battle.IsPokemonFainted(targetPokemon))
                        {
                            Item targetItem = battle.PBPGetHeldItem(targetPokemon);

                            // Bestow
                            if (effect.effectType == MoveEffectType.Bestow)
                            {
                                if (userItem != null
                                    && targetItem == null)
                                {
                                    if (apply)
                                    {
                                        userPokemon.UnsetItem(userItem);
                                        targetPokemon.SetItem(userItem);

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-bestow-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            itemID = userItem.itemID
                                        });
                                    }
                                    effectSuccess = true;
                                }
                            }
                            // Bind
                            else if (effect.effectType == MoveEffectType.Bind)
                            {
                                if (battle.IsPokemonOnField(userPokemon)
                                    && !battle.IsPokemonFainted(userPokemon))
                                {
                                    bool canBeBinded = true;
                                    if (!string.IsNullOrEmpty(targetPokemon.bProps.bindMove))
                                    {
                                        canBeBinded = false;
                                    }

                                    if (canBeBinded && apply)
                                    {
                                        targetPokemon.bProps.bindMove = moveData.ID;
                                        targetPokemon.bProps.bindPokemon = userPokemon.uniqueID;

                                        // turn calculation
                                        int turnCount = Mathf.FloorToInt(effect.GetFloat(1));
                                        MoveEffect turnRange = moveData.GetEffect(MoveEffectType.BindTurnRange);
                                        if (turnRange != null)
                                        {
                                            int minTurns = Mathf.FloorToInt(turnRange.GetFloat(0));
                                            int maxTurns = Mathf.FloorToInt(turnRange.GetFloat(1));
                                            turnCount = Random.Range(minTurns, maxTurns);
                                        }

                                        targetPokemon.bProps.bindTurns = turnCount;

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-bind-start-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                    effectSuccess = canBeBinded;
                                }
                            }
                            // Block
                            else if (effect.effectType == MoveEffectType.Block)
                            {
                                if (battle.IsPokemonOnField(userPokemon)
                                    && !battle.IsPokemonFainted(userPokemon))
                                {
                                    bool canBeBlocked = true;
                                    if (!string.IsNullOrEmpty(targetPokemon.bProps.blockMove))
                                    {
                                        canBeBlocked = false;
                                    }

                                    // Ghost-types can't be trapped
                                    if (canBeBlocked)
                                    {
                                        if (battle.DoesPokemonHaveType(targetPokemon, "ghost"))
                                        {
                                            canBeBlocked = false;
                                        }
                                    }

                                    if (canBeBlocked && apply)
                                    {
                                        targetPokemon.bProps.blockMove = moveData.ID;
                                        targetPokemon.bProps.blockPokemon = userPokemon.uniqueID;

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-block-start-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                    else if (forceEffectDisplay)
                                    {
                                        string textID = effect.GetString(1);
                                        textID = (textID == "DEFAULT") ? "move-block-fail-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                    effectSuccess = canBeBlocked;
                                }
                            }
                            // Burn Up
                            else if (effect.effectType == MoveEffectType.BurnUp
                                && effect.effectTargetType == MoveEffectTargetType.Target)
                            {
                                // Fails on Multitype
                                if (battle.PBPLegacyGetAbilityEffect(targetPokemon, AbilityEffectType.Multitype) == null)
                                {
                                    List<string> removableTypes = new List<string>();
                                    for (int i = 1; i < effect.stringParams.Length; i++)
                                    {
                                        removableTypes.Add(effect.stringParams[i]);
                                    }
                                    bool allTypes = false;
                                    if (removableTypes.Contains("ALL"))
                                    {
                                        allTypes = true;
                                        removableTypes = TypeDatabase.instance.GetAllTypes();
                                    }

                                    List<string> typesRemoved = new List<string>();
                                    List<Pokemon.BattleProperties.ForestsCurse> forestsCursesRemoved
                                        = new List<Pokemon.BattleProperties.ForestsCurse>();
                                    for (int i = 0; i < removableTypes.Count; i++)
                                    {
                                        bool typeRemoved = false;
                                        if (targetPokemon.bProps.types.Contains(removableTypes[i]))
                                        {
                                            typeRemoved = true;
                                            typesRemoved.Add(removableTypes[i]);
                                        }
                                        for (int k = 0; k < targetPokemon.bProps.forestsCurses.Count; k++)
                                        {
                                            if (targetPokemon.bProps.forestsCurses[k].typeID == removableTypes[i])
                                            {
                                                forestsCursesRemoved.Add(targetPokemon.bProps.forestsCurses[k]);
                                                if (!typeRemoved)
                                                {
                                                    typeRemoved = true;
                                                    typesRemoved.Add(removableTypes[i]);
                                                }
                                            }
                                        }
                                    }

                                    // Only record success if there actually types to be removed
                                    if (typesRemoved.Count > 0)
                                    {
                                        effectSuccess = true;
                                        if (apply)
                                        {
                                            for (int i = 0; i < typesRemoved.Count; i++)
                                            {
                                                targetPokemon.bProps.types.Remove(typesRemoved[i]);
                                            }
                                            for (int i = 0; i < forestsCursesRemoved.Count; i++)
                                            {
                                                targetPokemon.bProps.forestsCurses.Remove(forestsCursesRemoved[i]);
                                            }

                                            string textID = effect.GetString(0);
                                            textID = (textID == "DEFAULT") ? 
                                                (allTypes? "move-burnup-all" : "move-burnup") 
                                                : textID;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = textID,
                                                pokemonUserID = userPokemon.uniqueID,
                                                pokemonTargetID = targetPokemon.uniqueID,
                                                moveID = moveData.ID,
                                                typeIDs = new List<string>(typesRemoved)
                                            });
                                        }
                                    }
                                }

                            }
                            // Feint
                            else if (effect.effectType == MoveEffectType.Feint)
                            {
                                // Lifting Protection moves
                                if (targetPokemon.bProps.protect != null)
                                {
                                    effectSuccess = true;
                                    targetPokemon.bProps.protectMove = null;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = "move-feint",
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = targetPokemon.uniqueID
                                    });
                                }

                                // Lifting Mat Block moves
                                if (targetTeam != null)
                                {
                                    if (targetTeam.bProps.protectMovesActive.Count > 0)
                                    {
                                        string textID = effect.GetString(1);
                                        textID = (textID == "DEFAULT") ? "move-feint-matblock-default"
                                            : textID;

                                        for (int i = 0; i < targetTeam.bProps.protectMovesActive.Count; i++)
                                        {
                                            MoveData protectData =
                                                MoveDatabase.instance.GetMoveData(targetTeam.bProps.protectMovesActive[i]);
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = textID,
                                                pokemonUserID = userPokemon.uniqueID,
                                                pokemonTargetID = targetPokemon.uniqueID,
                                                teamID = targetTeam.teamID,
                                                moveID = protectData.ID
                                            });
                                        }
                                        targetTeam.bProps.protectMovesActive.Clear();
                                        effectSuccess = true;
                                    }
                                }
                            }
                            // Flinch
                            else if (effect.effectType == MoveEffectType.Flinch)
                            {
                                // TODO: checks to see if the target can flinch
                                if (battle.PBPLegacyGetAbilityDataWithEffect(targetPokemon, AbilityEffectType.InnerFocus) == null)
                                {
                                    if (apply)
                                    {
                                        targetPokemon.bProps.isFlinching = true;
                                    }
                                    effectSuccess = true;
                                }
                            }
                            // Follow Me
                            else if (effect.effectType == MoveEffectType.FollowMe)
                            {
                                if (apply)
                                {
                                    // TODO: checks to see if the target can be set to the center of attention
                                    targetPokemon.bProps.isCenterOfAttention = true;

                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-followme-default" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = targetPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }
                                effectSuccess = true;
                            }
                            // Forest's Curse
                            else if (effect.effectType == MoveEffectType.ForestsCurse
                                && effect.effectTargetType == MoveEffectTargetType.Target)
                            {
                                List<string> forestsCurseTypes = new List<string>();
                                for (int i = 1; i < effect.stringParams.Length; i++)
                                {
                                    forestsCurseTypes.Add(effect.stringParams[i]);
                                }
                                bool allTypes = false;
                                if (forestsCurseTypes.Contains("ALL"))
                                {
                                    allTypes = true;
                                    forestsCurseTypes = TypeDatabase.instance.GetAllTypes();
                                }

                                bool replaceForestsCurse = !effect.GetBool(0);
                                List<string> addableTypes = new List<string>();
                                for (int i = 0; i < forestsCurseTypes.Count; i++)
                                {
                                    bool canAddType = true;
                                    string curType = forestsCurseTypes[i];

                                    // Can't add natural type unless specified
                                    if (targetPokemon.bProps.types.Contains(curType) && !effect.GetBool(1))
                                    {
                                        canAddType = false;
                                    }

                                    // Check existing Forest's Curse effects
                                    if (canAddType)
                                    {
                                        for (int k = 0; k < targetPokemon.bProps.forestsCurses.Count; k++)
                                        {
                                            // Can't add same forest curse effect
                                            Pokemon.BattleProperties.ForestsCurse forestCurse = 
                                                targetPokemon.bProps.forestsCurses[k];
                                            if (forestCurse.moveID == moveData.ID
                                                && forestCurse.typeID == curType)
                                            {
                                                canAddType = false;
                                                break;
                                            }

                                            MoveData forestCurseData =
                                                MoveDatabase.instance.GetMoveData(
                                                    targetPokemon.bProps.forestsCurses[k].moveID
                                                    );
                                        
                                            // Replace non-stackable forest curse effects
                                            MoveEffect forestCurseEffect =
                                                forestCurseData.GetEffect(MoveEffectType.ForestsCurse);
                                            if (!forestCurseEffect.GetBool(0))
                                            {
                                                replaceForestsCurse = true;
                                            }
                                        }
                                    }
                            
                                    if (canAddType)
                                    {
                                        addableTypes.Add(curType);
                                    }
                                }

                                // Only record success if there are actually types to add
                                if (addableTypes.Count > 0)
                                {
                                    effectSuccess = true;
                                    if (apply)
                                    {
                                        int turns = Mathf.FloorToInt(effect.GetFloat(0));
                                        if (replaceForestsCurse)
                                        {
                                            targetPokemon.bProps.forestsCurses.Clear();
                                        }

                                        for (int i = 0; i < addableTypes.Count; i++)
                                        {
                                            Pokemon.BattleProperties.ForestsCurse forestCurse = 
                                                new Pokemon.BattleProperties.ForestsCurse(
                                                    moveData.ID,
                                                    typeID: addableTypes[i],
                                                    turns
                                                );
                                            targetPokemon.bProps.forestsCurses.Add(forestCurse);
                                        }

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ?
                                            (allTypes ? "move-forestscurse-all" : "move-forestscurse")
                                            : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID,
                                            typeIDs = new List<string>(addableTypes)
                                        });
                                    }
                                }
                            }
                            // Haze
                            else if (effect.effectType == MoveEffectType.Haze
                                && effect.effectTargetType == MoveEffectTargetType.Target)
                            {
                                if (apply)
                                {
                                    List<PokemonStats> statsToModify = GameTextDatabase.GetStatsFromList(effect.stringParams);
                                    if (statsToModify.Contains(PokemonStats.Attack))
                                    {
                                        targetPokemon.bProps.ATKStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.Defense))
                                    {
                                        targetPokemon.bProps.DEFStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.SpecialAttack))
                                    {
                                        targetPokemon.bProps.SPAStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.SpecialDefense))
                                    {
                                        targetPokemon.bProps.SPDStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.Speed))
                                    {
                                        targetPokemon.bProps.SPEStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.Accuracy))
                                    {
                                        targetPokemon.bProps.ACCStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.Evasion))
                                    {
                                        targetPokemon.bProps.EVAStage = 0;
                                    }

                                    string textID = "move-haze-pokemon";
                                    MoveEffect hazeText = moveData.GetEffect(MoveEffectType.HazeText);
                                    if (hazeText != null)
                                    {
                                        textID = hazeText.GetString(0);
                                    }
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = targetPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }

                                effectSuccess = true;
                            }
                            // Helping Hand
                            else if (effect.effectType == MoveEffectType.HelpingHand)
                            {
                                if (!targetPokemon.bProps.actedThisTurn)
                                {
                                    if (apply)
                                    {
                                        targetPokemon.bProps.helpingHandMoves.Add(moveData.ID);

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-helpinghand-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                    effectSuccess = true;
                                }
                            }
                            // Hold Hands
                            else if (effect.effectType == MoveEffectType.HoldHands)
                            {
                                if (battle.IsPokemonOnField(userPokemon)
                                    && !battle.IsPokemonFainted(userPokemon))
                                {
                                    if (apply)
                                    {
                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-holdhands-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                    effectSuccess = true;
                                }
                            }
                            // Ingrain
                            else if (effect.effectType == MoveEffectType.Ingrain)
                            {
                                if (!targetPokemon.bProps.ingrainMoves.Contains(moveData.ID))
                                {
                                    if (apply)
                                    {
                                        targetPokemon.bProps.ingrainMoves.Add(moveData.ID);

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-ingrain" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                    effectSuccess = true;
                                }
                            }
                            // Leech Seed
                            else if (effect.effectType == MoveEffectType.LeechSeed)
                            {
                                if (userPokemon != null)
                                {
                                    if (battle.IsPokemonOnField(userPokemon))
                                    {
                                        bool canBeSeeded = true;
                                        if (!string.IsNullOrEmpty(targetPokemon.bProps.leechSeedMove))
                                        {
                                            canBeSeeded = false;
                                        }
                                        if (canBeSeeded)
                                        {
                                            MoveEffect typeImmunity = moveData.GetEffect(MoveEffectType.LeechSeedTypeImmunity);
                                            if (typeImmunity != null)
                                            {
                                                if (battle.DoesPokemonHaveATypeInList(
                                                    targetPokemon,
                                                    typeImmunity.stringParams))
                                                {
                                                    canBeSeeded = false;
                                                }
                                            }
                                        }

                                        if (canBeSeeded && apply)
                                        {
                                            targetPokemon.bProps.leechSeedMove = moveData.ID;
                                            targetPokemon.bProps.leechSeedPosition = battle.GetPokemonPosition(userPokemon);

                                            string textID = effect.GetString(0);
                                            textID = (textID == "DEFAULT") ? "move-leechseed-seed-default" : textID;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = textID,
                                                pokemonUserID = userPokemon.uniqueID,
                                                pokemonTargetID = targetPokemon.uniqueID,
                                                moveID = moveData.ID
                                            });
                                        }
                                        else if (forceEffectDisplay)
                                        {
                                            string textID = effect.GetString(2);
                                            textID = (textID == "DEFAULT") ? "move-leechseed-fail-default" : textID;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = textID,
                                                pokemonUserID = userPokemon.uniqueID,
                                                pokemonTargetID = targetPokemon.uniqueID,
                                                moveID = moveData.ID
                                            });
                                        }

                                        effectSuccess = canBeSeeded;
                                    }
                                }
                            }
                            // Lock On
                            else if (effect.effectType == MoveEffectType.LockOn)
                            {
                                if (battle.IsPokemonOnFieldAndAble(userPokemon))
                                {
                                    bool canLockOn = true;
                                    string textID = effect.GetString(2);
                                    textID = (textID == "DEFAULT") ? "move-lockon-fail" : textID;

                                    if (canLockOn && battle.IsPokemonLockedOn(userPokemon, targetPokemon))
                                    {
                                        canLockOn = false;
                                    }

                                    if (canLockOn && apply)
                                    {
                                        int turns = Mathf.FloorToInt(effect.GetFloat(0));

                                        Pokemon.BattleProperties.LockOn lockOnTarget = 
                                            new Pokemon.BattleProperties.LockOn(
                                                targetPokemon.uniqueID,
                                                moveData.ID,
                                                turns
                                                );
                                        userPokemon.bProps.lockOnTargets.Add(lockOnTarget);

                                        textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-lockon" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                    else if (!canLockOn && forceEffectDisplay)
                                    {
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }

                                    effectSuccess = canLockOn;
                                }
                            }
                            // Powder
                            else if (effect.effectType == MoveEffectType.Powder)
                            {
                                if (apply)
                                {
                                    targetPokemon.bProps.powderMove = moveData.ID;

                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-powder-start-default" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = targetPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }
                                effectSuccess = true;
                            }
                            // Power Trick
                            else if (effect.effectType == MoveEffectType.PowerTrick)
                            {
                                PokemonStats statToSwap1 = GameTextDatabase.GetStatFromString(effect.GetString(1));
                                PokemonStats statToSwap2 = GameTextDatabase.GetStatFromString(effect.GetString(2));

                                if (apply)
                                {
                                    PokemonStats preStat1 
                                        = (statToSwap1 == PokemonStats.Attack) ? targetPokemon.bProps.ATKMappedStat
                                        : (statToSwap1 == PokemonStats.Defense) ? targetPokemon.bProps.DEFMappedStat
                                        : (statToSwap1 == PokemonStats.SpecialAttack) ? targetPokemon.bProps.SPAMappedStat
                                        : (statToSwap1 == PokemonStats.SpecialDefense) ? targetPokemon.bProps.SPDMappedStat
                                        : (statToSwap1 == PokemonStats.Speed) ? targetPokemon.bProps.SPEMappedStat
                                        : statToSwap1;

                                    PokemonStats preStat2 
                                        = (statToSwap2 == PokemonStats.Attack) ? targetPokemon.bProps.ATKMappedStat
                                        : (statToSwap2 == PokemonStats.Defense) ? targetPokemon.bProps.DEFMappedStat
                                        : (statToSwap2 == PokemonStats.SpecialAttack) ? targetPokemon.bProps.SPAMappedStat
                                        : (statToSwap2 == PokemonStats.SpecialDefense) ? targetPokemon.bProps.SPDMappedStat
                                        : (statToSwap2 == PokemonStats.Speed) ? targetPokemon.bProps.SPEMappedStat
                                        : statToSwap2;

                                    switch (statToSwap1)
                                    {
                                        case PokemonStats.Attack:
                                            targetPokemon.bProps.ATKMappedStat = preStat2;
                                            break;
                                        case PokemonStats.Defense:
                                            targetPokemon.bProps.DEFMappedStat = preStat2;
                                            break;
                                        case PokemonStats.SpecialAttack:
                                            targetPokemon.bProps.SPAMappedStat = preStat2;
                                            break;
                                        case PokemonStats.SpecialDefense:
                                            targetPokemon.bProps.SPDMappedStat = preStat2;
                                            break;
                                        case PokemonStats.Speed:
                                            targetPokemon.bProps.SPEMappedStat = preStat2;
                                            break;
                                    }
                                    switch (statToSwap2)
                                    {
                                        case PokemonStats.Attack:
                                            targetPokemon.bProps.ATKMappedStat = preStat1;
                                            break;
                                        case PokemonStats.Defense:
                                            targetPokemon.bProps.DEFMappedStat = preStat1;
                                            break;
                                        case PokemonStats.SpecialAttack:
                                            targetPokemon.bProps.SPAMappedStat = preStat1;
                                            break;
                                        case PokemonStats.SpecialDefense:
                                            targetPokemon.bProps.SPDMappedStat = preStat1;
                                            break;
                                        case PokemonStats.Speed:
                                            targetPokemon.bProps.SPEMappedStat = preStat1;
                                            break;
                                    }

                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-powertrick" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = targetPokemon.uniqueID,
                                        moveID = moveData.ID,
                                        statList = new List<PokemonStats> { statToSwap1, statToSwap2 }
                                    });
                                }
                                effectSuccess = true;
                            }
                            // Recover / Heal Pulse
                            else if (effect.effectType == MoveEffectType.Recover)
                            {
                                bool canRecover = true;
                                int HPGain = battle.GetPokemonHPByPercent(targetPokemon, effect.GetFloat(0));
                                int preHP = targetPokemon.currentHP;
                                int recoverableHP = targetPokemon.HPdifference;

                                if (recoverableHP == 0)
                                {
                                    canRecover = false;
                                    if (forceEffectDisplay)
                                    {
                                        string textID = effect.GetString(1);
                                        textID = (textID == "DEFAULT") ? "move-recover-fail-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                }
                                if (canRecover && apply)
                                {
                                    int hpRecovered = battle.AddPokemonHP(targetPokemon, HPGain);
                                    int postHP = targetPokemon.currentHP;

                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-recover-success-default" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonTargetID = targetPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });

                                    yield return StartCoroutine(PBPChangePokemonHP(
                                        pokemon: targetPokemon,
                                        preHP: preHP,
                                        hpChange: hpRecovered,
                                        postHP: postHP,
                                        heal: true
                                    ));
                                }
                                effectSuccess = canRecover;
                            
                            }
                            // Recycle
                            else if (effect.effectType == MoveEffectType.Recycle)
                            {
                                if (targetItem == null
                                    && !string.IsNullOrEmpty(targetPokemon.bProps.consumedItem))
                                {
                                    if (apply)
                                    {
                                        Item recycledItem = new Item(targetPokemon.bProps.consumedItem);
                                        targetPokemon.SetItem(recycledItem);

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-recycle-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            moveID = moveData.ID,
                                            itemID = recycledItem.itemID
                                        });
                                    }
                                    effectSuccess = true;
                                }
                            }
                            // Refresh
                            else if (effect.effectType == MoveEffectType.Refresh)
                            {
                                bool canRefresh = true;

                                List<StatusCondition> statusConditions = battle.GetPokemonStatusConditions(targetPokemon);
                                List<StatusCondition> healableConditions = new List<StatusCondition>();
                                bool invertFilter = effect.GetBool(0);
                                bool nonVolatileOnly = effect.GetBool(1);

                                List<string> healableStatuses = new List<string>(effect.stringParams);
                                for (int i = 0; i < statusConditions.Count; i++)
                                {
                                    bool canHeal = true;

                                    if (!healableStatuses.Contains("ALL"))
                                    {
                                        canHeal = healableStatuses.Contains(statusConditions[i].statusID) == !invertFilter;
                                    }

                                    if (nonVolatileOnly && !statusConditions[i].data.HasTag(PokemonSTag.NonVolatile))
                                    {
                                        canHeal = false;
                                    }
                                    if (statusConditions[i].data.HasTag(PokemonSTag.IsDefault))
                                    {
                                        canHeal = false;
                                    }

                                    if (canHeal)
                                    {
                                        healableConditions.Add(statusConditions[i]);
                                    }
                                }

                                canRefresh = healableConditions.Count > 0;

                                if (apply && canRefresh)
                                {
                                    for (int i = 0; i < healableConditions.Count; i++)
                                    {
                                        yield return StartCoroutine(HealPokemonSC(
                                            targetPokemon: targetPokemon,
                                            condition: healableConditions[i]
                                            ));
                                    }
                                }
                                else if (!canRefresh && forceEffectDisplay)
                                {
                                    string textID = "move-refresh-fail";
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonTargetID = targetPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }

                                effectSuccess = canRefresh;
                            }
                            // Smack Down
                            else if (effect.effectType == MoveEffectType.SmackDown)
                            {
                                if (!battle.PBPIsPokemonGrounded(targetPokemon))
                                {
                                    effectSuccess = true;
                                    if (apply)
                                    {
                                        targetPokemon.bProps.isSmackedDown = true;

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-smackdown-default" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID
                                        });
                                    }
                                }
                            }
                            // Soak
                            else if (effect.effectType == MoveEffectType.Soak
                                && effect.effectTargetType == MoveEffectTargetType.Target)
                            {
                                // Fails on Multitype
                                if (battle.PBPLegacyGetAbilityEffect(targetPokemon, AbilityEffectType.Multitype) == null)
                                {
                                    List<string> soakTypes = new List<string>();
                                    for (int i = 1; i < effect.stringParams.Length; i++)
                                    {
                                        soakTypes.Add(effect.stringParams[i]);
                                    }
                                    bool allTypes = false;
                                    if (soakTypes.Contains("ALL"))
                                    {
                                        allTypes = true;
                                        soakTypes = TypeDatabase.instance.GetAllTypes();
                                    }

                                    // Only record success if there actually types to be set
                                    if (soakTypes.Count > 0)
                                    {
                                        effectSuccess = true;
                                        if (apply)
                                        {
                                            targetPokemon.bProps.types = new List<string>(soakTypes);

                                            if (!effect.GetBool(0))
                                            {
                                                targetPokemon.bProps.forestsCurses.Clear();
                                            }

                                            string textID = effect.GetString(0);
                                            textID = (textID == "DEFAULT") ?
                                                (allTypes ? "move-soak-all" : "move-soak")
                                                : textID;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = textID,
                                                pokemonUserID = userPokemon.uniqueID,
                                                pokemonTargetID = targetPokemon.uniqueID,
                                                moveID = moveData.ID,
                                                typeIDs = new List<string>(soakTypes)
                                            });
                                        }
                                    }
                                }

                            }
                            // Spectral Thief
                            else if (effect.effectType == MoveEffectType.SpectralThief
                                && battle.IsPokemonOnField(userPokemon)
                                && !battle.IsPokemonFainted(userPokemon))
                            {
                                bool stoleStats = false;

                                List<PokemonStats> statsToSteal = GameTextDatabase.GetStatsFromList(effect.stringParams);
                                for (int i = 0; i < statsToSteal.Count; i++)
                                {
                                    bool stealStat = true;
                                    int statMod = 0;
                                    PokemonStats curStat = statsToSteal[i];

                                    if (curStat == PokemonStats.Attack)
                                    {
                                        if (targetPokemon.bProps.ATKStage != 0)
                                        {
                                            statMod = targetPokemon.bProps.ATKStage;
                                            if (effect.GetBool(0) && statMod < 0)
                                            {
                                                stealStat = false;
                                            }
                                            if (stealStat)
                                            {
                                                targetPokemon.bProps.ATKStage = 0;
                                            }
                                        }
                                    }
                                    else if (curStat == PokemonStats.Defense)
                                    {
                                        if (targetPokemon.bProps.DEFStage != 0)
                                        {
                                            statMod = targetPokemon.bProps.DEFStage;
                                            if (effect.GetBool(0) && statMod < 0)
                                            {
                                                stealStat = false;
                                            }
                                            if (stealStat)
                                            {
                                                targetPokemon.bProps.DEFStage = 0;
                                            }
                                        }
                                    }
                                    else if (curStat == PokemonStats.SpecialAttack)
                                    {
                                        if (targetPokemon.bProps.SPAStage != 0)
                                        {
                                            statMod = targetPokemon.bProps.SPAStage;
                                            if (effect.GetBool(0) && statMod < 0)
                                            {
                                                stealStat = false;
                                            }
                                            if (stealStat)
                                            {
                                                targetPokemon.bProps.SPAStage = 0;
                                            }
                                        }
                                    }
                                    else if (curStat == PokemonStats.SpecialDefense)
                                    {
                                        if (targetPokemon.bProps.SPDStage != 0)
                                        {
                                            statMod = targetPokemon.bProps.SPDStage;
                                            if (effect.GetBool(0) && statMod < 0)
                                            {
                                                stealStat = false;
                                            }
                                            if (stealStat)
                                            {
                                                targetPokemon.bProps.SPDStage = 0;
                                            }
                                        }
                                    }
                                    else if (curStat == PokemonStats.Speed)
                                    {
                                        if (targetPokemon.bProps.SPEStage != 0)
                                        {
                                            statMod = targetPokemon.bProps.SPEStage;
                                            if (effect.GetBool(0) && statMod < 0)
                                            {
                                                stealStat = false;
                                            }
                                            if (stealStat)
                                            {
                                                targetPokemon.bProps.SPEStage = 0;
                                            }
                                        }
                                    }
                                    else if (curStat == PokemonStats.Accuracy)
                                    {
                                        if (targetPokemon.bProps.ACCStage != 0)
                                        {
                                            statMod = targetPokemon.bProps.ACCStage;
                                            if (effect.GetBool(0) && statMod < 0)
                                            {
                                                stealStat = false;
                                            }
                                            if (stealStat)
                                            {
                                                targetPokemon.bProps.ACCStage = 0;
                                            }
                                        }
                                    }
                                    else if (curStat == PokemonStats.Evasion)
                                    {
                                        if (targetPokemon.bProps.EVAStage != 0)
                                        {
                                            statMod = targetPokemon.bProps.EVAStage;
                                            if (effect.GetBool(0) && statMod < 0)
                                            {
                                                stealStat = false;
                                            }
                                            if (stealStat)
                                            {
                                                targetPokemon.bProps.EVAStage = 0;
                                            }
                                        }
                                    }

                                    if (stealStat && statMod != 0)
                                    {
                                        if (!stoleStats)
                                        {
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = "move-spectralthief-default",
                                                pokemonUserID = userPokemon.uniqueID,
                                                pokemonTargetID = targetPokemon.uniqueID
                                            });
                                        }

                                        yield return StartCoroutine(TryToApplyStatStageMods(
                                        statsToModify: new List<PokemonStats> { curStat },
                                        modValue: statMod,
                                        targetPokemon: userPokemon,
                                        moveData: moveData,
                                        textCode: null,
                                        forceFailureMessage: forceEffectDisplay,
                                        apply: apply,
                                        callback: (result) =>
                                        {
                                            if (result)
                                            {
                                                stoleStats = true;
                                            }
                                        }
                                        ));
                                        effectSuccess = true;
                                    }
                                }
                            }
                            // Stat Stage Changes
                            else if (effect.effectType == MoveEffectType.StatStageMod
                                || effect.effectType == MoveEffectType.StatStageMax)
                            {
                                bool maximizeStats = (effect.effectType == MoveEffectType.StatStageMax)
                                    ? effect.GetBool(1) : false;
                                bool minimizeStats = (effect.effectType == MoveEffectType.StatStageMax)
                                    ? !effect.GetBool(1) : false;

                                int statMod = (effect.effectType == MoveEffectType.StatStageMod)
                                    ? Mathf.FloorToInt(effect.GetFloat(0)) : 0;

                                List<PokemonStats> statsToModify = GameTextDatabase.GetStatsFromList(effect.stringParams);
                                if (statsToModify.Count > 0)
                                {
                                    yield return StartCoroutine(TryToApplyStatStageMods(
                                        statsToModify: statsToModify,
                                        modValue: statMod,
                                        targetPokemon: targetPokemon,
                                        userPokemon: userPokemon,
                                        moveData: moveData,
                                        maximize: maximizeStats,
                                        minimize: minimizeStats,
                                        forceFailureMessage: forceEffectDisplay,
                                        apply: apply,
                                        callback: (result) =>
                                        {
                                            effectSuccess = result;
                                        }
                                        ));
                                }
                            }
                            // Status Conditions
                            else if (effect.effectType == MoveEffectType.InflictPokemonSC)
                            {
                                string statusID = effect.GetString(0);
                                StatusPKData statusData =
                                    StatusPKDatabase.instance.GetStatusData(statusID);

                                // turn calculation
                                int turnCount = Mathf.FloorToInt(effect.GetFloat(0));

                                // Use Default Turns Left
                                if (effect.GetBool(0))
                                {
                                    PokemonCEff turnEffect = statusData.GetEffect(PokemonSEType.DefaultTurnsLeft);
                                    if (turnEffect != null)
                                    {
                                        turnCount = Mathf.FloorToInt(turnEffect.GetFloat(0));
                                        if (turnEffect.GetBool(0))
                                        {
                                            int minTurns = Mathf.FloorToInt(turnEffect.GetFloat(0));
                                            int maxTurns = Mathf.FloorToInt(turnEffect.GetFloat(1));
                                            turnCount = Random.Range(minTurns, maxTurns);
                                        }
                                    }
                                }
                                // Use the turn range
                                else if (effect.GetBool(1))
                                {
                                    int minTurns = Mathf.FloorToInt(effect.GetFloat(0));
                                    int maxTurns = Mathf.FloorToInt(effect.GetFloat(1));
                                    turnCount = Random.Range(minTurns, maxTurns);
                                }

                                // try to apply the status condition
                                yield return StartCoroutine(TryToInflictPokemonSC(
                                    statusID: statusID,
                                    targetPokemon: targetPokemon,
                                    turnsLeft: turnCount,
                                    userPokemon: userPokemon,
                                    inflictingMove: moveData,
                                    forceFailureMessage: forceEffectDisplay,
                                    apply: apply,
                                    callback: (result) =>
                                    {
                                        effectSuccess = result;
                                    }
                                    ));
                            
                            }
                            // Stuff Cheeks
                            else if (effect.effectType == MoveEffectType.StuffCheeks
                                || effect.effectType == MoveEffectType.StuffCheeksMax)
                            {
                                if (targetItem != null)
                                {
                                    if (targetItem.data.pocket == ItemPocket.Berries
                                        && battle.CanPokemonItemBeLost(targetPokemon, targetItem))
                                    {
                                        // consume berry
                                        if (apply)
                                        {
                                            yield return StartCoroutine(ConsumeItem(
                                                pokemon: targetPokemon,
                                                item: targetItem,
                                                holderPokemon: targetPokemon,
                                                callback: (result) =>
                                                {

                                                }
                                                ));
                                        }

                                        // modify stats
                                        bool maximizeStats = (effect.effectType == MoveEffectType.StuffCheeksMax)
                                        ? effect.GetBool(1) : false;
                                        bool minimizeStats = (effect.effectType == MoveEffectType.StuffCheeksMax)
                                            ? !effect.GetBool(1) : false;

                                        int statMod = (effect.effectType == MoveEffectType.StuffCheeks)
                                        ? Mathf.FloorToInt(effect.GetFloat(0)) : 0;

                                        List<PokemonStats> statsToModify = GameTextDatabase.GetStatsFromList(effect.stringParams);
                                        if (statsToModify.Count > 0)
                                        {
                                            yield return StartCoroutine(TryToApplyStatStageMods(
                                                statsToModify: statsToModify,
                                                modValue: statMod,
                                                targetPokemon: targetPokemon,
                                                userPokemon: userPokemon,
                                                moveData: moveData,
                                                maximize: maximizeStats,
                                                minimize: minimizeStats,
                                                forceFailureMessage: forceEffectDisplay,
                                                apply: apply,
                                                callback: (result) =>
                                                {
                                                
                                                }
                                                ));
                                        }
                                        effectSuccess = true;
                                    }
                                }
                            }
                            // Teatime
                            else if (effect.effectType == MoveEffectType.Teatime)
                            {
                                if (targetItem != null)
                                {
                                    if (targetItem.data.pocket == ItemPocket.Berries
                                        && battle.CanPokemonItemBeLost(targetPokemon, targetItem))
                                    {
                                        // consume berry
                                        yield return StartCoroutine(ConsumeItem(
                                            pokemon: targetPokemon,
                                            item: targetItem,
                                            holderPokemon: targetPokemon,
                                            callback: (result) =>
                                            {

                                            }
                                            ));
                                    }
                                }
                            }
                            // Thaw Target
                            else if (effect.effectType == MoveEffectType.ThawTarget)
                            {
                                StatusCondition condition = 
                                    battle.GetPokemonFilteredStatus(targetPokemon, PokemonSEType.Freeze);
                                if (condition != null)
                                {
                                    effectSuccess = true;
                                    if (apply)
                                    {
                                        yield return StartCoroutine(HealPokemonSC(
                                            targetPokemon: targetPokemon,
                                            condition: condition
                                            ));
                                    }
                                }
                            }
                        }
                    }
                }
            
                // user-only effects
                if (userPokemon != null)
                {
                    if (battle.IsPokemonOnField(userPokemon))
                    {
                        if (!battle.IsPokemonFainted(userPokemon))
                        {
                            // Burn Up
                            if (effect.effectType == MoveEffectType.BurnUp
                                && effect.effectTargetType == MoveEffectTargetType.Self)
                            {
                                // Fails on Multitype
                                if (battle.PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.Multitype) == null)
                                {
                                    List<string> removableTypes = new List<string>();
                                    for (int i = 1; i < effect.stringParams.Length; i++)
                                    {
                                        removableTypes.Add(effect.stringParams[i]);
                                    }
                                    bool allTypes = false;
                                    if (removableTypes.Contains("ALL"))
                                    {
                                        allTypes = true;
                                        removableTypes = TypeDatabase.instance.GetAllTypes();
                                    }

                                    List<string> typesRemoved = new List<string>();
                                    List<Pokemon.BattleProperties.ForestsCurse> forestsCursesRemoved
                                        = new List<Pokemon.BattleProperties.ForestsCurse>();
                                    for (int i = 0; i < removableTypes.Count; i++)
                                    {
                                        bool typeRemoved = false;
                                        if (userPokemon.bProps.types.Contains(removableTypes[i]))
                                        {
                                            typeRemoved = true;
                                            typesRemoved.Add(removableTypes[i]);
                                        }
                                        for (int k = 0; k < userPokemon.bProps.forestsCurses.Count; k++)
                                        {
                                            if (userPokemon.bProps.forestsCurses[k].typeID == removableTypes[i])
                                            {
                                                forestsCursesRemoved.Add(userPokemon.bProps.forestsCurses[k]);
                                                if (!typeRemoved)
                                                {
                                                    typeRemoved = true;
                                                    typesRemoved.Add(removableTypes[i]);
                                                }
                                            }
                                        }
                                    }

                                    // Only record success if there actually types to be removed
                                    if (typesRemoved.Count > 0)
                                    {
                                        effectSuccess = true;
                                        if (apply)
                                        {
                                            for (int i = 0; i < typesRemoved.Count; i++)
                                            {
                                                userPokemon.bProps.types.Remove(typesRemoved[i]);
                                            }
                                            for (int i = 0; i < forestsCursesRemoved.Count; i++)
                                            {
                                                userPokemon.bProps.forestsCurses.Remove(forestsCursesRemoved[i]);
                                            }

                                            string textID = effect.GetString(0);
                                            textID = (textID == "DEFAULT") ?
                                                (allTypes ? "move-burnup-all" : "move-burnup")
                                                : textID;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = textID,
                                                pokemonTargetID = userPokemon.uniqueID,
                                                moveID = moveData.ID,
                                                typeIDs = new List<string>(typesRemoved)
                                            });
                                        }
                                    }
                                }

                            }
                            // Destiny Bond
                            else if (effect.effectType == MoveEffectType.DestinyBond)
                            {
                                if (apply)
                                {
                                    userPokemon.bProps.destinyBondMove = moveData.ID;
                                
                                    // snatch wait event
                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-destinybond-start-default" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }
                                effectSuccess = true;
                            }
                            // Endure
                            else if (effect.effectType == MoveEffectType.Endure)
                            {
                                if (apply)
                                {
                                    userPokemon.bProps.endureMove = moveData.ID;
                                    userPokemon.bProps.protectCounter++;

                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-endure-start-default" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }
                                effectSuccess = true;
                            }
                            // Forest's Curse
                            else if (effect.effectType == MoveEffectType.ForestsCurse
                                && effect.effectTargetType == MoveEffectTargetType.Self)
                            {
                                List<string> forestsCurseTypes = new List<string>();
                                for (int i = 1; i < effect.stringParams.Length; i++)
                                {
                                    forestsCurseTypes.Add(effect.stringParams[i]);
                                }
                                bool allTypes = false;
                                if (forestsCurseTypes.Contains("ALL"))
                                {
                                    allTypes = true;
                                    forestsCurseTypes = TypeDatabase.instance.GetAllTypes();
                                }

                                bool replaceForestsCurse = !effect.GetBool(0);
                                List<string> addableTypes = new List<string>();
                                for (int i = 0; i < forestsCurseTypes.Count; i++)
                                {
                                    bool canAddType = true;
                                    string curType = forestsCurseTypes[i];

                                    // Can't add natural type unless specified
                                    if (userPokemon.bProps.types.Contains(curType) && !effect.GetBool(1))
                                    {
                                        canAddType = false;
                                    }

                                    // Check existing Forest's Curse effects
                                    if (canAddType)
                                    {
                                        for (int k = 0; k < userPokemon.bProps.forestsCurses.Count; k++)
                                        {
                                            // Can't add same forest curse effect
                                            Pokemon.BattleProperties.ForestsCurse forestCurse =
                                                userPokemon.bProps.forestsCurses[k];
                                            if (forestCurse.moveID == moveData.ID
                                                && forestCurse.typeID == curType)
                                            {
                                                canAddType = false;
                                                break;
                                            }

                                            MoveData forestCurseData =
                                                MoveDatabase.instance.GetMoveData(
                                                    userPokemon.bProps.forestsCurses[k].moveID
                                                    );

                                            // Replace non-stackable forest curse effects
                                            MoveEffect forestCurseEffect =
                                                forestCurseData.GetEffect(MoveEffectType.ForestsCurse);
                                            if (!forestCurseEffect.GetBool(0))
                                            {
                                                replaceForestsCurse = true;
                                            }
                                        }
                                    }

                                    if (canAddType)
                                    {
                                        addableTypes.Add(curType);
                                    }
                                }

                                // Only record success if there are actually types to add
                                if (addableTypes.Count > 0)
                                {
                                    effectSuccess = true;
                                    if (apply)
                                    {
                                        int turns = Mathf.FloorToInt(effect.GetFloat(0));
                                        if (replaceForestsCurse)
                                        {
                                            userPokemon.bProps.forestsCurses.Clear();
                                        }

                                        for (int i = 0; i < addableTypes.Count; i++)
                                        {
                                            Pokemon.BattleProperties.ForestsCurse forestCurse =
                                                new Pokemon.BattleProperties.ForestsCurse(
                                                    moveData.ID,
                                                    typeID: addableTypes[i],
                                                    turns
                                                );
                                            userPokemon.bProps.forestsCurses.Add(forestCurse);
                                        }

                                        string textID = effect.GetString(0);
                                        textID = (textID == "DEFAULT") ?
                                            (allTypes ? "move-forestscurse-all" : "move-forestscurse")
                                            : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonTargetID = userPokemon.uniqueID,
                                            moveID = moveData.ID,
                                            typeIDs = new List<string>(addableTypes)
                                        });
                                    }
                                }
                            }
                            // Magic Coat
                            else if (effect.effectType == MoveEffectType.MagicCoat)
                            {
                                if (apply)
                                {
                                    userPokemon.bProps.isMagicCoatActive = true;
                                    userPokemon.bProps.magicCoatMove = effect.GetString(0);
                                }
                                effectSuccess = true;
                            }
                            // Minimize
                            else if (effect.effectType == MoveEffectType.Minimize)
                            {
                                if (apply)
                                {
                                    userPokemon.bProps.isMinimizeActive = true;
                                }
                                effectSuccess = true;
                            }
                            // Protect
                            else if (effect.effectType == MoveEffectType.Protect)
                            {
                                if (apply)
                                {
                                    userPokemon.bProps.protectMove = moveData.ID;
                                    userPokemon.bProps.protectCounter++;

                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-protect-start-default"
                                        : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }
                                effectSuccess = true;
                            }
                            // Rest
                            else if (effect.effectType == MoveEffectType.Rest)
                            {
                                // Put user to sleep
                                string textID = effect.GetString(0);
                                textID = (textID == "DEFAULT") ? "move-rest-default" : textID;

                                bool canRest = false;
                                yield return StartCoroutine(TryToInflictPokemonSC(
                                    statusID: "sleep",
                                    targetPokemon: userPokemon,
                                    bypassOtherConditions: true,
                                    turnsLeft: 3,
                                    inflictOverwrite: textID,
                                    callback: (result) =>
                                    {
                                        canRest = result;
                                    }
                                    ));

                                // Heal user
                                int preHP = userPokemon.currentHP;
                                int hpRecovered = battle.AddPokemonHP(userPokemon, userPokemon.maxHP);
                                int postHP = userPokemon.currentHP;

                                yield return StartCoroutine(PBPChangePokemonHP(
                                    pokemon: userPokemon,
                                    preHP: preHP,
                                    hpChange: hpRecovered,
                                    postHP: postHP,
                                    heal: true
                                ));
                            }
                            // Roost
                            else if (effect.effectType == MoveEffectType.Roost
                                || effect.effectType == MoveEffectType.RoostTypeLoss)
                            {
                                if (apply)
                                {
                                    userPokemon.bProps.roostMove = moveData.ID;
                                }
                                effectSuccess = true;
                            }
                            // Self Stat Stage Change
                            else if (effect.effectType == MoveEffectType.StatStageSelfMod 
                                || effect.effectType == MoveEffectType.StatStageSelfMax)
                            {
                                bool maximizeStats = (effect.effectType == MoveEffectType.StatStageSelfMax) 
                                    ? effect.GetBool(1) : false;
                                bool minimizeStats = (effect.effectType == MoveEffectType.StatStageSelfMax) 
                                    ? !effect.GetBool(1) : false;

                                int statMod = (effect.effectType == MoveEffectType.StatStageSelfMod) 
                                    ? Mathf.FloorToInt(effect.GetFloat(0)) : 0;

                                List<PokemonStats> statsToModify = GameTextDatabase.GetStatsFromList(effect.stringParams);
                                if (statsToModify.Count > 0)
                                {
                                    yield return StartCoroutine(TryToApplyStatStageMods(
                                        statsToModify: statsToModify,
                                        modValue: statMod,
                                        targetPokemon: userPokemon,
                                        userPokemon: userPokemon,
                                        moveData: moveData,
                                        maximize: maximizeStats,
                                        minimize: minimizeStats,
                                        forceFailureMessage: forceEffectDisplay,
                                        apply: apply,
                                        callback: (result) =>
                                        {
                                            effectSuccess = result;
                                        }
                                        ));
                                }
                            
                            }
                            // Snatch
                            else if (effect.effectType == MoveEffectType.Snatch)
                            {
                                if (apply)
                                {
                                    userPokemon.bProps.snatchMove = moveData.ID;

                                    // snatch wait event
                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-snatch-wait-default"
                                        : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = userPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }
                                effectSuccess = true;
                            }
                            // Soak
                            else if (effect.effectType == MoveEffectType.Soak
                                && effect.effectTargetType == MoveEffectTargetType.Self)
                            {
                                // Fails on Multitype
                                if (battle.PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.Multitype) == null)
                                {
                                    List<string> soakTypes = new List<string>();
                                    for (int i = 1; i < effect.stringParams.Length; i++)
                                    {
                                        soakTypes.Add(effect.stringParams[i]);
                                    }
                                    bool allTypes = false;
                                    if (soakTypes.Contains("ALL"))
                                    {
                                        allTypes = true;
                                        soakTypes = TypeDatabase.instance.GetAllTypes();
                                    }

                                    // Only record success if there actually types to be set
                                    if (soakTypes.Count > 0)
                                    {
                                        effectSuccess = true;
                                        if (apply)
                                        {
                                            userPokemon.bProps.types = new List<string>(soakTypes);

                                            if (!effect.GetBool(0))
                                            {
                                                userPokemon.bProps.forestsCurses.Clear();
                                            }

                                            string textID = effect.GetString(0);
                                            textID = (textID == "DEFAULT") ?
                                                (allTypes ? "move-soak-all" : "move-soak")
                                                : textID;
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = textID,
                                                pokemonTargetID = userPokemon.uniqueID,
                                                moveID = moveData.ID,
                                                typeIDs = new List<string>(soakTypes)
                                            });
                                        }
                                    }
                                }

                            }
                            // Substitute
                            else if (effect.effectType == MoveEffectType.Substitute)
                            {
                                bool canSetUp = true;
                                int subHP =
                                    Mathf.FloorToInt(battle.GetPokemonHPByPercent(userPokemon, effect.GetFloat(0)));
                                int HPLost =
                                    Mathf.FloorToInt(battle.GetPokemonHPByPercent(userPokemon, effect.GetFloat(1)));

                                // can't set up substitute if one's already active
                                if (canSetUp && !string.IsNullOrEmpty(userPokemon.bProps.substituteMove))
                                {
                                    canSetUp = false;
                                    if (forceEffectDisplay)
                                    {
                                        string textID = effect.GetString(2);
                                        textID = (textID == "DEFAULT") ? "move-substitute-already-default"
                                            : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonUserID = userPokemon.uniqueID,
                                            pokemonTargetID = userPokemon.uniqueID,
                                            moveID = moveData.ID
                                        });
                                    }
                                }

                                // can't set up substitute if not enough HP
                                if (canSetUp && HPLost >= userPokemon.currentHP)
                                {
                                    canSetUp = false;
                                    string textID = effect.GetString(3);
                                    textID = (textID == "DEFAULT") ? "move-substitute-fail-default"
                                        : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = userPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }

                                // passed all checks, set up substitute
                                if (canSetUp && apply)
                                {
                                    userPokemon.bProps.substituteMove = moveData.ID;
                                    userPokemon.bProps.substituteHP = subHP;

                                    int preHP = userPokemon.currentHP;
                                    int realHPLost = battle.SubtractPokemonHP(userPokemon, HPLost);
                                    int postHP = userPokemon.currentHP;

                                    yield return StartCoroutine(PBPChangePokemonHP(
                                        pokemon: userPokemon,
                                        preHP: preHP,
                                        hpChange: realHPLost,
                                        postHP: postHP
                                        ));

                                    string textID = effect.GetString(0);
                                    textID = (textID == "DEFAULT") ? "move-substitute-create-default" : textID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = userPokemon.uniqueID,
                                        moveID = moveData.ID
                                    });
                                }

                                effectSuccess = canSetUp;
                            }
                        }
                    }
                }

                // team effects
                if (targetTeam != null)
                {
                    // Aromatherapy
                    if (effect.effectType == MoveEffectType.Aromatherapy)
                    {
                        for (int i = 0; i < targetTeam.trainers.Count; i++)
                        {
                            Trainer trainer = targetTeam.trainers[i];
                            if (trainer.IsTheSameAs(battle.GetPokemonOwner(userPokemon))
                                || effect.GetBool(0))
                            {
                                for (int k = 0; k < trainer.party.Count; k++)
                                {
                                    Pokemon aromaRecipient = trainer.party[k];

                                    if (!battle.IsPokemonFainted(aromaRecipient)
                                        && !aromaRecipient.nonVolatileStatus.data.HasTag(PokemonSTag.IsDefault)
                                        && !battle.IsPokemonOnField(aromaRecipient))
                                    {
                                        if (apply)
                                        {
                                            yield return StartCoroutine(HealPokemonSC(
                                                targetPokemon: aromaRecipient,
                                                condition: aromaRecipient.nonVolatileStatus
                                                ));
                                        }
                                        effectSuccess = true;
                                    }
                                }
                            }
                        }
                    }
                    // Entry Hazards
                    else if (effect.effectType == MoveEffectType.EntryHazard)
                    {
                        bool canSetupHazard = true;
                        BattleTeamProperties.EntryHazard existingHazard = battle.GetTeamEntryHazard(targetTeam, moveData.ID);

                        int turns = Mathf.FloorToInt(effect.GetFloat(0));
                        int maxLayers = Mathf.FloorToInt(effect.GetFloat(1));

                        if (apply && canSetupHazard)
                        {
                            if (existingHazard == null)
                            {
                                BattleTeamProperties.EntryHazard entryHazard = new BattleTeamProperties.EntryHazard(
                                    moveID: moveData.ID,
                                    turnsLeft: turns,
                                    layers: 1
                                    );
                                battle.AddTeamEntryHazard(targetTeam, entryHazard);
                            }
                            else
                            {
                                existingHazard.layers = Mathf.Min(existingHazard.layers + 1, maxLayers);
                            }

                            string textID = effect.GetString(0);
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                teamID = targetTeam.teamID,
                                moveID = moveData.ID
                            });
                        }
                        else if (!canSetupHazard && forceEffectDisplay)
                        {
                            string textID = effect.GetString(1);
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                teamID = targetTeam.teamID,
                                moveID = moveData.ID
                            });
                        }
                        effectSuccess = canSetupHazard;
                    }
                    // Haze
                    else if (effect.effectType == MoveEffectType.Haze
                        && (effect.effectTargetType == MoveEffectTargetType.SelfTeam
                            || effect.effectTargetType == MoveEffectTargetType.Team))
                    {
                        if (apply)
                        {
                            List<Pokemon> fieldPokemon = battle.GetTeamPokemonOnField(targetTeam);
                            if (fieldPokemon.Count > 0)
                            {
                                List<PokemonStats> statsToModify = GameTextDatabase.GetStatsFromList(effect.stringParams);
                                for (int i = 0; i < fieldPokemon.Count; i++)
                                {
                                    if (statsToModify.Contains(PokemonStats.Attack))
                                    {
                                        fieldPokemon[i].bProps.ATKStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.Defense))
                                    {
                                        fieldPokemon[i].bProps.DEFStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.SpecialAttack))
                                    {
                                        fieldPokemon[i].bProps.SPAStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.SpecialDefense))
                                    {
                                        fieldPokemon[i].bProps.SPDStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.Speed))
                                    {
                                        fieldPokemon[i].bProps.SPEStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.Accuracy))
                                    {
                                        fieldPokemon[i].bProps.ACCStage = 0;
                                    }
                                    if (statsToModify.Contains(PokemonStats.Evasion))
                                    {
                                        fieldPokemon[i].bProps.EVAStage = 0;
                                    }
                                }

                                string textID = "move-haze-team";
                                MoveEffect hazeText = moveData.GetEffect(MoveEffectType.HazeText);
                                if (hazeText != null)
                                {
                                    textID = hazeText.GetString(0);
                                }
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = textID,
                                    pokemonUserID = userPokemon.uniqueID,
                                    teamID = targetTeam.teamID,
                                    moveID = moveData.ID
                                });
                            }
                        }

                        effectSuccess = true;
                    }
                    // Reflect
                    else if (effect.effectType == MoveEffectType.Reflect)
                    {
                        bool canSetup = true;

                        // Fails if exists already
                        if (canSetup)
                        {
                            if (battle.GetTeamReflectScreen(targetTeam, moveData.ID) != null)
                            {
                                canSetup = false;
                            }
                        }

                        if (apply && canSetup)
                        {
                            int turns = Mathf.FloorToInt(effect.GetFloat(0));
                            BattleTeamProperties.ReflectScreen reflectScreen = new BattleTeamProperties.ReflectScreen(
                                moveID: moveData.ID,
                                turnsLeft: turns
                                );
                            battle.AddTeamReflectScreen(targetTeam, reflectScreen);

                            string textID = effect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-reflect-default" : textID;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                teamID = targetTeam.teamID,
                                moveID = moveData.ID
                            });
                        }
                        else if (!canSetup && forceEffectDisplay)
                        {
                            string textID = effect.GetString(1);
                            textID = (textID == "DEFAULT") ? "move-reflect-fail" : textID;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                teamID = targetTeam.teamID,
                                moveID = moveData.ID
                            });
                        }
                        effectSuccess = canSetup;
                    }
                    // Safeguard
                    else if (effect.effectType == MoveEffectType.Safeguard)
                    {
                        bool canSetup = true;

                        // Fails if exists already
                        if (canSetup)
                        {
                            if (battle.GetTeamSafeguard(targetTeam, moveData.ID) != null)
                            {
                                canSetup = false;
                            }
                        }

                        if (apply && canSetup)
                        {
                            int turns = Mathf.FloorToInt(effect.GetFloat(0));
                            BattleTeamProperties.Safeguard safeguard = new BattleTeamProperties.Safeguard(
                                moveID: moveData.ID,
                                turnsLeft: turns
                                );
                            battle.AddTeamSafeguard(targetTeam, safeguard);

                            string textID = effect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-safeguard-default" : textID;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                teamID = targetTeam.teamID,
                                moveID = moveData.ID
                            });
                        }
                        else if (!canSetup && forceEffectDisplay)
                        {
                            string textID = effect.GetString(1);
                            textID = (textID == "DEFAULT") ? "move-reflect-fail" : textID;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                teamID = targetTeam.teamID,
                                moveID = moveData.ID
                            });
                        }
                        effectSuccess = canSetup;
                    }
                    // Team Status Conditions
                    else if (effect.effectType == MoveEffectType.InflictTeamSC)
                    {
                        string statusID = effect.GetString(0);
                        StatusTEData statusData =
                            StatusTEDatabase.instance.GetStatusData(statusID);

                        // turn calculation
                        int turnCount = Mathf.FloorToInt(effect.GetFloat(0));
                        // Use Default Turns Left
                        if (effect.GetBool(0))
                        {
                            TeamCEff turnEffect = statusData.GetEffect(TeamSEType.DefaultTurnsLeft);
                            if (turnEffect != null)
                            {
                                turnCount = Mathf.FloorToInt(turnEffect.GetFloat(0));
                                if (turnEffect.GetBool(0))
                                {
                                    int minTurns = Mathf.FloorToInt(turnEffect.GetFloat(0));
                                    int maxTurns = Mathf.FloorToInt(turnEffect.GetFloat(1));
                                    turnCount = Random.Range(minTurns, maxTurns);
                                }
                            }
                        }
                        // Use the turn range
                        else if (effect.GetBool(1))
                        {
                            int minTurns = Mathf.FloorToInt(effect.GetFloat(0));
                            int maxTurns = Mathf.FloorToInt(effect.GetFloat(1));
                            turnCount = Random.Range(minTurns, maxTurns);
                        }

                        // try to apply the team status condition
                        yield return StartCoroutine(TryToInflictTeamSC(
                            statusID: statusID,
                            targetTeam: targetTeam,
                            turnsLeft: turnCount,
                            userPokemon: userPokemon,
                            inflictingMove: moveData,
                            forceFailureMessage: forceEffectDisplay,
                            apply: apply,
                            callback: (result) =>
                            {
                                effectSuccess = result;
                            }
                            ));
                    }
                }

                // battle effects

                // Battle Status
                if (effect.effectType == MoveEffectType.InflictBattleSC)
                {
                    string statusID = effect.GetString(0);
                    StatusBTLData statusData =
                        StatusBTLDatabase.instance.GetStatusData(statusID);

                    // turn calculation
                    int turnCount = Mathf.FloorToInt(effect.GetFloat(0));
                
                    // Use Default Turns Left
                    if (effect.GetBool(0))
                    {

                    }
                    // Use the turn range
                    else if (effect.GetBool(1))
                    {
                        int minTurns = Mathf.FloorToInt(effect.GetFloat(0));
                        int maxTurns = Mathf.FloorToInt(effect.GetFloat(1));
                        turnCount = Random.Range(minTurns, maxTurns);
                    }

                    // try to apply the team status condition
                    yield return StartCoroutine(TryToInflictBattleSC(
                        statusID: statusID,
                        turnsLeft: turnCount,
                        userPokemon: userPokemon,
                        inflictingMove: moveData,
                        forceFailureMessage: forceEffectDisplay,
                        apply: apply,
                        callback: (result) =>
                        {
                            effectSuccess = result;
                        }
                        ));
                }
                // Haze
                else if (effect.effectType == MoveEffectType.Haze 
                    && effect.effectTargetType == MoveEffectTargetType.Battlefield)
                {
                    if (apply)
                    {
                        List<Pokemon> fieldPokemon = new List<Pokemon>(battle.pokemonOnField);
                        if (fieldPokemon.Count > 0)
                        {
                            List<PokemonStats> statsToModify = GameTextDatabase.GetStatsFromList(effect.stringParams);
                            for (int i = 0; i < fieldPokemon.Count; i++)
                            {
                                if (statsToModify.Contains(PokemonStats.Attack))
                                {
                                    fieldPokemon[i].bProps.ATKStage = 0;
                                }
                                if (statsToModify.Contains(PokemonStats.Defense))
                                {
                                    fieldPokemon[i].bProps.DEFStage = 0;
                                }
                                if (statsToModify.Contains(PokemonStats.SpecialAttack))
                                {
                                    fieldPokemon[i].bProps.SPAStage = 0;
                                }
                                if (statsToModify.Contains(PokemonStats.SpecialDefense))
                                {
                                    fieldPokemon[i].bProps.SPDStage = 0;
                                }
                                if (statsToModify.Contains(PokemonStats.Speed))
                                {
                                    fieldPokemon[i].bProps.SPEStage = 0;
                                }
                                if (statsToModify.Contains(PokemonStats.Accuracy))
                                {
                                    fieldPokemon[i].bProps.ACCStage = 0;
                                }
                                if (statsToModify.Contains(PokemonStats.Evasion))
                                {
                                    fieldPokemon[i].bProps.EVAStage = 0;
                                }
                            }

                            string textID = "move-haze";
                            MoveEffect hazeText = moveData.GetEffect(MoveEffectType.HazeText);
                            if (hazeText != null)
                            {
                                textID = hazeText.GetString(0);
                            }
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = userPokemon.uniqueID,
                                moveID = moveData.ID
                            });
                        }
                    }

                    effectSuccess = true;
                }
                // Pay Day
                else if (effect.effectType == MoveEffectType.PayDay)
                {
                    if (apply)
                    {
                        int paydayAmount = Mathf.FloorToInt(userPokemon.level * effect.GetFloat(0));
                        Trainer paydayTrainer = battle.GetPokemonOwner(userPokemon);
                        paydayTrainer.bProps.payDayMoney += paydayAmount;
                        SendEvent(new Battle.View.Events.Message
                        {
                            message = "Coins scattered everywhere! (" + paydayAmount + ")"
                        });
                    }
                    effectSuccess = true;
                }
        
            }

            callback(effectSuccess);
            yield return null;
        }
    
        public IEnumerator ExecuteMoveEffectsByTiming(
            Pokemon userPokemon,
            List<BattleHitTarget> battleHitTargets,
            List<BattleTeam> targetTeams,
            MoveData moveData,
            System.Action<bool> callback,
            MoveEffectTiming timing = MoveEffectTiming.Unique,
            bool bypassChecks = false,
            bool apply = true
            )
        {
            List<EffectDatabase.MoveEff.MoveEffect> effects = moveData.GetEffectsNewFiltered(timing);
            bool effectSuccess = false;

            yield return StartCoroutine(ExecuteMoveEffects(
                userPokemon: userPokemon,
                moveData: moveData,
                effects: effects,
                battleHitTargets: battleHitTargets,
                targetTeams: targetTeams,
                bypassChecks: bypassChecks,
                apply: apply,
                callback: (result) =>
                {
                    if (result)
                    {
                        effectSuccess = true;
                    }
                }
                ));
            callback(effectSuccess);
        }

        public IEnumerator ExecuteMoveEffects(
            Pokemon userPokemon,
            MoveData moveData,
            List<EffectDatabase.MoveEff.MoveEffect> effects,
            List<BattleHitTarget> battleHitTargets,
            List<BattleTeam> targetTeams,
            System.Action<bool> callback,
            bool bypassChecks = false,
            bool apply = true
            )
        {
            bool effectSuccess = false;
            BattleTeam userTeam = battle.GetTeam(userPokemon);

            // Filter out affected targets
            for (int i = 0; i < effects.Count; i++)
            {
                EffectDatabase.MoveEff.MoveEffect effect = effects[i];
                bool bypassCheck = false;

                // For each hit target, run for target, user, user's team and/or battlefield
                if (effect.occurrence == MoveEffectOccurrence.OnceForEachTarget)
                {
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        BattleTeam currentTeam = battle.GetTeam(currentTarget.pokemon);

                        Pokemon targetPokemon = currentTarget.pokemon;
                        if (effect.targetType == MoveEffectTargetType.Self)
                        {
                            targetPokemon = userPokemon;
                        }
                        if (effect.targetType == MoveEffectTargetType.Self
                            || effect.targetType == MoveEffectTargetType.SelfTeam)
                        {
                            currentTeam = userTeam;
                        }
                        if (effect.targetType == MoveEffectTargetType.Battlefield)
                        {
                            targetPokemon = null;
                            currentTeam = null;
                        }

                        if (currentTarget.affectedByMove || bypassChecks)
                        {
                            yield return StartCoroutine(ExecuteMoveEffect(
                                effect_: effect,
                                moveData: moveData,
                                userPokemon: userPokemon,
                                targetPokemon: targetPokemon,
                                targetTeam: currentTeam,
                                bypassChanceCheck: bypassCheck,
                                apply: apply,
                                callback: (success) =>
                                {
                                    if (success)
                                    {
                                        effectSuccess = true;
                                    }
                                }
                                ));
                        }
                    }
                }

                // For each hit team, run for user, user's team and/or battlefield
                else if (effect.occurrence == MoveEffectOccurrence.OnceForEachTeam)
                {
                    // Run once for each affected team
                    for (int k = 0; k < targetTeams.Count; k++)
                    {
                        BattleTeam targetTeam = targetTeams[k];

                        Pokemon targetPokemon = null;
                        if (effect.targetType == MoveEffectTargetType.Self)
                        {
                            targetPokemon = userPokemon;
                            targetTeam = userTeam;
                        }
                        if (effect.targetType == MoveEffectTargetType.SelfTeam)
                        {
                            targetTeam = userTeam;
                        }
                        if (effect.targetType == MoveEffectTargetType.Battlefield)
                        {
                            targetTeam = null;
                        }

                        yield return StartCoroutine(ExecuteMoveEffect(
                            effect_: effect,
                            moveData: moveData,
                            userPokemon: userPokemon,
                            targetPokemon: targetPokemon,
                            targetTeam: targetTeam,
                            bypassChanceCheck: bypassCheck,
                            apply: apply,
                            callback: (success) =>
                            {
                                if (success)
                                {
                                    effectSuccess = true;
                                }
                            }
                            ));
                    }
                }

                // Run once for user, user's team and/or battlefield
                else if (effect.occurrence == MoveEffectOccurrence.Once)
                {
                    Pokemon targetPokemon = null;
                    BattleTeam targetTeam = null;
                    if (effect.targetType == MoveEffectTargetType.Self)
                    {
                        targetPokemon = userPokemon;
                    }
                    if (effect.targetType == MoveEffectTargetType.Self
                        || effect.targetType == MoveEffectTargetType.SelfTeam)
                    {
                        targetTeam = userTeam;
                    }

                    yield return StartCoroutine(ExecuteMoveEffect(
                        effect_: effect,
                        moveData: moveData,
                        userPokemon: userPokemon,
                        targetPokemon: targetPokemon,
                        targetTeam: targetTeam,
                        bypassChanceCheck: bypassCheck,
                        apply: apply,
                        callback: (success) =>
                        {
                            if (success)
                            {
                                effectSuccess = true;
                            }
                        }
                        ));
                }

            }

            callback(effectSuccess);
        }

        public IEnumerator ExecuteMoveEffect(
            EffectDatabase.MoveEff.MoveEffect effect_,
            MoveData moveData,
            Pokemon userPokemon,
            System.Action<bool> callback,
            Pokemon targetPokemon = null,
            BattleTeam targetTeam = null,
            bool bypassChanceCheck = false,
            bool apply = true
            )
        {
            bool effectSuccess = false;
            if (bypassChanceCheck ||
                battle.DoesMoveEffectPassChecks(
                    effect: effect_,
                    moveData: moveData,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    targetTeam: targetTeam
                    ))
            {
                // Core Enforcer
                if (effect_ is EffectDatabase.MoveEff.CoreEnforcer && targetPokemon != null)
                {
                    EffectDatabase.MoveEff.CoreEnforcer coreEnforcer = effect_ as EffectDatabase.MoveEff.CoreEnforcer;
                    if (battle.IsPokemonOnFieldAndAble(targetPokemon))
                    {
                        List<Pokemon.Ability> abilities = battle.PBPGetAbilities(
                            pokemon: targetPokemon,
                            ignoreSuppression: true
                            );

                        for (int k = 0; k < abilities.Count; k++)
                        {
                            if (!abilities[k].data.HasTag(AbilityTag.CannotSuppress))
                            {
                                abilities[k].isSuppressed = true;
                                effectSuccess = true;
                                PBPShowAbility(targetPokemon, abilities[k].data);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = coreEnforcer.displayText,
                                    pokemonTargetID = targetPokemon.uniqueID,
                                    abilityID = abilities[k].abilityID
                                });
                            }
                            else
                            {
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = coreEnforcer.failText,
                                    pokemonTargetID = targetPokemon.uniqueID,
                                    abilityID = abilities[k].abilityID
                                });
                            }
                        }
                    }
                }
                // Corrosive Gas
                if (effect_ is EffectDatabase.MoveEff.CorrosiveGas && targetPokemon != null)
                {
                    EffectDatabase.MoveEff.CorrosiveGas effect = effect_ as EffectDatabase.MoveEff.CorrosiveGas;
                    Item targetItem = battle.PBPGetHeldItem(targetPokemon);
                    if (targetItem != null)
                    {
                        if (battle.CanPokemonItemBeLost(targetPokemon, targetItem)
                            && targetItem.useable)
                        {
                            effectSuccess = true;
                            if (apply)
                            {
                                targetItem.useable = false;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.displayText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = targetPokemon.uniqueID,
                                    itemID = targetItem.itemID
                                });
                            }
                        }
                    }
                }
                // Covet
                if (effect_ is EffectDatabase.MoveEff.Covet && targetPokemon != null)
                {
                    EffectDatabase.MoveEff.Covet effect = effect_ as EffectDatabase.MoveEff.Covet;
                    Item userItem = battle.PBPGetHeldItem(userPokemon);
                    Item targetItem = battle.PBPGetHeldItem(targetPokemon);
                    if (targetItem != null
                        && userItem == null
                        && battle.IsPokemonOnFieldAndAble(userPokemon))
                    {
                        if (battle.CanPokemonItemBeLost(targetPokemon, targetItem))
                        {
                            effectSuccess = true;
                            if (apply)
                            {
                                targetPokemon.UnsetItem(targetItem);
                                userPokemon.SetItem(targetItem);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.displayText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = targetPokemon.uniqueID,
                                    itemID = targetItem.itemID
                                });
                            }
                        }
                    }
                }
                // Endure
                else if (effect_ is EffectDatabase.MoveEff.Endure)
                {
                    effectSuccess = true;
                    userPokemon.bProps.protectCounter++;

                    EffectDatabase.MoveEff.Endure effect = effect_ as EffectDatabase.MoveEff.Endure;
                    if (effect.targetType == MoveEffectTargetType.Self
                        || effect.targetType == MoveEffectTargetType.Target)
                    {
                        targetPokemon.bProps.endure = effect.Clone();
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = effect.displayText,
                            pokemonUserID = userPokemon.uniqueID,
                            pokemonTargetID = targetPokemon.uniqueID
                        });
                    }
                }
                // Feint
                else if (effect_ is EffectDatabase.MoveEff.Feint)
                {
                    EffectDatabase.MoveEff.Feint effect = effect_ as EffectDatabase.MoveEff.Feint;

                    // Lifting Protection moves
                    if (targetPokemon.bProps.protect != null)
                    {
                        if (!targetPokemon.bProps.protect.maxGuard || effect.liftMaxGuard)
                        {
                            effectSuccess = true;
                            if (apply)
                            {
                                targetPokemon.bProps.protect = null;
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.displayText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }
                    }

                    // Lifting Mat Block moves
                    if (targetTeam != null)
                    {
                        if (targetTeam.bProps.matBlocks.Count > 0)
                        {
                            List<EffectDatabase.General.Protect> matBlocks = 
                                new List<EffectDatabase.General.Protect>(targetTeam.bProps.matBlocks);

                            bool protectionLifted = false;
                            for (int i = 0; i < matBlocks.Count; i++)
                            {
                                if (!matBlocks[i].maxGuard || effect.liftMaxGuard)
                                {
                                    effectSuccess = true;
                                    protectionLifted = true;
                                    if (apply)
                                    {
                                        targetTeam.bProps.matBlocks.Remove(matBlocks[i]);
                                    }
                                }
                            }

                            if (protectionLifted && apply)
                            {
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.displayTextMatBlock,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = targetPokemon.uniqueID,
                                    teamID = targetTeam.teamID
                                });
                            }
                        }
                    }
                }
                // Inflict Status
                else if (effect_ is EffectDatabase.MoveEff.InflictStatus)
                {
                    EffectDatabase.MoveEff.InflictStatus effect = effect_ as EffectDatabase.MoveEff.InflictStatus;
                    EffectDatabase.General.InflictStatus inflictEffect = effect.inflictStatus;
                    yield return StartCoroutine(ApplySC(
                        inflictStatus: inflictEffect,
                        targetPokemon: targetPokemon,
                        targetTeam: targetTeam,
                        userPokemon: userPokemon,
                        moveData: moveData,
                        forceFailMessage: effect.forceEffectDisplay,
                        apply: apply,
                        callback: (result) => 
                        {
                            if (result)
                            {
                                effectSuccess = true;
                            }
                        }
                        ));
                }
                // Knock Off
                else if (effect_ is EffectDatabase.MoveEff.KnockOff && targetPokemon != null)
                {
                    EffectDatabase.MoveEff.KnockOff effect = effect_ as EffectDatabase.MoveEff.KnockOff;
                    Item targetItem = battle.PBPGetHeldItem(targetPokemon);
                    if (targetItem != null)
                    {
                        if (battle.CanPokemonItemBeLost(targetPokemon, targetItem))
                        {
                            effectSuccess = true;
                            if (apply)
                            {
                                targetPokemon.UnsetItem(targetItem);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.displayText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = targetPokemon.uniqueID,
                                    itemID = targetItem.itemID
                                });
                            }
                        }
                    }
                }
                // Protect / Mat Block
                else if (effect_ is EffectDatabase.MoveEff.Protect)
                {
                    effectSuccess = true;
                    userPokemon.bProps.protectCounter++;

                    EffectDatabase.MoveEff.Protect effect = effect_ as EffectDatabase.MoveEff.Protect;
                    if (effect.targetType == MoveEffectTargetType.Self
                        || effect.targetType == MoveEffectTargetType.Target)
                    {
                        targetPokemon.bProps.protect = effect.protect.Clone();
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = effect.protect.displayText,
                            pokemonUserID = userPokemon.uniqueID,
                            pokemonTargetID = targetPokemon.uniqueID,
                            moveID = moveData.ID
                        });
                    }
                    else if (effect.targetType == MoveEffectTargetType.SelfTeam
                        || effect.targetType == MoveEffectTargetType.Team)
                    {
                        targetTeam.bProps.matBlocks.Add(effect.protect.Clone());
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = effect.protect.displayText,
                            pokemonUserID = userPokemon.uniqueID,
                            teamID = targetTeam.teamID,
                            moveID = moveData.ID
                        });
                    }
                }
                // Refresh
                else if (effect_ is EffectDatabase.MoveEff.Refresh)
                {
                    EffectDatabase.MoveEff.Refresh refresh = effect_ as EffectDatabase.MoveEff.Refresh;
                    List<Pokemon> appliedPokemon = new List<Pokemon>();
                
                    if (refresh.targetType == MoveEffectTargetType.Target
                        || refresh.targetType == MoveEffectTargetType.Self)
                    {
                        appliedPokemon.Add(targetPokemon);
                    }
                    else if (refresh.targetType == MoveEffectTargetType.Team
                        || refresh.targetType == MoveEffectTargetType.SelfTeam)
                    {
                        appliedPokemon.AddRange(battle.GetTeamPokemonOnField(targetTeam));
                    }
                    else if (refresh.targetType == MoveEffectTargetType.Battlefield)
                    {
                        appliedPokemon.AddRange(battle.pokemonOnField);
                    }

                    for (int i = 0; i < appliedPokemon.Count; i++)
                    {
                        Pokemon curPokemon = appliedPokemon[i];
                        if (battle.IsPokemonOnFieldAndAble(curPokemon))
                        {
                            List<StatusCondition> sConds = battle.PBPGetSCs(curPokemon);
                            bool healedAnyStatus = false;
                            for (int k = 0; k < sConds.Count; k++)
                            {
                                bool canHeal = false;

                                // Specific statuses
                                for (int j = 0; j < refresh.statuses.Count && !canHeal; j++)
                                {
                                    if (sConds[k].data.ID == refresh.statuses[j]
                                        || sConds[k].data.IsABaseID(refresh.statuses[j]))
                                    {
                                        canHeal = true;
                                    }
                                }

                                // Status Effect Types
                                List<PokemonSEType> statusEffectTypes = new List<PokemonSEType>(refresh.statusEffectTypes);
                                for (int j = 0; j < statusEffectTypes.Count && !canHeal; j++)
                                {
                                    if (sConds[k].data.GetEffectNew(statusEffectTypes[j]) != null)
                                    {
                                        canHeal = true;
                                    }
                                }

                                // Don't heal default statuses
                                if (sConds[i].data.HasTag(PokemonSTag.IsDefault))
                                {
                                    canHeal = false;
                                }

                                // Heal Status
                                if (canHeal)
                                {
                                    healedAnyStatus = true;
                                    if (apply)
                                    {
                                        yield return StartCoroutine(HealPokemonSC(
                                            targetPokemon: curPokemon,
                                            healerPokemon: userPokemon,
                                            condition: sConds[k]
                                            ));
                                    }
                                }
                            }

                            // Failure message
                            if (!healedAnyStatus 
                                && refresh.forceEffectDisplay)
                            {
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = refresh.failText,
                                    pokemonTargetID = curPokemon.uniqueID,
                                    moveID = moveData.ID
                                });
                            }
                        }
                    }
                }
                // Secret Power
                else if (effect_ is EffectDatabase.MoveEff.SecretPower)
                {
                    // Secret Power
                    EffectDatabase.MoveEff.SecretPower effect = effect_ as EffectDatabase.MoveEff.SecretPower;
                    if (effect != null)
                    {
                        List<EffectDatabase.MoveEff.MoveEffect> secretPowerEffects =
                            battle.GetPokemonSecretPowerEffects(userPokemon, moveData, effect);
                        for (int i = 0; i < secretPowerEffects.Count; i++)
                        {
                            if (effect.targetType == MoveEffectTargetType.Target 
                                || effect.targetType == MoveEffectTargetType.Self)
                            {
                                yield return StartCoroutine(ExecuteMoveEffect(
                                    effect_: secretPowerEffects[i],
                                    moveData: moveData,
                                    userPokemon: userPokemon,
                                    targetPokemon: targetPokemon,
                                    targetTeam: targetTeam,
                                    bypassChanceCheck: bypassChanceCheck,
                                    apply: apply,
                                    callback: (result) =>
                                    {
                                        if (result)
                                        {
                                            effectSuccess = true;
                                        }
                                    }
                                    ));
                            }
                        }
                    }
                }
                // Stat Stage Modification
                else if (effect_ is EffectDatabase.MoveEff.StatStageMod)
                {
                    EffectDatabase.MoveEff.StatStageMod effect = effect_ as EffectDatabase.MoveEff.StatStageMod;

                    if (effect.targetType == MoveEffectTargetType.Target
                        || effect.targetType == MoveEffectTargetType.Self)
                    {
                        yield return StartCoroutine(ApplyStatStageMod(
                            userPokemon: userPokemon,
                            targetPokemon: targetPokemon,
                            statStageMod: effect.statStageMod,
                            moveData: moveData,
                            forceFailureMessage: effect.forceEffectDisplay,
                            apply: apply,
                            callback: (result) =>
                            {
                                if (result)
                                {
                                    effectSuccess = true;
                                }
                            }));
                    }
                    else if (effect.targetType == MoveEffectTargetType.Team
                        || effect.targetType == MoveEffectTargetType.SelfTeam)
                    {
                        List<Pokemon> teamPokemon = battle.GetTeamPokemonOnField(targetTeam);
                        for (int i = 0; i < teamPokemon.Count; i++)
                        {
                            yield return StartCoroutine(ApplyStatStageMod(
                                userPokemon: userPokemon,
                                targetPokemon: teamPokemon[i],
                                statStageMod: effect.statStageMod,
                                moveData: moveData,
                                forceFailureMessage: effect.forceEffectDisplay,
                                apply: apply,
                                callback: (result) =>
                                {
                                    if (result)
                                    {
                                        effectSuccess = true;
                                    }
                                }));
                        }
                    }
                }
                // Steel Roller
                else if (effect_ is EffectDatabase.MoveEff.SteelRoller)
                {
                    EffectDatabase.MoveEff.SteelRoller effect = effect_ as EffectDatabase.MoveEff.SteelRoller;
                    if (!battle.terrain.data.HasTag(BattleSTag.Default))
                    {
                        yield return StartCoroutine(HealBattleSC(
                            condition: battle.terrain,
                            healerPokemon: userPokemon,
                            overwriteText: effect.displayText
                            ));
                    }
                }
            }

            callback(effectSuccess);
        }

        public IEnumerator ApplyStatStageMod(
            Pokemon targetPokemon,
            EffectDatabase.General.StatStageMod statStageMod,
            System.Action<bool> callback,
            Pokemon userPokemon = null,
            MoveData moveData = null,
            bool forceFailureMessage = false,
            bool isMirrorArmor = false,
            bool apply = true
            )
        {
            bool success = false;
            bool isSelf = (userPokemon != null) ? userPokemon.IsTheSameAs(targetPokemon) : false;
            bool isAlly = (userPokemon == null) ? false
                : (userPokemon == targetPokemon) ? false
                : battle.ArePokemonAllies(userPokemon, targetPokemon);
            bool isOpponent = (userPokemon == null) ? false
                : battle.ArePokemonEnemies(userPokemon, targetPokemon);
        
            if (battle.IsPokemonOnFieldAndAble(targetPokemon))
            {
                bool bypassAbility = false;
                // Mold Breaker bypasses ability immunities
                if (!bypassAbility && userPokemon != null)
                {
                    EffectDatabase.AbilityEff.AbilityEffect moldBreakerEffect =
                        battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                    if (moldBreakerEffect != null)
                    {
                        bypassAbility = true;
                    }
                    if (moveData != null)
                    {
                        // Sunsteel Strike bypasses ability immunities
                        EffectDatabase.MoveEff.MoveEffect effect = moveData.GetEffectNew(MoveEffectType.SunteelStrike);
                        if (effect != null)
                        {
                            bypassAbility = true;
                        }
                    }
                }

                List<AbilityData> abilities = battle.PBPGetAbilityDatas(targetPokemon, bypassAbility);
                List<string> defiantAbilities = new List<string>();
                List<string> mirrorArmorAbilities = new List<string>();

                EffectDatabase.General.StatStageMod defiantStatMod =
                    new EffectDatabase.General.StatStageMod();
                EffectDatabase.General.StatStageMod mirrorArmorStatMod =
                        new EffectDatabase.General.StatStageMod();

                bool canModify = true;

                Dictionary<PokemonStats, int> statStageMap = statStageMod.GetStatStageMap();
                List<PokemonStats> allStats = new List<PokemonStats>(GameSettings.btlPkmnStats);

                if (canModify)
                {
                    List<PokemonStats> alreadyMaxStats = new List<PokemonStats>();
                    List<PokemonStats> alreadyMinStats = new List<PokemonStats>();
                    List<PokemonStats> maximizedStats = new List<PokemonStats>();
                    List<PokemonStats> minimizedStats = new List<PokemonStats>();
                    Dictionary<int, List<PokemonStats>> realStatModMap = new Dictionary<int, List<PokemonStats>>();
                    Dictionary<AbilityData, List<PokemonStats>> hyperCutterStats = 
                        new Dictionary<AbilityData, List<PokemonStats>>();

                    for (int i = 0; i < allStats.Count; i++)
                    {
                        PokemonStats curStat = allStats[i];
                        int rawStatMod = statStageMap[curStat];
                        int curStatStage = battle.GetPokemonStatStage(targetPokemon, curStat);
                        bool keepStatChange = true;

                        // Mirror Armor
                        if (!isMirrorArmor && !isSelf)
                        {
                            for (int k = 0; k < abilities.Count && keepStatChange; k++)
                            {
                                EffectDatabase.AbilityEff.AbilityEffect mirrorArmor_ =
                                    abilities[k].GetEffectNew(AbilityEffectType.MirrorArmor);
                                if (mirrorArmor_ != null)
                                {
                                    EffectDatabase.AbilityEff.MirrorArmor mirrorArmor =
                                        mirrorArmor_ as EffectDatabase.AbilityEff.MirrorArmor;

                                    bool statReflected = false;
                                    if (mirrorArmor.lowerTriggers.Contains(curStat))
                                    {
                                        statReflected = true;
                                    }
                                    if (mirrorArmor.raiseTriggers.Contains(curStat))
                                    {
                                        statReflected = true;
                                    }
                                    if (mirrorArmor.onlyOpposing && !isOpponent)
                                    {
                                        statReflected = false;
                                    }

                                    if (statReflected)
                                    {
                                        keepStatChange = false;
                                        if (!mirrorArmorAbilities.Contains(abilities[k].ID))
                                        {
                                            mirrorArmorAbilities.Add(abilities[k].ID);
                                        }

                                        mirrorArmorStatMod.AddStatMod(
                                            statType: curStat,
                                            maxVal: rawStatMod >= GameSettings.btlMaxStatBoost,
                                            minVal: rawStatMod <= GameSettings.btlMinStatBoost,
                                            statVal: rawStatMod
                                            );
                                    }
                                }
                            }
                        }

                        // Contrary
                        for (int k = 0; k < abilities.Count && keepStatChange; k++)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect contrary_ =
                                abilities[k].GetEffectNew(AbilityEffectType.Contrary);
                            if (contrary_ != null)
                            {
                                EffectDatabase.AbilityEff.Contrary contrary =
                                    contrary_ as EffectDatabase.AbilityEff.Contrary;
                                rawStatMod = -rawStatMod;
                            }
                        }

                        // Simple
                        for (int k = 0; k < abilities.Count && keepStatChange; k++)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect simple_ = 
                                abilities[k].GetEffectNew(AbilityEffectType.Simple);
                            if (simple_ != null)
                            {
                                EffectDatabase.AbilityEff.Simple simple =
                                    simple_ as EffectDatabase.AbilityEff.Simple;
                                rawStatMod *= simple.statModScale;
                            }
                        }

                        // Clear Body / Hyper Cutter
                        for (int k = 0; k < abilities.Count && keepStatChange; k++)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect hyperCutter_ =
                                abilities[k].GetEffectNew(AbilityEffectType.HyperCutter);
                            if (hyperCutter_ != null)
                            {
                                EffectDatabase.AbilityEff.HyperCutter hyperCutter =
                                    hyperCutter_ as EffectDatabase.AbilityEff.HyperCutter;

                                if (hyperCutter.affectedStats.Contains(curStat) || hyperCutter.clearBody)
                                {
                                    bool statChangeBlocked = true;

                                    // Self-Change
                                    if (statChangeBlocked
                                        && hyperCutter.affectSelf
                                        && !isSelf)
                                    {
                                        statChangeBlocked = false;
                                    }

                                    // Prevent Lowering
                                    if (statChangeBlocked
                                        && hyperCutter.preventLower
                                        && rawStatMod < 0)
                                    {
                                        statChangeBlocked = false;
                                    }

                                    // Preventing Raising
                                    if (statChangeBlocked
                                        && hyperCutter.preventRaise
                                        && rawStatMod > 0)
                                    {
                                        statChangeBlocked = false;
                                    }

                                    if (statChangeBlocked)
                                    {
                                        keepStatChange = false;

                                        if (!hyperCutterStats.ContainsKey(abilities[k]))
                                        {
                                            hyperCutterStats.Add(abilities[k], new List<PokemonStats>());
                                        }
                                        hyperCutterStats[abilities[k]].Add(curStat);
                                    }
                                }
                            }
                        }

                        int statMod = rawStatMod;
                        if (statMod != 0 && keepStatChange)
                        {
                            // Maximized already
                            if (statMod > 0 && curStatStage == GameSettings.btlStatStageMax)
                            {
                                alreadyMaxStats.Add(curStat);
                            }
                            // Minimized already
                            else if (statMod < 0 && curStatStage == GameSettings.btlStatStageMin)
                            {
                                alreadyMinStats.Add(curStat);
                            }
                            // Stat can be modified
                            else
                            {
                                success = true;
                                if (statMod >= GameSettings.btlMaxStatBoost)
                                {
                                    maximizedStats.Add(curStat);
                                    if (apply)
                                    {
                                        battle.SetStatStage(targetPokemon, curStat, GameSettings.btlStatStageMax);
                                    }
                                }
                                else if (statMod <= GameSettings.btlMinStatBoost)
                                {
                                    minimizedStats.Add(curStat);
                                    if (apply)
                                    {
                                        battle.SetStatStage(targetPokemon, curStat, GameSettings.btlStatStageMin);
                                    }
                                }
                                else
                                {
                                    int afterStatStage = curStatStage + statMod;
                                    int realStatMod = (afterStatStage > GameSettings.btlStatStageMax)
                                        ? GameSettings.btlStatStageMax - curStatStage
                                        : (afterStatStage < GameSettings.btlStatStageMin)
                                        ? GameSettings.btlStatStageMin - curStatStage
                                        : afterStatStage - curStatStage;

                                    if (apply)
                                    {
                                        battle.SetStatStage(targetPokemon, curStat, curStatStage + realStatMod);
                                    }
                                    if (realStatModMap.ContainsKey(realStatMod))
                                    {
                                        realStatModMap[realStatMod].Add(curStat);
                                    }
                                    else
                                    {
                                        realStatModMap[realStatMod] = new List<PokemonStats> { curStat };
                                    }
                                }

                                // Defiant
                                for (int k = 0; k < abilities.Count; k++)
                                {
                                    EffectDatabase.AbilityEff.AbilityEffect defiant_ =
                                        abilities[k].GetEffectNew(AbilityEffectType.Defiant);
                                    if (defiant_ != null)
                                    {
                                        EffectDatabase.AbilityEff.Defiant defiant =
                                            defiant_ as EffectDatabase.AbilityEff.Defiant;
                                        bool defiantTriggered = false;
                                        if (statMod < 0 && defiant.lowerTriggers.Contains(curStat))
                                        {
                                            defiantTriggered = true;
                                        }
                                        if (statMod > 0 && defiant.raiseTriggers.Contains(curStat))
                                        {
                                            defiantTriggered = true;
                                        }
                                        if (defiant.onlyOpposing && !isOpponent)
                                        {
                                            defiantTriggered = false;
                                        }

                                        if (defiantTriggered)
                                        {
                                            if (!defiantAbilities.Contains(abilities[k].ID))
                                            {
                                                defiantAbilities.Add(abilities[k].ID);
                                            }
                                        
                                            if (defiant.statStageMod != null)
                                            {
                                                defiantStatMod.AddOther(defiant.statStageMod);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!success && forceFailureMessage)
                    {
                        if (alreadyMinStats.Count > 0)
                        {
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = "stats-min",
                                pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                                pokemonTargetID = targetPokemon.uniqueID,
                                statList = new List<PokemonStats>(alreadyMinStats)
                            });
                        }
                        if (alreadyMaxStats.Count > 0)
                        {
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = "stats-max",
                                pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                                pokemonTargetID = targetPokemon.uniqueID,
                                statList = new List<PokemonStats>(alreadyMaxStats)
                            });
                        }
                    }
                    if (hyperCutterStats.Count > 0 && forceFailureMessage)
                    {
                        for (int i = 0; i < abilities.Count; i++)
                        {
                            if (hyperCutterStats.ContainsKey(abilities[i]))
                            {
                                EffectDatabase.AbilityEff.HyperCutter hyperCutter =
                                    abilities[i].GetEffectNew(AbilityEffectType.HyperCutter)
                                    as EffectDatabase.AbilityEff.HyperCutter;

                                PBPShowAbility(targetPokemon, abilities[i]);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = hyperCutter.displayText,
                                    pokemonTargetID = targetPokemon.uniqueID,
                                    statList = new List<PokemonStats>(hyperCutterStats[abilities[i]])
                                });
                            }
                        }
                    }
                    if (minimizedStats.Count > 0 && apply)
                    {
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = "stats-minimize",
                            pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                            pokemonTargetID = targetPokemon.uniqueID,
                            statList = new List<PokemonStats>(minimizedStats)
                        });
                    }
                    if (realStatModMap.Count > 0 && apply)
                    {
                        List<int> allRealStatMods = new List<int>(realStatModMap.Keys);

                        // Display stat changes from lowest to highest
                        allRealStatMods.Sort();
                        for (int i = 0; i < allRealStatMods.Count; i++)
                        {
                            int curMod = allRealStatMods[i];
                            string textID = (curMod == 1) ? "stats-up1"
                                : (curMod == 2) ? "stats-up2"
                                : (curMod >= 3) ? "stats-up3"
                                : (curMod == -1) ? "stats-down1"
                                : (curMod == -2) ? "stats-down2"
                                : (curMod <= -3) ? "stats-down3"
                                : null;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                                pokemonTargetID = targetPokemon.uniqueID,
                                statList = new List<PokemonStats>(realStatModMap[curMod])
                            });
                        }
                    }
                    if (maximizedStats.Count > 0 && apply)
                    {
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = "stats-maximize",
                            pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                            pokemonTargetID = targetPokemon.uniqueID,
                            statList = new List<PokemonStats>(maximizedStats)
                        });
                    }

                    // Defiant for any lowered stats
                    if (!defiantStatMod.IsNoChange() && apply)
                    {
                        for (int i = 0; i < defiantAbilities.Count; i++)
                        {
                            AbilityData abilityData = AbilityDatabase.instance.GetAbilityData(defiantAbilities[i]);
                            PBPShowAbility(targetPokemon, abilityData);
                        }
                        yield return StartCoroutine(ApplyStatStageMod(
                            targetPokemon: targetPokemon,
                            statStageMod: defiantStatMod,
                            forceFailureMessage: true,
                            callback: (result) => { }
                            ));
                    }

                    // Mirror Armor
                    if (!isMirrorArmor && !mirrorArmorStatMod.IsNoChange() && apply)
                    {
                        for (int i = 0; i < mirrorArmorAbilities.Count; i++)
                        {
                            AbilityData abilityData = AbilityDatabase.instance.GetAbilityData(mirrorArmorAbilities[i]);
                            PBPShowAbility(targetPokemon, abilityData);
                        }
                        Pokemon mirrorArmorTarget = userPokemon;
                        if (mirrorArmorTarget == null)
                        {
                            mirrorArmorTarget = battle.GetRandomOpponent(targetPokemon);
                        }
                        if (mirrorArmorTarget != null)
                        {
                            yield return StartCoroutine(ApplyStatStageMod(
                                targetPokemon: mirrorArmorTarget,
                                userPokemon: targetPokemon,
                                statStageMod: mirrorArmorStatMod,
                                forceFailureMessage: true,
                                isMirrorArmor: true,
                                callback: (result) => { }
                                ));
                        }
                    }
                }
            }

            callback(success);
            yield return null;
        }

        public IEnumerator ApplySC(
            EffectDatabase.General.InflictStatus inflictStatus,
            System.Action<bool> callback,
            Pokemon targetPokemon = null,
            BattleTeam targetTeam = null,
            Pokemon userPokemon = null,
            MoveData moveData = null,
            bool forceFailMessage = false,
            bool apply = true
            )
        {
            bool success = false;

            // Pokemon Status Condition
            if (inflictStatus.statusType == StatusType.Pokemon)
            {
                StatusPKData statusData =
                    StatusPKDatabase.instance.GetStatusData(inflictStatus.statusID);
                StatusPKData modStatusData = statusData.Clone();
            
                if (inflictStatus.effectMode == EffectDatabase.General.InflictStatus.EffectMode.Additive)
                {
                    modStatusData.AddEffects(inflictStatus.customPokemonEffects);
                }
                else if (inflictStatus.effectMode == EffectDatabase.General.InflictStatus.EffectMode.Replace)
                {
                    modStatusData.SetEffects(inflictStatus.customPokemonEffects);
                }

                // turn calculation
                int turnCount = inflictStatus.turns;

                // Use Default Turns Left
                if (inflictStatus.useDefaultTurns)
                {
                    if (statusData.defaultTurns != null)
                    {
                        turnCount = statusData.defaultTurns.GetTurns();
                    }
                }
                // Use the turn range
                else if (inflictStatus.useTurnRange)
                {
                    int minTurns = inflictStatus.lowestTurns;
                    int maxTurns = inflictStatus.highestTurns;
                    turnCount = Random.Range(minTurns, maxTurns);
                }

                // try to apply the status condition
                List<Pokemon> appliedPokemon = new List<Pokemon>();
                if (targetPokemon != null)
                {
                    appliedPokemon.Add(targetPokemon);
                }
                else if (targetTeam != null)
                {
                    appliedPokemon.AddRange(battle.GetTeamPokemonOnField(targetTeam));
                }
            
                for (int i = 0; i < appliedPokemon.Count; i++)
                {
                    yield return StartCoroutine(ApplyPokemonSC(
                    statusData: modStatusData,
                    targetPokemon: appliedPokemon[i],
                    turnsLeft: turnCount,
                    userPokemon: userPokemon,
                    moveData: moveData,
                    forceFailMessage: forceFailMessage,
                    apply: apply,
                    callback: (result) =>
                    {
                        success = result;
                    }
                    ));
                }
            }
            // Team Status Condition
            else if (inflictStatus.statusType == StatusType.Team && targetTeam != null)
            {
                StatusTEData statusData =
                    StatusTEDatabase.instance.GetStatusData(inflictStatus.statusID);
                StatusTEData modStatusData = statusData.Clone();
            
                if (inflictStatus.effectMode == EffectDatabase.General.InflictStatus.EffectMode.Additive)
                {
                    modStatusData.AddEffects(inflictStatus.customTeamEffects);
                }
                else if (inflictStatus.effectMode == EffectDatabase.General.InflictStatus.EffectMode.Replace)
                {
                    modStatusData.SetEffects(inflictStatus.customTeamEffects);
                }

                // turn calculation
                int turnCount = inflictStatus.turns;

                // Use Default Turns Left
                if (inflictStatus.useDefaultTurns)
                {
                    if (statusData.defaultTurns != null)
                    {
                        turnCount = statusData.defaultTurns.GetTurns();
                    }
                }
                // Use the turn range
                else if (inflictStatus.useTurnRange)
                {
                    int minTurns = inflictStatus.lowestTurns;
                    int maxTurns = inflictStatus.highestTurns;
                    turnCount = Random.Range(minTurns, maxTurns);
                }

                // try to apply the status condition
                yield return StartCoroutine(ApplyTeamSC(
                    statusData: modStatusData,
                    targetTeam: targetTeam,
                    turnsLeft: turnCount,
                    userPokemon: userPokemon,
                    moveData: moveData,
                    forceFailureMessage: forceFailMessage,
                    apply: apply,
                    callback: (result) =>
                    {
                        success = result;
                    }
                    ));
            }
            // Battle Status Condition
            else if (inflictStatus.statusType == StatusType.Battle)
            {
                StatusBTLData statusData =
                    StatusBTLDatabase.instance.GetStatusData(inflictStatus.statusID);
                StatusBTLData modStatusData = statusData.Clone();
            
                if (inflictStatus.effectMode == EffectDatabase.General.InflictStatus.EffectMode.Additive)
                {
                    modStatusData.AddEffects(inflictStatus.customBattleEffects);
                }
                else if (inflictStatus.effectMode == EffectDatabase.General.InflictStatus.EffectMode.Replace)
                {
                    modStatusData.SetEffects(inflictStatus.customBattleEffects);
                }

                // turn calculation
                int turnCount = inflictStatus.turns;

                // Use Default Turns Left
                if (inflictStatus.useDefaultTurns)
                {
                    if (statusData.defaultTurns != null)
                    {
                        turnCount = statusData.defaultTurns.GetTurns();
                    }
                }
                // Use the turn range
                else if (inflictStatus.useTurnRange)
                {
                    int minTurns = inflictStatus.lowestTurns;
                    int maxTurns = inflictStatus.highestTurns;
                    turnCount = Random.Range(minTurns, maxTurns);
                }

                // try to apply the status condition
                yield return StartCoroutine(ApplyBattleSC(
                    statusData: modStatusData,
                    turnsLeft: turnCount,
                    userPokemon: userPokemon,
                    moveData: moveData,
                    forceFailureMessage: forceFailMessage,
                    apply: apply,
                    callback: (result) =>
                    {
                        success = result;
                    }
                    ));
            }

            callback(success);
            yield return null;
        }

        // Pokemon Status Conditions
        public IEnumerator ApplyPokemonSC(
            StatusPKData statusData,
            Pokemon targetPokemon,
            System.Action<bool> callback,
            int turnsLeft = -1,
            Pokemon userPokemon = null,
            MoveData moveData = null,
            string applyOverwrite = "",
            bool yawnCheck = false,
            bool isSynchronize = false,
            bool forceFailMessage = false,
            bool apply = true
            )
        {
            bool success = false;
            if (battle.IsPokemonOnFieldAndAble(targetPokemon))
            {
                bool isSelf = (userPokemon != null) ? userPokemon.IsTheSameAs(targetPokemon) : false;
                bool isAlly = (userPokemon == null) ? false
                    : (userPokemon == targetPokemon) ? false
                    : battle.ArePokemonAllies(userPokemon, targetPokemon);
                bool isOpponent = (userPokemon == null) ? false
                    : battle.ArePokemonEnemies(userPokemon, targetPokemon);

                bool isNonVolatile = statusData.GetEffectNew(PokemonSEType.NonVolatile) != null;
                bool statusBlockedFully = false;

                BattleTeam targetTeam = battle.GetTeam(targetPokemon);
                List<Pokemon> allyPokemon = battle.GetAllyPokemon(targetPokemon);
                List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(userPokemon);
                List<Pokemon.Ability> targetAbilites = battle.PBPGetAbilities(targetPokemon);
                // all checks here

                // Check if battle conditions block
                // Ex. Electric Terrain blocks sleep, Misty Terrain blocks non-volatile statuses
                List<BattleCondition> allConditions = battle.BBPGetSCs();
                for (int i = 0; i < allConditions.Count && !statusBlockedFully; i++)
                {
                    // Check all blocked status effects for a battle condition
                    List<EffectDatabase.StatusBTLEff.BattleSE> blockStatuses_ =
                        allConditions[i].data.GetEffectsNew(BattleSEType.BlockStatus);
                    for (int k = 0; k < blockStatuses_.Count && !statusBlockedFully; k++)
                    {
                        EffectDatabase.StatusBTLEff.BlockStatus blockStatus =
                            blockStatuses_[k] as EffectDatabase.StatusBTLEff.BlockStatus;
                        if (battle.BBPIsPokemonAffectedByBS(pokemon: targetPokemon, statusData: allConditions[i].data)
                            && battle.DoesBattleEFiltersPass(
                                effect: blockStatus,
                                userPokemon: userPokemon,
                                targetPokemon: targetPokemon
                                )
                            )
                        {
                            // Check all individual statuses
                            bool statusBlocked = false;
                            if (blockStatus.statusIDs.Contains(statusData.ID))
                            {
                                statusBlocked = true;
                            }
                            if (statusBlocked)
                            {
                                statusBlockedFully = true;
                                if (forceFailMessage)
                                {
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = blockStatus.blockText,
                                        pokemonTargetID = targetPokemon.uniqueID
                                    });
                                }
                            }
                        }
                    }
                }

                // Limber (Teammates)
                for (int n = 0; n < allyPokemon.Count && !statusBlockedFully; n++)
                {
                    Pokemon curPokemon = allyPokemon[n];
                    List<Pokemon.Ability> curAbilities = battle.PBPGetAbilities(curPokemon);

                    for (int i = 0; i < curAbilities.Count && !statusBlockedFully; i++)
                    {
                        Pokemon.Ability ability = curAbilities[i];
                        List<EffectDatabase.AbilityEff.AbilityEffect> limbers_ =
                            ability.data.GetEffectsNew(AbilityEffectType.Limber);

                        for (int k = 0; k < limbers_.Count && !statusBlockedFully; k++)
                        {
                            EffectDatabase.AbilityEff.Limber limber =
                                limbers_[k] as EffectDatabase.AbilityEff.Limber;

                            if (battle.DoEffectFiltersPass(
                                filters: limber.filters,
                                userPokemon: curPokemon,
                                targetPokemon: targetPokemon,
                                targetTeam: targetTeam
                                )
                                && limber.pastelVeil)
                            {
                                bool canLimber = true;

                                // Specific Conditions
                                if (canLimber && limber.conditions.Count > 0)
                                {
                                    canLimber = false;
                                    for (int j = 0; j < limber.conditions.Count; j++)
                                    {
                                        if (statusData.ID == limber.conditions[j]
                                            || statusData.IsABaseID(limber.conditions[j]))
                                        {
                                            canLimber = true;
                                        }
                                    }
                                }

                                // Condition Types
                                if (canLimber && limber.statusTypes.Count > 0)
                                {
                                    canLimber = false;
                                    List<PokemonSEType> statusTypes = new List<PokemonSEType>(limber.statusTypes);
                                    for (int j = 0; j < statusTypes.Count; j++)
                                    {
                                        if (statusData.GetEffectNew(statusTypes[j]) != null)
                                        {
                                            canLimber = true;
                                        }
                                    }
                                }

                                if (canLimber)
                                {
                                    statusBlockedFully = true;
                                    if (forceFailMessage)
                                    {
                                        PBPShowAbility(curPokemon, ability);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = statusData.failTextID,
                                            pokemonUserID = curPokemon.uniqueID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            statusID = statusData.ID
                                        });
                                    }
                                }
                            }
                        }
                    }

                }

                // Limber / Immunity / Vital Spirit / etc.
                for (int i = 0; i < targetAbilites.Count && !statusBlockedFully; i++)
                {
                    Pokemon.Ability ability = targetAbilites[i];
                    List<EffectDatabase.AbilityEff.AbilityEffect> limbers_ =
                        ability.data.GetEffectsNew(AbilityEffectType.Limber);
                    for (int k = 0; k < limbers_.Count && !statusBlockedFully; k++)
                    {
                        EffectDatabase.AbilityEff.Limber limber =
                            limbers_[k] as EffectDatabase.AbilityEff.Limber;

                        if (battle.DoEffectFiltersPass(
                            filters: limber.filters,
                            userPokemon: targetPokemon,
                            targetPokemon: targetPokemon,
                            targetTeam: targetTeam
                            )
                            && limber.healSelf)
                        {
                            bool canLimber = true;

                            // Specific Conditions
                            if (canLimber && limber.conditions.Count > 0)
                            {
                                canLimber = false;
                                for (int j = 0; j < limber.conditions.Count; j++)
                                {
                                    if (statusData.ID == limber.conditions[j]
                                        || statusData.IsABaseID(limber.conditions[j]))
                                    {
                                        canLimber = true;
                                    }
                                }
                            }

                            // Condition Types
                            if (canLimber && limber.statusTypes.Count > 0)
                            {
                                canLimber = false;
                                List<PokemonSEType> statusTypes = new List<PokemonSEType>(limber.statusTypes);
                                for (int j = 0; j < statusTypes.Count; j++)
                                {
                                    if (statusData.GetEffectNew(statusTypes[j]) != null)
                                    {
                                        canLimber = true;
                                    }
                                }
                            }

                            if (canLimber)
                            {
                                statusBlockedFully = true;
                                if (forceFailMessage)
                                {
                                    PBPShowAbility(targetPokemon, ability);
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = statusData.failTextID,
                                        pokemonTargetID = targetPokemon.uniqueID,
                                        statusID = statusData.ID
                                    });
                                }
                            }
                        }
                    }
                }

                // No two of the same status
                StatusCondition existingStatus = battle.PBPGetSC(targetPokemon, statusData.ID);
                if (!statusBlockedFully && existingStatus != null)
                {
                    statusBlockedFully = true;
                    if (forceFailMessage)
                    {
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = statusData.alreadyTextID,
                            pokemonTargetID = targetPokemon.uniqueID
                        });
                    }
                }

                // Can't overwrite Comatose
                if (!statusBlockedFully)
                {
                    Pokemon.Ability ability = battle.PBPGetComatoseSCAbility(targetPokemon);
                    if (ability != null)
                    {
                        statusBlockedFully = true;
                        if (forceFailMessage)
                        {
                            PBPShowAbility(targetPokemon, ability.data);
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = statusData.failTextID,
                                pokemonTargetID = targetPokemon.uniqueID
                            });
                        }
                    }
                }

                // Shields Down
                if (!statusBlockedFully)
                {
                    for (int i = 0; i < targetAbilites.Count && !statusBlockedFully; i++)
                    {
                        Pokemon.Ability ability = targetAbilites[i];
                        EffectDatabase.AbilityEff.AbilityEffect shieldsDown_ = 
                            ability.data.GetEffectNew(AbilityEffectType.ShieldsDown);
                        if (shieldsDown_ != null)
                        {
                            EffectDatabase.AbilityEff.ShieldsDown shieldsDown =
                                shieldsDown_ as EffectDatabase.AbilityEff.ShieldsDown;
                            for (int k = 0; k < shieldsDown.meteorForms.Count && !statusBlockedFully; k++)
                            {
                                if (shieldsDown.meteorForms[k].IsAForm(targetPokemon)
                                    && shieldsDown.meteorForms[k].IsStatusBlocked(statusData))
                                {
                                    statusBlockedFully = true;
                                    PBPShowAbility(targetPokemon, ability);
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = statusData.failTextID,
                                        pokemonTargetID = targetPokemon.uniqueID
                                    });
                                }
                            }
                        }
                    }
                }

                // Can't overwrite non-volatile statuses with a higher or equal priority
                if (!statusBlockedFully)
                {
                    EffectDatabase.StatusPKEff.PokemonSE newPriority_ =
                        statusData.GetEffectNew(PokemonSEType.NonVolatile);
                    if (newPriority_ != null)
                    {
                        EffectDatabase.StatusPKEff.NonVolatile newPriority =
                            newPriority_ as EffectDatabase.StatusPKEff.NonVolatile;
                        EffectDatabase.StatusPKEff.PokemonSE oldPriority_ =
                            targetPokemon.nonVolatileStatus.data.GetEffectNew(PokemonSEType.NonVolatile);
                        if (oldPriority_ != null)
                        {
                            EffectDatabase.StatusPKEff.NonVolatile oldPriority =
                                oldPriority_ as EffectDatabase.StatusPKEff.NonVolatile;
                            if (newPriority.priority <= oldPriority.priority)
                            {
                                statusBlockedFully = true;
                                if (forceFailMessage)
                                {
                                    string textID = (newPriority.priority < oldPriority.priority) 
                                        ? oldPriority.negateTextID
                                        : statusData.failTextID;
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonTargetID = targetPokemon.uniqueID
                                    });
                                }
                            }
                        }
                    }
                }

                // Type Immunities
                if (!statusBlockedFully)
                {
                    // Corrosion
                    bool isCorrosive = false;
                    if (!isCorrosive && userPokemon != null)
                    {
                        List<EffectDatabase.AbilityEff.AbilityEffect> corrosion_ =
                            battle.PBPGetAbilityEffects(userPokemon, AbilityEffectType.Corrosion);
                        for (int i = 0; i < corrosion_.Count && !isCorrosive; i++)
                        {
                            EffectDatabase.AbilityEff.Corrosion corrosion = 
                                corrosion_[i] as EffectDatabase.AbilityEff.Corrosion;
                            for (int k = 0; k < corrosion.statuses.Count && !isCorrosive; k++)
                            {
                                if (statusData.ID == corrosion.statuses[k]
                                    || statusData.IsABaseID(corrosion.statuses[k]))
                                {
                                    isCorrosive = true;
                                }
                            }
                        }
                    }
                
                    if (!isCorrosive)
                    {
                        EffectDatabase.StatusPKEff.PokemonSE typeImmunity_ =
                            statusData.GetEffectNew(PokemonSEType.TypeImmunity);
                        if (typeImmunity_ != null)
                        {
                            EffectDatabase.StatusPKEff.TypeImmunity typeImmunity =
                                typeImmunity_ as EffectDatabase.StatusPKEff.TypeImmunity;
                            if (!battle.DoEffectFiltersPass(
                                filters: typeImmunity.filters,
                                userPokemon: userPokemon,
                                targetPokemon: targetPokemon
                                ))
                            {
                                statusBlockedFully = true;
                                if (forceFailMessage)
                                {
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = statusData.failTextID,
                                        pokemonTargetID = targetPokemon.uniqueID
                                    });
                                }
                            }
                        }
                    }
                }

                // Apply non-volatile status
                if (isNonVolatile && !statusBlockedFully)
                {
                    success = true;
                    if (apply)
                    {
                        // Early Bird
                        if (turnsLeft > 0)
                        {
                            float applyTurns = turnsLeft;
                            List<EffectDatabase.AbilityEff.AbilityEffect> earlyBirds_ =
                                battle.PBPGetAbilityEffects(
                                    pokemon: targetPokemon,
                                    effectType: AbilityEffectType.EarlyBird
                                    );
                            for (int i = 0; i < earlyBirds_.Count; i++)
                            {
                                EffectDatabase.AbilityEff.EarlyBird earlyBird =
                                    earlyBirds_[i] as EffectDatabase.AbilityEff.EarlyBird;
                                for (int k = 0; k < earlyBird.conditions.Count; k++)
                                {
                                    if (statusData.ID == earlyBird.conditions[k]
                                        || statusData.IsABaseID(earlyBird.conditions[k]))
                                    {
                                        applyTurns *= earlyBird.turnModifier;
                                    }
                                }

                            }
                            turnsLeft = Mathf.Max(0, Mathf.FloorToInt(applyTurns));
                        }
                        StatusCondition condition = battle.ApplyStatusCondition(
                            pokemon: targetPokemon,
                            statusID: statusData.ID,
                            turnsLeft: turnsLeft);

                        string inflictText = (applyOverwrite == "") ? condition.data.inflictTextID : applyOverwrite;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = inflictText,
                            pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                            pokemonTargetID = targetPokemon.uniqueID
                        });
                    }
                }

                // Apply volatile statuses if possible
                if (!yawnCheck || !statusBlockedFully)
                {
                    List<EffectDatabase.StatusPKEff.PokemonSE> statusEffects =
                    new List<EffectDatabase.StatusPKEff.PokemonSE>(statusData.effectsNew);
                    for (int i = 0; i < statusEffects.Count; i++)
                    {
                        yield return StartCoroutine(ExecutePokemonSEffect(
                            effect_: statusEffects[i],
                            targetPokemon: targetPokemon,
                            statusData: statusData,
                            userPokemon: userPokemon,
                            moveData: moveData,
                            forceFailMessage: forceFailMessage,
                            apply: apply,
                            callback: (result) =>
                            {
                                if (result)
                                {
                                    success = true;
                                }
                            }
                            ));
                    }
                }

                // Synchronize
                if (isNonVolatile 
                    && !isSynchronize 
                    && userPokemon != null
                    && !isSelf)
                {
                    List<AbilityData> abilities = battle.PBPGetAbilityDatas(targetPokemon);

                    bool isReflected = false;
                    List<Pokemon.AbilityEffectPair> synchronizePairs =
                        battle.PBPGetAbilityEffectPairs(targetPokemon, AbilityEffectType.Synchronize);
                    for (int i = 0; i < synchronizePairs.Count && !isReflected; i++)
                    {
                        Pokemon.AbilityEffectPair effectPair = synchronizePairs[i];
                        EffectDatabase.AbilityEff.Synchronize synchronize =
                            effectPair.effect as EffectDatabase.AbilityEff.Synchronize;

                        if (battle.DoEffectFiltersPass(
                            filters: synchronize.filters,
                            userPokemon: userPokemon,
                            targetPokemon: targetPokemon
                            ))
                        {
                            if (battle.DoesEffectFilterPass(
                                effect_: synchronize.conditionCheck,
                                userPokemon: userPokemon,
                                targetPokemon: targetPokemon
                                ))
                            {
                                isReflected = true;
                                PBPShowAbility(targetPokemon, effectPair.ability);

                                yield return StartCoroutine(ApplyPokemonSC(
                                    statusData: statusData.Clone(),
                                    targetPokemon: userPokemon,
                                    userPokemon: targetPokemon,
                                    turnsLeft: turnsLeft,
                                    isSynchronize: true,
                                    callback: (result) => { }
                                    ));
                            }
                        }
                    }
                }
            }

            callback(success);
            yield return null;
        }
        public IEnumerator ExecutePokemonSEffect(
            EffectDatabase.StatusPKEff.PokemonSE effect_,
            Pokemon targetPokemon,
            StatusPKData statusData,
            System.Action<bool> callback,
            Pokemon userPokemon = null,
            MoveData moveData = null,
            bool forceFailMessage = false,
            bool apply = true
            )
        {
            bool success = false;
            List<Pokemon> allyPokemon = battle.GetAllyPokemon(targetPokemon);

            bool effectWasBlocked = false;

            // ---ABILITY CHECKS---
            bool bypassAbility = false;
            // Mold Breaker bypasses ability immunities
            if (!bypassAbility)
            {
                EffectDatabase.AbilityEff.AbilityEffect moldBreakerEffect =
                    battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                if (moldBreakerEffect != null)
                {
                    bypassAbility = true;
                }

                if (moveData != null)
                {
                    // Sunsteel Strike bypasses ability immunities
                    EffectDatabase.MoveEff.MoveEffect effect = moveData.GetEffectNew(MoveEffectType.SunteelStrike);
                    if (effect != null)
                    {
                        bypassAbility = true;
                    }
                }
            
            }

            // Aroma Veil Check
            if (!effectWasBlocked)
            {
                for (int i = 0; i < allyPokemon.Count; i++)
                {
                    Pokemon curPokemon = allyPokemon[i];
                    if (!battle.IsPokemonFainted(curPokemon))
                    {
                        List<EffectDatabase.AbilityEff.AbilityEffect> obliviousEffects =
                            battle.PBPGetAbilityEffects(curPokemon, AbilityEffectType.Oblivious, bypassAbility);
                        for (int k = 0; k < obliviousEffects.Count; k++)
                        {
                            EffectDatabase.AbilityEff.Oblivious aromaVeil =
                                obliviousEffects[k] as EffectDatabase.AbilityEff.Oblivious;

                            // Team-based blocking
                            if (aromaVeil.aromaVeil && aromaVeil.effectsBlocked.Contains(effect_.effectType))
                            {
                                effectWasBlocked = true;
                                if (forceFailMessage)
                                {
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = aromaVeil.displayText,
                                        pokemonTargetID = targetPokemon.uniqueID
                                    });
                                }
                                break;
                            }
                        }
                    }
                }
            }

            // Oblivious / Own Tempo / etc. check
            if (!effectWasBlocked)
            {
                EffectDatabase.AbilityEff.AbilityEffect oblivious_ = 
                    battle.PBPGetAbilityEffect(targetPokemon, AbilityEffectType.Oblivious, bypassAbility);
                if (oblivious_ != null)
                {
                    EffectDatabase.AbilityEff.Oblivious oblivious = oblivious_ as EffectDatabase.AbilityEff.Oblivious;
                    if (oblivious.effectsBlocked.Contains(effect_.effectType))
                    {
                        effectWasBlocked = true;
                        if (forceFailMessage)
                        {
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = oblivious.displayText,
                                pokemonTargetID = targetPokemon.uniqueID
                            });
                        }
                    }
                }
            }

            // ---EFFECT EXECUTION---
            if (!effectWasBlocked)
            {
                // Volatility
                if (effect_ is EffectDatabase.StatusPKEff.Volatile)
                {
                    EffectDatabase.StatusPKEff.Volatile volatileEffect =
                        effect_ as EffectDatabase.StatusPKEff.Volatile;
                    bool isAlready = false;
                    bool isFail = false;

                    // Move-Limiting
                    if (effect_ is EffectDatabase.StatusPKEff.MoveLimiting)
                    {
                        List<Pokemon.BattleProperties.MoveLimiter> moveLimiters =
                            new List<Pokemon.BattleProperties.MoveLimiter>(targetPokemon.bProps.moveLimiters);

                        // Disable
                        if (effect_ is EffectDatabase.StatusPKEff.Disable)
                        {
                            EffectDatabase.StatusPKEff.Disable effect = effect_ as EffectDatabase.StatusPKEff.Disable;
                            for (int i = 0; i < moveLimiters.Count && !isAlready; i++)
                            {
                                if (moveLimiters[i].effect is EffectDatabase.StatusPKEff.Disable)
                                {
                                    isAlready = true;
                                }
                            }
                            isFail = isAlready;

                            // No move used last
                            if (!isFail)
                            {
                                if (string.IsNullOrEmpty(targetPokemon.bProps.lastMove))
                                {
                                    isFail = true;
                                }
                            }
                            if (!isFail && !string.IsNullOrEmpty(targetPokemon.bProps.lastMove))
                            {
                                // Move not in moveset
                                if (!isFail && battle.DoesPokemonHaveMove(targetPokemon, targetPokemon.bProps.lastMove))
                                {
                                    isFail = true;
                                }
                                // Move can't be disabled
                                else
                                {
                                    MoveData lastData =
                                        MoveDatabase.instance.GetMoveData(targetPokemon.bProps.lastMove);
                                    if (lastData.HasTag(MoveTag.CannotDisable))
                                    {
                                        isFail = true;
                                    }
                                }
                            }

                            if (!isFail)
                            {
                                success = true;
                                Pokemon.BattleProperties.MoveLimiter limiter =
                                    new Pokemon.BattleProperties.MoveLimiter(
                                        effect: effect,
                                        turnsLeft: effect.defaultTurns.GetTurns(),
                                        affectedMoves: new string[] { targetPokemon.bProps.lastMove }
                                        );
                                targetPokemon.bProps.moveLimiters.Add(limiter);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.startText,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }
                        // Encore
                        else if (effect_ is EffectDatabase.StatusPKEff.Encore)
                        {
                            EffectDatabase.StatusPKEff.Encore effect = effect_ as EffectDatabase.StatusPKEff.Encore;
                            for (int i = 0; i < moveLimiters.Count && !isAlready; i++)
                            {
                                if (moveLimiters[i].effect is EffectDatabase.StatusPKEff.Encore)
                                {
                                    isAlready = true;
                                }
                            }
                            isFail = isAlready;

                            // No move used last
                            if (!isFail)
                            {
                                if (string.IsNullOrEmpty(targetPokemon.bProps.lastMove))
                                {
                                    isFail = true;
                                }
                            }
                            if (!isFail && !string.IsNullOrEmpty(targetPokemon.bProps.lastMove))
                            {
                                // Move not in moveset
                                if (!isFail && battle.DoesPokemonHaveMove(targetPokemon, targetPokemon.bProps.lastMove))
                                {
                                    isFail = true;
                                }
                                // Move can't be disabled
                                else
                                {
                                    MoveData lastData =
                                        MoveDatabase.instance.GetMoveData(targetPokemon.bProps.lastMove);
                                    if (lastData.HasTag(MoveTag.CannotEncore))
                                    {
                                        isFail = true;
                                    }
                                }
                            }

                            if (!isFail)
                            {
                                success = true;
                                Pokemon.BattleProperties.MoveLimiter limiter =
                                    new Pokemon.BattleProperties.MoveLimiter(
                                        effect: effect,
                                        turnsLeft: effect.defaultTurns.GetTurns(),
                                        affectedMoves: new string[] { targetPokemon.bProps.lastMove }
                                        );
                                targetPokemon.bProps.moveLimiters.Add(limiter);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.startText,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }
                        // Heal Block
                        else if (effect_ is EffectDatabase.StatusPKEff.HealBlock)
                        {
                            EffectDatabase.StatusPKEff.HealBlock effect = effect_ as EffectDatabase.StatusPKEff.HealBlock;
                            for (int i = 0; i < moveLimiters.Count && !isAlready; i++)
                            {
                                if (moveLimiters[i].effect is EffectDatabase.StatusPKEff.HealBlock)
                                {
                                    isAlready = true;
                                }
                            }
                            isFail = isAlready;

                            if (!isFail)
                            {
                                success = true;
                                Pokemon.BattleProperties.MoveLimiter limiter =
                                    new Pokemon.BattleProperties.MoveLimiter(
                                        effect: effect,
                                        turnsLeft: effect.defaultTurns.GetTurns()
                                        );
                                targetPokemon.bProps.moveLimiters.Add(limiter);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.startText,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }
                        // Taunt
                        else if (effect_ is EffectDatabase.StatusPKEff.Taunt)
                        {
                            EffectDatabase.StatusPKEff.Taunt effect = effect_ as EffectDatabase.StatusPKEff.Taunt;
                            for (int i = 0; i < moveLimiters.Count && !isAlready; i++)
                            {
                                if (moveLimiters[i].effect is EffectDatabase.StatusPKEff.Taunt)
                                {
                                    isAlready = true;
                                }
                            }
                            isFail = isAlready;

                            if (!isFail)
                            {
                                success = true;
                                Pokemon.BattleProperties.MoveLimiter limiter =
                                    new Pokemon.BattleProperties.MoveLimiter(
                                        effect: effect,
                                        turnsLeft: effect.defaultTurns.GetTurns()
                                        );
                                targetPokemon.bProps.moveLimiters.Add(limiter);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.startText,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }
                        // Torment
                        else if (effect_ is EffectDatabase.StatusPKEff.Torment)
                        {
                            EffectDatabase.StatusPKEff.Torment effect = effect_ as EffectDatabase.StatusPKEff.Torment;
                            for (int i = 0; i < moveLimiters.Count && !isAlready; i++)
                            {
                                if (moveLimiters[i].effect is EffectDatabase.StatusPKEff.Torment)
                                {
                                    isAlready = true;
                                }
                            }
                            isFail = isAlready;

                            if (!isFail)
                            {
                                success = true;
                                Pokemon.BattleProperties.MoveLimiter limiter =
                                    new Pokemon.BattleProperties.MoveLimiter(
                                        effect: effect,
                                        turnsLeft: effect.defaultTurns.GetTurns()
                                        );
                                targetPokemon.bProps.moveLimiters.Add(limiter);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.startText,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }
                    }
                    // Embargo
                    else if (effect_ is EffectDatabase.StatusPKEff.Embargo)
                    {
                        EffectDatabase.StatusPKEff.Embargo effect = effect_ as EffectDatabase.StatusPKEff.Embargo;
                        if (targetPokemon.bProps.embargo != null)
                        {
                            isAlready = true;
                        }
                        isFail = isAlready;

                        if (!isFail)
                        {
                            success = true;
                            Pokemon.BattleProperties.Embargo embargo =
                                new Pokemon.BattleProperties.Embargo(
                                    effect: effect,
                                    turnsLeft: effect.defaultTurns.GetTurns()
                                    );
                            targetPokemon.bProps.embargo = embargo;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = effect.startText,
                                pokemonTargetID = targetPokemon.uniqueID
                            });
                        }
                    }

                    if (isFail)
                    {
                        success = false;
                        if (forceFailMessage)
                        {
                            string textID = isAlready ? volatileEffect.alreadyText : volatileEffect.failText;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = textID,
                                pokemonTargetID = targetPokemon.uniqueID
                            });
                        }
                    }
                }
                // Defense Curl
                else if (effect_.effectType == PokemonSEType.DefenseCurl)
                {
                    EffectDatabase.StatusPKEff.DefenseCurl defenseCurl = effect_ as EffectDatabase.StatusPKEff.DefenseCurl;
                    success = true;
                    if (apply)
                    {
                        targetPokemon.bProps.defenseCurl = defenseCurl.Clone();
                    }
                }
                // Electrify
                else if (effect_.effectType == PokemonSEType.Electrify)
                {
                    EffectDatabase.StatusPKEff.Electrify electrify = effect_ as EffectDatabase.StatusPKEff.Electrify;
                    success = true;
                    if (apply)
                    {
                        targetPokemon.bProps.electrify = electrify.Clone();
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = electrify.displayText,
                            pokemonTargetID = targetPokemon.uniqueID
                        });
                    }
                }
                // Flinch
                else if (effect_.effectType == PokemonSEType.Flinch)
                {
                    // check inner focus

                    EffectDatabase.StatusPKEff.Flinch flinch = effect_ as EffectDatabase.StatusPKEff.Flinch;
                    if (targetPokemon.bProps.flinch == null
                        && targetPokemon.dynamaxState == Pokemon.DynamaxState.None)
                    {
                        success = true;
                        if (apply)
                        {
                            targetPokemon.bProps.flinch = flinch.Clone();
                        }
                    }
                }
                // Identification - Foresight / Odor Sleuth / Miracle Eye
                else if (effect_.effectType == PokemonSEType.Identified)
                {
                    EffectDatabase.StatusPKEff.Identification identified = effect_ as EffectDatabase.StatusPKEff.Identification;
                    List<string> affectedTypes = new List<string>(identified.types);
                    if (affectedTypes.Contains("ALL"))
                    {
                        affectedTypes = TypeDatabase.instance.GetAllTypes();
                    }

                    bool fail = false;
                    for (int i = 0; i < targetPokemon.bProps.identifieds.Count; i++)
                    {
                        EffectDatabase.StatusPKEff.Identification curIdentified = targetPokemon.bProps.identifieds[i];
                        if (battle.AreTypesContained(curIdentified.types, affectedTypes))
                        {
                            fail = true;
                        }
                    }

                    success = !fail;
                    if (success && apply)
                    {
                        targetPokemon.bProps.identifieds.Add(
                            new EffectDatabase.StatusPKEff.Identification(types: affectedTypes));
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = identified.alreadyText,
                            pokemonTargetID = targetPokemon.uniqueID
                        });
                    }
                    else if (fail && forceFailMessage)
                    {
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = identified.alreadyText,
                            pokemonTargetID = targetPokemon.uniqueID
                        });
                    }
                }
                // Imprison
                else if (effect_.effectType == PokemonSEType.Imprison)
                {
                    EffectDatabase.StatusPKEff.Imprison imprison = effect_ as EffectDatabase.StatusPKEff.Imprison;
                    success = true;
                    if (apply)
                    {
                        targetPokemon.bProps.imprison = imprison.Clone();
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = imprison.startText,
                            pokemonTargetID = targetPokemon.uniqueID
                        });
                    }
                }
                // Infatuation
                else if (effect_.effectType == PokemonSEType.Infatuation)
                {
                    if (userPokemon != null)
                    {
                        EffectDatabase.StatusPKEff.Infatuation infatuation = effect_ as EffectDatabase.StatusPKEff.Infatuation;
                        bool fail = false;

                        // can't be infatuated w/ multiple pokemon (maybe?)
                        if (!fail && targetPokemon.bProps.infatuation != null)
                        {
                            fail = true;
                            if (forceFailMessage)
                            {
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = infatuation.alreadyText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }
                        // can't be infatuated w/ same gender or no gender
                        if (!fail &&
                            (userPokemon.gender == PokemonGender.Genderless
                            || targetPokemon.gender == PokemonGender.Genderless
                            || userPokemon.gender == targetPokemon.gender))
                        {
                            fail = true;
                            if (forceFailMessage)
                            {
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = infatuation.failText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }

                        success = !fail;
                        // inflict infatuation
                        if (success && apply)
                        {
                            EffectDatabase.StatusPKEff.Infatuation appliedInfatuation = infatuation.Clone();
                            appliedInfatuation.infatuator = userPokemon.uniqueID;
                            targetPokemon.bProps.infatuation = appliedInfatuation;
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = infatuation.startText,
                                pokemonUserID = userPokemon.uniqueID,
                                pokemonTargetID = targetPokemon.uniqueID
                            });
                        }
                    }
                }
                // Perish Song
                else if (effect_.effectType == PokemonSEType.PerishSong)
                {
                    EffectDatabase.StatusPKEff.PerishSong perishSong = effect_ as EffectDatabase.StatusPKEff.PerishSong;
                    if (targetPokemon.bProps.perishSong == null)
                    {
                        success = true;
                        if (apply)
                        {
                            targetPokemon.bProps.perishSong = perishSong.Clone();
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = perishSong.startText,
                                pokemonTargetID = targetPokemon.uniqueID,
                                intArgs = new List<int> { perishSong.turnsLeft }
                            });
                        }
                    }
                }
                // Tar Shot
                else if (effect_.effectType == PokemonSEType.TarShot)
                {
                    EffectDatabase.StatusPKEff.TarShot tarShot = effect_ as EffectDatabase.StatusPKEff.TarShot;
                    bool canAdd = true;
                    for (int k = 0; k < targetPokemon.bProps.tarShots.Count; k++)
                    {
                        if (targetPokemon.bProps.tarShots[k].tarShotID == tarShot.tarShotID)
                        {
                            canAdd = false;
                            break;
                        }
                    }

                    if (canAdd)
                    {
                        success = true;
                        if (apply)
                        {
                            targetPokemon.bProps.tarShots.Add(tarShot.Clone());
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = tarShot.startText,
                                pokemonTargetID = targetPokemon.uniqueID
                            });
                        }
                    }
                }
                // Yawn
                else if (effect_.effectType == PokemonSEType.Yawn)
                {
                    EffectDatabase.StatusPKEff.Yawn yawn = effect_ as EffectDatabase.StatusPKEff.Yawn;

                    bool statusSuccess = false;
                    yield return StartCoroutine(ApplyPokemonSC(
                        statusData: StatusPKDatabase.instance.GetStatusData(yawn.statusID),
                        targetPokemon: targetPokemon,
                        userPokemon: userPokemon,
                        moveData: moveData,
                        yawnCheck: true,
                        forceFailMessage: forceFailMessage,
                        apply: false,
                        callback: (result) =>
                        {
                            if (result)
                            {
                                statusSuccess = true;
                            }
                        }
                        ));
                    if (statusSuccess)
                    {
                        success = true;
                        if (apply)
                        {
                            targetPokemon.bProps.yawn = yawn.Clone();
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = yawn.startText,
                                pokemonTargetID = targetPokemon.uniqueID
                            });
                        }
                    }
                }
            }

            callback(success);
            yield return null;
        }
        public IEnumerator ExecutePokemonSE(
            EffectDatabase.StatusPKEff.PokemonSE effect_,
            Pokemon pokemon,
            StatusCondition condition
            )
        {
            StatusPKData statusData = condition.data;
            if (!battle.IsPokemonFainted(pokemon))
            {
                List<Pokemon.Ability> abilities = battle.PBPGetAbilities(pokemon);

                // HP Loss (ex. Bind / Burn / Poison)
                if (effect_.effectType == PokemonSEType.HPLoss)
                {
                    EffectDatabase.StatusPKEff.HPLoss effect = effect_ as EffectDatabase.StatusPKEff.HPLoss;
                    if (battle.DoEffectFiltersPass(
                        filters: effect.filters,
                        targetPokemon: pokemon
                        ))
                    {
                        bool applyDamage = true;

                        // Poison Heal nullifies damage
                        if (applyDamage)
                        {
                            List<EffectDatabase.AbilityEff.AbilityEffect> poisonHeal_ =
                                battle.PBPGetAbilityEffects(pokemon, AbilityEffectType.PoisonHeal);
                            for (int i = 0; i < poisonHeal_.Count && applyDamage; i++)
                            {
                                EffectDatabase.AbilityEff.PoisonHeal poisonHeal =
                                    poisonHeal_[i] as EffectDatabase.AbilityEff.PoisonHeal;
                                if (battle.DoEffectFiltersPass(
                                    filters: poisonHeal.filters,
                                    targetPokemon: pokemon
                                    ))
                                {
                                    for (int k = 0; k < poisonHeal.conditions.Count && applyDamage; k++)
                                    {
                                        EffectDatabase.AbilityEff.PoisonHeal.HealCondition healCond =
                                            poisonHeal.conditions[k];

                                        for (int j = 0; j < healCond.conditions.Count && applyDamage; j++)
                                        {
                                            EffectDatabase.Filter.Harvest curCond = healCond.conditions[j];
                                            if (curCond.DoesStatusSatisfy(statusData))
                                            {
                                                applyDamage = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (applyDamage)
                        {
                            float toxicMultiplier = 1f;
                            if (effect.toxicStack)
                            {
                                toxicMultiplier *= condition.turnsActive;
                            }
                            int damage = battle.GetPokemonHPByPercent(pokemon, effect.hpLossPercent * toxicMultiplier);
                            // Toxic effect
                            if (damage > 0)
                            {
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = effect.displayText,
                                    pokemonTargetID = pokemon.uniqueID
                                });

                                yield return StartCoroutine(PBPDamagePokemon(
                                    pokemon: pokemon,
                                    HPToLose: damage
                                    ));
                            }
                        }
                    }
                }
            }

        

            yield return null;
        }

        // Team Status Conditions
        public IEnumerator ApplyTeamSC(
            StatusTEData statusData,
            BattleTeam targetTeam,
            System.Action<bool> callback,
            int turnsLeft = -1,
            Pokemon userPokemon = null,
            MoveData moveData = null,
            string inflictOverwrite = "",
            bool forceFailureMessage = false,
            bool apply = true
            )
        {
            bool success = true;

            bool isGMaxWildfire = false;

            // G-Max Wildfire
            if (success)
            {
                EffectDatabase.StatusTEEff.TeamSE newPriority_ = statusData.GetEffectNew(TeamSEType.GMaxWildfirePriority);
                if (newPriority_ != null)
                {
                    EffectDatabase.StatusTEEff.GMaxWildfirePriority newPriority = 
                        newPriority_ as EffectDatabase.StatusTEEff.GMaxWildfirePriority;
                    isGMaxWildfire = true;

                    BattleTeamProperties.GMaxWildfire existingStatus = targetTeam.bProps.GMaxWildfireStatus;
                    if (existingStatus != null)
                    {
                        StatusTEData existingData = StatusTEDatabase.instance.GetStatusData(existingStatus.statusID);

                        EffectDatabase.StatusTEEff.TeamSE oldPriority_ = 
                            existingData.GetEffectNew(TeamSEType.GMaxWildfirePriority);
                        EffectDatabase.StatusTEEff.GMaxWildfirePriority oldPriority =
                            oldPriority_ as EffectDatabase.StatusTEEff.GMaxWildfirePriority;

                        if (existingData.ID == statusData.ID)
                        {
                            success = false;
                            if (forceFailureMessage)
                            {

                            }
                        }
                        else if (newPriority.priority >= 0)
                        {
                            if (oldPriority.priority > newPriority.priority)
                            {
                                success = false;
                                if (forceFailureMessage)
                                {

                                }
                            }
                        }
                    }
                }
            }

            // Light Screen
            if (success)
            {

            }

            // can't apply duplicates
            if (success)
            {
                TeamCondition existingCondition = battle.TBPGetSC(
                    team: targetTeam,
                    statusID: statusData.ID,
                    descendant: false
                    );
                if (existingCondition != null)
                {
                    success = false;
                    if (forceFailureMessage)
                    {
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = statusData.alreadyTextID,
                            teamID = targetTeam.teamID,
                            statusTeamID = statusData.ID
                        });
                    }
                }
            }

            // Success
            if (success)
            {
                StatusTEData inflictData = statusData.Clone();
            
                if (apply)
                {
                    string inflictText = (inflictOverwrite == "") ? statusData.inflictTextID : inflictOverwrite;
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = inflictText,
                        teamID = targetTeam.teamID,
                        statusTeamID = statusData.ID
                    });

                    if (isGMaxWildfire)
                    {
                        targetTeam.bProps.GMaxWildfireStatus = 
                            new BattleTeamProperties.GMaxWildfire(inflictData.ID, turnsLeft);
                    }
                    else
                    {
                        TeamCondition condition = new TeamCondition(statusID: statusData.ID, turnsLeft: turnsLeft);
                        battle.ApplyTeamSC(targetTeam, condition);
                    }

                    // Apply effects on start
                    yield return StartCoroutine(ExecuteTeamSETiming(team: targetTeam, statusData: statusData));
                }
            }

            callback(success);
            yield return null;
        }
        public IEnumerator EndTeamSC(
            StatusTEData statusData,
            BattleTeam targetTeam,
            System.Action<bool> callback,
            string endOverwrite = "",
            bool apply = true
            )
        {
            bool success = false;

            // End G-Max Wildfire
            if (targetTeam.bProps.GMaxWildfireStatus != null)
            {
                if (targetTeam.bProps.GMaxWildfireStatus.statusID == statusData.ID)
                {
                    success = true;
                    if (apply)
                    {
                        targetTeam.bProps.GMaxWildfireStatus = null;
                    }
                }
            }

            if (success && apply)
            {
                string endText = (endOverwrite == "") ? statusData.endTextID : endOverwrite;
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = endText,
                    teamID = targetTeam.teamID,
                    statusTeamID = statusData.ID
                });
            }
        
            callback(success);
            yield return null;
        }

        public IEnumerator ExecuteTeamSETiming(
            BattleTeam team,
            StatusTEData statusData
            )
        {
            List<EffectDatabase.StatusTEEff.TeamSE> effects = statusData.GetEffectsNewFiltered(TeamSETiming.OnStart);
            yield return StartCoroutine(ExecuteTeamSEs(effects: effects, team: team, statusData: statusData));
        }
        public IEnumerator ExecuteTeamSEs(
            List<EffectDatabase.StatusTEEff.TeamSE> effects,
            BattleTeam team,
            StatusTEData statusData
            )
        {
            for (int i = 0; i < effects.Count; i++)
            {
                yield return StartCoroutine(ExecuteTeamSE(effect_: effects[i], team: team, statusData: statusData));
            }
        }
        public IEnumerator ExecuteTeamSE(
            EffectDatabase.StatusTEEff.TeamSE effect_,
            BattleTeam team,
            StatusTEData statusData
            )
        {
            // HP Loss (ex. G-Max Wildfire)
            if (effect_.effectType == TeamSEType.HPLoss)
            {
                EffectDatabase.StatusTEEff.HPLoss effect = effect_ as EffectDatabase.StatusTEEff.HPLoss;
                List<Pokemon> allPokemon = battle.GetTeamPokemonOnField(team);

                // create buffet events
                PBS.Battle.View.Events.MessageParameterized textEvent = new Battle.View.Events.MessageParameterized
                {

                };

                List<Pokemon> hitPokemon = new List<Pokemon>();
                List<PBS.Battle.View.Events.PokemonHealthDamage> dmgEvents = new List<PBS.Battle.View.Events.PokemonHealthDamage>();
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    bool buffet = true;
                    Pokemon pokemon = allPokemon[i];

                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        // Is Pokemon affected by condition
                        if (buffet)
                        {
                            if (!battle.TBPIsPokemonAffectedByTS(pokemon: pokemon, statusData: statusData))
                            {
                                buffet = false;
                            }
                        }

                        // type immunities done via <seealso cref="EffectDatabase.Filter.TypeList"/>.
                        if (buffet)
                        {
                            if (!battle.DoesTeamSEFiltersPass(
                                effect: effect,
                                targetPokemon: pokemon
                                ))
                            {
                                buffet = false;
                            }
                        }

                        // passed all checks, so we hit the pokemon
                        if (buffet)
                        {
                            hitPokemon.Add(pokemon);
                            float hpPercentLost = effect.hpLossPercent;

                            int preHP = pokemon.currentHP;
                            int damage = battle.GetPokemonHPByPercent(pokemon, hpPercentLost);
                            damage = Mathf.Max(1, damage);
                            int damageDealt = battle.SubtractPokemonHP(pokemon, damage);
                            int postHP = pokemon.currentHP;

                            PBS.Battle.View.Events.PokemonHealthDamage hpLossEvent = new PBS.Battle.View.Events.PokemonHealthDamage
                            {
                                pokemonUniqueID = pokemon.uniqueID,
                                preHP = preHP,
                                postHP = postHP,
                                maxHP = pokemon.maxHP
                            };
                            dmgEvents.Add(hpLossEvent);
                        }
                    }
                }

                // send buffet message if any pokemon lost HP
                if (hitPokemon.Count > 0)
                {
                    List<string> pokemonIDs = new List<string>();
                    for (int i = 0; i < hitPokemon.Count; i++)
                    {
                        pokemonIDs.Add(hitPokemon[i].uniqueID);
                    }
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = effect.displayText,
                        pokemonListIDs = pokemonIDs,
                        teamID = team.teamID,
                        statusTeamID = statusData.ID
                    });
                    for (int i = 0; i < dmgEvents.Count; i++)
                    {
                        SendEvent(dmgEvents[i]);
                    }
                }
            }

            yield return null;
        }

        // Battle Status Conditions
        public IEnumerator ApplyBattleSC(
            StatusBTLData statusData,
            System.Action<bool> callback,
            int turnsLeft = -1,
            Pokemon userPokemon = null,
            MoveData moveData = null,
            string inflictOverwrite = "",
            bool forceFailureMessage = false,
            bool apply = true
            )
        {
            bool success = false;
            bool stickCondition = true;

            // run a bunch of checks here

            // No two of the same status
            BattleCondition existingStatus = battle.BBPGetSC(statusData.ID);
            if (stickCondition && existingStatus != null)
            {
                // Undoes itself
                if (statusData.HasTag(BattleSTag.UndoesSelf))
                {
                    if (apply)
                    {
                        yield return StartCoroutine(HealBattleSC(
                            condition: existingStatus,
                            healerPokemon: userPokemon
                            ));
                    }
                }
                // Already exists
                else if (forceFailureMessage)
                {
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = statusData.alreadyTextID
                    });
                }
                stickCondition = false;
            }
        
            // Can't inflict lower priority battle environments
            EffectDatabase.StatusBTLEff.BattleEnvironment higherPriorityEnv = null;
            EffectDatabase.StatusBTLEff.BattleSE weather_ = statusData.GetEffectNew(BattleSEType.Weather);
            EffectDatabase.StatusBTLEff.BattleSE terrain_ = statusData.GetEffectNew(BattleSEType.Terrain);
            EffectDatabase.StatusBTLEff.BattleSE gravity_ = statusData.GetEffectNew(BattleSEType.Gravity);
            EffectDatabase.StatusBTLEff.BattleSE magicRoom_ = statusData.GetEffectNew(BattleSEType.MagicRoom);
            EffectDatabase.StatusBTLEff.BattleSE trickRoom_ = statusData.GetEffectNew(BattleSEType.TrickRoom);
            EffectDatabase.StatusBTLEff.BattleSE wonderRoom_ = statusData.GetEffectNew(BattleSEType.WonderRoom);

            // Weather
            if (stickCondition && weather_ != null)
            {
                EffectDatabase.StatusBTLEff.Weather newBattleEnv = weather_ as EffectDatabase.StatusBTLEff.Weather;
                EffectDatabase.StatusBTLEff.BattleSE oldBattleEnv_ =
                    battle.weather.data.GetEffectNew(BattleSEType.Weather);
                if (oldBattleEnv_ != null)
                {
                    EffectDatabase.StatusBTLEff.Weather oldBattleEnv = oldBattleEnv_ as EffectDatabase.StatusBTLEff.Weather;
                    if (newBattleEnv.priority < oldBattleEnv.priority)
                    {
                        stickCondition = false;
                        higherPriorityEnv = oldBattleEnv;
                    }
                }
            }
            // Terrain
            if (stickCondition && terrain_ != null)
            {
                EffectDatabase.StatusBTLEff.Terrain newBattleEnv = terrain_ as EffectDatabase.StatusBTLEff.Terrain;
                EffectDatabase.StatusBTLEff.BattleSE oldBattleEnv_ =
                    battle.terrain.data.GetEffectNew(BattleSEType.Terrain);
                if (oldBattleEnv_ != null)
                {
                    EffectDatabase.StatusBTLEff.Terrain oldBattleEnv = oldBattleEnv_ as EffectDatabase.StatusBTLEff.Terrain;
                    if (newBattleEnv.priority < oldBattleEnv.priority)
                    {
                        stickCondition = false;
                        higherPriorityEnv = oldBattleEnv;
                    }
                }
            }
            // Gravity
            if (stickCondition && gravity_ != null)
            {
                EffectDatabase.StatusBTLEff.Gravity newBattleEnv = gravity_ as EffectDatabase.StatusBTLEff.Gravity;
                EffectDatabase.StatusBTLEff.BattleSE oldBattleEnv_ =
                    battle.gravity.data.GetEffectNew(BattleSEType.Gravity);
                if (oldBattleEnv_ != null)
                {
                    EffectDatabase.StatusBTLEff.Gravity oldBattleEnv = oldBattleEnv_ as EffectDatabase.StatusBTLEff.Gravity;
                    if (newBattleEnv.priority < oldBattleEnv.priority)
                    {
                        stickCondition = false;
                        higherPriorityEnv = oldBattleEnv;
                    }
                }
            }
            // Magic Room
            if (stickCondition && magicRoom_ != null)
            {
                EffectDatabase.StatusBTLEff.MagicRoom newBattleEnv = magicRoom_ as EffectDatabase.StatusBTLEff.MagicRoom;
                EffectDatabase.StatusBTLEff.BattleSE oldBattleEnv_ =
                    battle.magicRoom.data.GetEffectNew(BattleSEType.MagicRoom);
                if (oldBattleEnv_ != null)
                {
                    EffectDatabase.StatusBTLEff.MagicRoom oldBattleEnv = oldBattleEnv_ as EffectDatabase.StatusBTLEff.MagicRoom;
                    if (newBattleEnv.priority < oldBattleEnv.priority)
                    {
                        stickCondition = false;
                        higherPriorityEnv = oldBattleEnv;
                    }
                }
            }
            // Trick Room
            if (stickCondition && trickRoom_ != null)
            {
                EffectDatabase.StatusBTLEff.TrickRoom newBattleEnv = trickRoom_ as EffectDatabase.StatusBTLEff.TrickRoom;
                EffectDatabase.StatusBTLEff.BattleSE oldBattleEnv_ =
                    battle.trickRoom.data.GetEffectNew(BattleSEType.TrickRoom);
                if (oldBattleEnv_ != null)
                {
                    EffectDatabase.StatusBTLEff.TrickRoom oldBattleEnv = oldBattleEnv_ as EffectDatabase.StatusBTLEff.TrickRoom;
                    if (newBattleEnv.priority < oldBattleEnv.priority)
                    {
                        stickCondition = false;
                        higherPriorityEnv = oldBattleEnv;
                    }
                }
            }
            // Wonder Room
            if (stickCondition && wonderRoom_ != null)
            {
                EffectDatabase.StatusBTLEff.WonderRoom newBattleEnv = wonderRoom_ as EffectDatabase.StatusBTLEff.WonderRoom;
                EffectDatabase.StatusBTLEff.BattleSE oldBattleEnv_ =
                    battle.wonderRoom.data.GetEffectNew(BattleSEType.WonderRoom);
                if (oldBattleEnv_ != null)
                {
                    EffectDatabase.StatusBTLEff.WonderRoom oldBattleEnv = oldBattleEnv_ as EffectDatabase.StatusBTLEff.WonderRoom;
                    if (newBattleEnv.priority < oldBattleEnv.priority)
                    {
                        stickCondition = false;
                        higherPriorityEnv = oldBattleEnv;
                    }
                }
            }

            // Failed because of a higher priority battle environment
            if (!stickCondition && forceFailureMessage)
            {
                if (higherPriorityEnv != null)
                {
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = higherPriorityEnv.negateText,
                        pokemonUserID = userPokemon.uniqueID
                    });
                }
            }

            // passed all checks, so apply status
            if (stickCondition)
            {
                success = true;
                if (apply)
                {
                    BattleCondition condition = battle.InflictBattleSC(
                        statusID: statusData.ID,
                        turnsLeft: turnsLeft);

                    string inflictText = (inflictOverwrite == "") ? statusData.inflictTextID : inflictOverwrite;
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = inflictText,
                        pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID
                    });

                    yield return StartCoroutine(ExecuteBattleSEsByTiming(
                        statusData: statusData,
                        timing: BattleSETiming.OnStart
                        ));

                    // Forecast
                    yield return StartCoroutine(PBPRunBCAbilityCheck());
                }
            }
            callback(success);
        }
    
        public IEnumerator ExecuteBattleSEsByTiming(
            StatusBTLData statusData,
            BattleSETiming timing
            )
        {
            yield return StartCoroutine(ExecuteBattleSEs(
                statusData: statusData,
                effects: statusData.GetEffectsNewFiltered(timing)
                ));
        }
        public IEnumerator ExecuteBattleSEs(
            StatusBTLData statusData,
            List<EffectDatabase.StatusBTLEff.BattleSE> effects
            )
        {
            for (int i = 0; i < effects.Count; i++)
            {
                yield return StartCoroutine(ExecuteBattleSE(
                    effect_: effects[i],
                    statusData: statusData
                    ));
            }
        }
        public IEnumerator ExecuteBattleSE(
            EffectDatabase.StatusBTLEff.BattleSE effect_,
            StatusBTLData statusData
            )
        {
            // Block Status
            if (effect_.effectType == BattleSEType.BlockStatus)
            {
                EffectDatabase.StatusBTLEff.BlockStatus effect = effect_ as EffectDatabase.StatusBTLEff.BlockStatus;
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    bool isAffected = true;
                    Pokemon pokemon = allPokemon[i];

                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        // Is Pokemon affected by condition
                        if (isAffected)
                        {
                            if (!battle.BBPIsPokemonAffectedByBS(pokemon: pokemon, statusData: statusData))
                            {
                                isAffected = false;
                            }
                        }

                        /// Filter checks
                        if (isAffected)
                        {
                            if (battle.DoesBattleEFiltersPass(
                                effect: effect,
                                targetPokemon: pokemon
                                ))
                            {
                                isAffected = false;
                            }
                        }
                
                        // Remove all blocked statuses
                        if (isAffected)
                        {
                            List<StatusCondition> statusConditions = battle.PBPGetSCs(pokemon);
                            for (int k = 0; k < statusConditions.Count; k++)
                            {
                                if (effect.statusIDs.Contains(statusConditions[k].statusID))
                                {
                                    yield return StartCoroutine(HealPokemonSC(
                                        targetPokemon: pokemon,
                                        condition: statusConditions[k]
                                        ));
                                }
                            }
                        }
                    }
                }
            }
            // HP Gain (ex. Grassy Terrain)
            if (effect_.effectType == BattleSEType.HPGain)
            {
                EffectDatabase.StatusBTLEff.HPGain effect = effect_ as EffectDatabase.StatusBTLEff.HPGain;
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);

                // create heal events
                List<Pokemon> hitPokemon = new List<Pokemon>();
                List<PBS.Battle.View.Events.PokemonHealthHeal> healEvents = new List<PBS.Battle.View.Events.PokemonHealthHeal>();
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    bool heal = true;
                    Pokemon pokemon = allPokemon[i];

                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        // Is Pokemon affected by condition
                        if (heal)
                        {
                            if (!battle.BBPIsPokemonAffectedByBS(pokemon: pokemon, statusData: statusData))
                            {
                                heal = false;
                            }
                        }

                        /// Filter checks
                        if (heal)
                        {
                            if (!battle.DoesBattleEFiltersPass(
                                effect: effect,
                                targetPokemon: pokemon
                                ))
                            {
                                heal = false;
                            }
                        }

                        // passed all checks, so we hit the pokemon
                        if (heal)
                        {
                            hitPokemon.Add(pokemon);
                            float hpPercentGained = effect.hpGainPercent;

                            int preHP = pokemon.currentHP;
                            int healAmount = battle.GetPokemonHPByPercent(pokemon, hpPercentGained);
                            healAmount = Mathf.Max(1, healAmount);
                            int realHealAmount = battle.AddPokemonHP(pokemon, healAmount);
                            int postHP = pokemon.currentHP;

                            PBS.Battle.View.Events.PokemonHealthHeal hpGainEvent = new PBS.Battle.View.Events.PokemonHealthHeal
                            {
                                pokemonUniqueID = pokemon.uniqueID,
                                preHP = preHP,
                                postHP = postHP,
                                maxHP = pokemon.maxHP
                            };
                            healEvents.Add(hpGainEvent);
                        }
                    }
                }

                // send heal message if any pokemon healed HP
                if (hitPokemon.Count > 0)
                {
                    List<string> pokemonIDs = new List<string>();
                    for (int i = 0; i < hitPokemon.Count; i++)
                    {
                        pokemonIDs.Add(hitPokemon[i].uniqueID);
                    }
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = effect.displayText,
                        pokemonListIDs = pokemonIDs
                    });
                    for (int i = 0; i < healEvents.Count; i++)
                    {
                        SendEvent(healEvents[i]);
                    }
                }
            }
            // HP Loss (ex. weather)
            else if (effect_.effectType == BattleSEType.HPLoss)
            {
                EffectDatabase.StatusBTLEff.HPLoss effect = effect_ as EffectDatabase.StatusBTLEff.HPLoss;
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);

                List<Pokemon> hitPokemon = new List<Pokemon>();
                List<PBS.Battle.View.Events.PokemonHealthDamage> dmgEvents = new List<PBS.Battle.View.Events.PokemonHealthDamage>();
                for (int i = 0; i < allPokemon.Count; i++)
                {
                    bool buffet = true;
                    Pokemon pokemon = allPokemon[i];

                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        // Is Pokemon affected by condition
                        if (buffet)
                        {
                            if (!battle.BBPIsPokemonAffectedByBS(pokemon: pokemon, statusData: statusData))
                            {
                                buffet = false;
                            }
                        }

                        /// type immunities done via <seealso cref="EffectDatabase.Filter.TypeList"/>.
                        if (buffet)
                        {
                            if (!battle.DoesBattleEFiltersPass(
                                effect: effect,
                                targetPokemon: pokemon
                                ))
                            {
                                buffet = false;
                            }
                        }
                    
                        // Overcoat
                        if (buffet)
                        {
                            List<EffectDatabase.AbilityEff.AbilityEffect> overcoat_ =
                                battle.PBPGetAbilityEffects(pokemon, AbilityEffectType.Overcoat);
                            for (int k = 0; k < overcoat_.Count && buffet; k++)
                            {
                                EffectDatabase.AbilityEff.Overcoat overcoat = overcoat_[k] as EffectDatabase.AbilityEff.Overcoat;
                                if (battle.DoEffectFiltersPass(
                                    filters: overcoat.filters,
                                    targetPokemon: pokemon
                                    ))
                                {
                                    // all weather
                                    if (overcoat.allWeather && statusData.GetEffectNew(BattleSEType.Weather) != null)
                                    {
                                        buffet = false;
                                    }

                                    // specific weather
                                    if (buffet && overcoat.conditions.Count > 0)
                                    {
                                        for (int j = 0; j < overcoat.conditions.Count && buffet; j++)
                                        {
                                            if (statusData.ID == overcoat.conditions[j]
                                                || statusData.IsABaseID(overcoat.conditions[j]))
                                            {
                                                buffet = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // passed all checks, so we hit the pokemon
                        if (buffet)
                        {
                            hitPokemon.Add(pokemon);
                            float hpPercentLost = effect.hpLossPercent;

                            int preHP = pokemon.currentHP;
                            int damage = battle.GetPokemonHPByPercent(pokemon, hpPercentLost);
                            damage = Mathf.Max(1, damage);
                            int damageDealt = battle.SubtractPokemonHP(pokemon, damage);
                            int postHP = pokemon.currentHP;

                            PBS.Battle.View.Events.PokemonHealthDamage hpLossEvent = new PBS.Battle.View.Events.PokemonHealthDamage
                            {
                                pokemonUniqueID = pokemon.uniqueID,
                                preHP = preHP,
                                postHP = postHP,
                                maxHP = pokemon.maxHP
                            };
                            dmgEvents.Add(hpLossEvent);
                        }
                    }
                }

                // send buffet message if any pokemon lost HP
                if (hitPokemon.Count > 0)
                {
                    List<string> pokemonIDs = new List<string>();
                    for (int i = 0; i < hitPokemon.Count; i++)
                    {
                        pokemonIDs.Add(hitPokemon[i].uniqueID);
                    }
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = effect.displayText,
                        pokemonListIDs = pokemonIDs,
                        statusEnvironmentID = statusData.ID
                    });
                    for (int i = 0; i < dmgEvents.Count; i++)
                    {
                        SendEvent(dmgEvents[i]);
                    }
                    yield return StartCoroutine(BattleFaintCheck());
                }
            }
            // Intensify Gravity
            else if (effect_.effectType == BattleSEType.Gravity)
            {
                EffectDatabase.StatusBTLEff.Gravity effect = effect_ as EffectDatabase.StatusBTLEff.Gravity;
                List<Pokemon> allPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
                List<Pokemon> hitPokemon = new List<Pokemon>();

                for (int i = 0; i < allPokemon.Count; i++)
                {
                    Pokemon pokemon = allPokemon[i];
                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        if (effect.intensifyGravity)
                        {
                            // Fly
                            if (pokemon.bProps.inFlyState)
                            {
                                if (!hitPokemon.Contains(pokemon))
                                {
                                    pokemon.bProps.inFlyState = false;
                                }
                            }
                            // Next Command
                            PokemonSavedCommand command = pokemon.bProps.nextCommand;
                            if (command != null)
                            {
                                MoveData moveData = MoveDatabase.instance.GetMoveData(command.moveID);
                                if (moveData.HasTag(MoveTag.CannotUseInGravity))
                                {
                                    // Free Sky Drop
                                    if (moveData.GetEffect(MoveEffectType.SkyDrop) != null)
                                    {
                                        for (int k = 0; k < pokemon.bProps.skyDropTargets.Count; k++)
                                        {
                                            Pokemon target = battle.GetFieldPokemonByID(pokemon.bProps.skyDropTargets[k]);
                                            if (target != null)
                                            {
                                                if (battle.IsPokemonOnField(target)
                                                    && !battle.IsPokemonFainted(target))
                                                {
                                                    yield return StartCoroutine(FreePokemonFromSkyDrop(target));
                                                    if (!hitPokemon.Contains(target))
                                                    {
                                                        hitPokemon.Add(target);
                                                    }
                                                }
                                            }
                                        }
                                        pokemon.bProps.attemptingToSkyDrop = false;
                                        pokemon.bProps.skyDropTargets.Clear();
                                    }
                                    pokemon.UnsetNextCommand();
                                    if (!hitPokemon.Contains(pokemon))
                                    {
                                        hitPokemon.Add(pokemon);
                                    }
                                }
                            }
                        }
                    }
                }

                if (hitPokemon.Count > 0)
                {
                    SendEvent(new Battle.View.Events.Message
                    {
                        message = effect.groundedText
                    });
                }

            }

            yield return null;
        }

        public IEnumerator ExecutePokemonAbilityEffect(
            EffectDatabase.AbilityEff.AbilityEffect effect_,
            Pokemon targetPokemon,
            AbilityData abilityData,
            System.Action<bool> callback,
            Pokemon userPokemon = null,
            MoveData moveData = null,
            bool forceFailMessage = false,
            bool apply = true
            )
        {
            bool effectSuccess = false;

            callback(effectSuccess);
            yield return null;
        }

        public IEnumerator ExecuteAfterMoveEvents(
            Pokemon userPokemon,
            List<BattleHitTarget> battleHitTargets,
            BattleCommand command,
            MoveData moveData
            )
        {
            BattlePosition userPosition = battle.GetPokemonPosition(userPokemon);

            // run team protection moves here
            List<BattleTeam> accountedTeams = new List<BattleTeam>();

            // run protection moves here
            for (int k = 0; k < battleHitTargets.Count; k++)
            {
                BattleHitTarget currentTarget = battleHitTargets[k];
                /*if (!string.IsNullOrEmpty(currentTarget.protectMove))
                {
                    MoveData protectData = MoveDatabase.instance.GetMoveData(currentTarget.protectMove);

                    // Spiky Shield
                    MoveEffect spikyShield = protectData.GetEffect(MoveEffectType.SpikyShield);
                    if (!battle.IsPokemonFainted(userPokemon) 
                        && spikyShield != null
                        && !command.isFutureSightMove)
                    {
                        if (!spikyShield.GetBool(0) || moveData.HasTag(MoveTag.MakesContact))
                        {
                            // Damage
                            float damagePercent = spikyShield.GetFloat(0);
                            int preHP = userPokemon.currentHP;
                            int damage = battle.GetPokemonHPByPercent(userPokemon, damagePercent);
                            damage = Mathf.Max(1, damage);
                            int damageDealt = battle.SubtractPokemonHP(userPokemon, damage);
                            int postHP = userPokemon.currentHP;
                            string textID = spikyShield.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-spikyshield-default"
                                : textID;

                            BTLEvent_GameText gameTextEvent = new BTLEvent_GameText();
                            gameTextEvent.SetCloneModel(battle);
                            gameTextEvent.Create(
                                textID: textID,
                                targetPokemon: userPokemon,
                                moveID: currentTarget.protectTeamMove
                                );

                            yield return StartCoroutine(CommonHPChangePokemon(
                                pokemon: userPokemon,
                                preHP: preHP,
                                hpChange: damageDealt,
                                postHP: postHP,
                                textEvent: gameTextEvent
                                ));
                        }
                    }

                    // Baneful Bunker
                    MoveEffect banefulBunker = protectData.GetEffect(MoveEffectType.BanefulBunker);
                    if (!battle.IsPokemonFainted(userPokemon) 
                        && banefulBunker != null
                        && !command.isFutureSightMove)
                    {
                        if (!banefulBunker.GetBool(0) || moveData.HasTag(MoveTag.MakesContact))
                        {
                            // Status
                            string statusID = banefulBunker.GetString(0);
                            int turns = Mathf.FloorToInt(banefulBunker.GetFloat(0));

                            // try to inflict status condition
                            yield return StartCoroutine(TryToInflictPokemonSC(
                                statusID: statusID,
                                targetPokemon: userPokemon,
                                turnsLeft: turns,
                                userPokemon: currentTarget.pokemon,
                                forceFailureMessage: false,
                                callback: (result) =>
                                {

                                }
                                ));
                        }
                    }

                    // King's Shield
                    MoveEffect kingsShield = protectData.GetEffect(MoveEffectType.KingsShield);
                    if (!battle.IsPokemonFainted(userPokemon) 
                        && kingsShield != null
                        && !command.isFutureSightMove)
                    {
                        if (!kingsShield.GetBool(0) || moveData.HasTag(MoveTag.MakesContact))
                        {
                            // Stat change
                            int modValue = Mathf.FloorToInt(kingsShield.GetFloat(0));
                            List<PokemonStats> statsToModify =
                                GameTextDatabase.GetStatsFromList(kingsShield.stringParams);

                            yield return StartCoroutine(TryToApplyStatStageMods(
                                statsToModify: statsToModify,
                                modValue: modValue,
                                targetPokemon: userPokemon,
                                (result) =>
                                {

                                },
                                userPokemon: currentTarget.pokemon
                                ));
                        }
                    }

                }*/
            }

            // run effects for each target #1 (Beak Blast, etc.)
            for (int k = 0; k < battleHitTargets.Count; k++)
            {
                BattleHitTarget currentTarget = battleHitTargets[k];
                if (currentTarget.affectedByMove)
                {
                    // Beak Blast
                    if (!string.IsNullOrEmpty(currentTarget.pokemon.bProps.beakBlastMove)
                        && !command.isFutureSightMove)
                    {
                        MoveData beakBlastData =
                            MoveDatabase.instance.GetMoveData(currentTarget.pokemon.bProps.beakBlastMove);
                        MoveEffect beakBlast = beakBlastData.GetEffect(MoveEffectType.BeakBlast);
                        if (!battle.IsPokemonFainted(userPokemon) && beakBlast != null)
                        {
                            if (!beakBlast.GetBool(0) || moveData.HasTag(MoveTag.MakesContact))
                            {
                                // Status
                                string statusID = beakBlast.GetString(0);
                                int turns = Mathf.FloorToInt(beakBlast.GetFloat(0));

                                // try to inflict status condition
                                yield return StartCoroutine(TryToInflictPokemonSC(
                                    statusID: statusID,
                                    targetPokemon: userPokemon,
                                    turnsLeft: turns,
                                    userPokemon: currentTarget.pokemon,
                                    forceFailureMessage: false,
                                    callback: (result) =>
                                    {

                                    }
                                    ));
                            }
                        }
                    }
                }
            }

            // run ability effects for each target
            // Ex. Iron Barbs, Static, etc.
            for (int i = 0; i < battleHitTargets.Count; i++)
            {
                BattleHitTarget currentTarget = battleHitTargets[i];
                if (currentTarget.affectedByMove)
                {
                    List<Pokemon.Ability> abilities = battle.PBPGetAbilities(currentTarget.pokemon);
                    for (int k = 0; k < abilities.Count; k++)
                    {
                        Pokemon.Ability ability = abilities[k];

                        // Iron Barbs / Rough Skin
                        EffectDatabase.AbilityEff.AbilityEffect roughSkin_ =
                            ability.data.GetEffectNew(AbilityEffectType.RoughSkin);
                        if (roughSkin_ != null
                                && battle.IsPokemonOnFieldAndAble(userPokemon)
                                && !userPokemon.IsTheSameAs(currentTarget.pokemon))
                        {
                            EffectDatabase.AbilityEff.RoughSkin roughSkin =
                                roughSkin_ as EffectDatabase.AbilityEff.RoughSkin;

                            if (battle.DoEffectFiltersPass(
                                filters: roughSkin.filters,
                                userPokemon: currentTarget.pokemon,
                                targetPokemon: userPokemon
                                ))
                            {
                                bool applyRoughSkin = true;

                                // Contact requirement
                                if (applyRoughSkin
                                    && roughSkin.onlyContact
                                    && !battle.DoesMoveMakeContact(
                                        userPokemon: userPokemon,
                                        targetPokemon: currentTarget.pokemon,
                                        moveData: moveData))
                                {
                                    applyRoughSkin = false;
                                }

                                if (applyRoughSkin)
                                {
                                    int damage = battle.GetDamage(
                                        damage: roughSkin.damage,
                                        targetPokemon: userPokemon,
                                        attackerPokemon: currentTarget.pokemon
                                        );

                                    PBPShowAbility(currentTarget.pokemon, ability);
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = roughSkin.damage.displayText,
                                        pokemonUserID = currentTarget.pokemon.uniqueID,
                                        pokemonTargetID = userPokemon.uniqueID
                                    });

                                    yield return StartCoroutine(PBPDamagePokemon(
                                        pokemon: userPokemon,
                                        HPToLose: damage
                                        ));
                                }
                            }
                        }

                        // Cute Charm / Flame Body / Static
                        EffectDatabase.AbilityEff.AbilityEffect flameBody_ =
                            abilities[k].data.GetEffectNew(AbilityEffectType.FlameBody);
                        if (flameBody_ != null && battle.IsPokemonOnFieldAndAble(userPokemon))
                        {
                            EffectDatabase.AbilityEff.FlameBody flameBody = 
                                flameBody_ as EffectDatabase.AbilityEff.FlameBody;
                            if (Random.value <= flameBody.chance
                                && battle.DoEffectFiltersPass(
                                    filters: flameBody.filters,
                                    userPokemon: currentTarget.pokemon,
                                    targetPokemon: userPokemon,
                                    moveData: moveData
                                    ))
                            {
                                EffectDatabase.General.InflictStatus inflictStatus = flameBody.inflictStatus;

                                // Effect Spore
                                if (flameBody.effectSpores.Count > 0)
                                {
                                    inflictStatus = flameBody.GetAnEffectSporeStatus();
                                }

                                if (inflictStatus != null)
                                {
                                    PBPShowAbility(currentTarget.pokemon, abilities[k].data);

                                    bool inflictSuccess = false;
                                    yield return StartCoroutine(ApplySC(
                                        inflictStatus: inflictStatus,
                                        targetPokemon: userPokemon,
                                        userPokemon: currentTarget.pokemon,
                                        forceFailMessage: true,
                                        callback: (result) => 
                                        {
                                            inflictSuccess = result;
                                        }
                                        ));

                                    // inflict self
                                    if (flameBody.perishBody && inflictSuccess)
                                    {
                                        yield return StartCoroutine(ApplySC(
                                        inflictStatus: inflictStatus,
                                        targetPokemon: currentTarget.pokemon,
                                        userPokemon: currentTarget.pokemon,
                                        forceFailMessage: true,
                                        callback: (result) => { }
                                        ));
                                    }
                                }
                            }
                        }

                        // Gooey / Tangling Hair / Cotton Down
                        EffectDatabase.AbilityEff.AbilityEffect gooey_ =
                            abilities[k].data.GetEffectNew(AbilityEffectType.Gooey);
                        if (gooey_ != null)
                        {
                            EffectDatabase.AbilityEff.Gooey gooey = gooey_ as EffectDatabase.AbilityEff.Gooey;

                            bool trigger = true;

                            // Only Damaging
                            if (trigger && gooey.onlyDamaging
                                && !battle.IsMoveDamaging(moveData))
                            {
                                trigger = false;
                            }
                        
                            // Only Contact
                            if (trigger && gooey.triggerTags.Count > 0)
                            {
                                trigger = false;
                                List<MoveTag> tags = new List<MoveTag>(gooey.triggerTags);
                                for (int j = 0; j < tags.Count && !trigger; j++)
                                {
                                    if (moveData.HasTag(tags[j]))
                                    {
                                        trigger = true;
                                    }
                                }
                            }

                            // Execute Gooey
                            if (trigger)
                            {
                                List<Pokemon> appliedPokemon = new List<Pokemon>();
                                if (gooey.cottonDown)
                                {
                                    appliedPokemon = battle.GetPokemonUnfaintedFrom(battle.pokemonOnField);
                                    appliedPokemon.Remove(currentTarget.pokemon);
                                }
                                else
                                {
                                    if (battle.IsPokemonOnFieldAndAble(userPokemon))
                                    {
                                        appliedPokemon.Add(userPokemon);
                                    }
                                }

                                if (appliedPokemon.Count > 0)
                                {
                                    PBPShowAbility(currentTarget.pokemon, abilities[k].data);

                                    for (int j = 0; j < appliedPokemon.Count; j++)
                                    {
                                        yield return StartCoroutine(ApplyStatStageMod(
                                            targetPokemon: appliedPokemon[j],
                                            userPokemon: currentTarget.pokemon,
                                            statStageMod: gooey.statStageMod,
                                            forceFailureMessage: true,
                                            callback: (result) => { }
                                            ));
                                    }
                                }
                            }
                        }

                        // Illusion
                        if (currentTarget.pokemon.bProps.illusion != null)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect illusion_ =
                                abilities[k].data.GetEffectNew(AbilityEffectType.Illusion);
                            if (illusion_ != null)
                            {
                                EffectDatabase.AbilityEff.Illusion illusion =
                                    illusion_ as EffectDatabase.AbilityEff.Illusion;

                                PBPShowAbility(currentTarget.pokemon, abilities[k]);
                                currentTarget.pokemon.bProps.illusion = null;
                            }
                        }
                    }
                }
            }

            // Mummy / Wandering Spirit
            bool triggeredWanderingSpirit = false;
            for (int i = 0; i < battleHitTargets.Count && !triggeredWanderingSpirit; i++)
            {
                BattleHitTarget currentTarget = battleHitTargets[i];
                List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(userPokemon);

                if (currentTarget.affectedByMove && battle.IsPokemonOnField(currentTarget.pokemon))
                {
                    List<Pokemon.Ability> targetAbilities = battle.PBPGetAbilities(currentTarget.pokemon);
                    for (int k = 0; k < targetAbilities.Count && !triggeredWanderingSpirit; k++)
                    {
                        Pokemon.Ability ability = targetAbilities[k];
                        EffectDatabase.AbilityEff.AbilityEffect mummy_ = 
                            ability.data.GetEffectNew(AbilityEffectType.Mummy);
                        if (mummy_ != null)
                        {
                            EffectDatabase.AbilityEff.Mummy mummy = mummy_ as EffectDatabase.AbilityEff.Mummy;
                            if (battle.DoEffectFiltersPass(
                                filters: mummy.filters,
                                userPokemon: userPokemon,
                                targetPokemon: currentTarget.pokemon,
                                moveData: moveData
                                ))
                            {
                                List<Pokemon.Ability> targetSetAbilities = new List<Pokemon.Ability>();
                                List<Pokemon.Ability> userSetAbilities = new List<Pokemon.Ability>();

                                // Set abilities
                                if (mummy.setAbilities != null)
                                {
                                    for (int j = 0; j < mummy.setAbilities.Count; j++)
                                    {
                                        userSetAbilities.Add(new Pokemon.Ability(mummy.setAbilities[j]));
                                    }
                                }
                                else
                                {
                                    // Copy ability
                                    userSetAbilities.Add(ability.Clone());
                                }

                                // Swap abilities
                                if (mummy.wanderingSpirit)
                                {
                                    if (battle.IsPokemonOnFieldAndAble(userPokemon))
                                    {
                                        for (int j = 0; j < userAbilities.Count; j++)
                                        {
                                            targetSetAbilities.Add(userAbilities[j].Clone());
                                        }
                                    }
                                }

                                bool applyMummy = false;

                                // Check if user can have replaced abilities
                                if (!applyMummy
                                    && userAbilities.Count > 0
                                    && battle.PBPGetAbilitiesReplaceable(
                                        userPokemon, 
                                        userSetAbilities).Count > 0)
                                {
                                    applyMummy = true;
                                }

                                // Check if the target can swap abilities
                                if (applyMummy && mummy.wanderingSpirit)
                                {
                                    applyMummy = false;
                                    if (targetSetAbilities.Count > 0
                                        && battle.PBPGetAbilitiesReplaceable(
                                            currentTarget.pokemon,
                                            targetSetAbilities).Count > 0)
                                    {
                                        applyMummy = true;

                                    }
                                }

                                // Apply Mummy
                                if (applyMummy)
                                {
                                    PBPShowAbility(currentTarget.pokemon, ability);
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = mummy.displayText,
                                        pokemonUserID = currentTarget.pokemon.uniqueID,
                                        pokemonTargetID = userPokemon.uniqueID
                                    });

                                    battle.PBPSetAbilitiesReplaceable(
                                        userPokemon, 
                                        userSetAbilities);

                                    // Wandering Spirit
                                    if (mummy.wanderingSpirit)
                                    {
                                        triggeredWanderingSpirit = true;
                                        battle.PBPSetAbilitiesReplaceable(
                                            currentTarget.pokemon, 
                                            targetSetAbilities);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Role Play
            /*if (moveData.GetEffect(MoveEffectType.RolePlay) != null
                && battle.IsPokemonOnFieldAndAble(userPokemon))
            {
                AbilityData userAbilityData = battle.PBPGetAbilityData(userPokemon);
                bool canRolePlay = true;
                if (userAbilityData != null)
                {
                    if (userAbilityData.HasTag(AbilityTag.CannotRolePlayUser))
                    {
                        canRolePlay = false;
                    }
                }

                if (canRolePlay)
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.RolePlay);
                    Pokemon enablerPokemon = null;
                    AbilityData copyAbilityData = null;

                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove)
                        {
                            AbilityData abilityData = battle.PBPGetAbilityData(currentTarget.pokemon);
                            if (abilityData != null)
                            {
                                if (!abilityData.HasTag(AbilityTag.CannotRolePlay))
                                {
                                    enablerPokemon = currentTarget.pokemon;
                                    copyAbilityData = abilityData;
                                    break;
                                }
                            }
                        }
                    }

                    if (enablerPokemon != null)
                    {
                        userPokemon.bProps.abilityOverride = copyAbilityData.ID;

                        if (effect.GetBool(1))
                        {
                            // TODO: Activate ability?
                        }

                        string textID = effect.GetString(0);
                        textID = (textID == "DEFAULT") ? "move-roleplay" : textID;

                        BTLEvent_GameText textEvent = new BTLEvent_GameText();
                        textEvent.SetCloneModel(battle);
                        textEvent.Create(
                            textID: textID,
                            userPokemon: userPokemon,
                            targetPokemon: enablerPokemon,
                            abilityID: copyAbilityData.ID
                            );
                        SendEvent(textEvent);
                    }
                }
            }*/

            /*/ Skill Swap
            if (moveData.GetEffect(MoveEffectType.SkillSwap) != null
                && battle.IsPokemonOnFieldAndAble(userPokemon))
            {
                AbilityData userAbilityData = battle.PBPGetAbilityData(userPokemon);
                bool canSkillSwap = true;
                if (userAbilityData != null)
                {
                    if (userAbilityData.HasTag(AbilityTag.CannotSkillSwapUser))
                    {
                        canSkillSwap = false;
                    }
                }
                else
                {
                    canSkillSwap = false;
                }

                if (canSkillSwap)
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.SkillSwap);
                    Pokemon enablerPokemon = null;
                    AbilityData copyAbilityData = null;

                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove
                            && battle.IsPokemonOnFieldAndAble(currentTarget.pokemon))
                        {
                            AbilityData abilityData = battle.PBPGetAbilityData(currentTarget.pokemon);
                            if (abilityData != null)
                            {
                                if (!abilityData.HasTag(AbilityTag.CannotSkillSwap))
                                {
                                    enablerPokemon = currentTarget.pokemon;
                                    copyAbilityData = abilityData;
                                    break;
                                }
                            }
                        }
                    }

                    if (enablerPokemon != null)
                    {
                        userPokemon.bProps.abilityOverride = copyAbilityData.ID;
                        enablerPokemon.bProps.abilityOverride = userAbilityData.ID;

                        if (effect.GetBool(1))
                        {
                            // TODO: Activate ability?
                        }

                        string textID = effect.GetString(0);
                        textID = (textID == "DEFAULT") ? "move-skillswap" : textID;

                        BTLEvent_GameText textEvent = new BTLEvent_GameText();
                        textEvent.SetCloneModel(battle);
                        textEvent.Create(
                            textID: textID,
                            userPokemon: userPokemon,
                            targetPokemon: enablerPokemon
                            );
                        SendEvent(textEvent);
                    }
                }
            }*/

            // Check Red Card

            // Run effects on non-Sky Drop pick-up turn
            if (!userPokemon.bProps.attemptingToSkyDrop)
            {
                // Power Split / Guard Split
                if (moveData.GetEffect(MoveEffectType.PowerSplit) != null
                    && battle.IsPokemonOnFieldAndAble(userPokemon))
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.PowerSplit);
                    Dictionary<PokemonStats, int> totalStatMap = new Dictionary<PokemonStats, int>();
                    List<PokemonStats> statsToMod = GameTextDatabase.GetStatsFromList(effect.stringParams);

                    List<Pokemon> pokemonToSplit = new List<Pokemon> { userPokemon };
                    for (int i = 0; i < statsToMod.Count; i++)
                    {
                        totalStatMap[statsToMod[i]]
                            = Mathf.FloorToInt(battle.GetPokemonStat(userPokemon, statsToMod[i], false, false));
                    }

                    for (int i = 0; i < battleHitTargets.Count; i++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[i];
                        if (currentTarget.affectedByMove)
                        {
                            Pokemon curPokemon = currentTarget.pokemon;
                            pokemonToSplit.Add(curPokemon);
                            for (int k = 0; k < statsToMod.Count; k++)
                            {
                                totalStatMap[statsToMod[k]]
                                    += Mathf.FloorToInt(battle.GetPokemonStat(curPokemon, statsToMod[k], false, false));
                            }
                        }
                    }

                    if (pokemonToSplit.Count > 1)
                    {
                        for (int i = 0; i < pokemonToSplit.Count; i++)
                        {
                            if (statsToMod.Contains(PokemonStats.Attack))
                            {
                                pokemonToSplit[i].bProps.ATKOverride
                                    = Mathf.FloorToInt((float)totalStatMap[PokemonStats.Attack] / pokemonToSplit.Count);
                            }
                            if (statsToMod.Contains(PokemonStats.Defense))
                            {
                                pokemonToSplit[i].bProps.DEFOverride
                                    = Mathf.FloorToInt((float)totalStatMap[PokemonStats.Defense] / pokemonToSplit.Count);
                            }
                            if (statsToMod.Contains(PokemonStats.SpecialAttack))
                            {
                                pokemonToSplit[i].bProps.SPAOverride
                                    = Mathf.FloorToInt((float)totalStatMap[PokemonStats.SpecialAttack] / pokemonToSplit.Count);
                            }
                            if (statsToMod.Contains(PokemonStats.SpecialDefense))
                            {
                                pokemonToSplit[i].bProps.SPDOverride
                                    = Mathf.FloorToInt((float)totalStatMap[PokemonStats.SpecialDefense] / pokemonToSplit.Count);
                            }
                            if (statsToMod.Contains(PokemonStats.Speed))
                            {
                                pokemonToSplit[i].bProps.SPEOverride
                                    = Mathf.FloorToInt((float)totalStatMap[PokemonStats.Speed] / pokemonToSplit.Count);
                            }
                        }

                        string textID = "move-powersplit";
                        MoveEffect textEffect = moveData.GetEffect(MoveEffectType.PowerSplitText);
                        if (textEffect != null)
                        {
                            textID = textEffect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-powersplit" : textID;
                        }
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = textID,
                            pokemonUserID = userPokemon.uniqueID,
                            statList = new List<PokemonStats>(statsToMod)
                        });
                    }
                }

                // Power Swap / Guard Swap
                if (moveData.GetEffect(MoveEffectType.PowerSwap) != null
                    && battle.IsPokemonOnFieldAndAble(userPokemon))
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.PowerSwap);
                    Pokemon swapPokemon = null;
                    List<PokemonStats> statsToMod = GameTextDatabase.GetStatsFromList(effect.stringParams);

                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove)
                        {
                            swapPokemon = currentTarget.pokemon;
                            break;
                        }
                    }

                    if (swapPokemon != null)
                    {
                        for (int i = 0; i < statsToMod.Count; i++)
                        {
                            int stage1 = battle.GetPokemonStatStage(userPokemon, statsToMod[i]);
                            int stage2 = battle.GetPokemonStatStage(swapPokemon, statsToMod[i]);

                            battle.SetPokemonStatStage(userPokemon, statsToMod[i], stage2);
                            battle.SetPokemonStatStage(swapPokemon, statsToMod[i], stage1);
                        }

                        string textID = "move-powerswap";
                        MoveEffect textEffect = moveData.GetEffect(MoveEffectType.PowerSwapText);
                        if (textEffect != null)
                        {
                            textID = textEffect.GetString(0);
                            textID = (textID == "DEFAULT") ? "move-powerswap" : textID;
                        }
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = textID,
                            pokemonUserID = userPokemon.uniqueID,
                            statList = new List<PokemonStats>(statsToMod)
                        });
                    }
                }

                // Stat Stage Abilities
                for (int i = 0; i < battleHitTargets.Count; i++)
                {
                    BattleHitTarget currentTarget = battleHitTargets[i];
                    if (battle.IsPokemonOnFieldAndAble(currentTarget.pokemon))
                    {
                        List<AbilityData> abilities = battle.PBPGetAbilityDatas(currentTarget.pokemon);

                        // Anger Point
                        if (currentTarget.criticalHit)
                        {
                            for (int k = 0; k < abilities.Count; k++)
                            {
                                EffectDatabase.AbilityEff.AbilityEffect angerPoint_ =
                                    abilities[k].GetEffectNew(AbilityEffectType.AngerPoint);
                                if (angerPoint_ != null)
                                {
                                    EffectDatabase.AbilityEff.AngerPoint angerPoint =
                                        angerPoint_ as EffectDatabase.AbilityEff.AngerPoint;

                                    PBPShowAbility(currentTarget.pokemon, abilities[k]);
                                    yield return StartCoroutine(ApplyStatStageMod(
                                        userPokemon: currentTarget.pokemon,
                                        targetPokemon: currentTarget.pokemon,
                                        statStageMod: angerPoint.statStageMod,
                                        forceFailureMessage: true,
                                        callback: (result) => { }
                                        ));
                                }
                            }
                        }

                        // Berserk
                        if (currentTarget.affectedByMove)
                        {
                            for (int k = 0; k < abilities.Count; k++)
                            {
                                EffectDatabase.AbilityEff.AbilityEffect berserk_ =
                                    abilities[k].GetEffectNew(AbilityEffectType.Berserk);
                                if (berserk_ != null)
                                {
                                    EffectDatabase.AbilityEff.Berserk berserk = berserk_ as EffectDatabase.AbilityEff.Berserk;
                                    if (currentTarget.preHPPercent > berserk.hpThreshold
                                        && currentTarget.postHPPercent <= berserk.hpThreshold)
                                    {
                                        PBPShowAbility(currentTarget.pokemon, abilities[k]);
                                        yield return StartCoroutine(ApplyStatStageMod(
                                            userPokemon: currentTarget.pokemon,
                                            targetPokemon: currentTarget.pokemon,
                                            statStageMod: berserk.statStageMod,
                                            forceFailureMessage: true,
                                            callback: (result) => { }
                                            ));
                                    }
                                }
                            }
                        }

                        // Justified
                        if (currentTarget.affectedByMove)
                        {
                            for (int k = 0; k < abilities.Count; k++)
                            {
                                EffectDatabase.AbilityEff.AbilityEffect justified_ =
                                    abilities[k].GetEffectNew(AbilityEffectType.Justified);
                                if (justified_ != null)
                                {
                                    EffectDatabase.AbilityEff.Justified justified =
                                        justified_ as EffectDatabase.AbilityEff.Justified;
                                    if (battle.DoEffectFiltersPass(
                                        filters: justified.filters,
                                        userPokemon: userPokemon,
                                        targetPokemon: currentTarget.pokemon,
                                        moveData: moveData
                                        ))
                                    {
                                        bool applyJustified = true;

                                        // Only Damaging
                                        if (applyJustified
                                            && justified.onlyDamaging
                                            && !battle.IsMoveDamaging(moveData))
                                        {
                                            applyJustified = true;
                                        }

                                        // Weak Armor
                                        if (applyJustified
                                            && justified.mustMatchCategory
                                            && moveData.category != justified.category)
                                        {
                                            applyJustified = false;
                                        }

                                        if (applyJustified)
                                        {
                                            PBPShowAbility(currentTarget.pokemon, abilities[k]);
                                            yield return StartCoroutine(ApplyStatStageMod(
                                                userPokemon: currentTarget.pokemon,
                                                targetPokemon: currentTarget.pokemon,
                                                statStageMod: justified.statStageMod,
                                                forceFailureMessage: true,
                                                callback: (result) => { }
                                                ));
                                        }
                                    }
                                }
                            }
                        }

                        // Rage
                    }
                }

                // Run Gulp Missile
                if (battle.IsPokemonOnFieldAndAble(userPokemon))
                {
                    for (int i = 0; i < battleHitTargets.Count; i++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[i];
                        if (currentTarget.affectedByMove
                            && battle.IsPokemonOnField(currentTarget.pokemon)
                            && currentTarget.pokemon.bProps.gulpMissile != null
                            && battle.IsPokemonOnFieldAndAble(userPokemon))
                        {
                            EffectDatabase.AbilityEff.GulpMissile.Missile missile = currentTarget.pokemon.bProps.gulpMissile;
                            int damage = battle.GetPokemonHPByPercent(userPokemon, missile.hpLossPercent);

                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = missile.displayText,
                                pokemonUserID = currentTarget.pokemon.uniqueID,
                                pokemonTargetID = userPokemon.uniqueID
                            });

                            // deal damage
                            yield return StartCoroutine(PBPDamagePokemon(
                                pokemon: userPokemon,
                                HPToLose: damage
                                ));

                            // inflict status effect
                            if (battle.IsPokemonOnFieldAndAble(userPokemon)
                                && missile.inflictStatus != null)
                            {
                                yield return StartCoroutine(ApplySC(
                                    inflictStatus: missile.inflictStatus,
                                    targetPokemon: userPokemon,
                                    userPokemon: currentTarget.pokemon,
                                    callback: (result) => { }
                                    ));
                            }

                            // inflict stat stage mod
                            if (battle.IsPokemonOnFieldAndAble(userPokemon)
                                && missile.statStageMod != null)
                            {
                                yield return StartCoroutine(ApplyStatStageMod(
                                    statStageMod: missile.statStageMod,
                                    targetPokemon: userPokemon,
                                    userPokemon: currentTarget.pokemon,
                                    callback: (result) => { }
                                    ));
                            }

                            // Change Form Back
                            currentTarget.pokemon.bProps.gulpMissile = null;
                            List<Pokemon.Ability> targetAbilities = battle.PBPGetAbilities(currentTarget.pokemon);
                            bool changedForm = false;
                            for (int k = 0; k < targetAbilities.Count && !changedForm; k++)
                            {
                                Pokemon.Ability ability = targetAbilities[k];
                                List<EffectDatabase.AbilityEff.AbilityEffect> gulpMissiles_ =
                                    ability.data.GetEffectsNew(AbilityEffectType.GulpMissile);
                                for (int j = 0; j < gulpMissiles_.Count && !changedForm; j++)
                                {
                                    EffectDatabase.AbilityEff.GulpMissile gulpMissile =
                                        gulpMissiles_[j] as EffectDatabase.AbilityEff.GulpMissile;
                                    for (int l = 0; l < gulpMissile.spitUpTransformations.Count && !changedForm; l++)
                                    {
                                        if (gulpMissile.spitUpTransformations[l].IsPokemonAPreForm(currentTarget.pokemon))
                                        {
                                            changedForm = true;
                                            PBS.Battle.View.Events.MessageParameterized formText = new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = gulpMissile.spitUpText,
                                                pokemonUserID = currentTarget.pokemon.uniqueID
                                            };

                                            PBPShowAbility(pokemon: currentTarget.pokemon, ability: ability);
                                            yield return StartCoroutine(PBPChangeForm(
                                                pokemon: currentTarget.pokemon,
                                                toForm: gulpMissile.spitUpTransformations[l].toForm,
                                                textEvent: formText
                                                ));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Color Change
                for (int i = 0; i < battleHitTargets.Count; i++)
                {
                    BattleHitTarget currentTarget = battleHitTargets[i];
                    if (currentTarget.affectedByMove && battle.IsPokemonOnFieldAndAble(currentTarget.pokemon))
                    {
                        List<Pokemon.Ability> abilites = battle.PBPGetAbilities(currentTarget.pokemon);
                        for (int k = 0; k < abilites.Count; k++)
                        {
                            // Color Change
                            EffectDatabase.AbilityEff.AbilityEffect colorChange_ =
                                abilites[k].data.GetEffectNew(AbilityEffectType.ColorChange);
                            if (colorChange_ != null)
                            {
                                EffectDatabase.AbilityEff.ColorChange colorChange =
                                    colorChange_ as EffectDatabase.AbilityEff.ColorChange;

                                bool changeType = true;
                                string toType = moveData.moveType;

                                if (changeType &&
                                    battle.AreTypesContained(toType, battle.PBPGetTypes(currentTarget.pokemon)))
                                {
                                    changeType = false;
                                }

                                if (changeType)
                                {
                                    PBPShowAbility(userPokemon, abilites[k].data);
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = colorChange.displayText,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID,
                                        typeID = toType
                                    });
                                }
                            }

                        }
                    }
                }

                // Worry Seed
                EffectDatabase.MoveEff.MoveEffect worrySeed_ =
                    moveData.GetEffectNew(MoveEffectType.WorrySeed);
                if (worrySeed_ != null)
                {
                    EffectDatabase.MoveEff.WorrySeed worrySeed = worrySeed_ as EffectDatabase.MoveEff.WorrySeed;
                    List<Pokemon.Ability> worrySeedAbilities = new List<Pokemon.Ability>();
                    if (worrySeed.abilities.Count > 0)
                    {
                        for (int i = 0; i < worrySeed.abilities.Count; i++)
                        {
                            Pokemon.Ability ability = new Pokemon.Ability(
                                abilityID: worrySeed.abilities[i]
                                );
                            worrySeedAbilities.Add(ability);
                        }
                    }
                    if (worrySeed.entrainment)
                    {
                        List<Pokemon.Ability> pbAbilities = battle.PBPGetAbilities(
                            pokemon: userPokemon,
                            ignoreSuppression: true
                            );
                        for (int i = 0; i < pbAbilities.Count; i++)
                        {
                            Pokemon.Ability ability = new Pokemon.Ability(
                                abilityID: pbAbilities[i].abilityID
                                );
                            worrySeedAbilities.Add(ability);
                        }
                    }
                    if (worrySeedAbilities.Count > 0)
                    {
                        for (int i = 0; i < battleHitTargets.Count; i++)
                        {
                            BattleHitTarget currentTarget = battleHitTargets[i];
                            if (currentTarget.affectedByMove
                                && battle.IsPokemonOnFieldAndAble(currentTarget.pokemon))
                            {
                                List<Pokemon.Ability> replaceableAbilities =
                                    battle.PBPGetAbilitiesReplaceable(
                                        pokemon: currentTarget.pokemon,
                                        worrySeedAbilities: worrySeedAbilities);
                                // Success
                                if (replaceableAbilities.Count > 0
                                    || currentTarget.pokemon.bProps.abilities.Count == 0)
                                {
                                    List<Pokemon.Ability> setAbilities =
                                        battle.PBPSetAbilitiesReplaceable(
                                            pokemon: currentTarget.pokemon,
                                            worrySeedAbilities: worrySeedAbilities
                                            );

                                    // Show Lost Abilities
                                    for (int k = 0; k < replaceableAbilities.Count; k++)
                                    {
                                        PBPShowAbility(currentTarget.pokemon, replaceableAbilities[k]);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = worrySeed.loseText,
                                            pokemonTargetID = currentTarget.pokemon.uniqueID,
                                            abilityID = replaceableAbilities[k].abilityID
                                        });
                                    }

                                    // Show Gained Abilities
                                    for (int k = 0; k < setAbilities.Count; k++)
                                    {
                                        PBPShowAbility(currentTarget.pokemon, setAbilities[k]);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = worrySeed.gainText,
                                            pokemonTargetID = currentTarget.pokemon.uniqueID,
                                            abilityID = setAbilities[k].abilityID
                                        });
                                    }
                                }
                                // Failure
                                else
                                {
                                    for (int k = 0; k < worrySeedAbilities.Count; k++)
                                    {
                                        PBPShowAbility(currentTarget.pokemon, worrySeedAbilities[k]);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = worrySeed.failText,
                                            pokemonTargetID = currentTarget.pokemon.uniqueID,
                                            abilityID = worrySeedAbilities[k].abilityID
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                // Core Enforcer
                EffectDatabase.MoveEff.MoveEffect coreEnforcer_ =
                    moveData.GetEffectNew(
                        effectType: MoveEffectType.CoreEnforcer,
                        forceUnique: true);
                if (coreEnforcer_ != null)
                {
                    EffectDatabase.MoveEff.CoreEnforcer coreEnforcer = coreEnforcer_ as EffectDatabase.MoveEff.CoreEnforcer;
                    for (int i = 0; i < battleHitTargets.Count; i++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[i];
                        if (currentTarget.affectedByMove)
                        {
                            yield return StartCoroutine(ExecuteMoveEffect(
                                effect_: coreEnforcer,
                                moveData: moveData,
                                userPokemon: userPokemon,
                                targetPokemon: currentTarget.pokemon,
                                callback: (result) => { }
                                ));
                        }
                    }
                }

                // Bug Bite
                if (moveData.GetEffect(MoveEffectType.BugBite) != null
                    && !command.isFutureSightMove)
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.BugBite);
                    string textID = effect.GetString(0);
                    textID = (textID == "DEFAULT") ? "move-bugbite-default"
                        : textID;

                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove)
                        {
                            Item targetItem = battle.PBPGetHeldItem(currentTarget.pokemon);
                            if (targetItem != null && !battle.IsPokemonFainted(userPokemon))
                            {
                                if (targetItem.data.pocket == ItemPocket.Berries
                                    && battle.CanPokemonItemBeLost(currentTarget.pokemon, targetItem))
                                {
                                    yield return StartCoroutine(ConsumeItem(
                                        pokemon: userPokemon,
                                        item: targetItem,
                                        holderPokemon: currentTarget.pokemon,
                                        consumeText: textID,
                                        callback: (result) =>
                                        {

                                        }
                                        ));
                                }
                            }
                        }
                    }
                }

                // Covet
                EffectDatabase.MoveEff.MoveEffect covet_ = moveData.GetEffectNew(MoveEffectType.Covet, true);
                if (covet_ != null)
                {
                    yield return StartCoroutine(ExecuteMoveEffects(
                        userPokemon: userPokemon,
                        moveData: moveData,
                        effects: new List<EffectDatabase.MoveEff.MoveEffect> { covet_ },
                        battleHitTargets: battleHitTargets,
                        targetTeams: new List<BattleTeam>(),
                        callback: (result) => { }
                        ));
                }
            
                // Magician
                if (battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.Magician) != null)
                {
                    List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(userPokemon);
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove)
                        {
                            Item userItem = battle.PBPGetHeldItem(userPokemon);
                            Item targetItem = battle.PBPGetHeldItem(currentTarget.pokemon);
                            if (targetItem != null
                                && userItem == null
                                && battle.IsPokemonOnFieldAndAble(userPokemon))
                            {
                                if (battle.CanPokemonItemBeLost(currentTarget.pokemon, targetItem)
                                    && battle.CanPokemonItemBeGained(userPokemon, targetItem))
                                {
                                    bool itemStolen = false;
                                    for (int l = 0; l < userAbilities.Count && !itemStolen; l++)
                                    {
                                        Pokemon.Ability ability = userAbilities[l];
                                        EffectDatabase.AbilityEff.AbilityEffect magician_ =
                                            ability.data.GetEffectNew(AbilityEffectType.Magician);
                                        if (magician_ != null)
                                        {
                                            EffectDatabase.AbilityEff.Magician magician =
                                                magician_ as EffectDatabase.AbilityEff.Magician;
                                            if (battle.DoEffectFiltersPass(
                                                filters: magician.filters,
                                                userPokemon: userPokemon,
                                                targetPokemon: currentTarget.pokemon,
                                                moveData: moveData
                                                ))
                                            {
                                                itemStolen = true;

                                                currentTarget.pokemon.UnsetItem(targetItem);
                                                userPokemon.SetItem(targetItem);

                                                PBPShowAbility(userPokemon, ability);
                                                SendEvent(new Battle.View.Events.MessageParameterized
                                                {
                                                    messageCode = magician.displayText,
                                                    pokemonUserID = userPokemon.uniqueID,
                                                    pokemonTargetID = currentTarget.pokemon.uniqueID,
                                                    itemID = targetItem.itemID
                                                });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Pickpocket
                for (int i = 0; i < battleHitTargets.Count; i++)
                {
                    BattleHitTarget currentTarget = battleHitTargets[i];
                    if (currentTarget.affectedByMove && battle.IsPokemonOnFieldAndAble(currentTarget.pokemon))
                    {
                        Item userItem = battle.PBPGetHeldItem(userPokemon);
                        Item targetItem = battle.PBPGetHeldItem(currentTarget.pokemon);
                        if (targetItem == null && userItem != null)
                        {
                            if (battle.CanPokemonItemBeLost(userPokemon, userItem)
                                && battle.CanPokemonItemBeGained(currentTarget.pokemon, userItem))
                            {
                                List<Pokemon.Ability> abilities = battle.PBPGetAbilities(currentTarget.pokemon);
                                for (int k = 0; k < abilities.Count; k++)
                                {
                                    Pokemon.Ability ability = abilities[k];
                                    EffectDatabase.AbilityEff.AbilityEffect pickpocket_ =
                                        abilities[k].data.GetEffectNew(AbilityEffectType.Pickpocket);
                                    if (pickpocket_ != null)
                                    {
                                        EffectDatabase.AbilityEff.Pickpocket pickpocket =
                                            pickpocket_ as EffectDatabase.AbilityEff.Pickpocket;
                                        if (battle.DoEffectFiltersPass(
                                            filters: pickpocket.filters,
                                            userPokemon: currentTarget.pokemon,
                                            targetPokemon: userPokemon
                                            ))
                                        {
                                            currentTarget.pokemon.UnsetItem(targetItem);
                                            userPokemon.SetItem(targetItem);

                                            PBPShowAbility(userPokemon, ability);
                                            SendEvent(new Battle.View.Events.MessageParameterized
                                            {
                                                messageCode = pickpocket.displayText,
                                                pokemonUserID = currentTarget.pokemon.uniqueID,
                                                pokemonTargetID = userPokemon.uniqueID,
                                                itemID = userItem.itemID
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Fling
                if (moveData.GetEffect(MoveEffectType.Fling) != null
                    && !command.isFutureSightMove)
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.Fling);
                    Item userItem = battle.GetPokemonItemFiltered(userPokemon, ItemEffectType.Fling);
                    bool lostItem = false;
                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        if (userItem != null)
                        {
                            if (battle.CanPokemonItemBeLost(userPokemon, userItem))
                            {
                                BattleHitTarget currentTarget = battleHitTargets[k];
                                if (currentTarget.affectedByMove)
                                {
                                    userPokemon.UnsetItem(userItem);
                                    lostItem = true;
                                    if (!battle.IsPokemonFainted(currentTarget.pokemon))
                                    {
                                        yield return StartCoroutine(ConsumeItem(
                                        pokemon: currentTarget.pokemon,
                                        item: userItem,
                                        consumeText: null,
                                        callback: (result) =>
                                        {

                                        }
                                        ));
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // Get rid of item if we haven't already
                    if (!lostItem)
                    {
                        userPokemon.UnsetItem(userItem);
                    }
                }

                // Incinerate
                if (moveData.GetEffect(MoveEffectType.Incinerate) != null)
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.Incinerate);
                    string textID = effect.GetString(0);
                    textID = (textID == "DEFAULT") ? "move-incinerate-default"
                        : textID;

                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove)
                        {
                            Item targetItem = battle.PBPGetHeldItem(currentTarget.pokemon);
                            if (targetItem != null)
                            {
                                if (targetItem.data.pocket == ItemPocket.Berries
                                    && battle.CanPokemonItemBeLost(currentTarget.pokemon, targetItem))
                                {
                                    currentTarget.pokemon.UnsetItem(targetItem);
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID,
                                        itemID = targetItem.itemID
                                    });
                                }
                            }
                        }
                    }
                }

                // Knock Off
                EffectDatabase.MoveEff.MoveEffect knockOff_ = moveData.GetEffectNew(MoveEffectType.KnockOff);
                if (knockOff_ != null)
                {
                    yield return StartCoroutine(ExecuteMoveEffects(
                        userPokemon: userPokemon,
                        moveData: moveData,
                        effects: new List<EffectDatabase.MoveEff.MoveEffect> { knockOff_ },
                        battleHitTargets: battleHitTargets,
                        targetTeams: new List<BattleTeam>(),
                        callback: (result) => { }
                        ));
                }

                // Corrosive Gas
                EffectDatabase.MoveEff.MoveEffect corrosiveGas_ = moveData.GetEffectNew(MoveEffectType.CorrosiveGas);
                if (corrosiveGas_ != null)
                {
                    yield return StartCoroutine(ExecuteMoveEffects(
                        userPokemon: userPokemon,
                        moveData: moveData,
                        effects: new List<EffectDatabase.MoveEff.MoveEffect> { corrosiveGas_ },
                        battleHitTargets: battleHitTargets,
                        targetTeams: new List<BattleTeam>(),
                        callback: (result) => { }
                        ));
                }

                // Trick
                if (moveData.GetEffect(MoveEffectType.Trick) != null
                    && !command.isFutureSightMove)
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.Trick);
                    string textID = effect.GetString(0);
                    textID = (textID == "DEFAULT") ? "move-trick-start-default"
                        : textID;

                    string textID2 = effect.GetString(1);
                    textID2 = (textID2 == "DEFAULT") ? "move-trick-swap-default"
                        : textID2;

                    for (int k = 0; k < battleHitTargets.Count; k++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[k];
                        if (currentTarget.affectedByMove)
                        {
                            Item userItem = battle.PBPGetHeldItem(userPokemon);
                            Item targetItem = battle.PBPGetHeldItem(currentTarget.pokemon);
                            if ((targetItem != null || userItem != null)
                                && !battle.IsPokemonFainted(userPokemon)
                                && !battle.IsPokemonFainted(currentTarget.pokemon))
                            {
                                if (battle.CanPokemonSwapItems(userPokemon, userItem, currentTarget.pokemon, targetItem))
                                {
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = textID,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID
                                    });

                                    if (targetItem != null)
                                    {
                                        currentTarget.pokemon.UnsetItem(targetItem);
                                        userPokemon.SetItem(targetItem);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID2,
                                            pokemonTargetID = userPokemon.uniqueID,
                                            itemID = userItem.itemID
                                        });
                                    }

                                    if (userItem != null)
                                    {
                                        userPokemon.UnsetItem(userItem);
                                        currentTarget.pokemon.SetItem(userItem);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID2,
                                            pokemonTargetID = currentTarget.pokemon.uniqueID,
                                            itemID = userItem.itemID
                                        });
                                    }
                                }
                            }
                        }
                    }
                }

                // Poison Touch
                List<Pokemon.Ability> poisonTouchAbilities = 
                    battle.PBPGetAbilitiesWithEffect(userPokemon, AbilityEffectType.PoisonTouch);
                if (poisonTouchAbilities.Count > 0)
                {
                    for (int i = 0; i < battleHitTargets.Count; i++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[i];
                        if (currentTarget.affectedByMove
                            && battle.IsPokemonOnFieldAndAble(currentTarget.pokemon))
                        {
                            for (int k = 0; k < poisonTouchAbilities.Count; k++)
                            {
                                Pokemon.Ability ability = poisonTouchAbilities[k];
                                List<EffectDatabase.AbilityEff.AbilityEffect> poisonTouch_ =
                                    ability.data.GetEffectsNew(AbilityEffectType.PoisonTouch);
                                for (int j = 0; j < poisonTouch_.Count; j++)
                                {
                                    EffectDatabase.AbilityEff.PoisonTouch poisonTouch =
                                        poisonTouch_[j] as EffectDatabase.AbilityEff.PoisonTouch;
                                    if (battle.DoEffectFiltersPass(
                                        filters: poisonTouch.filters,
                                        userPokemon: userPokemon,
                                        targetPokemon: currentTarget.pokemon,
                                        moveData: moveData
                                        ))
                                    {
                                        bool applyPoisonTouch = Random.value < poisonTouch.chance;

                                        if (applyPoisonTouch)
                                        {
                                            PBPShowAbility(userPokemon, ability);
                                            yield return StartCoroutine(ApplySC(
                                                inflictStatus: poisonTouch.inflictStatus,
                                                targetPokemon: currentTarget.pokemon,
                                                userPokemon: userPokemon,
                                                moveData: moveData,
                                                callback: (result) => { }
                                                ));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Inflict Status (Embargo)
                EffectDatabase.MoveEff.MoveEffect inflictStatus_ = moveData.GetEffectNew(MoveEffectType.InflictStatus, true);
                if (inflictStatus_ != null)
                {
                    yield return StartCoroutine(ExecuteMoveEffects(
                        userPokemon: userPokemon,
                        moveData: moveData,
                        effects: new List<EffectDatabase.MoveEff.MoveEffect> { inflictStatus_ },
                        battleHitTargets: battleHitTargets,
                        targetTeams: new List<BattleTeam>(),
                        callback: (result) => { }
                        ));
                }

                // check whirlwind
                EffectDatabase.MoveEff.MoveEffect whirlwind_ =
                    moveData.GetEffectNew(MoveEffectType.Whirlwind);
                if (whirlwind_ != null && !command.isFutureSightMove)
                {
                    EffectDatabase.MoveEff.Whirlwind whirlwind = whirlwind_ as EffectDatabase.MoveEff.Whirlwind;
                    for (int i = 0; i < battleHitTargets.Count; i++)
                    {
                        BattleHitTarget currentTarget = battleHitTargets[i];
                        if (battle.IsPokemonOnFieldAndAble(currentTarget.pokemon)
                            && currentTarget.affectedByMove)
                        {
                            if (battle.DoEffectFiltersPass(
                                filters: whirlwind.filters,
                                userPokemon: userPokemon,
                                targetPokemon: currentTarget.pokemon,
                                moveData: moveData
                                ))
                            {
                                bool canBeWhirlwinded = true;

                                // Check if they can even switch out
                                Trainer targetTrainer = battle.GetPokemonOwner(currentTarget.pokemon);
                                List<Pokemon> targetViables = battle.GetTrainerFirstXAvailablePokemon(
                                    targetTrainer, targetTrainer.party.Count);
                                if (targetViables.Count == 0)
                                {
                                    canBeWhirlwinded = false;
                                }

                                // Suction Cups
                                if (canBeWhirlwinded)
                                {
                                    List<Pokemon.AbilityEffectPair> suctionCupPairs =
                                        battle.PBPGetAbilityEffectPairs(
                                            currentTarget.pokemon, 
                                            AbilityEffectType.SuctionCups);
                                    for (int k = 0; k < suctionCupPairs.Count && canBeWhirlwinded; k++)
                                    {
                                        Pokemon.AbilityEffectPair abilityPair = suctionCupPairs[k];
                                        EffectDatabase.AbilityEff.SuctionCups suctionCups =
                                            abilityPair.effect as EffectDatabase.AbilityEff.SuctionCups;
                                        if (battle.DoEffectFiltersPass(
                                            filters: suctionCups.filters,
                                            userPokemon: userPokemon,
                                            targetPokemon: currentTarget.pokemon,
                                            moveData: moveData
                                            ))
                                        {
                                            canBeWhirlwinded = false;
                                            if (whirlwind.forceEffectDisplay)
                                            {
                                                PBPShowAbility(currentTarget.pokemon, abilityPair.ability);
                                            }
                                        }
                                    }
                                }

                                // Ingrain
                                if (canBeWhirlwinded)
                                {
                                    for (int k = 0; k < currentTarget.pokemon.bProps.ingrainMoves.Count; k++)
                                    {
                                        MoveData ingrainData = MoveDatabase.instance.GetMoveData(
                                            currentTarget.pokemon.bProps.ingrainMoves[k]
                                            );
                                        MoveEffect ingrainEffect = ingrainData.GetEffect(MoveEffectType.Ingrain);
                                        if (ingrainEffect.GetBool(0))
                                        {
                                            canBeWhirlwinded = false;
                                        }
                                    }
                                }

                                if (canBeWhirlwinded)
                                {
                                    int battlePos = currentTarget.pokemon.battlePos;
                                    Pokemon randomPokemon = targetViables[Random.Range(0, targetViables.Count)];
                                    battle.TrainerWithdrawPokemon(targetTrainer, currentTarget.pokemon);
                                    battle.TrainerSwapPokemon(targetTrainer, currentTarget.pokemon, randomPokemon);
                                    battle.TrainerSendPokemon(targetTrainer, randomPokemon, battlePos);
                                    UpdateClients();

                                    // whirlwind out event
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = whirlwind.forceOutText,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID
                                    });

                                    // whirlwind in event
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = whirlwind.forceInText,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID
                                    });

                                    // Intro / Hazards to new pokemon
                                    currentTarget.pokemon = randomPokemon;
                                    currentTarget.affectedByMove = false;
                                    yield return StartCoroutine(ApplyToPokemonSwitchInEvents(randomPokemon));
                                }
                                else if (whirlwind.forceEffectDisplay)
                                {
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = whirlwind.failText,
                                        pokemonUserID = userPokemon.uniqueID,
                                        pokemonTargetID = currentTarget.pokemon.uniqueID
                                    });
                                }
                            }
                        }
                    }
                }

                // HP-Trigger Berries
                EffectDatabase.ItemEff.ItemEffect triggerHPLoss = 
                    battle.PBPGetItemEffect(userPokemon, ItemEffectType.TriggerOnHPLoss);

            }

            yield return null;
        }

        public IEnumerator TryToFailMove(
            Pokemon userPokemon,
            BattleTeam userTeam,
            MoveData moveData,
            System.Action<bool> callback,
            bool apply = true)
        {
            bool failure = false;
            string textID = "move-FAIL-default";
            PBS.Battle.View.Events.MessageParameterized textEvent = new PBS.Battle.View.Events.MessageParameterized
            {
                messageCode = "move-FAIL-default"
            };

            // Pokemon User Constraints
            if (!failure && moveData.GetEffectNew(MoveEffectType.FailNotPokemon) != null)
            {
                EffectDatabase.MoveEff.MoveEffect effect_ = moveData.GetEffectNew(MoveEffectType.FailNotPokemon);
                EffectDatabase.MoveEff.FailNotPokemon effect = effect_ as EffectDatabase.MoveEff.FailNotPokemon;

                bool isAValidUser = false;
                bool isAFormPokemon = false;
                string userID = userPokemon.pokemonID;
                for (int i = 0; i < effect.pokemonIDs.Count; i++)
                {
                    PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(effect.pokemonIDs[i]);
                    if (userPokemon.pokemonID == pokemonData.ID
                        || userPokemon.data.IsABaseID(pokemonData.ID)
                        || pokemonData.IsABaseID(userPokemon.data.ID))
                    {
                        isAFormPokemon = true;
                    }

                    if (battle.PBPIsPokemonOfForm(
                        pokemon: userPokemon,
                        formPokemon: pokemonData.ID,
                        allowDerivatives: effect.allowDerivatives,
                        allowTransform: effect.allowTransform
                        ))
                    {
                        isAValidUser = true;
                        break;
                    }
                }

                // Invert?
                if (effect.invert)
                {
                    isAValidUser = !isAValidUser;
                }

                failure = !isAValidUser;
                if (failure)
                {
                    textID = isAFormPokemon ? effect.failFormText : effect.failText;
                }
            }
            if (!failure && moveData.GetEffect(MoveEffectType.FailNotPokemon) != null)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.FailNotPokemon);
                bool isValidMoveUser = false;
                bool isAFormPokemon = false;
                string userID = userPokemon.pokemonID;
                string userBaseID = userPokemon.data.baseID;
            
                bool invertFilter = effect.GetBool(0);
                bool includeDerivatives = effect.GetBool(1);
                bool includeAncestors = effect.GetBool(2);
                bool includeSameAncestor = effect.GetBool(3);
                for (int i = 0; i < effect.stringParams.Length; i++)
                {
                    PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(effect.stringParams[i]);

                    // form inequality
                    if (userID != pokemonData.ID)
                    {
                        // If the user is a derivative of the target form
                        if (userPokemon.data.IsABaseID(pokemonData.ID))
                        {
                            isAFormPokemon = true;
                            if (includeDerivatives)
                            {
                                isValidMoveUser = !invertFilter;
                                break;
                            }
                        
                        }
                        // The user is an ancestor of the target form
                        else if (pokemonData.IsABaseID(userID))
                        {
                            isAFormPokemon = true;
                            if (includeAncestors)
                            {
                                isValidMoveUser = !invertFilter;
                                break;
                            }
                        
                        }
                        // The user and target are derivatives of the same pokemon
                        else if (userBaseID == pokemonData.baseID 
                            && !string.IsNullOrEmpty(userBaseID))
                        {
                            isAFormPokemon = true;
                            if (includeSameAncestor)
                            {
                                isValidMoveUser = !invertFilter;
                                break;
                            }
                        }
                    }
                    // the forms are the same
                    else
                    {
                        isAFormPokemon = true;

                        // invert effect
                        isValidMoveUser = !invertFilter;
                        break;
                    }
                }

                failure = !isValidMoveUser;
                if (failure)
                {
                    textID = isAFormPokemon ? "move-FAIL-form" : "move-FAIL-pokemon";

                    MoveEffect failTextEffect = moveData.GetEffect(MoveEffectType.FailNotPokemonText);
                    if (failTextEffect != null)
                    {
                        textID = isAFormPokemon ? failTextEffect.GetString(1) : failTextEffect.GetString(0);
                    }
                }
            }

            // User Type Constraints
            if (!failure && moveData.GetEffect(MoveEffectType.FailUserType) != null)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.FailUserType);
                List<string> requiredTypes = new List<string>(effect.stringParams);
                bool isContained = battle.DoesPokemonHaveATypeInList(userPokemon, requiredTypes.ToArray());

                failure = isContained == effect.GetBool(0);
            }

            // Desolate Land / Primordial Sea 
            if (!failure)
            {
                TypeData moveTypeData = TypeDatabase.instance.GetTypeData(moveData.moveType);
                List<BattleCondition> bConditions = battle.BBPGetSCs();
                for (int i = 0; i < bConditions.Count; i++)
                {
                    EffectDatabase.StatusBTLEff.BattleSE desolateLand_ = 
                        bConditions[i].data.GetEffectNew(BattleSEType.DesolateLand);
                    if (desolateLand_ != null)
                    {
                        EffectDatabase.StatusBTLEff.DesolateLand desolateLand = 
                            desolateLand_ as EffectDatabase.StatusBTLEff.DesolateLand;
                        bool isContained = false;
                        for (int k = 0; k < desolateLand.types.Count; k++)
                        {
                            string curType = desolateLand.types[k];

                            // Not same type
                            if (moveTypeData.ID != curType)
                            {
                                // Descendant
                                if (moveTypeData.IsABaseID(curType))
                                {
                                    isContained = true;
                                    break;
                                }
                            }
                            // Same type
                            else
                            {
                                isContained = true;
                                break;
                            }
                        }

                        if (desolateLand.invert)
                        {
                            isContained = !isContained;
                        }
                        failure = isContained;
                        if (failure)
                        {
                            textID = desolateLand.negateText;
                            break;
                        }
                    }
                }
            }

            // Weather Constraints
            if (!failure && moveData.GetEffect(MoveEffectType.FailWeather) != null)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.FailWeather);
                List<string> weatherList = new List<string>(effect.stringParams);
                bool isContained = false;

                bool invertFilter = effect.GetBool(0);
                bool includeDerivatives = effect.GetBool(1);
                bool includeAncestors = effect.GetBool(2);
                bool includeSameAncestor = effect.GetBool(3);

                for (int i = 0; i < weatherList.Count; i++)
                {
                    StatusBTLData weatherData = StatusBTLDatabase.instance.GetStatusData(weatherList[i]);

                    if (battle.weather.statusID != weatherData.ID)
                    {
                        // If the user is a derivative of the target form
                        if (battle.weather.data.IsABaseID(weatherData.ID) && includeDerivatives)
                        {
                            // invert effect
                            isContained = !invertFilter;
                            break;

                        }
                        // The user is an ancestor of the target form
                        else if (weatherData.IsABaseID(battle.weather.statusID) && includeAncestors)
                        {
                            // invert effect
                            isContained = !invertFilter;
                            break;

                        }
                        // The user and target are derivatives of the same pokemon
                        else if (battle.weather.data.baseID == weatherData.baseID
                            && !string.IsNullOrEmpty(weatherData.baseID)
                            && includeSameAncestor)
                        {
                            // invert effect
                            isContained = !invertFilter;
                            break;
                        }
                    }
                    else
                    {
                        // invert effect
                        isContained = !invertFilter;
                        break;
                    }
                }
                failure = !isContained;
            }

            // Terrain Constraints
            if (!failure && moveData.GetEffect(MoveEffectType.FailTerrain) != null)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.FailTerrain);
                List<string> terrainList = new List<string>(effect.stringParams);
                bool isContained = false;

                bool invertFilter = effect.GetBool(0);
                bool includeDerivatives = effect.GetBool(1);
                bool includeAncestors = effect.GetBool(2);
                bool includeSameAncestor = effect.GetBool(3);

                for (int i = 0; i < terrainList.Count; i++)
                {
                    StatusBTLData terrainData = StatusBTLDatabase.instance.GetStatusData(terrainList[i]);

                    if (battle.terrain.statusID != terrainData.ID)
                    {
                        // If the user is a derivative of the target form
                        if (battle.weather.data.IsABaseID(terrainData.ID) && includeDerivatives)
                        {
                            // invert effect
                            isContained = !invertFilter;
                            break;

                        }
                        // The user is an ancestor of the target form
                        else if (terrainData.IsABaseID(battle.terrain.statusID) && includeAncestors)
                        {
                            // invert effect
                            isContained = !invertFilter;
                            break;

                        }
                        // The user and target are derivatives of the same pokemon
                        else if (battle.terrain.data.baseID == terrainData.baseID
                            && !string.IsNullOrEmpty(terrainData.baseID)
                            && includeSameAncestor)
                        {
                            // invert effect
                            isContained = !invertFilter;
                            break;
                        }
                    }
                    else
                    {
                        // invert effect
                        isContained = !invertFilter;
                        break;
                    }
                }
                failure = !isContained;
            }

            // Steel Roller
            if (!failure && moveData.GetEffectNew(MoveEffectType.SteelRoller) != null)
            {
                EffectDatabase.MoveEff.MoveEffect steelRoller_ = moveData.GetEffectNew(MoveEffectType.SteelRoller);
                EffectDatabase.MoveEff.SteelRoller steelRoller = steelRoller_ as EffectDatabase.MoveEff.SteelRoller;
                if (steelRoller.failOnNoTerrain && battle.terrain.data.HasTag(BattleSTag.Default))
                {
                    failure = true;
                }
            }

            // Damp
            if (!failure)
            {
                List<Pokemon> dampPokemon = battle.GetPokemonUnfainted();
                for (int i = 0; i < dampPokemon.Count && !failure; i++)
                {
                    List<Pokemon.Ability> abilities = battle.PBPGetAbilities(dampPokemon[i]);
                    for (int k = 0; k < abilities.Count && !failure; k++)
                    {
                        EffectDatabase.AbilityEff.AbilityEffect damp_ = 
                            abilities[k].data.GetEffectNew(AbilityEffectType.Damp);
                        if (damp_ != null)
                        {
                            EffectDatabase.AbilityEff.Damp damp = damp_ as EffectDatabase.AbilityEff.Damp;

                            List<MoveTag> tags = new List<MoveTag>(moveData.moveTags);
                            for (int l = 0; l < tags.Count && !failure; l++)
                            {
                                if (damp.moveTags.Contains(tags[l]))
                                {
                                    failure = true;

                                    PBPShowAbility(dampPokemon[i], abilities[k].data);
                                }
                            }
                        }
                    }
                }
            }

            // Ally Switch
            if (!failure && moveData.GetEffect(MoveEffectType.AllySwitch) != null)
            {
                failure = battle.GetAllyPokemon(userPokemon).Count == 0;
            }

            // Assist
            if (!failure && moveData.GetEffect(MoveEffectType.Assist) != null)
            {
                List<string> moveList = battle.GetPokemonAssistMoves(userPokemon);
                failure = moveList.Count == 0;
            }

            // Belch
            if (!failure && moveData.GetEffect(MoveEffectType.Belch) != null)
            {
                failure = userPokemon.bProps.consumedBerry == null;
            }

            // Bestow
            if (!failure && moveData.GetEffect(MoveEffectType.Bestow) != null)
            {
                Item item = battle.PBPGetHeldItem(userPokemon);
                if (item == null)
                {
                    failure = true;
                }
                else
                {
                    if (item.data.HasTag(ItemTag.CannotBestow))
                    {
                        failure = true;
                    }

                    if (!failure)
                    {
                        if (!battle.CanPokemonItemBeLost(userPokemon, item))
                        {
                            failure = true;
                        }
                    }
                }
            }

            // Bide
            if (!failure && moveData.GetEffect(MoveEffectType.Bide) != null)
            {
                if (userPokemon.bProps.bideTurnsLeft == 0 && userPokemon.bProps.bideDamageTaken == 0)
                {
                    failure = true;
                }
            }

            // Copycat
            if (!failure && moveData.GetEffect(MoveEffectType.Copycat) != null)
            {
                if (string.IsNullOrEmpty(battle.lastUsedMove))
                {
                    failure = true;
                }
                else
                {
                    MoveData callData = MoveDatabase.instance.GetMoveData(battle.lastUsedMove);
                    if (callData.HasTag(MoveTag.UncallableByCopycat)
                        || callData.HasTag(MoveTag.ZMove))
                    {
                        failure = true;
                    }
                }
            }

            // Counter / Mirror Coat
            if (!failure)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.Counter);
                if (effect != null)
                {
                    if (effect.GetBool(0))
                    {
                        if (userPokemon.bProps.turnTotalDamageTaken == 0)
                        {
                            failure = true;
                        }
                    }
                    else
                    {
                        if (effect.GetBool(1) && userPokemon.bProps.turnPhysicalDamageTaken == 0)
                        {
                            failure = true;
                        }
                        else if (!effect.GetBool(1) && userPokemon.bProps.turnSpecialDamageTaken == 0)
                        {
                            failure = true;
                        }
                    }
                }
            }

            // Destiny Bond
            if (!failure && moveData.GetEffect(MoveEffectType.DestinyBond) != null)
            {
                if (!string.IsNullOrEmpty(userPokemon.bProps.destinyBondMove))
                {
                    failure = true;
                }
            }

            // Endure
            if (!failure)
            {
                EffectDatabase.MoveEff.MoveEffect endure_ = moveData.GetEffectNew(MoveEffectType.Endure);
                if (endure_ != null)
                {
                    EffectDatabase.MoveEff.Endure endure = endure_ as EffectDatabase.MoveEff.Endure;
                    if (!endure.consecutiveUse)
                    {
                        float chance = Mathf.Pow(1f / 3, userPokemon.bProps.protectCounter);
                        chance = Mathf.Max(1f / 729, chance);
                        failure = Random.value > chance;
                        if (failure && apply)
                        {
                            battle.ResetPokemonProtect(userPokemon);
                        }
                    }
                }
            }

            // Fake Out
            if (!failure)
            {
                EffectDatabase.MoveEff.MoveEffect fakeOut_ = moveData.GetEffectNew(MoveEffectType.FakeOut);
                if (fakeOut_ != null)
                {
                    EffectDatabase.MoveEff.FakeOut fakeOut = fakeOut_ as EffectDatabase.MoveEff.FakeOut;
                    failure = userPokemon.bProps.turnsActive > fakeOut.maxTurn;
                }
            }

            // Fling
            if (!failure && moveData.GetEffect(MoveEffectType.Fling) != null)
            {
                Item item = battle.GetPokemonItemFiltered(userPokemon, ItemEffectType.Fling);
                if (item == null)
                {
                    failure = true;
                }
                else
                {
                    if (!battle.CanPokemonItemBeLost(userPokemon, item))
                    {
                        failure = true;
                    }
                }
            }

            // Hold Hands
            if (!failure && moveData.GetEffect(MoveEffectType.HoldHands) != null)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.HoldHands);
                if (battle.IsSinglesBattle() && effect.GetBool(0))
                {
                    failure = true;
                }
            }

            // Mat Block
            if (!failure && moveData.GetEffect(MoveEffectType.MatBlock) != null)
            {
                if (userTeam.bProps.protectMovesActive.Contains(moveData.ID))
                {
                    failure = true;
                    battle.ResetPokemonProtect(userPokemon);
                }
            }

            // Metronome
            if (!failure && moveData.GetEffect(MoveEffectType.Metronome) != null)
            {
                List<string> moveList = battle.GetMetronomeMoves(userPokemon);
                failure = moveList.Count == 0;
            }

            // Mimic
            if (!failure && moveData.GetEffect(MoveEffectType.Mimic) != null)
            {
                if (!string.IsNullOrEmpty(userPokemon.bProps.mimicBaseMove))
                {
                    failure = true;
                }
            }

            // Mirror Move
            if (!failure && moveData.GetEffect(MoveEffectType.MirrorMove) != null)
            {
                string callMove = userPokemon.bProps.lastMoveTargetedBy;
                if (string.IsNullOrEmpty(callMove))
                {
                    failure = true;
                }
                else
                {
                    MoveData callData = MoveDatabase.instance.GetMoveData(callMove);
                    if (callData.HasTag(MoveTag.UncallableByMirrorMove) || callData.HasTag(MoveTag.ZMove))
                    {
                        failure = true;
                    }
                }
            }

            // Natural Gift
            if (!failure && moveData.GetEffectNew(MoveEffectType.NaturalGift) != null)
            {
                Item naturalGiftItem = battle.PBPGetItemWithEffect(userPokemon, ItemEffectType.NaturalGift);
                if (naturalGiftItem == null)
                {
                    failure = true;
                }
            }

            // Protect
            if (!failure)
            {
                EffectDatabase.MoveEff.MoveEffect protect_ = moveData.GetEffectNew(MoveEffectType.Protect);
                if (protect_ != null)
                {
                    EffectDatabase.MoveEff.Protect protect = protect_ as EffectDatabase.MoveEff.Protect;
                    if (!protect.protect.consecutiveUse)
                    {
                        float chance = Mathf.Pow(1f / 3, userPokemon.bProps.protectCounter);
                        chance = Mathf.Max(1f / 729, chance);
                        failure = Random.value > chance;
                        if (failure && apply)
                        {
                            battle.ResetPokemonProtect(userPokemon);
                        }
                    }
                }
            }

            // Recycle
            if (!failure && moveData.GetEffect(MoveEffectType.Recycle) != null)
            {
                if (battle.PBPGetHeldItem(userPokemon) != null
                    || string.IsNullOrEmpty(userPokemon.bProps.consumedItem))
                {
                    failure = true;
                }
            }

            // Rest
            if (!failure && moveData.GetEffect(MoveEffectType.Rest) != null)
            {
                StatusCondition sleepCondition = battle.GetPokemonFilteredStatus(userPokemon, PokemonSEType.Sleep);
                if (sleepCondition != null)
                {
                    failure = true;
                }
                else if (userPokemon.currentHP == userPokemon.maxHP)
                {
                    failure = true;
                }
                else
                {
                    // See if they can be afflicted with sleep
                    yield return StartCoroutine(TryToInflictPokemonSC(
                        statusID: "sleep",
                        targetPokemon: userPokemon,
                        (result) =>
                        {
                            failure = !result;
                        },
                        bypassOtherConditions: true,
                        forceFailureMessage: true,
                        apply: false
                        ));
                }
            }

            // Role Play
            if (!failure && moveData.GetEffect(MoveEffectType.RolePlay) != null)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.RolePlay);
                if (effect.GetBool(0))
                {
                    AbilityData abilityData = battle.PBPGetAbilityData(userPokemon);
                    if (abilityData != null)
                    {
                        if (abilityData.HasTag(AbilityTag.CannotRolePlayUser))
                        {
                            failure = true;
                        }
                    }
                }
            }

            // Shell Trap
            if (!failure)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.ShellTrap);
                if (effect != null)
                {
                    bool canShellTrap = true;

                    if (canShellTrap
                        && !userPokemon.bProps.wasStruckForDamage)
                    {
                        canShellTrap = false;
                    }

                    if (canShellTrap
                        && effect.GetBool(0)
                        && userPokemon.bProps.turnPhysicalDamageTaken < 0)
                    {
                        canShellTrap = false;
                    }

                    if (canShellTrap
                        && effect.GetBool(1)
                        && !userPokemon.bProps.wasHitByOpponent)
                    {
                        canShellTrap = false;
                    }

                    if (!canShellTrap)
                    {
                        textID = effect.GetString(1);
                        textID = (textID == "DEFAULT") ? "move-shelltrap-fail-default" : textID;
                        failure = true;
                    }
                }
            }

            // Sketch
            if (!failure && moveData.GetEffect(MoveEffectType.Sketch) != null)
            {
                if (!string.IsNullOrEmpty(userPokemon.bProps.sketchBaseMove))
                {
                    failure = true;
                }
            }

            // Skill Swap
            if (!failure && moveData.GetEffect(MoveEffectType.SkillSwap) != null)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.SkillSwap);
                if (effect.GetBool(0))
                {
                    AbilityData abilityData = battle.PBPGetAbilityData(userPokemon);
                    if (abilityData != null)
                    {
                        if (abilityData.HasTag(AbilityTag.CannotSkillSwapUser))
                        {
                            failure = true;
                        }
                    }
                    else
                    {
                        failure = true;
                    }
                }
            }

            // Sleep Talk
            if (!failure && moveData.GetEffect(MoveEffectType.SleepTalk) != null)
            {
                StatusCondition sleepCondition = battle.GetPokemonFilteredStatus(userPokemon, PokemonSEType.Sleep);
                if (sleepCondition == null)
                {
                    failure = true;
                }
                else
                {
                    List<string> moveList = battle.GetPokemonSleepTalkMoves(userPokemon);
                    failure = moveList.Count == 0;
                }
            }

            // Stuff Cheeks
            if (!failure
                && (moveData.GetEffect(MoveEffectType.StuffCheeks) != null 
                    || moveData.GetEffect(MoveEffectType.StuffCheeksMax) != null))
            {
                Item item = battle.PBPGetHeldItem(userPokemon);
                if (item == null)
                {
                    failure = true;
                }
                else
                {
                    if (item.data.pocket != ItemPocket.Berries
                        || !battle.CanPokemonItemBeLost(userPokemon, item))
                    {
                        failure = true;
                    }
                }
            }

            // Teatime
            if (!failure && moveData.GetEffect(MoveEffectType.Teatime) != null)
            {
                failure = true;
                for (int i = 0; i < battle.pokemonOnField.Count; i++)
                {
                    Pokemon pokemon = battle.pokemonOnField[i];
                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        Item item = battle.PBPGetHeldItem(pokemon);
                        if (item != null)
                        {
                            if (item.data.pocket == ItemPocket.Berries
                                && battle.CanPokemonItemBeLost(userPokemon, item))
                            {
                                failure = false;
                                break;
                            }
                        }
                    }
                }

                if (failure)
                {
                    MoveEffect effect = moveData.GetEffect(MoveEffectType.Teatime);
                    textID = effect.GetString(0);
                    textID = (textID == "DEFAULT") ? "move-teatime-fail-default" : textID;
                }
            }

            // Transform
            if (!failure && moveData.GetEffect(MoveEffectType.Transform) != null)
            {
                failure = userPokemon.bProps.tProps != null;
            }

            // Trick
            if (!failure && moveData.GetEffect(MoveEffectType.Trick) != null)
            {
                Item item = battle.PBPGetHeldItem(userPokemon);
                if (item != null)
                {
                    if (!battle.CanPokemonItemBeLost(userPokemon, item))
                    {
                        failure = true;
                    }
                }
            }

            if (failure && apply)
            {
                textEvent.messageCode = textID;
                textEvent.pokemonUserID = userPokemon.uniqueID;
                textEvent.moveID = moveData.ID;
                textEvent.typeID = moveData.moveType;
                SendEvent(textEvent);
            }

            callback(failure);
            yield return null;
        }
    
        public IEnumerator TryToFailMoveWithTargets(
            Pokemon userPokemon,
            BattleTeam userTeam,
            MoveData moveData,
            List<Pokemon> targetPokemon,
            System.Action<bool> callback,
            bool apply = true
            )
        {
            bool failure = false;
            PBS.Battle.View.Events.MessageParameterized textEvent = new PBS.Battle.View.Events.MessageParameterized
            {
                messageCode = "move-FAIL-default"
            };

            // Sucker Punch
            if (!failure && moveData.GetEffectNew(MoveEffectType.SuckerPunch) != null)
            {
                EffectDatabase.MoveEff.MoveEffect suckerPunch_ = moveData.GetEffectNew(MoveEffectType.SuckerPunch);
                EffectDatabase.MoveEff.SuckerPunch suckerPunch = suckerPunch_ as EffectDatabase.MoveEff.SuckerPunch;

                bool foundSuckerPunchTarget = false;
                for (int i = 0; i < targetPokemon.Count; i++)
                {
                    BattleCommand targetCommand = GetPokemonCommand(targetPokemon[i]);
                    if (targetCommand != null)
                    {
                        if (targetCommand.commandType == BattleCommandType.Fight
                            && !targetPokemon[i].bProps.actedThisTurn)
                        {
                            bool foundMove = false;
                            MoveData targetMoveData = MoveDatabase.instance.GetMoveData(targetCommand.moveID);
                            if (!foundMove && suckerPunch.categories.Contains(targetMoveData.category))
                            {
                                foundMove = true;
                            }
                            if (!foundMove && suckerPunch.allowableMoves.Contains(targetMoveData.ID))
                            {
                                foundMove = true;
                            }
                            if (foundMove)
                            {
                                foundSuckerPunchTarget = true;
                                Debug.Log("DEBUG - Sucker Punch - " + targetPokemon[i].nickname + " was about to use " + targetMoveData.moveName);
                                break;
                            }
                        }
                    }
                }
                if (suckerPunch.invertFilter)
                {
                    foundSuckerPunchTarget = !foundSuckerPunchTarget;
                }
                failure = !foundSuckerPunchTarget;

                if (failure && apply)
                {
                    textEvent.pokemonUserID = userPokemon.uniqueID;
                    SendEvent(textEvent);
                }
            }

            callback(failure);
            yield return null;
        }
    
        public IEnumerator TryToProtectMoveHit(
            Pokemon userPokemon,
            Pokemon targetPokemon,
            MoveData moveData,
            BattleTeam userTeam,
            BattleTeam targetTeam,
            System.Action<EffectDatabase.General.Protect> callback,
            bool apply = true)
        {
            EffectDatabase.General.Protect returnProtect = null;

            // Set bypass protect if possible
            bool bypassProtect = false;
            bool bypassCraftyShield = false;
            bool bypassMaxGuard = false;
            if (moveData.HasTag(MoveTag.IgnoreProtect) || moveData.HasTag(MoveTag.IgnoreMaxGuard))
            {
                bypassProtect = true;
            }
            if (moveData.HasTag(MoveTag.IgnoreCraftyShield))
            {
                bypassCraftyShield = true;
            }
            if (moveData.HasTag(MoveTag.IgnoreMaxGuard))
            {
                bypassMaxGuard = true;
            }

            // Unseen Fist
            List<EffectDatabase.AbilityEff.AbilityEffect> unseenFist_ =
                battle.PBPGetAbilityEffects(userPokemon, AbilityEffectType.UnseenFist);
            for (int i = 0; i < unseenFist_.Count; i++)
            {
                EffectDatabase.AbilityEff.UnseenFist unseenFist =
                    unseenFist_[i] as EffectDatabase.AbilityEff.UnseenFist;
                if (battle.DoEffectFiltersPass(
                    filters: unseenFist.filters,
                    userPokemon: userPokemon,
                    targetPokemon: targetPokemon,
                    moveData: moveData
                    ))
                {
                    if (unseenFist.ignoreProtect)
                    {
                        bypassProtect = true;
                    }
                    if (unseenFist.ignoreCraftyShield)
                    {
                        bypassCraftyShield = true;
                    }
                    if (unseenFist.ignoreMaxGuard)
                    {
                        bypassMaxGuard = true;
                    }
                }
            }

            bool isTeamTargeting = battle.IsMoveMultiTargeting(userPokemon, moveData);

            if (battle.IsPokemonOnFieldAndAble(targetPokemon)
                && moveData.targetType != MoveTargetType.Battlefield
                && userPokemon != targetPokemon)
            {
                if (targetPokemon.bProps.protect != null)
                {
                    EffectDatabase.General.Protect protect = targetPokemon.bProps.protect;
                    bool isProtected = true;

                    // Bypass Crafty Shield / Protect / Max Guard
                    if (protect.craftyShield && bypassCraftyShield)
                    {
                        isProtected = !bypassMaxGuard || protect.maxGuard;
                    }
                    else if (!protect.craftyShield && bypassProtect)
                    {
                        isProtected = !bypassMaxGuard || protect.maxGuard;
                    }

                    // Crafty Shield (Status Only)
                    if (isProtected &&
                        protect.craftyShield && !bypassCraftyShield)
                    {
                        if (moveData.category != MoveCategory.Status)
                        {
                            isProtected = false;
                        }
                    }

                    // Quick Guard (Priority Only)
                    if (isProtected &&
                        !bypassProtect &&
                        protect.wideGuard && moveData.priority <= 0)
                    {
                        isProtected = false;
                    }

                    // Wide Guard (Multi-Targeting Only)
                    if (isProtected &&
                        !bypassProtect &&
                        protect.wideGuard && !isTeamTargeting)
                    {
                        isProtected = false;
                    }

                    // Damaging Only
                    if (isProtected &&
                        !bypassProtect &&
                        protect.damagingOnly &&
                            !(moveData.category == MoveCategory.Physical || moveData.category == MoveCategory.Special))
                    {
                        isProtected = false;
                    }

                    // Opposing only
                    if (isProtected &&
                        !bypassProtect &&
                        protect.opposingOnly && userTeam == targetTeam)
                    {
                        isProtected = false;
                    }

                    if (isProtected)
                    {
                        returnProtect = protect;
                    }
                }
            }

            callback(returnProtect);
            yield return null;
        }
        public IEnumerator TryToTeamProtectMoveHit(
            Pokemon userPokemon,
            MoveData moveData,
            BattleTeam userTeam,
            BattleTeam targetTeam,
            System.Action<EffectDatabase.General.Protect> callback,
            bool apply = true)
        {
            EffectDatabase.General.Protect returnProtect = null;

            // Set bypass protect if possible
            bool bypassProtect = false;
            bool bypassCraftyShield = false;
            bool bypassMaxGuard = false;
            if (moveData.HasTag(MoveTag.IgnoreProtect) || moveData.HasTag(MoveTag.IgnoreMaxGuard))
            {
                bypassProtect = true;
            }
            if (moveData.HasTag(MoveTag.IgnoreCraftyShield))
            {
                bypassCraftyShield = true;
            }
            if (moveData.HasTag(MoveTag.IgnoreMaxGuard))
            {
                bypassMaxGuard = true;
            }

            bool isMultiTargeting = battle.IsMoveMultiTargeting(userPokemon, moveData);

            if (moveData.targetType != MoveTargetType.Battlefield)
            {
                // Mat Block
                for (int i = 0; i < targetTeam.bProps.matBlocks.Count; i++)
                {
                    EffectDatabase.General.Protect protect = targetTeam.bProps.matBlocks[i];
                    bool isProtected = true;

                    // Bypass Crafty Shield / Protect / Max Guard
                    if (protect.craftyShield && bypassCraftyShield)
                    {
                        isProtected = !bypassMaxGuard || protect.maxGuard;
                    }
                    else if (!protect.craftyShield && bypassProtect)
                    {
                        isProtected = !bypassMaxGuard || protect.maxGuard;
                    }

                    // Crafty Shield (Status Only)
                    if (isProtected && 
                        protect.craftyShield && !bypassCraftyShield)
                    {
                        if (moveData.category != MoveCategory.Status)
                        {
                            isProtected = false;
                        }
                    }

                    // Quick Guard (Priority Only)
                    if (isProtected &&
                        !bypassProtect &&
                        protect.wideGuard && moveData.priority <= 0)
                    {
                        isProtected = false;
                    }

                    // Wide Guard (Multi-Targeting Only)
                    if (isProtected &&
                        !bypassProtect &&
                        protect.wideGuard && !isMultiTargeting)
                    {
                        isProtected = false;
                    }

                    // Damaging Only
                    if (isProtected &&
                        !bypassProtect &&
                        protect.damagingOnly &&
                            !(moveData.category == MoveCategory.Physical || moveData.category == MoveCategory.Special))
                    {
                        isProtected = false;
                    }

                    // Opposing only
                    if (isProtected &&
                        !bypassProtect &&
                        protect.opposingOnly && userTeam == targetTeam)
                    {
                        isProtected = false;
                    }

                    if (isProtected)
                    {
                        returnProtect = protect;
                        break;
                    }
                }
            }

            callback(returnProtect);
            yield return null;
        }

        public IEnumerator TryToFailMoveHit(
            Pokemon userPokemon,
            Pokemon targetPokemon,
            MoveData moveData,
            BattleTeam userTeam,
            BattleTeam targetTeam,
            System.Action<bool> callback,
            BattleTypeEffectiveness effectiveness = null,
            bool forceFailureMessage = true,
            bool apply = true)
        {
            bool failure = false;
            effectiveness = (effectiveness == null) ? new BattleTypeEffectiveness() : effectiveness;

            List<Pokemon> targetAllyPokemon = battle.GetAllyPokemon(targetPokemon);

            // Set substitute bypass if possible
            // Set substitute bypass if applicable
            bool bypassSubstitute = false;

            // Infiltrator
            List<EffectDatabase.AbilityEff.AbilityEffect> infiltrators_ =
                battle.PBPGetAbilityEffects(userPokemon, AbilityEffectType.Infiltrator);
            for (int k = 0; k < infiltrators_.Count && !bypassSubstitute; k++)
            {
                EffectDatabase.AbilityEff.Infiltrator infiltrator =
                    infiltrators_[k] as EffectDatabase.AbilityEff.Infiltrator;
                if (infiltrator.bypassSubstitute)
                {
                    bypassSubstitute = true;
                }
            }

            // force bypass or sound move
            if (moveData.HasTag(MoveTag.IgnoreSubstitute)
                || moveData.HasTag(MoveTag.SoundMove))
            {
                bypassSubstitute = true;
            }

            // Set ability bypass if possible
            bool bypassAbility = false;
            // Mold Breaker bypasses ability immunities
            if (!bypassAbility)
            {
                EffectDatabase.AbilityEff.AbilityEffect moldBreakerEffect =
                    battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                if (moldBreakerEffect != null)
                {
                    bypassAbility = true;
                }

                // Sunsteel Strike bypasses ability immunities
                EffectDatabase.MoveEff.MoveEffect effect = moveData.GetEffectNew(MoveEffectType.SunteelStrike);
                if (effect != null)
                {
                    bypassAbility = true;
                }
            }
        
            List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(
                pokemon: userPokemon);
            List<Pokemon.Ability> targetAbilities = battle.PBPGetAbilities(
                pokemon: targetPokemon,
                bypassAbility: bypassAbility);

            PBS.Battle.View.Events.MessageParameterized textEvent = new PBS.Battle.View.Events.MessageParameterized
            {
                messageCode = (battle.IsSinglesBattle())? "move-noeffect-default" : "move-noeffect-multi-default"
            };

            if (battle.IsPokemonOnFieldAndAble(targetPokemon))
            {
                // Moves that can't be used on self
                if (userPokemon != targetPokemon)
                {
                    if (!battle.IsMoveFieldTargeting(pokemon: userPokemon, moveData: moveData))
                    {
                        // Some immunities are only triggered if the move would have been effective
                        if (!failure && !effectiveness.IsImmune())
                        {
                            // Queenly Majesty (Allies)
                            if (!failure && targetAllyPokemon.Count > 0)
                            {
                                for (int i = 0; i < targetAllyPokemon.Count && !failure; i++)
                                {
                                    List<Pokemon.Ability> abilities = battle.PBPGetAbilities(
                                        pokemon: targetAllyPokemon[i], 
                                        bypassAbility: bypassAbility);
                                    for (int k = 0; k < abilities.Count && !failure; k++)
                                    {

                                        // Queenly Majesty / Dazzling
                                        EffectDatabase.AbilityEff.AbilityEffect queenlyMajesty_ =
                                            abilities[k].data.GetEffectNew(AbilityEffectType.QueenlyMajesty);
                                        if (queenlyMajesty_ != null && !failure)
                                        {
                                            EffectDatabase.AbilityEff.QueenlyMajesty queenlyMajesty =
                                                queenlyMajesty_ as EffectDatabase.AbilityEff.QueenlyMajesty;
                                            if (queenlyMajesty.affectsTeam 
                                                && battle.ArePokemonEnemies(userPokemon, targetAllyPokemon[i]))
                                            {
                                                if (moveData.priority > 0)
                                                {
                                                    failure = true;
                                                }
                                            }
                                        }

                                        if (failure)
                                        {
                                            if (forceFailureMessage)
                                            {
                                                PBPShowAbility(pokemon: targetAllyPokemon[i], abilityData: abilities[k].data);
                                            }
                                        }
                                    }
                                }
                            }

                            // Ability Immunities (Bulletproof, Soundproof)
                            if (!failure)
                            {
                                for (int i = 0; i < targetAbilities.Count && !failure; i++)
                                {
                                    AbilityData abilityData = targetAbilities[i].data;
                                    EffectDatabase.AbilityEff.AbilityEffect cacophony_ =
                                        abilityData.GetEffectNew(AbilityEffectType.Cacophony);
                                    if (cacophony_ != null && !failure)
                                    {
                                        EffectDatabase.AbilityEff.Cacophony cacophony =
                                            cacophony_ as EffectDatabase.AbilityEff.Cacophony;
                                        if (battle.DoEffectFiltersPass(
                                            filters: cacophony.filters,
                                            userPokemon: userPokemon,
                                            targetPokemon: targetPokemon,
                                            targetTeam: targetTeam,
                                            moveData: moveData,
                                            abilityData: abilityData
                                            ))
                                        {
                                            failure = true;
                                        }
                                    }
                                
                                    // Queenly Majesty / Dazzling
                                    EffectDatabase.AbilityEff.AbilityEffect queenlyMajesty_ =
                                        abilityData.GetEffectNew(AbilityEffectType.QueenlyMajesty);
                                    if (queenlyMajesty_ != null && !failure)
                                    {
                                        if (battle.ArePokemonEnemies(userPokemon, targetPokemon))
                                        {
                                            if (moveData.priority > 0)
                                            {
                                                failure = true;
                                            }
                                        }
                                    }

                                    if (failure)
                                    {
                                        if (forceFailureMessage)
                                        {
                                            PBPShowAbility(pokemon: targetPokemon, abilityData: abilityData);
                                        }
                                    }
                                }
                            }

                            // Volt Absorb / Lightning Rod / Dry Skin
                            if (!failure)
                            {
                                for (int i = 0; i < targetAbilities.Count && !failure; i++)
                                {
                                    AbilityData abilityData = targetAbilities[i].data;
                                    EffectDatabase.AbilityEff.AbilityEffect voltAbsorb_ =
                                        abilityData.GetEffectNew(AbilityEffectType.VoltAbsorb);
                                    if (voltAbsorb_ != null)
                                    {
                                        EffectDatabase.AbilityEff.VoltAbsorb voltAbsorb =
                                            voltAbsorb_ as EffectDatabase.AbilityEff.VoltAbsorb;

                                        if (battle.DoEffectFiltersPass(
                                            filters: voltAbsorb.filters,
                                            userPokemon: userPokemon,
                                            targetPokemon: targetPokemon,
                                            moveData: moveData
                                            ))
                                        {
                                            bool isBlocked = false;
                                            EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition triggerCondition =
                                                null;

                                            for (int k = 0; k < voltAbsorb.conditions.Count && !isBlocked; k++)
                                            {
                                                EffectDatabase.AbilityEff.VoltAbsorb.VoltAbsorbCondition
                                                    voltAbsorbCondition = voltAbsorb.conditions[k];

                                                // By type
                                                if (!isBlocked 
                                                    && battle.AreTypesContained(
                                                    containerTypes: voltAbsorbCondition.moveTypes,
                                                    checkType: moveData.moveType
                                                    ))
                                                {
                                                    isBlocked = true;
                                                }
                                            
                                                if (isBlocked)
                                                {
                                                    triggerCondition = voltAbsorbCondition;
                                                }
                                            }

                                            if (isBlocked)
                                            {
                                                failure = true;
                                                if (forceFailureMessage)
                                                {
                                                    PBPShowAbility(pokemon: targetPokemon, abilityData: abilityData);
                                                }
                                                if (apply 
                                                    && triggerCondition != null)
                                                {
                                                    textEvent.messageCode = voltAbsorb.displayText;

                                                    // HP-Absorb (Volt Absorb / Water Absorb)
                                                    if (triggerCondition.absorbPercent > 0)
                                                    {
                                                        yield return StartCoroutine(PBPHealPokemon(
                                                            pokemon: targetPokemon,
                                                            HPToAdd: battle.GetPokemonHPByPercent(
                                                                targetPokemon, 
                                                                triggerCondition.absorbPercent)
                                                            ));
                                                    }

                                                    // Stat-Change (Lightning Rod / Motor Drive / Storm Drain)
                                                    if (triggerCondition.motorDrive != null)
                                                    {
                                                        yield return StartCoroutine(ApplyStatStageMod(
                                                            statStageMod: triggerCondition.motorDrive,
                                                            targetPokemon: targetPokemon,
                                                            callback: (result) => { }
                                                            ));
                                                    }

                                                    // Flash Fire
                                                    if (triggerCondition.flashFireBoost > 0)
                                                    {
                                                        Pokemon.BattleProperties.FlashFireBoost flashFireBoost =
                                                            targetPokemon.bProps.GetFlashFireBoost(moveData.moveType);
                                                        if (flashFireBoost != null)
                                                        {
                                                            if (flashFireBoost.boost < triggerCondition.flashFireBoost)
                                                            {
                                                                targetPokemon.bProps.flashFireBoosts.Remove(flashFireBoost);
                                                                targetPokemon.bProps.flashFireBoosts.Add(
                                                                    new Pokemon.BattleProperties.FlashFireBoost(
                                                                        moveType: moveData.moveType,
                                                                        boost: triggerCondition.flashFireBoost
                                                                        ));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            targetPokemon.bProps.flashFireBoosts.Add(
                                                                new Pokemon.BattleProperties.FlashFireBoost(
                                                                    moveType: moveData.moveType,
                                                                    boost: triggerCondition.flashFireBoost
                                                                    ));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Prankster immunity
                            if (!failure)
                            {
                                for (int i = 0; i < userAbilities.Count && !failure; i++)
                                {
                                    Pokemon.Ability ability = userAbilities[i];
                                    EffectDatabase.AbilityEff.AbilityEffect prankster_ =
                                        ability.data.GetEffectNew(AbilityEffectType.Prankster);
                                    if (prankster_ != null && !failure)
                                    {
                                        EffectDatabase.AbilityEff.Prankster prankster =
                                            prankster_ as EffectDatabase.AbilityEff.Prankster;
                                        if (battle.DoEffectFiltersPass(
                                            filters: prankster.filters,
                                            userPokemon: userPokemon,
                                            targetPokemon: targetPokemon,
                                            targetTeam: targetTeam,
                                            moveData: moveData
                                            ))
                                        {
                                            failure = true;
                                        }
                                    }
                                }
                            }

                            // Telepathy Immunity
                            if (!failure && battle.ArePokemonAllies(userPokemon, targetPokemon))
                            {
                                for (int i = 0; i < targetAbilities.Count && !failure; i++)
                                {
                                    Pokemon.Ability ability = targetAbilities[i];
                                    EffectDatabase.AbilityEff.AbilityEffect telepathy_ =
                                        ability.data.GetEffectNew(AbilityEffectType.Telepathy);
                                    if (telepathy_ != null)
                                    {
                                        EffectDatabase.AbilityEff.Telepathy telepathy =
                                            telepathy_ as EffectDatabase.AbilityEff.Telepathy;
                                        if (battle.DoEffectFiltersPass(
                                            filters: telepathy.filters,
                                            userPokemon: userPokemon,
                                            targetPokemon: targetPokemon,
                                            moveData: moveData
                                            ))
                                        {
                                            failure = true;
                                            if (forceFailureMessage)
                                            {
                                                PBPShowAbility(pokemon: targetPokemon, ability);
                                            }
                                        }
                                    }
                                }
                            }

                            // Wonder Guard
                            if (!failure)
                            {
                                for (int i = 0; i < targetAbilities.Count && !failure; i++)
                                {
                                    Pokemon.Ability ability = targetAbilities[i];
                                    EffectDatabase.AbilityEff.AbilityEffect wonderGuard_ =
                                        ability.data.GetEffectNew(AbilityEffectType.WonderGuard);
                                    if (wonderGuard_ != null)
                                    {
                                        EffectDatabase.AbilityEff.WonderGuard wonderGuard =
                                            wonderGuard_ as EffectDatabase.AbilityEff.WonderGuard;
                                        if (battle.DoEffectFiltersPass(
                                            filters: wonderGuard.filters,
                                            userPokemon: userPokemon,
                                            targetPokemon: targetPokemon,
                                            moveData: moveData
                                            ))
                                        {
                                            failure = true;
                                            if (wonderGuard.affectNeutral && effectiveness.IsNeutral())
                                            {
                                                failure = false;
                                            }
                                            if (wonderGuard.affectSuperEffective && effectiveness.IsSuperEffective())
                                            {
                                                failure = false;
                                            }
                                            if (wonderGuard.affectNotVeryEffective && effectiveness.IsNotVeryEffective())
                                            {
                                                failure = false;
                                            }
                                        
                                            if (failure && forceFailureMessage)
                                            {
                                                PBPShowAbility(pokemon: targetPokemon, ability);
                                            }
                                        }
                                    }
                                }
                            }

                            // TODO: held items (ex. Safety Goggles)
                            if (!failure)
                            {

                            }

                        


                        }

                        // Substitute
                        if (!failure && !bypassSubstitute && !string.IsNullOrEmpty(targetPokemon.bProps.substituteMove))
                        {
                            if (moveData.category == MoveCategory.Status
                                || (moveData.GetEffect(MoveEffectType.SkyDrop) != null && userPokemon.bProps.attemptingToSkyDrop)
                                || moveData.HasTag(MoveTag.CannotUseOnSubstitute)
                                )
                            {
                                MoveData subMoveData = MoveDatabase.instance.GetMoveData(targetPokemon.bProps.substituteMove);
                                MoveEffect effect = subMoveData.GetEffect(MoveEffectType.Substitute);
                                string textID = effect.GetString(4);
                                textID = (textID == "DEFAULT") ? "move-substitute-block-default" : textID;
                                textEvent.messageCode = textID;
                                failure = true;
                            }
                        }

                        // After You
                        if (!failure && moveData.GetEffect(MoveEffectType.AfterYou) != null)
                        {
                            failure = targetPokemon.bProps.actedThisTurn;
                        }

                        // Bestow
                        if (!failure && moveData.GetEffect(MoveEffectType.Bestow) != null)
                        {
                            Item item = battle.PBPGetHeldItem(targetPokemon);
                            if (item != null)
                            {
                                failure = true;
                            }
                        }

                        // Helping hand
                        if (!failure && moveData.GetEffect(MoveEffectType.HelpingHand) != null)
                        {
                            if (targetPokemon.bProps.actedThisTurn)
                            {
                                failure = true;
                            }
                        }

                        // Instruct
                        if (!failure && moveData.GetEffect(MoveEffectType.Instruct) != null)
                        {
                            string lastMove = targetPokemon.bProps.lastMove;
                            if (lastMove != null)
                            {
                                MoveData instructData = MoveDatabase.instance.GetMoveData(lastMove);
                                if (!battle.DoesPokemonHaveMove(targetPokemon, lastMove)
                                    || battle.GetPokemonMovePP(targetPokemon, lastMove) <= 0
                                    || instructData.HasTag(MoveTag.CannotInstruct)
                                    || instructData.GetEffect(MoveEffectType.RechargeTurn) != null
                                    || instructData.GetEffect(MoveEffectType.MultiTurnAttack) != null
                                    || instructData.GetEffect(MoveEffectType.SkyDrop) != null
                                    || !string.IsNullOrEmpty(targetPokemon.bProps.beakBlastMove)
                                    || !string.IsNullOrEmpty(targetPokemon.bProps.focusPunchMove)
                                    || !string.IsNullOrEmpty(targetPokemon.bProps.shellTrapMove)
                                    || targetPokemon.bProps.nextCommand != null)
                                {
                                    failure = true;
                                }
                            }
                            else
                            {
                                failure = true;
                            }
                        }

                        // Me First
                        if (!failure && moveData.GetEffect(MoveEffectType.MeFirst) != null)
                        {
                            bool meFirstFail = false;
                            for (int i = 0; i < allCommands.Count; i++)
                            {
                                if (targetPokemon.IsTheSameAs(allCommands[i].commandUser))
                                {
                                    if (allCommands[i].commandType == BattleCommandType.Fight
                                        && !allCommands[i].completed
                                        && !allCommands[i].inProgress)
                                    {
                                        MoveData callData = MoveDatabase.instance.GetMoveData(allCommands[i].moveID);
                                        if (callData.HasTag(MoveTag.UncallableByMeFirst)
                                            || callData.HasTag(MoveTag.ZMove)
                                            || callData.category == MoveCategory.Status)
                                        {
                                            meFirstFail = true;
                                        }
                                    }
                                    else
                                    {
                                        meFirstFail = true;
                                    }
                                }
                            }
                            failure = meFirstFail;
                        }

                        // Mimic
                        if (!failure && moveData.GetEffect(MoveEffectType.Mimic) != null)
                        {
                            bool mimicFail = false;
                            if (string.IsNullOrEmpty(targetPokemon.bProps.lastMove))
                            {
                                mimicFail = true;
                            }
                            else if (battle.DoesPokemonHaveMove(userPokemon, targetPokemon.bProps.lastMove))
                            {
                                mimicFail = true;
                            }
                            else if (MoveDatabase.instance.GetMoveData(targetPokemon.bProps.lastMove)
                                .HasTag(MoveTag.CannotMimic))
                            {
                                mimicFail = true;
                            }

                            failure = mimicFail;
                        }

                        // Poltergeist
                        EffectDatabase.MoveEff.MoveEffect poltergeist_ = moveData.GetEffectNew(MoveEffectType.Poltergeist);
                        if (!failure && poltergeist_ != null)
                        {
                            EffectDatabase.MoveEff.Poltergeist poltergeist = poltergeist_ as EffectDatabase.MoveEff.Poltergeist;
                            if (poltergeist.failOnNoItem)
                            {
                                bool noItem = false;
                                Item targetItem = battle.PBPGetHeldItem(targetPokemon);
                                if (!noItem && targetItem == null)
                                {
                                    noItem = true;
                                }
                                if (!noItem && targetItem != null)
                                {
                                    if (!battle.CanPokemonUseItem(targetPokemon, targetItem))
                                    {
                                        noItem = true;
                                    }
                                }
                                failure = noItem;
                            }
                        }

                        // Quash
                        if (!failure && moveData.GetEffect(MoveEffectType.Quash) != null)
                        {
                            failure = targetPokemon.bProps.actedThisTurn;
                        }

                        // Role Play
                        if (!failure && moveData.GetEffect(MoveEffectType.RolePlay) != null)
                        {
                            MoveEffect effect = moveData.GetEffect(MoveEffectType.RolePlay);
                            if (effect.GetBool(0))
                            {
                                AbilityData abilityData = battle.PBPGetAbilityData(targetPokemon);
                                if (abilityData == null)
                                {
                                    failure = true;
                                }
                                else if (abilityData.HasTag(AbilityTag.CannotRolePlay))
                                {
                                    failure = true;
                                }
                            }
                        }

                        // Sketch
                        if (!failure && moveData.GetEffect(MoveEffectType.Sketch) != null)
                        {
                            bool sketchFail = false;
                            if (string.IsNullOrEmpty(targetPokemon.bProps.lastMove))
                            {
                                sketchFail = true;
                            }
                            else if (battle.DoesPokemonHaveMove(userPokemon, targetPokemon.bProps.lastMove))
                            {
                                sketchFail = true;
                            }
                            else if (MoveDatabase.instance.GetMoveData(targetPokemon.bProps.lastMove)
                                .HasTag(MoveTag.CannotSketch))
                            {
                                sketchFail = true;
                            }
                            failure = sketchFail;
                        }

                        // Skill Swap
                        if (!failure && moveData.GetEffect(MoveEffectType.SkillSwap) != null)
                        {
                            MoveEffect effect = moveData.GetEffect(MoveEffectType.SkillSwap);
                            if (effect.GetBool(0))
                            {
                                AbilityData abilityData = battle.PBPGetAbilityData(targetPokemon);
                                if (abilityData == null)
                                {
                                    failure = true;
                                }
                                else if (abilityData.HasTag(AbilityTag.CannotSkillSwap))
                                {
                                    failure = true;
                                }
                            }
                        }

                        // Synchronoise
                        if (!failure)
                        {
                            EffectDatabase.MoveEff.MoveEffect effect_ = moveData.GetEffectNew(MoveEffectType.Synchronoise);
                            if (effect_ != null)
                            {
                                EffectDatabase.MoveEff.Synchronoise effect = effect_ as EffectDatabase.MoveEff.Synchronoise;
                                bool foundType = false;
                                List<string> userTypes = battle.PBPGetTypes(userPokemon);
                                for (int i = 0; i < userTypes.Count; i++)
                                {
                                    if (battle.DoesPokemonHaveType(targetPokemon, userTypes[i]))
                                    {
                                        foundType = true;
                                    }
                                    else if (effect.exactMatch)
                                    {
                                        break;
                                    }
                                }

                                failure = foundType != !effect.invert;
                                if (failure)
                                {
                                    textEvent.messageCode = (!string.IsNullOrEmpty(effect.failTextID)) ? effect.failTextID : textEvent.messageCode;
                                }
                            }
                        }

                        // Trick
                        if (!failure && moveData.GetEffect(MoveEffectType.Trick) != null)
                        {
                            Item userItem = battle.PBPGetHeldItem(userPokemon);
                            Item targetItem = battle.PBPGetHeldItem(targetPokemon);
                            if (!battle.CanPokemonSwapItems(userPokemon, userItem, targetPokemon, targetItem))
                            {
                                failure = true;
                            }
                        }

                    }

                
                }

            }

            // move has been successfully failed
            if (failure && forceFailureMessage)
            {
                textEvent.pokemonUserID = userPokemon.uniqueID;
                textEvent.pokemonTargetID = targetPokemon.uniqueID;
                textEvent.moveID = moveData.ID;
                SendEvent(textEvent);
            }

            callback(failure);
            yield return null;
        }

        public IEnumerator TryToReflectMove(
            Pokemon userPokemon,
            List<Pokemon> targetPokemon,
            MoveData moveData,
            BattleCommand originalCommand,
            System.Action<EffectDatabase.General.MagicCoat> callback,
            bool forceOneHit = false,
            bool apply = true
            )
        {
            EffectDatabase.General.MagicCoat magicCoat = null;

            //BattleCommand reflectCommand = null;

            for (int i = 0; i < targetPokemon.Count && magicCoat == null; i++)
            {

            }

            callback(magicCoat);
            yield return null;
        }
        public IEnumerator TryToReflectMoveHit(
            Pokemon userPokemon,
            Pokemon targetPokemon,
            MoveData moveData,
            BattleCommand originalCommand,
            System.Action<EffectDatabase.General.MagicCoat> callback,
            bool forceOneHit = false,
            bool apply = true
            )
        {
            EffectDatabase.General.MagicCoat magicCoat = null;

            // Set ability bypass if possible
            bool bypassAbility = false;
            // Mold Breaker bypasses ability immunities
            if (!bypassAbility)
            {
                EffectDatabase.AbilityEff.AbilityEffect moldBreakerEffect =
                    battle.PBPGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                if (moldBreakerEffect != null)
                {
                    bypassAbility = true;
                }

                // Sunsteel Strike bypasses ability immunities
                EffectDatabase.MoveEff.MoveEffect effect = moveData.GetEffectNew(MoveEffectType.SunteelStrike);
                if (effect != null)
                {
                    bypassAbility = true;
                }
            }

            List<Pokemon.Ability> userAbilities = battle.PBPGetAbilities(
                pokemon: userPokemon);
            List<Pokemon.Ability> targetAbilities = battle.PBPGetAbilities(
                pokemon: targetPokemon,
                bypassAbility: bypassAbility);

            // only reflect opposing pokemon moves
            if (battle.ArePokemonEnemies(userPokemon, targetPokemon))
            {
                // Magic Coat
                if (magicCoat == null)
                {

                }

                // Magic Bounce
                if (magicCoat == null)
                {
                    for (int i = 0; i < targetAbilities.Count && magicCoat == null; i++)
                    {
                        Pokemon.Ability ability = targetAbilities[i];
                        EffectDatabase.AbilityEff.AbilityEffect magicBounce_ =
                            ability.data.GetEffectNew(AbilityEffectType.MagicBounce);
                        if (magicBounce_ != null)
                        {
                            EffectDatabase.AbilityEff.MagicBounce magicBounce =
                                magicBounce_ as EffectDatabase.AbilityEff.MagicBounce;
                            if (battle.DoEffectFiltersPass(
                                filters: magicBounce.filters,
                                userPokemon: userPokemon,
                                targetPokemon: targetPokemon,
                                moveData: moveData
                                ))
                            {
                                EffectDatabase.General.MagicCoat curMagicBounce = magicBounce.magicCoat;
                                magicCoat = curMagicBounce;

                                PBPShowAbility(targetPokemon, ability);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = magicCoat.reflectText,
                                    pokemonUserID = userPokemon.uniqueID,
                                    pokemonTargetID = targetPokemon.uniqueID
                                });
                            }
                        }
                    }
                }
            }

            callback(magicCoat);
            yield return null;
        }


        public IEnumerator TryToBatonPassOut(
            Pokemon withdrawPokemon,
            Trainer trainer,
            System.Action<bool> callback,
            MoveData uTurnData = null,
            bool isBatonPassing = true,
            bool bypassPursuitCheck = false
            )
        {
            bool success = true;
            withdrawPokemon.bProps.isSwitchingOut = true;

            // Pursuit
            if (success && !bypassPursuitCheck)
            {
                // check who's using pursuit
                List<BattleCommand> pursuitCommands = new List<BattleCommand>();
                for (int i = 0; i < allCommands.Count; i++)
                {
                    Pokemon pursuitPokemon = allCommands[i].commandUser;
                    if (!allCommands[i].inProgress 
                        && !allCommands[i].completed 
                        && allCommands[i].commandType == BattleCommandType.Fight)
                    {
                        MoveData pursuitData = MoveDatabase.instance.GetMoveData(allCommands[i].moveID);
                        EffectDatabase.MoveEff.MoveEffect effect_ = pursuitData.GetEffectNew(MoveEffectType.Pursuit);
                        if (effect_ != null)
                        {
                            EffectDatabase.MoveEff.Pursuit effect = effect_ as EffectDatabase.MoveEff.Pursuit;
                            if ((effect.applyToEnemies && battle.ArePokemonEnemies(withdrawPokemon, pursuitPokemon))
                                || (effect.applyToAllies && battle.ArePokemonAllies(withdrawPokemon, pursuitPokemon)))
                            {
                                pursuitCommands.Add(allCommands[i]);
                            }
                        }
                    }
                }
        
                // we've found a pursuiter
                if (pursuitCommands.Count > 0)
                {
                    BattleCommand command = pursuitCommands[0];
                    command.bypassRedirection = true;
                    command.isPursuitMove = true;
                    command.targetPositions = new BattlePosition[] { battle.GetPokemonPosition(withdrawPokemon) };

                    // run the command
                    yield return StartCoroutine(ExecuteCommand(command));
                }
            }

            // can't switch out 
            if (battle.IsPokemonFainted(withdrawPokemon))
            {
                success = false;
            }

            // pokemon can switch out
            if (success)
            {
                // TODO: get battle properties for baton pass
                int position = withdrawPokemon.battlePos;
                Pokemon.BattleProperties withdrawBProps 
                    = Pokemon.BattleProperties.Clone(withdrawPokemon.bProps, withdrawPokemon);
                Model updateModel = Model.CloneModel(battle);

                executingBatonPass = true;
                batonPassReplaceCommands = new List<BattleCommand>();
                UpdateClients();
                if (!trainer.IsAIControlled())
                {
                    waitConnections.Add(trainer.playerID);
                    SendEventToPlayer(new Battle.View.Events.CommandReplacementPrompt
                    {
                        playerID = trainer.playerID,
                        fillPositions = new int[] { position }
                    }, trainer.playerID);
                
                    // wait for player to confirm choices
                    yield return StartCoroutine(WaitForPlayer());
                }
                else
                {
                    ai.UpdateModel(updateModel);
                    batonPassReplaceCommands = ai.GetReplacementsByPrompt(trainer, new List<int> { position }); 
                }

                battle.TrainerWithdrawPokemon(trainer, withdrawPokemon);
                UpdateClients();
                SendEvent(new Battle.View.Events.TrainerWithdraw
                {
                    playerID = trainer.playerID,
                    pokemonUniqueIDs = new List<string> { withdrawPokemon.uniqueID }
                });
                yield return StartCoroutine(UntiePokemon(withdrawPokemon));

                Pokemon switchInPokemon = batonPassReplaceCommands[0].switchInPokemon;
                battle.TrainerSwapPokemon(trainer, withdrawPokemon, switchInPokemon);
                executingBatonPass = false;

                // send out
                battle.TrainerSendPokemon(trainer, switchInPokemon, position);
                UpdateClients();
                SendEvent(new Battle.View.Events.TrainerSendOut
                {
                    playerID = trainer.playerID,
                    pokemonUniqueIDs = new List<string> { switchInPokemon.uniqueID }
                });
            
                // Baton Pass
                if (isBatonPassing)
                {
                    switchInPokemon.bProps.BatonPassFrom(withdrawBProps);
                }

                // Switch-in events (ex. abilities, stealth rock)
                yield return StartCoroutine(ApplyToPokemonSwitchInEvents(switchInPokemon));
            }
            callback(success);
            yield return null;
        }
    
        public IEnumerator ExecuteReflectedMove(
            Pokemon userPokemon,
            List<Pokemon> targetPokemon,
            MoveData moveData,
            BattleCommand originalCommand,
            bool forceOneHit = false
            )
        {
            if (targetPokemon.Count > 0)
            {
                bool reflectSuccessful = false;
                BattleCommand reflectCommand = null;

                // Magic Coat Reflect
                if (!reflectSuccessful
                    && battle.IsMagicCoated(userPokemon, targetPokemon, moveData)
                    && !originalCommand.isMagicCoatMove)
                {
                    // TODO: actually find out magic coat user
                    Pokemon magicCoatUser = targetPokemon[0];
                    reflectCommand = BattleCommand.CreateMoveCommand(
                        magicCoatUser,
                        moveData.ID,
                        battle.GetMoveAutoTargets(
                            pokemon: magicCoatUser, 
                            moveData: moveData,
                            biasPosition: battle.GetPokemonPosition(userPokemon)
                            ),
                        false
                        );
                    reflectCommand.isMagicCoatMove = true;
                    reflectSuccessful = true;

                    string magicCoatString = magicCoatUser.bProps.magicCoatMove;
                    magicCoatString = (magicCoatString == "DEFAULT") ? "move-magiccoat-default"  : magicCoatString;
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = magicCoatString,
                        pokemonUserID = magicCoatUser.uniqueID,
                        pokemonTargetID = userPokemon.uniqueID,
                        moveID = moveData.ID
                    });
                }
                if (reflectCommand != null)
                {
                    reflectCommand.bypassStatusInterrupt = true;
                    reflectCommand.consumePP = false;
                    reflectCommand.displayMove = false;
                    reflectCommand.forceOneHit = forceOneHit;
                    reflectCommand.isMoveReflected = true;
                    yield return StartCoroutine(ExecuteCommandMove(reflectCommand));
                }
            }
        }
        public IEnumerator TryToHijackMove(
            Pokemon userPokemon,
            List<Pokemon> targetPokemon,
            MoveData moveData,
            BattleCommand originalCommand,
            System.Action<bool> callback,
            bool forceOneHit = false,
            bool apply = true
            )
        {
            bool success = false;
            BattleCommand hijackCommand = null;
        
            // Snatch
            if (!success 
                && moveData.HasTag(MoveTag.Snatchable)
                && !originalCommand.isMoveSnatched)
            {
                List<Pokemon> snatchPokemon = battle.GetPokemonUsingSnatch(userPokemon);
                snatchPokemon = battle.GetPokemonBySpeed(snatchPokemon);
                if (snatchPokemon.Count > 0)
                {
                    // execute snatch
                    Pokemon hijacker = snatchPokemon[0];
                    MoveData hijackData = MoveDatabase.instance.GetMoveData(hijacker.bProps.snatchMove);

                    hijacker.bProps.snatchMove = (apply)? null : hijacker.bProps.snatchMove;
                    hijackCommand = BattleCommand.CreateMoveCommand(
                        hijacker,
                        moveData.ID,
                        battle.GetMoveAutoTargets(
                            pokemon: hijacker,
                            moveData: moveData
                            ),
                        false
                        );
                    hijackCommand.isMoveSnatched = true;

                    if (apply)
                    {
                        // text event
                        string textID = hijackData.GetEffect(MoveEffectType.Snatch).GetString(1);
                        textID = (textID == "DEFAULT") ? "move-snatch-success-default" : textID;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = textID,
                            pokemonUserID = hijacker.uniqueID,
                            pokemonTargetID = userPokemon.uniqueID,
                            moveID = moveData.ID
                        });
                    }
                    success = true;
                }
            }

            // 
            if (success && apply)
            {
                hijackCommand.bypassStatusInterrupt = true;
                hijackCommand.consumePP = false;
                hijackCommand.displayMove = false;
                hijackCommand.forceOneHit = forceOneHit;
                hijackCommand.isMoveHijacked = true;
                yield return StartCoroutine(ExecuteCommand(hijackCommand));
            }

            callback(success);
            yield return null;
        }
    
        // Status Conditions
        public IEnumerator ApplyStatusEffectsByTiming(
            Pokemon pokemon,
            StatusCondition condition,
            PokemonSETiming timing
            )
        {
            yield return StartCoroutine(ApplyStatusEffects(
                pokemon,
                condition,
                condition.data.GetEffectsFiltered(timing)
                ));
        }
        public IEnumerator ApplyStatusEffects(
            Pokemon pokemon,
            StatusCondition condition,
            List<PokemonCEff> effects
            )
        {
            for (int i = 0; i < effects.Count; i++)
            {
                yield return StartCoroutine(ApplyStatusEffect(
                    effect: effects[i],
                    condition: condition,
                    pokemon: pokemon
                    ));
            }
        }
        public IEnumerator ApplyStatusEffect(
            PokemonCEff effect,
            StatusCondition condition,
            Pokemon pokemon
            )
        {
            if (pokemon != null)
            {
                if (!battle.IsPokemonFainted(pokemon))
                {
                    // HP Loss
                    if (effect.effectType == PokemonSEType.HPLoss)
                    {
                        float hpPercentLost = effect.GetFloat(0);

                        // Toxic effect
                        PokemonCEff toxicEffect = condition.data.GetEffect(PokemonSEType.ToxicStack);
                        if (toxicEffect != null)
                        {
                            hpPercentLost += toxicEffect.GetFloat(0) * condition.turnsActive;
                        }

                        int preHP = pokemon.currentHP;
                        int damage = battle.GetPokemonHPByPercent(pokemon, hpPercentLost);
                        damage = Mathf.Max(1, damage);
                        int damageDealt = battle.SubtractPokemonHP(pokemon, damage);
                        int postHP = pokemon.currentHP;
                        string textID = effect.GetString(0);
                        textID = (textID == "DEFAULT") ? "status-hploss-default" : textID;
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = textID,
                            pokemonTargetID = pokemon.uniqueID,
                            statusID = condition.statusID
                        });

                        yield return StartCoroutine(PBPChangePokemonHP(
                            pokemon: pokemon,
                            preHP: preHP,
                            hpChange: damageDealt,
                            postHP: postHP,
                            checkFaint: true
                            ));
                    }
                }
            }
            yield return null;
        }
        public IEnumerator TryToInflictPokemonSC(
            string statusID,
            Pokemon targetPokemon,
            System.Action<bool> callback,
            bool bypassOtherConditions = false,
            int turnsLeft = -1,
            Pokemon userPokemon = null,
            MoveData inflictingMove = null,
            string inflictOverwrite = "",
            bool forceFailureMessage = false,
            bool apply = true,
            bool isSynchronized = false
            )
        {
            bool success = true;
            StatusPKData statusData = StatusPKDatabase.instance.GetStatusData(statusID);
            BattleTeam targetTeam = battle.GetTeam(targetPokemon);

            // run a bunch of checks here
            
            PBS.Battle.View.Events.MessageParameterized defaultFail = new PBS.Battle.View.Events.MessageParameterized
            {
                messageCode = statusData.failTextID,
                pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                pokemonTargetID = targetPokemon.uniqueID,
                statusID = statusID
            };
            PBS.Battle.View.Events.MessageParameterized defaultAlready = new PBS.Battle.View.Events.MessageParameterized
            {
                messageCode = statusData.alreadyTextID,
                pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                pokemonTargetID = targetPokemon.uniqueID,
                statusID = statusID
            };

            // safeguard check
            if (success)
            {
                if (userPokemon != targetPokemon)
                {
                    bool areAllies = false;
                    bool infiltrator = (userPokemon == null) ? false
                        : battle.PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.Infiltrator) != null;
                    if (userPokemon != null)
                    {
                        if (battle.ArePokemonAllies(userPokemon, targetPokemon))
                        {
                            areAllies = true;
                        }
                    }

                    if (!areAllies)
                    {
                        for (int i = 0; i < targetTeam.bProps.safeguards.Count; i++)
                        {
                            MoveData safeguardData = MoveDatabase.instance.GetMoveData(targetTeam.bProps.safeguards[i].moveID);
                            MoveEffect effect = safeguardData.GetEffect(MoveEffectType.Safeguard);
                            MoveEffect safeguardEffect = safeguardData.GetEffect(MoveEffectType.SafeguardStatus);
                            if (safeguardEffect != null)
                            {
                                if (!effect.GetBool(0) || !infiltrator)
                                {
                                    List<string> statuses = new List<string>(safeguardEffect.stringParams);
                                    statuses.RemoveAt(0);
                                    bool invertFilter = safeguardEffect.GetBool(0);
                                    bool nonVolatileOnly = safeguardEffect.GetBool(1);
                                    bool canBlock = true;

                                    if (canBlock && !statuses.Contains("ALL"))
                                    {
                                        canBlock = statuses.Contains(statusID) == !invertFilter;
                                    }
                                    if (canBlock
                                        && nonVolatileOnly
                                        && !statusData.HasTag(PokemonSTag.NonVolatile))
                                    {
                                        canBlock = false;
                                    }

                                    if (canBlock)
                                    {
                                        success = false;
                                        if (forceFailureMessage)
                                        {
                                            string textID = safeguardEffect.GetString(0);
                                            textID = (textID == "DEFAULT") ? "move-safeguard-protect" : textID;
                                            defaultFail.messageCode = textID;
                                            defaultFail.teamID = targetTeam.teamID;
                                            defaultFail.moveID = safeguardData.ID;
                                            SendEvent(defaultFail);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                
                }
            }

            // can't be inflicted if the target already has the status
            if (success 
                && targetPokemon.HasStatusCondition(statusID)
                && !bypassOtherConditions)
            {
                if (forceFailureMessage)
                {
                    SendEvent(defaultAlready);
                }
                success = false;
            }

            // can't stack non-volatile statuses
            if (success 
                && statusData.HasTag(PokemonSTag.NonVolatile)
                && !bypassOtherConditions)
            {
                if (targetPokemon.nonVolatileStatus.statusID != "healthy")
                {
                    success = false;
                    if (forceFailureMessage)
                    {
                        SendEvent(defaultFail);
                    }
                }
            }

            // can't stack confusion statuses
            if (success && statusData.GetEffect(PokemonSEType.Confusion) != null)
            {
                StatusCondition existingCondition = battle.GetConfusionStatusCondition(targetPokemon);
                if (existingCondition != null)
                {
                    PokemonCEff effect = statusData.GetEffect(PokemonSEType.Confusion);
                    if (effect != null)
                    {
                        success = false;
                        if (forceFailureMessage)
                        {
                            defaultAlready.messageCode = "status-already-confusion";
                            SendEvent(defaultAlready);
                        }
                    }
                }
            }

            // Sleep status
            if (success && statusData.GetEffect(PokemonSEType.Sleep) != null)
            {
                bool canSleep = true;

                // Uproar prevents sleep
                if (canSleep) 
                {
                    List<Pokemon> ablePokemon = battle.GetPokemonUnfaintedFrom(battle.pokemonOnField);
                    for (int i = 0; i < ablePokemon.Count; i++)
                    {
                        Pokemon pokemon = ablePokemon[i];
                        if (!string.IsNullOrEmpty(pokemon.bProps.uproarMove))
                        {
                            if (forceFailureMessage)
                            {
                                MoveData uproarData = MoveDatabase.instance.GetMoveData(pokemon.bProps.uproarMove);
                                MoveEffect effect = uproarData.GetEffect(MoveEffectType.Uproar);
                                defaultFail.messageCode = "status-uproar-sleep-default";
                                defaultFail.moveID = uproarData.ID;
                                SendEvent(defaultFail);
                            }

                            canSleep = false;
                            break;
                        }
                    }
                }

                // existing sleep
                if (canSleep)
                {
                    StatusCondition existingCondition = 
                        battle.GetPokemonFilteredStatus(targetPokemon, PokemonSEType.Sleep);
                    if (existingCondition != null)
                    {
                        PokemonCEff effect = statusData.GetEffect(PokemonSEType.Sleep);
                        if (effect != null)
                        {
                            success = false;
                            if (forceFailureMessage)
                            {
                                defaultAlready.messageCode = "status-already-sleep";
                                SendEvent(defaultAlready);
                            }
                        }
                    }
                }
            
                success = canSleep;
            }

            // type immunity?
            if (success)
            {
                bool bypassType = false;

                // Corrosion bypasses type immunities
                if (!bypassType && userPokemon != null)
                {
                    AbilityEffect corrosionEffect = battle.PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.Corrosion);
                    if (corrosionEffect != null)
                    {
                        List<string> applicableStatus = new List<string>(corrosionEffect.stringParams);
                        if (applicableStatus.Contains(statusID))
                        {
                            bypassType = true;
                        }
                    }
                }

                // TODO: moves that bypass status type immunities

                // Poison & Steel can't be poisoned
                // Fire can't be burned
                // Electric can't be paralyzed, etc.
                PokemonCEff effect = statusData.GetEffect(PokemonSEType.TypeImmunity);
                if (!bypassType && effect != null)
                {
                    if (battle.DoesPokemonHaveATypeInList(targetPokemon, effect.stringParams))
                    {
                        success = false;
                        if (forceFailureMessage)
                        {
                            SendEvent(defaultFail);
                        }
                    }
                }
            }

            // ability immunity?
            if (success)
            {
                bool bypassAbility = false;

                // Mold Breaker bypasses ability immunities
                if (!bypassAbility && userPokemon != null)
                {
                    AbilityEffect moldBreakerEffect = 
                        battle.PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.MoldBreaker);
                    if (moldBreakerEffect != null)
                    {
                        bypassAbility = true;
                    }
                }

                // Sunsteel Strike bypasses ability immunities
                if (!bypassAbility && inflictingMove != null)
                {
                    MoveEffect effect = inflictingMove.GetEffect(MoveEffectType.SunteelStrike);
                    if (effect != null)
                    {
                        bypassAbility = true;
                    }
                }

                // Immunity / Limber
                AbilityData ability = battle.PBPLegacyGetAbilityDataWithEffect(
                    targetPokemon, 
                    AbilityEffectType.Immunity, 
                    bypassAbility);
                if (ability != null)
                {
                    AbilityEffect effect = ability.GetEffect(AbilityEffectType.Immunity);
                    List<string> applicableStatus = new List<string>(effect.stringParams);
                    if (applicableStatus.Contains(statusID))
                    {
                        success = false;
                        if (forceFailureMessage)
                        {
                            PBPShowAbility(pokemon: targetPokemon, abilityData: ability);
                            SendEvent(defaultFail);
                        }
                    }
                }
            }

            // passed all checks, so apply status
            if (success && apply)
            {
                if (statusData.HasTag(PokemonSTag.NonStick))
                {
                    StatusCondition condition = battle.ApplyStatusCondition(
                        pokemon: targetPokemon,
                        statusID: statusID,
                        turnsLeft: turnsLeft);
                    
                    string inflictText = (inflictOverwrite == "") ? condition.data.inflictTextID : inflictOverwrite;
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = inflictText,
                        pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                        pokemonTargetID = targetPokemon.uniqueID
                    });
                }

                // Status-Triggered Items
                Item statusTriggerItem = battle.GetPokemonItemFiltered(targetPokemon, ItemEffectType.TriggerOnStatus);
                if (statusTriggerItem != null)
                {
                    ItemEffect statusTrigger = statusTriggerItem.data.GetEffect(ItemEffectType.TriggerOnStatus);
                    bool willConsume = false;

                    ItemEffect healEffect = statusTriggerItem.data.GetEffect(ItemEffectType.HealStatus);
                    if (!willConsume && healEffect != null)
                    {

                    }

                    if (willConsume)
                    {
                        yield return StartCoroutine(TryToConsumeItem(
                            pokemon: targetPokemon,
                            holderPokemon: targetPokemon,
                            item: statusTriggerItem,
                            (result) => 
                            {
                            
                            }
                            ));
                    }
                }
            }
        
            callback(success);
            yield return null;
        }
        public IEnumerator HealPokemonSC(
            Pokemon targetPokemon,
            StatusCondition condition,
            Pokemon healerPokemon = null,
            string overwriteText = null
            )
        {
            battle.HealStatusCondition(targetPokemon, condition);
            string healText = (overwriteText == null) ? condition.data.healTextID
                : overwriteText;
            SendEvent(new Battle.View.Events.MessageParameterized
            {
                messageCode = healText,
                pokemonUserID = (healerPokemon == null)? "" : healerPokemon.uniqueID,
                pokemonTargetID = targetPokemon.uniqueID,
                statusID = condition.statusID
            });
            yield return null;
        }

        // Team Status Conditions
        public IEnumerator ApplyTeamStatusEffectsByTiming(
            BattleTeam team,
            TeamCondition condition,
            TeamSETiming timing
            )
        {
            yield return StartCoroutine(ApplyTeamStatusEffects(
                team,
                condition,
                condition.data.GetEffectsFiltered(timing)
                ));
        }
        public IEnumerator ApplyTeamStatusEffects(
            BattleTeam team,
            TeamCondition condition,
            List<TeamCEff> effects
            )
        {
            for (int i = 0; i < effects.Count; i++)
            {
                yield return StartCoroutine(ApplyTeamStatusEffect(
                    effect: effects[i],
                    condition: condition,
                    team: team
                    ));
            }
        }
        public IEnumerator ApplyTeamStatusEffect(
            TeamCEff effect,
            TeamCondition condition,
            BattleTeam team
            )
        {
            if (team != null)
            {

            }
            yield return null;
        }
        public IEnumerator TryToInflictTeamSC(
            string statusID,
            BattleTeam targetTeam,
            System.Action<bool> callback,
            int turnsLeft = -1,
            Pokemon userPokemon = null,
            MoveData inflictingMove = null,
            string inflictOverwrite = null,
            bool forceNoText = false,
            bool forceFailureMessage = false,
            bool apply = true
            )
        {
            forceFailureMessage = (forceNoText) ? false : forceFailureMessage;
            bool success = true;
            StatusTEData statusData = StatusTEDatabase.instance.GetStatusData(statusID);

            // can't be inflicted if the target already has the status
            if (success && targetTeam.HasStatusCondition(statusID))
            {
                if (forceFailureMessage)
                {
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = statusData.alreadyTextID,
                        pokemonUserID = userPokemon.uniqueID,
                        teamID = targetTeam.teamID,
                        statusTeamID = statusID
                    });
                }
                success = false;
            }

            // passed all checks, so apply status
            if (success && apply)
            {
                TeamCondition condition = battle.ApplyTeamSC(
                    team: targetTeam,
                    statusID: statusID,
                    turnsLeft: turnsLeft);

                string inflictText = (inflictOverwrite == null) ? condition.data.inflictTextID : inflictOverwrite;
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = inflictText,
                    pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                    teamID = targetTeam.teamID,
                    statusTeamID = condition.statusID
                });
            }

            callback(success);
            yield return null;
        }
        public IEnumerator HealTeamSC(
            BattleTeam targetTeam,
            TeamCondition condition,
            Pokemon healerPokemon = null,
            string overwriteText = null
            )
        {
            battle.HealTeamSC(targetTeam, condition);
            string healText = (overwriteText == null) ? condition.data.endTextID : overwriteText;
            SendEvent(new Battle.View.Events.MessageParameterized
            {
                messageCode = healText,
                pokemonUserID = (healerPokemon == null)? "" : healerPokemon.uniqueID,
                teamID = targetTeam.teamID,
                statusTeamID = condition.statusID
            });
            yield return null;
        }

    
        public IEnumerator TryToInflictBattleSC(
            string statusID,
            System.Action<bool> callback,
            int turnsLeft = -1,
            Pokemon userPokemon = null,
            MoveData inflictingMove = null,
            string inflictOverwrite = null,
            bool forceFailureMessage = false,
            bool forceNoText = false,
            bool apply = true
            )
        {
            bool success = true;
            StatusBTLData statusData = StatusBTLDatabase.instance.GetStatusData(statusID);

            // Can't inflict status if it's already active
            if (success && battle.HasStatusCondition(statusID))
            {
                // rooms undoes itself
                if (statusData.HasTag(BattleSTag.UndoesSelf))
                {
                    BattleCondition undoCondition = battle.BBPGetSC(statusID);
                    yield return StartCoroutine(HealBattleSC(
                        condition: undoCondition,
                        healerPokemon: userPokemon
                        ));
                }
                else
                {
                    if (forceFailureMessage)
                    {
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = statusData.alreadyTextID,
                            pokemonUserID = userPokemon.uniqueID,
                            statusID = statusID
                        });
                    }
                }
                success = false;
            }

            BattleCondition higherPriorityCondition = null;
            // Can't inflict lower priority weather
            if (success && statusData.GetEffect(BattleSEType.Weather) != null)
            {
                BattleCEff effect = statusData.GetEffect(BattleSEType.Weather);
                int newPriority = Mathf.FloorToInt(effect.GetFloat(0));
                if (battle.weather.data.GetEffect(BattleSEType.Weather) != null)
                {
                    BattleCEff oldEffect = battle.weather.data.GetEffect(BattleSEType.Weather);
                    int oldPriority = Mathf.FloorToInt(oldEffect.GetFloat(0));
                    if (newPriority < oldPriority)
                    {
                        success = false;
                        higherPriorityCondition = battle.weather;
                    }
                }
            }

            // Can't inflict lower priority terrain
            if (success && statusData.GetEffect(BattleSEType.Terrain) != null)
            {
                BattleCEff effect = statusData.GetEffect(BattleSEType.Terrain);
                int newPriority = Mathf.FloorToInt(effect.GetFloat(0));
                if (battle.terrain.data.GetEffect(BattleSEType.Terrain) != null)
                {
                    BattleCEff oldEffect = battle.terrain.data.GetEffect(BattleSEType.Terrain);
                    int oldPriority = Mathf.FloorToInt(oldEffect.GetFloat(0));
                    if (newPriority < oldPriority)
                    {
                        success = false;
                        higherPriorityCondition = battle.terrain;
                    }
                }
            }

            // Can't inflict lower priority gravity
            if (success && statusData.GetEffect(BattleSEType.Gravity) != null)
            {
                BattleCEff effect = statusData.GetEffect(BattleSEType.Gravity);
                int newPriority = Mathf.FloorToInt(effect.GetFloat(0));
                if (battle.gravity.data.GetEffect(BattleSEType.Gravity) != null)
                {
                    BattleCEff oldEffect = battle.gravity.data.GetEffect(BattleSEType.Gravity);
                    int oldPriority = Mathf.FloorToInt(oldEffect.GetFloat(0));
                    if (newPriority < oldPriority)
                    {
                        success = false;
                        higherPriorityCondition = battle.gravity;
                    }
                }
            }

            // Can't inflict lower priority rooms
            if (success && statusData.GetEffect(BattleSEType.MagicRoom) != null)
            {
                BattleCEff effect = statusData.GetEffect(BattleSEType.MagicRoom);
                int newPriority = Mathf.FloorToInt(effect.GetFloat(0));
                if (battle.magicRoom != null)
                {
                    BattleCEff oldEffect = battle.magicRoom.data.GetEffect(BattleSEType.MagicRoom);
                    int oldPriority = Mathf.FloorToInt(oldEffect.GetFloat(0));
                    if (newPriority < oldPriority)
                    {
                        success = false;
                        higherPriorityCondition = battle.magicRoom;
                    }
                }
            }
            if (success && statusData.GetEffect(BattleSEType.TrickRoom) != null)
            {
                BattleCEff effect = statusData.GetEffect(BattleSEType.TrickRoom);
                int newPriority = Mathf.FloorToInt(effect.GetFloat(0));
                if (battle.trickRoom != null)
                {
                    BattleCEff oldEffect = battle.trickRoom.data.GetEffect(BattleSEType.TrickRoom);
                    int oldPriority = Mathf.FloorToInt(oldEffect.GetFloat(0));
                    if (newPriority < oldPriority)
                    {
                        success = false;
                        higherPriorityCondition = battle.trickRoom;
                    }
                }
            }
            if (success && statusData.GetEffect(BattleSEType.WonderRoom) != null)
            {
                BattleCEff effect = statusData.GetEffect(BattleSEType.WonderRoom);
                int newPriority = Mathf.FloorToInt(effect.GetFloat(0));
                if (battle.wonderRoom != null)
                {
                    BattleCEff oldEffect = battle.wonderRoom.data.GetEffect(BattleSEType.WonderRoom);
                    int oldPriority = Mathf.FloorToInt(oldEffect.GetFloat(0));
                    if (newPriority < oldPriority)
                    {
                        success = false;
                        higherPriorityCondition = battle.wonderRoom;
                    }
                }
            }

            if (!success && forceFailureMessage)
            {
                if (higherPriorityCondition != null)
                {
                    BattleCEff priorityBlock = 
                        higherPriorityCondition.data.GetEffect(BattleSEType.BlockLowerPriorityCondition);
                    if (priorityBlock != null)
                    {
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = priorityBlock.GetString(0),
                            pokemonUserID = (userPokemon == null)? "" : userPokemon.uniqueID,
                            statusID = higherPriorityCondition.statusID
                        });
                    }
                }
            }

            // passed all checks, so apply status
            if (success && apply)
            {
                BattleCondition condition = battle.InflictBattleSC(statusID: statusID, turnsLeft: turnsLeft);

                string inflictText = (inflictOverwrite == null) ? condition.data.inflictTextID : inflictOverwrite;
                inflictText = (forceNoText) ? null : inflictText;
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = inflictText,
                    pokemonUserID = (userPokemon == null)? "" : userPokemon.nickname,
                    statusID = condition.statusID
                });

                yield return StartCoroutine(ExecuteBattleSEsByTiming(
                    statusData: condition.data,
                    timing: BattleSETiming.OnStart
                    ));
            }

            callback(success);
            yield return null;
        }
        public IEnumerator HealBattleSC(
            BattleCondition condition,
            Pokemon healerPokemon = null,
            string overwriteText = ""
            )
        {
            battle.HealBattleSC(condition);
            string healText = (overwriteText == "") ? condition.data.endTextID : overwriteText;
            SendEvent(new Battle.View.Events.MessageParameterized
            {
                messageCode = healText,
                pokemonUserID = (healerPokemon == null)? "" : healerPokemon.uniqueID,
                statusEnvironmentID = condition.statusID
            });
        
            // Forecast Check
            yield return StartCoroutine(PBPRunBCAbilityCheck());

            yield return null;
        }

        // Stat Stage Effects
        public IEnumerator TryToApplyStatStageMods(
            List<PokemonStats> statsToModify,
            int modValue,
            Pokemon targetPokemon,
            System.Action<bool> callback,
            Pokemon userPokemon = null,
            MoveData moveData = null,
            bool maximize = false,
            bool minimize = false,
            string textCode = "",
            bool forceFailureMessage = false,
            bool apply = true
            )
        {
            forceFailureMessage = (apply) ? forceFailureMessage : false;

            BattleTeam targetTeam = battle.GetTeam(targetPokemon);
            BattleTeam userTeam = (userPokemon == null) ? null : battle.GetTeam(userPokemon);

            // Get the correct stat mod value (factor in abilities, etc.)
            AbilityData targetAbilityData = battle.PBPGetAbilityData(targetPokemon);
            bool showAbility = false;

            // Contrary
            if (targetAbilityData != null)
            {
                AbilityEffect contraryEffect = targetAbilityData.GetEffect(AbilityEffectType.Contrary);
                if (contraryEffect != null)
                {
                    showAbility = true;
                    modValue = -modValue;
                    if (maximize)
                    {
                        maximize = false;
                        minimize = true;
                    }
                    else if (minimize)
                    {
                        maximize = true;
                        minimize = false;
                    }
                }
            }

            modValue = (maximize) ? GameSettings.GetMaxStatBoost() : modValue;
            modValue = (minimize) ? GameSettings.GetMinStatBoost() : modValue;

            // Simple
            if (targetAbilityData != null)
            {
                AbilityEffect simpleEffect = targetAbilityData.GetEffect(AbilityEffectType.Simple);
                if (simpleEffect != null
                    && !maximize
                    && !minimize)
                {
                    int simpleFactor = Mathf.FloorToInt(simpleEffect.GetFloat(0));
                    modValue *= simpleFactor;
                    showAbility = true;
                }
            }

            // Growth
            if (moveData != null)
            {
                MoveEffect effect = moveData.GetEffect(MoveEffectType.StatStageGrowth);
                if (effect != null)
                {
                    List<string> weatherList = new List<string>(effect.stringParams);
                    if (weatherList.Contains(battle.weather.statusID))
                    {
                        modValue *= Mathf.FloorToInt(effect.GetFloat(0));
                    }
                }
            }

            // run a bunch of checks here
            bool success = true;

            // Mist
            if (success && modValue < 0)
            {
                if (userPokemon != targetPokemon)
                {
                    bool areAllies = false;
                    bool infiltrator = (userPokemon == null) ? false
                        : battle.PBPLegacyGetAbilityEffect(userPokemon, AbilityEffectType.Infiltrator) != null;
                    if (userPokemon != null)
                    {
                        if (battle.ArePokemonAllies(userPokemon, targetPokemon))
                        {
                            areAllies = true;
                        }
                    }
            
                    if (!areAllies)
                    {
                        for (int i = 0; i < targetTeam.bProps.safeguards.Count; i++)
                        {
                            MoveData safeguardData = MoveDatabase.instance.GetMoveData(targetTeam.bProps.safeguards[i].moveID);
                            MoveEffect effect = safeguardData.GetEffect(MoveEffectType.Safeguard);
                            MoveEffect mistEffect = safeguardData.GetEffect(MoveEffectType.SafeguardMist);
                            if (mistEffect != null)
                            {
                                if (!effect.GetBool(0) || !infiltrator)
                                {
                                    List<string> tempStats = new List<string>();
                                    for (int k = 1; k < mistEffect.stringParams.Length; k++)
                                    {
                                        tempStats.Add(mistEffect.stringParams[k]);
                                    }

                                    List<PokemonStats> stats = GameTextDatabase.GetStatsFromList(tempStats.ToArray());
                                    List<PokemonStats> removedStats = new List<PokemonStats>();
                                    for (int k = 0; k < stats.Count; k++)
                                    {
                                        if (statsToModify.Contains(stats[k]))
                                        {
                                            statsToModify.Remove(stats[k]);
                                            removedStats.Add(stats[k]);
                                        }
                                    }

                                    if (removedStats.Count > 0 && forceFailureMessage)
                                    {
                                        string textID = mistEffect.GetString(0);
                                        textID = (textID == "DEFAULT") ? "move-mist-protect" : textID;
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = textID,
                                            pokemonTargetID = targetPokemon.uniqueID,
                                            teamID = targetTeam.teamID,
                                            moveID = safeguardData.ID,
                                            statList = new List<PokemonStats>(removedStats)
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // TODO: Clear Body, Hyper Cutter, etc. (prevent lowering of certain stats)

            // don't continue if there's no stats to modify
            if (success)
            {
                if (statsToModify.Count == 0)
                {
                    success = false;
                }
            } 

            // check if each stat can actually be modified
            if (success)
            {
                success = false;
                for (int i = 0; i < statsToModify.Count; i++)
                {
                    PokemonStats currentStat = statsToModify[i];
                    int currentMod = battle.GetPokemonStatStage(targetPokemon, currentStat);

                    int afterMod = currentMod + modValue;
                    afterMod = (afterMod >= GameSettings.btlStatStageMax) ? GameSettings.btlStatStageMax
                        : (afterMod <= GameSettings.btlStatStageMin) ? GameSettings.btlStatStageMin
                        : afterMod;
                    int realMod = afterMod - currentMod;

                    // if one stat is modifiable, we can modify stats
                    if (realMod != 0)
                    {
                        success = true;
                    }
                }
                if (!success 
                    && forceFailureMessage
                    && apply)
                {
                    bool tooHigh = modValue > 0;
                    SendEvent(new Battle.View.Events.PokemonStatUnchangeable
                    {
                        pokemonUniqueID = targetPokemon.uniqueID,
                        tooHigh = tooHigh,
                        statsToMod = new List<PokemonStats>(statsToModify)
                    });
                }
            }
        
            // modify stats
            List<int> statMods = new List<int>();
            Dictionary<int, List<PokemonStats>> statModMap = new Dictionary<int, List<PokemonStats>>();
            if (success && apply)
            {
                // commit to modifying stats
                for (int i = 0; i < statsToModify.Count; i++)
                {
                    PokemonStats currentStat = statsToModify[i];
                    int currentMod = battle.GetPokemonStatStage(targetPokemon, currentStat);

                    int afterMod = currentMod + modValue;
                    afterMod = (afterMod >= GameSettings.btlStatStageMax) ? GameSettings.btlStatStageMax
                        : (afterMod <= GameSettings.btlStatStageMin) ? GameSettings.btlStatStageMin
                        : afterMod;
                    int realMod = afterMod - currentMod;

                    if (maximize || minimize)
                    {
                        realMod = modValue;
                    }
                    if (statModMap.ContainsKey(realMod))
                    {
                        statModMap[realMod].Add(currentStat);
                    }
                    else
                    {
                        statModMap.Add(realMod, new List<PokemonStats> { currentStat });
                        statMods.Add(realMod);
                    }

                    if (realMod != 0)
                    {
                        battle.SetStatStage(targetPokemon, currentStat, afterMod);
                    }
                }

                // create the stat change events
                bool runAbility = false;
                bool statsWereModded = false;
                List<PBS.Battle.View.Events.PokemonStatChange> statStageEvents 
                    = new List<PBS.Battle.View.Events.PokemonStatChange>();
                for (int i = 0; i < statMods.Count; i++)
                {
                    int mod = statMods[i];
                    if (mod != 0)
                    {
                        statsWereModded = true;
                        if (!runAbility && showAbility)
                        {
                            runAbility = true;
                        }
                        // create stat event
                        PBS.Battle.View.Events.PokemonStatChange statStageEvent 
                            = new PBS.Battle.View.Events.PokemonStatChange
                            {
                                pokemonUniqueID = targetPokemon.uniqueID,
                                modValue = mod,
                                maximize = maximize,
                                minimize = minimize,
                                statsToMod = new List<PokemonStats>(statModMap[mod])
                            };
                        statStageEvents.Add(statStageEvent);
                    }
                }

                // run ability animations
                if (runAbility)
                {
                    PBPShowAbility(pokemon: targetPokemon, abilityData: targetAbilityData);
                }

                // send message if stats were modded
                if (statsWereModded)
                {
                    // run individual stat stage events
                    for (int i = 0; i < statStageEvents.Count; i++)
                    {
                        SendEvent(statStageEvents[i]);
                    }
                }
            }
        
            callback(success);
            yield return null;
        }

        public IEnumerator TryToConsumeItem(
            Pokemon pokemon,
            Pokemon holderPokemon,
            Item item,
            System.Action<bool> callback,
            bool bypassChecks = false,
            string consumeText = "",
            string typeID = null,
            bool apply = true)
        {
            bool canConsume = true;

            if (canConsume)
            {
                if (!battle.IsPokemonOnField(pokemon) || battle.IsPokemonFainted(pokemon))
                {
                    canConsume = false;
                }
            }

            // Unnerve
            if (canConsume
                && !bypassChecks
                && item.data.pocket == ItemPocket.Berries
                && pokemon == holderPokemon)
            {
                if (!battle.CanPokemonConsumeBerries(pokemon))
                {
                    canConsume = false;
                }
            }

            if (canConsume && apply)
            {
                yield return StartCoroutine(ConsumeItem(
                    pokemon: pokemon,
                    item: item,
                    holderPokemon: holderPokemon,
                    consumeText: consumeText,
                    typeID: typeID,
                    apply: apply,
                    callback: (result) =>
                    {

                    }
                    ));
            }

            callback(canConsume);
            yield return null;
        }

        public IEnumerator ConsumeItem(
            Pokemon pokemon,
            Item item,
            System.Action<bool> callback,
            Pokemon holderPokemon = null,
            string consumeText = "",
            string typeID = null,
            bool apply = true)
        {
            bool itemSuccess = false;

            // Only record consuming the item if it was a held item
            if (holderPokemon != null)
            {
                if (item.data.HasTag(ItemTag.Consumable) && apply)
                {
                    pokemon.bProps.consumedItem = item.itemID;
                    if (item.data.pocket == ItemPocket.Berries)
                    {
                        pokemon.bProps.consumedBerry = item.itemID;
                    }

                    if (!item.data.HasTag(ItemTag.StaysOnConsume))
                    {
                        holderPokemon.UnsetItem(item);
                    }
                }
            }

            // Anim / Text here
            if (apply)
            {
                string textID = (consumeText == "") ? "item-consume-default" : consumeText;
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = textID,
                    pokemonUserID = pokemon.uniqueID,
                    pokemonTargetID = holderPokemon.uniqueID,
                    itemID = item.itemID,
                    typeID = typeID
                });
            }

            // Legacy
            yield return StartCoroutine(ApplyItemEffects(
                pokemon: pokemon,
                item: item,
                apply: apply,
                callback: (result) =>
                {
                    if (result)
                    {
                        itemSuccess = true;
                    }
                }
                ));

            // Ripen
            List<Pokemon.Ability> ripenAbilities =
                battle.PBPGetAbilitiesWithEffect(pokemon, AbilityEffectType.Ripen);
            float ripenMultiplier = 1f;
            for (int i = 0; i < ripenAbilities.Count; i++)
            {
                Pokemon.Ability ability = ripenAbilities[i];
                EffectDatabase.AbilityEff.AbilityEffect ripen_ =
                    ability.data.GetEffectNew(AbilityEffectType.Ripen);
                if (ripen_ != null)
                {
                    EffectDatabase.AbilityEff.Ripen ripen = ripen_ as EffectDatabase.AbilityEff.Ripen;
                    if (battle.DoEffectFiltersPass(
                        filters: ripen.filters,
                        targetPokemon: pokemon,
                        item: item
                        ))
                    {
                        if (apply)
                        {
                            PBPShowAbility(pokemon, ability);
                        }
                        ripenMultiplier *= ripen.effectMultiplier;
                    }
                }
            }

            // Run consume item-effects here
            yield return StartCoroutine(ApplyItemEffects(
                pokemon: pokemon,
                item: item,
                apply: apply,
                effects: item.data.GetEffectsOnConsume(),
                ripenMultiplier: ripenMultiplier,
                callback: (result) =>
                {
                    if (result)
                    {
                        itemSuccess = true;
                    }
                }
                ));

            // Cheek Pouch
            if (battle.IsPokemonOnFieldAndAble(pokemon))
            {
                List<Pokemon.Ability> abilities = battle.PBPGetAbilities(pokemon);
                for (int i = 0; i < abilities.Count; i++)
                {
                    EffectDatabase.AbilityEff.AbilityEffect cheekPouch_ =
                        abilities[i].data.GetEffectNew(AbilityEffectType.CheekPouch);
                    if (cheekPouch_ != null)
                    {
                        EffectDatabase.AbilityEff.CheekPouch cheekPouch =
                            cheekPouch_ as EffectDatabase.AbilityEff.CheekPouch;

                        bool active = false;
                        if (cheekPouch.onBerry && item.data.pocket == ItemPocket.Berries)
                        {
                            active = true;
                        }

                        if (active)
                        {
                            // Regain HP
                            if (cheekPouch.hpGainPercent > 0 && battle.GetPokemonHPAsPercentage(pokemon) < 1f)
                            {
                                PBPShowAbility(pokemon: pokemon, abilityData: abilities[i].data);

                                int preHP = pokemon.currentHP;
                                int HPGain = battle.GetPokemonHPByPercent(pokemon, cheekPouch.hpGainPercent);
                                yield return StartCoroutine(PBPHealPokemon(
                                    pokemon: pokemon,
                                    HPToAdd: HPGain
                                    ));
                            }
                        }
                    }
                }
            }

            // Symbiosis
            Item curItem = battle.PBPGetHeldItem(pokemon);
            if (curItem == null)
            {
                bool triggeredSymbiosis = false;
                List<Pokemon> allyPokemon = battle.GetAllyPokemon(pokemon);
                for (int i = 0; i < allyPokemon.Count && !triggeredSymbiosis; i++)
                {
                    Pokemon curAlly = allyPokemon[i];
                    List<Pokemon.AbilityEffectPair> symbiosisPairs = 
                        battle.PBPGetAbilityEffectPairs(curAlly, AbilityEffectType.Symbiosis);
                    Item targetItem = battle.PBPGetHeldItem(curAlly);
                    if (targetItem != null && symbiosisPairs.Count > 0)
                    {
                        if (battle.CanPokemonItemBeLost(curAlly, targetItem)
                            && battle.CanPokemonItemBeGained(pokemon, targetItem))
                        {
                            for (int k = 0; k < symbiosisPairs.Count && !triggeredSymbiosis; k++)
                            {
                                Pokemon.AbilityEffectPair symbiosisPair = symbiosisPairs[k] as Pokemon.AbilityEffectPair;
                                EffectDatabase.AbilityEff.Symbiosis symbiosis = 
                                    symbiosisPair.effect as EffectDatabase.AbilityEff.Symbiosis;
                                if (battle.DoEffectFiltersPass(
                                    filters: symbiosis.filters,
                                    userPokemon: curAlly,
                                    targetPokemon: pokemon,
                                    item: targetItem
                                    ))
                                {
                                    triggeredSymbiosis = true;
                                    if (apply)
                                    {
                                        PBPShowAbility(curAlly, symbiosisPair.ability);
                                        pokemon.SetItem(targetItem);
                                        curAlly.UnsetItem(targetItem);
                                        SendEvent(new Battle.View.Events.MessageParameterized
                                        {
                                            messageCode = symbiosis.displayText,
                                            pokemonUserID = curAlly.uniqueID,
                                            pokemonTargetID = pokemon.uniqueID,
                                            itemID = targetItem.itemID
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }

            callback(itemSuccess);
            yield return null;
        }

        public IEnumerator ApplyItemEffects(
            Pokemon pokemon,
            Item item,
            List<EffectDatabase.ItemEff.ItemEffect> effects,
            System.Action<bool> callback,
            float ripenMultiplier = 1f,
            bool apply = true)
        {
            bool success = false;
            for (int i = 0; i < effects.Count; i++)
            {
                yield return StartCoroutine(ApplyItemEffect(
                    pokemon: pokemon,
                    item: item,
                    effect_: effects[i],
                    ripenMultiplier: ripenMultiplier,
                    apply: apply,
                    callback: (result) =>
                    {
                        if (result)
                        {
                            success = result;
                        }
                    }
                    ));
            }
            callback(success);
        }
        public IEnumerator ApplyItemEffect(
            Pokemon pokemon,
            Item item,
            EffectDatabase.ItemEff.ItemEffect effect_,
            System.Action<bool> callback,
            float ripenMultiplier = 1f,
            bool apply = true)
        {
            bool effectSuccess = false;
            bool forceEffectDisplay = true;

            if (pokemon != null)
            {
                // Heal HP
                if (effect_ is EffectDatabase.ItemEff.Potion)
                {
                    EffectDatabase.ItemEff.Potion effect = effect_ as EffectDatabase.ItemEff.Potion;
                    yield return StartCoroutine(ApplyHealHP(
                        healHP: effect.healHP,
                        scaleAmount: ripenMultiplier,
                        targetPokemon: pokemon,
                        forceFailMessage: forceEffectDisplay,
                        apply: apply,
                        callback: (result) =>
                        {
                            effectSuccess = result;
                        }
                        ));
                }
                // Liechi Berry (Stat Stage Mod)
                else if (effect_ is EffectDatabase.ItemEff.LiechiBerry)
                {
                    EffectDatabase.ItemEff.LiechiBerry liechiBerry = effect_ as EffectDatabase.ItemEff.LiechiBerry;
                    EffectDatabase.General.StatStageMod statStageMod =
                        liechiBerry.statStageMod.Clone();
                    List<PokemonStats> statTypes = GameSettings.btlPkmnStats;
                    for (int i = 0; i < statTypes.Count; i++)
                    {
                        statStageMod.ScaleStatMod(statTypes[i], ripenMultiplier);
                    }
                    yield return StartCoroutine(ApplyStatStageMod(
                        statStageMod: statStageMod,
                        targetPokemon: pokemon,
                        apply: apply,
                        callback: (result) => { }
                        ));
                }
            }
            callback(effectSuccess);
            yield return null;
        }

        public IEnumerator ApplyItemEffects(
            Pokemon pokemon,
            Item item,
            System.Action<bool> callback,
            bool apply = true)
        {
            bool effectSuccess = false;
            ItemEffect[] effects = item.data.effects;
            for (int i = 0; i < effects.Length; i++)
            {
                yield return StartCoroutine(ApplyItemEffect(
                    pokemon,
                    item,
                    effects[i],
                    (result) =>
                    {
                        if (result)
                        {
                            effectSuccess = true;
                        }
                    },
                    apply: apply
                    ));
            }
            callback(effectSuccess);
        }
        public IEnumerator ApplyItemEffect(
            Pokemon pokemon, 
            Item item, 
            ItemEffect effect, 
            System.Action<bool> callback,
            bool apply = true)
        {
            bool effectSuccess = false;
            bool forceEffectDisplay = true;

            if (pokemon != null)
            {
                if (!battle.IsPokemonFainted(pokemon))
                {
                    // Heal HP
                    if (effect.effectType == ItemEffectType.Potion
                        || effect.effectType == ItemEffectType.HealPercent
                        || effect.effectType == ItemEffectType.Revive)
                    {
                        bool canRecover = true;
                        int HPGain =
                            (effect.effectType == ItemEffectType.Potion) ? Mathf.FloorToInt(effect.GetFloat(0))
                            : battle.GetPokemonHPByPercent(pokemon, effect.GetFloat(0));
                        int preHP = pokemon.currentHP;
                        int recoverableHP = pokemon.HPdifference;

                        if (recoverableHP == 0)
                        {
                            canRecover = false;
                            if (forceEffectDisplay)
                            {
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = "pokemon-heal-fail",
                                    pokemonTargetID = pokemon.uniqueID
                                });
                            }
                        }
                        if (canRecover && apply)
                        {
                            int hpRecovered = battle.AddPokemonHP(pokemon, HPGain);
                            int postHP = pokemon.currentHP;
                            yield return StartCoroutine(PBPChangePokemonHP(
                                pokemon: pokemon,
                                preHP: preHP,
                                hpChange: hpRecovered,
                                postHP: postHP,
                                heal: true
                            ));
                        }
                        effectSuccess = canRecover;
                    }
                    // Heal Status
                    if (effect.effectType == ItemEffectType.HealStatus)
                    {
                        List<string> statusThatCanHeal = new List<string>(effect.stringParams);

                        List<StatusCondition> conditions = battle.GetPokemonStatusConditions(pokemon);
                        for (int i = 0; i < conditions.Count; i++)
                        {
                            if (statusThatCanHeal.Contains(conditions[i].statusID))
                            {
                                if (apply)
                                {
                                    yield return StartCoroutine(HealPokemonSC(
                                        targetPokemon: pokemon,
                                        condition: conditions[i]
                                        ));
                                }
                                effectSuccess = true;
                            }
                        }
                    }
                    // Heal Non-Volatile Status
                    else if (effect.effectType == ItemEffectType.HealStatusNonVolatile)
                    {
                        List<StatusCondition> conditions = battle.GetPokemonStatusConditions(pokemon);
                        for (int i = 0; i < conditions.Count; i++)
                        {
                            if (conditions[i].data.HasTag(PokemonSTag.NonVolatile))
                            {
                                if (apply)
                                {
                                    yield return StartCoroutine(HealPokemonSC(
                                        targetPokemon: pokemon,
                                        condition: conditions[i]
                                        ));
                                }
                                effectSuccess = true;
                            }
                        }
                    }
                    // Stat Stage Changes
                    else if (effect.effectType == ItemEffectType.XAttack
                        || effect.effectType == ItemEffectType.StatStageMax)
                    {
                        bool maximizeStats = (effect.effectType == ItemEffectType.StatStageMax)
                            ? effect.GetBool(1) : false;
                        bool minimizeStats = (effect.effectType == ItemEffectType.StatStageMax)
                            ? !effect.GetBool(1) : false;

                        int statMod = (effect.effectType == ItemEffectType.XAttack)
                            ? Mathf.FloorToInt(effect.GetFloat(0)) : 0;

                        List<PokemonStats> statsToModify = GameTextDatabase.GetStatsFromList(effect.stringParams);
                        if (statsToModify.Count > 0)
                        {
                            yield return StartCoroutine(TryToApplyStatStageMods(
                                statsToModify: statsToModify,
                                modValue: statMod,
                                targetPokemon: pokemon,
                                maximize: maximizeStats,
                                minimize: minimizeStats,
                                forceFailureMessage: forceEffectDisplay,
                                apply: apply,
                                callback: (result) =>
                                {
                                    effectSuccess = result;
                                }
                                ));
                        }
                    }
                }
            }
            callback(effectSuccess);
            yield return null;
        }

        public IEnumerator ExecuteCommandRecharge(BattleCommand command)
        {
            // essential variables
            Pokemon userPokemon = command.commandUser;
            if (userPokemon.bProps.rechargeTurns > 0)
            {
                userPokemon.bProps.rechargeTurns--;
            }

            // send text event
            SendEvent(new Battle.View.Events.MessageParameterized
            {
                messageCode = "move-recharge-default",
                pokemonUserID = userPokemon.uniqueID
            });
            yield return null;
        }

        public IEnumerator ExecuteCommandParty(BattleCommand command)
        {
            Pokemon withdrawPokemon = command.commandUser;
            Pokemon sendPokemon = command.switchInPokemon;
            Trainer trainer = command.switchingTrainer;

            withdrawPokemon.bProps.isSwitchingOut = true;

            bool faintOnSwitchOut = false;
            // withdraw if possible
            if (withdrawPokemon != null)
            {
                // Pursuit Check
                // check who's using pursuit
                List<BattleCommand> pursuitCommands = new List<BattleCommand>();
                for (int i = 0; i < allCommands.Count; i++)
                {
                    Pokemon pursuitPokemon = allCommands[i].commandUser;
                    if (!allCommands[i].inProgress
                        && !allCommands[i].completed
                        && allCommands[i].commandType == BattleCommandType.Fight)
                    {
                        MoveData pursuitData = MoveDatabase.instance.GetMoveData(allCommands[i].moveID);
                        EffectDatabase.MoveEff.MoveEffect effect_ = pursuitData.GetEffectNew(MoveEffectType.Pursuit);
                        if (effect_ != null)
                        {
                            EffectDatabase.MoveEff.Pursuit effect = effect_ as EffectDatabase.MoveEff.Pursuit;
                            if ((effect.applyToEnemies && battle.ArePokemonEnemies(withdrawPokemon, pursuitPokemon))
                                || (effect.applyToAllies && battle.ArePokemonAllies(withdrawPokemon, pursuitPokemon)))
                            {
                                pursuitCommands.Add(allCommands[i]);
                            }
                        }
                    }
                }

                // we've found a pursuiter
                if (pursuitCommands.Count > 0)
                {
                    BattleCommand chosenCommand = pursuitCommands[0];
                    chosenCommand.bypassRedirection = true;
                    chosenCommand.isPursuitMove = true;
                    chosenCommand.targetPositions = new BattlePosition[] { battle.GetPokemonPosition(withdrawPokemon) };

                    // run the command
                    yield return StartCoroutine(ExecuteCommand(chosenCommand));
                }

                if (battle.IsPokemonFainted(withdrawPokemon))
                {
                    faintOnSwitchOut = true;
                    yield return StartCoroutine(BattleFaintCheck(withdrawPokemon));
                }
                else
                {
                    battle.TrainerWithdrawPokemon(trainer, withdrawPokemon);
                    UpdateClients();
                    SendEvent(new Battle.View.Events.TrainerWithdraw
                    {
                        playerID = trainer.playerID,
                        pokemonUniqueIDs = new List<string> { withdrawPokemon.uniqueID }
                    });
                    yield return StartCoroutine(UntiePokemon(withdrawPokemon));
                    battle.TrainerSwapPokemon(trainer, withdrawPokemon, sendPokemon);
                }
            }

            // Switch in if the withdrawing pokemon didn't faint
            if (!faintOnSwitchOut)
            {
                // send out
                List<int> emptyPositions = battle.GetEmptyTrainerControlPositions(trainer);
                if (emptyPositions.Count > 0)
                {
                    battle.TrainerSendPokemon(trainer, sendPokemon, emptyPositions[0]);
                }
                UpdateClients();

                // TODO: checks for actually sending out / ???
                SendEvent(new Battle.View.Events.TrainerSendOut
                {
                    playerID = trainer.playerID,
                    pokemonUniqueIDs = new List<string> { sendPokemon.uniqueID }
                });

                sendPokemon.bProps.actedThisTurn = true;

                yield return StartCoroutine(ApplyToPokemonSwitchInEvents(sendPokemon));
            }
        }

        public IEnumerator ExecuteCommandReplace(BattleCommand command)
        {
            int battlePos = command.switchPosition;
            Pokemon switchInPokemon = command.switchInPokemon;
            Trainer trainer = command.switchingTrainer;

            // swap positions in party
            Pokemon faintedPokemon = battle.GetTrainerPokemonAtFaintPos(trainer, battlePos);
            if (faintedPokemon != null)
            {
                battle.TrainerSwapPokemon(trainer, faintedPokemon, switchInPokemon);
                faintedPokemon.faintPos = -1;
            }

            // send out
            battle.TrainerSendPokemon(trainer, switchInPokemon, battlePos);
            UpdateClients();
            SendEvent(new Battle.View.Events.TrainerSendOut
            {
                playerID = trainer.playerID,
                pokemonUniqueIDs = new List<string> { switchInPokemon.uniqueID }
            });
            switchInPokemon.bProps.actedThisTurn = true;

            yield return null;
        }

        public IEnumerator ApplyToPokemonSwitchInEvents(Pokemon pokemon)
        {
            if (battle.IsPokemonOnFieldAndAble(pokemon))
            {
                yield return StartCoroutine(ApplyToPokemonEntryHazards(pokemon));
            }
            if (battle.IsPokemonOnFieldAndAble(pokemon))
            {
                yield return StartCoroutine(PBPRunEnterAbilities(pokemon));
            }
            yield return null;
        }
        public IEnumerator ApplyToPokemonEntryHazards(Pokemon pokemon)
        {
            BattleTeam team = battle.GetTeam(pokemon);
            List<BattleTeamProperties.EntryHazard> entryHazards 
                = new List<BattleTeamProperties.EntryHazard>(team.bProps.entryHazards);

            for (int i = 0; i < entryHazards.Count; i++)
            {
                yield return StartCoroutine(ApplyToPokemonEntryHazard(pokemon, team, entryHazards[i]));
            }
        }
        public IEnumerator ApplyToPokemonEntryHazard(Pokemon pokemon, BattleTeam team, BattleTeamProperties.EntryHazard entryHazard)
        {
            if (!battle.IsPokemonFainted(pokemon))
            {
                MoveData moveData = MoveDatabase.instance.GetMoveData(entryHazard.hazardID);
                MoveEffect effect = moveData.GetEffect(MoveEffectType.EntryHazard);

                bool hazardContact = true;
                int maxLayers = Mathf.FloorToInt(effect.GetFloat(1));

                // Grounded contact check
                if (hazardContact
                    && effect.GetBool(0) 
                    && !battle.PBPIsPokemonGrounded(pokemon))
                {
                    hazardContact = false;
                }

                // Remove hazard on contact?
                if (hazardContact) 
                {
                    // Ex. Poison removes Toxic Spikes
                    List<string> pokemonTypes = battle.PBPGetTypes(pokemon);
                    for (int i = 0; i < pokemonTypes.Count; i++)
                    {
                        TypeData typeData = TypeDatabase.instance.GetTypeData(pokemonTypes[i]);
                        TypeEffect removeEffect = typeData.GetEffect(TypeEffectType.RemoveEntryHazard);
                        if (removeEffect != null)
                        {
                            List<string> removableHazards = new List<string>(removeEffect.stringParams);
                            if (removableHazards.Count > 0)
                            {
                                if (removableHazards.Contains(entryHazard.hazardID)
                                    || removableHazards.Contains("ALL"))
                                {
                                    yield return StartCoroutine(TBPRemoveEntryHazard(team, entryHazard));
                                    hazardContact = false;
                                    break;
                                }
                            }
                        }
                    }

                }

                // run hazard effects
                if (hazardContact)
                {
                    // Damage
                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        bool affectedByDamage = true;
                        if (affectedByDamage)
                        {
                            MoveEffect dmgEffect = moveData.GetEffect(MoveEffectType.EntryHazardDamage);
                            if (dmgEffect != null)
                            {
                                float hpPercentLost = 
                                    dmgEffect.GetFloat(0) + (dmgEffect.GetFloat(1) * (entryHazard.layers - 1));
                                MoveEffect SREffect = moveData.GetEffect(MoveEffectType.EntryHazardStealthRock);
                                if (SREffect != null)
                                {
                                    List<string> pokemonTypes = battle.PBPGetTypes(pokemon);
                                    List<string> SRTypes = new List<string>(SREffect.stringParams);
                                    hpPercentLost *=
                                        battle.GetTypeEffectiveness(
                                            offensiveTypes: SRTypes,
                                            targetPokemon: pokemon
                                            ).GetTotalEffectiveness();
                                }

                                int preHP = pokemon.currentHP;
                                int damage = battle.GetPokemonHPByPercent(pokemon, hpPercentLost);
                                damage = Mathf.Max(1, damage);
                                int damageDealt = battle.SubtractPokemonHP(pokemon, damage);
                                int postHP = pokemon.currentHP;
                                string textID = dmgEffect.GetString(0);

                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = textID,
                                    pokemonTargetID = pokemon.uniqueID,
                                    moveID = moveData.ID
                                });
                                yield return StartCoroutine(PBPChangePokemonHP(
                                    pokemon: pokemon,
                                    preHP: preHP,
                                    hpChange: damageDealt,
                                    postHP: postHP
                                    ));

                                yield return StartCoroutine(BattleFaintCheck(pokemon));
                            }
                        }
                    }
                
                    // Stat Change
                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        bool affectedStats = true;
                        if (affectedStats)
                        {
                            List<MoveEffect> statsEffects = moveData.GetEffects(MoveEffectType.EntryHazardStickyWeb);
                            for (int i = 0; i < statsEffects.Count; i++)
                            {
                                int layer = Mathf.FloorToInt(statsEffects[i].GetFloat(1));
                                bool applyToLayer = true;

                                if (layer > 0)
                                {
                                    if (layer > entryHazard.layers)
                                    {
                                        applyToLayer = false;
                                    }
                                    else if (layer < entryHazard.layers
                                        && statsEffects[i].GetBool(0))
                                    {
                                        applyToLayer = false;
                                    }
                                }

                                if (applyToLayer)
                                {
                                    int statMod = Mathf.FloorToInt(statsEffects[i].GetFloat(0));

                                    List<PokemonStats> statsToModify = GameTextDatabase.GetStatsFromList(statsEffects[i].stringParams);
                                    if (statsToModify.Count > 0)
                                    {
                                        yield return StartCoroutine(TryToApplyStatStageMods(
                                            statsToModify: statsToModify,
                                            modValue: statMod,
                                            targetPokemon: pokemon,
                                            forceFailureMessage: statsEffects[i].forceEffectDisplay,
                                            apply: true,
                                            callback: (result) =>
                                            {

                                            }
                                            ));
                                    }
                                }
                            }
                        }
                    }
                
                    // Status
                    if (!battle.IsPokemonFainted(pokemon))
                    {
                        bool affectedByStatus = true;
                        if (affectedByStatus)
                        {
                            List<MoveEffect> statusEffects = moveData.GetEffects(MoveEffectType.EntryHazardToxicSpikes);
                            for (int i = 0; i < statusEffects.Count; i++)
                            {
                                int layer = Mathf.FloorToInt(statusEffects[i].GetFloat(1));
                                bool applyToLayer = true;

                                if (layer > 0)
                                {
                                    if (layer > entryHazard.layers)
                                    {
                                        applyToLayer = false;
                                    }
                                    else if (layer < entryHazard.layers
                                        && statusEffects[i].GetBool(0))
                                    {
                                        applyToLayer = false;
                                    }
                                }

                                if (applyToLayer)
                                {
                                    string statusID = statusEffects[i].GetString(0);
                                    int statusTurns = Mathf.FloorToInt(statusEffects[i].GetFloat(0));

                                    // try to inflict status condition
                                    yield return StartCoroutine(TryToInflictPokemonSC(
                                        statusID: statusID,
                                        targetPokemon: pokemon,
                                        turnsLeft: statusTurns,
                                        callback: (result) =>
                                        {

                                        }
                                        ));
                                }
                            }
                        }
                    }
                
                }
            }
            yield return null;
        }
    
        // ---POKEMON PROPERTIES---
        public IEnumerator PBPChangeForm(
            Pokemon pokemon, 
            string toForm, 
            bool checkAbility = true,
            string changeText = null,
            PBS.Battle.View.Events.MessageParameterized textEvent = null)
        {
            // Send Load Event
            string prevForm = pokemon.pokemonID;
            battle.PBPChangeForm(pokemon, toForm);
            UpdateClients(updatePokemon: true, loadAsset: true);

            SendEvent(new Battle.View.Events.PokemonChangeForm
            {
                pokemonUniqueID = pokemon.uniqueID,
                preForm = prevForm,
                postForm = toForm
            });
            // TODO: More descriptive
            if (textEvent != null)
            {
                SendEvent(textEvent);
            }

            if (checkAbility)
            {
                yield return StartCoroutine(PBPRunEnterAbilities(pokemon));
            }

            yield return null;
        }
    
        public IEnumerator PBPRunBCAbilityCheck()
        {
            List<Pokemon> onFieldPokemon = battle.GetPokemonBySpeed(battle.pokemonOnField);
            for (int i = 0; i < onFieldPokemon.Count; i++)
            {
                yield return StartCoroutine(PBPRunBCAbilityCheck(onFieldPokemon[i]));
            }
        }
        public IEnumerator PBPRunBCAbilityCheck(Pokemon pokemon)
        {
            if (battle.IsPokemonOnFieldAndAble(pokemon))
            {
                List<Pokemon.Ability> abilities = battle.PBPGetAbilities(pokemon);
                for (int i = 0; i < abilities.Count; i++)
                {
                    Pokemon.Ability ability = abilities[i];

                    // Forecast
                    bool forecastAbilitySatisfied = false;
                    List<EffectDatabase.AbilityEff.AbilityEffect> forecasts_ =
                        abilities[i].data.GetEffectsNew(AbilityEffectType.Forecast);
                    for (int k = 0; k < forecasts_.Count && !forecastAbilitySatisfied; k++)
                    {
                        yield return StartCoroutine(PBPRunAbilityEffect(
                            pokemon: pokemon,
                            ability: ability,
                            effect_: forecasts_[k],
                            callback: (result) =>
                            {
                                forecastAbilitySatisfied = result;
                            }
                            ));
                    }

                    // Mimicry
                    bool mimicrySatisfied = false;
                    List<EffectDatabase.AbilityEff.AbilityEffect> mimicrys_ =
                        ability.data.GetEffectsNew(AbilityEffectType.Mimicry);
                    for (int k = 0; k < mimicrys_.Count && !mimicrySatisfied; k++)
                    {
                        yield return StartCoroutine(PBPRunAbilityEffect(
                            pokemon: pokemon,
                            ability: ability,
                            effect_: mimicrys_[k],
                            callback: (result) =>
                            {
                                mimicrySatisfied = result;
                            }
                            ));
                    }
                }
            }
        }

        public IEnumerator PBPRemoveForestsCurse(
            Pokemon pokemon,
            Pokemon.BattleProperties.ForestsCurse forestsCurse = null,
            string forestsCurseID = null,
            string textID = ""
            )
        {
            MoveData moveData = (forestsCurse == null) ?
                ((forestsCurseID == null) ? null : MoveDatabase.instance.GetMoveData(forestsCurseID))
                : MoveDatabase.instance.GetMoveData(forestsCurse.moveID);
            if (moveData != null)
            {
                List<string> removedTypes = new List<string>();
                if (forestsCurse != null)
                {
                    removedTypes.Add(forestsCurse.typeID);
                    pokemon.bProps.forestsCurses.Remove(forestsCurse);
                }
                else
                {
                    List<Pokemon.BattleProperties.ForestsCurse> existingForestsCurses =
                        new List<Pokemon.BattleProperties.ForestsCurse>(pokemon.bProps.forestsCurses);

                    for (int i = 0; i < existingForestsCurses.Count; i++)
                    {
                        if (existingForestsCurses[i].moveID == moveData.ID)
                        {
                            if (!removedTypes.Contains(existingForestsCurses[i].typeID))
                            {
                                removedTypes.Add(existingForestsCurses[i].typeID);
                            }
                            pokemon.bProps.forestsCurses.Remove(existingForestsCurses[i]);
                        }
                    }
                }

                if (textID == "")
                {
                    textID = "move-forestscurse-loss";
                }
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = textID,
                    pokemonUserID = pokemon.uniqueID,
                    moveID = moveData.ID,
                    typeIDs = new List<string>(removedTypes)
                });
                yield return null;
            }
        }

        // Ability Interactions
        public IEnumerator PBPRunEnterAbilities(List<Pokemon> pokemon, bool order = true)
        {
            List<Pokemon> abilityPokemon = new List<Pokemon>(pokemon);
            if (order)
            {
                abilityPokemon = battle.GetPokemonBySpeed(abilityPokemon);
            }

            for (int i = 0; i < abilityPokemon.Count; i++)
            {
                yield return StartCoroutine(PBPRunEnterAbilities(abilityPokemon[i]));
            }
        }
        public IEnumerator PBPRunEnterAbilities(Pokemon pokemon)
        {
            List<Pokemon.Ability> abilities = battle.PBPGetAbilities(pokemon);
            for (int i = 0; i < abilities.Count; i++)
            {
                yield return StartCoroutine(PBPRunEnterAbility(pokemon, abilities[i]));
            }
        }
        public IEnumerator PBPRunEnterAbility(Pokemon pokemon, Pokemon.Ability ability)
        {
            AbilityData abilityData = ability.data;
            if (battle.IsPokemonOnFieldAndAble(pokemon))
            {
                bool abilityShown = false;
                List<Pokemon> opposingPokemon = new List<Pokemon>();

                // Forecast Check
                yield return StartCoroutine(PBPRunBCAbilityCheck(pokemon: pokemon));
            
                // Mimicry
                EffectDatabase.AbilityEff.AbilityEffect mimicry_ = abilityData.GetEffectNew(AbilityEffectType.Mimicry);
                if (mimicry_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                        pokemon: pokemon,
                        ability: ability,
                        effect_: mimicry_,
                        callback: (result) => { }
                        ));
                }

                // Air Lock
                EffectDatabase.AbilityEff.AbilityEffect airLock_ = abilityData.GetEffectNew(AbilityEffectType.AirLock);
                if (airLock_ != null)
                {
                    EffectDatabase.AbilityEff.AirLock airLock = airLock_ as EffectDatabase.AbilityEff.AirLock;
                    if (!abilityShown)
                    {
                        abilityShown = true;
                        PBPShowAbility(pokemon, abilityData);
                    }
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = airLock.displayText,
                        pokemonUserID = pokemon.uniqueID
                    });
                }

                // Anticipation
                EffectDatabase.AbilityEff.AbilityEffect anticipation_ = abilityData.GetEffectNew(AbilityEffectType.Anticipation);
                if (anticipation_ != null)
                {
                    EffectDatabase.AbilityEff.Anticipation anticipation = anticipation_ as EffectDatabase.AbilityEff.Anticipation;
                    bool showAnticipation = false;
                    opposingPokemon = battle.GetOpposingPokemon(pokemon);

                    for (int i = 0; i < opposingPokemon.Count && !showAnticipation; i++)
                    {
                        List<Pokemon.Moveslot> moveslots = battle.GetPokemonBattleMoveslots(opposingPokemon[i]);
                        bool foundMove = false;
                        for (int k = 0; k < moveslots.Count && !foundMove; k++)
                        {
                            MoveData moveData = MoveDatabase.instance.GetMoveData(moveslots[k].moveID);
                        
                            // Hidden Power
                            if (moveData.GetEffectNew(MoveEffectType.HiddenPower) != null)
                            {
                                moveData = battle.GetPokemonMoveData(
                                    userPokemon: opposingPokemon[i],
                                    moveID: moveData.ID,
                                    targetPokemon: pokemon
                                    );
                            }

                            if (moveData.category == MoveCategory.Physical || moveData.category == MoveCategory.Special)
                            {
                                // Super-effective moves
                                BattleTypeEffectiveness effectiveness = battle.GetMoveEffectiveness(
                                    userPokemon: opposingPokemon[i],
                                    moveData: moveData,
                                    targetPokemon: pokemon
                                    );
                                if (effectiveness.GetTotalEffectiveness() > 1) foundMove = true;

                                // OHKO moves
                                if (anticipation.notifyOHKO)
                                {
                                    if (moveData.GetEffectNew(MoveEffectType.Guillotine) != null) foundMove = true;
                                }
                            }
                        }
                        if (foundMove) showAnticipation = true;
                    }

                    if (showAnticipation)
                    {
                        if (!abilityShown)
                        {
                            abilityShown = true;
                            PBPShowAbility(pokemon, abilityData);
                        }
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = anticipation.displayText,
                            pokemonUserID = pokemon.uniqueID
                        });
                    }
                }

                // Forewarn
                EffectDatabase.AbilityEff.AbilityEffect forewarn_ = abilityData.GetEffectNew(AbilityEffectType.Forewarn);
                if (forewarn_ != null)
                {
                    EffectDatabase.AbilityEff.Forewarn forewarn = forewarn_ as EffectDatabase.AbilityEff.Forewarn;
                    bool showForewarn = false;

                    opposingPokemon = battle.GetOpposingPokemon(pokemon);
                    List<Pokemon> forewarnPokemon = new List<Pokemon>();
                    MoveData strongestMove = null;

                    for (int i = 0; i < opposingPokemon.Count; i++)
                    {
                        List<Pokemon.Moveslot> moveslots = battle.GetPokemonBattleMoveslots(opposingPokemon[i]);
                        bool foundMove = false;
                        for (int k = 0; k < moveslots.Count && !foundMove; k++)
                        {
                            MoveData moveData = MoveDatabase.instance.GetMoveData(moveslots[k].moveID);
                            if (battle.IsMoveDamaging(moveData))
                            {
                                if (strongestMove == null)
                                {
                                    forewarnPokemon.Add(opposingPokemon[i]);
                                    strongestMove = moveData;
                                }
                                else
                                {
                                    if (moveData.basePower >= strongestMove.basePower)
                                    {
                                        if (strongestMove.ID == moveData.ID)
                                        {
                                            forewarnPokemon.Add(opposingPokemon[i]);
                                        }
                                        else
                                        {
                                            if (moveData.basePower > strongestMove.basePower)
                                            {
                                                strongestMove = moveData;
                                                forewarnPokemon.Clear();
                                                forewarnPokemon.Add(opposingPokemon[i]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (foundMove) showForewarn = true;
                    }

                    if (showForewarn)
                    {
                        if (!abilityShown)
                        {
                            abilityShown = true;
                            PBPShowAbility(pokemon, abilityData);
                        }
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = forewarn.displayText,
                            pokemonUserID = pokemon.uniqueID, 
                            pokemonTargetID = forewarnPokemon[Random.Range(0, forewarnPokemon.Count)].uniqueID,
                            moveID = strongestMove.ID
                        });
                    }
                }

                // Frisk
                EffectDatabase.AbilityEff.AbilityEffect frisk_ = abilityData.GetEffectNew(AbilityEffectType.Frisk);
                if (frisk_ != null)
                {
                    EffectDatabase.AbilityEff.Frisk frisk = frisk_ as EffectDatabase.AbilityEff.Frisk;
                    opposingPokemon = battle.GetOpposingPokemon(pokemon);
                    List<Pokemon> itemPokemon = new List<Pokemon>();

                    for (int i = 0; i < opposingPokemon.Count; i++)
                    {
                        Item item = battle.PBPGetHeldItem(opposingPokemon[i]);
                        if (item != null
                            && !opposingPokemon[i].bProps.friskIdentified)
                        {
                            itemPokemon.Add(opposingPokemon[i]);
                        }
                    }

                    if (itemPokemon.Count > 0)
                    { 
                        if (!abilityShown)
                        {
                            abilityShown = true;
                            PBPShowAbility(pokemon, abilityData);
                        }
                        for (int i = 0; i < itemPokemon.Count; i++)
                        {
                            Pokemon showPokemon = itemPokemon[i];
                            showPokemon.bProps.friskIdentified = true;
                            Item item = battle.PBPGetHeldItem(showPokemon);
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = frisk.displayText,
                                pokemonUserID = pokemon.uniqueID,
                                pokemonTargetID = showPokemon.uniqueID,
                                itemID = item.itemID
                            });
                        }
                    }
                }

                // Aura Break
                EffectDatabase.AbilityEff.AbilityEffect auraBreak_ = abilityData.GetEffectNew(AbilityEffectType.AuraBreak);
                if (auraBreak_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                       pokemon: pokemon,
                       ability: ability,
                       effect_: auraBreak_,
                       callback: (result) => { }
                       ));
                }

                // Dark Aura
                List<EffectDatabase.AbilityEff.AbilityEffect> darkAuras_ = abilityData.GetEffectsNew(AbilityEffectType.DarkAura);
                for (int i = 0; i < darkAuras_.Count; i++)
                {
                    EffectDatabase.AbilityEff.DarkAura darkAura = darkAuras_[i] as EffectDatabase.AbilityEff.DarkAura;
                    List<string> affectedTypes = new List<string>();
                    for (int k = 0; k < darkAura.filters.Count; k++)
                    {
                        if (darkAura.filters[k] is EffectDatabase.Filter.TypeList)
                        {
                            EffectDatabase.Filter.TypeList typeList = darkAura.filters[k] as EffectDatabase.Filter.TypeList;
                            if (typeList.targetType == EffectDatabase.Filter.TypeList.TargetType.Move)
                            {
                                affectedTypes.AddRange(typeList.types);
                                break;
                            }
                        }
                    }

                    if (!abilityShown)
                    {
                        abilityShown = true;
                        PBPShowAbility(pokemon, abilityData);
                    }
                    // TODO: More descriptive
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = darkAura.displayText,
                        pokemonUserID = pokemon.uniqueID,
                        typeIDs = new List<string>(affectedTypes)
                    });

                }

                // Download
                EffectDatabase.AbilityEff.AbilityEffect download_ = abilityData.GetEffectNew(AbilityEffectType.Download);
                opposingPokemon = battle.GetOpposingPokemon(pokemon);
                if (download_ != null && opposingPokemon.Count > 0)
                {
                    if (!abilityShown)
                    {
                        abilityShown = true;
                        PBPShowAbility(pokemon, abilityData);
                    }

                    EffectDatabase.AbilityEff.Download download = download_ as EffectDatabase.AbilityEff.Download;
                    for (int i = 0; i < download.downloadComparisons.Count; i++)
                    {
                        EffectDatabase.General.StatStageMod statStageMod = null;
                        float totalStat1 = 0;
                        float totalStat2 = 0;
                        for (int k = 0; k < opposingPokemon.Count; k++)
                        {

                            totalStat1 += battle.GetPokemonStat(
                                pokemon: opposingPokemon[k],
                                statType: download.downloadComparisons[i].stats1,
                                applyModifiers: false
                                );

                            totalStat2 += battle.GetPokemonStat(
                                pokemon: opposingPokemon[k],
                                statType: download.downloadComparisons[i].stats2,
                                applyModifiers: false
                                );
                        }

                        if (totalStat2 < totalStat1)
                        {
                            statStageMod = download.downloadComparisons[i].statStageMod2;
                        }
                        else
                        {
                            statStageMod = download.downloadComparisons[i].statStageMod1;
                        }

                        yield return StartCoroutine(ApplyStatStageMod(
                            statStageMod: statStageMod,
                            targetPokemon: pokemon,
                            userPokemon: pokemon,
                            forceFailureMessage: true,
                            callback: (result) => { }
                            ));
                    }
                }

                // Drought
                EffectDatabase.AbilityEff.AbilityEffect drought_ = abilityData.GetEffectNew(AbilityEffectType.Drought);
                if (drought_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                        pokemon: pokemon,
                        ability: ability,
                        effect_: drought_,
                        callback: (result) => { }
                        ));
                }

                // Intimidate
                List<EffectDatabase.AbilityEff.AbilityEffect> intimidate_ = abilityData.GetEffectsNew(AbilityEffectType.Intimidate);
                opposingPokemon = battle.GetOpposingPokemon(pokemon);
                if (opposingPokemon.Count > 0 && intimidate_.Count > 0)
                {
                    if (!abilityShown)
                    {
                        abilityShown = true;
                        PBPShowAbility(pokemon, abilityData);
                    }

                    bool affectedByIntimidate = true;
                    for (int i = 0; i < opposingPokemon.Count; i++)
                    {
                        List<Pokemon.Ability> oppAbilities = battle.PBPGetAbilities(opposingPokemon[i]);

                        // Intimidate Block
                        for (int k = 0; k < oppAbilities.Count && affectedByIntimidate; k++)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect intimidateBlock_ =
                                oppAbilities[k].data.GetEffectNew(AbilityEffectType.IntimidateBlock);
                            if (intimidateBlock_ != null)
                            {
                                EffectDatabase.AbilityEff.IntimidateBlock intimidateBlock =
                                    intimidateBlock_ as EffectDatabase.AbilityEff.IntimidateBlock;
                                if (intimidateBlock.abilitiesBlocked.Contains(abilityData.ID))
                                {
                                    affectedByIntimidate = false;

                                    PBPShowAbility(opposingPokemon[i], oppAbilities[k].data);
                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = intimidateBlock.displayText,
                                        pokemonTargetID = opposingPokemon[i].uniqueID,
                                        abilityID = ability.abilityID
                                    });
                                }
                            }
                        }

                        if (affectedByIntimidate)
                        {
                            for (int k = 0; k < intimidate_.Count; k++)
                            {
                                EffectDatabase.AbilityEff.Intimidate intimidate = 
                                    intimidate_[k] as EffectDatabase.AbilityEff.Intimidate;
                                yield return StartCoroutine(ApplyStatStageMod(
                                    statStageMod: intimidate.statStageMod,
                                    targetPokemon: opposingPokemon[k],
                                    userPokemon: pokemon,
                                    forceFailureMessage: true,
                                    callback: (result) => { }
                                    ));
                            }

                            // Rattled
                            for  (int k = 0; k < oppAbilities.Count; k++)
                            {
                                EffectDatabase.AbilityEff.AbilityEffect intimidateTrigger_ =
                                    oppAbilities[k].data.GetEffectNew(AbilityEffectType.IntimidateTrigger);
                                if (intimidateTrigger_ != null)
                                {
                                    EffectDatabase.AbilityEff.IntimidateTrigger intimidateTrigger =
                                        intimidateTrigger_ as EffectDatabase.AbilityEff.IntimidateTrigger;
                                    bool isTriggered = false;

                                    if (intimidateTrigger.abilityTriggers.Contains(abilityData.ID))
                                    {
                                        isTriggered = true;
                                    }

                                    if (isTriggered)
                                    {
                                        yield return StartCoroutine(ApplyStatStageMod(
                                            statStageMod: intimidateTrigger.statStageMod,
                                            targetPokemon: opposingPokemon[k],
                                            userPokemon: opposingPokemon[k],
                                            forceFailureMessage: true,
                                            callback: (result) => { }
                                            ));
                                    }
                                }
                            }
                        }
                    }
                }
            
                // Intrepid Sword / Dauntless Shield
                List<EffectDatabase.AbilityEff.AbilityEffect> intrepidSword_ = abilityData.GetEffectsNew(AbilityEffectType.IntrepidSword);
                for (int i = 0; i < intrepidSword_.Count; i++)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                        pokemon: pokemon,
                        ability: ability,
                        effect_: intrepidSword_[i],
                        callback: (result) => { }
                        ));
                }

                // Limber
                List<EffectDatabase.AbilityEff.AbilityEffect> limber_ = ability.data.GetEffectsNew(AbilityEffectType.Limber);
                if (limber_.Count > 0)
                {
                    for (int i = 0; i < limber_.Count; i++)
                    {
                        EffectDatabase.AbilityEff.Limber limber = limber_[i] as EffectDatabase.AbilityEff.Limber;
                        List<Pokemon> pokemonToHeal = new List<Pokemon>();
                        if (limber.healSelf)
                        {
                            pokemonToHeal.Add(pokemon);
                        }
                        if (limber.pastelVeil)
                        {
                            pokemonToHeal.AddRange(battle.GetAllyPokemon(pokemon));
                        }

                        for (int k = 0; k < pokemonToHeal.Count; k++)
                        {
                            Pokemon curPokemon = pokemonToHeal[k];
                            BattleTeam curTeam = battle.GetTeam(curPokemon);
                            if (battle.DoEffectFiltersPass(
                                filters: limber.filters,
                                userPokemon: pokemon,
                                targetPokemon: curPokemon,
                                targetTeam: curTeam
                                )
                                && battle.IsPokemonOnFieldAndAble(curPokemon))
                            {
                                List<StatusCondition> sConds = battle.PBPGetSCs(curPokemon);

                                // check each status condition
                                for (int j = 0; j < sConds.Count; j++)
                                {
                                    StatusCondition curCond = sConds[j];
                                    bool isHealed = false;

                                    // specific conditions
                                    if (!isHealed && limber.conditions.Count > 0)
                                    {
                                        for (int l = 0; l < limber.conditions.Count && !isHealed; l++)
                                        {
                                            if (curCond.data.ID == limber.conditions[l]
                                                || curCond.data.IsABaseID(limber.conditions[l]))
                                            {
                                                isHealed = true;
                                            }
                                        }
                                    }

                                    // status types
                                    if (!isHealed && limber.statusTypes.Count > 0)
                                    {
                                        List<PokemonSEType> statusTypes = new List<PokemonSEType>(limber.statusTypes);
                                        for (int l = 0; l < statusTypes.Count && !isHealed; l++)
                                        {
                                            if (curCond.data.GetEffectNew(statusTypes[l]) != null)
                                            {
                                                isHealed = true;
                                            }
                                        }
                                    }

                                    if (isHealed)
                                    {
                                        if (!abilityShown)
                                        {
                                            abilityShown = true;
                                            PBPShowAbility(pokemon, abilityData);
                                        }
                                        yield return StartCoroutine(HealPokemonSC(
                                            targetPokemon: curPokemon,
                                            condition: curCond,
                                            healerPokemon: pokemon
                                            ));
                                    }
                                }
                            }
                        }
                    }
                }

                // Mold Breaker
                EffectDatabase.AbilityEff.AbilityEffect moldBreaker_ = ability.data.GetEffectNew(AbilityEffectType.MoldBreaker);
                if (moldBreaker_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                       pokemon: pokemon,
                       ability: ability,
                       effect_: moldBreaker_,
                       callback: (result) => { }
                       ));
                }

                // Pressure
                EffectDatabase.AbilityEff.AbilityEffect pressure_ = ability.data.GetEffectNew(AbilityEffectType.Pressure);
                if (pressure_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                       pokemon: pokemon,
                       ability: ability,
                       effect_: pressure_,
                       callback: (result) => { }
                       ));
                }

                // Neutralizing Gas
                EffectDatabase.AbilityEff.AbilityEffect neutralizingGas_ = ability.data.GetEffectNew(AbilityEffectType.NeutralizingGas);
                if (moldBreaker_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                       pokemon: pokemon,
                       ability: ability,
                       effect_: neutralizingGas_,
                       callback: (result) => { }
                       ));
                }

                // Screen Cleaner
                EffectDatabase.AbilityEff.AbilityEffect screenCleaner_ = ability.data.GetEffectNew(AbilityEffectType.ScreenCleaner);
                if (screenCleaner_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                        pokemon: pokemon,
                        ability: ability,
                        effect_: screenCleaner_,
                        callback: (result) => { }
                        ));
                }

                // Slow Start
                EffectDatabase.AbilityEff.AbilityEffect slowStart_ = ability.data.GetEffectNew(AbilityEffectType.SlowStart);
                if (slowStart_ != null)
                {
                    EffectDatabase.AbilityEff.SlowStart slowStart = slowStart_ as EffectDatabase.AbilityEff.SlowStart;
                    if (ability.turnsActive <= slowStart.turnsActive)
                    {
                        yield return StartCoroutine(PBPRunAbilityEffect(
                            pokemon: pokemon,
                            ability: ability,
                            effect_: slowStart_,
                            callback: (result) => { }
                            ));
                    }
                }

                // Trace
                EffectDatabase.AbilityEff.AbilityEffect trace_ = ability.data.GetEffectNew(AbilityEffectType.Trace);
                if (trace_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                        pokemon: pokemon,
                        ability: ability,
                        effect_: trace_,
                        callback: (result) => { }
                        ));
                }

                // Zen Mode
                List<EffectDatabase.AbilityEff.AbilityEffect> zenMode_ = ability.data.GetEffectsNew(AbilityEffectType.ZenMode);
                if (zenMode_.Count > 0)
                {
                    bool changedForm = false;
                    for (int i = 0; i < zenMode_.Count && !changedForm; i++)
                    {
                        yield return StartCoroutine(PBPRunAbilityEffect(
                            pokemon: pokemon,
                            ability: ability,
                            effect_: zenMode_[i],
                            callback: (result) =>
                            {
                                changedForm = result;
                            }
                            ));
                    }
                }

            }

            yield return null;
        }
        public IEnumerator PBPRunEndTurnAbilities(Pokemon pokemon)
        {
            List<Pokemon.Ability> abilities = battle.PBPGetAbilities(pokemon);
            for (int i = 0; i < abilities.Count; i++)
            {
                yield return StartCoroutine(PBPRunEndTurnAbility(pokemon, abilities[i]));
            }
        }
        public IEnumerator PBPRunEndTurnAbility(Pokemon pokemon, Pokemon.Ability ability)
        {
            AbilityData abilityData = ability.data;
            if (battle.IsPokemonOnFieldAndAble(pokemon))
            {
                bool abilityShown = false;

                // Bad Dreams
                EffectDatabase.AbilityEff.AbilityEffect badDreams_ = abilityData.GetEffectNew(AbilityEffectType.BadDreams);
                if (badDreams_ != null)
                {
                    EffectDatabase.AbilityEff.BadDreams badDreams = badDreams_ as EffectDatabase.AbilityEff.BadDreams;

                    List<Pokemon> opposingPokemon = battle.GetOpposingPokemon(pokemon);
                    for (int k = 0; k < opposingPokemon.Count; k++)
                    {
                        Pokemon targetPokemon = opposingPokemon[k];

                        if (battle.IsPokemonOnFieldAndAble(targetPokemon))
                        {
                            bool isAffected = false;

                            for (int j = 0; j < badDreams.affectedStatuses.Count && !isAffected; j++)
                            {
                                StatusPKData statusData =
                                    StatusPKDatabase.instance.GetStatusData(badDreams.affectedStatuses[j]);
                                if (statusData.ID == targetPokemon.nonVolatileStatus.statusID)
                                {
                                    isAffected = true;
                                }
                            }

                            if (isAffected)
                            {
                                if (!abilityShown)
                                {
                                    abilityShown = true;
                                    PBPShowAbility(pokemon, abilityData);
                                }

                                // Lose HP
                                if (badDreams.hpLossPercent > 0)
                                {
                                    int preHP = targetPokemon.currentHP;
                                    int damage = battle.GetPokemonHPByPercent(targetPokemon, badDreams.hpLossPercent);
                                    damage = Mathf.Max(1, damage);
                                    int damageDealt = battle.SubtractPokemonHP(targetPokemon, damage);
                                    int postHP = targetPokemon.currentHP;

                                    SendEvent(new Battle.View.Events.MessageParameterized
                                    {
                                        messageCode = badDreams.displayText,
                                        pokemonUserID = pokemon.uniqueID,
                                        pokemonTargetID = targetPokemon.uniqueID,
                                        abilityID = ability.abilityID
                                    });
                                    yield return StartCoroutine(PBPChangePokemonHP(
                                        pokemon: targetPokemon,
                                        preHP: preHP,
                                        hpChange: damageDealt,
                                        postHP: postHP,
                                        checkFaint: true
                                        ));
                                }
                            }
                        }
                    }
                }

                // Ball Fetch
                EffectDatabase.AbilityEff.AbilityEffect ballFetch_ = abilityData.GetEffectNew(AbilityEffectType.BallFetch);
                if (ballFetch_ != null)
                {
                    Trainer trainer = battle.GetPokemonOwner(pokemon);

                    EffectDatabase.AbilityEff.BallFetch ballFetch = ballFetch_ as EffectDatabase.AbilityEff.BallFetch;
                    if (battle.PBPGetHeldItem(pokemon) != null
                        && !trainer.bProps.usedBallFetch
                        && trainer.bProps.failedPokeball != null)
                    {
                        ItemData itemData = ItemDatabase.instance.GetItemData(trainer.bProps.failedPokeball);
                        pokemon.SetItem(new Item(itemID: itemData.ID));
                        trainer.bProps.usedBallFetch = true;
                        trainer.bProps.failedPokeball = null;

                        if (!abilityShown)
                        {
                            abilityShown = true;
                            PBPShowAbility(pokemon, abilityData);
                        }
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = ballFetch.displayText,
                            pokemonUserID = pokemon.uniqueID,
                            itemID = itemData.ID,
                            abilityID = ability.abilityID
                        });
                    }
                }

                // Dry Skin
                EffectDatabase.AbilityEff.AbilityEffect drySkin_ = abilityData.GetEffectNew(AbilityEffectType.DrySkin);
                if (drySkin_ != null)
                {
                    EffectDatabase.AbilityEff.DrySkin drySkin = drySkin_ as EffectDatabase.AbilityEff.DrySkin;
                    if (battle.DoEffectFiltersPass(
                        filters: drySkin.filters,
                        targetPokemon: pokemon
                        ))
                    {
                        for (int i = 0; i < drySkin.conditions.Count; i++)
                        {
                            EffectDatabase.AbilityEff.DrySkin.DrySkinCondition drySkinCondition =
                                drySkin.conditions[i];
                            bool activated = false;

                            if (battle.IsPokemonOnFieldAndAble(pokemon))
                            {
                                for (int k = 0; k < drySkinCondition.conditions.Count && !activated; k++)
                                {
                                    if (battle.BBPGetSC(
                                        statusID: drySkinCondition.conditions[k],
                                        descendant: true
                                        ) != null)
                                    {
                                        activated = true;
                                    }
                                }

                                if (activated)
                                {
                                    if (!abilityShown)
                                    {
                                        abilityShown = true;
                                        PBPShowAbility(pokemon, abilityData);
                                    }

                                    // Lose HP
                                    if (drySkinCondition.hpLosePercent > 0)
                                    {
                                        yield return StartCoroutine(PBPDamagePokemon(
                                            pokemon: pokemon,
                                            HPToLose: battle.GetPokemonHPByPercent(pokemon, drySkinCondition.hpLosePercent)
                                            ));
                                    }
                                    // Gain HP
                                    else if (drySkinCondition.hpGainPercent > 0)
                                    {
                                        yield return StartCoroutine(PBPHealPokemon(
                                            pokemon: pokemon,
                                            HPToAdd: battle.GetPokemonHPByPercent(pokemon, drySkinCondition.hpGainPercent)
                                            ));
                                    }
                                }
                            }
                        
                        }
                    }
                }

                // Harvest
                List<EffectDatabase.AbilityEff.AbilityEffect> harvest_ = abilityData.GetEffectsNew(AbilityEffectType.Harvest);
                if (harvest_.Count > 0
                    && pokemon.bProps.consumedItem != null)
                {
                    bool isItemHarvested = false;
                    ItemData consumedItemData = ItemDatabase.instance.GetItemData(pokemon.bProps.consumedItem);

                    for (int i = 0; i < harvest_.Count && !isItemHarvested; i++)
                    {
                        EffectDatabase.AbilityEff.Harvest harvest = harvest_[i] as EffectDatabase.AbilityEff.Harvest;
                        if (battle.DoEffectFiltersPass(
                            filters: harvest.filters,
                            targetPokemon: pokemon
                            ))
                        {
                            bool applyHarvest = false;

                            // Berry
                            if (harvest.pockets.Count > 0)
                            {
                                if (harvest.pockets.Contains(consumedItemData.pocket))
                                {
                                    applyHarvest = true;
                                }
                            }

                            // Apply Harvest
                            if (applyHarvest && Random.value <= harvest.chance)
                            {
                                isItemHarvested = true;
                                Item harvestItem = new Item(pokemon.bProps.consumedItem);

                                if (!abilityShown)
                                {
                                    abilityShown = true;
                                    PBPShowAbility(pokemon, abilityData);
                                }
                                pokemon.SetItem(harvestItem);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = harvest.displayText,
                                    pokemonUserID = pokemon.uniqueID,
                                    itemID = harvestItem.itemID,
                                    abilityID = ability.abilityID
                                });
                            }
                        }
                    }
                }

                // Pickup
                EffectDatabase.AbilityEff.AbilityEffect pickup_ = abilityData.GetEffectNew(AbilityEffectType.Pickup);
                if (pickup_ != null)
                {
                    EffectDatabase.AbilityEff.Pickup pickup = pickup_ as EffectDatabase.AbilityEff.Pickup;
                    if (battle.DoEffectFiltersPass(
                        filters: pickup.filters,
                        userPokemon: pokemon
                        ))
                    {
                        Item pickupItem = null;

                        if (pickupItem != null)
                        {
                            if (!abilityShown)
                            {
                                abilityShown = true;
                                PBPShowAbility(pokemon, ability);
                            }
                            pokemon.SetItem(pickupItem.Clone());
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = pickup.displayText,
                                pokemonUserID = pokemon.uniqueID,
                                itemID = pickupItem.itemID,
                                abilityID = ability.abilityID
                            });
                        }
                    }
                }

                // Poison Heal
                List<EffectDatabase.AbilityEff.AbilityEffect> poisonHeal_ = ability.data.GetEffectsNew(AbilityEffectType.PoisonHeal);
                if (poisonHeal_.Count > 0)
                {
                    for (int i = 0; i < poisonHeal_.Count; i++)
                    {
                        yield return StartCoroutine(PBPRunAbilityEffect(
                            pokemon: pokemon,
                            ability: ability,
                            effect_: poisonHeal_[i],
                            callback: (result) => { }
                            ));
                    }
                }

                // Hydration
                List<EffectDatabase.AbilityEff.AbilityEffect> hydration_ = abilityData.GetEffectsNew(AbilityEffectType.Hydration);
                if (hydration_.Count > 0)
                {
                    for (int i = 0; i < hydration_.Count; i++)
                    {
                        EffectDatabase.AbilityEff.Hydration hydration = hydration_[i] as EffectDatabase.AbilityEff.Hydration;
                        List<Pokemon> pokemonToHeal = new List<Pokemon>();
                        if (hydration.healSelf)
                        {
                            pokemonToHeal.Add(pokemon);
                        }
                        if (hydration.healer)
                        {
                            pokemonToHeal.AddRange(battle.GetAllyPokemon(pokemon));
                        }

                        // one-time heal
                        if (Random.value <= hydration.chance || !hydration.oneTimeAll)
                        {
                            for (int k = 0; k < pokemonToHeal.Count; k++)
                            {
                                Pokemon curPokemon = pokemonToHeal[k];
                                BattleTeam curTeam = battle.GetTeam(curPokemon);
                                if (battle.DoEffectFiltersPass(
                                    filters: hydration.filters,
                                    userPokemon: pokemon,
                                    targetPokemon: curPokemon,
                                    targetTeam: curTeam
                                    )
                                    && battle.IsPokemonOnFieldAndAble(curPokemon))
                                {
                                    List<StatusCondition> sConds = battle.PBPGetSCs(curPokemon);

                                    // check each status condition
                                    for (int j = 0; j < sConds.Count; j++)
                                    {
                                        StatusCondition curCond = sConds[j];
                                        bool isHealed = false;

                                        // specific conditions
                                        if (!isHealed && hydration.conditions.Count > 0)
                                        {
                                            for (int l = 0; l < hydration.conditions.Count && !isHealed; l++)
                                            {
                                                if (curCond.data.ID == hydration.conditions[l]
                                                    || curCond.data.IsABaseID(hydration.conditions[l]))
                                                {
                                                    isHealed = true;
                                                }
                                            }
                                        }

                                        // status types
                                        if (!isHealed && hydration.statusTypes.Count > 0)
                                        {
                                            List<PokemonSEType> statusTypes = new List<PokemonSEType>(hydration.statusTypes);
                                            for (int l = 0; l < statusTypes.Count && !isHealed; l++)
                                            {
                                                if (curCond.data.GetEffectNew(statusTypes[l]) != null)
                                                {
                                                    isHealed = true;
                                                }
                                            }
                                        }

                                        if (isHealed)
                                        {
                                            // individual heal
                                            if (Random.value <= hydration.chance || hydration.oneTimeAll)
                                            {
                                                if (!abilityShown)
                                                {
                                                    abilityShown = true;
                                                    PBPShowAbility(pokemon, abilityData);
                                                }
                                                yield return StartCoroutine(HealPokemonSC(
                                                    targetPokemon: curPokemon,
                                                    condition: curCond,
                                                    healerPokemon: pokemon
                                                    ));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Hunger Switch
                List<EffectDatabase.AbilityEff.AbilityEffect> hungerSwitch_ =
                    abilityData.GetEffectsNew(AbilityEffectType.HungerSwitch);
                if (hungerSwitch_.Count > 0)
                {
                    bool changedForm = false;
                    for (int k = 0; k < hungerSwitch_.Count && !changedForm; k++)
                    {
                        EffectDatabase.AbilityEff.HungerSwitch hungerSwitch =
                            hungerSwitch_[k] as EffectDatabase.AbilityEff.HungerSwitch;
                        if (hungerSwitch.mode == EffectDatabase.AbilityEff.HungerSwitch.ChangeMode.Alternating)
                        {
                            bool willChangeForm = false;
                            string toForm = null;
                            if (battle.PBPIsPokemonOfForm(pokemon, hungerSwitch.pokemonID1))
                            {
                                toForm = hungerSwitch.pokemonID2;
                                willChangeForm = true;
                            }
                            else if (battle.PBPIsPokemonOfForm(pokemon, hungerSwitch.pokemonID2))
                            {
                                toForm = hungerSwitch.pokemonID1;
                                willChangeForm = true;
                            }

                            if (willChangeForm)
                            {
                                if (!abilityShown)
                                {
                                    abilityShown = true;
                                    PBPShowAbility(pokemon, abilityData);
                                }

                                yield return StartCoroutine(PBPChangeForm(
                                    pokemon: pokemon,
                                    toForm: toForm,
                                    changeText: hungerSwitch.displayText));

                                changedForm = true;
                            }
                        }
                        else if (hungerSwitch.mode == EffectDatabase.AbilityEff.HungerSwitch.ChangeMode.Consecutive)
                        {
                            if (battle.PBPIsPokemonOfForm(pokemon, hungerSwitch.pokemonID1))
                            {
                                if (!abilityShown)
                                {
                                    abilityShown = true;
                                    PBPShowAbility(pokemon, abilityData);
                                }

                                yield return StartCoroutine(PBPChangeForm(
                                    pokemon: pokemon,
                                    toForm: hungerSwitch.pokemonID2,
                                    changeText: hungerSwitch.displayText));

                                changedForm = true;
                            }
                        }
                    }
                }

                // Zen Mode
                List<EffectDatabase.AbilityEff.AbilityEffect> zenMode_ = ability.data.GetEffectsNew(AbilityEffectType.ZenMode);
                if (zenMode_.Count > 0)
                {
                    bool changedForm = false;
                    for (int i = 0; i < zenMode_.Count && !changedForm; i++)
                    {
                        yield return StartCoroutine(PBPRunAbilityEffect(
                            pokemon: pokemon,
                            ability: ability,
                            effect_: zenMode_[i],
                            callback: (result) =>
                            {
                                changedForm = result;
                            }
                            ));
                    }
                }

                // Speed Boost
                EffectDatabase.AbilityEff.AbilityEffect speedBoost_ =
                    ability.data.GetEffectNew(AbilityEffectType.SpeedBoost);
                if (speedBoost_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                        pokemon: pokemon,
                        ability: ability,
                        effect_: speedBoost_,
                        callback: (result) => { }
                        ));
                }

                // Moody
                EffectDatabase.AbilityEff.AbilityEffect moody_ =
                    ability.data.GetEffectNew(AbilityEffectType.Moody);
                if (moody_ != null)
                {
                    yield return StartCoroutine(PBPRunAbilityEffect(
                        pokemon: pokemon,
                        ability: ability,
                        effect_: moody_,
                        callback: (result) => { }
                        ));
                }

            }

            yield return null;
        }
        public IEnumerator PBPRunAbilityEffect(
            Pokemon pokemon,
            Pokemon.Ability ability,
            EffectDatabase.AbilityEff.AbilityEffect effect_,
            System.Action<bool> callback,
            bool apply = true
            )
        {
            bool success = false;

            // Aura Break
            if (effect_ is EffectDatabase.AbilityEff.AuraBreak)
            {
                EffectDatabase.AbilityEff.AuraBreak auraBreak = effect_ as EffectDatabase.AbilityEff.AuraBreak;
                success = true;
                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = auraBreak.displayText,
                        pokemonUserID = pokemon.uniqueID,
                        abilityID = ability.abilityID
                    });
                }
            }

            // Drought / Drizzle / Delta Stream / etc.
            else if (effect_ is EffectDatabase.AbilityEff.Drought)
            {
                EffectDatabase.AbilityEff.Drought drought = effect_ as EffectDatabase.AbilityEff.Drought;
                if (battle.BBPGetSC(drought.inflictStatus.statusID) == null)
                {
                    if (apply)
                    {
                        PBPShowAbility(pokemon, ability);
                    }
                    yield return StartCoroutine(ApplySC(
                        inflictStatus: drought.inflictStatus,
                        userPokemon: pokemon,
                        forceFailMessage: true,
                        apply: apply,
                        callback: (result) => { success = result; }
                        ));
                }
            }

            // Forecast
            else if (effect_ is EffectDatabase.AbilityEff.Forecast)
            {
                // loop through forecast transformations
                // loop through forecast effects
                bool foundTransformation = false;
                EffectDatabase.AbilityEff.Forecast forecast = effect_ as EffectDatabase.AbilityEff.Forecast;
                for (int i = 0; i < forecast.transformations.Count && !foundTransformation; i++)
                {
                    // Check Battle Conditions
                    EffectDatabase.AbilityEff.Forecast.ForecastTransformation curTransformation =
                        forecast.transformations[i];

                    // stop searching if we've satisfied Forecast
                    if (curTransformation.transformation.toForm == pokemon.pokemonID)
                    {
                        foundTransformation = true;
                    }
                    else
                    {
                        // loop through conditions
                        for (int k = 0; k < curTransformation.conditions.Count && !foundTransformation; k++)
                        {
                            if (curTransformation.transformation.IsPokemonAPreForm(pokemon))
                            {
                                success = true;
                                foundTransformation = true;
                                if (apply)
                                {
                                    PBPShowAbility(pokemon, ability);
                                    yield return StartCoroutine(PBPChangeForm(
                                        pokemon: pokemon,
                                        toForm: curTransformation.transformation.toForm,
                                        checkAbility: false
                                        ));
                                }
                            }
                        }
                    }
                }

                // Go to default-form if possible
                if (!foundTransformation
                    && forecast.defaultTransformation != null)
                {
                    // Revert to default form
                    if (forecast.defaultTransformation.IsPokemonAPreForm(pokemon))
                    {
                        success = true;
                        if (apply)
                        {
                            PBPShowAbility(pokemon, ability);
                            yield return StartCoroutine(PBPChangeForm(
                                pokemon: pokemon,
                                toForm: forecast.defaultTransformation.toForm,
                                checkAbility: false
                                ));
                        }
                    }
                }
            }

            // Healer

            // Gulp Missile
            else if (effect_ is EffectDatabase.AbilityEff.GulpMissile)
            {
                EffectDatabase.AbilityEff.GulpMissile gulpMissile = effect_ as EffectDatabase.AbilityEff.GulpMissile;
                bool changedForm = false;
                for (int i = 0; i < gulpMissile.gulpTransformations.Count && !changedForm; i++)
                {

                }
            }
            // Intrepid Sword / Dauntless Shield
            else if (effect_ is EffectDatabase.AbilityEff.IntrepidSword)
            {
                EffectDatabase.AbilityEff.IntrepidSword intrepidSword = effect_ as EffectDatabase.AbilityEff.IntrepidSword;

                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                }
                yield return StartCoroutine(ApplyStatStageMod(
                    statStageMod: intrepidSword.statStageMod,
                    targetPokemon: pokemon,
                    userPokemon: pokemon,
                    forceFailureMessage: true,
                    apply: apply,
                    callback: (result) => 
                    {
                        success = result;
                    }
                    ));
            }
            // Mimicry
            else if (effect_ is EffectDatabase.AbilityEff.Mimicry)
            {
                EffectDatabase.AbilityEff.Mimicry mimicry = effect_ as EffectDatabase.AbilityEff.Mimicry;
                List<string> curTypes = battle.PBPGetTypes(pokemon);

                bool foundCondition = false;
                for (int i = 0; i < mimicry.conditions.Count && !foundCondition; i++)
                {
                    EffectDatabase.AbilityEff.Mimicry.MimicryCondition condition = mimicry.conditions[i];
                    if (battle.BBPIsActiveFromList(condition.conditions))
                    {
                        foundCondition = true;
                        if (!battle.AreTypesContained(containerTypes: condition.types, checkTypes: curTypes))
                        {
                            success = true;
                            if (apply)
                            {
                                battle.SetPokemonTypes(pokemon, condition.types);
                                PBPShowAbility(pokemon, ability);
                                SendEvent(new Battle.View.Events.MessageParameterized
                                {
                                    messageCode = condition.displayText,
                                    pokemonUserID = pokemon.uniqueID,
                                    typeIDs = new List<string>(condition.types)
                                });
                            }
                        }
                    }
                }

                if (!foundCondition)
                {
                    List<string> originalTypes = pokemon.data.types;
                    if (!battle.AreTypesContained(containerTypes: originalTypes, checkTypes: curTypes))
                    {
                        success = true;
                        if (apply)
                        {
                            battle.SetPokemonTypes(pokemon, originalTypes);
                            PBPShowAbility(pokemon, ability);
                            SendEvent(new Battle.View.Events.MessageParameterized
                            {
                                messageCode = mimicry.revertText,
                                pokemonUserID = pokemon.uniqueID,
                                typeIDs = new List<string>(originalTypes)
                            });
                        }
                    }
                }
            }
            // Mold Breaker
            else if (effect_ is EffectDatabase.AbilityEff.MoldBreaker)
            {
                EffectDatabase.AbilityEff.MoldBreaker moldBreaker = effect_ as EffectDatabase.AbilityEff.MoldBreaker;
                success = true;
                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = moldBreaker.displayText,
                        pokemonUserID = pokemon.uniqueID
                    });
                }
            }
            // Moody
            else if (effect_ is EffectDatabase.AbilityEff.Moody)
            {
                EffectDatabase.AbilityEff.Moody moody = effect_ as EffectDatabase.AbilityEff.Moody;
                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                }
                if (moody.statStageMods1 != null)
                {
                    EffectDatabase.General.StatStageMod statStageMod1 =
                        moody.statStageMods1[Random.Range(0, moody.statStageMods1.Count)];
                    yield return StartCoroutine(ApplyStatStageMod(
                        targetPokemon: pokemon,
                        statStageMod: statStageMod1,
                        forceFailureMessage: true,
                        apply: apply,
                        callback: (result) =>
                        {
                            if (result)
                            {
                                success = true;
                            }
                        }
                        ));
                }
                if (moody.statStageMods2 != null)
                {
                    EffectDatabase.General.StatStageMod statStageMod2 =
                        moody.statStageMods2[Random.Range(0, moody.statStageMods2.Count)];
                    yield return StartCoroutine(ApplyStatStageMod(
                        targetPokemon: pokemon,
                        statStageMod: statStageMod2,
                        forceFailureMessage: true,
                        apply: apply,
                        callback: (result) => { success = result; }
                        ));
                }
            }
            // Neutralizing Gas
            else if (effect_ is EffectDatabase.AbilityEff.NeutralizingGas)
            {
                EffectDatabase.AbilityEff.NeutralizingGas neutralizingGas = 
                    effect_ as EffectDatabase.AbilityEff.NeutralizingGas;
                success = true;
                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = neutralizingGas.displayText,
                        pokemonUserID = pokemon.uniqueID
                    });
                }
            }
            // Poison Heal
            else if (effect_ is EffectDatabase.AbilityEff.PoisonHeal)
            {
                EffectDatabase.AbilityEff.PoisonHeal poisonHeal = effect_ as EffectDatabase.AbilityEff.PoisonHeal;
                bool applyHeal = false;
                if (battle.DoEffectFiltersPass(
                    filters: poisonHeal.filters,
                    targetPokemon: pokemon
                    ))
                {
                    for (int i = 0; i < poisonHeal.conditions.Count && applyHeal; i++)
                    {
                        EffectDatabase.AbilityEff.PoisonHeal.HealCondition healCond = poisonHeal.conditions[i];
                        if (healCond.heal != null)
                        {
                            for (int k = 0; k < healCond.conditions.Count && applyHeal; k++)
                            {
                                EffectDatabase.Filter.Harvest curCond = healCond.conditions[k];
                                if (battle.PBPGetSCs(pokemon, curCond).Count > 0)
                                {
                                    applyHeal = true;
                                    success = true;
                                    if (apply)
                                    {
                                        int healAmount = battle.GetHeal(
                                            heal: healCond.heal,
                                            targetPokemon: pokemon,
                                            healerPokemon: pokemon
                                            );
                                        if (healAmount > 0)
                                        {
                                            PBPShowAbility(pokemon, ability);
                                            yield return StartCoroutine(PBPHealPokemon(
                                                pokemon: pokemon,
                                                HPToAdd: healAmount
                                                ));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Pressure
            else if (effect_ is EffectDatabase.AbilityEff.Pressure)
            {
                EffectDatabase.AbilityEff.Pressure pressure = effect_ as EffectDatabase.AbilityEff.Pressure;
                success = true;
                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = pressure.displayText,
                        pokemonUserID = pokemon.uniqueID,
                        abilityID = ability.abilityID
                    });
                }
            }
            // Screen Cleaner
            else if (effect_ is EffectDatabase.AbilityEff.ScreenCleaner)
            {
                EffectDatabase.AbilityEff.ScreenCleaner screenCleaner = effect_ as EffectDatabase.AbilityEff.ScreenCleaner;

                bool showAbility = true;
                // Ally screens
                if (screenCleaner.affectAlly)
                {
                    BattleTeam team = battle.GetTeam(pokemon);
                    List<TeamCondition> conditions = new List<TeamCondition>(team.bProps.lightScreens);
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        EffectDatabase.StatusTEEff.TeamSE lightScreen_ = conditions[i].data.GetEffectNew(TeamSEType.LightScreen);
                        if (lightScreen_ != null)
                        {
                            EffectDatabase.StatusTEEff.LightScreen lightScreen =
                                lightScreen_ as EffectDatabase.StatusTEEff.LightScreen;
                            if (lightScreen.canBeScreenCleaned)
                            {
                                success = true;
                                if (showAbility)
                                {
                                    PBPShowAbility(pokemon, ability);
                                    showAbility = false;
                                }

                                yield return StartCoroutine(HealTeamSC(
                                    targetTeam: team,
                                    condition: conditions[i],
                                    healerPokemon: pokemon
                                    ));
                            }
                        }
                    }
                }

                // Enemy Screens
                if (screenCleaner.affectOpposing)
                {
                    BattleTeam team = battle.GetPokemonOpposingTeam(pokemon);
                    List<TeamCondition> conditions = new List<TeamCondition>(team.bProps.lightScreens);
                    for (int i = 0; i < conditions.Count; i++)
                    {
                        EffectDatabase.StatusTEEff.TeamSE lightScreen_ = conditions[i].data.GetEffectNew(TeamSEType.LightScreen);
                        if (lightScreen_ != null)
                        {
                            EffectDatabase.StatusTEEff.LightScreen lightScreen =
                                lightScreen_ as EffectDatabase.StatusTEEff.LightScreen;
                            if (lightScreen.canBeScreenCleaned)
                            {
                                success = true;
                                if (showAbility)
                                {
                                    PBPShowAbility(pokemon, ability);
                                    showAbility = false;
                                }

                                yield return StartCoroutine(HealTeamSC(
                                    targetTeam: team,
                                    condition: conditions[i],
                                    healerPokemon: pokemon
                                    ));
                            }
                        }
                    }
                }
            }
            // Slow Start
            else if (effect_ is EffectDatabase.AbilityEff.SlowStart)
            {
                EffectDatabase.AbilityEff.SlowStart slowStart = effect_ as EffectDatabase.AbilityEff.SlowStart;
                success = true;
                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = slowStart.displayText,
                        pokemonUserID = pokemon.uniqueID,
                        abilityID = ability.abilityID
                    });
                }
            }
            // Speed Boost
            else if (effect_ is EffectDatabase.AbilityEff.SpeedBoost)
            {
                EffectDatabase.AbilityEff.SpeedBoost speedBoost = effect_ as EffectDatabase.AbilityEff.SpeedBoost;
                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                }
                yield return StartCoroutine(ApplyStatStageMod(
                    targetPokemon: pokemon,
                    statStageMod: speedBoost.statStageMod,
                    forceFailureMessage: true,
                    apply: apply,
                    callback: (result) => { success = result; }
                    ));
            }
            // Steadfast
            else if (effect_ is EffectDatabase.AbilityEff.Steadfast)
            {
                EffectDatabase.AbilityEff.Steadfast steadfast = effect_ as EffectDatabase.AbilityEff.Steadfast;
                if (apply)
                {
                    PBPShowAbility(pokemon, ability);
                }
                yield return StartCoroutine(ApplyStatStageMod(
                    targetPokemon: pokemon,
                    statStageMod: steadfast.statStageMod,
                    forceFailureMessage: true,
                    apply: apply,
                    callback: (result) => { success = result; }
                    ));
            }
            // Trace
            else if (effect_ is EffectDatabase.AbilityEff.Trace)
            {
                EffectDatabase.AbilityEff.Trace trace = effect_ as EffectDatabase.AbilityEff.Trace;
                List<Pokemon> traceablePokemon = new List<Pokemon>();
                List<Pokemon> opposingPokemon = battle.GetOpposingPokemon(pokemon);
                for (int i = 0; i < opposingPokemon.Count; i++)
                {
                    if (battle.PBPGetAbilitiesGainable(opposingPokemon[i]).Count > 0)
                    {
                        traceablePokemon.Add(opposingPokemon[i]);
                    }
                }

                if (traceablePokemon.Count > 0)
                {
                    success = true;
                    if (apply)
                    {
                        Pokemon tracePokemon = traceablePokemon[Random.Range(0, traceablePokemon.Count)];
                        List<Pokemon.Ability> traceAbilities = battle.PBPGetAbilitiesGainable(tracePokemon);
                        PBPShowAbility(pokemon, ability);
                        SendEvent(new Battle.View.Events.MessageParameterized
                        {
                            messageCode = trace.displayText,
                            pokemonUserID = pokemon.uniqueID,
                            pokemonTargetID = tracePokemon.uniqueID
                        });

                        yield return StartCoroutine(PBPRemoveAbility(pokemon: pokemon, ability: ability));
                        yield return StartCoroutine(PBPAddAbilities(pokemon: pokemon, abilities: traceAbilities));
                    }
                }
            }
            // Zen Mode
            else if (effect_ is EffectDatabase.AbilityEff.ZenMode)
            {
                EffectDatabase.AbilityEff.ZenMode zenMode = effect_ as EffectDatabase.AbilityEff.ZenMode;
                if (battle.DoEffectFiltersPass(
                    filters: zenMode.filters,
                    targetPokemon: pokemon
                    ))
                {
                    if (zenMode.transformation.IsPokemonAPreForm(pokemon)
                        && zenMode.transformation.toForm != pokemon.pokemonID)
                    {
                        bool canFormChange = true;
                        float hpPercent = battle.GetPokemonHPAsPercentage(pokemon);

                        if (canFormChange)
                        {
                            if (zenMode.checkBelow && hpPercent > zenMode.hpThreshold)
                            {
                                canFormChange = false;
                            }
                            else if (!zenMode.checkBelow && hpPercent <= zenMode.hpThreshold)
                            {
                                canFormChange = false;
                            }
                        }

                        if (canFormChange)
                        {
                            success = true;
                            if (apply)
                            {
                                PBPShowAbility(pokemon, ability);
                                yield return StartCoroutine(PBPChangeForm(
                                    pokemon: pokemon,
                                    toForm: zenMode.transformation.toForm
                                    ));
                            }
                        }
                    }
                }
            }

            callback(success);
            yield return null;
        }

        public IEnumerator PBPAddAbility(
            Pokemon pokemon,
            Pokemon.Ability ability,
            bool clearAll = false,
            bool activate = true
            )
        {
            yield return StartCoroutine(PBPAddAbilities(
                pokemon: pokemon,
                abilities: new List<Pokemon.Ability> { ability },
                clearAll: clearAll,
                activate: activate
                ));
        }
        public IEnumerator PBPAddAbilities(
            Pokemon pokemon,
            List<Pokemon.Ability> abilities,
            bool clearAll = false,
            bool activate = true
            )
        {
            if (clearAll)
            {
                pokemon.bProps.abilities.Clear();
            }
            for (int i = 0; i < abilities.Count; i++)
            {
                Pokemon.Ability ability = abilities[i];

                // cannot stack same ability
                if (!battle.PBPHasAbility(pokemon: pokemon, abilityID: ability.data.ID))
                {
                    PBPShowAbility(pokemon, abilities[i]);
                    pokemon.bProps.abilities.Add(abilities[i]);
                    if (activate)
                    {
                        yield return StartCoroutine(PBPRunEnterAbility(pokemon, abilities[i]));
                    }
                }
            }
        }
        public IEnumerator PBPRemoveAbility(Pokemon pokemon, Pokemon.Ability ability )
        {
            yield return StartCoroutine(PBPRemoveAbilities(
                pokemon: pokemon,
                abilities: new List<Pokemon.Ability> { ability }
                ));
        }
        public IEnumerator PBPRemoveAbilities(
            Pokemon pokemon,
            List<Pokemon.Ability> abilities,
            bool deactivate = true
            )
        {
            for (int i = 0; i < abilities.Count; i++)
            {
                Pokemon.Ability ability = abilities[i];
                if (deactivate)
                {
                    // TODO: Illusion
                }
                pokemon.bProps.abilities.Remove(ability);
            }
            yield return null;
        }

        public void PBPShowAbility(Pokemon pokemon, Pokemon.Ability ability)
        {
            PBPShowAbility(pokemon, ability.data);
        }
        public void PBPShowAbility(Pokemon pokemon, AbilityData abilityData)
        {
            UpdateClients();
            SendEvent(new Battle.View.Events.PokemonAbilityActivate
            {
                pokemonUniqueID = pokemon.uniqueID,
                abilityID = abilityData.ID
            });
        }

        // ---TEAM PROPERTIES---
        public IEnumerator TBPRemoveEntryHazard(
            BattleTeam team, 
            BattleTeamProperties.EntryHazard entryHazard,
            string textID = "")
        {
            MoveData moveData = MoveDatabase.instance.GetMoveData(entryHazard.hazardID);
            MoveEffect effect = moveData.GetEffect(MoveEffectType.EntryHazard);

            team.bProps.entryHazards.Remove(entryHazard);
            if (textID == "")
            {
                textID = effect.GetString(2);
            }

            SendEvent(new Battle.View.Events.MessageParameterized
            {
                messageCode = textID,
                teamID = team.teamID,
                moveID = moveData.ID
            });
            yield return null;
        }
        public IEnumerator TBPRemoveReflectScreen(
            BattleTeam team, 
            BattleTeamProperties.ReflectScreen reflectScreen,
            string textID = "")
        {
            MoveData moveData = MoveDatabase.instance.GetMoveData(reflectScreen.moveID);
            MoveEffect effect = moveData.GetEffect(MoveEffectType.Reflect);

            team.bProps.reflectScreens.Remove(reflectScreen);
            if (textID == "")
            {
                textID = effect.GetString(2);
                textID = (textID == "DEFAULT") ? "move-reflect-remove" : textID;
            }

            SendEvent(new Battle.View.Events.MessageParameterized
            {
                messageCode = textID,
                teamID = team.teamID,
                moveID = moveData.ID
            });
            yield return null;
        }
        public IEnumerator TBPRemoveSafeguard(
            BattleTeam team,
            BattleTeamProperties.Safeguard safeguard,
            string textID = "")
        {
            MoveData moveData = MoveDatabase.instance.GetMoveData(safeguard.moveID);
            MoveEffect effect = moveData.GetEffect(MoveEffectType.Safeguard);

            team.bProps.safeguards.Remove(safeguard);
            if (textID == "")
            {
                textID = effect.GetString(2);
                textID = (textID == "DEFAULT") ? "move-safeguard-remove" : textID;
            }

            SendEvent(new Battle.View.Events.MessageParameterized
            {
                messageCode = textID,
                teamID = team.teamID,
                moveID = moveData.ID
            });
            yield return null;
        }

        // ---BATTLE PROPERTIES---

        public IEnumerator UntieListedPokemon(List<Pokemon> pokemon)
        {
            for (int i = 0; i < pokemon.Count; i++)
            {
                yield return StartCoroutine(UntiePokemon(pokemon[i]));
            }
        }
        public IEnumerator UntiePokemon(Pokemon tiedPokemon)
        {
            List<Pokemon.Ability> abilities = battle.PBPGetAbilities(tiedPokemon);
            List<Pokemon> otherPokemon = new List<Pokemon>(battle.pokemonOnField);
            otherPokemon.Remove(tiedPokemon);

            // Desolate Land
            List<EffectDatabase.AbilityEff.AbilityEffect> drought_ 
                = battle.PBPGetAbilityEffects(pokemon: tiedPokemon, effectType: AbilityEffectType.Drought);
            for (int i = 0; i < drought_.Count; i++)
            {
                EffectDatabase.AbilityEff.Drought drought = drought_[i] as EffectDatabase.AbilityEff.Drought;
                if (drought.desolateLand)
                {
                    BattleCondition existingCondition = battle.BBPGetSC(drought.inflictStatus.statusID);
                    if (existingCondition != null)
                    {
                        bool otherUserActive = false;

                        // Check for any other on-field users with the same effect
                        for (int k = 0; k < otherPokemon.Count && !otherUserActive; k++)
                        {
                            if (battle.IsPokemonOnFieldAndAble(otherPokemon[k]))
                            {
                                List<EffectDatabase.AbilityEff.AbilityEffect> onFieldDrought_ =
                                    battle.PBPGetAbilityEffects(
                                        pokemon: otherPokemon[k], 
                                        effectType: AbilityEffectType.Drought);
                                for (int j = 0; j < onFieldDrought_.Count && !otherUserActive; j++)
                                {
                                    EffectDatabase.AbilityEff.Drought onFieldDrought =
                                        onFieldDrought_[j] as EffectDatabase.AbilityEff.Drought;
                                    if (onFieldDrought.desolateLand 
                                        && onFieldDrought.inflictStatus.statusID == drought.inflictStatus.statusID)
                                    {
                                        otherUserActive = true;
                                    }
                                }
                            }
                        }

                        if (!otherUserActive)
                        {
                            yield return StartCoroutine(HealBattleSC(existingCondition));
                        }
                    }
                }
            }

            for (int i = 0; i < battle.pokemonOnField.Count; i++)
            {
                yield return StartCoroutine(UntiePokemon(
                    targetPokemon: battle.pokemonOnField[i],
                    tiedPokemon: tiedPokemon
                    ));
            }

            // Unset Neutralizing Gas
            bool setNeutralizingGas = battle.PBPGetAbilityEffect(tiedPokemon, AbilityEffectType.NeutralizingGas) != null;
            if (setNeutralizingGas && !battle.PBPIsNeutralizingGasInEffect())
            {
                yield return StartCoroutine(PBPRunEnterAbilities(pokemon: battle.pokemonOnField));
            }
        }
        public IEnumerator UntiePokemon(Pokemon targetPokemon, Pokemon tiedPokemon)
        {
            // Unset Bind
            if (targetPokemon.bProps.bindPokemon == tiedPokemon.uniqueID)
            {
                battle.UnsetPokemonBindMove(targetPokemon);
            }

            // Unset Block
            if (targetPokemon.bProps.blockPokemon == tiedPokemon.uniqueID)
            {
                battle.UnsetPokemonBlockMove(targetPokemon);
            }

            // Unset Infatuation
            if (targetPokemon.bProps.infatuation != null)
            {
                if (targetPokemon.bProps.infatuation.infatuator == tiedPokemon.uniqueID)
                {
                    targetPokemon.bProps.infatuation = null;
                }
            }

            // Unset Lock On
            List<Pokemon.BattleProperties.LockOn> lockOnTargets 
                = new List<Pokemon.BattleProperties.LockOn>(targetPokemon.bProps.lockOnTargets);
            for (int i = 0; i < lockOnTargets.Count; i++)
            {
                if (lockOnTargets[i].pokemonUniqueID == tiedPokemon.uniqueID)
                {
                    targetPokemon.bProps.lockOnTargets.Remove(lockOnTargets[i]);
                }
            }

            // Unset Sky Drop
            if (targetPokemon.bProps.skyDropUser == tiedPokemon.uniqueID)
            {
                yield return StartCoroutine(FreePokemonFromSkyDrop(targetPokemon));
            }

            yield return null;
        }
        public IEnumerator FreePokemonFromSkyDrop(Pokemon targetPokemon)
        {
            string skyDropMove = targetPokemon.bProps.skyDropMove;
            if (skyDropMove != null)
            {
                MoveData moveData = MoveDatabase.instance.GetMoveData(skyDropMove);
                MoveEffect effect = moveData.GetEffect(MoveEffectType.SkyDrop);
                targetPokemon.bProps.skyDropMove = null;
                targetPokemon.bProps.skyDropUser = null;

                targetPokemon.bProps.inDigState = false;
                targetPokemon.bProps.inDiveState = false;
                targetPokemon.bProps.inFlyState = false;
                targetPokemon.bProps.inShadowForceState = false;

                string textID = effect.GetString(1);
                textID = (textID == "DEFAULT") ? "move-skydrop-free-default" : textID;

                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = textID,
                    pokemonTargetID = targetPokemon.uniqueID,
                    moveID = moveData.ID
                });
            }
            yield return null;
        }

        public IEnumerator ExecuteCommandBag(BattleCommand command)
        {
            Pokemon itemPokemon = command.commandUser;
            Trainer trainer = command.itemTrainer;

            bool itemSuccess = true;

            Item item = new Item(command.itemID);

            // check item stock
            if (itemSuccess) 
            {
                if (trainer.GetItemCount(item.itemID) == 0)
                {
                    itemSuccess = false;
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = "item-fail-run-out",
                        itemID = item.itemID,
                        trainerID = trainer.playerID
                    });
                }
            }

            // Fainted?
            if (itemSuccess)
            {
                if (battle.IsPokemonFainted(itemPokemon))
                {
                    // check items that can be used on fainted pokemon
                    if (item.data.GetEffect(ItemEffectType.Revive) == null)
                    {
                        itemSuccess = false;
                    }
                }
                else
                {
                    // check items that can only be used on fainted pokemon
                    if (item.data.GetEffect(ItemEffectType.Revive) != null)
                    {
                        itemSuccess = false;
                    }
                }
            }

            // Embargo check
            if (itemSuccess)
            {
                if (itemPokemon.bProps.embargo != null && !item.data.HasTag(ItemTag.BypassEmbargo))
                {
                    itemSuccess = false;
                    SendEvent(new Battle.View.Events.MessageParameterized
                    {
                        messageCode = itemPokemon.bProps.embargo.effect.attemptText,
                        pokemonUserID = itemPokemon.uniqueID,
                        itemID = item.itemID
                    });
                }
            }

            // Only useable in battle
            if (itemSuccess)
            {
                if (item.data.HasTag(ItemTag.OnlyUseableInBattle)
                    && !battle.IsPokemonOnField(itemPokemon))
                {
                    itemSuccess = false;
                }
            }

            // run item effects if we satisfied all checks
            if (itemSuccess)
            {
                SendEvent(new Battle.View.Events.TrainerItemUse
                {
                    playerID = trainer.playerID,
                    itemID = item.itemID
                });

                bool effectSuccess = false;

                yield return StartCoroutine(ApplyItemEffects(
                    pokemon: itemPokemon,
                    item: item,
                    callback: (result) =>
                    {

                    }
                    ));

                // Run On-Use Effects
                yield return StartCoroutine(ApplyItemEffects(
                    pokemon: itemPokemon,
                    item: item,
                    effects: item.data.GetEffectsOnUse(),
                    callback: (result) =>
                    {
                        if (result)
                        {
                            effectSuccess = true;
                        }
                    }
                    ));

                if (!effectSuccess)
                {
                    // Display "Nothing Happened!"
                }

                trainer.RemoveItem(item.itemID);
            }
        

            if (battle.IsPokemonOnFieldAndAble(itemPokemon))
            {
                itemPokemon.bProps.actedThisTurn = true;
            }
            yield return null;
        }

        public IEnumerator TryToFailItem(
            Pokemon pokemon,
            Item item,
            System.Action<bool> callback
            )
        {
            bool success = true;



            callback(success);
            yield return null;
        }

        public IEnumerator ExecuteCommandRun(BattleCommand command)
        {
            Pokemon runPokemon = command.commandUser;

            bool runSuccess = true;

            // Can't run if fainted
            if (runSuccess)
            {
                if (!battle.IsPokemonOnFieldAndAble(runPokemon))
                {
                    runSuccess = false;
                }
            }

            if (runSuccess)
            {
                battle.runningPokemon = runPokemon;

                Pokemon.Ability runAwayAbility = 
                    battle.PBPGetAbilityWithEffect(pokemon: runPokemon, effectType: AbilityEffectType.RunAway);
                if (runAwayAbility != null)
                {
                    EffectDatabase.AbilityEff.RunAway runAway = 
                        runAwayAbility.data.GetEffectNew(AbilityEffectType.RunAway) as EffectDatabase.AbilityEff.RunAway;
                    PBPShowAbility(pokemon: runPokemon, ability: runAwayAbility);
                }
                SendEvent(new Battle.View.Events.MessageParameterized
                {
                    messageCode = "pokemon-run",
                    pokemonUserID = runPokemon.uniqueID
                });
            
            }
            if (battle.IsPokemonOnFieldAndAble(runPokemon))
            {
                runPokemon.bProps.actedThisTurn = true;
            }
        
            yield return null;
        }

        // Sending / Receiving Player Commands / Events
        public void ReceiveCommands(
            int playerID,
            List<BattleCommand> commands, 
            bool isReplace = false)
        {
            battle.ConvertToInstanceBattleCommands(commands);

            if (executingBatonPass)
            {
                batonPassReplaceCommands.AddRange(commands);
                executingBatonPass = false;
            }
            else
            {
                if (isReplace)
                {
                    replaceCommands.AddRange(commands);
                }
                else
                {
                    allCommands.AddRange(commands);
                }
            }
            
            // Stop waiting for this player
            waitConnections.Remove(playerID);
        }
        public void ReceiveCommands(
            int playerID,
            List<PBS.Player.Command> commands, 
            bool isReplace = false)
        {
            // Convert commands to instance battle commands, referencing battle model pokemon, items, trainers, etc.
            List<BattleCommand> sanitizedCommands = battle.SanitizeCommands(commands);
            if (executingBatonPass)
            {
                batonPassReplaceCommands.AddRange(sanitizedCommands);
                executingBatonPass = false;
            }
            else
            {
                if (isReplace)
                {
                    replaceCommands.AddRange(sanitizedCommands);
                }
                else
                {
                    allCommands.AddRange(sanitizedCommands);
                }
            }
            
            // Stop waiting for this player
            waitConnections.Remove(playerID);
        }
        public IEnumerator WaitForPlayer()
        {
            while (waitConnections.Count > 0)
            {
                yield return null;
            }
        }

        // AI Commands / Events
        public void SelectAICommands()
        {
            for (int i = 0; i < battle.teams.Count; i++)
            {
                for (int j = 0; j < battle.teams[i].trainers.Count; j++)
                {
                    Trainer trainer = battle.teams[i].trainers[j];
                    if (trainer.IsAIControlled())
                    {
                        List<Pokemon> commandablePokemon = battle.GetTrainerCommandablePokemon(trainer);
                        if (commandablePokemon.Count > 0)
                        {
                            List<BattleCommand> aiCommands = ai.GetCommandsByPrompt(
                                trainer,
                                commandablePokemon
                                );
                            ReceiveCommands(trainer.playerID, aiCommands, false);
                        }
                    }
                
                }
            }
            ai.ResetSelf();
        }
        public void SelectAIReplacements()
        {
            for (int i = 0; i < battle.teams.Count; i++)
            {
                for (int j = 0; j < battle.teams[i].trainers.Count; j++)
                {
                    Trainer trainer = battle.teams[i].trainers[j];
                    if (trainer.IsAIControlled())
                    {
                        if (battle.IsTrainerAbleToBattle(trainer))
                        {
                            List<int> emptyPositions = battle.GetEmptyTrainerControlPositions(trainer);
                            if (emptyPositions.Count > 0)
                            {
                                List<Pokemon> availablePokemon =
                                    battle.GetTrainerFirstXAvailablePokemon(trainer, emptyPositions.Count);
                                if (availablePokemon.Count > 0)
                                {
                                    List<BattleCommand> aiCommands = ai.GetReplacementsByPrompt(
                                        trainer,
                                        emptyPositions.GetRange(0, availablePokemon.Count)
                                        );
                                    ReceiveCommands(trainer.playerID, aiCommands, true);
                                }
                            }
                        }
                    }
                }
            }
            ai.ResetSelf();
        }

    }
}


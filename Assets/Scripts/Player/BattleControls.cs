using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PBS.Player
{
    public class BattleControls : MonoBehaviour
    {
        // Model
        public PBS.Battle.View.Model battleModel;
        public PBS.Networking.Player player;

        // Components
        public PBS.Battle.View.UI.Canvas battleUI;
        public Battle.View.Scene.Canvas sceneCanvas;

        // Controller
        public enum ControlContext
        {
            None,
            Command,
            CommandExtra,
            Fight,
            FieldTarget,
            Party,
            PartyCommand,
            BagPocket,
            Bag,
            BagTarget
        }
        private Controls _controls;
        private ControlContext context;
        private bool lockControls = false;

        private Coroutine
            waitCR,
            controlCommandCR,
            controlExtraCommandCR,
            controlFightCR,
            controlFightTargetCR,
            controlPartyCR,
            controlPartyCommandCR,
            controlBagPocketCR,
            controlBagCR,
            controlBagTargetCR;
        private bool waitCRActive = false;
        Battle.View.Events.CommandGeneralPrompt commandPromptEvent;
        private PBS.Battle.View.Events.CommandAgent commandAgent;
        private PBS.Battle.View.Compact.Trainer commandTrainer;
        private Command playerCommand;
        private Command[] committedCommands;
    
        [HideInInspector] public bool isSendingCommands = false;
        [HideInInspector] public bool isSendingReplacements = false;
        [HideInInspector] public Command[] commandsToBeSent;

        // Command
        private bool choosingCommand;
        private List<BattleCommandType> commandTypes;
        private int commandIndex = 0;

        // Command Extra
        private bool choosingExtraCommand;
        private List<BattleExtraCommand> extraCommands;
        private int extraCommandIndex = 0;

        // Fight
        private bool choosingFight;
        private bool forceMove;
        private List<Battle.View.Events.CommandAgent.Moveslot> moveslots;
        private int moveIndex;
        private bool canMegaEvolve, chooseMegaEvolve;
        private bool canZMove, chooseZMove;
        private bool canDynamax, chooseDynamax;
        private bool canFormChange; // Ultra-Burst

        // Fight Target
        private bool choosingFightTarget;
        private Battle.View.Events.CommandAgent.Moveslot selectedMoveslot;
        private List<List<BattlePosition>> moveTargets;
        private int moveTargetIndex;

        // Party
        private bool choosingParty;
        private bool forceSwitch;
        private bool forceReplace;
        private List<Battle.View.Compact.Pokemon> partySlots;
        private int partyIndex;
        private int switchPosition;

        // Bag
        private bool choosingBagPocket;
        private List<ItemBattlePocket> itemPockets;
        private int itemPocketIndex;

        private bool choosingItem;
        private List<Item> itemSlots;
        private int itemOffset;
        private int itemIndex;
        private Item selectedItem;

        private void Awake()
        {
            _controls = new Controls();
            SwitchControlContext(ControlContext.None);
        }

        private void OnEnable()
        {
            _controls.Enable();
        }
        private void OnDisable()
        {
            _controls.Disable();
        }

        // Controls

        public void SetControls()
        {
            _controls.BattleDialog.Select.performed += (obj) =>
            {
                battleUI.dialog.advancedDialogPressed = true;
            };
        }
        public bool AreControlsLocked()
        {
            return lockControls || waitCRActive;
        }
        public IEnumerator DelayControls(float waitTime = 0.02f)
        {
            lockControls = true;
            yield return new WaitForSeconds(waitTime);
            lockControls = false;
        }
        public void SwitchControlContext(ControlContext newContext, bool delayControls = true)
        {
            // Delay new control input
            if (delayControls)
            {
                StartCoroutine(DelayControls());
            }

            // unset previous listeners
            switch (context)
            {
                case ControlContext.Command:
                    _controls.BattleMenuCommand.Cancel.performed -= CancelCommandMenu;
                    _controls.BattleMenuCommand.Select.performed -= SelectCommandMenu;
                    _controls.BattleMenuCommand.Move.performed -= NavigateCommandMenuHorizontal;
                    break;

                case ControlContext.Fight:
                    _controls.BattleMenuFight.Cancel.performed -= CancelFightMenu;
                    _controls.BattleMenuFight.Select.performed -= SelectFightMenu;
                    _controls.BattleMenuFight.Special.performed -= SelectFightSpecial;
                    _controls.BattleMenuFight.Move.performed -= NavigateFightMenuQuad;
                    break;

                case ControlContext.FieldTarget:
                    _controls.BattleMenuFieldTarget.Cancel.performed -= CancelFieldTargetMenu;
                    _controls.BattleMenuFieldTarget.Select.performed -= SelectFieldTargetMenu;
                    _controls.BattleMenuFieldTarget.Move.performed -= NavigateFieldTargetMenu;
                    break;

                case ControlContext.Party:
                    _controls.BattleMenuParty.Cancel.performed -= CancelPartyMenu;
                    _controls.BattleMenuParty.Select.performed -= SelectPartyMenu;
                    _controls.BattleMenuParty.Move.performed -= NavigatePartyMenuQuad;
                    break;

                case ControlContext.PartyCommand:
                    _controls.BattleMenuPartyCommand.Cancel.performed -= CancelPartyCommandMenu;
                    _controls.BattleMenuPartyCommand.Select.performed -= SelectPartyCommandMenu;
                    _controls.BattleMenuPartyCommand.Move.performed -= NavigatePartyCommandMenu;
                    break;

                case ControlContext.BagPocket:
                    _controls.BattleMenuBag.Cancel.performed -= CancelBagPocketMenu;
                    _controls.BattleMenuBag.Select.performed -= SelectBagPocketMenu;
                    _controls.BattleMenuBag.Move.performed -= NavigateBagPocketMenuQuad;
                    break;

                case ControlContext.Bag:
                    _controls.BattleMenuBagItem.Cancel.performed -= CancelBagMenu;
                    _controls.BattleMenuBagItem.Select.performed -= SelectBagMenu;
                    _controls.BattleMenuBagItem.Move.performed -= NavigateBagMenuQuad;
                    break;

                default:
                    break;
            }

            // switch to new context, setting relevant listeners
            context = newContext;
            switch (context)
            {
                case ControlContext.Command:
                    _controls.BattleMenuCommand.Cancel.performed += CancelCommandMenu;
                    _controls.BattleMenuCommand.Select.performed += SelectCommandMenu;
                    _controls.BattleMenuCommand.Move.performed += NavigateCommandMenuHorizontal;
                    break;

                case ControlContext.Fight:
                    _controls.BattleMenuFight.Cancel.performed += CancelFightMenu;
                    _controls.BattleMenuFight.Select.performed += SelectFightMenu;
                    _controls.BattleMenuFight.Special.performed += SelectFightSpecial;
                    _controls.BattleMenuFight.Move.performed += NavigateFightMenuQuad;
                    break;

                case ControlContext.FieldTarget:
                    _controls.BattleMenuFieldTarget.Cancel.performed += CancelFieldTargetMenu;
                    _controls.BattleMenuFieldTarget.Select.performed += SelectFieldTargetMenu;
                    _controls.BattleMenuFieldTarget.Move.performed += NavigateFieldTargetMenu;
                    break;

                case ControlContext.Party:
                    _controls.BattleMenuParty.Cancel.performed += CancelPartyMenu;
                    _controls.BattleMenuParty.Select.performed += SelectPartyMenu;
                    _controls.BattleMenuParty.Move.performed += NavigatePartyMenuQuad;
                    break;

                case ControlContext.PartyCommand:
                    _controls.BattleMenuPartyCommand.Cancel.performed += CancelPartyCommandMenu;
                    _controls.BattleMenuPartyCommand.Select.performed += SelectPartyCommandMenu;
                    _controls.BattleMenuPartyCommand.Move.performed += NavigatePartyCommandMenu;
                    break;

                case ControlContext.BagPocket:
                    _controls.BattleMenuBag.Cancel.performed += CancelBagPocketMenu;
                    _controls.BattleMenuBag.Select.performed += SelectBagPocketMenu;
                    _controls.BattleMenuBag.Move.performed += NavigateBagPocketMenuQuad;
                    break;

                case ControlContext.Bag:
                    _controls.BattleMenuBagItem.Cancel.performed += CancelBagMenu;
                    _controls.BattleMenuBagItem.Select.performed += SelectBagMenu;
                    _controls.BattleMenuBagItem.Move.performed += NavigateBagMenuQuad;
                    break;

                default:
                    break;
            }
        }

        private void SetDefaultPromptVars()
        {
            // set Default Values
            commandTypes = new List<BattleCommandType>();
            commandIndex = 0;

            moveslots = new List<PBS.Battle.View.Events.CommandAgent.Moveslot>();
            moveIndex = -1;
            canMegaEvolve = false;
            chooseMegaEvolve = false;
            canZMove = false;
            chooseZMove = false;
            canDynamax = false;
            chooseDynamax = false;
            canFormChange = false;

            moveTargets = new List<List<BattlePosition>>();
            moveTargetIndex = 0;

            partySlots = new List<PBS.Battle.View.Compact.Pokemon>();
            partyIndex = 0;
            forceSwitch = false;
            forceReplace = false;

            itemPockets = new List<ItemBattlePocket>();
            itemPocketIndex = 0;

            itemSlots = new List<Item>();
            itemIndex = 0;
            itemOffset = 0;
            selectedItem = null;
        }

        public IEnumerator HandlePromptCommands(
            PBS.Battle.View.Events.CommandGeneralPrompt bEvent,
            Action<Command[]> callback)
        {
            commandPromptEvent = bEvent;
            Battle.View.Compact.Trainer trainer = battleModel.GetMatchingTrainer(bEvent.playerID);
            committedCommands = new Command[bEvent.pokemonToCommand.Count];

            List<PBS.Battle.View.Events.CommandAgent> pokemonToControl 
                = new List<PBS.Battle.View.Events.CommandAgent>(bEvent.pokemonToCommand);

            battleUI.SetPokemonHUDsActive(true);
            for (int i = 0; i < pokemonToControl.Count; i++)
            {
                battleUI.UndrawDialogBox();

                playerCommand = null;
                committedCommands[i] = null;
                PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(pokemonToControl[i].pokemonUniqueID);
                switchPosition = pokemon.battlePos;

                SetDefaultPromptVars();
                controlCommandCR = StartCoroutine(ControlPromptCommand(
                    agent: pokemonToControl[i],
                    trainer: trainer,
                    pokemonIndex: i,
                    setCommands: committedCommands));
                yield return controlCommandCR;

                // go back a pokemon if we clicked back
                if (playerCommand == null)
                {
                    i -= 2;
                }
                // set the chosen command
                else
                {
                    committedCommands[i] = playerCommand;
                }
                battleUI.UnsetPanels();
            }
            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.None);
            this.commandPromptEvent = null;
            callback(committedCommands);
        }

        public IEnumerator HandlePromptReplace(
            PBS.Battle.View.Events.CommandReplacementPrompt bEvent,
            Action<Command[]> callback)
        {
            PBS.Battle.View.Compact.Trainer trainer = battleModel.GetMatchingTrainer(bEvent.playerID);
            int[] fillPositions = bEvent.fillPositions;
            committedCommands = new Command[bEvent.fillPositions.Length];

            battleUI.SetPokemonHUDsActive(true);
            for (int i = 0; i < fillPositions.Length; i++)
            {
                playerCommand = null;
                committedCommands[i] = null;

                // set Default Values
                partySlots = new List<PBS.Battle.View.Compact.Pokemon>();
                partyIndex = 0;
                switchPosition = fillPositions[i];
                forceSwitch = true;
                forceReplace = true;

                // dialog prompt
                Debug.Log("Who will you send in?");

                controlCommandCR = StartCoroutine(ControlPromptParty(
                    agent: null,
                    trainer: trainer,
                    setCommands: committedCommands,
                    forceSwitch: forceSwitch));
                yield return controlCommandCR;
                SwitchControlContext(ControlContext.None);

                // go back a pokemon if we clicked back
                if (playerCommand == null)
                {
                    i -= 2;
                }
                // set the chosen command
                else
                {
                    playerCommand.commandType = BattleCommandType.PartyReplace;
                    committedCommands[i] = this.playerCommand;
                }
            }
            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.None);
            callback(committedCommands);
        }

        private IEnumerator ControlPromptCommand(
            PBS.Battle.View.Events.CommandAgent agent, 
            PBS.Battle.View.Compact.Trainer trainer, 
            int pokemonIndex, 
            Command[] setCommands)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(agent);
            commandAgent = agent;
            commandTrainer = trainer;
            committedCommands = (setCommands == null) ? new Command[0] : setCommands;

            // set the viable commands for this pokemon
            commandTypes = new List<BattleCommandType>(agent.commandTypes);

            // can only go back a command if we can control more than one pokemon
            if (pokemonIndex > 0)
            {
                commandTypes.Insert(0, BattleCommandType.Back);
            }

            // set the initial command index
            if (commandIndex > commandTypes.Count)
            {
                commandIndex = 0;
            }
            // do not default to "Back" command, start at "Fight"
            if (commandTypes[commandIndex] == BattleCommandType.Back)
            {
                commandIndex = commandTypes.IndexOf(BattleCommandType.Fight);
            }

            // set the controls and ui elements
            choosingCommand = true;
            SwitchControlContext(ControlContext.Command);

            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Command);
            battleUI.SetCommands(pokemon, commandTypes);
            battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);

            while (choosingCommand)
            {
                yield return null;
            }

            SwitchControlContext(ControlContext.None);
        }

        private IEnumerator ControlPromptCommandExtra(
            PBS.Battle.View.Events.CommandAgent agent, 
            PBS.Battle.View.Compact.Trainer trainer,
            Command[] setCommands,
            List<BattleExtraCommand> commandList)
        {
            commandAgent = agent;
            commandTrainer = trainer;
            committedCommands = (setCommands == null) ? new Command[0] : setCommands;

            // set the viable commands for this pokemon
            extraCommands = commandList;

            // set the initial command index
            if (extraCommandIndex > extraCommands.Count)
            {
                extraCommandIndex = 0;
            }

            // set the controls and ui elements
            choosingExtraCommand = true;
            SwitchControlContext(ControlContext.PartyCommand);

            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.PartyCommand);
            battleUI.SetPartyCommands(partySlots[partyIndex], extraCommands);
            battleUI.SwitchSelectedPartyCommandTo(extraCommands[extraCommandIndex]);

            while (choosingExtraCommand)
            {
                yield return null;
            }
        }

        public IEnumerator ControlPromptFight(
            PBS.Battle.View.Events.CommandAgent agent, 
            PBS.Battle.View.Compact.Trainer trainer,
            Command[] setCommands,
            bool forceMove = false)
        {
            PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(agent);
            commandAgent = agent;
            commandTrainer = trainer;
            committedCommands = (setCommands == null) ? new Command[0] : setCommands;
            this.forceMove = forceMove;

            // set the viable moves for this pokemon
            moveslots = (agent.isDynamaxed)?  new List<Battle.View.Events.CommandAgent.Moveslot>(agent.dynamaxMoveSlots)
                : new List<Battle.View.Events.CommandAgent.Moveslot>(agent.moveslots);
            bool anyUseable = false;
            for (int i = 0; i < moveslots.Count; i++)
            {
                if (moveslots[i].useable)
                {
                    anyUseable = true;
                }
            }

            // if there's no useable moves, show struggle option
            if (!anyUseable)
            {
                moveslots = new List<PBS.Battle.View.Events.CommandAgent.Moveslot> { new Battle.View.Events.CommandAgent.Moveslot("struggle") };
            }
            else
            {
                // Check Mega-Evolution / Form Change
                if (battleModel.settings.canMegaEvolve
                    && commandPromptEvent.canMegaEvolve
                    && agent.canMegaEvolve
                    && !canZMove 
                    && !canDynamax 
                    && !canFormChange)
                {
                    canMegaEvolve = true;
                }

                // Check Z-Move
                if (battleModel.settings.canZMove
                    && commandPromptEvent.canZMove
                    && agent.canZMove
                    && !canMegaEvolve 
                    && !canDynamax
                    && !agent.isDynamaxed)
                {
                    canZMove = true;
                }

                // Check Dynamax
                if (battleModel.settings.canDynamax
                    && commandPromptEvent.canDynamax
                    && agent.canDynamax
                    && !canMegaEvolve 
                    && !canZMove)
                {
                    canDynamax = true;
                }
            }
            chooseDynamax = commandAgent.isDynamaxed;

            // set the initial move index
            moveIndex = 0;

            choosingFight = true;
            SwitchControlContext(ControlContext.Fight);

            battleUI.SetMoves(
                pokemon: pokemon,
                moveslots: moveslots, 
                canMegaEvolve: canMegaEvolve, 
                canZMove: canZMove,
                canDynamax: canDynamax,
                choosingZMove: chooseZMove,
                choosingMaxMove: chooseDynamax);
            battleUI.SwitchPanel(Battle.View.Enums.Panel.Fight);
            battleUI.SwitchSelectedMoveTo(
                pokemon: pokemon, 
                selected: moveIndex, 
                choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                choosingZMove: chooseZMove,
                choosingMaxMove: chooseDynamax);

            while (choosingFight)
            {
                yield return null;
            }
        }

        public IEnumerator ControlPromptFieldTarget(
            PBS.Battle.View.Events.CommandAgent agent,
            PBS.Battle.View.Compact.Trainer trainer,
            PBS.Battle.View.Events.CommandAgent.Moveslot selectedMoveslot,
            Command[] setCommands = null
            )
        {
            PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(agent);
            commandAgent = agent;
            commandTrainer = trainer;
            this.selectedMoveslot = selectedMoveslot;
            committedCommands = (setCommands == null) ? new Command[0] : setCommands;

            // get the possible targets for the given move
            moveTargets = new List<List<BattlePosition>>(selectedMoveslot.possibleTargets);
            moveTargets.Insert(0, null);

            // set the initial move index
            if (moveTargetIndex > moveTargets.Count)
            {
                moveTargetIndex = 0;
            }
            // do not default to "Back" command if possible
            if (moveTargets[moveTargetIndex] == null && moveTargets.Count > 0)
            {
                moveTargetIndex++;
            }

            choosingFightTarget = true;
            SwitchControlContext(ControlContext.FieldTarget);

            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.FieldTargeting);
            battleUI.SetFieldTargets(battleModel, trainer.teamPos);
            battleUI.SwitchSelectedMoveTargetsTo(battleModel, battleModel.GetPokemonPosition(pokemon), moveTargetIndex, moveTargets);

            while (choosingFightTarget)
            {
                // The player is selecting target
                yield return null;
            }
        }

        public IEnumerator ControlPromptParty(
            PBS.Battle.View.Events.CommandAgent agent,
            PBS.Battle.View.Compact.Trainer trainer,
            Command[] setCommands = null,
            bool forceSwitch = false
            )
        {
            commandAgent = agent;
            commandTrainer = trainer;
            committedCommands = (setCommands == null) ? new Command[0] : setCommands;
            this.forceSwitch = forceSwitch;

            // set the possible party pokemon that can switch in
            partySlots = new List<PBS.Battle.View.Compact.Pokemon>(commandTrainer.party);

            // set the initial party index
            partyIndex = 0;

            choosingParty = true;
            SwitchControlContext(ControlContext.Party);

            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Party);
            battleUI.SetParty(partySlots, forceSwitch, selectedItem);
            battleUI.SwitchSelectedPartyMemberTo(partySlots[partyIndex]);

            while (choosingParty)
            {
                yield return null;
            }
        }

        public IEnumerator ControlPromptBagPocket(
            PBS.Battle.View.Events.CommandAgent agent,
            PBS.Battle.View.Compact.Trainer trainer
            )
        {
            commandAgent = agent;
            commandTrainer = trainer;

            itemPockets = new List<ItemBattlePocket>
            {
                ItemBattlePocket.HPRestore,
                ItemBattlePocket.Pokeballs,
                ItemBattlePocket.StatusRestore,
                ItemBattlePocket.BattleItems
            };

            // set the initial item index
            itemPocketIndex = 0;

            choosingBagPocket = true;
            SwitchControlContext(ControlContext.BagPocket);

            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Bag);
            battleUI.SetBagPockets(itemPockets);
            battleUI.SwitchSelectedBagPocketTo(itemPockets[itemPocketIndex]);

            while (choosingBagPocket)
            {
                yield return null;
            }
        }

        public IEnumerator ControlPromptBag(
            PBS.Battle.View.Events.CommandAgent agent,
            PBS.Battle.View.Compact.Trainer trainer,
            ItemBattlePocket pocket,
            Command[] setCommands = null
            )
        {
            PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(agent);
            commandAgent = agent;
            commandTrainer = trainer;
            committedCommands = (setCommands == null) ? new Command[0] : setCommands;

            // set the possible items that the trainer can use
            itemSlots = new List<Item>();
            HashSet<string> addedItems = new HashSet<string>();
            for (int i = 0; i < trainer.items.Count; i++)
            {
                Item item = new Item(trainer.items[i]);
                if (item.data.battlePocket == pocket && !addedItems.Contains(item.itemID))
                {
                    itemSlots.Add(item);
                    addedItems.Add(item.itemID);
                } 
            }

            // set the initial item index
            itemOffset = 0;
            itemIndex = 0;

            choosingItem = true;
            SwitchControlContext(ControlContext.Bag);

            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.BagItem);
            battleUI.SetItems(trainer, pocket, itemSlots, itemOffset);

            if (itemSlots.Count == 0)
            {
                itemIndex = -1;
                battleUI.SwitchSelectedItemToBack();
            }
            else
            {
                battleUI.SwitchSelectedItemTo(itemSlots[itemOffset + itemIndex]);
            }

            while (choosingItem)
            {
                yield return null;
            }
        }


        // COMMAND MENU
        private void NavigateCommandMenuHorizontal(InputAction.CallbackContext obj)
        {
            int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            NavigateCommandMenu(addIndex);
        }
        private void NavigateCommandMenu(int scrollAmount)
        {
            if (AreControlsLocked())
            {
                return;
            }
            if (scrollAmount != 0)
            {
                commandIndex += scrollAmount;
                commandIndex %= this.commandTypes.Count;
                if (commandIndex < 0)
                {
                    commandIndex += commandTypes.Count;
                }
                battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
            }
        }
    
        private void SelectCommandMenu(InputAction.CallbackContext obj)
        {
            SelectCommandMenu();
        }
        private void SelectCommandMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            BattleCommandType commandType = commandTypes[commandIndex];

            // Exit Commands
            if (commandType == BattleCommandType.Back)
            {
                CancelCommandMenu();
            }
            else
            {
                Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
                if (commandType == BattleCommandType.Fight)
                {
                    controlFightCR = StartCoroutine(ControlPromptFight(
                        commandAgent,
                        commandTrainer,
                        committedCommands,
                        forceMove
                        ));
                }
                else if (commandType == BattleCommandType.Party)
                {
                    controlPartyCR = StartCoroutine(ControlPromptParty(
                        commandAgent,
                        commandTrainer,
                        committedCommands,
                        forceSwitch
                        ));
                }
                else if (commandType == BattleCommandType.Bag)
                {
                    controlBagPocketCR = StartCoroutine(ControlPromptBagPocket(
                        commandAgent,
                        commandTrainer
                        ));
                }
                else if (commandType == BattleCommandType.Run)
                {
                    Command attemptedCommand = Command.CreateRunCommand(
                        commandUser: pokemon.uniqueID, 
                        commandTrainer: commandTrainer.playerID,
                        isExplicitlySelected: true);

                    waitCRActive = true;
                    waitCR = StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) =>
                    {
                        if (success)
                        {
                            playerCommand = attemptedCommand;
                            choosingCommand = false;
                        }
                        waitCRActive = false;
                    }));
                }
            }
        }
        private void CancelCommandMenu(InputAction.CallbackContext obj)
        {
            CancelCommandMenu();
        }
        private void CancelCommandMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            // Only leave if we're able to
            if (commandTypes.Contains(BattleCommandType.Back))
            {
                playerCommand = null;
                choosingCommand = false;
            }
        }



        // FIGHT MENU
        private void NavigateFightMenuQuad(InputAction.CallbackContext obj)
        {
            int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            int scrollY = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);

            if (scrollX != 0 || scrollY != 0)
            {
                bool moveToBack = false;
                int newIndex = moveIndex;

                if (moveIndex < 0)
                {
                    if (scrollY > 0)
                    {
                        if (moveslots.Count >= 4) newIndex = 3;
                        else if (moveslots.Count >= 3) newIndex = 2;
                        else if (moveslots.Count >= 2) newIndex = 1;
                        else newIndex = 0;
                    }
                    else if (scrollY < 0)
                    {
                        if (moveslots.Count >= 2) newIndex = 1;
                        else newIndex = 0;
                    }
                }
                else
                {
                    if (scrollX != 0)
                    {
                        if (moveIndex == 0) newIndex = 1;
                        else if (moveIndex == 1) newIndex = 0;
                        else if (moveIndex == 2) newIndex = 3;
                        else if (moveIndex == 3) newIndex = 2;
                    }
                    else if (scrollY != 0)
                    {
                        if ((scrollY > 0 && (moveIndex == 0 || moveIndex == 1))
                            || (scrollY < 0 && (moveIndex == 2 || moveIndex == 3)))
                        {
                            moveToBack = true;
                        }
                        else
                        {
                            if (moveIndex == 0) newIndex = 2;
                            else if (moveIndex == 1) newIndex = 3;
                            else if (moveIndex == 2) newIndex = 0;
                            else if (moveIndex == 3) newIndex = 1;
                        }
                    }
                }
                
                if (newIndex >= moveslots.Count)
                {
                    moveToBack = true;
                }

                if (moveToBack)
                {
                    battleUI.SwitchSelectedMoveToBack();
                    moveIndex = -1;
                }
                else
                {
                    NavigateFightMenu(newIndex - moveIndex);
                }
            }
        }
        private void NavigateFightMenu(int scrollAmount)
        {
            if (AreControlsLocked())
            {
                return;
            }
            if (scrollAmount != 0)
            {
                moveIndex += scrollAmount;
                moveIndex %= moveslots.Count;
                if (moveIndex < 0)
                {
                    moveIndex += moveslots.Count;
                }
                PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
                battleUI.SwitchSelectedMoveTo(
                    pokemon: pokemon,
                    selected: moveIndex,
                    choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                    choosingZMove: chooseZMove,
                    choosingMaxMove: chooseDynamax);
            }
        }

        private void SelectFightSpecial(InputAction.CallbackContext obj)
        {
            SelectFightSpecial();
        }
        private void SelectFightSpecial()
        {
            if (AreControlsLocked())
            {
                return;
            }

            // select special action
            if (canMegaEvolve)
            {
                chooseMegaEvolve = !chooseMegaEvolve;
            }
            else if (canZMove)
            {
                chooseZMove = !chooseZMove;
            }
            else if (canDynamax)
            {
                chooseDynamax = !chooseDynamax;
            }

            List<PBS.Battle.View.Events.CommandAgent.Moveslot> selectedMoveslots 
                = new List<Battle.View.Events.CommandAgent.Moveslot>();
            if (chooseZMove)
            {
                selectedMoveslots = new List<Battle.View.Events.CommandAgent.Moveslot>(commandAgent.zMoveSlots);
            }
            else if (chooseDynamax)
            {
                selectedMoveslots = new List<Battle.View.Events.CommandAgent.Moveslot>(commandAgent.dynamaxMoveSlots);
            }
            else
            {
                selectedMoveslots = new List<Battle.View.Events.CommandAgent.Moveslot>(commandAgent.moveslots);
            }

            // set the viable moves for this pokemon
            moveslots = new List<PBS.Battle.View.Events.CommandAgent.Moveslot>(selectedMoveslots);
            bool anyUseable = false;
            for (int i = 0; i < moveslots.Count; i++)
            {
                if (moveslots[i].useable)
                {
                    anyUseable = true;
                }
            }
            // if there's no useable moves, show struggle option
            if (!anyUseable)
            {
                moveslots = new List<PBS.Battle.View.Events.CommandAgent.Moveslot> { new PBS.Battle.View.Events.CommandAgent.Moveslot("struggle") };
            }

            PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
            battleUI.SetMoves(
                pokemon: pokemon,
                moveslots: moveslots,
                canMegaEvolve: canMegaEvolve,
                canZMove: canZMove,
                canDynamax: canDynamax,
                choosingZMove: chooseZMove,
                choosingMaxMove: chooseDynamax);

            if (moveIndex >= 0)
            {
                battleUI.SwitchSelectedMoveTo(
                    pokemon: pokemon,
                    selected: moveIndex,
                    choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                    choosingZMove: chooseZMove,
                    choosingMaxMove: chooseDynamax);
            }
        }

        private void SelectFightMenu(InputAction.CallbackContext obj)
        {
            SelectFightMenu();
        }
        private void SelectFightMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }

            // exit to commands
            if (moveIndex < 0)
            {
                CancelFightMenu();
            }
            // create command
            else
            {
                PBS.Battle.View.Events.CommandAgent.Moveslot rawChoice = this.moveslots[moveIndex];
                PBS.Battle.View.Events.CommandAgent.Moveslot choice = this.commandAgent.moveslots[moveIndex];
                bool validMove = true;
                if (chooseZMove)
                {
                    if (rawChoice.hide)
                    {
                        validMove = false;
                        StartCoroutine(battleUI.DrawTextInstant("This move cannot use Z-Power!", undrawOnFinish: true));
                    }
                }

                if (validMove)
                {
                    // if auto-target (ex. Singles), we're done here
                    PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
                    if (battleModel.settings.battleType == BattleType.Single)
                    {
                        Command attemptedCommand = Command.CreateMoveCommand(
                            commandUser: pokemon.uniqueID,
                            commandTrainer: commandTrainer.playerID,
                            moveID: choice.moveID,
                            targetPositions: new List<BattlePosition>(),
                            isExplicitlySelected: true,
                            isMegaEvolving: chooseMegaEvolve,
                            isZMove: chooseZMove,
                            isDynamaxing: chooseDynamax);

                        waitCRActive = true;
                        waitCR = StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) =>
                        {
                            if (success)
                            {
                                playerCommand = attemptedCommand;
                                choosingFight = false;
                                choosingCommand = false;
                            }
                            else
                            {
                                battleUI.SwitchSelectedMoveTo(
                                    pokemon: pokemon,
                                    selected: moveIndex,
                                    choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                                    choosingZMove: chooseZMove,
                                    choosingMaxMove: chooseDynamax);
                            }
                            waitCRActive = false;
                        }));
                    }
                    // we have to specifically choose the target
                    else
                    {
                        controlFightTargetCR = StartCoroutine(ControlPromptFieldTarget(
                            commandAgent,
                            commandTrainer,
                            moveslots[moveIndex],
                            committedCommands));
                    }
                }
            }
        }
        private void CancelFightMenu(InputAction.CallbackContext obj)
        {
            CancelFightMenu();
        }
        private void CancelFightMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            // Only leave if we're able to
            if (!this.forceMove)
            {
                playerCommand = null;
                choosingFight = false;
                chooseMegaEvolve = false;
                chooseZMove = false;
                chooseDynamax = false;

                SwitchControlContext(ControlContext.Command);
                battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Command);
                battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
            }
        }


        // FIELD TARGETING MENU
        private void NavigateFieldTargetMenu(InputAction.CallbackContext obj)
        {
            int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            NavigateFightTargetMenu(addIndex);
        }
        private void NavigateFightTargetMenu(int scrollAmount)
        {
            if (AreControlsLocked())
            {
                return;
            }
            if (scrollAmount != 0)
            {
                moveTargetIndex += scrollAmount;
                moveTargetIndex %= moveTargets.Count;
                if (moveTargetIndex < 0)
                {
                    moveTargetIndex += moveTargets.Count;
                }
                battleUI.SwitchSelectedMoveTargetsTo(battleModel, battleModel.GetPokemonPosition(commandAgent), moveTargetIndex, moveTargets);
            }
        }
        private void SelectFieldTargetMenu(InputAction.CallbackContext obj)
        {
            SelectFightTargetMenu();
        }
        private void SelectFightTargetMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            List<BattlePosition> choice = moveTargets[moveTargetIndex];

            // exit to fight
            if (choice == null)
            {
                CancelFieldTargetMenu();
            }
            // create command
            else
            {
                bool validMove = true;
                PBS.Battle.View.Events.CommandAgent.Moveslot rawChoice = this.moveslots[moveIndex];
                if (chooseZMove)
                {
                    if (rawChoice.hide)
                    {
                        validMove = false;
                        StartCoroutine(battleUI.DrawTextInstant("This move cannot use Z-Power!", undrawOnFinish: true));
                    }
                }

                if (validMove)
                {
                    PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
                    Command attemptedCommand = Command.CreateMoveCommand(
                        commandUser: pokemon.uniqueID,
                        commandTrainer: commandTrainer.playerID,
                        moveID: commandAgent.moveslots[moveIndex].moveID,
                        targetPositions: moveTargets[moveTargetIndex],
                        isExplicitlySelected: true,
                        isMegaEvolving: chooseMegaEvolve,
                        isZMove: chooseZMove,
                        isDynamaxing: chooseDynamax);
                    waitCRActive = true;
                    waitCR = StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) =>
                    {
                        if (success)
                        {
                            playerCommand = attemptedCommand;
                            choosingFightTarget = false;
                            choosingFight = false;
                            choosingCommand = false;
                        }
                        else
                        {
                            battleUI.SwitchSelectedMoveTargetsTo(battleModel, battleModel.GetPokemonPosition(pokemon), moveTargetIndex, moveTargets);
                        }
                        waitCRActive = false;
                    }));
                }
            }
        }
        private void CancelFieldTargetMenu(InputAction.CallbackContext obj)
        {
            CancelFieldTargetMenu();
        }
        private void CancelFieldTargetMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            // Only leave if we're able to
            if (moveTargets.Contains(null))
            {
                playerCommand = null;
                choosingFightTarget = false;

                PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
                SwitchControlContext(ControlContext.Fight);
                battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Fight);
                battleUI.SwitchSelectedMoveTo(
                    pokemon: pokemon,
                    selected: moveIndex,
                    choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                    choosingZMove: chooseZMove,
                    choosingMaxMove: chooseDynamax);
            }
        }


        // PARTY MENU
        private void NavigatePartyMenuQuad(InputAction.CallbackContext obj)
        {
            int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            int scrollY = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);

            if (scrollX != 0 || scrollY != 0)
            {
                bool moveToBack = false;
                int newIndex = partyIndex;

                if (partyIndex < 0)
                {
                    if (scrollY > 0)
                    {
                        if (partySlots.Count >= 6) newIndex = 5;
                        else if (partySlots.Count >= 5) newIndex = 4;
                        else if (partySlots.Count >= 4) newIndex = 3;
                        else if (partySlots.Count >= 3) newIndex = 2;
                        else if (partySlots.Count >= 2) newIndex = 1;
                        else newIndex = 0;
                    }
                    else if (scrollY < 0)
                    {
                        if (partySlots.Count >= 2) newIndex = 1;
                        else newIndex = 0;
                    }
                }
                else
                {
                    if (scrollX != 0)
                    {
                        if (partyIndex == 0) newIndex = 1;
                        else if (partyIndex == 1) newIndex = 0;
                        else if (partyIndex == 2) newIndex = 3;
                        else if (partyIndex == 3) newIndex = 2;
                        else if (partyIndex == 4) newIndex = 5;
                        else if (partyIndex == 5) newIndex = 4;
                    }
                    else if (scrollY != 0)
                    {
                        if ((scrollY > 0 && (partyIndex == 0 || partyIndex == 1))
                            || (scrollY < 0 && (partyIndex == 4 || partyIndex == 5)))
                        {
                            moveToBack = true;
                        }
                        else
                        {
                            if (partyIndex == 0) newIndex = 2;
                            else if (partyIndex == 1) newIndex = 3;
                            else if (partyIndex == 2) newIndex = (scrollY > 0)? 0 : 4;
                            else if (partyIndex == 3) newIndex = (scrollY > 0)? 1 : 5;
                            else if (partyIndex == 4) newIndex = 2;
                            else if (partyIndex == 5) newIndex = 3;
                        }
                    }
                }
                
                if (newIndex >= partySlots.Count)
                {
                    moveToBack = true;
                }

                if (moveToBack)
                {
                    battleUI.SwitchSelectedPartyMemberToBack();
                    partyIndex = -1;
                }
                else
                {
                    NavigatePartyMenu(newIndex - partyIndex);
                }
            }
        }
        private void NavigatePartyMenu(int scrollAmount, bool skipBackButton = false)
        {
            if (AreControlsLocked())
            {
                return;
            }
            if (scrollAmount != 0)
            {
                partyIndex += scrollAmount;
                partyIndex %= partySlots.Count;
                if (partyIndex < 0)
                {
                    partyIndex += partySlots.Count;
                }

                if (skipBackButton && partySlots[partyIndex] == null)
                {
                    partyIndex++;
                    partyIndex %= partySlots.Count;
                }
                battleUI.SwitchSelectedPartyMemberTo(partySlots[partyIndex]);
            }
        }
    
        private void SelectPartyMenu(InputAction.CallbackContext obj)
        {
            SelectPartyMenu();
        }
        private void SelectPartyMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }

            // exit to commands
            if (partyIndex < 0)
            {
                CancelPartyMenu();
            }
            else
            {
                // no selected item
                PBS.Battle.View.Compact.Pokemon choice = partySlots[partyIndex];
                if (selectedItem == null)
                {
                    controlPartyCommandCR = StartCoroutine(ControlPromptCommandExtra(
                    commandAgent,
                    commandTrainer,
                    committedCommands,
                    new List<BattleExtraCommand>
                    {
                        BattleExtraCommand.Summary,
                        BattleExtraCommand.Switch,
                        BattleExtraCommand.Moves,
                        BattleExtraCommand.Cancel
                    }
                    ));
                }
                // try to use the item on the pokemon
                else
                {
                    Command attemptedCommand = Command.CreateBagCommand(
                        itemID: selectedItem.itemID,
                        itemPokemon: choice.uniqueID,
                        trainer: commandTrainer.playerID,
                        isExplicitlySelected: true);

                    waitCRActive = true;
                    waitCR = StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) =>
                    {
                        if (success)
                        {
                            playerCommand = attemptedCommand;
                            choosingParty = false;
                            choosingItem = false;
                            choosingBagPocket = false;
                            choosingCommand = false;
                        }
                        waitCRActive = false;
                    }));
                }
            
            }
        }
        private void CancelPartyMenu(InputAction.CallbackContext obj)
        {
            CancelPartyMenu();
        }
        private void CancelPartyMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            // Only leave if we're able to
            if (!forceSwitch && !forceReplace)
            {
                playerCommand = null;
                choosingParty = false;

                if (selectedItem == null)
                {
                    SwitchControlContext(ControlContext.Command);
                    battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Command);
                    battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
                }
                else
                {
                    selectedItem = null;
                    SwitchControlContext(ControlContext.Bag);
                    battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.BagItem);
                }
            }
        }


        // PARTY COMMAND MENU
        private void NavigatePartyCommandMenu(InputAction.CallbackContext obj)
        {
            int addIndexX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            int addIndexY = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);
            if (addIndexY != 0)
            {
                NavigatePartyCommandMenu(addIndexY);
            }
            else
            {
                if (AreControlsLocked())
                {
                    return;
                }
                NavigatePartyMenu(addIndexX, true);
                battleUI.SwitchSelectedPartyCommandTo(extraCommands[extraCommandIndex]);
            }
        }
        private void NavigatePartyCommandMenu(int scrollAmount)
        {
            if (AreControlsLocked())
            {
                return;
            }
            if (scrollAmount != 0)
            {
                extraCommandIndex -= scrollAmount;
                extraCommandIndex %= extraCommands.Count;
                if (extraCommandIndex < 0)
                {
                    extraCommandIndex += extraCommands.Count;
                }
                battleUI.SwitchSelectedPartyCommandTo(extraCommands[extraCommandIndex]);
            }
        }
        private void SelectPartyCommandMenu(InputAction.CallbackContext obj)
        {
            SelectPartyCommandMenu();
        }
        private void SelectPartyCommandMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            BattleExtraCommand commandType = extraCommands[extraCommandIndex];
            PBS.Battle.View.Compact.Pokemon choice = partySlots[partyIndex];

            // Exit Commands
            if (commandType == BattleExtraCommand.Cancel)
            {
                CancelPartyCommandMenu();
            }
            else
            {
                if (commandType == BattleExtraCommand.Summary)
                {
                    Debug.Log("Party summary unimplemented");
                }
                else if (commandType == BattleExtraCommand.Moves)
                {
                    Debug.Log("Party moves unimplemented");
                }
                else if (commandType == BattleExtraCommand.Switch)
                {
                    // make sure we can actually switch
                    Command attemptedCommand = (forceReplace) ?
                        Command.CreateReplaceCommand(
                            switchPosition: switchPosition,
                            trainer: commandTrainer.playerID,
                            switchInPokemon: choice.uniqueID,
                            isExplicitlySelected: true)
                        :
                        Command.CreateSwitchCommand(
                            commandUser: commandAgent.pokemonUniqueID,
                            switchPosition: switchPosition,
                            trainer: commandTrainer.playerID,
                            switchInPokemon: choice.uniqueID,
                            isExplicitlySelected: true);

                    waitCRActive = true;
                    waitCR = StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) => 
                    {
                        if (success)
                        {
                            playerCommand = attemptedCommand;
                            choosingParty = false;
                            choosingExtraCommand = false;
                            choosingCommand = false;
                        }
                        else
                        {
                            //view.battleUI.SwitchSelectedPartyTo(partySlots[partyIndex]);
                            //view.battleUI.SwitchSelectedPartyCommandTo(extraCommands[extraCommandIndex]);
                            //view.battleUI.SwitchSelectedExtraCommandTo(extraCommands[extraCommandIndex], extraCommands);
                        }
                        waitCRActive = false;
                    }));
                }
            }
        }
        private void CancelPartyCommandMenu(InputAction.CallbackContext obj)
        {
            CancelPartyCommandMenu();
        }
        private void CancelPartyCommandMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            // Only leave if we're able to
            if (extraCommands.Contains(BattleExtraCommand.Cancel))
            {
                choosingExtraCommand = false;

                SwitchControlContext(ControlContext.Party);
                battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Party);
                battleUI.SwitchSelectedPartyMemberTo(partySlots[partyIndex]);
            }
        }


        // BAG POCKET MENU
        private void NavigateBagPocketMenuQuad(InputAction.CallbackContext obj)
        {
            int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            int scrollY = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);

            if (scrollX != 0 || scrollY != 0)
            {
                bool moveToBack = false;
                int newIndex = itemPocketIndex;

                if (itemPocketIndex < 0)
                {
                    if (scrollY > 0)
                    {
                        newIndex = 3;
                    }
                    else if (scrollY < 0)
                    {
                        newIndex = 1;
                    }
                }
                else
                {
                    if (scrollX != 0)
                    {
                        if (itemPocketIndex == 0) newIndex = 1;
                        else if (itemPocketIndex == 1) newIndex = 0;
                        else if (itemPocketIndex == 2) newIndex = 3;
                        else if (itemPocketIndex == 3) newIndex = 2;
                    }
                    else if (scrollY != 0)
                    {
                        if ((scrollY > 0 && (itemPocketIndex == 0 || itemPocketIndex == 1))
                            || (scrollY < 0 && (itemPocketIndex == 2 || itemPocketIndex == 3)))
                        {
                            moveToBack = true;
                        }
                        else
                        {
                            if (itemPocketIndex == 0) newIndex = 2;
                            else if (itemPocketIndex == 1) newIndex = 3;
                            else if (itemPocketIndex == 2) newIndex = 0;
                            else if (itemPocketIndex == 3) newIndex = 1;
                        }
                    }
                }
                
                if (newIndex >= itemPockets.Count)
                {
                    moveToBack = true;
                }

                if (moveToBack)
                {
                    battleUI.SwitchSelectedBagPocketToBack();
                    itemPocketIndex = -1;
                }
                else
                {
                    NavigateBagPocketMenu(newIndex - itemPocketIndex);
                }
            }
        }
        private void NavigateBagPocketMenu(int scrollAmount)
        {
            if (AreControlsLocked())
            {
                return;
            }
            if (scrollAmount != 0)
            {
                itemPocketIndex += scrollAmount;
                itemPocketIndex %= itemPockets.Count;
                if (itemPocketIndex < 0)
                {
                    itemPocketIndex += itemPockets.Count;
                }
                battleUI.SwitchSelectedBagPocketTo(itemPockets[itemPocketIndex]);
            }
        }
    
        private void SelectBagPocketMenu(InputAction.CallbackContext obj)
        {
            SelectBagPocketMenu();
        }
        private void SelectBagPocketMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }

            // Exit Commands
            if (itemPocketIndex < 0)
            {
                CancelBagPocketMenu();
            }
            else
            {
                ItemBattlePocket pocketType = itemPockets[itemPocketIndex];
                controlBagCR = StartCoroutine(ControlPromptBag(
                    commandAgent,
                    commandTrainer,
                    pocketType,
                    committedCommands
                    ));
            }
        }
        private void CancelBagPocketMenu(InputAction.CallbackContext obj)
        {
            CancelBagPocketMenu();
        }
        private void CancelBagPocketMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            // Only leave if we're able to
            if (itemPockets.Contains(ItemBattlePocket.None))
            {
                playerCommand = null;
                choosingBagPocket = false;

                SwitchControlContext(ControlContext.Command);
                battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Command);
                battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
            }
        }



        // BAG MENU
        private void NavigateBagMenuQuad(InputAction.CallbackContext obj)
        {
            int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            int scrollY = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);

            if (scrollX != 0 || scrollY != 0)
            {
                bool moveToBack = false;
                bool scrollLeft = false;
                bool scrollRight = false;
                int newIndex = itemIndex;

                int trueIndex = itemOffset + itemIndex;
                int curCount = 0;
                for (int i = itemOffset; i < itemOffset + 4 && i < itemSlots.Count; i++)
                {
                    curCount++;
                }

                if (itemIndex < 0)
                {
                    if (scrollY > 0)
                    {
                        if (curCount >= 4) newIndex = 3;
                        else if (curCount >= 3) newIndex = 2;
                        else if (curCount >= 2) newIndex = 1;
                        else if (curCount >= 1) newIndex = 0;
                    }
                    else if (scrollY < 0)
                    {
                        if (curCount >= 2) newIndex = 1;
                        else if (curCount >= 1) newIndex = 0;
                    }
                }
                else
                {
                    if (scrollX != 0)
                    {
                        if (scrollX > 0)
                        {
                            if (itemIndex == 0)
                            {
                                newIndex = 1;
                            }
                            else if (itemIndex == 1)
                            {
                                newIndex = 0;
                                scrollRight = true;
                            }
                            else if (itemIndex == 2)
                            {
                                newIndex = 3;
                            }
                            else if (itemIndex == 3)
                            {
                                newIndex = 2;
                                scrollRight = true;
                            }
                        }
                        else
                        {
                            if (itemIndex == 0)
                            {
                                newIndex = 1;
                                scrollLeft = true;
                            }
                            else if (itemIndex == 1)
                            {
                                newIndex = 0;
                            }
                            else if (itemIndex == 2)
                            {
                                newIndex = 3;
                                scrollLeft = true;
                            }
                            else if (itemIndex == 3)
                            {
                                newIndex = 2;
                            }
                        }
                    }
                    else if (scrollY != 0)
                    {
                        if ((scrollY > 0 && (itemIndex == 0 || itemIndex == 1))
                            || (scrollY < 0 && (itemIndex == 2 || itemIndex == 3)))
                        {
                            moveToBack = true;
                        }
                        else
                        {
                            if (itemIndex == 0) newIndex = 2;
                            else if (itemIndex == 1) newIndex = 3;
                            else if (itemIndex == 2) newIndex = 0;
                            else if (itemIndex == 3) newIndex = 1;
                        }
                    }
                }
                
                if (newIndex >= curCount && scrollY != 0)
                {
                    moveToBack = true;
                }

                if (moveToBack)
                {
                    battleUI.SwitchSelectedItemToBack();
                    itemIndex = -1;
                }
                else
                {
                    int nextOffset = itemOffset;
                    int nextCount = curCount;

                    int lastOffset = (itemSlots.Count / 4) * 4;
                    int lastOffsetCount = Mathf.Min(4, itemSlots.Count - lastOffset);

                    if (scrollLeft)
                    {
                        nextOffset = itemOffset - 4;
                        if (nextOffset < 0)
                        {
                            nextOffset = lastOffset;
                        }
                        nextCount = Mathf.Min(4, itemSlots.Count - nextOffset);

                        if (nextOffset + newIndex >= nextOffset + nextCount)
                        {
                            if (nextOffset + newIndex >= nextOffset + nextCount)
                            {
                                newIndex = 3;
                            }
                            if (nextOffset + newIndex >= nextOffset + 3)
                            {
                                newIndex = 2;
                            }
                            if (nextOffset + newIndex >= nextOffset + 2)
                            {
                                newIndex = 1;
                            }
                            if (nextOffset + newIndex >= nextOffset + 1)
                            {
                                newIndex = 0;
                            }
                        }
                    }
                    else if (scrollRight)
                    {
                        nextOffset = itemOffset + 4;
                        if (nextOffset > lastOffset)
                        {
                            nextOffset = 0;
                        }
                        nextCount = Math.Min(4, itemSlots.Count - nextOffset);

                        if (nextOffset + newIndex >= nextOffset + nextCount)
                        {
                            if (itemIndex == 3)
                            {
                                newIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        if (newIndex >= nextCount && scrollX > 0)
                        {
                            if (itemIndex == 2 && nextCount < 2)
                            {
                                newIndex = 0;
                            }
                            else
                            {
                                newIndex = itemIndex;
                            }
                        } 
                    }
                    
                    itemOffset = nextOffset;
                    NavigateBagMenu(newIndex: newIndex, changePage: scrollLeft || scrollRight);
                }
            }
        }
        private void NavigateBagMenu(int newIndex, bool changePage = false)
        {
            if (AreControlsLocked())
            {
                return;
            }
            if (itemIndex != newIndex || changePage)
            {
                itemIndex = newIndex;
                if (changePage)
                {
                    battleUI.SetItems(
                        trainer: commandTrainer,
                        pocket: itemPockets[itemPocketIndex],
                        list: itemSlots,
                        offset: itemOffset
                        );
                }
                battleUI.SwitchSelectedItemTo(itemSlots[itemOffset + itemIndex]);
            }
        }
        private void SelectBagMenu(InputAction.CallbackContext obj)
        {
            SelectBagMenu();
        }
        private void SelectBagMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }

            // exit to bag pockets
            if (itemIndex < 0)
            {
                CancelBagMenu();
            }
            // choose the pokemon to use the item on
            else
            {
                selectedItem = itemSlots[itemOffset + itemIndex];
                controlPartyCR = StartCoroutine(ControlPromptParty(
                    commandAgent,
                    commandTrainer,
                    committedCommands
                    ));
            }
        }
        private void CancelBagMenu(InputAction.CallbackContext obj)
        {
            CancelBagMenu();
        }
        private void CancelBagMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            playerCommand = null;
            choosingItem = false;

            SwitchControlContext(ControlContext.BagPocket);
            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Bag);
            battleUI.SwitchSelectedBagPocketTo(itemPockets[itemPocketIndex]);
        }

        private IEnumerator AttemptCommand(
            Command attemptedCommand, 
            Command[] setCommands,
            Action<bool> callback)
        {
            bool commandSuccess = true;

            List<Command> previousCommands = new List<Command>();
            for (int i = 0; i < setCommands.Length; i++)
            {
                if (setCommands[i] != null)
                {
                    previousCommands.Add(setCommands[i]);
                }
            }

            yield return StartCoroutine(player.RunQueryResponsePollingSystem(attemptedCommand, previousCommands));
            if (player.queryResponse != null)
            {
                commandSuccess = player.queryResponse.isQuerySuccessful;
                if (!player.queryResponse.isQuerySuccessful)
                {
                    yield return StartCoroutine(battleUI.DrawText(
                        PBS.Battle.View.UI.Canvas.RenderMessage(
                            message: player.queryResponse,
                            myModel: battleModel,
                            myPlayerID: player.playerID,
                            myTrainer: player.myTrainer,
                            myTeamPerspective: player.myTeamPerspective)
                        ));
                }
            }

            callback(commandSuccess);
        }
    }
}

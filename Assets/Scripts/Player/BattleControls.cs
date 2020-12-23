using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PBS.Player
{
    /// <summary>
    /// This class handles everything related to player input during battle. This includes navigating the different
    /// menus and selecting battle commands.
    /// </summary>
    public class BattleControls : MonoBehaviour
    {
        #region Attributes
        #region Battle Model
        /// <summary>
        /// This is the current battle state. The controls need to know the current state
        /// to determine what commands are legal or illegal for the player.
        /// </summary>
        public PBS.Battle.View.Model battleModel;
        /// <summary>
        /// A reference to the player object. Selected commands are sent through the player object to be executed
        /// in the battle.
        /// </summary>
        public PBS.Networking.Player player;
        #endregion

        #region Visual Components
        /// <summary>
        /// A reference to the UI that is shown to the player.
        /// </summary>
        public PBS.Battle.View.UI.Canvas battleUI;
        
        /// <summary>
        /// A reference to the visual representation of the battle, shown to the player.
        /// </summary>
        public Battle.View.Scene.Canvas sceneCanvas;
        #endregion

        #region Controller
        /// <summary>
        /// The control context determines what menu the player is currently interacting with.
        /// </summary>
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
        
        /// <summary>
        /// The underlying literal controls that are accessible.
        /// </summary>
        private Controls _controls;
        
        /// <summary>
        /// The current control context. See <seealso cref="ControlContext"/>.
        /// </summary>
        private ControlContext context;
        
        /// <summary>
        /// While true, the player's input is ignored.
        /// </summary>
        private bool lockControls = false;

        /// <summary>
        /// This coroutine
        /// </summary>
        private Coroutine controlCommandCR;
        /// <summary>
        /// A flag to indicate that a Coroutine (such as dialog) is currently running, so temporarily
        /// ignore the player's input.
        /// </summary>
        private bool waitCRActive = false;
        #endregion

        #region Commands
        /// <summary>
        /// The current command prompt for the player. The command prompt determines which pokemon the player
        /// can actually select commands for.
        /// </summary>
        Battle.View.Events.CommandGeneralPrompt commandPromptEvent;
        
        /// <summary>
        /// The pokemon that is currently having its command selected.
        /// </summary>
        private PBS.Battle.View.Events.CommandAgent commandAgent;

        /// <summary>
        /// The trainer in-battle that the player is currently selecting commands for.
        /// </summary>
        private PBS.Battle.View.WifiFriendly.Trainer commandTrainer;
        
        /// <summary>
        /// The command that is currently being constructed to be added to <seealso cref="committedCommands"/>
        /// </summary>
        private Command playerCommand;
        /// <summary>
        /// The list of commands the player has recently committed to. Once all commands are committed, this is
        /// sent to <seealso cref="player"/>.
        /// </summary>
        private Command[] committedCommands;
        

        #region General Commands
        /// <summary>
        /// A flag to indicate that the player is currently choosing a command.
        /// </summary>
        private bool choosingCommand;
        
        /// <summary>
        /// The list of possible commands available for the <seealso cref="commandAgent"/>.
        /// </summary>
        private List<BattleCommandType> commandTypes;

        /// <summary>
        /// The currently highlighted command in <seealso cref="commandTypes"/>. If -1, the
        /// player is on the back button.
        /// </summary>
        private int commandIndex = 0;
        #endregion

        #region Custom Commands
        /// <summary>
        /// A flag to indicate that the player is currently on a custom command menu.
        /// </summary>
        private bool choosingExtraCommand;
        /// <summary>
        /// The list of possible custom commands available.
        /// </summary>
        private List<BattleExtraCommand> extraCommands;
        /// <summary>
        /// The currently highlighted custom command in <seealso cref="extraCommands"/>.
        /// </summary>
        private int extraCommandIndex = 0;
        #endregion

        #region Fight
        /// <summary>
        /// A flag to indicate that the player is currently selecting a move.
        /// </summary>
        private bool choosingFight;
        
        /// <summary>
        /// If true, the player is forced to select a move, and cannot back out into other commands.
        /// </summary>
        private bool forceMove;
        
        /// <summary>
        /// The moves available for selection for <seealso cref="commandAgent"/>.
        /// </summary>
        private List<Battle.View.Events.CommandAgent.Moveslot> moveslots;

        /// <summary>
        /// The currently highlighted move in <seealso cref="moveslots"/>.
        /// </summary>
        private int moveIndex;

        /// <summary>
        /// If true, the <seealso cref="commandAgent"/> has the option to mega-evolve this turn.
        /// </summary>
        private bool canMegaEvolve;
        /// <summary>
        /// If true, the <seealso cref="commandAgent"/> has selected to mega-evolve this turn.
        /// </summary>
        private bool chooseMegaEvolve;

        /// <summary>
        /// If true, the <seealso cref="commandAgent"/> has the option to use a Z-Move this turn.
        /// </summary>
        private bool canZMove;
        /// <summary>
        /// If true, the <seealso cref="commandAgent"/> has selected to use a Z-Move this turn.
        /// </summary>
        private bool chooseZMove;

        /// <summary>
        /// If true, the <seealso cref="commandAgent"/> has the option to Dynamax this turn.
        /// </summary>
        private bool canDynamax;
        /// <summary>
        /// If true, the <seealso cref="commandAgent"/> has selected to Dynamax this turn.
        /// </summary>
        private bool chooseDynamax;

        /// <summary>
        /// If true, <seealso cref="commandAgent"/> has the option to undergo a custom form-change this turn.
        /// ex: Ultra-Burst
        /// </summary>
        private bool canFormChange;
        #endregion

        #region Fight Target
        /// <summary>
        /// A flag to indicate that the player is currently choosing targets for <seealso cref="selectedMoveslot"/>.
        /// </summary>
        private bool choosingFightTarget;
        /// <summary>
        /// The selected move for which the player is choosing targets.
        /// </summary>
        private Battle.View.Events.CommandAgent.Moveslot selectedMoveslot;
        /// <summary>
        /// The possible groups of targets for <seealso cref="selectedMoveslot"/>.
        /// </summary>
        private List<List<BattlePosition>> moveTargets;
        /// <summary>
        /// The currently highlight move targets in <seealso cref="moveTargets"/>.
        /// </summary>
        private int moveTargetIndex;
        #endregion

        #region Party
        /// <summary>
        /// A flag to indicate that the player is currently in the party menu.
        /// </summary>
        private bool choosingParty;
        /// <summary>
        /// If true, the player is forced to switch to a party member (ex. after Baton Pass).
        /// </summary>
        private bool forceSwitch;
        /// <summary>
        /// If true, the player is forced to send in a party member (ex. after a pokemon faints).
        /// </summary>
        private bool forceReplace;
        /// <summary>
        /// The list of party members for the player to select.
        /// </summary>
        private List<Battle.View.WifiFriendly.Pokemon> partySlots;
        /// <summary>
        /// The currently highlighted party member in <seealso cref="partySlots"/>.
        /// </summary>
        private int partyIndex;
        /// <summary>
        /// The in-battle position of a pokemon meaning to switch out.
        /// </summary>
        private int switchPosition;
        #endregion

        #region Bag Pockets
        /// <summary>
        /// A flag to indicate that the player is currently selecting a bag pocket/category.
        /// </summary>
        private bool choosingBagPocket;
        /// <summary>
        /// The possible bag pockets the player has to choose from.
        /// </summary>
        private List<ItemBattlePocket> itemPockets;
        /// <summary>
        /// The currently highlighted pocket in <seealso cref="itemPockets"/>.
        /// </summary>
        private int itemPocketIndex;
        #endregion
        
        #region Items
        /// <summary>
        /// A flag to indicate that the player is currently selecting an item in a bag pocket.
        /// </summary>
        private bool choosingItem;
        /// <summary>
        /// The list of items to select from.
        /// </summary>
        private List<Item> itemSlots;
        /// <summary>
        /// The offset index to display items from <seealso cref="itemSlots"/>.
        /// </summary>
        private int itemOffset;
        /// <summary>
        /// The currently highlight item in <seealso cref="itemSlots"/>.
        /// </summary>
        private int itemIndex;
        /// <summary>
        /// The currently selected item to be used (ex. on a party member).
        /// </summary>
        private Item selectedItem;
        #endregion

        #endregion
        #endregion

        private void Awake()
        {
            _controls = new Controls();
            SwitchControlContext(ControlContext.None);
        }

        #region Control Defaults
        private void OnEnable()
        {
            _controls.Enable();
        }
        private void OnDisable()
        {
            _controls.Disable();
        }

        /// <summary>
        /// Allows for scrolling dialog to instantly fill when "Select" is pressed.
        /// </summary>
        public void EnableQuickAdvanceDialog()
        {
            _controls.BattleDialog.Select.performed += (obj) =>
            {
                battleUI.dialog.advancedDialogPressed = true;
            };
        }
        /// <summary>
        /// A check that determines if the player's input is currently locked, meaning it is ignored.
        /// </summary>
        /// <returns></returns>
        public bool AreControlsLocked()
        {
            return lockControls || waitCRActive;
        }
        /// <summary>
        /// Locks the player's controls for a moment, allowing menus to animate before the player can select again.
        /// </summary>
        /// <param name="waitTime">The amount of time in seconds to wait.</param>
        /// <returns></returns>
        public IEnumerator DelayControls(float waitTime = 0.02f)
        {
            lockControls = true;
            yield return new WaitForSeconds(waitTime);
            lockControls = false;
        }
        /// <summary>
        /// Switches the current control context to one of <seealso cref="ControlContext"/>.
        /// </summary>
        /// <param name="newContext">The context to switch to.</param>
        /// <param name="delayControls">If true, the player's input is locked for a moment.</param>
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
                    _controls.BattleMenuBagItem.Cancel.performed -= CancelItemMenu;
                    _controls.BattleMenuBagItem.Select.performed -= SelectItemMenu;
                    _controls.BattleMenuBagItem.Move.performed -= NavigateItemMenuQuad;
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
                    _controls.BattleMenuBagItem.Cancel.performed += CancelItemMenu;
                    _controls.BattleMenuBagItem.Select.performed += SelectItemMenu;
                    _controls.BattleMenuBagItem.Move.performed += NavigateItemMenuQuad;
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// Sets the default command variables when a pokemon is prompted.
        /// </summary>
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

            partySlots = new List<PBS.Battle.View.WifiFriendly.Pokemon>();
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
        #endregion

        #region Command Prompts

        /// <summary>
        /// Gives the player control to select commands according to the prompt event. Callbacks the selected commands.
        /// </summary>
        /// <param name="bEvent">Determines how command selection will be executed.</param>
        /// <param name="callback">The selected commands from this prompt.</param>
        /// <returns></returns>
        public IEnumerator HandlePromptCommands(
            Battle.View.Events.CommandGeneralPrompt bEvent,
            Action<Command[]> callback)
        {
            commandPromptEvent = bEvent;
            Battle.View.WifiFriendly.Trainer trainer = battleModel.GetMatchingTrainer(bEvent.playerID);
            committedCommands = new Command[bEvent.pokemonToCommand.Count];

            List<PBS.Battle.View.Events.CommandAgent> pokemonToControl 
                = new List<PBS.Battle.View.Events.CommandAgent>(bEvent.pokemonToCommand);

            battleUI.SetPokemonHUDsActive(true);
            for (int i = 0; i < pokemonToControl.Count; i++)
            {
                battleUI.UndrawDialogBox();

                playerCommand = null;
                committedCommands[i] = null;
                PBS.Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(pokemonToControl[i].pokemonUniqueID);
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
                battleUI.HidePanels();
            }
            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.None);
            this.commandPromptEvent = null;
            callback(committedCommands);
        }

        /// <summary>
        /// Gives the player control to replace on-field pokemon according to the prompt event. Callbacks the selected
        /// replacement commands.
        /// </summary>
        /// <param name="bEvent">Determines how replacement will be executed.</param>
        /// <param name="callback">The selected commands from this prompt.</param>
        /// <returns></returns>
        public IEnumerator HandlePromptReplace(
            Battle.View.Events.CommandReplacementPrompt bEvent,
            Action<Command[]> callback)
        {
            PBS.Battle.View.WifiFriendly.Trainer trainer = battleModel.GetMatchingTrainer(bEvent.playerID);
            int[] fillPositions = bEvent.fillPositions;
            committedCommands = new Command[bEvent.fillPositions.Length];

            battleUI.SetPokemonHUDsActive(true);
            for (int i = 0; i < fillPositions.Length; i++)
            {
                playerCommand = null;
                committedCommands[i] = null;

                // set Default Values
                partySlots = new List<PBS.Battle.View.WifiFriendly.Pokemon>();
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

        /// <summary>
        /// Gives the player control to select a command category.
        /// </summary>
        /// <param name="agent">The pokemon to select the category for.</param>
        /// <param name="trainer">The in-game trainer that the player is controlling.</param>
        /// <param name="pokemonIndex">The amount of previous pokemon who the player already selected commands for.
        /// </param>
        /// <param name="setCommands">The commands already committed to by the player.</param>
        /// <returns></returns>
        private IEnumerator ControlPromptCommand(
            Battle.View.Events.CommandAgent agent, 
            Battle.View.WifiFriendly.Trainer trainer, 
            int pokemonIndex, 
            Command[] setCommands)
        {
            Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(agent);
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

        /// <summary>
        /// Gives the player control to select a custom command category.
        /// </summary>
        /// <param name="agent">The pokemon to select the category for.</param>
        /// <param name="trainer">The in-game trainer that the player is controlling.</param>
        /// <param name="setCommands">The commands already committed to by the player.</param>
        /// <param name="commandList">The custom commands availble to select from.</param>
        /// <returns></returns>
        private IEnumerator ControlPromptCommandExtra(
            Battle.View.Events.CommandAgent agent, 
            Battle.View.WifiFriendly.Trainer trainer,
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

        /// <summary>
        /// Gives the player control to select a Fight command.
        /// </summary>
        /// <param name="agent">The pokemon to select the category for.</param>
        /// <param name="trainer">The in-game trainer that the player is controlling.</param>
        /// <param name="setCommands">The commands already committed to by the player.</param>
        /// <param name="forceMove">Sets <seealso cref="forceMove"/> to this value.</param>
        /// <returns></returns>
        public IEnumerator ControlPromptFight(
            Battle.View.Events.CommandAgent agent, 
            Battle.View.WifiFriendly.Trainer trainer,
            Command[] setCommands,
            bool forceMove = false)
        {
            Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(agent);
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

        /// <summary>
        /// Gives the player control to select move targets.
        /// </summary>
        /// <param name="agent">The pokemon to select the category for.</param>
        /// <param name="trainer">The in-game trainer that the player is controlling.</param>
        /// <param name="selectedMoveslot">Sets <seealso cref="selectedMoveslot"/> to this value.</param>
        /// <param name="setCommands">The commands already committed to by the player.</param>
        /// <returns></returns>
        public IEnumerator ControlPromptFieldTarget(
            Battle.View.Events.CommandAgent agent,
            Battle.View.WifiFriendly.Trainer trainer,
            Battle.View.Events.CommandAgent.Moveslot selectedMoveslot,
            Command[] setCommands = null
            )
        {
            PBS.Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(agent);
            commandAgent = agent;
            commandTrainer = trainer;
            this.selectedMoveslot = selectedMoveslot;
            committedCommands = (setCommands == null) ? new Command[0] : setCommands;

            // get the possible targets for the given move
            yield return StartCoroutine(player.RunQueryResponsePollingSystem(
                new Query.MoveTarget
                { 
                    pokemonUniqueID = commandAgent.pokemonUniqueID,
                    moveID = selectedMoveslot.moveID
                }));
            Query.MoveTargetResponse response = player.queryResponse as Query.MoveTargetResponse;
            moveTargets = new List<List<BattlePosition>>(response.possibleTargets);
            moveTargets.Insert(0, null);

            // set the initial move index
            if (moveTargetIndex > moveTargets.Count)
            {
                moveTargetIndex = 0;
            }
            // do not default to "Back" command if possible
            if (moveTargets[moveTargetIndex] == null && moveTargets.Count > 1)
            {
                moveTargetIndex = 1;
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

        /// <summary>
        /// Gives the player control to select in the given trainer's party.
        /// </summary>
        /// <param name="agent">The pokemon to select the category for.</param>
        /// <param name="trainer">The in-game trainer that the player is controlling.</param>
        /// <param name="setCommands">The commands already committed to by the player.</param>
        /// <param name="forceSwitch">Sets <seealso cref="forceSwitch"/> to this value.</param>
        /// <returns></returns>
        public IEnumerator ControlPromptParty(
            Battle.View.Events.CommandAgent agent,
            Battle.View.WifiFriendly.Trainer trainer,
            Command[] setCommands = null,
            bool forceSwitch = false
            )
        {
            commandAgent = agent;
            commandTrainer = trainer;
            committedCommands = (setCommands == null) ? new Command[0] : setCommands;
            this.forceSwitch = forceSwitch;

            // set the possible party pokemon that can switch in
            partySlots = new List<PBS.Battle.View.WifiFriendly.Pokemon>(commandTrainer.party);

            // set the initial party index
            partyIndex = 0;

            choosingParty = true;
            SwitchControlContext(ControlContext.Party);

            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Party);
            battleUI.SetParty(partySlots, forceSwitch);
            battleUI.SwitchSelectedPartyMemberTo(partySlots[partyIndex]);

            while (choosingParty)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Gives the player control to select within the given trainer's bag pockets.
        /// </summary>
        /// <param name="agent">The pokemon to select the category for.</param>
        /// <param name="trainer">The in-game trainer that the player is controlling.</param>
        /// <returns></returns>
        public IEnumerator ControlPromptBagPocket(
            Battle.View.Events.CommandAgent agent,
            Battle.View.WifiFriendly.Trainer trainer
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

        /// <summary>
        /// Gives the player control to select items from the given trainer's bag in the given pocket.
        /// </summary>
        /// <param name="agent">The pokemon to select the category for.</param>
        /// <param name="trainer">The in-game trainer that the player is controlling.</param>
        /// <param name="pocket">The selected pocket to select items from.</param>
        /// <param name="setCommands">The commands already committed to by the player.</param>
        /// <returns></returns>
        public IEnumerator ControlPromptBag(
            PBS.Battle.View.Events.CommandAgent agent,
            PBS.Battle.View.WifiFriendly.Trainer trainer,
            ItemBattlePocket pocket,
            Command[] setCommands = null
            )
        {
            PBS.Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(agent);
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
        #endregion

        #region Menu Navigation

        #region Command Menu
        /// <summary>
        /// Handles horizontal navigation of the command menu.
        /// </summary>
        /// <param name="obj"></param>
        private void NavigateCommandMenuHorizontal(InputAction.CallbackContext obj)
        {
            int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            NavigateCommandMenu(addIndex);
        }
        /// <summary>
        /// Handles navigation of the command menu in general.
        /// </summary>
        /// <param name="scrollAmount">The amount by which to shift <seealso cref="commandIndex"/>.</param>
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
        /// <summary>
        /// Handles command selection triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectCommandMenu(InputAction.CallbackContext obj)
        {
            SelectCommandMenu();
        }
        /// <summary>
        /// Handles command selection in general.
        /// </summary>
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
                Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
                if (commandType == BattleCommandType.Fight)
                {
                    StartCoroutine(ControlPromptFight(
                        commandAgent,
                        commandTrainer,
                        committedCommands,
                        forceMove
                        ));
                }
                else if (commandType == BattleCommandType.Party)
                {
                    StartCoroutine(ControlPromptParty(
                        commandAgent,
                        commandTrainer,
                        committedCommands,
                        forceSwitch
                        ));
                }
                else if (commandType == BattleCommandType.Bag)
                {
                    StartCoroutine(ControlPromptBagPocket(
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
                    StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) =>
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
        /// <summary>
        /// Handles command cancellation triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void CancelCommandMenu(InputAction.CallbackContext obj)
        {
            CancelCommandMenu();
        }
        /// <summary>
        /// Handles command selection in general.
        /// </summary>
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
        #endregion

        #region Fight Menu
        /// <summary>
        /// Handles navigation of the fight menu triggered by 4-directional InputActions.
        /// </summary>
        /// <param name="obj"></param>
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
        /// <summary>
        /// Handles navigation of the fight menu in general.
        /// </summary>
        /// <param name="scrollAmount">The amount by which to shift <seealso cref="moveIndex"/>.</param>
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
                PBS.Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
                battleUI.SwitchSelectedMoveTo(
                    pokemon: pokemon,
                    selected: moveIndex,
                    choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                    choosingZMove: chooseZMove,
                    choosingMaxMove: chooseDynamax);
            }
        }
        /// <summary>
        /// Handles selection of special fight options (ex. Mega Evolution, Z-Moves) triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectFightSpecial(InputAction.CallbackContext obj)
        {
            SelectFightSpecial();
        }
        /// <summary>
        /// Handles selection of special fight options (ex. Mega Evolution, Z-Moves) in general.
        /// </summary>
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

            PBS.Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
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
        /// <summary>
        /// Handles move selection triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectFightMenu(InputAction.CallbackContext obj)
        {
            SelectFightMenu();
        }
        /// <summary>
        /// Handles move selection in general.
        /// </summary>
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
                    PBS.Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
                    if (!commandPromptEvent.multiTargetSelection)
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
                        StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) =>
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
                        StartCoroutine(ControlPromptFieldTarget(
                            commandAgent,
                            commandTrainer,
                            moveslots[moveIndex],
                            committedCommands));
                    }
                }
            }
        }
        /// <summary>
        /// Handles move cancellation triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void CancelFightMenu(InputAction.CallbackContext obj)
        {
            CancelFightMenu();
        }
        /// <summary>
        /// Handles move cancellation in general.
        /// </summary>
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
        #endregion

        #region Field Targeting Menu
        /// <summary>
        /// Handles navigation of the field targeting menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void NavigateFieldTargetMenu(InputAction.CallbackContext obj)
        {
            int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
            NavigateFightTargetMenu(addIndex);
        }
        /// <summary>
        /// Handles navigation of the field targeting menu in general.
        /// </summary>
        /// <param name="scrollAmount">The amount by which to shift <seealso cref="moveTargetIndex"/> by.</param>
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
        /// <summary>
        /// Handles selection in the field targeting menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectFieldTargetMenu(InputAction.CallbackContext obj)
        {
            SelectFightTargetMenu();
        }
        /// <summary>
        /// Handles selection in the field targeting menu in general.
        /// </summary>
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
                    PBS.Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
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
                    StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) =>
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
        /// <summary>
        /// Handles target cancellation triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void CancelFieldTargetMenu(InputAction.CallbackContext obj)
        {
            CancelFieldTargetMenu();
        }
        /// <summary>
        /// Handles target cancellation in general.
        /// </summary>
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

                PBS.Battle.View.WifiFriendly.Pokemon pokemon = battleModel.GetMatchingPokemon(commandAgent.pokemonUniqueID);
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
        #endregion

        #region Party Menu
        /// <summary>
        /// Handles 4-directional navigation in the party menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
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
        /// <summary>
        /// Handles navigation in the party menu in general.
        /// </summary>
        /// <param name="scrollAmount">The amount by which to shift <seealso cref="partyIndex"/> by.</param>
        /// <param name="skipBackButton">If true, the back button is skipped over.</param>
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
        /// <summary>
        /// Handles selection in the party menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectPartyMenu(InputAction.CallbackContext obj)
        {
            SelectPartyMenu();
        }
        /// <summary>
        /// Handles selection in the party menu in general.
        /// </summary>
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
                PBS.Battle.View.WifiFriendly.Pokemon choice = partySlots[partyIndex];
                if (selectedItem == null)
                {
                    StartCoroutine(ControlPromptCommandExtra(
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
                    StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) =>
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
        /// <summary>
        /// Handles party menu cancellation triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void CancelPartyMenu(InputAction.CallbackContext obj)
        {
            CancelPartyMenu();
        }
        /// <summary>
        /// Handles party menu cancellation in general.
        /// </summary>
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
        /// <summary>
        /// Handles navigation in the party command menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
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
        /// <summary>
        /// Handles navigation in the party command menu in general.
        /// </summary>
        /// <param name="scrollAmount"></param>
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
        /// <summary>
        /// Handles selection in the party command menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectPartyCommandMenu(InputAction.CallbackContext obj)
        {
            SelectPartyCommandMenu();
        }
        /// <summary>
        /// Handles selection in the party command menu in general.
        /// </summary>
        private void SelectPartyCommandMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            BattleExtraCommand commandType = extraCommands[extraCommandIndex];
            PBS.Battle.View.WifiFriendly.Pokemon choice = partySlots[partyIndex];

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
                    StartCoroutine(AttemptCommand(attemptedCommand, committedCommands, (success) => 
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
        /// <summary>
        /// Handles party command cancellation triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void CancelPartyCommandMenu(InputAction.CallbackContext obj)
        {
            CancelPartyCommandMenu();
        }
        /// <summary>
        /// Handles party command cancellation in general.
        /// </summary>
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
        #endregion

        #region Bag Pocket Menu
        /// <summary>
        /// Handles 4-directional navigation in the bag pocket menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
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
        /// <summary>
        /// Handles navigation in the bag pocket menu in general.
        /// </summary>
        /// <param name="scrollAmount"></param>
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
        /// <summary>
        /// Handles selection in the bag pocket menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectBagPocketMenu(InputAction.CallbackContext obj)
        {
            SelectBagPocketMenu();
        }
        /// <summary>
        /// Handles selection in the bag pocket menu in general.
        /// </summary>
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
                StartCoroutine(ControlPromptBag(
                    commandAgent,
                    commandTrainer,
                    pocketType,
                    committedCommands
                    ));
            }
        }
        /// <summary>
        /// Handles bag pocket cancellation triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void CancelBagPocketMenu(InputAction.CallbackContext obj)
        {
            CancelBagPocketMenu();
        }
        /// <summary>
        /// Handles bag pocket cancellation in general.
        /// </summary>
        private void CancelBagPocketMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }
            playerCommand = null;
            choosingBagPocket = false;

            SwitchControlContext(ControlContext.Command);
            battleUI.SwitchPanel(PBS.Battle.View.Enums.Panel.Command);
            battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
        }
        #endregion

        #region Item Menu
        /// <summary>
        /// Handles 4-directional navigation in the item menu triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void NavigateItemMenuQuad(InputAction.CallbackContext obj)
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
                    NavigateItemMenu(newIndex: newIndex, changePage: scrollLeft || scrollRight);
                }
            }
        }
        /// <summary>
        /// Handles navigation in the item menu in general.
        /// </summary>
        /// <param name="newIndex">The new item position to highlight.</param>
        /// <param name="changePage">If true, the item page will be different from the previous page.</param>
        private void NavigateItemMenu(int newIndex, bool changePage = false)
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
                        items: itemSlots,
                        offset: itemOffset
                        );
                }
                battleUI.SwitchSelectedItemTo(itemSlots[itemOffset + itemIndex]);
            }
        }
        /// <summary>
        /// Handles item selection triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void SelectItemMenu(InputAction.CallbackContext obj)
        {
            SelectItemMenu();
        }
        /// <summary>
        /// Handles item selection in general.
        /// </summary>
        private void SelectItemMenu()
        {
            if (AreControlsLocked())
            {
                return;
            }

            // exit to bag pockets
            if (itemIndex < 0)
            {
                CancelItemMenu();
            }
            // choose the pokemon to use the item on
            else
            {
                selectedItem = itemSlots[itemOffset + itemIndex];
                StartCoroutine(ControlPromptParty(
                    commandAgent,
                    commandTrainer,
                    committedCommands
                    ));
            }
        }
        /// <summary>
        /// Handles item cancellation triggered by InputActions.
        /// </summary>
        /// <param name="obj"></param>
        private void CancelItemMenu(InputAction.CallbackContext obj)
        {
            CancelItemMenu();
        }
        /// <summary>
        /// Handles item cancellation in general.
        /// </summary>
        private void CancelItemMenu()
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
        #endregion

        #endregion

        #region Command Validation
        /// <summary>
        /// Verifies that the selected command can be selected.
        /// </summary>
        /// <param name="attemptedCommand">The command to be checked for validity.</param>
        /// <param name="setCommands">The already committed commands by the player.</param>
        /// <param name="callback">The validity of the command.</param>
        /// <returns></returns>
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

            yield return StartCoroutine(player.RunCommandQueryResponsePollingSystem(attemptedCommand, previousCommands));
            if (player.queryMessageResponse != null)
            {
                commandSuccess = player.queryMessageResponse.isQuerySuccessful;
                if (!player.queryMessageResponse.isQuerySuccessful)
                {
                    yield return StartCoroutine(battleUI.DrawText(
                        PBS.Battle.View.UI.Canvas.RenderMessage(
                            message: player.queryMessageResponse,
                            myModel: battleModel,
                            myPlayerID: player.playerID,
                            myTrainer: player.myTrainer,
                            myTeamPerspective: player.myTeamPerspective)
                        ));
                }
            }

            callback(commandSuccess);
        }
        #endregion
    }
}

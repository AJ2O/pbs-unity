using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BTLPlayerControl : MonoBehaviour
{
    [Header("View Components")]
    public BTLView view;
    private Battle battleModel;

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
    private Pokemon commandPokemon;
    private Trainer commandTrainer;
    private BattleCommand playerCommand;
    private BattleCommand[] committedCommands;
    
    [HideInInspector] public bool isSendingCommands = false;
    [HideInInspector] public bool isSendingReplacements = false;
    [HideInInspector] public BattleCommand[] commandsToBeSent;

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
    private List<Pokemon.Moveslot> moveslots;
    private int moveIndex;
    private bool canMegaEvolve, chooseMegaEvolve;
    private bool canZMove, chooseZMove;
    private bool canDynamax, chooseDynamax;
    private bool canFormChange, chooseFormChange; // Ultra-Burst

    // Fight Target
    private bool choosingFightTarget;
    private Pokemon.Moveslot selectedMoveslot;
    private List<List<BattlePosition>> moveTargets;
    private int moveTargetIndex;

    // Party
    private bool choosingParty;
    private bool forceSwitch;
    private bool forceReplace;
    private List<Pokemon> partySlots;
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
    

    // Event Runner
    private List<BTLEvent> eventQueue;
    private Coroutine eventRunner;
    private Coroutine executerCoroutine;
    private bool isExecutingEvents;
    [HideInInspector] public bool isFinishedEvents;

    private void Awake()
    {
        _controls = new Controls();
        //Legacy_SwitchControlContext(ControlContext.None);
        SwitchControlContext(ControlContext.None);
        SetControls();
        SetDebugControls();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }
    private void OnDisable()
    {
        _controls.Disable();
    }

    // General Controls
    public void SetControls()
    {
        _controls.BattleDialog.Select.performed += (obj) =>
        {
            view.battleUI.dialog.advancedDialogPressed = true;
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

    // Model
    private void UpdateModel(Battle battleModel)
    {
        this.battleModel = battleModel;
        view.UpdateModel(battleModel);
    }

    // Sending / Receiving
    public List<BattleCommand> GetCommandsToSend()
    {
        List<BattleCommand> commands = new List<BattleCommand>();
        if (commandsToBeSent != null)
        {
            for (int i = 0; i < commandsToBeSent.Length; i++)
            {
                commands.Add(commandsToBeSent[i]);
            }
        }
        commandsToBeSent = new BattleCommand[0];
        isSendingCommands = false;
        isSendingReplacements = false;
        return commands;
    }

    // Events
    public void StartEventRunner()
    {
        isExecutingEvents = true;
        isFinishedEvents = false;
        eventQueue = new List<BTLEvent>();
        eventRunner = StartCoroutine(ExecuteIncomingEvents());
    }
    public void StopEventRunner()
    {
        StopCoroutine(eventRunner);
        isExecutingEvents = false;
    }
    public void AddEvent(BTLEvent bEvent)
    {
        isFinishedEvents = false;
        eventQueue.Add(bEvent);
    }
    public IEnumerator ExecuteIncomingEvents()
    {
        /*string exampleText = "\\cred\\The Charmander\\c.\\ \\bwill be \\c#00ff00\\using\nDynamax Cannon, a \\bbase \\cred\\1\\cgreen\\0\\cblue\\0\\c.\\, 100 percent accurate Dragon-type " +
            "special move that deals double damage to Dynamaxed or Gigantamaxed targets. It is the signature move of " +
            "the Dragon & Poison-type legendary Pokemon \\i\\cpurple\\Eternatus\\c.\\.";
        yield return StartCoroutine(view.battleUI.dialog.DrawText(exampleText, true));*/

        while (true)
        {
            if (eventQueue.Count > 0 && isExecutingEvents)
            {
                // Run all messages in queue
                BTLEvent bEvent = eventQueue[0];
                executerCoroutine = StartCoroutine(HandleEvent(bEvent));
                yield return executerCoroutine;

                eventQueue.RemoveAt(0);

                // If we have run all the messages, tell the server that we're ready
                if (eventQueue.Count == 0)
                {
                    isFinishedEvents = true;
                }
            }
            yield return null;
        }
    }
    
    public IEnumerator HandleEvent(BTLEvent bEvent)
    {
        yield return StartCoroutine(

            // Load event
            (bEvent is BTLEvent_Load) ? HandleLoad(bEvent as BTLEvent_Load)
            : (bEvent is BTLEvent_Update) ? HandleUpdate(bEvent as BTLEvent_Update)

            // battle sequence events
            : (bEvent is BTLEvent_StartBattle) ? HandleBattleStart(bEvent as BTLEvent_StartBattle)
            : (bEvent is BTLEvent_EndBattle) ? HandleBattleEnd(bEvent as BTLEvent_EndBattle)

            // general events
            : (bEvent is BTLEvent_Message) ? HandleMessage(bEvent as BTLEvent_Message)
            : (bEvent is BTLEvent_GameText) ? HandleGameText(bEvent as BTLEvent_GameText)
            : (bEvent is BTLEvent_SendOut) ? HandleSendOut(bEvent as BTLEvent_SendOut)
            : (bEvent is BTLEvent_ForceOut) ? HandleForceOut(bEvent as BTLEvent_ForceOut)
            : (bEvent is BTLEvent_Withdraw) ? HandleWithdraw(bEvent as BTLEvent_Withdraw)

            : (bEvent is BTLEvent_Move) ? HandleMove(bEvent as BTLEvent_Move)
            : (bEvent is BTLEvent_MoveHit) ? HandleMoveHit(bEvent as BTLEvent_MoveHit)
            : (bEvent is BTLEvent_Damage) ? HandleDamage(bEvent as BTLEvent_Damage)
            : (bEvent is BTLEvent_MultiDamage) ? HandleMultiDamage(bEvent as BTLEvent_MultiDamage)
            : (bEvent is BTLEvent_Heal) ? HandleHeal(bEvent as BTLEvent_Heal)
            : (bEvent is BTLEvent_MultiHeal) ? HandleMultiHeal(bEvent as BTLEvent_MultiHeal)
            : (bEvent is BTLEvent_StatusCondition) ? HandleStatusAilment(bEvent as BTLEvent_StatusCondition)
            : (bEvent is BTLEvent_StatStageMod) ? HandleStatStageMod(bEvent as BTLEvent_StatStageMod)
            : (bEvent is BTLEvent_SwitchPosition) ? HandleSwitchPosition(bEvent as BTLEvent_SwitchPosition)
            : (bEvent is BTLEvent_ChangePokemon) ? HandleChangePokemon(bEvent as BTLEvent_ChangePokemon)
            : (bEvent is BTLEvent_Faint) ? HandleFaint(bEvent as BTLEvent_Faint)

            : (bEvent is BTLEvent_Ability) ? HandleAbility(bEvent as BTLEvent_Ability)

            // player commands
            : (bEvent is BTLEvent_PromptCommands) ? HandlePromptCommands(
                bEvent as BTLEvent_PromptCommands,
                (result) =>
                {
                    isSendingCommands = true;
                    commandsToBeSent = result;
                })
            : (bEvent is BTLEvent_PromptReplace) ? HandlePromptReplace(
                bEvent as BTLEvent_PromptReplace,
                (result) =>
                {
                    isSendingReplacements = true;
                    commandsToBeSent = result;
                })

            : HandleNull());
    }
    public IEnumerator HandleNull()
    {
        yield return null;
    }

    public IEnumerator HandleMessage(BTLEvent_Message bEvent)
    {
        Debug.Log(bEvent.message);
        yield return null;
    }

    public IEnumerator HandleLoad(BTLEvent_Load bEvent)
    {
        if (bEvent.battleModel != null)
        {
            yield return StartCoroutine(BattleAssetLoader.instance.LoadBattleAssets(bEvent.battleModel));
        }
        if (bEvent.pokemonData != null)
        {
            yield return StartCoroutine(BattleAssetLoader.instance.LoadPokemon(bEvent.pokemonData));
        }
        if (bEvent.pokemon != null)
        {
            yield return StartCoroutine(BattleAssetLoader.instance.LoadPokemon(bEvent.pokemon));
        }
        if (bEvent.item != null)
        {
            yield return StartCoroutine(BattleAssetLoader.instance.LoadItem(bEvent.item));
        }
    
    }

    public IEnumerator HandleUpdate(BTLEvent_Update bEvent)
    {
        UpdateModel(bEvent.battleModel);
        view.UpdateViaEvent(
            bEvent.updatePokemon
            );

        yield return null;
    }

    public IEnumerator HandleGameText(BTLEvent_GameText bEvent)
    {
        string gameText = view.GetGameText(bEvent);
        if (gameText != null)
        {
            yield return StartCoroutine(view.battleUI.DrawText(gameText));
        }
    }

    public IEnumerator HandleBattleStart(BTLEvent_StartBattle bEvent)
    {
        UpdateModel(bEvent.battleModel);

        // set the team position
        view.SetPlayerID(GlobalVariables.playerID);
        view.SetView();

        yield return StartCoroutine(view.StartBattle());
    }
    public IEnumerator HandleBattleEnd(BTLEvent_EndBattle bEvent)
    {
        UpdateModel(bEvent.battleModel);
        yield return StartCoroutine(view.EndBattle());
    }

    public IEnumerator HandleSendOut(BTLEvent_SendOut bEvent, bool skipUpdate = false)
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }

        Trainer trainer = bEvent.trainer;
        List<Pokemon> pokemon = new List<Pokemon>();
        for (int i = 0; i < bEvent.sendPokemon.Length; i++)
        {
            pokemon.Add(bEvent.sendPokemon[i]);
        }

        yield return StartCoroutine(
            view.SendOutPokemon(
                trainer,
                pokemon
                )
            );
    }

    public IEnumerator HandleForceOut(BTLEvent_ForceOut bEvent)
    {
        UpdateModel(bEvent.battleModel);

        BattleTeam allyTeam = battleModel.GetAllyTeam(view.teamPos);
        List<BattleTeam> enemyTeams = battleModel.GetEnemyTeams(view.teamPos);

        List<BTLEvent_SendOut> enemySendEvents = new List<BTLEvent_SendOut>();
        List<BTLEvent_SendOut> allySendEvents = new List<BTLEvent_SendOut>();
        BTLEvent_SendOut playerSendEvent = null;

        for (int i = 0; i < bEvent.sendOutEvents.Length; i++)
        {
            BTLEvent_SendOut sendEvent = bEvent.sendOutEvents[i];
            if (sendEvent.trainer.teamPos != view.teamPos)
            {
                enemySendEvents.Add(sendEvent);
            }
            else if (sendEvent.trainer.playerID != view.playerID)
            {
                allySendEvents.Add(sendEvent);
            }
            else
            {
                playerSendEvent = sendEvent;
            }
        }

        // run Enemy Send In
        for (int i = 0; i < enemySendEvents.Count; i++)
        {
            yield return StartCoroutine(HandleSendOut(enemySendEvents[i], true));
        }

        // run Ally Send In
        for (int i = 0; i < allySendEvents.Count; i++)
        {
            yield return StartCoroutine(HandleSendOut(allySendEvents[i], true));
        }

        // run player Send In
        if (playerSendEvent != null)
        {
            yield return StartCoroutine(HandleSendOut(playerSendEvent, true));
        }
    }

    public IEnumerator HandleWithdraw(BTLEvent_Withdraw bEvent, bool skipUpdate = false)
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }

        Trainer trainer = bEvent.trainer;
        List<Pokemon> pokemon = new List<Pokemon>();
        for (int i = 0; i < bEvent.withdrawPokemon.Length; i++)
        {
            pokemon.Add(bEvent.withdrawPokemon[i]);
        }
        yield return StartCoroutine(
            view.WithdrawPokemon(
                trainer,
                pokemon
                )
            );
    }

    public IEnumerator HandleMove(BTLEvent_Move bEvent)
    {
        UpdateModel(bEvent.battleModel);

        // Display move used if it was the first hit
        yield return StartCoroutine(view.UseMoveDisplay(
            bEvent.user,
            bEvent.moveID
            ));
    }
    public IEnumerator HandleMoveHit(BTLEvent_MoveHit bEvent)
    {
        UpdateModel(bEvent.battleModel);
        // run animation
        yield return StartCoroutine(view.UseMove(
            bEvent.user,
            bEvent.moveID,
            bEvent.moveHit,
            bEvent.battleHitTargets
            ));
    }

    public IEnumerator HandleDamage(BTLEvent_Damage bEvent, bool skipUpdate = false)
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }
        yield return StartCoroutine(view.DealDamage(
            bEvent.targetPokemon,
            bEvent.preHP,
            bEvent.postHP,
            bEvent.damageDealt,
            bEvent.effectiveness,
            bEvent.criticalHit,
            bEvent.hitDisplay
            ));
    }

    public IEnumerator HandleMultiDamage(BTLEvent_MultiDamage bEvent, bool skipUpdate = false)
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }

        List<Coroutine> dmgRoutines = new List<Coroutine>();
        // run hit events
        for (int i = 0; i < bEvent.targets.Length; i++)
        {
            Coroutine dCR = StartCoroutine(HandleDamage(bEvent.targets[i], true));
            dmgRoutines.Add(dCR);
        }
        for (int i = 0; i < dmgRoutines.Count; i++)
        {
            yield return dmgRoutines[i];
        }

        yield return null;
    }

    public IEnumerator HandleHeal(BTLEvent_Heal bEvent, bool skipUpdate = false)
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }
        yield return StartCoroutine(view.HealDamage(
            bEvent.targetPokemon,
            bEvent.preHP,
            bEvent.postHP,
            bEvent.hpHealed
            ));
    }

    public IEnumerator HandleMultiHeal(BTLEvent_MultiHeal bEvent, bool skipUpdate = false)
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }

        List<Coroutine> healRoutines = new List<Coroutine>();
        // run hit events
        for (int i = 0; i < bEvent.targets.Length; i++)
        {
            Coroutine dCR = StartCoroutine(HandleHeal(bEvent.targets[i], true));
            healRoutines.Add(dCR);
        }
        for (int i = 0; i < healRoutines.Count; i++)
        {
            yield return healRoutines[i];
        }

        yield return null;
    }

    public IEnumerator HandleStatusAilment(
        BTLEvent_StatusCondition bEvent,
        bool skipUpdate = false)
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }
        yield return StartCoroutine(view.InflictStatus(
            bEvent.targetPokemon,
            bEvent.statusID
            ));
    }

    public IEnumerator HandleStatStageMod(
        BTLEvent_StatStageMod bEvent,
        bool skipUpdate = false
        )
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }
        yield return StartCoroutine(view.ApplyStatStageMod(
            bEvent.targetPokemon,
            bEvent.modValue,
            bEvent.statsToMod
            ));
        
        BTLEvent_GameText gameTextEvent = bEvent.gameText;
        bool displayDefaultStats = false;
        if (gameTextEvent == null)
        {
            displayDefaultStats = true;
        }
        else
        {
            if (string.IsNullOrEmpty(gameTextEvent.textID))
            {
                displayDefaultStats = true;
            }
        }

        if (displayDefaultStats)
        {
            string modTextCode = (bEvent.maximize)? "stats-maximize"
                : (bEvent.minimize)? "stats-minimize"
                : (bEvent.modValue == 1) ? "stats-up1"
                : (bEvent.modValue == 2) ? "stats-up2"
                : (bEvent.modValue >= 3) ? "stats-up3"
                : (bEvent.modValue == -1) ? "stats-down1"
                : (bEvent.modValue == -2) ? "stats-down2"
                : (bEvent.modValue <= -3) ? "stats-down3"
                : "";
            gameTextEvent.textID = modTextCode;
            yield return StartCoroutine(HandleGameText(gameTextEvent));
        }
    }

    public IEnumerator HandleAbility(
        BTLEvent_Ability bEvent,
        bool skipUpdate = false
        )
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }
        yield return StartCoroutine(view.IndicateAbility(
            bEvent.pokemon,
            bEvent.abilityID
            ));
    }

    public IEnumerator HandleSwitchPosition(BTLEvent_SwitchPosition bEvent)
    {
        UpdateModel(bEvent.battleModel);

        yield return StartCoroutine(view.SwitchPokemonPositions(
            pokemon1: bEvent.pokemon1,
            pokemon2: bEvent.pokemon2
            ));
    }

    public IEnumerator HandleChangePokemon(BTLEvent_ChangePokemon bEvent)
    {
        UpdateModel(bEvent.battleModel);

        yield return StartCoroutine(view.ChangePokemon(
            pokemon: bEvent.pokemon,
            prePokemonID: bEvent.prePokemon,
            postPokemonID: bEvent.postPokemon
            ));
    }

    public IEnumerator HandleFaint(BTLEvent_Faint bEvent, bool skipUpdate = false)
    {
        if (!skipUpdate)
        {
            UpdateModel(bEvent.battleModel);
        }
        yield return StartCoroutine(view.FaintPokemon(
            new List<Pokemon>(bEvent.faintedPokemon)
            ));
    }

    // Controller

    public void Legacy_SwitchControlContext(ControlContext newContext)
    {
        // switch from previous context, unsetting relevant listeners
        switch (context)
        {
            case ControlContext.Command:
                _controls.Battle.Cancel.performed -= CancelCommandMenu;
                _controls.Battle.Select.performed -= SelectCommandMenu;
                _controls.Battle.Move.performed -= NavigateCommandMenuQuad;
                break;

            case ControlContext.Fight:
                _controls.Battle.Cancel.performed -= CancelFightMenu;
                _controls.Battle.Select.performed -= SelectFightMenu;
                _controls.Battle.Special.performed -= SelectFightSpecial;
                _controls.Battle.Move.performed -= NavigateFightMenuQuad;
                break;

            case ControlContext.FieldTarget:
                _controls.Battle.Cancel.performed -= CancelFieldTargetMenu;
                _controls.Battle.Select.performed -= SelectFieldTargetMenu;
                _controls.Battle.Move.performed -= NavigateFieldTargetMenu;
                break;

            case ControlContext.Party:
                _controls.Battle.Cancel.performed -= CancelPartyMenu;
                _controls.Battle.Select.performed -= SelectPartyMenu;
                _controls.Battle.Move.performed -= NavigatePartyMenuQuad;
                break;

            case ControlContext.PartyCommand:
                _controls.Battle.Cancel.performed -= CancelPartyCommandMenu;
                _controls.Battle.Select.performed -= SelectPartyCommandMenu;
                _controls.Battle.Move.performed -= NavigatePartyCommandMenu;
                break;

            case ControlContext.BagPocket:
                _controls.Battle.Cancel.performed -= CancelBagPocketMenu;
                _controls.Battle.Select.performed -= SelectBagPocketMenu;
                _controls.Battle.Move.performed -= NavigateBagPocketMenuQuad;
                break;

            case ControlContext.Bag:
                _controls.Battle.Cancel.performed -= CancelBagMenu;
                _controls.Battle.Select.performed -= SelectBagMenu;
                _controls.Battle.Move.performed -= NavigateBagMenuQuad;
                break;

            case ControlContext.BagTarget:
                break;

            default:
                break;
        }

        // switch to new context, setting relevant listeners
        context = newContext;
        switch (newContext)
        {
            case ControlContext.Command:
                _controls.Battle.Cancel.performed += CancelCommandMenu;
                _controls.Battle.Select.performed += SelectCommandMenu;
                _controls.Battle.Move.performed += NavigateCommandMenuQuad;
                break;

            case ControlContext.Fight:
                _controls.Battle.Cancel.performed += CancelFightMenu;
                _controls.Battle.Select.performed += SelectFightMenu;
                _controls.Battle.Special.performed += SelectFightSpecial;
                _controls.Battle.Move.performed += NavigateFightMenuQuad;
                break;

            case ControlContext.FieldTarget:
                _controls.Battle.Cancel.performed += CancelFieldTargetMenu;
                _controls.Battle.Select.performed += SelectFieldTargetMenu;
                _controls.Battle.Move.performed += NavigateFieldTargetMenu;
                break;

            case ControlContext.Party:
                _controls.Battle.Cancel.performed += CancelPartyMenu;
                _controls.Battle.Select.performed += SelectPartyMenu;
                _controls.Battle.Move.performed += NavigatePartyMenuQuad;
                break;

            case ControlContext.PartyCommand:
                _controls.Battle.Cancel.performed += CancelPartyCommandMenu;
                _controls.Battle.Select.performed += SelectPartyCommandMenu;
                _controls.Battle.Move.performed += NavigatePartyCommandMenu;
                break;

            case ControlContext.BagPocket:
                _controls.Battle.Cancel.performed += CancelBagPocketMenu;
                _controls.Battle.Select.performed += SelectBagPocketMenu;
                _controls.Battle.Move.performed += NavigateBagPocketMenuQuad;
                break;

            case ControlContext.Bag:
                _controls.Battle.Cancel.performed += CancelBagMenu;
                _controls.Battle.Select.performed += SelectBagMenu;
                _controls.Battle.Move.performed += NavigateBagMenuQuad;
                break;

            case ControlContext.BagTarget:
                break;

            default:
                break;
        }
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

        moveslots = new List<Pokemon.Moveslot>();
        moveIndex = 0;
        canMegaEvolve = false;
        chooseMegaEvolve = false;
        canZMove = false;
        chooseZMove = false;
        canDynamax = false;
        chooseDynamax = false;
        canFormChange = false;
        chooseFormChange = false;

        moveTargets = new List<List<BattlePosition>>();
        moveTargetIndex = 0;

        partySlots = new List<Pokemon>();
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
        BTLEvent_PromptCommands bEvent,
        Action<BattleCommand[]> callback)
    {
        UpdateModel(bEvent.battleModel);

        Trainer trainer = bEvent.trainer;
        committedCommands = new BattleCommand[bEvent.pokemonToCommand.Length];

        List<Pokemon> pokemonToControl = new List<Pokemon>();
        for (int i = 0; i < bEvent.pokemonToCommand.Length; i++)
        {
            pokemonToControl.Add(bEvent.pokemonToCommand[i]);
        }

        view.battleUI.SetPokemonHUDsActive(true);
        for (int i = 0; i < pokemonToControl.Count; i++)
        {
            view.battleUI.UndrawDialogBox();

            playerCommand = null;
            committedCommands[i] = null;
            switchPosition = pokemonToControl[i].battlePos;

            SetDefaultPromptVars();
            controlCommandCR = StartCoroutine(ControlPromptCommand(
                    pokemon: pokemonToControl[i],
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
            view.battleUI.UnsetPanels();
        }
        view.battleUI.SwitchPanel(BTLUI_Base.Panel.None);
        callback(committedCommands);
    }

    public IEnumerator HandlePromptReplace(
        BTLEvent_PromptReplace bEvent,
        Action<BattleCommand[]> callback)
    {
        UpdateModel(bEvent.battleModel);

        Trainer trainer = bEvent.trainer;
        int[] fillPositions = bEvent.fillPositions;
        committedCommands = new BattleCommand[bEvent.fillPositions.Length];

        view.battleUI.SetPokemonHUDsActive(true);
        for (int i = 0; i < fillPositions.Length; i++)
        {
            playerCommand = null;
            committedCommands[i] = null;

            // set Default Values
            partySlots = new List<Pokemon>();
            partyIndex = 0;
            switchPosition = fillPositions[i];
            forceSwitch = true;
            forceReplace = true;

            // dialog prompt
            Debug.Log("Who will you send in?");

            controlCommandCR = StartCoroutine(ControlPromptParty(
                    null,
                    trainer,
                    committedCommands,
                    forceSwitch));
            yield return controlCommandCR;
            //Legacy_SwitchControlContext(ControlContext.None);
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
        view.battleUI.SwitchPanel(BTLUI_Base.Panel.None);
        callback(committedCommands);
    }

    private IEnumerator ControlPromptCommand(
        Pokemon pokemon, 
        Trainer trainer, 
        int pokemonIndex, 
        BattleCommand[] setCommands)
    {
        commandPokemon = pokemon;
        commandTrainer = trainer;
        committedCommands = (setCommands == null) ? new BattleCommand[0] : setCommands;

        // set the viable commands for this pokemon
        commandTypes = battleModel.GetPokemonPossibleCommandTypes(pokemon);
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
        // do not default to "Back" command
        if (commandTypes[commandIndex] == BattleCommandType.Back)
        {
            commandIndex++;
        }

        // set the controls and ui elements
        choosingCommand = true;
        //Legacy_SwitchControlContext(ControlContext.Command);
        SwitchControlContext(ControlContext.Command);

        view.battleUI.SwitchPanel(BTLUI_Base.Panel.Command);
        view.battleUI.SetCommands(pokemon, commandTypes);
        view.battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);

        while (choosingCommand)
        {
            yield return null;
        }

        //Legacy_SwitchControlContext(ControlContext.None);
        SwitchControlContext(ControlContext.None);
    }

    private IEnumerator ControlPromptCommandExtra(
        Pokemon pokemon,
        Trainer trainer,
        BattleCommand[] setCommands,
        List<BattleExtraCommand> commandList
        )
    {
        commandPokemon = pokemon;
        commandTrainer = trainer;
        committedCommands = (setCommands == null) ? new BattleCommand[0] : setCommands;

        // set the viable commands for this pokemon
        extraCommands = commandList;

        // set the initial command index
        if (extraCommandIndex > extraCommands.Count)
        {
            extraCommandIndex = 0;
        }

        // set the controls and ui elements
        choosingExtraCommand = true;
        //Legacy_SwitchControlContext(ControlContext.PartyCommand);
        SwitchControlContext(ControlContext.PartyCommand);

        view.battleUI.SwitchPanel(BTLUI_Base.Panel.PartyCommand);
        view.battleUI.SetPartyCommands(partySlots[partyIndex], extraCommands);
        view.battleUI.SwitchSelectedPartyCommandTo(extraCommands[extraCommandIndex]);

        while (choosingExtraCommand)
        {
            yield return null;
        }
    }

    public IEnumerator ControlPromptFight(
        Pokemon pokemon,
        Trainer trainer,
        BattleCommand[] setCommands = null,
        bool forceMove = false
        )
    {
        commandPokemon = pokemon;
        commandTrainer = trainer;
        committedCommands = (setCommands == null) ? new BattleCommand[0] : setCommands;
        this.forceMove = forceMove;

        // set the viable moves for this pokemon
        moveslots = battleModel.GetPokemonBattleMoveslots(pokemon);

        // if there's no useable moves, show struggle option
        if (battleModel.GetPokemonUseableMoves(pokemon).Count == 0)
        {
            moveslots = new List<Pokemon.Moveslot> { new Pokemon.Moveslot("struggle") };
        }
        else
        {
            // Check Mega-Evolution / Form Change
            if (!commandTrainer.bProps.usedMegaEvolution
                && battleModel.battleSettings.canMegaEvolve
                && !canZMove 
                && !canDynamax 
                && !canFormChange)
            {
                Item item = pokemon.item;
                if (item != null)
                {
                    EffectDatabase.ItemEff.ItemEffect formChangeItemEffect =
                        battleModel.PBPGetItemFormChangeEffect(pokemon, item);
                    if (formChangeItemEffect != null)
                    {
                        if (formChangeItemEffect is EffectDatabase.ItemEff.MegaStone)
                        {
                            canMegaEvolve = true;
                        }
                        // Ultra Burst / Other form change items
                        else
                        {

                        }
                    }
                }
            }

            // Check Z-Move
            if (!commandTrainer.bProps.usedZMove
                && battleModel.battleSettings.canZMove
                && !canMegaEvolve 
                && !canDynamax)
            {
                for (int i = 0; i < moveslots.Count && !canZMove; i++)
                {
                    MoveData ZMoveData = battleModel.GetPokemonZMoveData(pokemon, moveslots[i].moveID);
                    if (ZMoveData != null)
                    {
                        canZMove = true;
                        break;
                    }
                }
            }

            // Check Dynamax
            if (!commandTrainer.bProps.usedDynamax
                && battleModel.battleSettings.canDynamax
                && !canMegaEvolve 
                && !canZMove)
            {
                canDynamax = true;
            }
        }

        // do not allow the back button if a move has to be chosen
        if (!forceMove)
        {
            // null represents the back button
            moveslots.Insert(0, null);
        }

        // set the initial move index
        if (moveIndex > moveslots.Count)
        {
            moveIndex = 0;
        }
        // do not default to "Back" button
        if (moveslots[moveIndex] == null)
        {
            moveIndex++;
        }

        choosingFight = true;
        //Legacy_SwitchControlContext(ControlContext.Fight);
        SwitchControlContext(ControlContext.Fight);

        view.battleUI.SetMoves(
            pokemon: commandPokemon,
            moveslots: moveslots, 
            canMegaEvolve: canMegaEvolve, 
            canZMove: canZMove,
            canDynamax: canDynamax,
            choosingZMove: chooseZMove,
            choosingMaxMove: chooseDynamax || commandPokemon.dynamaxState != Pokemon.DynamaxState.None);
        view.battleUI.SwitchPanel(BTLUI_Base.Panel.Fight);
        view.battleUI.SwitchSelectedMoveTo(
            pokemon: commandPokemon, 
            selected: moveslots[moveIndex], 
            choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
            choosingZMove: chooseZMove,
            choosingMaxMove: chooseDynamax || pokemon.dynamaxState != Pokemon.DynamaxState.None);

        while (choosingFight)
        {
            yield return null;
        }
    }

    public IEnumerator ControlPromptFieldTarget(
        Pokemon pokemon,
        Trainer trainer,
        Pokemon.Moveslot selectedMoveslot,
        BattleCommand[] setCommands = null
        )
    {
        commandPokemon = pokemon;
        commandTrainer = trainer;
        this.selectedMoveslot = selectedMoveslot;
        committedCommands = (setCommands == null) ? new BattleCommand[0] : setCommands;

        // get the possible targets for the given move
        string moveID = selectedMoveslot.moveID;
        if (chooseZMove)
        {
            MoveData ZMoveData = battleModel.GetPokemonZMoveData(userPokemon: commandPokemon, moveID: selectedMoveslot.moveID);
            moveID = ZMoveData.ID;
        }
        if (chooseDynamax)
        {
            MoveData maxMoveData = battleModel.GetPokemonMaxMoveData(
                userPokemon: commandPokemon, 
                moveData: MoveDatabase.instance.GetMoveData(selectedMoveslot.moveID));
            moveID = maxMoveData.ID;
        }

        moveTargets = battleModel.GetMovePossibleTargets(commandPokemon, MoveDatabase.instance.GetMoveData(moveID));
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
        //Legacy_SwitchControlContext(ControlContext.FieldTarget);
        SwitchControlContext(ControlContext.FieldTarget);

        view.battleUI.SwitchPanel(BTLUI_Base.Panel.FieldTargeting);
        view.battleUI.SetFieldTargets(view.teamPos);
        view.battleUI.SwitchSelectedMoveTargetsTo(battleModel.GetPokemonPosition(pokemon), moveTargetIndex, moveTargets);

        while (choosingFightTarget)
        {
            // The player is selecting target
            yield return null;
        }
    }

    public IEnumerator ControlPromptParty(
        Pokemon pokemon,
        Trainer trainer,
        BattleCommand[] setCommands = null,
        bool forceSwitch = false
        )
    {
        commandPokemon = pokemon;
        commandTrainer = trainer;
        committedCommands = (setCommands == null) ? new BattleCommand[0] : setCommands;
        this.forceSwitch = forceSwitch;

        // set the possible party pokemon that can switch in
        partySlots = new List<Pokemon>(commandTrainer.party);
        if (!forceSwitch)
        {
            partySlots.Insert(0, null);
        }

        // set the initial party index
        if (partyIndex > partySlots.Count)
        {
            partyIndex = 0;
        }
        // do not default to "Back" command
        if (partySlots[partyIndex] == null)
        {
            partyIndex++;
        }

        choosingParty = true;
        //Legacy_SwitchControlContext(ControlContext.Party);
        SwitchControlContext(ControlContext.Party);

        view.battleUI.SwitchPanel(BTLUI_Base.Panel.Party);
        view.battleUI.SetParty(partySlots, forceSwitch, selectedItem);
        view.battleUI.SwitchSelectedPartyMemberTo(partySlots[partyIndex]);

        while (choosingParty)
        {
            yield return null;
        }
    }

    public IEnumerator ControlPromptBagPocket(
        Pokemon pokemon,
        Trainer trainer
        )
    {
        commandPokemon = pokemon;
        commandTrainer = trainer;

        itemPockets = battleModel.GetTrainerItemPockets(commandTrainer);
        
        // None serves as the back button
        itemPockets.Insert(0, ItemBattlePocket.None);

        // set the initial item index
        if (itemPocketIndex > itemPockets.Count)
        {
            itemPocketIndex = 0;
        }
        // do not default to "Back" command
        if (itemPockets[itemPocketIndex] == ItemBattlePocket.None)
        {
            itemPocketIndex++;
        }

        choosingBagPocket = true;
        //Legacy_SwitchControlContext(ControlContext.BagPocket);
        SwitchControlContext(ControlContext.BagPocket);

        view.battleUI.SwitchPanel(BTLUI_Base.Panel.Bag);
        view.battleUI.SetBagPockets(itemPockets);
        view.battleUI.SwitchSelectedBagPocketTo(itemPockets[itemPocketIndex]);

        while (choosingBagPocket)
        {
            yield return null;
        }
    }

    public IEnumerator ControlPromptBag(
        Pokemon pokemon,
        Trainer trainer,
        ItemBattlePocket pocket,
        BattleCommand[] setCommands = null
        )
    {
        commandPokemon = pokemon;
        commandTrainer = trainer;
        committedCommands = (setCommands == null) ? new BattleCommand[0] : setCommands;

        // set the possible items that the trainer can use
        itemSlots = trainer.GetItemsByBattlePocket(pocket);

        // set the initial item index
        if (itemOffset + itemIndex >= itemSlots.Count)
        {
            itemOffset = 0;
            itemIndex = 0;
        }
        // do not default to "Back" command
        if (itemSlots.Count == 0)
        {
            itemSlots.Insert(0, null);
        }

        choosingItem = true;
        //Legacy_SwitchControlContext(ControlContext.Bag);
        SwitchControlContext(ControlContext.Bag);

        view.battleUI.SwitchPanel(BTLUI_Base.Panel.BagItem);
        view.battleUI.SetItems(trainer, pocket, itemSlots, itemOffset);
        view.battleUI.SwitchSelectedItemTo(itemSlots[itemOffset + itemIndex]);

        while (choosingItem)
        {
            yield return null;
        }
    }

    // GENERAL MENU
    private object GetMenuSelectMap(int rows, int columns, bool accountForBack, List<object> list)
    {
        object[,] map = new object[rows, columns];
        int start = accountForBack ? 1 : 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i == 0 && accountForBack)
                {
                    map[i, j] = null;
                }
                else
                {
                    if (start >= list.Count)
                    {
                        map[i, j] = null;
                    }
                    else
                    {
                        map[i, j] = list[start];
                    }
                    start++;
                }
            }
        }
        return map;
    }
    private int[] GetMenuSelectXYPos(object selected, object[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == selected)
                {
                    return new int[] { i, j };
                }
            }
        }

        return new int[] { -1, -1 };
    }


    // COMMAND MENU
    private void NavigateCommandMenuVertical(InputAction.CallbackContext obj)
    {
        int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);
        NavigateCommandMenu(addIndex);
    }
    private void NavigateCommandMenuHorizontal(InputAction.CallbackContext obj)
    {
        int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
        NavigateCommandMenu(addIndex);
    }
    private void NavigateCommandMenuQuad(InputAction.CallbackContext obj)
    {
        int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
        int scrollY = -Mathf.RoundToInt(obj.ReadValue<Vector2>().y);

        if (scrollX != 0 || scrollY != 0)
        {
            bool accountForBack = commandTypes.Contains(BattleCommandType.Back);
            int rows = accountForBack ? 3 : 2;
            int columns = 2;
            BattleCommandType[,] cmdMap = GetCommandTypeMap(rows, columns, accountForBack);

            int newPos = commandIndex;
            int[] curPos = GetCommandXYPos(commandTypes[commandIndex], cmdMap);
            if (curPos[0] != -1)
            {
                int newRow = (curPos[0] + scrollY) % rows;
                if (newRow < 0) newRow += rows;

                int newColumn = (curPos[1] + scrollX) % columns;
                if (newColumn < 0) newColumn += columns;

                BattleCommandType nextCmd = cmdMap[newRow, newColumn];
                if (nextCmd != BattleCommandType.None)
                {
                    newPos = commandTypes.IndexOf(nextCmd);
                }
            }
            NavigateCommandMenu(newPos - commandIndex);
        }
    }
    private BattleCommandType[,] GetCommandTypeMap(int rows, int columns, bool accountForBack)
    {
        BattleCommandType[,] map = new BattleCommandType[rows, columns];
        int commandStart = accountForBack ? 1 : 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i == 0 && accountForBack)
                {
                    map[i, j] = (j == 0) ? BattleCommandType.Back : BattleCommandType.None;
                }
                else
                {
                    map[i, j] = commandTypes[commandStart];
                    commandStart++;
                }
            }
        }
        return map;
    }
    private int[] GetCommandXYPos(BattleCommandType commandType, BattleCommandType[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i,j] == commandType)
                {
                    return new int[] { i, j };
                }
            }
        }

        return new int[] { -1, -1 };
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
            view.battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
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
            if (commandType == BattleCommandType.Fight)
            {
                controlFightCR = StartCoroutine(ControlPromptFight(
                    commandPokemon,
                    commandTrainer,
                    committedCommands,
                    forceMove
                    ));
            }
            else if (commandType == BattleCommandType.Party)
            {
                controlPartyCR = StartCoroutine(ControlPromptParty(
                    commandPokemon,
                    commandTrainer,
                    committedCommands,
                    forceSwitch
                    ));
            }
            else if (commandType == BattleCommandType.Bag)
            {
                controlBagPocketCR = StartCoroutine(ControlPromptBagPocket(
                    commandPokemon,
                    commandTrainer
                    ));
            }
            else if (commandType == BattleCommandType.Run)
            {
                BattleCommand attemptedCommand = BattleCommand.CreateRunCommand(commandPokemon, true);

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
    private void NavigateFightMenuVertical(InputAction.CallbackContext obj)
    {
        int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);
        NavigateFightMenu(addIndex);
    }
    private void NavigateFightMenuQuad(InputAction.CallbackContext obj)
    {
        int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
        int scrollY = -Mathf.RoundToInt(obj.ReadValue<Vector2>().y);

        if (scrollX != 0 || scrollY != 0)
        {
            bool accountForBack = moveslots.Contains(null);
            int rows = ((moveslots.Count + (accountForBack ? -1 : 0)) / 2) + (accountForBack ? 1 : 0);
            int columns = 2;
            Pokemon.Moveslot[,] map = GetMoveslotMap(rows, columns, accountForBack, moveslots);

            int newPos = moveIndex;
            int[] curPos = GetMoveslotXYPos(moveslots[moveIndex], map);
            if (curPos[0] != -1)
            {
                int newRow = (curPos[0] + scrollY) % rows;
                if (newRow < 0) newRow += rows;

                int newColumn = (curPos[1] + scrollX) % columns;
                if (newColumn < 0) newColumn += columns;

                Pokemon.Moveslot nxtSlot = map[newRow, newColumn];
                newPos = moveslots.IndexOf(nxtSlot);
            }
            NavigateFightMenu(newPos - moveIndex);
        }
    }
    private Pokemon.Moveslot[,] GetMoveslotMap(int rows, int columns, bool accountForBack, List<Pokemon.Moveslot> list)
    {
        Pokemon.Moveslot[,] map = new Pokemon.Moveslot[rows, columns];
        int start = accountForBack ? 1 : 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i == 0 && accountForBack)
                {
                    map[i, j] = null;
                }
                else
                {
                    if (start >= list.Count)
                    {
                        map[i, j] = null;
                    }
                    else
                    {
                        map[i, j] = list[start];
                    }
                    start++;
                }
            }
        }
        return map;
    }
    private int[] GetMoveslotXYPos(Pokemon.Moveslot moveslot, Pokemon.Moveslot[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == moveslot)
                {
                    return new int[] { i, j };
                }
            }
        }

        return new int[] { -1, -1 };
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
            view.battleUI.SwitchSelectedMoveTo(
                pokemon: commandPokemon,
                selected: moveslots[moveIndex],
                choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                choosingZMove: chooseZMove,
                choosingMaxMove: chooseDynamax || commandPokemon.dynamaxState != Pokemon.DynamaxState.None);
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
        view.battleUI.SetMoves(
            pokemon: commandPokemon,
            moveslots: moveslots,
            canMegaEvolve: canMegaEvolve,
            canZMove: canZMove,
            canDynamax: canDynamax,
            choosingZMove: chooseZMove,
            choosingMaxMove: chooseDynamax || commandPokemon.dynamaxState != Pokemon.DynamaxState.None);
        view.battleUI.SwitchSelectedMoveTo(
            pokemon: commandPokemon,
            selected: moveslots[moveIndex],
            choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
            choosingZMove: chooseZMove,
            choosingMaxMove: chooseDynamax || commandPokemon.dynamaxState != Pokemon.DynamaxState.None);
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
        Pokemon.Moveslot choice = moveslots[moveIndex];

        // exit to commands
        if (choice == null)
        {
            CancelFightMenu();
        }
        // create command
        else
        {
            bool validMove = true;
            if (chooseZMove)
            {
                MoveData ZMoveData =
                    battleModel.GetPokemonZMoveData(userPokemon: commandPokemon, moveID: choice.moveID);
                if (ZMoveData == null)
                {
                    validMove = false;
                }
                else
                {
                    StartCoroutine(view.battleUI.DrawTextInstant("This move cannot use Z-Power!", undrawOnFinish: true));
                }
            }

            if (validMove)
            {
                // if auto-target (ex. Singles), we're done here
                if (battleModel.DoesBattleAutoTarget())
                {
                    BattleCommand attemptedCommand = BattleCommand.CreateMoveCommand(
                        commandUser: commandPokemon,
                        moveID: choice.moveID,
                        targetPositions: 
                            battleModel.GetMoveAutoTargets(
                                commandPokemon, 
                                MoveDatabase.instance.GetMoveData(choice.moveID)),
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
                            view.battleUI.SwitchSelectedMoveTo(
                                pokemon: commandPokemon,
                                selected: moveslots[moveIndex],
                                choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                                choosingZMove: chooseZMove,
                                choosingMaxMove: chooseDynamax || commandPokemon.dynamaxState != Pokemon.DynamaxState.None);
                        }
                        waitCRActive = false;
                    }));
                }
                // we have to specifically choose the target
                else
                {
                    controlFightTargetCR = StartCoroutine(ControlPromptFieldTarget(
                        commandPokemon,
                        commandTrainer,
                        choice,
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
        if (moveslots.Contains(null))
        {
            playerCommand = null;
            choosingFight = false;

            //Legacy_SwitchControlContext(ControlContext.Command);
            SwitchControlContext(ControlContext.Command);
            view.battleUI.SwitchPanel(BTLUI_Base.Panel.Command);
            view.battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
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
            view.battleUI.SwitchSelectedMoveTargetsTo(battleModel.GetPokemonPosition(commandPokemon), moveTargetIndex, moveTargets);
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
            if (chooseZMove)
            {
                MoveData ZMoveData = 
                    battleModel.GetPokemonZMoveData(userPokemon: commandPokemon, moveID: selectedMoveslot.moveID);
                if (ZMoveData == null)
                {
                    validMove = false;
                }
                else
                {
                    StartCoroutine(view.battleUI.DrawTextInstant("This move cannot use Z-Power!", undrawOnFinish: true));
                }
            }

            if (validMove)
            {
                BattleCommand attemptedCommand = BattleCommand.CreateMoveCommand(
                    commandUser: commandPokemon,
                    moveID: selectedMoveslot.moveID,
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
                        view.battleUI.SwitchSelectedMoveTargetsTo(battleModel.GetPokemonPosition(commandPokemon), moveTargetIndex, moveTargets);
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

            //Legacy_SwitchControlContext(ControlContext.Fight);
            SwitchControlContext(ControlContext.Fight);
            view.battleUI.SwitchPanel(BTLUI_Base.Panel.Fight);
            view.battleUI.SwitchSelectedMoveTo(
                pokemon: commandPokemon,
                selected: moveslots[moveIndex],
                choosingSpecial: chooseMegaEvolve || chooseZMove || chooseDynamax,
                choosingZMove: chooseZMove,
                choosingMaxMove: chooseDynamax || commandPokemon.dynamaxState != Pokemon.DynamaxState.None);
        }
    }



    // PARTY MENU
    private void NavigatePartyMenuVertical(InputAction.CallbackContext obj)
    {
        int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);
        NavigatePartyMenu(addIndex);
    }
    private void NavigatePartyMenuQuad(InputAction.CallbackContext obj)
    {
        int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
        int scrollY = -Mathf.RoundToInt(obj.ReadValue<Vector2>().y);

        if (scrollX != 0 || scrollY != 0)
        {
            bool accountForBack = partySlots.Contains(null);
            int rows = 1 + ((partySlots.Count - 1) / 2) + (accountForBack ? 1 : 0);
            int columns = 2;
            Pokemon[,] map = GetPartyslotMap(rows, columns, accountForBack);

            int newPos = partyIndex;
            int[] curPos = GetPartyslotXYPos(partySlots[partyIndex], map);
            if (curPos[0] != -1)
            {
                int newRow = (curPos[0] + scrollY) % rows;
                if (newRow < 0) newRow += rows;

                int newColumn = (curPos[1] + scrollX) % columns;
                if (newColumn < 0) newColumn += columns;

                Pokemon nxtSlot = map[newRow, newColumn];
                if (nxtSlot != null || accountForBack)
                {
                    newPos = partySlots.IndexOf(nxtSlot);
                }
            }
            NavigatePartyMenu(newPos - partyIndex);
        }
    }
    private Pokemon[,] GetPartyslotMap(int rows, int columns, bool accountForBack)
    {
        Pokemon[,] map = new Pokemon[rows, columns];
        int start = accountForBack ? 1 : 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i == 0 && accountForBack)
                {
                    map[i, j] = null;
                }
                else
                {
                    if (start >= partySlots.Count)
                    {
                        map[i, j] = null;
                    }
                    else
                    {
                        map[i, j] = partySlots[start];
                    }
                    start++;
                }
            }
        }
        return map;
    }
    private int[] GetPartyslotXYPos(Pokemon selected, Pokemon[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == selected)
                {
                    return new int[] { i, j };
                }
            }
        }

        return new int[] { -1, -1 };
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
            view.battleUI.SwitchSelectedPartyMemberTo(partySlots[partyIndex]);
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
        Pokemon choice = partySlots[partyIndex];

        // exit to commands
        if (choice == null)
        {
            CancelPartyMenu();
        }
        else
        {
            // no selected item
            if (selectedItem == null)
            {
                controlPartyCommandCR = StartCoroutine(ControlPromptCommandExtra(
                commandPokemon,
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
                BattleCommand attemptedCommand = BattleCommand.CreateBagCommand(
                    itemID: selectedItem.itemID,
                    itemPokemon: choice,
                    trainer: commandTrainer,
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
        if (partySlots.Contains(null))
        {
            playerCommand = null;
            choosingParty = false;
            if (!forceSwitch)
            {
                if (selectedItem == null)
                {
                    //Legacy_SwitchControlContext(ControlContext.Command);
                    SwitchControlContext(ControlContext.Command);
                    view.battleUI.SwitchPanel(BTLUI_Base.Panel.Command);
                    view.battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
                }
                else
                {
                    selectedItem = null;
                    //Legacy_SwitchControlContext(ControlContext.Bag);
                    SwitchControlContext(ControlContext.Bag);
                    view.battleUI.SwitchPanel(BTLUI_Base.Panel.BagItem);
                }
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
            view.battleUI.SwitchSelectedPartyCommandTo(extraCommands[extraCommandIndex]);
            //view.battleUI.SwitchSelectedExtraCommandTo(extraCommands[extraCommandIndex], extraCommands);
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
            view.battleUI.SwitchSelectedPartyCommandTo(extraCommands[extraCommandIndex]);
            //view.battleUI.SwitchSelectedExtraCommandTo(extraCommands[extraCommandIndex], extraCommands);
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
        Pokemon choice = partySlots[partyIndex];

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
                BattleCommand attemptedCommand = (forceReplace) ?
                    BattleCommand.CreateReplaceCommand(
                        switchPosition,
                        commandTrainer,
                        choice,
                        true)
                    :
                    BattleCommand.CreateSwitchCommand(
                        commandPokemon,
                        switchPosition,
                        commandTrainer,
                        choice,
                        true);

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

            //Legacy_SwitchControlContext(ControlContext.Party);
            SwitchControlContext(ControlContext.Party);
            view.battleUI.SwitchPanel(BTLUI_Base.Panel.Party);
            view.battleUI.SwitchSelectedPartyMemberTo(partySlots[partyIndex]);
        }
    }



    // BAG POCKET MENU
    private void NavigateBagPocketMenu(InputAction.CallbackContext obj)
    {
        int addIndex = Mathf.RoundToInt(obj.ReadValue<Vector2>().y);
        NavigateBagPocketMenu(addIndex);
    }
    private void NavigateBagPocketMenuQuad(InputAction.CallbackContext obj)
    {
        int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
        int scrollY = -Mathf.RoundToInt(obj.ReadValue<Vector2>().y);

        if (scrollX != 0 || scrollY != 0)
        {
            bool accountForBack = itemPockets.Contains(ItemBattlePocket.None);
            int rows = accountForBack ? 3 : 2;
            int columns = 2;
            ItemBattlePocket[,] map = GetBagPocketMap(rows, columns, accountForBack);

            int newPos = itemPocketIndex;
            int[] curPos = GetBagPocketXYPos(itemPockets[itemPocketIndex], map);
            if (curPos[0] != -1)
            {
                int newRow = (curPos[0] + scrollY) % rows;
                if (newRow < 0) newRow += rows;

                int newColumn = (curPos[1] + scrollX) % columns;
                if (newColumn < 0) newColumn += columns;

                ItemBattlePocket nextChoice = map[newRow, newColumn];
                newPos = itemPockets.IndexOf(nextChoice);
            }
            NavigateBagPocketMenu(newPos - itemPocketIndex);
        }
    }
    private ItemBattlePocket[,] GetBagPocketMap(int rows, int columns, bool accountForBack)
    {
        ItemBattlePocket[,] map = new ItemBattlePocket[rows, columns];
        int startChoice = accountForBack ? 1 : 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i == 0 && accountForBack)
                {
                    map[i, j] = ItemBattlePocket.None;
                }
                else
                {
                    map[i, j] = itemPockets[startChoice];
                    startChoice++;
                }
            }
        }
        return map;
    }
    private int[] GetBagPocketXYPos(ItemBattlePocket commandType, ItemBattlePocket[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == commandType)
                {
                    return new int[] { i, j };
                }
            }
        }

        return new int[] { -1, -1 };
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
            view.battleUI.SwitchSelectedBagPocketTo(itemPockets[itemPocketIndex]);
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
        ItemBattlePocket pocketType = itemPockets[itemPocketIndex];

        // Exit Commands
        if (pocketType == ItemBattlePocket.None)
        {
            CancelBagPocketMenu();
        }
        else
        {
            controlBagCR = StartCoroutine(ControlPromptBag(
                commandPokemon,
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

            //Legacy_SwitchControlContext(ControlContext.Command);
            SwitchControlContext(ControlContext.Command);
            view.battleUI.SwitchPanel(BTLUI_Base.Panel.Command);
            view.battleUI.SwitchSelectedCommandTo(commandTypes[commandIndex]);
        }
    }



    // BAG MENU
    private void NavigateBagMenuQuad(InputAction.CallbackContext obj)
    {
        int scrollX = Mathf.RoundToInt(obj.ReadValue<Vector2>().x);
        int scrollY = -Mathf.RoundToInt(obj.ReadValue<Vector2>().y);
        NavigateBagMenu(scrollX, scrollY);
    }
    private void NavigateBagMenuNew(int scrollX, int scrollY)
    {
        if (scrollX != 0 || scrollY != 0)
        {
            int maxItemCount = view.battleUI.bagItemPanel.maxItemCount;
            int itemCount = 0;
            int curIndex = itemIndex;

            List<Item> selectableItems = new List<Item>();
            selectableItems.Add(null);
            for (int i = itemOffset; i < curIndex + maxItemCount && i < itemSlots.Count; i++)
            {
                selectableItems.Add(itemSlots[i]);
            }

            bool accountForBack = selectableItems.Contains(null);
            int rows = ((selectableItems.Count + (accountForBack ? -1 : 0)) / 2) + (accountForBack ? 1 : 0);
            int columns = 2;
            Item[,] map = GetBagItemMap(rows, columns, accountForBack, selectableItems);

            int newPos = itemIndex;
            int[] curPos = GetBagItemXYPos(itemSlots[curIndex], map);
            if (curPos[0] != -1)
            {
                int newRow = (curPos[0] + scrollY) % rows;
                if (newRow < 0) newRow += rows;

                int newColumn = (curPos[1] + scrollX) % columns;
                if (newColumn < 0) newColumn += columns;

                Item nxtSlot = map[newRow, newColumn];
                newPos = itemSlots.IndexOf(nxtSlot);
            }
            NavigateFightMenu(newPos - curIndex);
        }
    }
    private Item[,] GetBagItemMap(int rows, int columns, bool accountForBack, List<Item> list)
    {
        Item[,] map = new Item[rows, columns];
        int start = accountForBack ? 1 : 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (i == 0 && accountForBack)
                {
                    map[i, j] = null;
                }
                else
                {
                    if (start >= list.Count)
                    {
                        map[i, j] = null;
                    }
                    else
                    {
                        map[i, j] = list[start];
                    }
                    start++;
                }
            }
        }
        return map;
    }
    private int[] GetBagItemXYPos(Item moveslot, Item[,] map)
    {
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j] == moveslot)
                {
                    return new int[] { i, j };
                }
            }
        }

        return new int[] { -1, -1 };
    }

    private void NavigateBagMenu(int scrollAmount)
    {
        if (AreControlsLocked())
        {
            return;
        }
        if (scrollAmount != 0)
        {
            itemIndex += scrollAmount;
            itemIndex %= itemSlots.Count;
            if (itemIndex < 0)
            {
                itemIndex += itemSlots.Count;
            }
            view.battleUI.SwitchSelectedItemTo(itemSlots[itemOffset + itemIndex]);
        }
    }
    private void NavigateBagMenuScroll(bool scrollRight)
    {
        if (AreControlsLocked())
        {
            return;
        }

    }

    private void NavigateBagMenu(int scrollX, int scrollY)
    {
        if (AreControlsLocked())
        {
            return;
        }

        // Scroll pages
        int maxItemCount = view.battleUI.bagItemPanel.maxItemCount;
        if (scrollX != 0
            && itemIndex != maxItemCount)
        {
            int preOffset = itemOffset;
            itemOffset += maxItemCount * scrollX;
            if (itemOffset + Mathf.Min(itemIndex, maxItemCount) >= itemSlots.Count)
            {
                itemIndex = 0;
                if (itemOffset >= itemSlots.Count)
                {
                    itemOffset = 0;
                }
            }
            else if (itemOffset < 0)
            {
                itemOffset = itemSlots.Count - (itemSlots.Count % maxItemCount);
                if (itemOffset + Mathf.Min(itemIndex, maxItemCount) >= itemSlots.Count)
                {
                    itemIndex = 0;
                }
            }
            if (preOffset != itemOffset)
            {
                view.battleUI.SetItems(commandTrainer, itemPockets[itemPocketIndex], itemSlots, itemOffset);

                if (itemIndex == maxItemCount)
                {
                    view.battleUI.SwitchSelectedItemTo(null);
                }
                else
                {
                    view.battleUI.SwitchSelectedItemTo(itemSlots[itemOffset + itemIndex]);
                }
            }
        }
        else if (scrollY != 0)
        {
            if (itemIndex == maxItemCount)
            {
                itemIndex = (scrollY > 0) ? 0 : maxItemCount - 1;
            }
            else
            {
                itemIndex += scrollY;
            }
            if (itemIndex == -1
                || itemIndex == maxItemCount
                || itemOffset + itemIndex == itemSlots.Count)
            {
                itemIndex = maxItemCount;
                view.battleUI.SwitchSelectedItemTo(null);
            }
            else
            {
                itemIndex %= maxItemCount;
                if (itemIndex < 0)
                {
                    itemIndex += maxItemCount;
                }
                if (itemOffset + itemIndex >= itemSlots.Count)
                {
                    itemIndex = (itemSlots.Count % maxItemCount) - 1;
                }
                view.battleUI.SwitchSelectedItemTo(itemSlots[itemOffset + itemIndex]);
            }
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
        Item choice;
        int maxItemCount = view.battleUI.bagItemPanel.maxItemCount;
        if (itemIndex == maxItemCount)
        {
            choice = null;
        }
        else
        {
            choice = itemSlots[itemOffset + itemIndex];
        }

        // exit to bag pockets
        if (choice == null)
        {
            CancelBagMenu();
        }
        // choose the pokemon to use the item on
        else
        {
            selectedItem = choice;
            controlPartyCR = StartCoroutine(ControlPromptParty(
                commandPokemon,
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

        //Legacy_SwitchControlContext(ControlContext.BagPocket);
        SwitchControlContext(ControlContext.BagPocket);
        view.battleUI.SwitchPanel(BTLUI_Base.Panel.Bag);
        view.battleUI.SwitchSelectedBagPocketTo(itemPockets[itemPocketIndex]);
    }

    private IEnumerator AttemptCommand(
        BattleCommand attemptedCommand, 
        BattleCommand[] setCommands,
        Action<bool> callback)
    {
        bool bypassChecks = false;
        bool commandSuccess = true;
        Pokemon userPokemon = attemptedCommand.commandUser;

        // move commands
        if (attemptedCommand.commandType == BattleCommandType.Fight)
        {
            // auto-success for struggle
            MoveData moveData = MoveDatabase.instance.GetMoveData(attemptedCommand.moveID);
            if (moveData.GetEffect(MoveEffectType.Struggle) != null)
            {
                bypassChecks = true;
            }

            string gameText = null;

            if (!bypassChecks)
            {
                // Can't Mega-Evolve / Z-Move / Dynamax twice
                if (commandSuccess)
                {
                    for (int i = 0; i < setCommands.Length; i++)
                    {
                        if (setCommands[i] != null)
                        {
                            if (setCommands[i].isMegaEvolving && attemptedCommand.isMegaEvolving)
                            {
                                gameText = "You are already Mega-Evolving another Pokémon!";
                                commandSuccess = false;
                                break;
                            }
                            else if (setCommands[i].isZMove && attemptedCommand.isZMove)
                            {
                                gameText = "You are already using a Z-Move!";
                                commandSuccess = false;
                                break;
                            }
                            else if (setCommands[i].isDynamaxing && attemptedCommand.isDynamaxing)
                            {
                                gameText = "You are already Dynamaxing with another Pokémon!";
                                commandSuccess = false;
                                break;
                            }
                        }
                    }
                }

                // Imprison
                if (commandSuccess)
                {
                    Pokemon imprisonPokemon = battleModel.PBPGetImprison(userPokemon, moveData);
                    if (imprisonPokemon != null)
                    {
                        commandSuccess = false;
                        BTLEvent_GameText textEvent = new BTLEvent_GameText();
                        textEvent.SetCloneModel(battleModel);
                        textEvent.Create(
                            textID: imprisonPokemon.bProps.imprison.chooseText,
                            userPokemon: userPokemon
                            );
                        gameText = view.GetGameText(textEvent);
                    }
                }

                // Choiced Moves
                if (commandSuccess)
                {
                    if (!string.IsNullOrEmpty(userPokemon.bProps.choiceMove)
                        && battleModel.IsPokemonChoiced(userPokemon))
                    {
                        bool bypassChoiceLock = moveData.HasTag(MoveTag.IgnoreChoiceLock);
                        MoveData choiceMoveData = MoveDatabase.instance.GetMoveData(userPokemon.bProps.choiceMove);
                        if (!bypassChoiceLock)
                        {
                            commandSuccess = false;
                            if (choiceMoveData.ID != moveData.ID)
                            {
                                BTLEvent_GameText textEvent = new BTLEvent_GameText();
                                textEvent.battleModel = battleModel;
                                textEvent.Create(
                                    textID: "pokemon-choiced",
                                    userPokemon: userPokemon,
                                    moveID: choiceMoveData.ID
                                    );
                                gameText = view.GetGameText(textEvent);
                            }
                        }
                    }
                }

                // Limited Moves
                if (commandSuccess)
                {
                    for (int i = 0; i < userPokemon.bProps.moveLimiters.Count && commandSuccess; i++)
                    {
                        Pokemon.BattleProperties.MoveLimiter moveLimiter =
                            userPokemon.bProps.moveLimiters[i];
                        EffectDatabase.StatusPKEff.MoveLimiting effect_ = moveLimiter.effect;

                        string moveLimitID = moveData.ID;
                        
                        // Disable
                        if (effect_ is EffectDatabase.StatusPKEff.Disable)
                        {
                            if (moveLimiter.affectedMoves.Contains(moveData.ID))
                            {
                                commandSuccess = false;
                            }
                        }
                        // Encore
                        else if (effect_ is EffectDatabase.StatusPKEff.Encore)
                        {
                            if (!moveLimiter.affectedMoves.Contains(moveData.ID))
                            {
                                commandSuccess = false;
                                moveLimitID = moveLimiter.GetMove();
                            }
                        }
                        // Heal Block
                        else if (effect_ is EffectDatabase.StatusPKEff.HealBlock)
                        {
                            if (battleModel.IsHealingMove(moveData))
                            {
                                commandSuccess = false;
                            }
                        }
                        // Taunt
                        else if (effect_ is EffectDatabase.StatusPKEff.Taunt)
                        {
                            EffectDatabase.StatusPKEff.Taunt taunt = effect_ as EffectDatabase.StatusPKEff.Taunt;
                            if (taunt.category == MoveCategory.Status)
                            {
                                commandSuccess = false;
                            }
                        }
                        // Torment
                        else if (effect_ is EffectDatabase.StatusPKEff.Torment)
                        {
                            if (moveLimiter.affectedMoves.Contains(moveData.ID))
                            {
                                commandSuccess = false;
                            }
                        }

                        if (!commandSuccess)
                        {
                            BTLEvent_GameText textEvent = new BTLEvent_GameText();
                            textEvent.battleModel = battleModel;
                            textEvent.Create(
                                textID: effect_.attemptText,
                                userPokemon: userPokemon,
                                moveID: moveLimitID
                                );
                            gameText = view.GetGameText(textEvent);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(gameText))
            {
                yield return StartCoroutine(view.battleUI.DrawText(gameText, undrawOnFinish: true));
            }
        }
        // party commands
        else if (attemptedCommand.commandType == BattleCommandType.Party
            || attemptedCommand.commandType == BattleCommandType.PartyReplace)
        {
            string gameText = null;

            // check if switching is possible

            // party-specfic restrictions
            if (commandSuccess
                && attemptedCommand.commandType == BattleCommandType.Party)
            {
                // Trapped
                if (commandSuccess)
                {
                    // Block
                    if (commandSuccess && !string.IsNullOrEmpty(userPokemon.bProps.blockMove))
                    {
                        Pokemon blockUser = battleModel.GetFieldPokemonByID(userPokemon.bProps.blockPokemon);
                        MoveData moveData = MoveDatabase.instance.GetMoveData(userPokemon.bProps.blockMove);
                        MoveEffect effect = moveData.GetEffect(MoveEffectType.Block);

                        string textCodeID = effect.GetString(2);
                        textCodeID = (textCodeID == "DEFAULT") ? "move-block-trap-default"
                            : textCodeID;

                        BTLEvent_GameText textEvent = new BTLEvent_GameText();
                        textEvent.SetBattleModel(battleModel);
                        textEvent.Create(
                            textID: textCodeID,
                            userPokemon: blockUser,
                            targetPokemon: userPokemon,
                            moveID: userPokemon.bProps.blockMove
                            );

                        gameText = view.GetGameText(textEvent);
                        commandSuccess = false;
                    }

                    // Bind
                    if (commandSuccess && !string.IsNullOrEmpty(userPokemon.bProps.bindMove))
                    {
                        Pokemon bindUser = battleModel.GetFieldPokemonByID(userPokemon.bProps.bindPokemon);
                        MoveData moveData = MoveDatabase.instance.GetMoveData(userPokemon.bProps.bindMove);
                        MoveEffect effect = moveData.GetEffect(MoveEffectType.Bind);

                        string textCodeID = effect.GetString(3);
                        textCodeID = (textCodeID == "DEFAULT") ? "move-bind-trap-default"
                            : textCodeID;

                        BTLEvent_GameText textEvent = new BTLEvent_GameText();
                        textEvent.SetBattleModel(battleModel);
                        textEvent.Create(
                            textID: textCodeID,
                            userPokemon: bindUser,
                            targetPokemon: userPokemon,
                            moveID: userPokemon.bProps.bindMove
                            );

                        gameText = view.GetGameText(textEvent);
                        commandSuccess = false;
                    }

                    // Sky Drop
                    if (commandSuccess && !string.IsNullOrEmpty(userPokemon.bProps.skyDropMove))
                    {
                        Pokemon trapUser = battleModel.GetFieldPokemonByID(userPokemon.bProps.skyDropUser);
                        MoveData moveData = MoveDatabase.instance.GetMoveData(userPokemon.bProps.skyDropMove);
                        MoveEffect effect = moveData.GetEffect(MoveEffectType.SkyDrop);

                        string textCodeID = effect.GetString(2);
                        textCodeID = (textCodeID == "DEFAULT") ? "move-skydrop-trap-default"
                            : textCodeID;

                        BTLEvent_GameText textEvent = new BTLEvent_GameText();
                        textEvent.SetBattleModel(battleModel);
                        textEvent.Create(
                            textID: textCodeID,
                            userPokemon: trapUser,
                            targetPokemon: userPokemon,
                            moveID: userPokemon.bProps.skyDropMove
                            );

                        gameText = view.GetGameText(textEvent);
                        commandSuccess = false;
                    }

                    // Ingrain
                    if (commandSuccess)
                    {
                        for (int i = 0; i < userPokemon.bProps.ingrainMoves.Count; i++)
                        {
                            MoveData ingrainData = MoveDatabase.instance.GetMoveData(
                                userPokemon.bProps.ingrainMoves[i]
                                );
                            MoveEffect ingrainEffect = ingrainData.GetEffect(MoveEffectType.Ingrain);
                            if (ingrainEffect.GetBool(0))
                            {
                                BTLEvent_GameText textEvent = new BTLEvent_GameText();
                                textEvent.battleModel = battleModel;
                                textEvent.Create(
                                    textID: "pokemon-switch-ingrain",
                                    userPokemon: userPokemon,
                                    moveID: ingrainData.ID
                                    );

                                gameText = view.GetGameText(textEvent);
                                commandSuccess = false;
                                break;
                            }
                        }
                    }
                }

                // TODO: Trapping Abilities (Arena Trap, Shadow Tag, etc.)
                if (commandSuccess)
                {
                    for (int i = 0; i < battleModel.pokemonOnField.Count && commandSuccess; i++)
                    {
                        Pokemon trapPokemon = battleModel.pokemonOnField[i];
                        AbilityData abilityData = 
                            battleModel.PBPGetAbilityDataWithEffect(trapPokemon, AbilityEffectType.ShadowTag);
                        if (!trapPokemon.IsTheSameAs(userPokemon) && abilityData != null)
                        {
                            EffectDatabase.AbilityEff.AbilityEffect shadowTag_ = 
                                abilityData.GetEffectNew(AbilityEffectType.ShadowTag);
                            EffectDatabase.AbilityEff.ShadowTag shadowTag = 
                                shadowTag_ as EffectDatabase.AbilityEff.ShadowTag;

                            if (battleModel.DoEffectFiltersPass(
                                filters: shadowTag.filters,
                                userPokemon: trapPokemon,
                                targetPokemon: userPokemon,
                                abilityData: abilityData
                                ))
                            {
                                bool trapped = true;
                                
                                // Shed Shell
                                if (trapped)
                                {
                                    EffectDatabase.ItemEff.ItemEffect shedShell_ =
                                        battleModel.PBPGetItemEffect(userPokemon, ItemEffectType.ShedShell);
                                    if (shedShell_ != null)
                                    {
                                        EffectDatabase.ItemEff.ShedShell shedShell = 
                                            shedShell_ as EffectDatabase.ItemEff.ShedShell;
                                        trapped = false;
                                    }
                                }

                                // Immune to own ability
                                if (trapped 
                                    && shadowTag.immuneToSelf)
                                {
                                    AbilityData abilityData2 = battleModel.PBPGetAbilityDataWithEffect(
                                        userPokemon, 
                                        AbilityEffectType.ShadowTag);
                                    if (abilityData2 != null)
                                    {
                                        if (abilityData.ID == abilityData2.ID)
                                        {
                                            trapped = false;
                                        }
                                    }
                                }

                                // Arena Trap
                                if (trapped 
                                    && shadowTag.arenaTrap 
                                    && !battleModel.PBPIsPokemonGrounded(userPokemon))
                                {
                                    trapped = false;
                                }

                                // Must be adjacent
                                if (trapped
                                    && shadowTag.mustBeAdjacent 
                                    && !battleModel.ArePokemonAdjacent(trapPokemon, userPokemon))
                                {
                                    trapped = false;
                                }
                                
                                if (trapped)
                                {
                                    commandSuccess = false;
                                    BTLEvent_GameText textEvent = new BTLEvent_GameText();
                                    textEvent.SetBattleModel(battleModel);
                                    textEvent.Create(
                                        textID: shadowTag.displayText,
                                        userPokemon: trapPokemon,
                                        targetPokemon: userPokemon,
                                        abilityID: abilityData.ID
                                        );
                                    gameText = view.GetGameText(textEvent);
                                }
                            }
                        }
                    }
                }

            }

            // can't switch in fainted pokemon
            if (commandSuccess
                && battleModel.IsPokemonFainted(attemptedCommand.switchInPokemon))
            {
                gameText = attemptedCommand.switchInPokemon.nickname + " is unable to battle!";
                commandSuccess = false;
            }

            // can't switch in on-field pokemon
            if (commandSuccess 
                && battleModel.IsPokemonOnField(attemptedCommand.switchInPokemon))
            {
                gameText = attemptedCommand.switchInPokemon.nickname + " is already in battle!";
                commandSuccess = false;
            }

            // can't switch in a pokemon already committed to switch in
            if (commandSuccess)
            {
                // can't switch in a pokemon already committed to switch in
                for (int i = 0; i < setCommands.Length; i++)
                {
                    if (setCommands[i] != null)
                    {
                        Pokemon otherPokemon = setCommands[i].switchInPokemon;
                        if (otherPokemon != null)
                        {
                            if (otherPokemon.IsTheSameAs(attemptedCommand.switchInPokemon))
                            {
                                commandSuccess = false;
                                gameText = attemptedCommand.switchInPokemon.nickname + " is already switching in!";
                                break;
                            }
                        }
                    }
                }
            }
        
            if (!string.IsNullOrEmpty(gameText))
            {
                yield return StartCoroutine(view.battleUI.DrawTextInstantNoWait(
                    text: gameText,
                    textBox: view.battleUI.partyPanel.promptText));
            }
        }
        // bag commands
        else if (attemptedCommand.commandType == BattleCommandType.Bag)
        {
            Pokemon itemPokemon = attemptedCommand.commandUser;
            Trainer itemTrainer = attemptedCommand.itemTrainer;
            ItemData itemData = ItemDatabase.instance.GetItemData(attemptedCommand.itemID);

            BTLEvent_GameText textEvent = new BTLEvent_GameText();
            textEvent.SetCloneModel(battleModel);
            string textID = "item-use-fail";
            string gameText = null;

            // Make sure the trainer has enough of the items in their inventory
            if (commandSuccess)
            {
                int itemCount = itemTrainer.GetItemCount(itemData.ID);
                for (int i = 0; i < setCommands.Length; i++)
                {
                    if (setCommands[i] != null)
                    {
                        BattleCommand itemCommand = setCommands[i];
                        if (itemCommand.itemID == itemData.ID)
                        {
                            itemCount--;
                        }
                    }
                }

                if (itemCount <= 0)
                {
                    commandSuccess = false;
                    textID = "item-use-fail-notenough";
                    textEvent.Create(
                        textID: textID,
                        userPokemon: itemPokemon,
                        itemID: itemData.ID
                        );

                    gameText = view.GetGameText(textEvent);
                }
            }

            // Fainted?
            if (commandSuccess)
            {
                if (battleModel.IsPokemonFainted(itemPokemon))
                {
                    // check items that can be used on fainted pokemon
                    if (itemData.GetEffect(ItemEffectType.Revive) == null)
                    {
                        commandSuccess = false;
                        textEvent.Create(
                            textID: textID,
                            userPokemon: itemPokemon,
                            itemID: itemData.ID
                            );

                        gameText = view.GetGameText(textEvent);
                    }
                }
                else
                {
                    // check items that can only be used on fainted pokemon
                    if (itemData.GetEffect(ItemEffectType.Revive) != null)
                    {
                        commandSuccess = false;
                        textEvent.Create(
                            textID: textID,
                            userPokemon: itemPokemon,
                            itemID: itemData.ID
                            );

                       gameText = view.GetGameText(textEvent);
                    }
                }
            }

            // Embargo
            if (commandSuccess)
            {
                if (itemPokemon.bProps.embargo != null && !itemData.HasTag(ItemTag.BypassEmbargo))
                {
                    commandSuccess = false;

                    textEvent.Create(
                        textID: itemPokemon.bProps.embargo.effect.attemptText,
                        userPokemon: itemPokemon,
                        itemID: itemData.ID
                        );

                    gameText = view.GetGameText(textEvent);
                }
            }

            // Only useable in battle
            if (commandSuccess)
            {
                if (itemData.HasTag(ItemTag.OnlyUseableInBattle)
                    && !battleModel.IsPokemonOnField(itemPokemon))
                {
                    commandSuccess = false;

                    textID = "item-use-fail-battle";
                    textEvent.Create(
                        textID: textID,
                        userPokemon: itemPokemon,
                        itemID: itemData.ID
                        );
                    gameText = view.GetGameText(textEvent);
                }
            }

            if (!string.IsNullOrEmpty(gameText))
            {
                yield return StartCoroutine(view.battleUI.DrawTextInstantNoWait(
                        text: gameText,
                        textBox: view.battleUI.partyPanel.promptText));
            }
        }
        // run commands
        else if (attemptedCommand.commandType == BattleCommandType.Run)
        {
            BTLEvent_GameText textEvent = new BTLEvent_GameText();
            textEvent.SetCloneModel(battleModel);
            textEvent.Create(
                textID: "pokemon-run-trap",
                userPokemon: userPokemon
                );
            string gameText = null;

            // Trapped
            if (commandSuccess)
            {
                EffectDatabase.AbilityEff.AbilityEffect runAway_ = 
                    battleModel.PBPGetAbilityEffect(userPokemon, AbilityEffectType.RunAway);
                EffectDatabase.AbilityEff.RunAway runAway = (runAway_ == null) ? null :
                    runAway_ as EffectDatabase.AbilityEff.RunAway;

                // trainer battle
                if (!battleModel.battleSettings.isWildBattle)
                {
                    textEvent.Create(
                        textID: "bpc-run-trainer"
                        );
                    gameText = view.GetGameText(textEvent);
                    commandSuccess = false;
                }

                // Block
                if (commandSuccess 
                    && !string.IsNullOrEmpty(userPokemon.bProps.blockMove)
                    && runAway == null)
                {
                    gameText = view.GetGameText(textEvent);
                    commandSuccess = false;
                }

                // Bind
                if (commandSuccess 
                    && !string.IsNullOrEmpty(userPokemon.bProps.bindMove)
                    && runAway == null)
                {
                    gameText = view.GetGameText(textEvent);
                    commandSuccess = false;
                }

                // Sky Drop
                if (commandSuccess && !string.IsNullOrEmpty(userPokemon.bProps.skyDropMove))
                {
                    gameText = view.GetGameText(textEvent);
                    commandSuccess = false;
                }

                // Trapped
                StatusCondition trapStatus = battleModel.GetTrapStatusCondition(attemptedCommand.commandUser);
                if (commandSuccess 
                    && trapStatus != null
                    && runAway == null)
                {
                    gameText = view.GetGameText(textEvent);
                    commandSuccess = false;
                }

                // Ingrain
                if (commandSuccess && runAway == null)
                {
                    for (int i = 0; i < userPokemon.bProps.ingrainMoves.Count; i++)
                    {
                        MoveData ingrainData = MoveDatabase.instance.GetMoveData(
                            userPokemon.bProps.ingrainMoves[i]
                            );
                        MoveEffect ingrainEffect = ingrainData.GetEffect(MoveEffectType.Ingrain);
                        if (ingrainEffect.GetBool(0))
                        {
                            textEvent.Create(
                                textID: "pokemon-run-ingrain",
                                userPokemon: userPokemon,
                                moveID: ingrainData.ID
                                );

                            gameText = view.GetGameText(textEvent);
                            commandSuccess = false;
                            break;
                        }
                    }
                }
            }

            // TODO: Trapping Abilities (Arena Trap, Shadow Tag, etc.)
        
            if (!string.IsNullOrEmpty(gameText))
            {
                yield return StartCoroutine(view.battleUI.DrawText(gameText, undrawOnFinish: true));
            }
        }
        callback(commandSuccess);
    }

    // Debug Controls
    public void SetDebugControls()
    {
        _controls.Debug.DisplayBattle.performed += (obj) => 
        {
            if (battleModel != null)
            {
                view.RedrawModel();
            }
        };
    }




}

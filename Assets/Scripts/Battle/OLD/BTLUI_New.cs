using PBS.Main.Pokemon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUI_New : BTLUI_Base
{
    [Header("Panels")]
    public BTLUIPanelCommand cmdPanel;
    public BTLUIPanelFight fightPanel;
    public BTLUIPanelFieldTarget fieldTargetPanel;
    public BTLUIPanelParty partyPanel;
    public BTLUIPanelBag bagPanel;
    public BTLUIPanelBagItem bagItemPanel;
    public BTLUIPanelHUD HUDPanel;

    [HideInInspector] public BTLUIPanel currentPanel;

    protected override void Awake()
    {
        base.Awake();
        currentPanel = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        panelType = Panel.None;
        SwitchPanel(panelType);
        UnsetPanels();
        dialog.gameObject.SetActive(true);
        dialog.ClearBox();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Model Management
    public override void UpdateModel(Battle battle)
    {
        battleModel = battle;
    }

    // Panel Management
    public override void SwitchPanel(Panel newPanel)
    {
        panelType = newPanel;
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(false);
        }
        
        switch (newPanel)
        {
            case Panel.Command:
                currentPanel = cmdPanel;
                break;

            case Panel.Fight:
                currentPanel = fightPanel;
                break;

            case Panel.FieldTargeting:
                currentPanel = fieldTargetPanel;
                break;

            case Panel.Party:
                currentPanel = partyPanel;
                partyPanel.commandPanel.gameObject.SetActive(false);
                break;

            case Panel.PartyCommand:
                currentPanel = partyPanel;
                partyPanel.commandPanel.gameObject.SetActive(true);
                break;

            case Panel.Bag:
                currentPanel = bagPanel;
                break;

            case Panel.BagItem:
                currentPanel = bagItemPanel;
                break;

            default:
                currentPanel = null;
                break;
        }
    
        if (currentPanel != null)
        {
            currentPanel.gameObject.SetActive(true);
        }

        // HUD (only on None or Fight Panels)
        HUDPanel.gameObject.SetActive(
            panelType == Panel.None 
            || panelType == Panel.Command);
    }
    public override void UnsetPanels()
    {
        base.UnsetPanels();
        cmdPanel.gameObject.SetActive(false);
        fightPanel.gameObject.SetActive(false);
        fieldTargetPanel.gameObject.SetActive(false);
        partyPanel.gameObject.SetActive(false);
        bagPanel.gameObject.SetActive(false);
        SwitchPanel(Panel.None);
    }

    // Command Panel
    public override void SetCommands(Pokemon pokemon, IEnumerable<BattleCommandType> commandList)
    {
        HashSet<BattleCommandType> commandSet = new HashSet<BattleCommandType>(commandList);
        cmdPanel.SetCommands(pokemon, commandList);
    }
    public override void SwitchSelectedCommandTo(BattleCommandType commandType)
    {
        cmdPanel.HighlightCommand(commandType);
    }

    // Fight Panel
    public override void SetMoves(
        Pokemon pokemon, 
        List<Moveslot> moveslots, 
        bool canMegaEvolve, bool canZMove = false, bool canDynamax = false,
        bool choosingZMove = false, bool choosingMaxMove = false)
    {
        List<Moveslot> filteredMoveslots = new List<Moveslot>(moveslots);
        for (int i = 0; i < filteredMoveslots.Count; i++)
        {
            Moveslot moveslot = filteredMoveslots[i];
            if (moveslot == null)
            {
                filteredMoveslots.RemoveAt(i);
                i--;
            }
        }
        fightPanel.SetMoves(
            battle: battleModel,
            pokemon: pokemon,
            moveList: filteredMoveslots,
            canMegaEvolve: canMegaEvolve, canZMove: canZMove, canDynamax: canDynamax,
            choosingZMove: choosingZMove, choosingMaxMove: choosingMaxMove);
    }
    public override void SwitchSelectedMoveTo(
        Pokemon pokemon, 
        Moveslot selected, 
        bool choosingSpecial, bool choosingZMove, bool choosingMaxMove)
    {
        if (selected == null)
        {
            fightPanel.HighlightBackButton();
        }
        else
        {
            fightPanel.HighlightMove(selected);
        }

        if (choosingSpecial || choosingZMove || choosingMaxMove)
        {
            fightPanel.specialBtn.SelectSelf();
        }
        else
        {
            fightPanel.specialBtn.UnselectSelf();
        }
    }

    // Field Targeting Panel
    public override void SetFieldTargets(int teamPos)
    {
        fieldTargetPanel.SetFieldTargets(teamPos: teamPos, battleModel: battleModel);
    }
    public override void SwitchSelectedMoveTargetsTo(
        BattlePosition userPos,
        int chooseIndex,
        List<List<BattlePosition>> choices)
    {
        List<BattlePosition> choice = choices[chooseIndex];
        if (choice == null)
        {
            fieldTargetPanel.HighlightBackButton();
        }
        else
        {
            fieldTargetPanel.HighlightFieldTargets(userPos, choice);
        }
    }

    // Party Panel
    public override void SetParty(List<Pokemon> pokemon, bool forceSwitch = false, Item item = null)
    {
        List<Pokemon> filteredPokemon = new List<Pokemon>();
        for (int i = 0; i < pokemon.Count; i++)
        {
            if (pokemon[i] != null)
            {
                filteredPokemon.Add(pokemon[i]);
            }
        }

        partyPanel.SetParty(party: filteredPokemon, item: item);
        partyPanel.backBtn.gameObject.SetActive(pokemon.Contains(null));
    }
    public override void SwitchSelectedPartyMemberTo(Pokemon selected)
    {
        if (selected == null)
        {
            partyPanel.HighlightBackButton();
        }
        else
        {
            partyPanel.HighlightPokemon(selected);
        }
    }
    // Party Commands
    public override void SetPartyCommands(Pokemon pokemon, List<BattleExtraCommand> commands)
    {
        partyPanel.SetCommands(commands);
    }
    public override void SwitchSelectedPartyCommandTo(BattleExtraCommand selected)
    {
        partyPanel.HighlightCommand(selected);
    }

    // Bag Panel
    public override void SetBagPockets(List<ItemBattlePocket> list)
    {
        bagPanel.SetPockets(list);
    }
    public override void SwitchSelectedBagPocketTo(ItemBattlePocket selected)
    {
        if (selected == ItemBattlePocket.None)
        {
            bagPanel.HighlightBackButton();
        }
        else
        {
            bagPanel.HighlightPocket(selected);
        }
    }

    // Bag Item Panel
    public override void SetItems(Trainer trainer, ItemBattlePocket pocket, List<Item> list, int offset)
    {
        List<Item> filteredItems = new List<Item>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                filteredItems.Add(list[i]);
            }
        }
        bagItemPanel.SetItems(trainer, filteredItems, offset);
        bagItemPanel.backBtn.gameObject.SetActive(true);
    }
    public override void SwitchSelectedItemTo(Item selected)
    {
        if (selected == null)
        {
            bagItemPanel.HighlightBackButton();
        }
        else
        {
            bagItemPanel.HighlightButton(selected);
        }
    }

    // HUD
    public override BTLUI_PokemonHUD DrawPokemonHUD(Pokemon pokemon, Battle battle, bool isNear)
    {
        return HUDPanel.DrawPokemonHUD(pokemon, battle, isNear);
    }
    public override bool UndrawPokemonHUD(Pokemon pokemon)
    {
        return HUDPanel.UndrawPokemonHUD(pokemon);
    }

    public override BTLUI_PokemonHUD GetPokemonHUD(Pokemon pokemon)
    {
        return HUDPanel.GetPokemonHUD(pokemon);
    }
    public override void UpdatePokemonHUD(Pokemon pokemon, Battle battle)
    {
        HUDPanel.UpdatePokemonHUD(pokemon, battle);
    }

    public override void SetPokemonHUDActive(Pokemon pokemon, bool active)
    {
        HUDPanel.SetPokemonHUDActive(pokemon, active);
    }
    public override void SetPokemonHUDsActive(bool active)
    {
        HUDPanel.SetPokemonHUDsActive(active);
    }

    public override IEnumerator AnimatePokemonHUDHPChange(Pokemon pokemon, int preHP, int postHP, float timeSpan = 1f)
    {
        yield return StartCoroutine(HUDPanel.AnimatePokemonHUDHPChange(
            pokemon: pokemon,
            battleModel: battleModel,
            preHP: preHP, postHP: postHP,
            timeSpan: timeSpan
            ));
    }

    // Dialog
    public void UndrawDialogBox()
    {
        dialog.dialogBox.gameObject.SetActive(false);
        if (panelType == Panel.Command)
        {
            cmdPanel.gameObject.SetActive(true);
        }
        else if (panelType == Panel.Fight)
        {
            fightPanel.gameObject.SetActive(true);
        }
        HUDPanel.pokemonHUDNearRoot.gameObject.SetActive(true);
    }

    public IEnumerator DrawText(string text, bool undrawOnFinish = false, Text textBox = null, int lines = -1)
    {
        yield return StartCoroutine(DrawText(
            text: text,
            secPerChar: 1f / dialog.charPerSec,
            undrawOnFinish: undrawOnFinish,
            textBox: textBox,
            lines: lines));
    }
    public IEnumerator DrawTextNoWait(string text, bool undrawOnFinish = false, Text textBox = null, int lines = -1)
    {
        yield return StartCoroutine(DrawText(
            text: text,
            secPerChar: 1f / dialog.charPerSec,
            time: 0,
            undrawOnFinish: undrawOnFinish,
            textBox: textBox,
            lines: lines));
    }
    public IEnumerator DrawTextInstant(string text, bool undrawOnFinish = false, Text textBox = null, int lines = -1)
    {
        yield return StartCoroutine(DrawText(
            text: text,
            secPerChar: 0,
            undrawOnFinish: undrawOnFinish,
            textBox: textBox,
            lines: lines));
    }
    public IEnumerator DrawTextInstantNoWait(string text, bool undrawOnFinish = false, Text textBox = null, int lines = -1)
    {
        yield return StartCoroutine(DrawText(
            text: text,
            secPerChar: 0,
            time: 0,
            undrawOnFinish: undrawOnFinish,
            textBox: textBox,
            lines: lines));
    }
    public IEnumerator DrawText(string text, float time, float lockedTime, bool undrawOnFinish = false, Text textBox = null, int lines = -1)
    {
        yield return StartCoroutine(DrawText(
            text: text,
            secPerChar: 1f / dialog.charPerSec,
            time: time,
            lockedTime: lockedTime,
            silent: true,
            undrawOnFinish: undrawOnFinish,
            textBox: textBox,
            lines: lines));
    }
    public override IEnumerator DrawText(
        string text,
        float secPerChar,
        float time = 2f,
        float lockedTime = 0,
        bool silent = true,
        bool hold = false,
        bool undrawOnFinish = false,
        Text textBox = null,
        int lines = -1
        )
    {
        // Default Dialog Box
        if (textBox == null)
        {
            HUDPanel.pokemonHUDNearRoot.gameObject.SetActive(false);
        }

        yield return StartCoroutine(dialog.DrawText(
            text: text,
            secPerChar: secPerChar,
            time: time,
            lockedTime: lockedTime,
            silent: silent,
            hold: hold,
            undrawOnFinish: undrawOnFinish,
            textBox: textBox,
            lines: lines
            ));

        if (textBox == null && undrawOnFinish)
        {
            UndrawDialogBox();
        }
    }
}

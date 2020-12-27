using PBS.Main.Pokemon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUI_Base : MonoBehaviour
{
    public enum Panel
    {
        None,
        Command,
        Fight,
        FieldTargeting,
        Party,
        PartyCommand,
        Bag,
        BagItem,
    }
    
    [Header("Dialog")]
    public BTLUI_Dialog dialog;

    [Header("HUD")]
    public GameObject HUDRoot;

    [HideInInspector] public Panel panelType;
    public Battle battleModel;

    protected virtual void Awake()
    {
        panelType = Panel.None;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Model Management
    public virtual void UpdateModel(Battle battle) { }

    // Panel Management
    public virtual void SwitchPanel(Panel newPanel) { }
    public virtual void UnsetPanels() { }

    // Command Panel
    public virtual void SetCommands(Pokemon pokemon, IEnumerable<BattleCommandType> commandList) { }
    public virtual void SwitchSelectedCommandTo(BattleCommandType commandType) { }

    // Fight Panel
    public virtual void SetMoves(
        Pokemon pokemon,
        List<Moveslot> moveslots,
        bool canMegaEvolve, bool canZMove = false, bool canDynamax = false,
        bool choosingZMove = false, bool choosingMaxMove = false)
    { }
    public virtual void SwitchSelectedMoveTo(
        Pokemon pokemon,
        Moveslot selected,
        bool choosingSpecial, bool choosingZMove, bool choosingMaxMove)
    { }

    // Field Targeting Panel
    public virtual void SetFieldTargets(int teamPos) { }
    public virtual void SwitchSelectedMoveTargetsTo(
        BattlePosition userPos, 
        int chooseIndex, 
        List<List<BattlePosition>> choices)
    { }

    // Party Panel
    public virtual void SetParty(List<Pokemon> choices, bool forceSwitch, Item item = null) { }
    public virtual void SwitchSelectedPartyMemberTo(Pokemon selected) { }
    // Party Commands
    public virtual void SetPartyCommands(Pokemon pokemon, List<BattleExtraCommand> commands) { }
    public virtual void SwitchSelectedPartyCommandTo(BattleExtraCommand selected) { }

    // Bag Panel
    public virtual void SetBagPockets(List<ItemBattlePocket> list) { }
    public virtual void SwitchSelectedBagPocketTo(ItemBattlePocket selected) { }

    // Bag Item Panel
    public virtual void SetItems(Trainer trainer, ItemBattlePocket pocket, List<Item> list, int offset) { }
    public virtual void SwitchSelectedItemTo(Item selected) { }

    // HUD
    public virtual BTLUI_PokemonHUD DrawPokemonHUD(Pokemon pokemon, Battle battle, bool isNear)
    {
        Debug.LogWarning("Unimplimented!");
        return null;
    }
    public virtual bool UndrawPokemonHUD(Pokemon pokemon)
    {
        Debug.LogWarning("Unimplimented!");
        return false;
    }
    public virtual BTLUI_PokemonHUD GetPokemonHUD(Pokemon pokemon)
    {
        Debug.LogWarning("Unimplimented!");
        return null;
    }
    public virtual void UpdatePokemonHUD(Pokemon pokemon, Battle battle) { }
    public virtual void SetPokemonHUDActive(Pokemon pokemon, bool active) { }
    public virtual void SetPokemonHUDsActive(bool active) { }
    public virtual IEnumerator AnimatePokemonHUDHPChange(Pokemon pokemon, int preHP, int postHP, float timeSpan = 1f)
    {
        yield return null;
    }

    // Dialog
    public virtual IEnumerator DrawText(
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
        yield return null;
    }
}

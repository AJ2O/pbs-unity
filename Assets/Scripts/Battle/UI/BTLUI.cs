using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using System;

public class BTLUI : MonoBehaviour
{
    [Header("Components")]
    public BTLUI_Dialog dialog;

    [Header("Prefabs")]
    public BTLUI_ButtonTxt txtBtnPrefab;
    public BTLUI_ButtonCmd cmdBtnPrefab;
    public BTLUI_ButtonCmdExtra cmdExtraBtnPrefab;
    public BTLUI_ButtonFight fightBtnPrefab;
    public BTLUI_ButtonFieldTarget fieldTargetBtnPrefab;
    public BTLUI_ButtonParty partyBtnPrefab;
    public BTLUI_ButtonParty partyBtnBackPrefab;
    public BTLUI_ButtonItem itemBtnPrefab;
    public BTLUI_ButtonItem itemBtnBackPrefab;
    public BTLUI_PokemonHUD pokemonHUDPrefab;
    public BTLUI_PokemonHUD pokemonHUDPrefabSmall;

    [Header("Common")]
    public Color colorSel;
    public Color colorUnsel;
    public Color colorTxtSel;
    public Color colorTxtUnsel;

    [Header("Command Panel")]
    public GameObject cmdPanel;
    public GameObject cmdOptionObj;
    public Text cmdTxt;
    private List<BTLUI_ButtonCmd> cmdBtns;
    private List<BattleCommandType> cmdList;

    [Header("Fight Panel")]
    public GameObject fightPanel;
    public GameObject fightOptionObj;
    public Text fightTxt;
    public BTLUI_ButtonTxt fightSpecialBtn;
    public Image fightSpecialIcon;
    private List<BTLUI_ButtonFight> fightBtns;
    private List<Pokemon.Moveslot> fightList;
    public Sprite fightMegaIcon,
        fightZMoveIcon,
        fightDynamaxIcon;

    [Header("Field Targeting Panel")]
    public GameObject fieldTargetPanel;
    public GameObject fieldTargetOptionObjFar,
        fieldTargetOptionObjNear;
    public Text fieldTargetTxt;
    private List<BTLUI_ButtonFieldTarget> fieldTargetBtns;
    private List<BattlePosition> positionList;

    [Header("Party Panel")]
    public GameObject partyPanel;
    public GameObject partyOptionObj;
    public Text partyTxt;
    private List<BTLUI_ButtonParty> partyBtns;
    private List<Pokemon> partyList;

    [Header("Party Command Panel")]
    public GameObject partyCmdPanel;
    public GameObject partyCmdOptionObj;
    public Text partyCmdTxt;
    private List<BTLUI_ButtonCmdExtra> partyCmdBtns;

    [Header("Bag Pocket Panel")]
    public GameObject bagPocketPanel;
    public GameObject bagPocketOptionObj;
    public Text bagPocketTxt;
    private List<BTLUI_ButtonTxt> bagPocketBtns;
    private List<ItemBattlePocket> bagPocketList;

    [Header("Item Panel")]
    public GameObject itemPanel;
    public GameObject itemOptionObj;
    public int maxItemCount = 4;
    public Text itemPocketTxt,
        itemPageTxt,
        itemNameTxt,
        itemDescTxt;
    private List<BTLUI_ButtonItem> itemBtns;
    private List<Item> itemList;

    [Header("HUD Panel")]
    public GameObject HUDPanel;
    [Header("HUD Spawns")]
    public GameObject spawnNear;
    public GameObject spawnFar;
    public Transform 
        spawnNearSingle,
        spawnFarSingle,

        spawnNearDouble0,
        spawnNearDouble1,
        spawnFarDouble0,
        spawnFarDouble1,

        spawnNearTriple0,
        spawnNearTriple1,
        spawnNearTriple2,
        spawnFarTriple0,
        spawnFarTriple1,
        spawnFarTriple2
        ;

    // UI Objects
    [HideInInspector] public List<BTLUI_PokemonHUD> pokemonHUDs = new List<BTLUI_PokemonHUD>();

    public Coroutine dialogRoutine;
    public Battle battleModel;

    public enum Panel
    {
        None,
        Command,
        Fight,
        FieldTargeting,
        Party,
        PartyCommand,
        BagPocket,
        Bag,
    }
    [HideInInspector] public Panel panel;
    [HideInInspector] public GameObject curPanelObj;

    private void Awake()
    {
        curPanelObj = null;

        cmdBtns = new List<BTLUI_ButtonCmd>();
        cmdList = new List<BattleCommandType>();

        fightBtns = new List<BTLUI_ButtonFight>();
        fightList = new List<Pokemon.Moveslot>();

        fieldTargetBtns = new List<BTLUI_ButtonFieldTarget>();
        positionList = new List<BattlePosition>();

        partyBtns = new List<BTLUI_ButtonParty>();
        partyList = new List<Pokemon>();

        partyCmdBtns = new List<BTLUI_ButtonCmdExtra>();

        bagPocketBtns = new List<BTLUI_ButtonTxt>();
        bagPocketList = new List<ItemBattlePocket>();

        itemBtns = new List<BTLUI_ButtonItem>();
        itemList = new List<Item>();
    }

    // Start is called before the first frame update
    void Start()
    {
        panel = Panel.None;
        SwitchPanel(Panel.None);
        UnsetAllPanels();
        dialog.gameObject.SetActive(true);
    }
    
    // Model
    public void UpdateModel(Battle battle)
    {
        battleModel = battle;
    }

    // Panel Management
    public void SwitchPanel(Panel newPanel, bool unsetPrevious = false)
    {
        if (unsetPrevious)
        {
            UnsetPanel(panel);
        }
        panel = newPanel;
        dialog.UndrawBox();

        cmdPanel.SetActive(panel == Panel.Command);
        fightPanel.SetActive(panel == Panel.Fight);
        fieldTargetPanel.SetActive(panel == Panel.FieldTargeting);
        partyPanel.SetActive(panel == Panel.Party);
        partyCmdPanel.SetActive(panel == Panel.PartyCommand);
        bagPocketPanel.SetActive(panel == Panel.BagPocket);
        itemPanel.SetActive(panel == Panel.Bag);

        HUDPanel.SetActive(
            panel == Panel.None
            || panel == Panel.Command
            || panel == Panel.Fight
            || panel == Panel.BagPocket
            );

        switch (newPanel)
        {
            case Panel.None:
                curPanelObj = null;
                break;

            case Panel.Command:
                curPanelObj = cmdPanel;
                break;

            case Panel.Fight:
                curPanelObj = fightPanel;
                break;

            case Panel.FieldTargeting:
                curPanelObj = null;
                break;

            case Panel.Party:
                curPanelObj = partyPanel;
                break;

            case Panel.PartyCommand:
                curPanelObj = partyPanel;
                break;

            case Panel.BagPocket:
                curPanelObj = bagPocketPanel;
                break;

            case Panel.Bag:
                curPanelObj = partyPanel;
                break;

        }
    }
    public void SetPanelActive(bool active = true)
    {
        if (curPanelObj != null)
        {
            curPanelObj.gameObject.SetActive(active);
        }
    }
    public void UnsetPanel(Panel panelType)
    {
        switch (panelType)
        {
            case Panel.Command:
                for (int i = 0; i < cmdBtns.Count; i++)
                {
                    Destroy(cmdBtns[i].gameObject);
                }
                cmdBtns.Clear();
                cmdList.Clear();
                cmdPanel.SetActive(false);
                break;

            case Panel.Fight:
                for (int i = 0; i < fightBtns.Count; i++)
                {
                    Destroy(fightBtns[i].gameObject);
                }
                fightBtns.Clear();
                fightList.Clear();
                fightPanel.SetActive(false);
                break;

            case Panel.FieldTargeting:
                for (int i = 0; i < fieldTargetBtns.Count; i++)
                {
                    Destroy(fieldTargetBtns[i].gameObject);
                }
                fieldTargetBtns.Clear();
                positionList.Clear();
                fieldTargetPanel.SetActive(false);
                break;

            case Panel.Party:
                for (int i = 0; i < partyBtns.Count; i++)
                {
                    Destroy(partyBtns[i].gameObject);
                }
                partyBtns.Clear();
                partyList.Clear();
                partyPanel.SetActive(false);
                break;

            case Panel.PartyCommand:
                for (int i = 0; i < partyCmdBtns.Count; i++)
                {
                    Destroy(partyCmdBtns[i].gameObject);
                }
                partyCmdBtns.Clear();
                partyCmdPanel.SetActive(false);
                break;

            case Panel.BagPocket:
                for (int i = 0; i < bagPocketBtns.Count; i++)
                {
                    Destroy(bagPocketBtns[i].gameObject);
                }
                bagPocketBtns.Clear();
                bagPocketList.Clear();
                bagPocketPanel.SetActive(false);
                break;

            case Panel.Bag:
                ClearItems();
                itemPanel.SetActive(false);
                break;

        }


    }
    public void UnsetAllPanels()
    {
        UnsetPanel(Panel.Command);
        UnsetPanel(Panel.Fight);
        UnsetPanel(Panel.FieldTargeting);
        UnsetPanel(Panel.Party);
        UnsetPanel(Panel.BagPocket);
        UnsetPanel(Panel.Bag);
    }

    // GENERAL BUTTON
    private void SelectTxtBtn(BTLUI_ButtonTxt btn, bool select)
    {
        btn.image.color = select ? btn.colorSel : btn.colorUnsel;
        btn.txt.color = select ? colorTxtSel : colorTxtUnsel;
    }


    // COMMAND
    public void SetCommands(Pokemon pokemon, List<BattleCommandType> commands)
    {
        cmdBtns = new List<BTLUI_ButtonCmd>();
        cmdList = new List<BattleCommandType>(commands);

        int realChoices = 0;
        for (int i = 0; i < cmdList.Count; i++)
        {
            BTLUI_ButtonCmd newBtn = Instantiate(cmdBtnPrefab, cmdOptionObj.transform);
            newBtn.InitializeSelf();
            CreateCmdBtn(cmdList[i], newBtn);
            cmdBtns.Add(newBtn);

            if (commands[i] == BattleCommandType.Back)
            {
                newBtn.transform.localPosition += new Vector3(-14, 20);
                newBtn.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48f);
            }
            else
            {
                int xPos = (realChoices % 2) * 80;
                int yPos = (realChoices < 2) ? 0 : -20;
                newBtn.transform.localPosition += new Vector3(xPos, yPos);
                realChoices++;
            }
        }

        string promptString = "What will \\cyellow\\" + pokemon.nickname + "\\c.\\ do?";
        StartCoroutine(DrawTextInstant(text: promptString, textBox: cmdTxt));
    }
    private void CreateCmdBtn(BattleCommandType commandType, BTLUI_ButtonCmd btn)
    {
        btn.commandType = commandType;
        btn.txt.text = commandType.ToString();
        btn.colorSel = colorSel;
        btn.colorUnsel = colorUnsel;
    }
    public void SwitchSelectedCommandTo(BattleCommandType selected)
    {
        for (int i = 0; i < cmdBtns.Count; i++)
        {
            SelectCmdBtn(cmdBtns[i], cmdBtns[i].commandType == selected);
        }
    }
    private void SelectCmdBtn(BTLUI_ButtonCmd btn, bool select)
    {
        btn.image.color = select ? btn.colorSel : btn.colorUnsel;
        btn.txt.color = select ? colorTxtSel : colorTxtUnsel;
    }


    // FIGHT
    public void SetMoves(
        List<Pokemon.Moveslot> choices, 
        bool canMegaEvolve = false,
        bool canZMove = false,
        bool canDynamax = false)
    {
        fightBtns = new List<BTLUI_ButtonFight>();
        fightList = new List<Pokemon.Moveslot>(choices);

        int realBtns = 0;
        for (int i = 0; i < fightList.Count; i++)
        {
            BTLUI_ButtonFight newBtn = Instantiate(fightBtnPrefab, fightOptionObj.transform);
            newBtn.InitializeSelf();
            fightBtns.Add(newBtn);

            // Back
            if (fightList[i] == null)
            {
                newBtn.moveTxt.text = "Back";
                newBtn.colorSel = colorSel;
                newBtn.colorUnsel = colorUnsel;
                newBtn.transform.localPosition += new Vector3(-32, 20);
                newBtn.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48f);
            }
            else
            {
                int xPos = (realBtns % 2) * 116;
                int yPos = (realBtns < 2) ? 0 : -20;
                CreateFightBtn(fightList[i], newBtn);
                newBtn.transform.localPosition += new Vector3(xPos, yPos);
                realBtns++;
            }
        }
    
        // Enable Special Button
        if (canMegaEvolve || canZMove || canDynamax)
        {
            fightSpecialBtn.gameObject.SetActive(true);
            fightSpecialIcon.gameObject.SetActive(true);
            if (canMegaEvolve)
            {
                fightSpecialBtn.colorSel = new Color(0.5f, 1f, 1f, 0.75f);
                fightSpecialBtn.txt.text = "Z - Mega Evolution";
                fightSpecialIcon.sprite = fightMegaIcon;
            }
            else if (canZMove)
            {
                fightSpecialBtn.colorSel = new Color(0.75f, 1f, 0.5f, 0.75f);
                fightSpecialBtn.txt.text = "Z - Z-Move";
                fightSpecialIcon.sprite = fightZMoveIcon;
            }
            else if (canDynamax)
            {
                fightSpecialBtn.colorSel = new Color(1f, 0.5f, 0.5f, 0.75f);
                fightSpecialBtn.txt.text = "Z - Dynamax";
                fightSpecialIcon.sprite = fightDynamaxIcon;
            }
        }
        else
        {
            fightSpecialBtn.gameObject.SetActive(false);
            fightSpecialIcon.gameObject.SetActive(false);
        }
    }
    private void CreateFightBtn(Pokemon.Moveslot moveslot, BTLUI_ButtonFight btn)
    {
        btn.moveslot = moveslot;
        btn.moveTxt.text = MoveDatabase.instance.GetMoveData(moveslot.moveID).moveName;
        btn.colorSel = colorSel;
        btn.colorUnsel = colorUnsel;
    }
    public void SwitchSelectedMoveTo(
        Pokemon pokemon, 
        Pokemon.Moveslot selected, 
        bool choosingSpecial,
        bool choosingZMove,
        bool choosingMaxMove)
    {
        for (int i = 0; i < fightBtns.Count; i++)
        {
            SelectFightBtn(
                pokemon: pokemon, 
                btn: fightBtns[i], 
                select: fightBtns[i].moveslot == selected,
                ZMove: choosingZMove,
                maxMove: choosingMaxMove);
        }
        if (choosingSpecial)
        {
            fightSpecialBtn.image.color = fightSpecialBtn.colorSel;
        }
        else
        {
            fightSpecialBtn.image.color = fightSpecialBtn.colorUnsel;
        }
    }
    private void SelectFightBtn(
        Pokemon pokemon, 
        BTLUI_ButtonFight btn, 
        bool select, 
        bool ZMove = false,
        bool maxMove = false)
    {
        btn.image.color = select ? btn.colorSel : btn.colorUnsel;
        btn.moveTxt.color = select ? colorTxtSel : colorTxtUnsel;

        MoveData moveData = (btn.moveslot == null) ? null : MoveDatabase.instance.GetMoveData(btn.moveslot.moveID);
        if (moveData != null)
        {
            if (ZMove)
            {
                moveData = battleModel.GetPokemonZMoveData(pokemon, btn.moveslot.moveID);
            }
            if (maxMove)
            {
                moveData = battleModel.GetPokemonMaxMoveData(pokemon, moveData);
            }
            if (moveData != null)
            {
                btn.moveTxt.text = moveData.moveName;
            }
            else
            {
                btn.moveTxt.text = "";
            }
        }
        
        if (select)
        {
            string promptString = "";
            if (btn.moveslot == null)
            {
                promptString = "Go back to\nCommands";
            }
            else
            {
                if (moveData != null)
                {
                    TypeData typeData = TypeDatabase.instance.GetTypeData(moveData.moveType);
                    promptString += "\\c" + typeData.typeColor + "\\" + typeData.typeName + "\\c.\\";
                    promptString += "\nPP: " + btn.moveslot.PP + " / " + btn.moveslot.maxPP;
                }
                else
                {
                    if (ZMove)
                    {
                        promptString = "Cannot use Z-Power";
                    }
                }
            }
            StartCoroutine(DrawTextInstant(text: promptString, textBox: fightTxt));
        }
    }
    

    // FIELD TARGETING
    public void SetFieldTargets(int teamPos)
    {
        List<BattlePosition> allPositions = battleModel.GetAllBattlePositions();
        for (int i = 0; i < allPositions.Count; i++)
        {
            BattlePosition curPos = allPositions[i];
            BattleTeam team = battleModel.GetTeamFromBattlePosition(curPos);
            bool isAlly = (teamPos == curPos.teamPos);
            GameObject spawn = isAlly ? fieldTargetOptionObjNear : fieldTargetOptionObjFar;

            BTLUI_ButtonFieldTarget newBtn = Instantiate(fieldTargetBtnPrefab, spawn.transform);
            newBtn.InitializeSelf();

            switch (team.teamMode)
            {
                case BattleTeam.TeamMode.Single:
                    newBtn.rectTransform.localPosition = new Vector3(0, 0);
                    break;

                case BattleTeam.TeamMode.Double:
                    float xPosDouble = isAlly ? -48f : 48f;
                    if (curPos.battlePos == 1) xPosDouble = -xPosDouble;
                    newBtn.rectTransform.localPosition = new Vector3(xPosDouble, 0);
                    break;

                case BattleTeam.TeamMode.Triple:
                    float xPosTriple = isAlly ? -96f : 96f;
                    xPosTriple += curPos.battlePos * (isAlly ? 96f : -96f);
                    newBtn.rectTransform.localPosition = new Vector3(xPosTriple, 0);
                    break;
            }
            CreateFieldTargetBtn(curPos, newBtn);
            fieldTargetBtns.Add(newBtn);
        }
    }
    public void CreateFieldTargetBtn(BattlePosition position, BTLUI_ButtonFieldTarget btn)
    {
        btn.position = position;
        Pokemon pokemon = battleModel.GetPokemonAtPosition(position);
        if (pokemon != null)
        {
            btn.pokemon = pokemon;

            btn.nameTxt.text = pokemon.nickname;
            PokemonGender gender = pokemon.gender;
            if (gender != PokemonGender.Genderless)
            {
                btn.nameTxt.text += (gender == PokemonGender.Male) ? " <color=#8080FF>♂</color>"
                    : " <color=#FF8080>♀</color>";
            }

            btn.lvlTxt.text = "Lv" + pokemon.level;

            StatusCondition condition = pokemon.nonVolatileStatus;
            if (condition.statusID == "healthy")
            {
                btn.statusTxt.text = "";
            }
            else
            {
                btn.statusTxt.text = condition.data.shortName;
            }

            float hpPercent = pokemon.HPPercent;
            btn.hpBar.fillAmount = hpPercent;

            btn.hpBar.color = (hpPercent > 0.5f) ? btn.hpHigh
                : (hpPercent > 0.25f) ? btn.hpMed
                : btn.hpLow;

            // draw icon
            string drawPath = "pokemonSprites/icon/" + pokemon.data.displayID;
            btn.icon.sprite = BattleAssetLoader.instance.nullPokemonIconSprite;
            if (BattleAssetLoader.instance.loadedPokemonSprites.ContainsKey(drawPath))
            {
                btn.icon.sprite = BattleAssetLoader.instance.loadedPokemonSprites[drawPath];
            }
            else
            {
                StartCoroutine(BattleAssetLoader.instance.LoadPokemon(
                    pokemon: pokemon,
                    useicon: true,
                    imagePokemon: btn.icon
                    ));
            }
        }
        else
        {
            btn.nameTxt.gameObject.SetActive(false);
            btn.lvlTxt.gameObject.SetActive(false);
            btn.statusTxt.gameObject.SetActive(false);
            btn.hpObj.SetActive(false);
            btn.icon.gameObject.SetActive(false);
        }
    }
    public void SwitchSelectedMoveTargetsTo(BattlePosition userPos, int chooseIndex, List<List<BattlePosition>> choices)
    {
        List<BattlePosition> targetPositions = choices[chooseIndex];

        string descText = "";
        if (targetPositions == null)
        {
            descText = "<<-  Back  ->>"; 
        }
        else
        {
            int optionNo = choices.Contains(null) ? chooseIndex : chooseIndex + 1;
            descText = "<<-  Option " + optionNo + "  ->>";
        }
        StartCoroutine(DrawTextInstant(text: descText, textBox: fieldTargetTxt));

        for (int i = 0; i < fieldTargetBtns.Count; i++)
        {
            bool posWasTargeted = false;
            BattlePosition curPos = fieldTargetBtns[i].position;

            if (targetPositions != null)
            {
                for (int k = 0; k < targetPositions.Count; k++)
                {
                    if (targetPositions[k].IsTheSameAs(curPos))
                    {
                        posWasTargeted = true;
                        break;
                    }
                }
            }

            if (posWasTargeted)
            {
                SelectFieldTarget(fieldTargetBtns[i], true);
            }
            else
            {
                SelectFieldTarget(fieldTargetBtns[i], false);
                if (userPos.IsTheSameAs(curPos))
                {
                    fieldTargetBtns[i].image.color = fieldTargetBtns[i].colorUser;
                }
            }
        }
    }
    public void SelectFieldTarget(BTLUI_ButtonFieldTarget btn, bool select)
    {
        btn.image.color = select ? btn.colorSel : btn.colorUnsel;
        btn.nameTxt.color = select ? colorTxtSel : colorTxtUnsel;

        if (btn.pokemon != null)
        {
            if (select)
            {
                btn.lvlTxt.color = colorTxtSel;
                btn.statusTxt.color = colorTxtSel;
            }
            else
            {
                btn.lvlTxt.color = colorTxtUnsel;
                btn.statusTxt.color = colorTxtUnsel;
            }
        }
    }


    // PARTY
    public void SetParty(List<Pokemon> choices, bool forceSwitch, Item item = null)
    {
        partyBtns = new List<BTLUI_ButtonParty>();
        partyList = new List<Pokemon>(choices);

        int realBtns = 0;
        for (int i = 0; i < partyList.Count; i++)
        {
            BTLUI_ButtonParty newBtn = Instantiate(
                (partyList[i] == null)? partyBtnBackPrefab : partyBtnPrefab, 
                partyOptionObj.transform
                );
            newBtn.InitializeSelf();
            partyBtns.Add(newBtn);

            // Back
            if (partyList[i] == null)
            {
                newBtn.nameTxt.text = "Back";
                newBtn.transform.localPosition += new Vector3(-48, -124);
            }
            else
            {
                int xPos = (realBtns % 2) * 148;
                int yPos = -((realBtns / 2) * 40) - (realBtns % 2) * 22;
                CreatePartyBtn(partyList[i], newBtn);
                newBtn.transform.localPosition += new Vector3(xPos, yPos);
                realBtns++;
            }
        }

        string promptString = (forceSwitch)? 
            "Who will you\nswitch in?" : "Select a\nparty member.";
        if (item != null)
        {
            promptString = "Use the " + item.data.itemName + "\non which party member?";
        }
        StartCoroutine(DrawTextInstant(text: promptString, textBox: partyTxt));
    }
    private void CreatePartyBtn(Pokemon pokemon, BTLUI_ButtonParty btn)
    {
        btn.pokemon = pokemon;

        btn.nameTxt.text = pokemon.nickname;
        PokemonGender gender = pokemon.gender;
        if (gender != PokemonGender.Genderless)
        {
            btn.nameTxt.text += (gender == PokemonGender.Male) ? " <color=#8080FF>♂</color>"
                : " <color=#FF8080>♀</color>";
        }

        btn.lvlTxt.text = "Lv" + pokemon.level;
        btn.hpTxt.text = pokemon.currentHP + " / " + pokemon.maxHP;

        StatusCondition condition = pokemon.nonVolatileStatus;
        if (condition.statusID == "healthy")
        {
            btn.statusTxt.text = "";
        }
        else
        {
            btn.statusTxt.text = condition.data.shortName;
        }

        float hpPercent = pokemon.HPPercent;
        btn.hpBar.fillAmount = hpPercent;

        btn.hpBar.color = (hpPercent > 0.5f) ? btn.hpHigh
            : (hpPercent > 0.25f) ? btn.hpMed
            : btn.hpLow;

        // draw icon
        string drawPath = "pokemonSprites/icon/" + pokemon.data.displayID;
        btn.icon.sprite = BattleAssetLoader.instance.nullPokemonIconSprite;
        if (BattleAssetLoader.instance.loadedPokemonSprites.ContainsKey(drawPath))
        {
            btn.icon.sprite = BattleAssetLoader.instance.loadedPokemonSprites[drawPath];
        }
        else
        {
            StartCoroutine(BattleAssetLoader.instance.LoadPokemon(
                pokemon: pokemon,
                useicon: true,
                imagePokemon: btn.icon
                ));
        }
    }
    public void SwitchSelectedPartyTo(Pokemon selected)
    {
        for (int i = 0; i < partyBtns.Count; i++)
        {
            SelectPartyBtn(partyBtns[i], partyBtns[i].pokemon == selected);
        }
    }
    private void SelectPartyBtn(BTLUI_ButtonParty btn, bool select)
    {
        btn.image.color = select ? colorSel : colorUnsel;
        btn.nameTxt.color = select ? colorTxtSel : colorTxtUnsel;

        if (btn.pokemon != null)
        {
            if (select)
            {
                btn.lvlTxt.color = colorTxtSel;
                btn.hpTxt.color = colorTxtSel;
                btn.statusTxt.color = colorTxtSel;
            }
            else
            {
                btn.lvlTxt.color = colorTxtUnsel;
                btn.hpTxt.color = colorTxtUnsel;
                btn.statusTxt.color = colorTxtUnsel;
            }
        }
        
    }


    // PARTY COMMAND
    public void SetPartyCommands(Pokemon pokemon, List<BattleExtraCommand> commands)
    {
        partyCmdBtns = new List<BTLUI_ButtonCmdExtra>();

        for (int i = 0; i < commands.Count; i++)
        {
            BTLUI_ButtonCmdExtra newBtn = Instantiate(cmdExtraBtnPrefab, partyCmdOptionObj.transform);
            newBtn.InitializeSelf();
            CreateCmdExtraBtn(commands[i], newBtn);
            partyCmdBtns.Add(newBtn);

            int xPos = 0;
            int yPos = -20 * i;
            newBtn.transform.localPosition += new Vector3(xPos, yPos);
        }

        string promptString = "Do what with\n\\cyellow\\" + pokemon.nickname + "\\c.\\?";
        StartCoroutine(DrawTextInstant(text: promptString, textBox: partyCmdTxt));
    }
    private void CreateCmdExtraBtn(BattleExtraCommand commandType, BTLUI_ButtonCmdExtra btn)
    {
        btn.commandType = commandType;
        btn.txt.text = commandType.ToString();
    }
    public void SwitchSelectedPartyCommandTo(BattleExtraCommand selected)
    {
        for (int i = 0; i < partyCmdBtns.Count; i++)
        {
            SelectCmdExtraBtn(partyCmdBtns[i], partyCmdBtns[i].commandType == selected);
        }
    }
    private void SelectCmdExtraBtn(BTLUI_ButtonCmdExtra btn, bool select)
    {
        btn.image.color = select ? colorSel : colorUnsel;
        btn.txt.color = select ? colorTxtSel : colorTxtUnsel;
    }


    // BAG POCKET
    public void SetBagPockets(List<ItemBattlePocket> list)
    {
        bagPocketBtns = new List<BTLUI_ButtonTxt>();
        bagPocketList = new List<ItemBattlePocket>(list);

        int realChoices = 0;
        for (int i = 0; i < list.Count; i++)
        {
            BTLUI_ButtonTxt newBtn = Instantiate(txtBtnPrefab, bagPocketOptionObj.transform);
            newBtn.InitializeSelf();
            CreateBagPocketBtn(list[i], newBtn);
            bagPocketBtns.Add(newBtn);

            if (list[i] == ItemBattlePocket.None)
            {
                newBtn.transform.localPosition += new Vector3(-22, 20);
                newBtn.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48f);
            }
            else
            {
                int xPos = (realChoices % 2) * 96;
                int yPos = (realChoices < 2) ? 0 : -20;
                newBtn.transform.localPosition += new Vector3(xPos, yPos);
                realChoices++;
            }
        }

        bagPocketTxt.text = "Which type of\nitems to use?";
    }
    private string GetBagPocketString(ItemBattlePocket pocket)
    {
        return (pocket == ItemBattlePocket.HPRestore) ? "HP Restore"
            : (pocket == ItemBattlePocket.StatusRestore) ? "Status Restore"
            : (pocket == ItemBattlePocket.Pokeballs) ? "Poké Balls"
            : (pocket == ItemBattlePocket.BattleItems) ? "Battle Items"
            : (pocket == ItemBattlePocket.None) ? "Back"
            : "";
    }
    private void CreateBagPocketBtn(ItemBattlePocket objType, BTLUI_ButtonTxt btn)
    {
        btn.txt.text = GetBagPocketString(objType);
        btn.colorSel = colorSel;
        btn.colorUnsel = colorUnsel;
    }
    public void SwitchSelectedBagPocketTo(ItemBattlePocket selected)
    {
        for (int i = 0; i < bagPocketBtns.Count; i++)
        {
            SelectTxtBtn(bagPocketBtns[i], bagPocketList[i] == selected);
        }
    }


    // BAG ITEMS
    public void SetItems(Trainer trainer, ItemBattlePocket pocket, List<Item> list, int offset)
    {
        ClearItems();

        itemBtns = new List<BTLUI_ButtonItem>();
        itemList = new List<Item>();

        // Create the back button
        BTLUI_ButtonItem backBtn = Instantiate(itemBtnBackPrefab, itemOptionObj.transform);
        backBtn.InitializeSelf();
        itemBtns.Add(backBtn);
        backBtn.itemID = null;
        backBtn.nameTxt.text = "Back";
        backBtn.rectTransform.localPosition = new Vector3(-48, -128);
        backBtn.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48f);

        for (int i = offset; i < offset + maxItemCount; i++)
        {
            if (i < list.Count)
            {
                itemList.Add(list[i]);
            }
        }

        int realBtns = 0;
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] != null)
            {
                BTLUI_ButtonItem newBtn = Instantiate(itemBtnPrefab, itemOptionObj.transform);
                newBtn.InitializeSelf();
                itemBtns.Add(newBtn);

                int xPos = 0;
                int yPos = -realBtns * 34;
                CreateItemBtn(trainer, itemList[i], newBtn);
                newBtn.transform.localPosition += new Vector3(xPos, yPos);
                realBtns++;
            }
        }

        int curPage = 1 + (offset / maxItemCount);
        int itemPages = 1 + (list.Count / maxItemCount);
        itemPocketTxt.text = GetBagPocketString(pocket);
        itemPageTxt.text = "Page " + curPage + " / " + itemPages;
    }
    private void CreateItemBtn(Trainer trainer, Item itemSlot, BTLUI_ButtonItem btn)
    {
        btn.itemID = itemSlot.itemID;
        btn.nameTxt.text = itemSlot.data.itemName;
        btn.colorSel = colorSel;
        btn.colorUnsel = colorUnsel;

        int amount = trainer.GetItemCount(itemSlot.itemID);
        btn.amountTxt.text = "x" + amount;

        // draw icon
        string drawPath = "itemSprites/" + itemSlot.data.ID;
        btn.icon.sprite = BattleAssetLoader.instance.nullSprite;
        if (BattleAssetLoader.instance.loadedItemSprites.ContainsKey(drawPath))
        {
            btn.icon.sprite = BattleAssetLoader.instance.loadedItemSprites[drawPath];
        }
        else
        {
            BattleAssetLoader.instance.LoadItem(
            item: itemSlot,
            image: btn.icon
            );
        }
    }
    public void SwitchSelectedItemTo(Item selected)
    {
        for (int i = 0; i < itemBtns.Count; i++)
        {
            if (selected == null)
            {
                SelectItemBtn(itemBtns[i], itemBtns[i].itemID == null);
            }
            else
            {
                SelectItemBtn(itemBtns[i], itemBtns[i].itemID == selected.itemID);
            }
        }
    }
    private void SelectItemBtn(BTLUI_ButtonItem btn, bool select)
    {
        btn.image.color = select ? colorSel : colorUnsel;
        btn.nameTxt.color = select ? colorTxtSel : colorTxtUnsel;

        if (select)
        {
            itemNameTxt.text = "<color=yellow>" + btn.nameTxt.text + "</color>";
            if (btn.itemID == null)
            {
                itemDescTxt.text = "Go back to Bag Pockets.";
            }
            else
            {
                itemDescTxt.text = "The item's description should go here.";
            }
        }

        if (btn.itemID != null)
        {
            btn.amountTxt.color = select ? colorTxtSel : colorTxtUnsel;
        }
    }
    private void ClearItems()
    {
        for (int i = 0; i < itemBtns.Count; i++)
        {
            Destroy(itemBtns[i].gameObject);
        }
        itemBtns.Clear();
        itemList.Clear();
    }


    // HUD
    public BTLUI_PokemonHUD DrawPokemonHUD(Pokemon pokemon, Battle battle, bool isNear)
    {
        // get spawn position
        Transform spawnPos = this.transform;
        BattleTeam team = battle.GetTeam(pokemon);
        switch (team.teamMode)
        {
            case BattleTeam.TeamMode.Single:
                spawnPos = (isNear) ? spawnNearSingle : spawnFarSingle;
                break;

            case BattleTeam.TeamMode.Double:
                spawnPos = (pokemon.battlePos == 0) ? (isNear ? spawnNearDouble0 : spawnFarDouble0)
                    : isNear ? spawnNearDouble1 : spawnFarDouble1;
                break;

            case BattleTeam.TeamMode.Triple:
                spawnPos = (pokemon.battlePos == 0) ? (isNear ? spawnNearTriple0 : spawnFarTriple0)
                    : (pokemon.battlePos == 1) ? (isNear ? spawnNearTriple1 : spawnFarTriple1)
                    : isNear ? spawnNearTriple2 : spawnFarTriple2;
                break;
        }

        // draw pokemon HUD
        BTLUI_PokemonHUD pokemonHUD = Instantiate(pokemonHUDPrefab, spawnPos.position, Quaternion.identity, spawnPos);
        pokemonHUD.pokemonUniqueID = pokemon.uniqueID;
        pokemonHUD.hpObj.gameObject.SetActive(isNear 
            && (team.teamMode == BattleTeam.TeamMode.Single
                || team.teamMode == BattleTeam.TeamMode.Double));
        /*pokemonHUD.expObj.SetActive(isNear
            && (team.teamMode == BattleTeam.TeamMode.Single
                || team.teamMode == BattleTeam.TeamMode.Double));*/
        pokemonHUDs.Add(pokemonHUD);

        UpdatePokemonHUD(pokemon, battle);

        return pokemonHUD;
    }
    public bool UndrawPokemonHUD(Pokemon pokemon)
    {
        BTLUI_PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
        if (pokemonHUD != null)
        {
            pokemonHUDs.Remove(pokemonHUD);
            Destroy(pokemonHUD.gameObject);
            return true;
        }
        return false;
    }

    public BTLUI_PokemonHUD GetPokemonHUD(Pokemon pokemon)
    {
        for (int i = 0; i < pokemonHUDs.Count; i++)
        {
            if (pokemonHUDs[i].pokemonUniqueID == pokemon.uniqueID)
            {
                return pokemonHUDs[i];
            }
        }
        return null;
    }
    public void UpdatePokemonHUD(Pokemon pokemon, Battle battle)
    {
        BTLUI_PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
        if (pokemonHUD != null)
        {
            pokemonHUD.nameTxt.text = pokemon.nickname;
            PokemonGender gender = pokemon.gender;
            if (gender != PokemonGender.Genderless)
            {
                pokemonHUD.nameTxt.text += (gender == PokemonGender.Male) ? " <color=#8080FF>♂</color>"
                    : " <color=#FF8080>♀</color>";
            }

            pokemonHUD.lvlTxt.text = "Lv" + pokemon.level;
            pokemonHUD.statusTxt.text = "";
            StatusCondition condition = pokemon.nonVolatileStatus;
            if (condition != null)
            {
                if (condition.statusID != "healthy")
                {
                    pokemonHUD.statusTxt.text = condition.data.shortName;
                }
            }

            pokemonHUD.hpTxt.text = pokemon.currentHP + " / " + pokemon.maxHP;

            float hpPercent = battle.GetPokemonHPAsPercentage(pokemon);
            pokemonHUD.hpBar.fillAmount = hpPercent;

            pokemonHUD.hpBar.color = (hpPercent > 0.5f) ? pokemonHUD.hpHigh
                : (hpPercent > 0.25f) ? pokemonHUD.hpMed
                : pokemonHUD.hpLow;
        }
    }

    public void SetPokemonHUDActive(Pokemon pokemon, bool active)
    {
        BTLUI_PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
        if (pokemonHUD != null)
        {
            pokemonHUD.gameObject.SetActive(active);
        }
    }
    public void SetPokemonHUDsActive(bool active)
    {
        if (active)
        {
            spawnFar.SetActive(true);
            spawnNear.SetActive(true);
        }
        for (int i = 0; i < pokemonHUDs.Count; i++)
        {
            pokemonHUDs[i].gameObject.SetActive(active);
        }
    }

    public IEnumerator AnimatePokemonHUDHPChange(Pokemon pokemon, int preHP, int postHP, float timeSpan = 1f)
    {
        BTLUI_PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
        if (pokemonHUD != null)
        {
            int maxHP = pokemon.maxHP;
            float preValue = pokemonHUD.hpBar.fillAmount;
            float postValue = battleModel.GetPokemonHPPercentGivenHP(pokemon, postHP);
            
            float difference = postValue - preValue;
            float increment = (timeSpan == 0)? 1f : 0f;
            while (increment < 1)
            {
                increment += (1 / timeSpan) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                float curFillAmount = preValue + difference * increment;
                int displayHP = Mathf.FloorToInt(curFillAmount * maxHP);
                if (displayHP == 0 && curFillAmount > 0)
                {
                    displayHP = 1;
                }
                
                // display changes
                pokemonHUD.hpTxt.text = displayHP + " / " + maxHP;
                pokemonHUD.hpBar.fillAmount = curFillAmount;
                pokemonHUD.hpBar.color = (curFillAmount > 0.5f) ? pokemonHUD.hpHigh
                    : (curFillAmount > 0.25f) ? pokemonHUD.hpMed
                    : pokemonHUD.hpLow;

                yield return null;
            }

            pokemonHUD.hpTxt.text = postHP + " / " + maxHP;
            pokemonHUD.hpBar.fillAmount = postValue;
            pokemonHUD.hpBar.color = (postValue > 0.5f) ? pokemonHUD.hpHigh
                : (postValue > 0.25f) ? pokemonHUD.hpMed
                : pokemonHUD.hpLow;
        }
    }

    // Dialog
    public void UndrawDialogBox()
    {
        dialog.dialogBox.gameObject.SetActive(false);
        if (panel == Panel.Command)
        {
            cmdPanel.gameObject.SetActive(true);
        }
        else if (panel == Panel.Fight)
        {
            fightPanel.gameObject.SetActive(true);
        }
        spawnNear.SetActive(true);
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
    public IEnumerator DrawText(
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
        if (textBox == null)
        {
            cmdPanel.SetActive(false);
            fightPanel.SetActive(false);
            spawnNear.SetActive(false);
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

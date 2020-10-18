using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUIPanelFieldTarget : BTLUIPanel
{
    [Header("Buttons")]
    public BTLUI_ButtonFieldTarget targetBtnNearSingle;
    public BTLUI_ButtonFieldTarget targetBtnFarSingle,

        targetBtnNearDouble0,
        targetBtnNearDouble1,
        targetBtnFarDouble0,
        targetBtnFarDouble1,

        targetBtnNearTriple0,
        targetBtnNearTriple1,
        targetBtnNearTriple2,
        targetBtnFarTriple0,
        targetBtnFarTriple1,
        targetBtnFarTriple2;
    List<BTLUI_ButtonFieldTarget> activeTargetBtns;
    public BTLUI_Button backBtn;

    [Header("Text")]
    public Text promptText;

    private void Awake()
    {
        ClearSelf();
    }

    public override void ClearSelf()
    {
        base.ClearSelf();
        activeTargetBtns = new List<BTLUI_ButtonFieldTarget>();
        CleanAllButtons();
        HideAllButtons();
    }

    public void HideAllButtons()
    {
        List<BTLUI_ButtonFieldTarget> allBtns = GetAllFieldTargetButtons();
        for (int i = 0; i < allBtns.Count; i++)
        {
            BTLUI_ButtonFieldTarget curBtn = allBtns[i];
            curBtn.UnselectSelf();
            curBtn.gameObject.SetActive(false);
        }
    }
    public void CleanAllButtons()
    {
        List<BTLUI_ButtonFieldTarget> allBtns = GetAllFieldTargetButtons();
        for (int i = 0; i < allBtns.Count; i++)
        {
            BTLUI_ButtonFieldTarget curBtn = allBtns[i];
            curBtn.RefreshSelf();
            curBtn.position = null;
        }
    }
    public List<BTLUI_ButtonFieldTarget> GetAllFieldTargetButtons()
    {
        List<BTLUI_ButtonFieldTarget> allBtns = new List<BTLUI_ButtonFieldTarget>
        {
            targetBtnNearSingle, targetBtnFarSingle,
            targetBtnNearDouble0, targetBtnNearDouble1, targetBtnFarDouble0, targetBtnFarDouble1,
            targetBtnNearTriple0, targetBtnNearTriple1, targetBtnNearTriple2,
            targetBtnFarTriple0, targetBtnFarTriple1, targetBtnFarTriple2
        };
        return allBtns;
    }
    public BTLUI_ButtonFieldTarget GetFieldTargetButton(BattlePosition position, int teamPerspective, Battle battleModel)
    {
        BattleTeam team = battleModel.GetTeamFromBattlePosition(position);
        bool isAlly = (teamPerspective == position.teamPos);

        BTLUI_ButtonFieldTarget curBtn =

            // singles battle
            (team.teamMode == BattleTeam.TeamMode.Single) ? (isAlly ? targetBtnNearSingle : targetBtnFarSingle)

            // doubles battle
            : (team.teamMode == BattleTeam.TeamMode.Double) ?
                (isAlly ?
                    ((position.battlePos == 0) ? targetBtnNearDouble0 : targetBtnNearDouble1)
                    : ((position.battlePos == 0) ? targetBtnFarDouble0 : targetBtnFarDouble1))

            // triples battle
            : (team.teamMode == BattleTeam.TeamMode.Triple) ?
                (isAlly ?
                    ((position.battlePos == 0) ? targetBtnNearTriple0
                        : (position.battlePos == 1) ? targetBtnNearTriple1 : targetBtnNearTriple2)
                    : ((position.battlePos == 0) ? targetBtnFarTriple0
                        : (position.battlePos == 1) ? targetBtnFarTriple1 : targetBtnFarTriple2))
            : null;
        return curBtn;
    }

    public void SetFieldTargets(int teamPos, Battle battleModel)
    {
        activeTargetBtns = new List<BTLUI_ButtonFieldTarget>();
        HideAllButtons();
        CleanAllButtons();

        List<BattlePosition> allPositions = battleModel.GetAllBattlePositions();
        for (int i = 0; i < allPositions.Count; i++)
        {
            BattlePosition curPos = allPositions[i];
            BattleTeam team = battleModel.GetTeamFromBattlePosition(curPos);
            bool isAlly = (teamPos == curPos.teamPos);

            BTLUI_ButtonFieldTarget curBtn = GetFieldTargetButton(
                position: curPos,
                teamPerspective: teamPos,
                battleModel: battleModel);

            if (curBtn != null)
            {
                CreateFieldTargetBtn(curPos, battleModel, curBtn);
                activeTargetBtns.Add(curBtn);
                curBtn.gameObject.SetActive(true);
            }
        }
    }
    public void CreateFieldTargetBtn(BattlePosition position, Battle battleModel, BTLUI_ButtonFieldTarget btn)
    {
        btn.position = position;
        Pokemon pokemon = battleModel.GetPokemonAtPosition(position);

        btn.RefreshSelf(active: pokemon != null);
        if (pokemon != null)
        {
            btn.nameTxt.text = pokemon.nickname;
            PokemonGender gender = pokemon.gender;
            if (gender != PokemonGender.Genderless)
            {
                btn.nameTxt.text += (gender == PokemonGender.Male) ? " <color=#8080FF>♂</color>"
                    : " <color=#FF8080>♀</color>";
            }

            btn.lvlTxt.text = "<color=yellow>Lv</color>" + pokemon.level;

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
    }

    public void HighlightFieldTargets(BattlePosition userPos, List<BattlePosition> targetPositions)
    {
        backBtn.UnselectSelf();
        promptText.text = "Choose a target by scrolling left or right.";
        List<BTLUI_ButtonFieldTarget> fieldTargetBtns = activeTargetBtns;
        for (int i = 0; i < fieldTargetBtns.Count; i++)
        {
            bool posWasTargeted = false;
            BTLUI_ButtonFieldTarget curBtn = fieldTargetBtns[i];
            BattlePosition curPos = fieldTargetBtns[i].position;

            for (int k = 0; k < targetPositions.Count; k++)
            {
                if (targetPositions[k].IsTheSameAs(curPos))
                {
                    posWasTargeted = true;
                    break;
                }
            }

            if (posWasTargeted)
            {
                curBtn.SelectSelf();
            }
            else
            {
                curBtn.UnselectSelf();
                if (userPos.IsTheSameAs(curPos))
                {
                    curBtn.image.color = curBtn.colorUser;
                }
            }
        }
    }
    public void HighlightBackButton()
    {
        List<BTLUI_ButtonFieldTarget> allBtns = GetAllFieldTargetButtons();
        for (int i = 0; i < allBtns.Count; i++)
        {
            allBtns[i].UnselectSelf();
        }
        backBtn.SelectSelf();
        promptText.text = "Go back to Move select.";
    }
}

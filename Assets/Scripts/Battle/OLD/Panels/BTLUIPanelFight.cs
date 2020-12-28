using PBS.Data;
using PBS.Databases;
using PBS.Main.Pokemon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUIPanelFight : BTLUIPanel
{
    [Header("Buttons")]
    public BTLUI_ButtonFight move1Btn;
    public BTLUI_ButtonFight move2Btn,
        move3Btn,
        move4Btn;
    public BTLUI_ButtonTxt specialBtn;
    public BTLUI_Button backBtn;

    [Header("Text")]
    public Text promptText;

    [Header("Icons")]
    public Image specialIcon;
    public Sprite fightMegaIcon,
        fightZMoveIcon,
        fightDynamaxIcon;

    public void SetMoves(
        Battle battle, 
        PBS.Main.Pokemon.Pokemon pokemon, 
        List<Moveslot> moveList,
        bool canMegaEvolve, bool canZMove = false, bool canDynamax = false,
        bool choosingZMove = false, bool choosingMaxMove = false)
    {
        // Set each move button
        for (int i = 0; i < moveList.Count; i++)
        {
            Moveslot moveslot = moveList[i];
            BTLUI_ButtonFight curBtn = (i == 0) ? move1Btn
                : (i == 1) ? move2Btn
                : (i == 2) ? move3Btn
                : (i == 3) ? move4Btn
                : null;
            if (curBtn != null)
            {
                SetMoveButton(
                    battle: battle,
                    pokemon: pokemon,
                    moveslot: moveslot,
                    choosingZMove: choosingZMove,
                    choosingMaxMove: choosingMaxMove,
                    moveBtn: curBtn);
            }
        }

        // Remove unnecessary buttons
        if (moveList.Count < 4)
        {
            move4Btn.gameObject.SetActive(false);
        }
        if (moveList.Count < 3)
        {
            move3Btn.gameObject.SetActive(false);
        }
        if (moveList.Count < 2)
        {
            move2Btn.gameObject.SetActive(false);
        }
        if (moveList.Count < 1)
        {
            move1Btn.gameObject.SetActive(false);
        }

        // Enable Special Button
        if (canMegaEvolve || canZMove || canDynamax)
        {
            specialBtn.gameObject.SetActive(true);
            specialIcon.gameObject.SetActive(true);
            if (canMegaEvolve)
            {
                specialBtn.colorSel = new Color(0.5f, 1f, 1f, 0.75f);
                specialBtn.txt.text = "Z - Mega Evolution";
                specialIcon.sprite = fightMegaIcon;
            }
            else if (canZMove)
            {
                specialBtn.colorSel = new Color(0.75f, 1f, 0.5f, 0.75f);
                specialBtn.txt.text = "Z - Z-Move";
                specialIcon.sprite = fightZMoveIcon;
            }
            else if (canDynamax)
            {
                specialBtn.colorSel = new Color(1f, 0.5f, 0.5f, 0.75f);
                specialBtn.txt.text = "Z - Dynamax";
                specialIcon.sprite = fightDynamaxIcon;
            }
        }
        else
        {
            specialBtn.gameObject.SetActive(false);
            specialIcon.gameObject.SetActive(false);
        }
    }
    public void SetMoveButton(
        Battle battle, 
        PBS.Main.Pokemon.Pokemon pokemon, 
        Moveslot moveslot,
        BTLUI_ButtonFight moveBtn,
        bool choosingZMove = false, bool choosingMaxMove = false)
    {
        MoveData moveData = battle.GetPokemonMoveData(userPokemon: pokemon, moveID: moveslot.moveID);
        if (choosingZMove)
        {
            moveData = battle.GetPokemonZMoveData(pokemon, moveslot.moveID);
        }
        if (choosingMaxMove)
        {
            moveData = battle.GetPokemonMaxMoveData(pokemon, moveData);
        }
        if (moveData != null)
        {
            PBS.Data.ElementalType typeData = PBS.Databases.ElementalTypes.instance.GetTypeData(moveData.moveType);
            Color typeColor = Color.clear;
            ColorUtility.TryParseHtmlString(typeData.typeColor, out typeColor);

            moveBtn.moveTxt.text = moveData.moveName;
            moveBtn.ppTxt.text = moveslot.PP + "/" + moveslot.maxPP;
            moveBtn.typeTxt.text = typeData.typeName;
            moveBtn.typeTxt.color = typeColor;

            moveBtn.moveData = moveData;
            moveBtn.moveslot = moveslot;
            moveBtn.hiddenByZMove = false;
            moveBtn.colorSel = new Color(typeColor.r, typeColor.g, typeColor.b, 0.75f);
        }
        else
        {
            moveBtn.moveTxt.text = "";
            moveBtn.ppTxt.text = "";
            moveBtn.typeTxt.text = "";
            moveBtn.moveData = null;
            moveBtn.moveslot = null;
            moveBtn.hiddenByZMove = true;
        }
        moveBtn.UnselectSelf();
    }

    public void HighlightMove(Moveslot moveslot)
    {
        BTLUI_ButtonFight selectedBtn = null;

        if (move1Btn.moveslot != null)
        {
            if (move1Btn.moveslot == moveslot)
            {
                selectedBtn = move1Btn;
            }
            else
            {
                move1Btn.UnselectSelf();
            }
        }
        if (move2Btn.moveslot != null)
        {
            if (move2Btn.moveslot == moveslot)
            {
                selectedBtn = move2Btn;
            }
            else
            {
                move2Btn.UnselectSelf();
            }
        }
        if (move3Btn.moveslot != null)
        {
            if (move3Btn.moveslot == moveslot)
            {
                selectedBtn = move3Btn;
            }
            else
            {
                move3Btn.UnselectSelf();
            }
        }
        if (move4Btn.moveslot != null)
        {
            if (move4Btn.moveslot == moveslot)
            {
                selectedBtn = move4Btn;
            }
            else
            {
                move4Btn.UnselectSelf();
            }
        }

        if (selectedBtn != null)
        {
            selectedBtn.SelectSelf();
            backBtn.UnselectSelf();

            MoveData moveData = selectedBtn.moveData;
            PBS.Data.ElementalType typeData = PBS.Databases.ElementalTypes.instance.GetTypeData(moveData.moveType);
            Color typeColor = Color.clear;
            ColorUtility.TryParseHtmlString(typeData.typeColor, out typeColor);
            string moveText = "<color=" + typeData.typeColor + ">" + typeData.typeName + "</color>\n";
            moveText += moveData.category.ToString() + " / ";
            moveText += (moveData.basePower > 0) ? moveData.basePower + " BP / " : "";
            moveText += (moveData.accuracy > 0) ? (Mathf.FloorToInt(moveData.accuracy * 100)) + "% ACC" 
                : "Never Misses";
            promptText.text = moveText;
        }

    }
    public void HighlightSpecialButton()
    {
        HighlightMove(null);
        specialBtn.SelectSelf();
        promptText.text = "";
    }
    public void HighlightBackButton()
    {
        HighlightMove(null);
        backBtn.SelectSelf();
        promptText.text = "Go back to commands.";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class Fight : BasePanel
    {
        [Header("Buttons")]
        public Panels.FightButton move1Btn;
        public Panels.FightButton move2Btn,
            move3Btn,
            move4Btn;
        public BTLUI_ButtonTxt specialBtn;
        public Panels.FightButton backBtn;

        [Header("Text")]
        public Text promptText;

        [Header("Icons")]
        public Image specialIcon;
        public Sprite fightMegaIcon,
            fightZMoveIcon,
            fightDynamaxIcon;

        private void Awake()
        {
            backBtn.moveID = "";
        }

        public void SetMoves(
            PBS.Battle.View.Compact.Pokemon pokemon, 
            List<PBS.Battle.View.Events.CommandAgent.Moveslot> moveList,
            bool canMegaEvolve, bool canZMove = false, bool canDynamax = false,
            bool choosingZMove = false, bool choosingMaxMove = false)
        {
            // Set each move button
            for (int i = 0; i < moveList.Count; i++)
            {
                PBS.Battle.View.Events.CommandAgent.Moveslot moveslot = moveList[i];
                Panels.FightButton curBtn = (i == 0) ? move1Btn
                    : (i == 1) ? move2Btn
                    : (i == 2) ? move3Btn
                    : (i == 3) ? move4Btn
                    : null;
                if (curBtn != null)
                {
                    SetMoveButton(
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
            PBS.Battle.View.Compact.Pokemon pokemon, 
            PBS.Battle.View.Events.CommandAgent.Moveslot moveslot,
            Panels.FightButton moveBtn,
            bool choosingZMove = false, bool choosingMaxMove = false)
        {
            MoveData moveData = MoveDatabase.instance.GetMoveData(moveslot.moveID);
            moveBtn.moveslot = moveslot;

            if (!moveslot.hide)
            {
                TypeData typeData = TypeDatabase.instance.GetTypeData(moveData.moveType);
                Color typeColor = Color.clear;
                ColorUtility.TryParseHtmlString(typeData.typeColor, out typeColor);

                moveBtn.moveTxt.text = moveData.moveName;
                moveBtn.ppTxt.text = moveslot.PP + "/" + moveslot.maxPP;
                moveBtn.typeTxt.text = typeData.typeName;
                moveBtn.typeTxt.color = typeColor;

                moveBtn.moveID = moveData.ID;
                moveBtn.colorSel = new Color(typeColor.r, typeColor.g, typeColor.b, 0.75f);
            }
            else
            {
                moveBtn.moveTxt.text = "";
                moveBtn.ppTxt.text = "";
                moveBtn.typeTxt.text = "";
                moveBtn.moveID = "";
            }
            moveBtn.UnselectSelf();
        }

        public void HighlightMove(int moveIndex)
        {
            Panels.FightButton selectedBtn = null;

            if (moveIndex == 0)
            {
                selectedBtn = move1Btn;
            }
            else
            {
                move1Btn.UnselectSelf();
            }

            if (moveIndex == 1)
            {
                selectedBtn = move2Btn;
            }
            else
            {
                move2Btn.UnselectSelf();
            }

            if (moveIndex == 2)
            {
                selectedBtn = move3Btn;
            }
            else
            {
                move3Btn.UnselectSelf();
            }

            if (moveIndex == 3)
            {
                selectedBtn = move4Btn;
            }
            else
            {
                move4Btn.UnselectSelf();
            }

            if (selectedBtn != null)
            {
                selectedBtn.SelectSelf();
                backBtn.UnselectSelf();

                if (selectedBtn.moveslot.hide)
                {
                    promptText.text = "This move can't be selected...";
                }
                else
                {
                    MoveData moveData = MoveDatabase.instance.GetMoveData(selectedBtn.moveID);
                    TypeData typeData = TypeDatabase.instance.GetTypeData(moveData.moveType);
                    Color typeColor = Color.clear;
                    ColorUtility.TryParseHtmlString(typeData.typeColor, out typeColor);
                    string moveText = "<color=" + typeData.typeColor + ">" + typeData.typeName + "</color>\n";
                    moveText += moveData.category.ToString() + " / ";
                    moveText += (selectedBtn.moveslot.basePower > 0) ? selectedBtn.moveslot.basePower + " BP / " : "";
                    moveText += (selectedBtn.moveslot.accuracy > 0) ? (Mathf.FloorToInt(selectedBtn.moveslot.accuracy * 100)) + "% ACC" 
                        : "Never Misses";
                    promptText.text = moveText;
                }
            }

        }
        public void HighlightSpecialButton()
        {
            specialBtn.SelectSelf();
            promptText.text = "";
        }
        public void HighlightBackButton()
        {
            move1Btn.UnselectSelf();
            move2Btn.UnselectSelf();
            move3Btn.UnselectSelf();
            move4Btn.UnselectSelf();

            backBtn.SelectSelf();
            promptText.text = "Go back to commands.";
        }
    }
}

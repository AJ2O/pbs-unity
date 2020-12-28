using PBS.Data;
using PBS.Databases;
using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    /// <summary>
    /// This component handles the UI menu for field target selection.
    /// </summary>
    public class FieldTarget : BasePanel
    {
        #region Attributes
        [Header("Buttons")]
        public FieldTargetButton targetBtnNearSingle;
        public FieldTargetButton targetBtnFarSingle,

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
        List<FieldTargetButton> activeTargetBtns;
        public BTLUI_Button backBtn;

        [Header("Text")]
        public Text promptText;
        #endregion

        #region Unity
        private void Awake()
        {
            ClearSelf();
        }
        #endregion

        #region Button Components
        /// <summary>
        /// Clears the components of this panel.
        /// </summary>
        public override void ClearSelf()
        {
            base.ClearSelf();
            activeTargetBtns = new List<FieldTargetButton>();
            CleanAllButtons();
            HideAllButtons();
        }

        /// <summary>
        /// Hides all field target buttons on the panel.
        /// </summary>
        public void HideAllButtons()
        {
            List<FieldTargetButton> allBtns = GetAllFieldTargetButtons();
            for (int i = 0; i < allBtns.Count; i++)
            {
                FieldTargetButton curBtn = allBtns[i];
                curBtn.UnselectSelf();
                curBtn.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Removes components of all buttons on this panel.
        /// </summary>
        public void CleanAllButtons()
        {
            List<FieldTargetButton> allBtns = GetAllFieldTargetButtons();
            for (int i = 0; i < allBtns.Count; i++)
            {
                FieldTargetButton curBtn = allBtns[i];
                curBtn.RefreshSelf();
                curBtn.position = null;
            }
        }
        /// <summary>
        /// Returns all field target buttons on this panel.
        /// </summary>
        /// <returns></returns>
        public List<FieldTargetButton> GetAllFieldTargetButtons()
        {
            List<FieldTargetButton> allBtns = new List<FieldTargetButton>
            {
                targetBtnNearSingle, targetBtnFarSingle,
                targetBtnNearDouble0, targetBtnNearDouble1, targetBtnFarDouble0, targetBtnFarDouble1,
                targetBtnNearTriple0, targetBtnNearTriple1, targetBtnNearTriple2,
                targetBtnFarTriple0, targetBtnFarTriple1, targetBtnFarTriple2
            };
            return allBtns;
        }
        /// <summary>
        /// Return a field target button to be associated with the given battle position.
        /// </summary>
        /// <param name="position">The battle position to use for association.</param>
        /// <param name="teamPerspective">The team persepctive used to determine the button to return.</param>
        /// <param name="battleModel">The model used to determine the button to return.</param>
        /// <returns></returns>
        public FieldTargetButton GetFieldTargetButton(BattlePosition position, int teamPerspective, Model battleModel)
        {
            WifiFriendly.Team team = battleModel.GetMatchingTeam(position.teamPos);
            bool isAlly = (teamPerspective == position.teamPos);

            FieldTargetButton curBtn =

                // singles battle
                (team.teamMode == TeamMode.Single) ? (isAlly ? targetBtnNearSingle : targetBtnFarSingle)

                // doubles battle
                : (team.teamMode == TeamMode.Double) ?
                    (isAlly ?
                        ((position.battlePos == 0) ? targetBtnNearDouble0 : targetBtnNearDouble1)
                        : ((position.battlePos == 0) ? targetBtnFarDouble0 : targetBtnFarDouble1))

                // triples battle
                : (team.teamMode == TeamMode.Triple) ?
                    (isAlly ?
                        ((position.battlePos == 0) ? targetBtnNearTriple0
                            : (position.battlePos == 1) ? targetBtnNearTriple1 : targetBtnNearTriple2)
                        : ((position.battlePos == 0) ? targetBtnFarTriple0
                            : (position.battlePos == 1) ? targetBtnFarTriple1 : targetBtnFarTriple2))
                : null;
            return curBtn;
        }
        #endregion

        #region Field Targets
        /// <summary>
        /// Sets the field targets to display on this panel.
        /// </summary>
        /// <param name="teamPerspective">The team perspective used to determine the layout of buttons.</param>
        /// <param name="battleModel">The model used to determine which buttons to display.</param>
        public void SetFieldTargets(int teamPerspective, Model battleModel)
        {
            activeTargetBtns = new List<FieldTargetButton>();
            HideAllButtons();
            CleanAllButtons();

            List<BattlePosition> allPositions = battleModel.GetAllBattlePositions();
            for (int i = 0; i < allPositions.Count; i++)
            {
                BattlePosition curPos = allPositions[i];

                FieldTargetButton curBtn = GetFieldTargetButton(
                    position: curPos,
                    teamPerspective: teamPerspective,
                    battleModel: battleModel);

                if (curBtn != null)
                {
                    CreateFieldTargetBtn(curPos, battleModel, curBtn);
                    activeTargetBtns.Add(curBtn);
                    curBtn.gameObject.SetActive(true);
                }
            }
        }
        /// <summary>
        /// Associates the given battle position with the given field target button.
        /// </summary>
        /// <param name="position">The position to associate.</param>
        /// <param name="battleModel">Used to determine the pokemon associated with the given position.</param>
        /// <param name="btn">The button to associate.</param>
        public void CreateFieldTargetBtn(BattlePosition position, Model battleModel, FieldTargetButton btn)
        {
            btn.position = position;
            WifiFriendly.Pokemon pokemon = battleModel.GetPokemonAtPosition(position);

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

                btn.statusTxt.text = "";
                if (!string.IsNullOrEmpty(pokemon.nonVolatileStatus))
                {
                    StatusPKData statusData = PokemonStatuses.instance.GetStatusData(pokemon.nonVolatileStatus);
                    btn.statusTxt.text = statusData.shortName;
                }

                float hpPercent = ((float)pokemon.currentHP) / pokemon.maxHP;
                btn.hpBar.fillAmount = hpPercent;

                btn.hpBar.color = (hpPercent > 0.5f) ? btn.hpHigh
                    : (hpPercent > 0.25f) ? btn.hpMed
                    : btn.hpLow;

                // draw icon
                string drawPath = "pokemonSprites/icon/" + Pokemon.instance.GetPokemonData(pokemon.pokemonID).displayID;
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
        
        /// <summary>
        /// Highlights the selected battle positions, but unhighlights the rest.
        /// </summary>
        /// <param name="userPos">The position of the move user Pokemon.</param>
        /// <param name="targetPositions">The battle positions to highlight.</param>
        public void HighlightFieldTargets(BattlePosition userPos, List<BattlePosition> targetPositions)
        {
            backBtn.UnselectSelf();
            promptText.text = "Choose a target by scrolling left or right.";
            List<FieldTargetButton> fieldTargetBtns = activeTargetBtns;
            for (int i = 0; i < fieldTargetBtns.Count; i++)
            {
                bool posWasTargeted = false;
                FieldTargetButton curBtn = fieldTargetBtns[i];
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
        /// <summary>
        /// Highlights the back button on the UI.
        /// </summary>
        public void HighlightBackButton()
        {
            List<FieldTargetButton> allBtns = GetAllFieldTargetButtons();
            for (int i = 0; i < allBtns.Count; i++)
            {
                allBtns[i].UnselectSelf();
            }
            backBtn.SelectSelf();
            promptText.text = "Go back to Move select.";
        }
        #endregion
    }
}

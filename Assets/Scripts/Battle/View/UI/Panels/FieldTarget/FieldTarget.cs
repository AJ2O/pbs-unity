using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class FieldTarget : BasePanel
    {
        [Header("Buttons")]
        public Panels.FieldTargetButton targetBtnNearSingle;
        public Panels.FieldTargetButton targetBtnFarSingle,

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
        List<Panels.FieldTargetButton> activeTargetBtns;
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
            activeTargetBtns = new List<Panels.FieldTargetButton>();
            CleanAllButtons();
            HideAllButtons();
        }

        public void HideAllButtons()
        {
            List<Panels.FieldTargetButton> allBtns = GetAllFieldTargetButtons();
            for (int i = 0; i < allBtns.Count; i++)
            {
                Panels.FieldTargetButton curBtn = allBtns[i];
                curBtn.UnselectSelf();
                curBtn.gameObject.SetActive(false);
            }
        }
        public void CleanAllButtons()
        {
            List<Panels.FieldTargetButton> allBtns = GetAllFieldTargetButtons();
            for (int i = 0; i < allBtns.Count; i++)
            {
                Panels.FieldTargetButton curBtn = allBtns[i];
                curBtn.RefreshSelf();
                curBtn.position = null;
            }
        }
        public List<Panels.FieldTargetButton> GetAllFieldTargetButtons()
        {
            List<Panels.FieldTargetButton> allBtns = new List<Panels.FieldTargetButton>
            {
                targetBtnNearSingle, targetBtnFarSingle,
                targetBtnNearDouble0, targetBtnNearDouble1, targetBtnFarDouble0, targetBtnFarDouble1,
                targetBtnNearTriple0, targetBtnNearTriple1, targetBtnNearTriple2,
                targetBtnFarTriple0, targetBtnFarTriple1, targetBtnFarTriple2
            };
            return allBtns;
        }
        public Panels.FieldTargetButton GetFieldTargetButton(
            BattlePosition position, 
            int teamPerspective, 
            PBS.Battle.View.Model battleModel)
        {
            PBS.Battle.View.Compact.Team team = battleModel.GetMatchingTeam(position.teamPos);
            bool isAlly = (teamPerspective == position.teamPos);

            Panels.FieldTargetButton curBtn =

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

        public void SetFieldTargets(
            int teamPos, 
            PBS.Battle.View.Model battleModel)
        {
            activeTargetBtns = new List<Panels.FieldTargetButton>();
            HideAllButtons();
            CleanAllButtons();

            List<BattlePosition> allPositions = battleModel.GetAllBattlePositions();
            for (int i = 0; i < allPositions.Count; i++)
            {
                BattlePosition curPos = allPositions[i];
                PBS.Battle.View.Compact.Team team = battleModel.GetMatchingTeam(curPos.teamPos);
                bool isAlly = (teamPos == curPos.teamPos);

                Panels.FieldTargetButton curBtn = GetFieldTargetButton(
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
        public void CreateFieldTargetBtn(
            BattlePosition position, 
            PBS.Battle.View.Model battleModel, 
            Panels.FieldTargetButton btn)
        {
            btn.position = position;
            PBS.Battle.View.Compact.Pokemon pokemon = battleModel.GetPokemonAtPosition(position);

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
                    StatusPKData statusData = StatusPKDatabase.instance.GetStatusData(pokemon.nonVolatileStatus);
                    btn.statusTxt.text = statusData.shortName;
                }

                float hpPercent = ((float)pokemon.currentHP) / pokemon.maxHP;
                btn.hpBar.fillAmount = hpPercent;

                btn.hpBar.color = (hpPercent > 0.5f) ? btn.hpHigh
                    : (hpPercent > 0.25f) ? btn.hpMed
                    : btn.hpLow;

                // draw icon
                string drawPath = "pokemonSprites/icon/" + PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID).displayID;
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
            List<Panels.FieldTargetButton> fieldTargetBtns = activeTargetBtns;
            for (int i = 0; i < fieldTargetBtns.Count; i++)
            {
                bool posWasTargeted = false;
                Panels.FieldTargetButton curBtn = fieldTargetBtns[i];
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
            List<Panels.FieldTargetButton> allBtns = GetAllFieldTargetButtons();
            for (int i = 0; i < allBtns.Count; i++)
            {
                allBtns[i].UnselectSelf();
            }
            backBtn.SelectSelf();
            promptText.text = "Go back to Move select.";
        }
    }
}

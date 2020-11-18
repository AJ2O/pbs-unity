using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class Command : BasePanel
    {
        [Header("Prompts")]
        public GameObject promptBox;
        public Text promptText;

        [Header("Buttons")]
        public BTLUI_ButtonCmd fightBtn;
        public BTLUI_ButtonCmd partyBtn,
            bagBtn,
            runBtn;
        public BTLUI_Button backBtn;

        public void SetCommands(PBS.Battle.View.Compact.Pokemon pokemon, IEnumerable<BattleCommandType> commandList)
        {
            HashSet<BattleCommandType> commandSet = new HashSet<BattleCommandType>(commandList);
            fightBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Fight));
            partyBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Party));
            bagBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Bag));
            runBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Run));
            backBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Back));
            promptText.text = "What will <color=yellow>" + pokemon.nickname + "</color> do?";
        }

        public void HighlightCommand(BattleCommandType commandType)
        {
            fightBtn.UnselectSelf();
            partyBtn.UnselectSelf();
            bagBtn.UnselectSelf();
            runBtn.UnselectSelf();
            backBtn.UnselectSelf();

            switch (commandType)
            {
                case BattleCommandType.Fight:
                    fightBtn.SelectSelf();
                    break;

                case BattleCommandType.Party:
                    partyBtn.SelectSelf();
                    break;

                case BattleCommandType.Bag:
                    bagBtn.SelectSelf();
                    break;

                case BattleCommandType.Run:
                    runBtn.SelectSelf();
                    break;

                case BattleCommandType.Back:
                    backBtn.SelectSelf();
                    break;

                default:
                    break;
            }
        }
        public void HighlightBackButton()
        {
            HighlightCommand(BattleCommandType.None);
            backBtn.SelectSelf();
        }

        public void PromptPokemon(PBS.Battle.View.Compact.Pokemon pokemon)
        {
            promptText.text = "What will " + pokemon.nickname + " do?";
        }
    }
}

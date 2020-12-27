using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    /// <summary>
    /// This component handles the UI menu fir command selection.
    /// </summary>
    public class Command : BasePanel
    {
        #region Attributes
        [Header("Prompts")]
        public GameObject promptBox;
        public Text promptText;

        [Header("Buttons")]
        public BTLUI_ButtonCmd fightBtn;
        public BTLUI_ButtonCmd partyBtn,
            bagBtn,
            runBtn;
        public BTLUI_Button backBtn;
        #endregion

        #region Commands
        /// <summary>
        /// Sets the commands to be displayed on this component.
        /// </summary>
        /// <param name="pokemon">The pokemon to display commands for.</param>
        /// <param name="commandList">The list of commands to display.</param>
        public void SetCommands(WifiFriendly.Pokemon pokemon, IEnumerable<BattleCommandType> commandList)
        {
            HashSet<BattleCommandType> commandSet = new HashSet<BattleCommandType>(commandList);
            fightBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Fight));
            partyBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Party));
            bagBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Bag));
            runBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Run));
            backBtn.gameObject.SetActive(commandSet.Contains(BattleCommandType.Back));
            promptText.text = "What will <color=yellow>" + pokemon.nickname + "</color> do?";
        }
        /// <summary>
        /// Highlights the button matching the given command type, and unhighlights the rest.
        /// </summary>
        /// <param name="commandType">The command type to match.</param>
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

        /// <summary>
        /// Highlights the back button on the UI.
        /// </summary>
        public void HighlightBackButton()
        {
            HighlightCommand(BattleCommandType.None);
            backBtn.SelectSelf();
        }
        /// <summary>
        /// Displays a message prompt for the given Pokemon.
        /// </summary>
        /// <param name="pokemon"></param>
        public void PromptPokemon(WifiFriendly.Pokemon pokemon)
        {
            promptText.text = "What will " + pokemon.nickname + " do?";
        }
        #endregion
    }
}

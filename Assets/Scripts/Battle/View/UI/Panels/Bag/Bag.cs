using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    /// <summary>
    /// This component handles the UI menu for Bag Pocket selection.
    /// </summary>
    public class Bag : BasePanel
    {
        #region Attributes
        [Header("Buttons")]
        public BTLUI_Button bagBtnHealth;
        public BTLUI_Button bagBtnPokeballs,
            bagBtnStatus,
            bagBtnBattle;
        public BTLUI_Button backBtn;

        [Header("Text")]
        public Text promptText;
        #endregion

        #region Pockets

        /// <summary>
        /// Displays the buttons associated with the given bag pockets.
        /// </summary>
        /// <param name="battlePockets"></param>
        public void SetPockets(List<ItemBattlePocket> battlePockets)
        {
            bagBtnHealth.gameObject.SetActive(battlePockets.Contains(ItemBattlePocket.HPRestore));
            bagBtnPokeballs.gameObject.SetActive(battlePockets.Contains(ItemBattlePocket.Pokeballs));
            bagBtnStatus.gameObject.SetActive(battlePockets.Contains(ItemBattlePocket.StatusRestore));
            bagBtnBattle.gameObject.SetActive(battlePockets.Contains(ItemBattlePocket.BattleItems));
            bagBtnHealth.UnselectSelf();
            bagBtnPokeballs.UnselectSelf();
            bagBtnStatus.UnselectSelf();
            bagBtnBattle.UnselectSelf();

            backBtn.gameObject.SetActive(true);
        }

        /// <summary>
        /// Highlights the given pocket on the UI, and unhighlights the rest.
        /// </summary>
        /// <param name="battlePocket">The pocket to highlight.</param>
        public void HighlightPocket(ItemBattlePocket battlePocket)
        {
            bagBtnHealth.UnselectSelf();
            bagBtnPokeballs.UnselectSelf();
            bagBtnStatus.UnselectSelf();
            bagBtnBattle.UnselectSelf();
            backBtn.UnselectSelf();

            switch (battlePocket)
            {
                case ItemBattlePocket.HPRestore:
                    bagBtnHealth.SelectSelf();
                    promptText.text = "Items that recover Hit Points.";
                    break;

                case ItemBattlePocket.Pokeballs:
                    bagBtnPokeballs.SelectSelf();
                    promptText.text = "Items that can capture Pokémon.";
                    break;

                case ItemBattlePocket.StatusRestore:
                    bagBtnStatus.SelectSelf();
                    promptText.text = "Items that alleviate status conditions.";
                    break;

                case ItemBattlePocket.BattleItems:
                    bagBtnBattle.SelectSelf();
                    promptText.text = "Items that are particularly useful in-battle.";
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
            bagBtnHealth.UnselectSelf();
            bagBtnPokeballs.UnselectSelf();
            bagBtnStatus.UnselectSelf();
            bagBtnBattle.UnselectSelf();

            backBtn.SelectSelf();
            promptText.text = "Go back to commands.";
        }
        #endregion
    }
}

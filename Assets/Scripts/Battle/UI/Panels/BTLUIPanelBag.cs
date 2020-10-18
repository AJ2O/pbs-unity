using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUIPanelBag : BTLUIPanel
{
    [Header("Buttons")]
    public BTLUI_Button bagBtnHealth;
    public BTLUI_Button bagBtnPokeballs,
        bagBtnStatus,
        bagBtnBattle;
    public BTLUI_Button backBtn;

    [Header("Text")]
    public Text promptText;

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
    public void HighlightBackButton()
    {
        HighlightPocket(ItemBattlePocket.None);
        backBtn.SelectSelf();
        promptText.text = "Go back to commands.";
    }
}

using PBS.Main.Trainer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUIPanelBagItem : BTLUIPanel
{
    [Header("Buttons")]
    public BTLUI_ButtonItem bagBtn1;
    public BTLUI_ButtonItem bagBtn2,
        bagBtn3,
        bagBtn4;
    public BTLUI_Button scrollRightBtn,
        scrollLeftBtn;
    public BTLUI_Button backBtn;

    [Header("Text")]
    public Text promptText;
    public Text pageText;

    public int maxItemCount = 4;

    public void SetItems(Trainer trainer, List<Item> list, int offset)
    {
        bagBtn1.itemID = null;
        bagBtn2.itemID = null;
        bagBtn3.itemID = null;
        bagBtn4.itemID = null;
        for (int i = offset; i < offset + 4 && i < list.Count; i++)
        {
            Item item = list[i];
            BTLUI_ButtonItem curBtn = (i == 0) ? bagBtn1
                : (i == 1) ? bagBtn2
                : (i == 2) ? bagBtn3
                : (i == 3) ? bagBtn4
                : null;
            if (curBtn != null)
            {
                SetItemButton(trainer, item, curBtn);
                curBtn.gameObject.SetActive(true);
            }
        }

        int itemButtonCount = Mathf.Min(4, list.Count - offset);
        if (itemButtonCount < 4) bagBtn4.gameObject.SetActive(false);
        if (itemButtonCount < 3) bagBtn3.gameObject.SetActive(false);
        if (itemButtonCount < 2) bagBtn2.gameObject.SetActive(false);
        if (itemButtonCount < 1) bagBtn1.gameObject.SetActive(false);

        int totalPages = (list.Count / maxItemCount) + 1;
        int currentPage = (offset / maxItemCount) + 1;
        pageText.text = "Page " + currentPage + " / " + totalPages;
    }
    public void SetItemButton(Trainer trainer, Item item, BTLUI_ButtonItem button)
    {
        button.nameTxt.text = item.data.itemName;
        button.amountTxt.text = "x" + trainer.GetItemCount(item.itemID);

        // draw icon
        string drawPath = "itemSprites/" + item.data.ID;
        button.icon.sprite = BattleAssetLoader.instance.nullSprite;
        if (BattleAssetLoader.instance.loadedItemSprites.ContainsKey(drawPath))
        {
            button.icon.sprite = BattleAssetLoader.instance.loadedItemSprites[drawPath];
        }
        else
        {
            BattleAssetLoader.instance.LoadItem(item: item, image: button.icon);
        }

        button.itemID = item.itemID;
        button.UnselectSelf();
    }

    public void HighlightButton(Item item)
    {
        BTLUI_ButtonItem selectedBtn = null;
        scrollLeftBtn.UnselectSelf();
        scrollRightBtn.UnselectSelf();

        if (bagBtn1.itemID != null)
        {
            if (bagBtn1.itemID == item.itemID)
            {
                selectedBtn = bagBtn1;
                scrollLeftBtn.SelectSelf();
            }
            else
            {
                bagBtn1.UnselectSelf();
            }
        }
        if (bagBtn2.itemID != null)
        {
            if (bagBtn2.itemID == item.itemID)
            {
                selectedBtn = bagBtn2;
                scrollRightBtn.SelectSelf();
            }
            else
            {
                bagBtn2.UnselectSelf();
            }
        }
        if (bagBtn3.itemID != null)
        {
            if (bagBtn3.itemID == item.itemID)
            {
                selectedBtn = bagBtn3;
                scrollLeftBtn.SelectSelf();
            }
            else
            {
                bagBtn3.UnselectSelf();
            }
        }
        if (bagBtn4.itemID != null)
        {
            if (bagBtn4.itemID == item.itemID)
            {
                selectedBtn = bagBtn4;
                scrollRightBtn.SelectSelf();
            }
            else
            {
                bagBtn4.UnselectSelf();
            }
        }

        if (selectedBtn != null)
        {
            selectedBtn.SelectSelf();
            backBtn.UnselectSelf();
            promptText.text = "Choose an item.";
        }
    }
    public void HighlightBackButton()
    {
        bagBtn1.UnselectSelf();
        bagBtn2.UnselectSelf();
        bagBtn3.UnselectSelf();
        bagBtn4.UnselectSelf();

        backBtn.SelectSelf();
        promptText.text = "Go back to commands.";
    }
}

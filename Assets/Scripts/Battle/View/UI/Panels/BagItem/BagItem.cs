using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class BagItem : BasePanel
    {
        [Header("Buttons")]
        public Panels.BagItemButton bagBtn1;
        public Panels.BagItemButton bagBtn2,
            bagBtn3,
            bagBtn4;
        public Panels.BagItemButton scrollRightBtn,
            scrollLeftBtn;
        public Panels.BagItemButton backBtn;

        [Header("Text")]
        public Text promptText;
        public Text pageText;

        public int maxItemCount = 4;

        private void Awake()
        {
            backBtn.itemID = "";
        }

        public void SetItems(PBS.Battle.View.Compact.Trainer trainer, List<Item> list, int offset)
        {
            bagBtn1.itemID = null;
            bagBtn2.itemID = null;
            bagBtn3.itemID = null;
            bagBtn4.itemID = null;
            for (int i = offset; i < offset + 4 && i < list.Count; i++)
            {
                Item item = list[i];
                Panels.BagItemButton curBtn = ((i - offset) == 0) ? bagBtn1
                    : ((i - offset) == 1) ? bagBtn2
                    : ((i - offset) == 2) ? bagBtn3
                    : ((i - offset) == 3) ? bagBtn4
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
        public void SetItemButton(PBS.Battle.View.Compact.Trainer trainer, Item item, Panels.BagItemButton button)
        {
            button.nameTxt.text = item.data.itemName;
            int itemCount = 0;
            for (int i = 0; i < trainer.items.Count; i++)
            {
                if (trainer.items[i] == item.itemID)
                {
                    itemCount++;
                }
            }

            button.amountTxt.text = "x" + itemCount;

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

        public void HighlightButton(string itemID)
        {
            Panels.BagItemButton selectedBtn = null;
            scrollLeftBtn.UnselectSelf();
            scrollRightBtn.UnselectSelf();

            if (bagBtn1.itemID != null)
            {
                if (bagBtn1.itemID == itemID)
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
                if (bagBtn2.itemID == itemID)
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
                if (bagBtn3.itemID == itemID)
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
                if (bagBtn4.itemID == itemID)
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
}

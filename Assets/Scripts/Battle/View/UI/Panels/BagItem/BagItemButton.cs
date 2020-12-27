using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    /// <summary>
    /// This component handles displaying an item on the <seealso cref="BagItem"/> panel.
    /// </summary>
    public class BagItemButton : BaseButton
    {
        [Tooltip("Text displaying the name of the item.")]
        public Text nameTxt;
        [Tooltip("Text displaying the quantity of the item.")]
        public Text amountTxt;
        [Tooltip("An image of the item.")]
        public Image icon;

        [Tooltip("The ID of the item associated to this button.")]
        public string itemID = null;
    }
}


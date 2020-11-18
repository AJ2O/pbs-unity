using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class BagItemButton : BaseButton
    {
        public Text nameTxt;
        public Text amountTxt;
        public Image icon;

        public string itemID = null;
    }
}


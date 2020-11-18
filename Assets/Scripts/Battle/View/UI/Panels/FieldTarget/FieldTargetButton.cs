using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class FieldTargetButton : BaseButton
    {
        [Header("Text")]
        public Text nameTxt;
        public Text lvlTxt, 
            statusTxt;

        [Header("Health")]
        public GameObject hpObj;
        public Image icon,
            hpBar;
        public Color
            hpHigh,
            hpMed,
            hpLow;

        public Color colorUser;
        [HideInInspector] public BattlePosition position = null;

        public void RefreshSelf(bool active = true)
        {
            nameTxt.gameObject.SetActive(active);
            lvlTxt.gameObject.SetActive(active);
            statusTxt.gameObject.SetActive(active);
            hpObj.SetActive(active);
            icon.gameObject.SetActive(active);
        }
    }
}


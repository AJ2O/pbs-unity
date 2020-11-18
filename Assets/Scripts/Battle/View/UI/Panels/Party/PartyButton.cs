using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class PartyButton : BaseButton
    {
        [Header("Text")]
        public Text nameTxt;
        public Text lvlTxt,
            hpTxt,
            statusTxt;

        [Header("Health")]
        public Image icon;
        public Image hpBar;
        public Color
            hpHigh,
            hpMed,
            hpLow;

        [HideInInspector] public string pokemonUniqueID;
    }
}


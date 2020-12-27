using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    /// <summary>
    /// This component handles displaying a party member on the <seealso cref="Party"/> panel.
    /// </summary>
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


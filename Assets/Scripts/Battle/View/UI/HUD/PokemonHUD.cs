using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.HUD
{
    /// <summary>
    /// This component handles the HUD block of a Pokemon. This includes its name, gender,
    /// level, health, experience points, etc.
    /// </summary>
    public class PokemonHUD : MonoBehaviour
    {
        [Header("Text")]
        public Text nameTxt;
        public Text lvlTxt,
            statusTxt,
            hpTxt;

        [Header("Health")]
        public GameObject hpObj;
        public GameObject expObj;
        public Image expBar,
            hpBar;
        public Color
            hpHigh,
            hpMed,
            hpLow;

        /// <summary>
        /// The unique ID of the Pokemon that this HUD block is associated with.
        /// </summary>
        public string pokemonUniqueID;
    }
}

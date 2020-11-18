using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.HUD
{
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

        public string pokemonUniqueID;
    }
}

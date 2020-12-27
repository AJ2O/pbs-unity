using PBS.Main.Pokemon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUI_ButtonParty : BTLUI_Button
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

    [HideInInspector] public Pokemon pokemon = null;
}

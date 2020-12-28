using PBS.Data;
using PBS.Main.Pokemon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUI_ButtonFight : BTLUI_Button
{
    public Text moveTxt;
    public Text ppTxt;
    public Text typeTxt;
    [HideInInspector] public bool hiddenByZMove = false;
    [HideInInspector] public Moveslot moveslot = null;
    public Move moveData = null;
}

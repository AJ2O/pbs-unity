﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUI_ButtonFight : BTLUI_Button
{
    public Text moveTxt;
    public Text ppTxt;
    public Text typeTxt;
    [HideInInspector] public Pokemon.Moveslot moveslot = null;
    public MoveData moveData = null;
}

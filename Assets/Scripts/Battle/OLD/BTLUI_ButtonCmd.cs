using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUI_ButtonCmd : BTLUI_Button
{
    protected void Awake()
    {
        image.color = colorUnsel;
    }

    public Text txt;
    public BattleCommandType commandType;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class FightButton : BaseButton
    {
        public Text moveTxt;
        public Text ppTxt;
        public Text typeTxt;
        [HideInInspector] public string moveID;
        [HideInInspector] public Events.CommandAgent.Moveslot moveslot;
    }
}


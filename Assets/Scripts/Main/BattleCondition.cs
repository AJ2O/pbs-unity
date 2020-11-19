using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCondition
{
    // General
    public string statusID;
    public int turnsActive;
    public int turnsLeft;
    public StatusBTLData data
    {
        get
        {
            return StatusBTLDatabase.instance.GetStatusData(statusID);
        }
    }

    // Constructor
    public BattleCondition(
        string statusID,
        int turnsActive = 0,
        int turnsLeft = -1
        )
    {
        this.statusID = statusID;
        this.turnsActive = turnsActive;
        this.turnsLeft = turnsLeft;
    }

    // Clone
    public static BattleCondition Clone(BattleCondition original)
    {
        BattleCondition cloneAilment = new BattleCondition(
            statusID: original.statusID,
            turnsActive: original.turnsActive,
            turnsLeft: original.turnsLeft
            );
        return cloneAilment;
    }

    public void AdvanceSelf()
    {
        if (turnsLeft > 0)
        {
            turnsLeft--;
        }
    }

}

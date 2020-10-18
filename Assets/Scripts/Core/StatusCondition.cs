﻿using System.Collections;
using System.Collections.Generic;

public class StatusCondition
{
    // General
    public string statusID { get; set; }
    public int turnsActive { get; set; }
    public int turnsLeft { get; set; }
    public StatusPKData data
    {
        get
        {
            return StatusPKDatabase.instance.GetStatusData(this.statusID);
        }
    }

    // Constructor
    public StatusCondition(
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
    public static StatusCondition Clone(StatusCondition original)
    {
        StatusCondition cloneAilment = new StatusCondition(
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

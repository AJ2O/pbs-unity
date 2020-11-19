using System.Collections;
using System.Collections.Generic;

public class TeamCondition
{
    // General
    public string statusID;
    public int turnsActive;
    public int turnsLeft;
    public StatusTEData data
    {
        get
        {
            return StatusTEDatabase.instance.GetStatusData(statusID);
        }
    }

    // Constructor
    public TeamCondition(
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
    public static TeamCondition Clone(TeamCondition original)
    {
        TeamCondition cloneAilment = new TeamCondition(
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

using PBS.Data;
using PBS.Databases;
using System.Collections;
using System.Collections.Generic;

public class StatusCondition
{
    // General
    public string statusID;
    public int turnsActive;
    public int turnsLeft;
    public PokemonStatus data
    {
        get
        {
            return PokemonStatuses.instance.GetStatusData(this.statusID);
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

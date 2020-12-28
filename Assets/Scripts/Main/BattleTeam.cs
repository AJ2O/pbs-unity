using PBS.Main.Trainer;
using System.Collections;
using System.Collections.Generic;

public class BattleTeam
{
    public enum TeamMode
    {
        Single,
        Double,
        Triple
    }

    // General
    public int teamID;
    public TeamMode teamMode;

    // Trainers
    public List<Trainer> trainers;

    // Battle-only Properties
    public BattleTeamProperties bProps;

    // Constructor
    public BattleTeam(
        TeamMode teamMode = TeamMode.Single,
        List<Trainer> trainers = null,
        int teamID = 0,
        bool setupControl = true)
    {
        this.teamMode = teamMode;
        this.teamID = teamID;
        this.trainers = new List<Trainer>();
        if (trainers != null)
        {
            this.trainers.AddRange(trainers);
            if (setupControl)
            {
                SetTeamPos(this.teamID);
                SetUpTrainerControl();
            }
        }

        bProps = new BattleTeamProperties();
    }

    // Clone
    public static BattleTeam Clone(BattleTeam original)
    {
        List<Trainer> trainers = new List<Trainer>();
        for (int i = 0; i < original.trainers.Count; i++)
        {
            trainers.Add(original.trainers[i].Clone());
        }

        BattleTeam cloneTeam = new BattleTeam(
            teamMode: original.teamMode,
            trainers: trainers,
            teamID: original.teamID,
            setupControl: false
            );
        cloneTeam.bProps = BattleTeamProperties.Clone(original.bProps);
        return cloneTeam;
    }
    
    public void SetTeamPos(int pos)
    {
        teamID = pos;
        for (int i = 0; i < trainers.Count; i++)
        {
            trainers[i].teamID = pos;
        }
    }

    public void SetUpTrainerControl()
    {
        if (trainers.Count == 1 && teamMode != TeamMode.Single)
        {
            trainers[0].controlPos = (teamMode == TeamMode.Double) ? new int[] { 0, 1 }
            : (teamMode == TeamMode.Triple) ? new int[] { 0, 1, 2 }
            : new int[] { 0 };
        }
        else
        {
            // each trainer controls one position
            for (int i = 0; i < trainers.Count; i++)
            {
                trainers[i].controlPos = new int[] { i };
            }
        }
    }

    // Status Condition Methods
    public bool AddStatusCondition(TeamCondition condition)
    {
        if (!HasStatusCondition(condition.statusID))
        {
            bProps.conditions.Add(condition);
            return true;
        }
        return false;
    }
    public bool HasStatusCondition(string statusID)
    {
        for (int i = 0; i < bProps.conditions.Count; i++)
        {
            if (bProps.conditions[i].statusID == statusID)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveStatusCondition(string statusID)
    {
        List<TeamCondition> newStatusConditions = new List<TeamCondition>();
        for (int i = 0; i < bProps.conditions.Count; i++)
        {
            if (bProps.conditions[i].statusID != statusID)
            {
                newStatusConditions.Add(bProps.conditions[i]);
            }
        }
        bProps.conditions.Clear();
        bProps.conditions.AddRange(newStatusConditions);
    }
    public List<TeamCondition> GetAllStatusConditions()
    {
        List<TeamCondition> conditions = new List<TeamCondition>();
        conditions.AddRange(bProps.conditions);
        return conditions;
    }

    public bool IsPlayerOnTeam(int playerID)
    {
        for (int i = 0; i < trainers.Count; i++)
        {
            if (trainers[i].playerID == playerID)
            {
                return true;
            }
        }
        return false;
    }

}

public class BattleTeamProperties
{
    public class EntryHazard
    {
        public string hazardID;
        public int turnsLeft;
        public int layers;

        public EntryHazard(string moveID, int turnsLeft, int layers)
        {
            this.hazardID = moveID;
            this.turnsLeft = turnsLeft;
            this.layers = layers;
        }
        public static EntryHazard Clone(EntryHazard other)
        {
            return new EntryHazard(
                moveID: other.hazardID,
                turnsLeft: other.turnsLeft,
                layers: other.layers
                );
        }
    }
    public List<EntryHazard> entryHazards;
    
    public class GMaxWildfire
    {
        public string statusID;
        public int turnsLeft;
        public GMaxWildfire(
            string statusID,
            int turnsLeft
            )
        {
            this.statusID = statusID;
            this.turnsLeft = turnsLeft;
        }
        public GMaxWildfire Clone()
        {
            return new GMaxWildfire(statusID, turnsLeft);
        }
    }
    public GMaxWildfire GMaxWildfireStatus;

    public List<TeamCondition> lightScreens;

    public List<EffectDatabase.General.Protect> matBlocks;

    public class ReflectScreen
    {
        public string moveID;
        public int turnsLeft;

        public ReflectScreen(string moveID, int turnsLeft)
        {
            this.moveID = moveID;
            this.turnsLeft = turnsLeft;
        }
        public static ReflectScreen Clone(ReflectScreen other)
        {
            return new ReflectScreen(
                moveID: other.moveID,
                turnsLeft: other.turnsLeft
                );
        }
    }
    public List<ReflectScreen> reflectScreens;

    public class Safeguard
    {
        public string moveID;
        public int turnsLeft;

        public Safeguard(string moveID, int turnsLeft)
        {
            this.moveID = moveID;
            this.turnsLeft = turnsLeft;
        }
        public static Safeguard Clone(Safeguard other)
        {
            return new Safeguard(
                moveID: other.moveID,
                turnsLeft: other.turnsLeft
                );
        }
    }
    public List<Safeguard> safeguards;

    public List<string> protectMovesActive;
    public List<TeamCondition> conditions;

    // Constructor
    public BattleTeamProperties()
    {
        Reset();
    }

    // Clone
    public static BattleTeamProperties Clone(BattleTeamProperties original)
    {
        BattleTeamProperties clone = new BattleTeamProperties();

        for (int i = 0; i < original.entryHazards.Count; i++)
        {
            clone.entryHazards.Add(EntryHazard.Clone(original.entryHazards[i]));
        }

        clone.GMaxWildfireStatus = (original.GMaxWildfireStatus == null) ? null : original.GMaxWildfireStatus.Clone();

        for (int i = 0; i < original.lightScreens.Count; i++)
        {
            clone.lightScreens.Add(TeamCondition.Clone(original.lightScreens[i]));
        }
        for (int i = 0; i < original.matBlocks.Count; i++)
        {
            clone.matBlocks.Add(original.matBlocks[i].Clone());
        }

        for (int i = 0; i < original.reflectScreens.Count; i++)
        {
            clone.reflectScreens.Add(ReflectScreen.Clone(original.reflectScreens[i]));
        }

        for (int i = 0; i < original.safeguards.Count; i++)
        {
            clone.safeguards.Add(Safeguard.Clone(original.safeguards[i]));
        }

        clone.protectMovesActive = new List<string>(original.protectMovesActive);

        for (int i = 0; i < original.conditions.Count; i++)
        {
            clone.conditions.Add(TeamCondition.Clone(original.conditions[i]));
        }

        return clone;
    }

    public void Reset()
    {
        entryHazards = new List<EntryHazard>();
        GMaxWildfireStatus = null;
        lightScreens = new List<TeamCondition>();
        matBlocks = new List<EffectDatabase.General.Protect>();
        reflectScreens = new List<ReflectScreen>();
        safeguards = new List<Safeguard>();

        protectMovesActive = new List<string>();

        conditions = new List<TeamCondition>();
    }
}

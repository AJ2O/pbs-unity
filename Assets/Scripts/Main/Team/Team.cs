using PBS.Main.Trainer;
using System.Collections;
using System.Collections.Generic;

namespace PBS.Main.Team
{
    public class Team
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
        public List<Main.Trainer.Trainer> trainers;

        // Battle-only Properties
        public BattleProperties bProps;

        // Constructor
        public Team(
            TeamMode teamMode = TeamMode.Single,
            List<Main.Trainer.Trainer> trainers = null,
            int teamID = 0,
            bool setupControl = true)
        {
            this.teamMode = teamMode;
            this.teamID = teamID;
            this.trainers = new List<Main.Trainer.Trainer>();
            if (trainers != null)
            {
                this.trainers.AddRange(trainers);
                if (setupControl)
                {
                    SetTeamPos(this.teamID);
                    SetUpTrainerControl();
                }
            }

            bProps = new BattleProperties();
        }

        // Clone
        public static Team Clone(Team original)
        {
            List<Main.Trainer.Trainer> trainers = new List<Main.Trainer.Trainer>();
            for (int i = 0; i < original.trainers.Count; i++)
            {
                trainers.Add(original.trainers[i].Clone());
            }

            Team cloneTeam = new Team(
                teamMode: original.teamMode,
                trainers: trainers,
                teamID: original.teamID,
                setupControl: false
                );
            cloneTeam.bProps = BattleProperties.Clone(original.bProps);
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
                trainers[0].controlPos = teamMode == TeamMode.Double ? new int[] { 0, 1 }
                : teamMode == TeamMode.Triple ? new int[] { 0, 1, 2 }
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
}
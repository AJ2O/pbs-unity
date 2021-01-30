using PBS.Main.Trainer;
using System.Collections;
using System.Collections.Generic;

namespace PBS.Main.Team
{
    /// <summary>
    /// This class represents a team, which is composed of a group of trainers.
    /// </summary>
    public class Team
    {
        public enum TeamMode
        {
            Single,
            Double,
            Triple
        }

        #region Attributes
        /// <summary>
        /// The unique identifier for this team.
        /// </summary>
        public int teamID;
        /// <summary>
        /// The configuration assigned to this team.
        /// </summary>
        public TeamMode teamMode;

        /// <summary>
        /// The trainers on this team.
        /// </summary>
        public List<Trainer.Trainer> trainers;

        /// <summary>
        /// The battle-specific properties assigned to this team.
        /// </summary>
        public BattleProperties bProps;
        #endregion

        #region Constructor
        public Team(
            TeamMode teamMode = TeamMode.Single,
            List<Trainer.Trainer> trainers = null,
            int teamID = 0,
            bool setupControl = true)
        {
            this.teamMode = teamMode;
            this.teamID = teamID;
            this.trainers = new List<Trainer.Trainer>();
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
        #endregion

        #region Positioning Methods
        /// <summary>
        /// Assigns a team ID to this team.
        /// </summary>
        /// <param name="pos">The team ID to assign.</param>
        public void SetTeamPos(int pos)
        {
            teamID = pos;
            for (int i = 0; i < trainers.Count; i++)
            {
                trainers[i].teamID = pos;
            }
        }

        /// <summary>
        /// Automatically calculates the in-battle positions controlled by each trainer on this team.
        /// </summary>
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
        #endregion

        #region Status Condition Methods
        /// <summary>
        /// Adds the given status condition to this team.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool AddStatusCondition(TeamCondition condition)
        {
            if (!HasStatusCondition(condition.statusID))
            {
                bProps.conditions.Add(condition);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns true if this team has the given status.
        /// </summary>
        /// <param name="statusID"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Removes the given status from this team.
        /// </summary>
        /// <param name="statusID"></param>
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
        /// <summary>
        /// Returns all the status conditions inflicted on this team.
        /// </summary>
        /// <returns></returns>
        public List<TeamCondition> GetAllStatusConditions()
        {
            List<TeamCondition> conditions = new List<TeamCondition>();
            conditions.AddRange(bProps.conditions);
            return conditions;
        }
        #endregion

    }
}
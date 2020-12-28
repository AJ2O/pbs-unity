using System.Collections.Generic;

namespace PBS.Databases.Effects.TeamStatuses
{
    public class TeamSE
    {
        /// <summary>
        /// The type of battle status effect it is.
        /// </summary>
        public TeamSEType effectType;
        /// <summary>
        /// The time that this effect applies itself.
        /// </summary>
        public TeamSETiming timing;
        /// <summary>
        /// If true, this effect can stack where applicable.
        /// </summary>
        public bool isStackable;

        /// <summary>
        /// Additional restrictions on how the effect is applied.
        /// </summary>
        public List<Filter.FilterEffect> filters;

        public TeamSE(
            TeamSEType effectType,
            TeamSETiming timing = TeamSETiming.Unique,
            bool isStackable = false,
            IEnumerable<Filter.FilterEffect> filters = null
            )
        {
            this.effectType = effectType;
            this.timing = timing;
            this.isStackable = isStackable;
            this.filters = new List<Filter.FilterEffect>();
            if (filters != null)
            {
                List<Filter.FilterEffect> tempFilters = new List<Filter.FilterEffect>(filters);
                for (int i = 0; i < tempFilters.Count; i++)
                {
                    this.filters.Add(tempFilters[i].Clone());
                }
            }
        }
        public TeamSE Clone()
        {
            return
                this is GMaxWildfirePriority ? (this as GMaxWildfirePriority).Clone()
                : this is LightScreen ? (this as LightScreen).Clone()
                : this is HPLoss ? (this as HPLoss).Clone()
                :
                new TeamSE(
                    effectType: effectType,
                    timing: timing,
                    isStackable: isStackable,
                    filters: filters
                    );
        }
    }


    public class GMaxWildfirePriority : TeamSE
    {
        /// <summary>
        /// The priority value of this status condition. Low priority statuses can't overwrite 
        /// higher priority ones.
        /// </summary>
        public int priority;

        /// <summary>
        /// The text that displays when this status condition negates a lower priority one.
        /// </summary>
        public string negateTextID;

        public GMaxWildfirePriority(
            int priority = 1,
            string negateTextID = null
            )
            : base(TeamSEType.GMaxWildfirePriority)
        {
            this.priority = priority;
            this.negateTextID = negateTextID;
        }
        public new GMaxWildfirePriority Clone()
        {
            return new GMaxWildfirePriority(priority: priority, negateTextID: negateTextID);
        }
    }

    /// <summary>
    /// Damages affected Pokémon by a percentage of their maximum HP.
    /// </summary>
    public class HPLoss : TeamSE
    {
        /// <summary>
        /// The text to display if Pokémon lose HP.
        /// </summary>
        public string displayText;

        /// <summary>
        /// The percentage of HP lost from this effect.
        /// </summary>
        public float hpLossPercent;

        public HPLoss(
            string displayText = null,
            float hpLossPercent = 1f / 16,

            TeamSETiming timing = TeamSETiming.EndOfTurn,
            IEnumerable<Filter.FilterEffect> filters = null
            )
            : base(effectType: TeamSEType.HPLoss, timing: timing, filters: filters)
        {
            this.displayText = displayText;
            this.hpLossPercent = hpLossPercent;
        }
        public new HPLoss Clone()
        {
            return new HPLoss(
                displayText: displayText, hpLossPercent: hpLossPercent,

                timing: timing, filters: filters
                );
        }
    }

    /// <summary>
    /// Scales damage dealt to Pokémon on this team.
    /// </summary>
    public class LightScreen : TeamSE
    {
        /// <summary>
        /// The amount that damage is scaled by when used against a single target.
        /// </summary>
        public float damageMultiplier;
        /// <summary>
        /// The amount that damage is scaled by when used against multiple targets.
        /// </summary>
        public float damageMultiMultiplier;
        /// <summary>
        /// If true, this effect can be removed by <seealso cref="Abilities.ScreenCleaner"/>.
        /// </summary>
        public bool canBeScreenCleaned;
        /// <summary>
        /// If true, this effect can be bypassed by <seealso cref="Abilities.Infiltrator"/>.
        /// </summary>
        public bool canBeInfiltrated;

        public LightScreen(
            float damageMultiplier = 0.5f,
            float damageMultiMultiplier = 2f / 3,
            bool canBeScreenCleaned = true, bool canBeInfiltrated = true,
            IEnumerable<Filter.FilterEffect> filters = null)
            : base(effectType: TeamSEType.LightScreen, filters: filters)
        {
            this.damageMultiplier = damageMultiplier;
            this.damageMultiMultiplier = damageMultiMultiplier;
            this.canBeScreenCleaned = canBeScreenCleaned;
            this.canBeInfiltrated = canBeInfiltrated;
        }
        public new LightScreen Clone()
        {
            return new LightScreen(
                damageMultiplier: damageMultiplier,
                damageMultiMultiplier: damageMultiMultiplier,
                canBeScreenCleaned: canBeScreenCleaned, canBeInfiltrated: canBeInfiltrated,
                filters: filters
                );
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusTEData
{
    public string ID { get; private set; }
    public string baseID { get; private set; }
    private string p_conditionName;
    public string conditionName
    {
        get
        {
            if (string.IsNullOrEmpty(p_conditionName) && !string.IsNullOrEmpty(baseID))
            {
                return MoveDatabase.instance.GetMoveData(baseID).moveName;
            }
            return p_conditionName;
        }
        private set
        {
            p_conditionName = value;
        }
    }

    public string inflictTextID { get; set; }
    public string natureTextID { get; set; }
    public string endTextID { get; set; }
    public string failTextID { get; set; }
    public string alreadyTextID { get; set; }

    // Turns
    private EffectDatabase.General.DefaultTurns p_defaultTurns { get; set; }
    public EffectDatabase.General.DefaultTurns defaultTurns
    {
        get
        {
            if (p_defaultTurns == null && !string.IsNullOrEmpty(baseID))
            {
                return StatusBTLDatabase.instance.GetStatusData(baseID).defaultTurns;
            }
            return p_defaultTurns;
        }
        set
        {
            p_defaultTurns = value;
        }
    }

    // Tags
    public HashSet<TeamSTag> tags { get; set; }
    // Status Condition Effects
    public TeamCEff[] conditionEffects { get; private set; }

    // New Effects
    private bool combineBaseEffects;
    private List<EffectDatabase.StatusTEEff.TeamSE> p_effectsNew { get; set; }
    public List<EffectDatabase.StatusTEEff.TeamSE> effectsNew
    {
        get
        {
            if (combineBaseEffects && !string.IsNullOrEmpty(baseID))
            {
                List<EffectDatabase.StatusTEEff.TeamSE> unionEffects = new List<EffectDatabase.StatusTEEff.TeamSE>();
                unionEffects.AddRange(p_effectsNew);
                unionEffects.AddRange(StatusTEDatabase.instance.GetStatusData(baseID).effectsNew);
                return unionEffects;
            }
            return p_effectsNew;
        }
        private set
        {
            p_effectsNew = value;
        }
    }

    // Constructor
    public StatusTEData(
        string ID,
        string baseID = null,
        string conditionName = null,
        string inflictTextID = null,
        string natureTextID = null,
        string endTextID = null,
        string alreadyTextID = null,
        string failTextID = null,
        EffectDatabase.General.DefaultTurns defaultTurns = null,

        IEnumerable<TeamSTag> tags = null,
        TeamCEff[] conditionEffects = null,

        bool combineBaseEffects = false, EffectDatabase.StatusTEEff.TeamSE[] effectsNew = null)
    {
        this.ID = ID;
        this.baseID = baseID;
        this.conditionName = conditionName;
        this.inflictTextID = inflictTextID;
        this.natureTextID = natureTextID;
        this.endTextID = endTextID;
        this.alreadyTextID = alreadyTextID;
        this.failTextID = failTextID;

        this.defaultTurns = (defaultTurns == null) ? null : defaultTurns.Clone();

        this.tags = new HashSet<TeamSTag>();
        if (tags != null)
        {
            this.tags.UnionWith(tags);
        }
        this.conditionEffects = (conditionEffects == null) ? new TeamCEff[0]
            : new TeamCEff[conditionEffects.Length];
        if (conditionEffects != null)
        {
            for (int i = 0; i < conditionEffects.Length; i++)
            {
                this.conditionEffects[i] = TeamCEff.Clone(conditionEffects[i]);
            }
        }

        this.combineBaseEffects = combineBaseEffects;
        this.effectsNew = new List<EffectDatabase.StatusTEEff.TeamSE>();
        if (effectsNew != null)
        {
            List<EffectDatabase.StatusTEEff.TeamSE> addableEffects = new List<EffectDatabase.StatusTEEff.TeamSE>();
            for (int i = 0; i < effectsNew.Length; i++)
            {
                addableEffects.Add(effectsNew[i].Clone());
            }
            this.effectsNew = addableEffects;
        }
    }

    // Clone
    public StatusTEData Clone()
    {
        StatusTEData cloneData = new StatusTEData(
            ID: ID,
            baseID: baseID,
            conditionName: conditionName,
            inflictTextID: inflictTextID,
            natureTextID: natureTextID,
            endTextID: endTextID,
            alreadyTextID: alreadyTextID,
            failTextID: failTextID,

            defaultTurns: defaultTurns,

            tags: tags,
            conditionEffects: conditionEffects,

            combineBaseEffects: combineBaseEffects,
            effectsNew: effectsNew.ToArray()
            );
        return cloneData;
    }

    public bool IsABaseID(string tryBaseID)
    {
        if (baseID == tryBaseID)
        {
            return baseID != null;
        }
        if (baseID != null)
        {
            StatusTEData baseData = StatusTEDatabase.instance.GetStatusData(baseID);
            return baseData.IsABaseID(tryBaseID);
        }
        return false;
    }

    public bool HasTag(TeamSTag statusTag)
    {
        return tags.Contains(statusTag);
    }

    public List<TeamCEff> GetEffectsFiltered(TeamSETiming timing)
    {
        List<TeamCEff> effects = new List<TeamCEff>();
        for (int i = 0; i < conditionEffects.Length; i++)
        {
            if (conditionEffects[i].effectTiming == timing
                || conditionEffects[i].effectTiming == TeamSETiming.Any)
            {
                effects.Add(conditionEffects[i]);
            }
        }
        return effects;
    }
    public TeamCEff GetEffect(TeamSEType effectType)
    {
        for (int i = 0; i < conditionEffects.Length; i++)
        {
            if (conditionEffects[i].effectType == effectType)
            {
                return conditionEffects[i];
            }
        }
        return null;
    }
    public List<TeamCEff> GetEffects(TeamSEType effectType)
    {
        List<TeamCEff> effects = new List<TeamCEff>();
        for (int i = 0; i < conditionEffects.Length; i++)
        {
            if (conditionEffects[i].effectType == effectType)
            {
                effects.Add(conditionEffects[i]);
            }
        }
        return effects;
    }

    public List<EffectDatabase.StatusTEEff.TeamSE> GetEffectsNewFiltered(TeamSETiming timing)
    {
        List<EffectDatabase.StatusTEEff.TeamSE> effects = new List<EffectDatabase.StatusTEEff.TeamSE>();
        for (int i = 0; i < effectsNew.Count; i++)
        {
            if (effectsNew[i].timing == timing)
            {
                effects.Add(effectsNew[i]);
            }
        }
        return effects;
    }
    public List<EffectDatabase.StatusTEEff.TeamSE> GetEffectsNew(TeamSEType effectType)
    {
        List<EffectDatabase.StatusTEEff.TeamSE> effects = new List<EffectDatabase.StatusTEEff.TeamSE>();
        for (int i = 0; i < effectsNew.Count; i++)
        {
            if (effectsNew[i].effectType == effectType)
            {
                effects.Add(effectsNew[i]);
            }
        }
        return effects;
    }
    public EffectDatabase.StatusTEEff.TeamSE GetEffectNew(TeamSEType effectType)
    {
        for (int i = 0; i < effectsNew.Count; i++)
        {
            if (effectsNew[i].effectType == effectType)
            {
                return effectsNew[i];
            }
        }
        return null;
    }

    public void AddEffects(IEnumerable<EffectDatabase.StatusTEEff.TeamSE> effects, bool before = true)
    {
        if (before)
        {
            List<EffectDatabase.StatusTEEff.TeamSE> unionEffects = new List<EffectDatabase.StatusTEEff.TeamSE>(effects);
            unionEffects.AddRange(p_effectsNew);
            effectsNew = unionEffects;
        }
        else
        {
            p_effectsNew.AddRange(effects);
        }
    }
    public void SetEffects(IEnumerable<EffectDatabase.StatusTEEff.TeamSE> effects)
    {
        effectsNew = new List<EffectDatabase.StatusTEEff.TeamSE>(effects);
    }
}

public class TeamCEff : GameEffect
{
    public TeamSEType effectType { get; set; }
    public TeamSETiming effectTiming { get; set; }
    public bool forceEffectDisplay { get; set; }

    // Constructor
    public TeamCEff(
        TeamSEType effectType,
        TeamSETiming effectTiming = TeamSETiming.Unique,
        bool forceEffectDisplay = false,
        bool[] boolParams = null,
        float[] floatParams = null,
        string[] stringParams = null
        ) : base(boolParams, floatParams, stringParams)
    {
        this.effectType = effectType;
        this.effectTiming = effectTiming;
        this.forceEffectDisplay = forceEffectDisplay;
    }

    // Clone
    public static TeamCEff Clone(TeamCEff original)
    {
        TeamCEff cloneEffect = new TeamCEff(
            effectType: original.effectType,
            effectTiming: original.effectTiming,
            forceEffectDisplay: original.forceEffectDisplay,
            boolParams: original.boolParams,
            floatParams: original.floatParams,
            stringParams: original.stringParams
            );
        return cloneEffect;
    }

}

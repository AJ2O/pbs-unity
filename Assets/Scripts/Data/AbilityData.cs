using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData
{
    // General
    public string ID { get; private set; }
    public string baseID { get; private set; }
    private string p_abilityName;
    public string abilityName
    {
        get
        {
            if (string.IsNullOrEmpty(p_abilityName) && !string.IsNullOrEmpty(baseID))
            {
                return AbilityDatabase.instance.GetAbilityData(baseID).abilityName;
            }
            return p_abilityName;
        }
        private set
        {
            p_abilityName = value;
        }
    }

    // Tags
    HashSet<AbilityTag> tags { get; set; }

    // Move Effects
    public AbilityEffect[] effects { get; private set; }

    // New Effects
    private bool combineBaseEffects;
    private List<EffectDatabase.AbilityEff.AbilityEffect> p_effectsNew { get; set; }
    public List<EffectDatabase.AbilityEff.AbilityEffect> effectsNew
    {
        get
        {
            if (combineBaseEffects && !string.IsNullOrEmpty(baseID))
            {
                List<EffectDatabase.AbilityEff.AbilityEffect> unionEffects = 
                    new List<EffectDatabase.AbilityEff.AbilityEffect>();
                unionEffects.AddRange(p_effectsNew);
                unionEffects.AddRange(AbilityDatabase.instance.GetAbilityData(baseID).effectsNew);
                return unionEffects;
            }
            return p_effectsNew;
        }
        private set
        {
            p_effectsNew = value;
        }
    }

    public AbilityData(
        string ID,
        string baseID = null,
        string abilityName = null,

        IEnumerable<AbilityTag> tags = null,

        AbilityEffect[] effects = null,

        bool combineBaseEffects = false,
        EffectDatabase.AbilityEff.AbilityEffect[] effectsNew = null)
    {
        this.ID = ID;
        this.baseID = baseID;
        this.abilityName = abilityName;

        this.tags = new HashSet<AbilityTag>();
        if (tags != null)
        {
            this.tags.UnionWith(tags);
        }

        this.effects = (effects == null) ? new AbilityEffect[0] : new AbilityEffect[effects.Length];
        if (effects != null)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                this.effects[i] = AbilityEffect.Clone(effects[i]);
            }
        }

        this.combineBaseEffects = combineBaseEffects;
        this.effectsNew = new List<EffectDatabase.AbilityEff.AbilityEffect>();
        if (effectsNew != null)
        {
            List<EffectDatabase.AbilityEff.AbilityEffect> addableEffects = new List<EffectDatabase.AbilityEff.AbilityEffect>();
            for (int i = 0; i < effectsNew.Length; i++)
            {
                addableEffects.Add(effectsNew[i].Clone());
            }
            this.effectsNew = addableEffects;
        }
    }

    public bool HasTag(AbilityTag tag)
    {
        return tags.Contains(tag);
    }

    public AbilityEffect GetEffect(AbilityEffectType effectType)
    {
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].effectType == effectType)
            {
                return effects[i];
            }
        }
        return null;
    }


    public List<EffectDatabase.AbilityEff.AbilityEffect> GetEffectsNew(AbilityEffectType effectType)
    {
        List<EffectDatabase.AbilityEff.AbilityEffect> effects = new List<EffectDatabase.AbilityEff.AbilityEffect>();
        for (int i = 0; i < effectsNew.Count; i++)
        {
            if (effectsNew[i].effectType == effectType)
            {
                effects.Add(effectsNew[i]);
            }
        }
        return effects;
    }
    public EffectDatabase.AbilityEff.AbilityEffect GetEffectNew(AbilityEffectType effectType)
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

}

public class AbilityEffect : GameEffect
{
    public AbilityEffectType effectType { get; set; }

    // Constructor
    public AbilityEffect(
        AbilityEffectType effectType,
        bool[] boolParams = null,
        float[] floatParams = null,
        string[] stringParams = null
        ) : base(boolParams, floatParams, stringParams)
    {
        this.effectType = effectType;
    }

    // Clone
    public static AbilityEffect Clone(AbilityEffect original)
    {
        AbilityEffect cloneEffect = new AbilityEffect(
            effectType: original.effectType,
            boolParams: original.boolParams,
            floatParams: original.floatParams,
            stringParams: original.stringParams
            );
        return cloneEffect;
    }
}

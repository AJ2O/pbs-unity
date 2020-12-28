using PBS.Databases;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeData
{
    // General
    public string ID { get; private set; }
    public string baseID { get; private set; }
    private string p_typeName { get; set; }
    public string typeName
    {
        get
        {
            if (string.IsNullOrEmpty(p_typeName) && !string.IsNullOrEmpty(baseID))
            {
                return ElementalTypes.instance.GetTypeData(baseID).typeName;
            }
            return p_typeName;
        }
        private set
        {
            p_typeName = value;
        }
    }

    // Properties
    private bool combineResistances;
    private List<string> p_resistances { get; set; }
    public List<string> resistances 
    {
        get
        {
            if (combineResistances && !string.IsNullOrEmpty(baseID))
            {
                List<string> unionList = new List<string>(p_resistances);
                unionList.AddRange(ElementalTypes.instance.GetTypeData(baseID).resistances);
                return unionList;
            }
            return p_resistances;
        }
        private set
        {
            p_resistances = value;
        } 
    }
    
    private bool combineWeaknesses;
    private List<string> p_weaknesses { get; set; }
    public List<string> weaknesses
    {
        get
        {
            if (combineWeaknesses && !string.IsNullOrEmpty(baseID))
            {
                List<string> unionList = new List<string>(p_weaknesses);
                unionList.AddRange(ElementalTypes.instance.GetTypeData(baseID).weaknesses);
                return unionList;
            }
            return p_weaknesses;
        }
        private set
        {
            p_weaknesses = value;
        }
    }

    private bool combineImmunities;
    private List<string> p_immunities { get; set; }
    public List<string> immunities
    {
        get
        {
            if (combineImmunities && !string.IsNullOrEmpty(baseID))
            {
                List<string> unionList = new List<string>(p_immunities);
                unionList.AddRange(ElementalTypes.instance.GetTypeData(baseID).immunities);
                return unionList;
            }
            return p_immunities;
        }
        private set
        {
            p_immunities = value;
        }
    }

    // Tags
    private bool combineTags;
    private HashSet<TypeTag> p_tags { get; set; }
    public HashSet<TypeTag> tags 
    {
        get
        {
            if (combineTags && !string.IsNullOrEmpty(baseID))
            {
                HashSet<TypeTag> unionTags = new HashSet<TypeTag>(p_tags);
                unionTags.UnionWith(ElementalTypes.instance.GetTypeData(baseID).tags);
                return unionTags;
            }
            return p_tags;
        }
        private set
        {
            p_tags = value;
        } 
    }

    // Type Effects
    private bool combineEffects;
    private List<TypeEffect> p_effects { get; set; }
    public List<TypeEffect> effects 
    {
        get
        {
            if (combineEffects && !string.IsNullOrEmpty(baseID))
            {
                List<TypeEffect> unionList = new List<TypeEffect>(p_effects);
                unionList.AddRange(ElementalTypes.instance.GetTypeData(baseID).effects);
                return unionList;
            }
            return p_effects;
        }
        private set
        {
            p_effects = value;
        }
    }

    private List<Effects.TypeEff.TypeEffect> p_effectsNew { get; set; }
    public List<Effects.TypeEff.TypeEffect> effectsNew
    {
        get
        {
            if (combineEffects && !string.IsNullOrEmpty(baseID))
            {
                List<Effects.TypeEff.TypeEffect> unionEffects = new List<Effects.TypeEff.TypeEffect>();
                unionEffects.AddRange(p_effectsNew);
                unionEffects.AddRange(ElementalTypes.instance.GetTypeData(baseID).effectsNew);
                return unionEffects;
            }
            return p_effectsNew;
        }
        private set
        {
            p_effectsNew = value;
        }
    }

    // Aesthetics
    public string typeColor { get; set; }

    // Other Mechanics
    public string maxMove { get; set; }

    public TypeData(
        string ID,
        string baseID = null,
        string typeName = "",
        string typeColor = "ffffff",
        string maxMove = "maxstrike",

        bool combineResistances = false, IEnumerable<string> resistances = null,
        bool combineWeaknesses = false, IEnumerable<string> weaknesses = null,
        bool combineImmunities = false, IEnumerable<string> immunities = null,

        bool combineTags = false, IEnumerable<TypeTag> tags = null,

        bool combineEffects = false, TypeEffect[] effects = null,
        Effects.TypeEff.TypeEffect[] effectsNew = null)
    {
        this.ID = ID;
        this.baseID = baseID;
        this.typeName = typeName;

        this.combineResistances = combineResistances;
        this.resistances = (resistances == null) ? new List<string>() { "" } : new List<string>(resistances);
        this.combineWeaknesses = combineWeaknesses;
        this.weaknesses = (weaknesses == null) ? new List<string>() { "" } : new List<string>(weaknesses);
        this.combineImmunities = combineImmunities;
        this.immunities = (immunities == null) ? new List<string>() { "" } : new List<string>(immunities);

        this.combineTags = combineTags;
        this.tags = (tags == null) ? new HashSet<TypeTag>() : new HashSet<TypeTag>(tags);

        this.combineEffects = combineEffects;
        this.effects = new List<TypeEffect>();
        if (effects != null)
        {
            List<TypeEffect> newEffects = new List<TypeEffect>();
            for (int i = 0; i < effects.Length; i++)
            {
                newEffects.Add(TypeEffect.Clone(effects[i]));
            }
            this.effects = new List<TypeEffect>(newEffects);
        }

        this.effectsNew = new List<Effects.TypeEff.TypeEffect>();
        if (effectsNew != null)
        {
            List<Effects.TypeEff.TypeEffect> addableEffects = new List<Effects.TypeEff.TypeEffect>();
            for (int i = 0; i < effectsNew.Length; i++)
            {
                addableEffects.Add(effectsNew[i].Clone());
            }
            this.effectsNew = addableEffects;
        }

        this.typeColor = typeColor;
        this.maxMove = maxMove;
    }

    public bool IsABaseID(string tryBaseID)
    {
        if (baseID == tryBaseID)
        {
            return baseID != null;
        }
        if (baseID != null)
        {
            TypeData baseData = ElementalTypes.instance.GetTypeData(baseID);
            return baseData.IsABaseID(tryBaseID);
        }
        return false;
    }

    public bool HasTag(TypeTag typeTag)
    {
        return tags.Contains(typeTag);
    }

    public TypeEffect GetEffect(TypeEffectType effectType)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].effectType == effectType)
            {
                return effects[i];
            }
        }
        return null;
    }
}

public class TypeEffect : GameEffect
{
    public TypeEffectType effectType { get; set; }
    public bool forceEffectDisplay { get; set; }

    // Constructor
    public TypeEffect(
        TypeEffectType effectType,
        bool forceEffectDisplay = false,
        bool[] boolParams = null,
        float[] floatParams = null,
        string[] stringParams = null
        ) : base(boolParams, floatParams, stringParams)
    {
        this.effectType = effectType;
        this.forceEffectDisplay = forceEffectDisplay;
    }

    // Clone
    public static TypeEffect Clone(TypeEffect original)
    {
        TypeEffect cloneEffect = new TypeEffect(
            effectType: original.effectType,
            forceEffectDisplay: original.forceEffectDisplay,
            boolParams: original.boolParams,
            floatParams: original.floatParams,
            stringParams: original.stringParams
            );
        return cloneEffect;
    }


}

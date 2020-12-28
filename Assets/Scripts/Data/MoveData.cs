using PBS.Databases;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveData
{
    // General
    public string ID { get; private set; }
    public string baseID { get; private set; }
    
    private string p_moveName;
    public string moveName
    {
        get
        {
            if (string.IsNullOrEmpty(p_moveName) && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).moveName;
            }
            return p_moveName;
        }
        private set
        {
            p_moveName = value;
        }
    }
    
    private string p_moveType { get; set; }
    public string moveType 
    {
        get
        {
            if (string.IsNullOrEmpty(p_moveType) && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).moveType;
            }
            return p_moveType;
        }
        set
        {
            p_moveType = value;
        }
    }
    
    private bool useSameCategory;
    private MoveCategory p_category { get; set; }
    public MoveCategory category 
    {
        get
        {
            if (useSameCategory && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).category;
            }
            return p_category;
        }
        set
        {
            p_category = value;
        } 
    }
    
    private bool useSameTargetType;
    private MoveTargetType p_targetType { get; set; }
    public MoveTargetType targetType 
    {
        get
        {
            if (useSameTargetType && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).targetType;
            }
            return p_targetType;
        }
        set
        {
            p_targetType = value;
        }
    }

    // Stats
    private bool useSameBasePower;
    private int p_basePower { get; set; }
    public int basePower 
    {
        get
        {
            if (useSameBasePower && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).basePower;
            }
            return p_basePower;
        }
        set
        {
            p_basePower = value;
        }
    }

    private bool useSameAccuracy;
    private float p_accuracy { get; set; }
    public float accuracy 
    {
        get
        {
            if (useSameAccuracy && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).accuracy;
            }
            return p_accuracy;
        }
        set
        {
            p_accuracy = value;
        }
    }

    private bool useSamePP;
    private int p_PP { get; set; }
    public int PP 
    {
        get
        {
            if (useSamePP && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).PP;
            }
            return p_PP;
        }
        set
        {
            p_PP = value;
        }
    }

    private bool useSamePriority;
    private int p_priority { get; set; }
    public int priority 
    {
        get
        {
            if (useSamePriority && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).priority;
            }
            return p_priority;
        }
        set
        {
            p_priority = value;
        }
    }

    // Tags
    private bool combineBaseTags;
    private HashSet<MoveTag> p_moveTags { get; set; }
    public HashSet<MoveTag> moveTags 
    {
        get
        {
            if (combineBaseTags && !string.IsNullOrEmpty(baseID))
            {
                HashSet<MoveTag> unionTags = new HashSet<MoveTag>(p_moveTags);
                unionTags.UnionWith(Moves.instance.GetMoveData(baseID).moveTags);
                return unionTags;
            }
            return p_moveTags;
        }
        set
        {
            p_moveTags = value;
        }
    }

    // Move Effects
    public List<MoveEffect> moveEffects { get; private set; }

    // New Effects
    private bool combineBaseEffects;
    private List<PBS.Databases.Effects.Moves.MoveEffect> p_effectsNew { get; set; }
    public List<PBS.Databases.Effects.Moves.MoveEffect> effectsNew 
    {
        get
        {
            if (combineBaseEffects && !string.IsNullOrEmpty(baseID))
            {
                List<PBS.Databases.Effects.Moves.MoveEffect> unionEffects = new List<PBS.Databases.Effects.Moves.MoveEffect>();
                unionEffects.AddRange(p_effectsNew);
                unionEffects.AddRange(Moves.instance.GetMoveData(baseID).effectsNew);
                return unionEffects;
            }
            return p_effectsNew;
        }
        private set
        {
            p_effectsNew = value;
        }
    }

    // Z-Moves
    private int p_ZBasePower { get; set; }
    public int ZBasePower
    {
        get
        {
            if (useSameBasePower && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).ZBasePower;
            }
            return p_ZBasePower;
        }
        set
        {
            p_ZBasePower = value;
        }
    }

    // Max Moves
    private int p_MaxPower { get; set; }
    public int MaxPower
    {
        get
        {
            if (useSameBasePower && !string.IsNullOrEmpty(baseID))
            {
                return Moves.instance.GetMoveData(baseID).MaxPower;
            }
            return p_MaxPower;
        }
        set
        {
            p_MaxPower = value;
        }
    }


    private List<PBS.Databases.Effects.Moves.MoveEffect> p_ZEffectsNew { get; set; }
    public List<PBS.Databases.Effects.Moves.MoveEffect> ZEffectsNew
    {
        get
        {
            if (combineBaseEffects && !string.IsNullOrEmpty(baseID))
            {
                List<PBS.Databases.Effects.Moves.MoveEffect> unionEffects = new List<PBS.Databases.Effects.Moves.MoveEffect>();
                unionEffects.AddRange(p_ZEffectsNew);
                unionEffects.AddRange(Moves.instance.GetMoveData(baseID).ZEffectsNew);
                return unionEffects;
            }
            return p_ZEffectsNew;
        }
        private set
        {
            p_ZEffectsNew = value;
        }
    }

    // TODO: Contest Stats / Effects

    // Constructor
    public MoveData(
        string ID,
        string baseID = null,
        string moveName = null,
        string moveType = "",
        bool useSameCategory = false, MoveCategory category = MoveCategory.Status,
        bool useSameTargetType = false, MoveTargetType targetType = MoveTargetType.Self,

        bool useSameBasePower = false, bool useSameAccuracy = false, bool useSamePP = false,
        int basePower = 0, float accuracy = -1, int PP = 5,
        bool useSamePriority = false,
        int priority = 0,

        bool combineBaseTags = false,
        IEnumerable<MoveTag> moveTags = null,
        
        MoveEffect[] moveEffects = null,

        bool combineBaseEffects = false,
        PBS.Databases.Effects.Moves.MoveEffect[] effectsNew = null,
        
        int ZBasePower = 0,
        PBS.Databases.Effects.Moves.MoveEffect[] ZEffectsNew = null,
        
        int MaxPower = 0)
    {
        this.ID = ID;
        this.baseID = baseID;
        this.moveName = moveName;
        this.moveType = moveType;
        this.useSameCategory = useSameCategory;
        this.category = category;
        this.useSameTargetType = useSameTargetType;
        this.targetType = targetType;

        this.useSameBasePower = useSameBasePower;
        this.basePower = basePower;
        this.useSameAccuracy = useSameAccuracy;
        this.accuracy = accuracy;
        this.useSamePP = useSamePP;
        this.PP = PP;
        this.useSamePriority = useSamePriority;
        this.priority = priority;

        this.combineBaseTags = combineBaseTags;
        this.moveTags = new HashSet<MoveTag>();
        if (moveTags != null)
        {
            this.moveTags.UnionWith(moveTags);
        }

        this.moveEffects = new List<MoveEffect>();
        if (moveEffects != null)
        {
            for (int i = 0; i < moveEffects.Length; i++)
            {
                this.moveEffects.Add (MoveEffect.Clone(moveEffects[i]));
            }
        }

        this.combineBaseEffects = combineBaseEffects;
        this.effectsNew = new List<PBS.Databases.Effects.Moves.MoveEffect>();
        if (effectsNew != null)
        {
            List<PBS.Databases.Effects.Moves.MoveEffect> addableEffects = new List<PBS.Databases.Effects.Moves.MoveEffect>();
            for (int i = 0; i < effectsNew.Length; i++)
            {
                addableEffects.Add(effectsNew[i].Clone());
            }
            this.effectsNew = addableEffects;
        }

        this.ZBasePower = ZBasePower;
        this.ZEffectsNew = new List<PBS.Databases.Effects.Moves.MoveEffect>();
        if (ZEffectsNew != null)
        {
            List<PBS.Databases.Effects.Moves.MoveEffect> addableEffects = new List<PBS.Databases.Effects.Moves.MoveEffect>();
            for (int i = 0; i < ZEffectsNew.Length; i++)
            {
                addableEffects.Add(ZEffectsNew[i].Clone());
            }
            this.ZEffectsNew = addableEffects;
        }

        this.MaxPower = MaxPower;
    }

    // Clone
    public MoveData Clone()
    {
        MoveData cloneData = new MoveData(
            ID: ID,
            baseID: baseID,
            moveName: moveName,
            moveType: moveType,
            useSameCategory: useSameCategory, category: category,
            useSameTargetType: useSameTargetType, targetType: targetType,

            useSameBasePower: useSameBasePower, useSameAccuracy: useSameAccuracy, useSamePP: useSamePP,
            basePower: basePower, accuracy: accuracy, PP: PP,
            useSamePriority: useSamePriority,
            priority: priority,

            combineBaseTags: combineBaseTags,
            moveTags: moveTags,

            moveEffects: moveEffects.ToArray(),

            combineBaseEffects: combineBaseEffects,
            effectsNew: effectsNew.ToArray(),

            ZBasePower: ZBasePower,
            ZEffectsNew: ZEffectsNew.ToArray(),

            MaxPower: MaxPower
            );
        return cloneData;
    }
    public MoveData PartialClone(
        MoveCategory category,
        string moveType,
        float basePower, float accuracy, int priority
        )
    {
        MoveData clone = this.Clone();
        this.category = category;
        this.moveType = moveType;
        this.basePower = Mathf.FloorToInt(basePower);
        this.accuracy = accuracy;
        this.priority = priority;
        return clone;
    }

    public bool IsABaseID(string tryBaseID)
    {
        if (baseID == tryBaseID)
        {
            return baseID != null;
        }
        if (baseID != null)
        {
            MoveData baseData = Moves.instance.GetMoveData(baseID);
            return baseData.IsABaseID(tryBaseID);
        }
        return false;
    }

    public bool HasTag(MoveTag moveTag)
    {
        return moveTags.Contains(moveTag);
    }

    public void AddEffects(List<MoveEffect> newEffects)
    {
        moveEffects.AddRange(newEffects);
    }

    public List<MoveEffect> GetEffectsFiltered(MoveEffectTiming timing)
    {
        List<MoveEffect> effects = new List<MoveEffect>();
        for (int i = 0; i < moveEffects.Count; i++)
        {
            if (moveEffects[i].effectTiming == timing
                || moveEffects[i].effectTiming == MoveEffectTiming.Any)
            {
                effects.Add(moveEffects[i]);
            }
        }
        return effects;
    }

    public MoveEffect GetEffect(MoveEffectType effectType)
    {
        for (int i = 0; i < moveEffects.Count; i++)
        {
            if (moveEffects[i].effectType == effectType)
            {
                return moveEffects[i];
            }
        }
        return null;
    }

    public List<MoveEffect> GetEffects(MoveEffectType effectType)
    {
        List<MoveEffect> effects = new List<MoveEffect>();
        for (int i = 0; i < moveEffects.Count; i++)
        {
            if (moveEffects[i].effectType == effectType)
            {
                effects.Add(moveEffects[i]);
            }
        }
        return effects;
    }

    public List<PBS.Databases.Effects.Moves.MoveEffect> GetEffectsNewFiltered(MoveEffectTiming timing)
    {
        List<PBS.Databases.Effects.Moves.MoveEffect> effects = new List<PBS.Databases.Effects.Moves.MoveEffect>();
        for (int i = 0; i < effectsNew.Count; i++)
        {
            if (effectsNew[i].timing == timing
                || effectsNew[i].timing == MoveEffectTiming.Any)
            {
                effects.Add(effectsNew[i]);
            }
        }
        return effects;
    }
    public List<PBS.Databases.Effects.Moves.MoveEffect> GetEffectsNew(MoveEffectType effectType, bool forceUnique = false)
    {
        List<PBS.Databases.Effects.Moves.MoveEffect> effects = new List<PBS.Databases.Effects.Moves.MoveEffect>();
        for (int i = 0; i < effectsNew.Count; i++)
        {
            if (effectsNew[i].effectType == effectType)
            {
                if (!forceUnique || effectsNew[i].timing == MoveEffectTiming.Unique)
                {
                    effects.Add(effectsNew[i]);
                }
            }
        }
        return effects;
    }
    public PBS.Databases.Effects.Moves.MoveEffect GetEffectNew(MoveEffectType effectType, bool forceUnique = false)
    {
        List<PBS.Databases.Effects.Moves.MoveEffect> effectList = GetEffectsNew(effectType, forceUnique);
        if (effectList.Count > 0)
        {
            return effectList[0];
        }
        return null;
    }

    public List<PBS.Databases.Effects.Moves.MoveEffect> GetZEffects()
    {
        return new List<PBS.Databases.Effects.Moves.MoveEffect>(ZEffectsNew);
    }
}

public class MoveEffect : GameEffect
{
    public MoveEffectType effectType { get; set; }
    public MoveEffectTiming effectTiming { get; set; }
    public MoveEffectTargetType effectTargetType { get; set; }
    public HashSet<MoveEffectFilter> effectFilters { get; set; }
    public float effectChance { get; set; }
    public bool forceEffectDisplay { get; set; }
    public bool sheerForceEffect { get; set; }

    // Constructor
    public MoveEffect(
        MoveEffectType effectType,
        MoveEffectTiming effectTiming = MoveEffectTiming.Unique,
        MoveEffectTargetType effectTargetType = MoveEffectTargetType.Unique,
        IEnumerable<MoveEffectFilter> effectFilters = null,
        float effectChance = -1,
        bool forceEffectDisplay = false,
        bool[] boolParams = null,
        float[] floatParams = null,
        string[] stringParams = null,
        bool sheerForceEffect = false
        ) : base(boolParams, floatParams, stringParams)
    {
        this.effectType = effectType;
        this.effectTiming = effectTiming;
        this.effectTargetType = effectTargetType;
        this.effectFilters = new HashSet<MoveEffectFilter>();
        if (effectFilters != null)
        {
            this.effectFilters.UnionWith(effectFilters);
        }
        this.effectChance = effectChance;
        this.forceEffectDisplay = forceEffectDisplay;
        this.sheerForceEffect = sheerForceEffect;
    }

    // Clone
    public static MoveEffect Clone(MoveEffect original)
    {
        MoveEffect cloneEffect = new MoveEffect(
            effectType: original.effectType,
            effectTiming: original.effectTiming,
            effectTargetType: original.effectTargetType,
            effectFilters: original.effectFilters,
            effectChance: original.effectChance,
            forceEffectDisplay: original.forceEffectDisplay,
            boolParams: original.boolParams,
            floatParams: original.floatParams,
            stringParams: original.stringParams,
            sheerForceEffect: original.sheerForceEffect
            );
        return cloneEffect;
    }

    public bool HasFilter(MoveEffectFilter filter)
    {
        return effectFilters.Contains(filter);
    }
}

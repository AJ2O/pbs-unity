using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Every class in this file is legacy code that's meant to be refactored and replaced.


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

public class BattleCEff : GameEffect
{
    public BattleSEType effectType { get; set; }
    public BattleSETiming effectTiming { get; set; }

    // Constructor
    public BattleCEff(
        BattleSEType effectType,
        BattleSETiming effectTiming = BattleSETiming.Unique,
        bool[] boolParams = null,
        float[] floatParams = null,
        string[] stringParams = null
        ) : base(boolParams, floatParams, stringParams)
    {
        this.effectType = effectType;
        this.effectTiming = effectTiming;
    }

    // Clone
    public static BattleCEff Clone(BattleCEff original)
    {
        BattleCEff cloneEffect = new BattleCEff(
            effectType: original.effectType,
            effectTiming: original.effectTiming,
            boolParams: original.boolParams,
            floatParams: original.floatParams,
            stringParams: original.stringParams
            );
        return cloneEffect;
    }

}

public class ItemEffect : GameEffect
{
    public ItemEffectType effectType { get; set; }

    // Constructor
    public ItemEffect(
        ItemEffectType effectType,
        bool[] boolParams = null,
        float[] floatParams = null,
        string[] stringParams = null
        ) : base(boolParams, floatParams, stringParams)
    {
        this.effectType = effectType;
    }

    // Clone
    public static ItemEffect Clone(ItemEffect original)
    {
        ItemEffect cloneEffect = new ItemEffect(
            effectType: original.effectType,
            boolParams: original.boolParams,
            floatParams: original.floatParams,
            stringParams: original.stringParams
            );
        return cloneEffect;
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

public class PokemonCEff : GameEffect
{
    public PokemonSEType effectType { get; set; }
    public PokemonSETiming effectTiming { get; set; }
    public bool forceEffectDisplay { get; set; }

    // Constructor
    public PokemonCEff(
        PokemonSEType effectType,
        PokemonSETiming effectTiming = PokemonSETiming.Unique,
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
    public static PokemonCEff Clone(PokemonCEff original)
    {
        PokemonCEff cloneEffect = new PokemonCEff(
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
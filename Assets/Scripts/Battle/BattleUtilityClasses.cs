using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleSettings
{
    public BattleType battleType;
    public bool isWildBattle;
    public bool isInverse;
    public bool canMegaEvolve, canZMove, canDynamax;

    public BattleSettings(
        BattleType battleType = BattleType.Single,
        bool isWildBattle = false,
        bool isInverse = false,
        bool canMegaEvolve = true,
        bool canZMove = true,
        bool canDynamax = true
        )
    {
        this.battleType = battleType;
        this.isWildBattle = isWildBattle;
        this.isInverse = isInverse;
        this.canMegaEvolve = canMegaEvolve;
        this.canZMove = canZMove;
        this.canDynamax = canDynamax;
    }
}

public class BattlePosition
{
    public int teamPos;
    public int battlePos;

    // Constructor
    public BattlePosition(int teamPos, int battlePos) {
        this.teamPos = teamPos;
        this.battlePos = battlePos;
    }
    public BattlePosition(Pokemon pokemon)
    {
        teamPos = pokemon.teamPos;
        battlePos = pokemon.battlePos;
    }

    // Clone
    public static BattlePosition Clone(BattlePosition original)
    {
        if (original == null)
        {
            return null;
        }
        BattlePosition clonePosition = new BattlePosition(
            teamPos: original.teamPos,
            battlePos: original.battlePos
            );
        return clonePosition;
    }

    public bool IsTheSameAs(BattlePosition other)
    {
        return teamPos == other.teamPos
            && battlePos == other.battlePos;
    }

    public override string ToString()
    {
        return "(" + teamPos + ", " + battlePos + ")";
    }

}

public class BattleCommand
{
    // General
    public BattleCommandType commandType;
    public Pokemon commandUser;
    public Trainer commandTrainer;
    public bool inProgress = false;
    public bool completed = false;
    public int commandPriority = 0;
    public int iteration = 1;
    public bool isExplicitlySelected = false;

    // Move
    public string moveID;
    public bool consumePP = true;
    public BattlePosition[] targetPositions;
    public bool displayMove = true;
    public bool forceOneHit = false;
    public bool bypassRedirection = false;
    public bool bypassStatusInterrupt = false;
    public bool isDanceMove = false;
    public bool isMoveCalled = false;
    public bool isMoveReflected = false;
    public bool isMoveHijacked = false;
    public bool isFutureSightMove = false;
    public bool isPursuitMove = false;
    public bool isMoveSnatched = false;
    public bool isMagicCoatMove = false;

    public bool isMegaEvolving = false;
    public bool isZMove = false;
    public bool isDynamaxing = false;

    // Switch
    public int switchPosition;
    public Trainer switchingTrainer;
    public Pokemon switchInPokemon;

    // Item
    public string itemID;
    public Trainer itemTrainer;

    private BattleCommand() { }
    private BattleCommand(bool isExplicitlySelected = false)
    {
        commandType = BattleCommandType.None;
        this.isExplicitlySelected = isExplicitlySelected;
    }

    public static BattleCommand CreateMoveCommand(
        Pokemon commandUser,
        string moveID,
        List<BattlePosition> targetPositions,
        bool isExplicitlySelected = false,
        bool isMegaEvolving = false,
        bool isZMove = false,
        bool isDynamaxing = false)
    {
        BattleCommand command = new BattleCommand(isExplicitlySelected);
        command.commandType = BattleCommandType.Fight;
        command.commandUser = commandUser;
        command.moveID = moveID;

        command.targetPositions = targetPositions.ToArray();
        command.isMegaEvolving = isMegaEvolving;
        command.isZMove = isZMove;
        command.isDynamaxing = isDynamaxing;

        return command;
    }

    public static BattleCommand CreateSwitchCommand(
        Pokemon commandUser,
        int switchPosition,
        Trainer trainer,
        Pokemon switchInPokemon,
        bool isExplicitlySelected = false)
    {
        BattleCommand command = new BattleCommand(isExplicitlySelected);
        command.commandType = BattleCommandType.Party;
        command.switchPosition = switchPosition;
        command.switchingTrainer = trainer;
        command.commandUser = commandUser;
        command.switchInPokemon = switchInPokemon;
        return command;
    }

    public static BattleCommand CreateReplaceCommand(
        int switchPosition,
        Trainer trainer,
        Pokemon switchInPokemon,
        bool isExplicitlySelected = false)
    {
        BattleCommand command = new BattleCommand(isExplicitlySelected);
        command.commandType = BattleCommandType.PartyReplace;
        command.switchPosition = switchPosition;
        command.switchingTrainer = trainer;
        command.commandUser = switchInPokemon;
        command.switchInPokemon = switchInPokemon;
        return command;
    }

    public static BattleCommand CreateRechargeCommand(
        Pokemon commandUser
        )
    {
        BattleCommand command = new BattleCommand();
        command.commandType = BattleCommandType.Recharge;
        command.commandUser = commandUser;
        return command;
    }

    public static BattleCommand CreateBagCommand(
        string itemID,
        Pokemon itemPokemon,
        Trainer trainer,
        bool isExplicitlySelected = false)
    {
        BattleCommand command = new BattleCommand(isExplicitlySelected);
        command.commandType = BattleCommandType.Bag;
        command.itemID = itemID;
        command.commandUser = itemPokemon;
        command.itemTrainer = trainer;
        return command;
    }

    public static BattleCommand CreateRunCommand(
        Pokemon commandUser,
        bool isExplicitlySelected = false
        )
    {
        BattleCommand command = new BattleCommand(isExplicitlySelected);
        command.commandType = BattleCommandType.Run;
        command.commandUser = commandUser;
        return command;
    }

    public static BattleCommand CreateFromPlayerCommand(PBS.Player.Command playerCommand)
    {
        BattleCommand command = new BattleCommand();
        command.commandType = playerCommand.commandType;
        command.inProgress = playerCommand.inProgress;
        command.completed = playerCommand.completed;
        command.commandPriority = playerCommand.commandPriority;
        command.iteration = playerCommand.iteration;
        command.isExplicitlySelected = playerCommand.isExplicitlySelected;

        command.moveID = playerCommand.moveID;
        command.consumePP = playerCommand.consumePP;
        command.targetPositions = playerCommand.targetPositions;
        command.displayMove = playerCommand.displayMove;
        command.forceOneHit = playerCommand.forceOneHit;
        command.bypassRedirection = playerCommand.bypassRedirection;
        command.bypassStatusInterrupt = playerCommand.bypassStatusInterrupt;
        command.isDanceMove = playerCommand.isDanceMove;
        command.isMoveCalled = playerCommand.isMoveCalled;
        command.isMoveReflected = playerCommand.isMoveReflected;
        command.isMoveHijacked = playerCommand.isMoveHijacked;
        command.isFutureSightMove = playerCommand.isFutureSightMove;
        command.isPursuitMove = playerCommand.isPursuitMove;
        command.isMoveSnatched = playerCommand.isMoveSnatched;
        command.isMagicCoatMove = playerCommand.isMagicCoatMove;

        command.isMegaEvolving = playerCommand.isMegaEvolving;
        command.isZMove = playerCommand.isZMove;
        command.isDynamaxing = playerCommand.isDynamaxing;

        command.switchPosition = playerCommand.switchPosition;

        command.itemID = playerCommand.itemID;
        return command;
    }
}

public class BattleFutureSightCommand
{
    public string pokemonID;
    public string moveID;
    public List<BattlePosition> targetPositions;
    public int turnsLeft;

    // Constructor
    public BattleFutureSightCommand(
        string pokemonID,
        string moveID,
        BattlePosition[] targetPositions,
        int turnsLeft = 0
        )
    {
        this.pokemonID = pokemonID;
        this.moveID = moveID;
        this.targetPositions = new List<BattlePosition>();
        for (int i = 0; i < targetPositions.Length; i++)
        {
            this.targetPositions.Add(BattlePosition.Clone(targetPositions[i]));
        }
        this.turnsLeft = turnsLeft;
    }

    public bool IsTargetingPosition(BattlePosition position)
    {
        for (int i = 0; i < targetPositions.Count; i++)
        {
            if (targetPositions[i].IsTheSameAs(position))
            {
                return true;
            }
        }
        return false;
    }

    // Clone
    public static BattleFutureSightCommand Clone(BattleFutureSightCommand original)
    {
        BattleFutureSightCommand clone = new BattleFutureSightCommand(
            pokemonID: original.pokemonID,
            moveID: original.moveID,
            targetPositions: original.targetPositions.ToArray(),
            turnsLeft: original.turnsLeft);
        return clone;
    }

}

public class BattleWishCommand
{
    public string pokemonID;
    public string moveID;
    public List<BattlePosition> wishPositions;
    public int turnsLeft;
    public int hpRecovered;

    // Constructor
    public BattleWishCommand(
        string pokemonID,
        string moveID,
        BattlePosition[] wishPositions,
        int turnsLeft = 0,
        int hpRecovered = 0
        )
    {
        this.pokemonID = pokemonID;
        this.moveID = moveID;
        this.wishPositions = new List<BattlePosition>();
        for (int i = 0; i < wishPositions.Length; i++)
        {
            this.wishPositions.Add(BattlePosition.Clone(wishPositions[i]));
        }
        this.turnsLeft = turnsLeft;
        this.hpRecovered = hpRecovered;
    }

    public bool IsTargetingPosition(BattlePosition position)
    {
        for (int i = 0; i < wishPositions.Count; i++)
        {
            if (wishPositions[i].IsTheSameAs(position))
            {
                return true;
            }
        }
        return false;
    }

    // Clone
    public static BattleWishCommand Clone(BattleWishCommand original)
    {
        BattleWishCommand clone = new BattleWishCommand(
            pokemonID: original.pokemonID,
            moveID: original.moveID,
            wishPositions: original.wishPositions.ToArray(),
            turnsLeft: original.turnsLeft,
            hpRecovered: original.hpRecovered);
        return clone;
    }
}

public class BattleEnvironment
{
    // General
    public BattleEnvironmentType environmentType;
    public string naturePowerMove;

    // Constructor
    public BattleEnvironment(
        BattleEnvironmentType environmentType = BattleEnvironmentType.Building,
        string naturePowerMove = null
        )
    {
        this.environmentType = environmentType;
        this.naturePowerMove = naturePowerMove;
    }

    // Clone
    public static BattleEnvironment Clone(BattleEnvironment original)
    {
        BattleEnvironment clone = new BattleEnvironment(
            environmentType: original.environmentType,
            naturePowerMove: original.naturePowerMove
            );
        return clone;
    }
}

public class BattleHitTarget
{
    public Pokemon pokemon;
    public float preHPPercent;
    public float postHPPercent;
    public int preHP;
    public int postHP;
    public int subDamage;
    public int damageDealt;
    public Pokemon.Ability disguise;

    public BattleTypeEffectiveness effectiveness;

    public bool affectedByMove;
    public bool missed;
    public bool criticalHit;
    public bool fainted;
    public bool isMoveReflected;

    public string destinyBondMove;

    public EffectDatabase.General.Protect protection;
    public EffectDatabase.General.Protect teamProtection;
    public string protectAbility;
    public string protectItem;
    public EffectDatabase.General.MagicCoat reflection;

    public string reflectMove;
    public string reflectAbility;
    public string reflectItem;

    public EffectDatabase.MoveEff.Endure endure;
    public Pokemon.AbilityEffectPair sturdyPair;
    public Item focusBand;
    public string surviveAbility;
    public string surviveItem;

    public BattleHitTarget(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        preHPPercent = pokemon.HPPercent;
        postHPPercent = pokemon.HPPercent;
        preHP = 0;
        postHP = 0;
        subDamage = -1;
        damageDealt = -1;
        disguise = null;

        effectiveness = new BattleTypeEffectiveness();

        affectedByMove = true;
        missed = false;
        criticalHit = false;
        fainted = false;
        isMoveReflected = false;

        destinyBondMove = null;

        protection = null;
        teamProtection = null;
        protectAbility = null;
        protectItem = null;
        reflection = null;

        reflectMove = null;
        reflectAbility = null;
        reflectItem = null;

        endure = null;
        sturdyPair = null;
        focusBand = null;
        surviveAbility = null;
        surviveItem = null;
    }

    public void CloneFrom(BattleHitTarget other)
    {
        preHPPercent = other.preHPPercent;
        postHPPercent = other.postHPPercent;
        preHP = other.preHP;
        postHP = other.postHP;
        subDamage = other.subDamage;
        damageDealt = other.damageDealt;
        disguise = (other.disguise == null) ? null : other.disguise.Clone();

        effectiveness = other.effectiveness.Clone();

        affectedByMove = other.affectedByMove;
        missed = other.missed;
        criticalHit = other.criticalHit;
        fainted = other.fainted;
        isMoveReflected = other.isMoveReflected;

        destinyBondMove = other.destinyBondMove;

        protection = (other.protection == null) ? null : other.protection.Clone();
        teamProtection = (other.teamProtection == null) ? null : other.teamProtection.Clone();
        protectAbility = other.protectAbility;
        protectItem = other.protectItem;
        reflection = (other.reflection == null) ? null : other.reflection.Clone();

        reflectMove = other.reflectMove;
        reflectAbility = other.reflectAbility;
        reflectItem = other.reflectItem;

        endure = (other.endure == null) ? null : other.endure.Clone();
        sturdyPair = (other.sturdyPair == null) ? null : other.sturdyPair.Clone();
        focusBand = (other.focusBand == null) ? null : other.focusBand.Clone();
        surviveAbility = other.surviveAbility;
        surviveItem = other.surviveItem;
    }
}

public class BattleHitTeam
{
    public BattleTeam team;
    public bool affectedByMove;
    public EffectDatabase.General.Protect protection;
    public EffectDatabase.General.MagicCoat reflection;

    public BattleHitTeam(BattleTeam team)
    {
        this.team = team;
        this.affectedByMove = true;
        this.protection = null;
        this.reflection = null;
    }
}

public class BattleHit
{
    public List<BattleHitTarget> hitTargets;
    public BattleHit()
    {
        hitTargets = new List<BattleHitTarget>();
    }
}

public class BattleTypeEffectiveness
{
    public float rawEffectiveness;
    public float baseEffectiveness;
    public Dictionary<string, float> typeChart;

    public BattleTypeEffectiveness()
    {
        rawEffectiveness = -1;
        baseEffectiveness = 1f;
        typeChart = new Dictionary<string, float>();
    }
    public BattleTypeEffectiveness Clone()
    {
        BattleTypeEffectiveness clone = new BattleTypeEffectiveness();
        clone.rawEffectiveness = rawEffectiveness;
        clone.baseEffectiveness = baseEffectiveness;

        List<string> keys = new List<string>(typeChart.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            clone.SetType(keys[i], typeChart[keys[i]]);
        }
        return clone;
    }
    
    public void SetType(string type, float effectiveness)
    {
        typeChart[type] = effectiveness;
    }
    public void FactorType(string type, float effectiveness)
    {
        if (typeChart.ContainsKey(type))
        {
            typeChart[type] *= effectiveness;
        }
        else
        {
            SetType(type, effectiveness);
        }
    }
    public void FactorOther(BattleTypeEffectiveness other)
    {
        baseEffectiveness *= other.baseEffectiveness;
        List<string> keys = new List<string>(other.typeChart.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            FactorType(keys[i], other.typeChart[keys[i]]);
        }
    }
    
    public float GetTypeEffectiveness(string type)
    {
        if (typeChart.ContainsKey(type))
        {
            return typeChart[type];
        }
        return 1f;
    }
    public bool IsTypeNeutral(string type)
    {
        return GetTypeEffectiveness(type) == 1f;
    }
    public bool IsTypeSuperEffective(string type)
    {
        return GetTypeEffectiveness(type) > 1;
    }
    public bool IsTypeNotVeryEffective(string type)
    {
        float typeEffectiveness = GetTypeEffectiveness(type);
        return typeEffectiveness > 0 && typeEffectiveness < 1;
    }
    public bool IsTypeNoEffect(string type)
    {
        return GetTypeEffectiveness(type) == 0;
    }
    
    public float GetTotalEffectiveness()
    {
        if (rawEffectiveness >= 0)
        {
            return rawEffectiveness;
        }

        float effectiveness = baseEffectiveness;
        List<float> effectivenessList = new List<float>(typeChart.Values);
        for (int i = 0; i < effectivenessList.Count; i++)
        {
            effectiveness *= effectivenessList[i];
        }
        return effectiveness;
    }
    public bool IsNeutral()
    {
        return GetTotalEffectiveness() == 1f;
    }
    public bool IsSuperEffective()
    {
        return GetTotalEffectiveness() > 1f;
    }
    public bool IsNotVeryEffective()
    {
        float value = GetTotalEffectiveness();
        return value > 0 && value < 1f;
    }
    public bool IsImmune()
    {
        return GetTotalEffectiveness() == 0;
    }

    public List<string> GetResistedTypes()
    {
        List<string> types = new List<string>();
        List<string> keys = new List<string>(typeChart.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            string curType = keys[i];

            if (typeChart[curType] > 0 && typeChart[curType] < 1)
            {
                types.Add(curType);
            }
        }
        return types;
    }
    public List<string> GetWeakTypes()
    {
        List<string> types = new List<string>();
        List<string> keys = new List<string>(typeChart.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            string curType = keys[i];

            if (typeChart[curType] > 1)
            {
                types.Add(curType);
            }
        }
        return types;
    }
    public List<string> GetImmuneTypes()
    {
        List<string> types = new List<string>();
        List<string> keys = new List<string>(typeChart.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            string curType = keys[i];

            if (typeChart[curType] == 0)
            {
                types.Add(curType);
            }
        }
        return types;
    }
}

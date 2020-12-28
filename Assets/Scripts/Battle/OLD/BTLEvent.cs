using PBS.Main.Pokemon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLEvent
{
    public Battle battleModel = null;
    public List<BTLEvent> trailEvents = new List<BTLEvent>();

    public BTLEvent()
    {
    }
    public BTLEvent(Battle battleModel)
    {
        SetBattleModel(Battle.CloneModel(battleModel));
    }

    /// <summary>
    /// Sets the event's battle model to the given one.
    /// </summary>
    /// <param name="battleModel"></param>
    public void SetBattleModel(Battle battleModel)
    {
        this.battleModel = battleModel;
    }
    public void SetCloneModel(Battle battleModel)
    {
        this.battleModel = Battle.CloneModel(battleModel);
    }

    public void AddTrailEvent(BTLEvent bEvent)
    {
        trailEvents.Add(bEvent);
    }
}

public class BTLEvent_Load : BTLEvent
{
    public PokemonData pokemonData = null;
    public Pokemon pokemon = null;

    public ItemData itemData = null;
    public Item item = null;
}

public class BTLEvent_Update : BTLEvent
{
    public List<Pokemon> updatePokemon;

    public void Create(
        List<Pokemon> updatePokemon = null
        )
    {
        this.updatePokemon = new List<Pokemon>();
        if (updatePokemon != null)
        {
            for (int i = 0; i < updatePokemon.Count; i++)
            {
                this.updatePokemon.Add(battleModel.GetBattleInstanceOfPokemon(updatePokemon[i]));
            }
        }
    }
}

public class BTLEvent_Message : BTLEvent
{
    public string message;

    public BTLEvent_Message(string message)
    {
        this.message = message;
    }
}

public class BTLEvent_GameText : BTLEvent
{
    public string textID = null;
    public Pokemon pokemon = null;
    public Pokemon userPokemon = null;
    public Pokemon targetPokemon = null;
    public Pokemon[] pokemonList = null;
    public Trainer trainer = null;
    public BattleTeam targetTeam = null;
    public PokemonStats[] statList = null;
    public string typeID = null;
    public string moveID = null;
    public string abilityID = null;
    public string itemID = null;
    public string statusID = null;
    public string teamStatusID = null;
    public string battleStatusID = null;
    public string[] moveIDs = null;
    public string[] typeIDs = null;

    public List<int> intArgs = null;

    public void Create(
        string textID = null,
        Pokemon pokemon = null,
        Pokemon userPokemon = null,
        Pokemon targetPokemon = null,
        Trainer trainer = null,
        Pokemon[] pokemonList = null,
        BattleTeam targetTeam = null,
        PokemonStats[] statList = null,
        string typeID = null,
        string moveID = null,
        string abilityID = null,
        string itemID = null,
        string statusID = null,
        string teamStatusID = null,
        string battleStatusID = null,
        string[] moveIDs = null,
        string[] typeIDs = null,

        List<int> intArgs = null
        )
    {
        this.textID = textID ?? this.textID;
        this.pokemon = 
            (pokemon == null) ? this.pokemon : battleModel.GetBattleInstanceOfPokemon(pokemon);
        this.userPokemon = 
            (userPokemon == null) ? this.userPokemon : battleModel.GetBattleInstanceOfPokemon(userPokemon);
        this.targetPokemon = 
            (targetPokemon == null) ? this.targetPokemon : battleModel.GetBattleInstanceOfPokemon(targetPokemon);
        this.pokemonList = null;
        if (pokemonList != null)
        {
            this.pokemonList = new Pokemon[pokemonList.Length];
            for (int i = 0; i < pokemonList.Length; i++)
            {
                this.pokemonList[i] = battleModel.GetBattleInstanceOfPokemon(pokemonList[i]);
            }
        }

        this.trainer = (trainer == null) ? null : battleModel.GetBattleInstanceOfTrainer(trainer);
        this.targetTeam = (targetTeam == null) ? null : battleModel.GetBattleInstanceOfTeam(targetTeam);
        this.statList = statList;

        this.typeID = typeID ?? this.typeID;
        this.moveID = moveID ?? this.moveID;
        this.abilityID = abilityID ?? this.abilityID;
        this.itemID = itemID ?? this.itemID;
        this.statusID = statusID ?? this.statusID;
        this.teamStatusID = teamStatusID ?? this.teamStatusID;
        this.battleStatusID = battleStatusID ?? this.battleStatusID;

        this.moveIDs = (moveIDs == null) ? null : new string[moveIDs.Length];
        if (moveIDs != null)
        {
            for (int i = 0; i < moveIDs.Length; i++)
            {
                this.moveIDs[i] = moveIDs[i];
            }
        }

        this.typeIDs = (typeIDs == null) ? null : new string[typeIDs.Length];
        if (typeIDs != null)
        {
            for (int i = 0; i < typeIDs.Length; i++)
            {
                this.typeIDs[i] = typeIDs[i];
            }
        }

        this.intArgs = (intArgs == null) ? null : new List<int>(intArgs);
    }
}

public class BTLEvent_Simul : BTLEvent
{
    public List<BTLEvent> simulEvents;

    public void Create(IEnumerable<BTLEvent> events)
    {
        simulEvents = new List<BTLEvent>(events);
    }
}

public class BTLEvent_StartBattle : BTLEvent
{
    public BTLEvent_StartBattle(Battle battleModel) : base(battleModel)
    {

    }
}
public class BTLEvent_EndBattle : BTLEvent
{

}

public class BTLEvent_SendOut : BTLEvent
{
    public Trainer trainer;
    public Pokemon[] sendPokemon;

    public BTLEvent_SendOut()
    {
    }

    public void Create(Trainer trainer, List<Pokemon> sendPokemon)
    {
        this.trainer = battleModel.GetBattleInstanceOfTrainer(trainer);
        this.sendPokemon = new Pokemon[sendPokemon.Count];
        for (int i = 0; i < sendPokemon.Count; i++)
        {
            this.sendPokemon[i] = battleModel.GetBattleInstanceOfPokemon(sendPokemon[i]);
        }
    }
}

public class BTLEvent_ForceOut : BTLEvent
{
    public BTLEvent_SendOut[] sendOutEvents;

    public BTLEvent_ForceOut()
    {
    }

    public void Create(BTLEvent_SendOut[] sendOutEvents)
    {
        this.sendOutEvents = sendOutEvents;
    }
}

public class BTLEvent_Withdraw : BTLEvent
{
    public Trainer trainer;
    public Pokemon[] withdrawPokemon;

    public BTLEvent_Withdraw()
    {
    }

    public void Create(Trainer trainer, Pokemon pokemon)
    {
        Create(trainer, new List<Pokemon> { pokemon });
    }
    public void Create(Trainer trainer, List<Pokemon> withdrawPokemon)
    {
        this.trainer = battleModel.GetBattleInstanceOfTrainer(trainer);
        this.withdrawPokemon = new Pokemon[withdrawPokemon.Count];
        for (int i = 0; i < withdrawPokemon.Count; i++)
        {
            this.withdrawPokemon[i] = battleModel.GetBattleInstanceOfPokemon(withdrawPokemon[i]);
        }
    }
}

public class BTLEvent_PromptCommands : BTLEvent
{
    public Trainer trainer;
    public Pokemon[] pokemonToCommand;

    public void Create(Trainer trainer, List<Pokemon> pokemonToCommand)
    {
        this.trainer = battleModel.GetBattleInstanceOfTrainer(trainer);

        this.pokemonToCommand = new Pokemon[pokemonToCommand.Count];
        for (int i = 0; i < pokemonToCommand.Count; i++)
        {
            this.pokemonToCommand[i] = battleModel.GetBattleInstanceOfPokemon(pokemonToCommand[i]);
        }
    }
}

public class BTLEvent_PromptReplace : BTLEvent
{
    public Trainer trainer;
    public int[] fillPositions;

    public void Create(Trainer trainer, List<int> fillPositions)
    {
        this.trainer = battleModel.GetBattleInstanceOfTrainer(trainer);

        this.fillPositions = new int[fillPositions.Count];
        for (int i = 0; i < fillPositions.Count; i++)
        {
            this.fillPositions[i] = fillPositions[i];
        }
    }
}

public class BTLEvent_Move : BTLEvent
{
    public Pokemon user;
    public string moveID;

    public void Create(
        Pokemon user,
        string moveID)
    {
        this.user = battleModel.GetBattleInstanceOfPokemon(user);
        this.moveID = moveID;
    }

}

public class BTLEvent_MoveHit : BTLEvent
{
    public Pokemon user;
    public string moveID;
    public int moveHit;
    public List<BattleHitTarget> battleHitTargets;

    public void SetUser(Pokemon user)
    {
        this.user = battleModel.GetBattleInstanceOfPokemon(user);
    }
    public void SetHitTargets(List<BattleHitTarget> battleHitTargets)
    {
        this.battleHitTargets = new List<BattleHitTarget>();
        for (int i = 0; i < battleHitTargets.Count; i++)
        {
            this.battleHitTargets.Add(
                new BattleHitTarget(battleModel.GetBattleInstanceOfPokemon(battleHitTargets[i].pokemon))
                );
            this.battleHitTargets[i].CloneFrom(battleHitTargets[i]);
        }
    }
}

public class BTLEvent_MoveHitTarget : BTLEvent
{
    public Pokemon targetPokemon;
    public bool affectedByMove = false;
    public BTLEvent_Damage dmgEvent;

    public void SetTarget(Pokemon pokemon)
    {
        targetPokemon = pokemon;
        if (dmgEvent != null)
        {
            dmgEvent.SetTarget(pokemon);
        }
    }

    public void Create(Pokemon pokemon)
    {
        SetTarget(battleModel.GetBattleInstanceOfPokemon(pokemon));
    }
}

public class BTLEvent_Damage : BTLEvent
{
    public Pokemon targetPokemon;
    public int damageDealt;
    public int preHP;
    public int postHP;
    public float effectiveness = 1f;
    public bool criticalHit = false;
    public int hitDisplay = 0;

    public void SetTarget(Pokemon pokemon)
    {
        targetPokemon = pokemon;
    }
}

public class BTLEvent_MultiDamage : BTLEvent
{
    public BTLEvent_Damage[] targets;

    public void SetDmgEvents(List<BTLEvent_Damage> hitTargets)
    {
        this.targets = new BTLEvent_Damage[hitTargets.Count];
        for (int i = 0; i < hitTargets.Count; i++)
        {
            this.targets[i] = hitTargets[i];
        }
    }
}

public class BTLEvent_Heal : BTLEvent
{
    public Pokemon targetPokemon;
    public int hpHealed;
    public int preHP;
    public int postHP;

    public void SetTarget(Pokemon pokemon)
    {
        targetPokemon = pokemon;
    }
}

public class BTLEvent_MultiHeal : BTLEvent
{
    public BTLEvent_Heal[] targets;

    public void SetDmgEvents(List<BTLEvent_Heal> hitTargets)
    {
        this.targets = new BTLEvent_Heal[hitTargets.Count];
        for (int i = 0; i < hitTargets.Count; i++)
        {
            this.targets[i] = hitTargets[i];
        }
    }
}

public class BTLEvent_StatusCondition : BTLEvent
{
    public Pokemon targetPokemon;
    public string statusID;

    public void Create(Pokemon targetPokemon, string statusID)
    {
        this.targetPokemon = battleModel.GetBattleInstanceOfPokemon(targetPokemon);
        this.statusID = statusID;
    }
}

public class BTLEvent_StatStageMod : BTLEvent
{
    public Pokemon targetPokemon;
    public PokemonStats[] statsToMod;
    public int modValue;
    public bool runAnim = false;
    public bool maximize = false;
    public bool minimize = false;
    public BTLEvent_GameText gameText;

    public void Create(
        Pokemon targetPokemon,
        List<PokemonStats> statsToMod,
        int modValue
        )
    {
        this.targetPokemon = battleModel.GetBattleInstanceOfPokemon(targetPokemon);
        this.modValue = modValue;
        this.statsToMod = new PokemonStats[statsToMod.Count];
        for (int i = 0; i < statsToMod.Count; i++)
        {
            this.statsToMod[i] = statsToMod[i];
        }
    }
}

public class BTLEvent_Ability : BTLEvent
{
    public Pokemon pokemon;
    public string abilityID;

    public void SetPokemon(Pokemon pokemon)
    {
        this.pokemon = battleModel.GetBattleInstanceOfPokemon(pokemon);
    }
}

public class BTLEvent_SwitchPosition : BTLEvent
{
    public Pokemon pokemon1;
    public Pokemon pokemon2;

    public void Create(Pokemon pokemon1, Pokemon pokemon2)
    {
        this.pokemon1 = battleModel.GetBattleInstanceOfPokemon(pokemon1);
        this.pokemon2 = battleModel.GetBattleInstanceOfPokemon(pokemon2);
    }
}

public class BTLEvent_ChangePokemon : BTLEvent
{
    public Pokemon pokemon;
    public string prePokemon;
    public string postPokemon;

    public void Create(Pokemon pokemon, string prePokemon, string postPokemon)
    {
        this.pokemon = battleModel.GetBattleInstanceOfPokemon(pokemon);
        this.prePokemon = prePokemon;
        this.postPokemon = postPokemon;
    }
}

public class BTLEvent_Faint : BTLEvent
{
    public Pokemon[] faintedPokemon;

    public void Create(Pokemon faintedPokemon)
    {
        Create(new List<Pokemon> { faintedPokemon });
    }
    public void Create(List<Pokemon> faintedPokemon)
    {
        this.faintedPokemon = new Pokemon[faintedPokemon.Count];
        for (int i = 0; i < faintedPokemon.Count; i++)
        {
            this.faintedPokemon[i] = battleModel.GetBattleInstanceOfPokemon(faintedPokemon[i]);
        }
    }
}




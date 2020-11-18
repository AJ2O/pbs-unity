using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLView : MonoBehaviour
{
    public Battle battleModel;

    [Header("Components")]
    public BTLSCN battleScene;
    public BTLUI_New battleUI;
    public BattleAssetLoader assetLoader;

    [Header("Player View")]
    public int playerID = 0;
    public int teamPos = -1;
    public bool isSpectator = true;

    public enum ViewPerspective
    {
        Player,
        Ally,
        Enemy
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Model
    public void UpdateModel(Battle model)
    {
        // Update model
        battleModel = model;
        battleScene.UpdateModel(model);
        battleUI.UpdateModel(model);
    }

    public void SetView()
    {
        SetView(playerID);
    }
    public void SetView(int playerID)
    {
        for (int i = 0; i < battleModel.teams.Count; i++)
        {
            for (int j = 0; j < battleModel.teams[i].trainers.Count; j++)
            {
                Trainer trainer = battleModel.teams[i].trainers[j];
                if (trainer.playerID == playerID)
                {
                    teamPos = trainer.teamID;
                    isSpectator = false;
                    return;
                }
            }
        }

        // if you're a spectator, view a random team
        isSpectator = true;
        teamPos = battleModel.teams[Random.Range(0, battleModel.teams.Count)].teamID;
    }
    public void SetPlayerID(int playerID)
    {
        this.playerID = playerID;
    }

    public void UpdateViaEvent(
        List<Pokemon> pokemon
        )
    {
        for (int i = 0; i < pokemon.Count; i++)
        {
            battleUI.UpdatePokemonHUD(pokemon[i], battleModel);
        }
    }

    public void DrawModel()
    {
        // weather
        // terrain

        /*for (int i = 0; i < battleModel.teams.Count; i++)
        {
            BattleTeam team = battleModel.teams[i];
            Debug.Log("===== Team " + team.teamPos + " =====");
            for (int k = 0; k < team.trainers.Count; k++)
            {
                Trainer trainer = team.trainers[k];
                Debug.Log("=== Trainer " + trainer.name + " ===");
            }
        }*/

        string text = "";
        for (int i = 0; i < battleModel.pokemonOnField.Count; i++)
        {
            Pokemon pokemon = battleModel.pokemonOnField[i];
            double hpPercent = System.Math.Round(battleModel.GetPokemonHPAsPercentage(pokemon), 2);

            string typeString = " (Types - ";
            List<string> pokemonTypes = battleModel.PBPGetTypes(pokemon);
            typeString += GameTextDatabase.ConvertTypesToString(pokemonTypes.ToArray());
            typeString += ")";

            string statusString = " (";
            statusString += (pokemon.nonVolatileStatus.statusID == "healthy") ? ""
                : pokemon.nonVolatileStatus.data.conditionName;
            for (int k = 0; k < pokemon.bProps.statusConditions.Count; k++)
            {
                statusString += (string.IsNullOrEmpty(statusString)) ? "" : "/";
                statusString += (pokemon.bProps.statusConditions[k].statusID == "healthy") ? ""
                    : pokemon.bProps.statusConditions[k].data.conditionName;
                if (k != pokemon.bProps.statusConditions.Count - 1)
                {
                    statusString += "/";
                }
            }
            statusString += ")";

            string statModString = " (Stat Stages - "
                + pokemon.bProps.ATKStage + "/"
                + pokemon.bProps.DEFStage + "/"
                + pokemon.bProps.SPAStage + "/"
                + pokemon.bProps.SPDStage + "/"
                + pokemon.bProps.SPEStage + "/"
                + pokemon.bProps.ACCStage + "/"
                + pokemon.bProps.EVAStage + ")";

            string statString = " (Stats - "
                + battleModel.GetPokemonATK(pokemon, false, false) + "/"
                + battleModel.GetPokemonDEF(pokemon, false, false) + "/"
                + battleModel.GetPokemonSPA(pokemon, false, false) + "/"
                + battleModel.GetPokemonSPD(pokemon, false, false) + "/"
                + battleModel.GetPokemonSPE(pokemon, false, false) + ")";

            string abilityString = " (Ability: ";
            AbilityData abilityData = battleModel.PBPGetAbilityData(pokemon);
            if (abilityData == null)
            {
                abilityString += "---";
            }
            else
            {
                abilityString += abilityData.abilityName;
            }
            abilityString += ")";

            string subString = (pokemon.bProps.substituteHP > 0) ? " (Substitute)" : "";

            string itemString = (pokemon.item == null) ? "" : " (Item: " + pokemon.item.data.itemName + ")";

            text += GetPokemonName(pokemon) 
                + subString
                + " - Lv." + pokemon.level 
                //+ " - " + Debug_GetPokemonHealth(pokemon)
                + " - " + (hpPercent * 100) + "%"
                + typeString
                + statusString
                + statModString
                + statString
                + abilityString
                + itemString
                + ((i == battleModel.pokemonOnField.Count - 1)? "" : "\n");
        }
        Debug.Log(text);
    }
    public void RedrawModel()
    {
        DrawModel();
    }
    public string Debug_GetPokemonHealth(Pokemon pokemon)
    {
        string hpText = "==========";
        float hpPercent = battleModel.GetPokemonHPAsPercentage(pokemon);
        int bars = Mathf.Clamp(Mathf.RoundToInt(hpPercent * 10), 0, 10);
        hpText = hpText.Substring(0, bars);
        for (int i = bars; i < 10; i++)
        {
            hpText += "-";
        }
        return "|" + hpText + "|";
    }

    // Sequences
    public IEnumerator StartBattle()
    {
        // trainer intro sequence
        string startText = "";

        Trainer playerTrainer = null;
        List<Trainer> enemyTrainers = new List<Trainer>();
        List<Trainer> allyTrainers = new List<Trainer>();

        List<Trainer> allTrainers = battleModel.GetTrainers();
        for (int i = 0; i < allTrainers.Count; i++)
        {
            if (allTrainers[i].teamID != teamPos)
            {
                enemyTrainers.Add(allTrainers[i]);
            }
            else if (allTrainers[i].playerID != playerID)
            {
                allyTrainers.Add(allTrainers[i]);
            }
            else
            {
                playerTrainer = allTrainers[i];
            }
        }
        string enemyText = GetTrainerNames(enemyTrainers);
        string allyText = GetTrainerNames(allyTrainers);

        // if wild battle, say you were challenged by pokemon
        if (battleModel.battleSettings.isWildBattle)
        {
            List<Pokemon> wildPokemon = new List<Pokemon>();
            for (int i = 0; i < enemyTrainers.Count; i++)
            {
                wildPokemon.AddRange(enemyTrainers[i].party);
            }

            startText = "A wild " + GetPokemonNames(wildPokemon) + " appeared!";
        }
        else
        {
            // if we're spectating, say one team challenged the other
            if (isSpectator)
            {
                startText = "A Pokémon battle has started between " + allyText + " and " + enemyText + "!";
            }
            else
            {
                startText += "You " + ((!string.IsNullOrEmpty(allyText)) ? "and " + allyText + "" : "")
                    + "are challenged by " + enemyText + " to a Pokémon battle!";
            }
        }
        
        yield return StartCoroutine(battleUI.DrawText(startText));
    }

    public IEnumerator EndBattle()
    {
        BattleTeam winningTeam = battleModel.GetWinningTeam();
        List<BattleTeam> losingTeams = battleModel.GetLosingTeams();

        battleUI.SetPokemonHUDsActive(false);

        // No exit message
        if (battleModel.battleSettings.isWildBattle
            || battleModel.runningPokemon != null)
        {

        }
        // Trainer Battle
        else
        {
            // Draw
            if (winningTeam == null)
            {
                yield return StartCoroutine(battleUI.DrawText("The battle ended in a draw!"));
            }
            else
            {
                // gather all losing trainers...
                List<Trainer> losingTrainers = new List<Trainer>();
                for (int i = 0; i < losingTeams.Count; i++)
                {
                    losingTrainers.AddRange(losingTeams[i].trainers);
                }

                // Spectating
                if (isSpectator)
                {
                    string text = GetTrainerNames(winningTeam.trainers) + " defeated " + GetTrainerNames(losingTrainers) + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                // Player
                else
                {
                    // Player wins
                    if (winningTeam.teamID == teamPos) 
                    {
                        string text = "You defeated " + GetTrainerNames(losingTrainers) + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }
                    // Player loses
                    else
                    {
                        string text = "You lost to " + GetTrainerNames(winningTeam.trainers) + "!";
                        yield return StartCoroutine(battleUI.DrawText(text));
                    }
                
                
                }
            }
        }
        

        yield return null;
    }

    // Switching Pokemon
    public IEnumerator SendOutPokemon(Trainer trainer, Pokemon pokemon)
    {
        yield return StartCoroutine(SendOutPokemon(trainer, new List<Pokemon> { pokemon }));
    }
    public IEnumerator SendOutPokemon(Trainer trainer, List<Pokemon> pokemon)
    {
        string text = "";
        if (trainer.playerID == playerID)
        {
            text = "You sent out " + GetPokemonNames(pokemon) + "!";
        }
        else
        {
            text = trainer.name + " sent out " + GetPokemonNames(pokemon) + "!";
        }

        yield return StartCoroutine(battleUI.DrawText(text));
        battleUI.UndrawDialogBox();

        for (int i = 0; i < pokemon.Count; i++)
        {
            battleScene.DrawPokemon(pokemon[i], battleModel, pokemon[i].teamPos == teamPos);
            battleUI.DrawPokemonHUD(pokemon[i], battleModel, pokemon[i].teamPos == teamPos);
        }
    }
    public IEnumerator WithdrawPokemon(Trainer trainer, Pokemon pokemon)
    {
        yield return StartCoroutine(WithdrawPokemon(trainer, new List<Pokemon> { pokemon }));
    }
    public IEnumerator WithdrawPokemon(Trainer trainer, List<Pokemon> pokemon)
    {
        string text = "";
        if (trainer.playerID == playerID)
        {
            text = "Come back, " + GetPokemonNames(pokemon) + "!";
        }
        else
        {
            text = trainer.name + " withdrew " + GetPokemonNames(pokemon) + "!";
        }

        yield return StartCoroutine(battleUI.DrawText(text));
        battleUI.UndrawDialogBox();

        for (int i = 0; i < pokemon.Count; i++)
        {
            battleScene.UndrawPokemon(pokemon[i]);
            battleUI.UndrawPokemonHUD(pokemon[i]);
        }
        yield return null;
    }

    // Move Execution
    public IEnumerator UseMoveDisplay(
        Pokemon pokemon, 
        string moveID)
    {
        string moveName = MoveDatabase.instance.GetMoveData(moveID).moveName;

        // Dialog
        string text = GetPrefix(pokemon) + GetPokemonName(pokemon) + " used " + moveName + "!";
        yield return StartCoroutine(battleUI.DrawText(text));
    }

    public IEnumerator UseMove(
        Pokemon userPokemon,
        string moveID,
        int moveHit,
        List<BattleHitTarget> hitTargets
        )
    {
        List<Pokemon> missedPokemon = new List<Pokemon>();
        for (int i = 0; i < hitTargets.Count; i++)
        {
            if (hitTargets[i].missed)
            {
                missedPokemon.Add(hitTargets[i].pokemon);
            }
        }

        // display missed pokemon
        if (missedPokemon.Count > 0)
        {
            if (battleModel.IsSinglesBattle())
            {
                string missText = "It missed!";
                yield return StartCoroutine(battleUI.DrawText(missText));
            }
            else
            {
                List<Pokemon> enemyDodgers = FilterPokemonByPerspective(missedPokemon, ViewPerspective.Enemy);
                if (enemyDodgers.Count > 0) 
                {
                    string text = GetPrefix(ViewPerspective.Enemy) 
                        + GetPokemonNames(enemyDodgers) 
                        + " avoided the " 
                        + ((moveHit == 1) ? "attack" : "hit") + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> allyDodgers = FilterPokemonByPerspective(missedPokemon, ViewPerspective.Ally);
                if (allyDodgers.Count > 0)
                {
                    string text = GetPrefix(ViewPerspective.Ally)
                        + GetPokemonNames(allyDodgers)
                        + " avoided the "
                        + ((moveHit == 1) ? "attack" : "hit") + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> playerDodgers = FilterPokemonByPerspective(missedPokemon, ViewPerspective.Player);
                if (playerDodgers.Count > 0)
                {
                    string text = GetPrefix(ViewPerspective.Player)
                        + GetPokemonNames(playerDodgers)
                        + " avoided the "
                        + ((moveHit == 1) ? "attack" : "hit") + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
            }
        }
        else if (moveHit == 1
            && hitTargets.Count == 0
            && missedPokemon.Count == 0)
        {
            yield return StartCoroutine(battleUI.DrawText("But there was no target..."));
        }

        // display immune pokemon
        List<Pokemon> immunePokemon = new List<Pokemon>();
        for (int i = 0; i < hitTargets.Count; i++)
        {
            if (hitTargets[i].affectedByMove && hitTargets[i].effectiveness.GetTotalEffectiveness() == 0)
            {
                immunePokemon.Add(hitTargets[i].pokemon);
            }
        }
        if (immunePokemon.Count > 0)
        {
            if (battleModel.IsSinglesBattle())
            {
                yield return StartCoroutine(battleUI.DrawText("It had no effect..."));
            }
            else
            {
                string prefixTxt = "It had no effect on ";

                List<Pokemon> enemyImmune = FilterPokemonByPerspective(immunePokemon, ViewPerspective.Enemy);
                if (enemyImmune.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Enemy, true)
                        + GetPokemonNames(enemyImmune, true)
                        + "...";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> allyImmune = FilterPokemonByPerspective(immunePokemon, ViewPerspective.Ally);
                if (allyImmune.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Ally, true)
                        + GetPokemonNames(allyImmune, true)
                        + "...";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> playerImmune = FilterPokemonByPerspective(immunePokemon, ViewPerspective.Player);
                if (playerImmune.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Player, true)
                        + GetPokemonNames(playerImmune, true)
                        + "...";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
            }
        }

        // TODO: Run animation here
        yield return StartCoroutine(UseMoveAnimation(userPokemon, moveID, moveHit, hitTargets, missedPokemon));
    }

    public IEnumerator UseMoveAnimation(
        Pokemon userPokemon,
        string moveID,
        int moveHit,
        List<BattleHitTarget> hitTargets,
        List<Pokemon> missedPokemon
        )
    {
        battleUI.UndrawDialogBox();
        battleUI.SetPokemonHUDsActive(false);

        // Animate HP Loss
        List<Coroutine> hpRoutines = new List<Coroutine>();
        for (int i = 0; i < hitTargets.Count; i++)
        {
            BattleHitTarget curTarget = hitTargets[i];
            if (curTarget.affectedByMove && curTarget.damageDealt >= 0)
            {
                Coroutine cr = StartCoroutine(DealDamage(
                    pokemon: curTarget.pokemon,
                    preHP: curTarget.preHP,
                    postHP: curTarget.postHP,
                    damageDealt: curTarget.damageDealt,
                    effectiveness: curTarget.effectiveness.GetTotalEffectiveness(),
                    criticalHit: curTarget.criticalHit
                    ));
                hpRoutines.Add(cr);
            }
        }
        for (int i = 0; i < hpRoutines.Count; i++)
        {
            yield return hpRoutines[i];
        }

        // Critical Hits / Effectiveness
        List<Pokemon> criticalTargets = new List<Pokemon>();
        List<Pokemon> superEffTargets = new List<Pokemon>();
        List<Pokemon> nveEffTargets = new List<Pokemon>();
    
        for (int i = 0; i < hitTargets.Count; i++)
        {
            BattleHitTarget curTarget = hitTargets[i];
            if (curTarget.affectedByMove)
            {
                if (curTarget.criticalHit)
                {
                    criticalTargets.Add(curTarget.pokemon);
                }
                if (curTarget.effectiveness.GetTotalEffectiveness() > 1)
                {
                    superEffTargets.Add(curTarget.pokemon);
                }
                else if (curTarget.effectiveness.GetTotalEffectiveness() < 1)
                {
                    nveEffTargets.Add(curTarget.pokemon);
                }
            }
        }
        if (criticalTargets.Count > 0)
        {
            if (battleModel.IsSinglesBattle())
            {
                yield return StartCoroutine(battleUI.DrawText("A critical hit!"));
            }
            else
            {
                string prefixTxt = "It was a critical hit on ";
                List<Pokemon> enemyPokemon = FilterPokemonByPerspective(criticalTargets, ViewPerspective.Enemy);
                if (enemyPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Enemy, true)
                        + GetPokemonNames(enemyPokemon)
                        + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> allyPokemon = FilterPokemonByPerspective(criticalTargets, ViewPerspective.Ally);
                if (allyPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Ally, true)
                        + GetPokemonNames(allyPokemon)
                        + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> playerPokemon = FilterPokemonByPerspective(criticalTargets, ViewPerspective.Player);
                if (playerPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Player, true)
                        + GetPokemonNames(playerPokemon)
                        + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
            }
        }
        if (superEffTargets.Count > 0)
        {
            if (battleModel.IsSinglesBattle())
            {
                yield return StartCoroutine(battleUI.DrawText("It was super-effective!"));
            }
            else
            {
                string prefixTxt = "It was super-effective on ";
                List<Pokemon> enemyPokemon = FilterPokemonByPerspective(superEffTargets, ViewPerspective.Enemy);
                if (enemyPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Enemy, true)
                        + GetPokemonNames(enemyPokemon)
                        + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> allyPokemon = FilterPokemonByPerspective(superEffTargets, ViewPerspective.Ally);
                if (allyPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Ally, true)
                        + GetPokemonNames(allyPokemon)
                        + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> playerPokemon = FilterPokemonByPerspective(superEffTargets, ViewPerspective.Player);
                if (playerPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Player, true)
                        + GetPokemonNames(playerPokemon)
                        + "!";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
            }
        }
        if (nveEffTargets.Count > 0)
        {
            if (battleModel.IsSinglesBattle())
            {
                yield return StartCoroutine(battleUI.DrawText("It was not very effective."));
            }
            else
            {
                string prefixTxt = "It was not very effective on ";
                List<Pokemon> enemyPokemon = FilterPokemonByPerspective(nveEffTargets, ViewPerspective.Enemy);
                if (enemyPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Enemy, true)
                        + GetPokemonNames(enemyPokemon)
                        + ".";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> allyPokemon = FilterPokemonByPerspective(nveEffTargets, ViewPerspective.Ally);
                if (allyPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Ally, true)
                        + GetPokemonNames(allyPokemon)
                        + ".";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }

                List<Pokemon> playerPokemon = FilterPokemonByPerspective(nveEffTargets, ViewPerspective.Player);
                if (playerPokemon.Count > 0)
                {
                    string text = prefixTxt
                        + GetPrefix(ViewPerspective.Player, true)
                        + GetPokemonNames(playerPokemon)
                        + ".";
                    yield return StartCoroutine(battleUI.DrawText(text));
                }
            }
        }
        
    }

    public IEnumerator SwitchPokemonPositions(
        Pokemon pokemon1,
        Pokemon pokemon2
        )
    {
        battleScene.UndrawPokemon(pokemon1);
        battleUI.UndrawPokemonHUD(pokemon1);
        battleScene.UndrawPokemon(pokemon2);
        battleUI.UndrawPokemonHUD(pokemon2);

        battleScene.DrawPokemon(pokemon1, battleModel, pokemon1.teamPos == teamPos);
        battleUI.DrawPokemonHUD(pokemon1, battleModel, pokemon1.teamPos == teamPos);
        battleScene.DrawPokemon(pokemon2, battleModel, pokemon2.teamPos == teamPos);
        battleUI.DrawPokemonHUD(pokemon2, battleModel, pokemon2.teamPos == teamPos);

        yield return null;
    }

    public IEnumerator ChangePokemon(
        Pokemon pokemon,
        string prePokemonID,
        string postPokemonID
        )
    {
        battleScene.UndrawPokemon(pokemon);
        battleUI.UndrawPokemonHUD(pokemon);

        PokemonData preFormData = PokemonDatabase.instance.GetPokemonData(prePokemonID);
        PokemonData postFormData = PokemonDatabase.instance.GetPokemonData(postPokemonID);

        Debug.Log("DEBUG - " + pokemon.nickname + " changed from "
            + preFormData.speciesName + " (" + preFormData.formName + ") to "
            + postFormData.speciesName + " (" + postFormData.formName + ") ");

        battleScene.DrawPokemon(
            pokemon: pokemon, 
            battle: battleModel, 
            isNear: pokemon.teamPos == teamPos);
        battleUI.DrawPokemonHUD(pokemon, battleModel, pokemon.teamPos == teamPos);
        yield return null;
    }

    // Damage Execution
    public IEnumerator DealDamage(
        Pokemon pokemon,
        int preHP,
        int postHP,
        int damageDealt,
        float effectiveness = 1f,
        bool criticalHit = false,
        int hitDisplay = 0
        )
    {
        string effectivenessString = (effectiveness > 1f) ? "(Super-effective) "
            : (effectiveness < 1f && effectiveness > 0) ? "(Resisted)"
            : (effectiveness == 0) ? "(Immune)"
            : "";
        string criticalString = (criticalHit) ? "(Critical)" : "";
        string hitCountString = (hitDisplay == 0) ? "" : "(Hit " + hitDisplay + ")";

        Debug.Log(GetPokemonName(pokemon) + " lost " + damageDealt + " HP!" 
            + " - (" + postHP + "/" + pokemon.maxHP + ")" 
            + " " + effectivenessString
            + " " + criticalString
            + " " + hitCountString);

        battleUI.UndrawDialogBox();
        battleUI.SetPokemonHUDActive(pokemon, true);
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(battleUI.AnimatePokemonHUDHPChange(
            pokemon,
            preHP,
            postHP
            ));
        yield return new WaitForSeconds(0.25f);
    }

    public IEnumerator HealDamage(
        Pokemon pokemon,
        int preHP,
        int postHP,
        int hpHealed
        )
    {
        Debug.Log(GetPokemonName(pokemon) + " recovered " + hpHealed + " HP!"
            + " - (" + postHP + "/" + pokemon.maxHP + ")");

        battleUI.UndrawDialogBox();
        battleUI.SetPokemonHUDActive(pokemon, true);
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(battleUI.AnimatePokemonHUDHPChange(
            pokemon,
            preHP,
            postHP
            ));
        yield return new WaitForSeconds(0.25f);
    }

    public IEnumerator InflictStatus(
        Pokemon pokemon,
        string statusID
        )
    {
        string statusString = StatusPKDatabase.instance.GetStatusData(statusID).conditionName;
        string text = statusString + " was inflicted on " + GetPokemonName(pokemon);
        Debug.Log("ANIM - " + text);

        battleUI.UpdatePokemonHUD(pokemon, battleModel);
        yield return null;
    }

    public IEnumerator ApplyStatStageMod(
        Pokemon pokemon,
        int modValue,
        PokemonStats[] statMods
        )
    {
        string text = "(";
        for (int i = 0; i < statMods.Length; i++)
        {
            text += statMods[i].ToString() + ((i == statMods.Length - 1)? ")" : " / ");
        }
        text += " (" + modValue + ")";
        Debug.Log(text);
        yield return null;
    }

    // Abilities
    public IEnumerator IndicateAbility(
        Pokemon pokemon,
        string abilityID
        )
    {
        string text = "("
            + GetPokemonName(pokemon) 
            + "'s " + AbilityDatabase.instance.GetAbilityData(abilityID).abilityName
            + ")";
        Debug.Log(text);
        yield return null;
    }

    public IEnumerator FaintPokemon(
        List<Pokemon> pokemon)
    {
        if (pokemon.Count > 0)
        {
            string faintNames = GetPokemonNames(pokemon);
            List<Pokemon> enemyPokemon = new List<Pokemon>();
            List<Pokemon> allyPokemon = new List<Pokemon>();
            List<Pokemon> playerPokemon = new List<Pokemon>();

            for (int i = 0; i < pokemon.Count; i++)
            {
                ViewPerspective viewPerspective = GetPerspective(pokemon[i]);
                if (viewPerspective == ViewPerspective.Ally)
                {
                    allyPokemon.Add(pokemon[i]);
                }
                else if (viewPerspective == ViewPerspective.Enemy)
                {
                    enemyPokemon.Add(pokemon[i]);
                }
                else
                {
                    playerPokemon.Add(pokemon[i]);
                }
            }
            
            if (enemyPokemon.Count > 0)
            {
                string text = GetPrefix(ViewPerspective.Enemy) + GetPokemonNames(enemyPokemon) + " fainted!";
                yield return StartCoroutine(battleUI.DrawText(text));
            }
            if (allyPokemon.Count > 0)
            {
                string text = GetPrefix(ViewPerspective.Ally) + GetPokemonNames(allyPokemon) + " fainted!";
                yield return StartCoroutine(battleUI.DrawText(text));
            }
            if (playerPokemon.Count > 0)
            {
                string text = GetPrefix(ViewPerspective.Player) + GetPokemonNames(playerPokemon) + " fainted!";
                yield return StartCoroutine(battleUI.DrawText(text));
            }
        
            // undraw them all
            for (int i = 0; i < pokemon.Count; i++)
            {
                battleScene.UndrawPokemon(pokemon[i]);
                battleUI.UndrawPokemonHUD(pokemon[i]);
            }
        }
    }

    // Text / Names
    public ViewPerspective GetPerspective(Pokemon pokemon)
    {
        Trainer trainer = battleModel.GetPokemonOwner(pokemon);
        if (trainer.teamID != teamPos)
        {
            return ViewPerspective.Enemy;
        }
        else if (isSpectator || trainer.playerID == playerID)
        {
            return ViewPerspective.Player;
        }
        else
        {
            return ViewPerspective.Ally;
        }
    }
    public List<Pokemon> FilterPokemonByPerspective(List<Pokemon> pokemon, ViewPerspective viewPerspective)
    {
        List<Pokemon> filteredPokemon = new List<Pokemon>();
        for (int i = 0; i < pokemon.Count; i++)
        {
            if (GetPerspective(pokemon[i]) == viewPerspective)
            {
                filteredPokemon.Add(pokemon[i]);
            }
        }
        return filteredPokemon;
    }
    public string GetPrefix(ViewPerspective viewPerspective, bool lowercase = false)
    {
        string prefix = (viewPerspective == ViewPerspective.Ally) ? "The ally "
            : (viewPerspective == ViewPerspective.Enemy) ? 
                (battleModel.battleSettings.isWildBattle? "The wild " : "The foe's ")
            : "";
        if (lowercase)
        {
            prefix = prefix.ToLower();
        }

        return prefix;
    }
    public string GetPrefix(Pokemon pokemon, bool capitalize = true)
    {
        string text = "";
        if (pokemon.teamPos != teamPos)
        {
            text = "The opposing ";
        }
        else
        {
            if (battleModel.GetPokemonOwner(pokemon).playerID != playerID)
            {
                text = "The ally ";
            }
        }
        if (!capitalize)
        {
            text = text.ToLower();
            text = " " + text;
        }
        return text;
    }
    public string GetPokemonName(Pokemon pokemon)
    {
        return GetPokemonNames(new List<Pokemon> { pokemon });
    }
    public string GetPokemonNames(List<Pokemon> pokemonList, bool orConjunct = false)
    {
        string conjunct = (orConjunct) ? "or" : "and";

        string names = "";
        if (pokemonList.Count == 1)
        {
            return pokemonList[0].nickname;
        }
        else if (pokemonList.Count == 2)
        {
            return pokemonList[0].nickname + " " + conjunct + " " + pokemonList[1].nickname;
        }
        else
        {
            for (int i = 0; i < pokemonList.Count; i++)
            {
                names += (i == pokemonList.Count - 1) ?
                    conjunct + " " + pokemonList[i].nickname :
                    pokemonList[i].nickname + ", ";
            }
        }
        return names;
    }
    public string GetTrainerName(Trainer trainer)
    {
        return GetTrainerNames(new List<Trainer> { trainer });
    }
    public string GetTrainerNames(List<Trainer> trainers)
    {
        string names = "";
        if (trainers.Count == 1)
        {
            return trainers[0].name;
        }
        else if (trainers.Count == 2)
        {
            return trainers[0].name + " and " + trainers[1].name;
        }
        else
        {
            for (int i = 0; i < trainers.Count; i++)
            {
                names += (i == trainers.Count - 1) ?
                    "and " + trainers[i].name :
                    trainers[i].name + ", ";
            }
        }
        return names;
    }

    // Controller

    // Dialog
    public string GetGameText(BTLEvent_GameText bEvent)
    {
        if (bEvent.textID != null)
        {
            GameTextData gameTextData = GameTextDatabase.instance.GetGameTextData(bEvent.textID);
            string baseText = gameTextData.GetText();
            string gameText = GameTextDatabase.ConvertToString(
                baseString: baseText,
                viewPos: teamPos,
                playerID: playerID,
                pokemon: bEvent.pokemon,
                userPokemon: bEvent.userPokemon,
                targetPokemon: bEvent.targetPokemon,
                pokemonList: bEvent.pokemonList,
                trainer: bEvent.trainer,
                targetTeam: bEvent.targetTeam,
                statList: bEvent.statList,
                typeID: bEvent.typeID,
                moveID: bEvent.moveID,
                abilityID: bEvent.abilityID,
                itemID: bEvent.itemID,
                statusID: bEvent.statusID,
                teamStatusID: bEvent.teamStatusID,
                battleStatusID: bEvent.battleStatusID,
                moveIDs: bEvent.moveIDs,
                typeIDs: bEvent.typeIDs,
                battleModel: bEvent.battleModel,

                intArgs: bEvent.intArgs
                );
            return gameText;
        }
        return null;
    }

}

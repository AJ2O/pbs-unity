using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLUIPanelHUD : BTLUIPanel
{
    [Header("Pokemon HUD")]
    public BTLUI_PokemonHUD pokemonHUDPrefab;
    public GameObject pokemonHUDNearRoot,
        pokemonHUDFarRoot;
    public Transform
        pokemonHUDSpawnNearSingle,
        pokemonHUDSpawnFarSingle,

        pokemonHUDSpawnNearDouble0,
        pokemonHUDSpawnNearDouble1,
        pokemonHUDSpawnFarDouble0,
        pokemonHUDSpawnFarDouble1,

        pokemonHUDSpawnNearTriple0,
        pokemonHUDSpawnNearTriple1,
        pokemonHUDSpawnNearTriple2,
        pokemonHUDSpawnFarTriple0,
        pokemonHUDSpawnFarTriple1,
        pokemonHUDSpawnFarTriple2;
    [HideInInspector] public List<BTLUI_PokemonHUD> pokemonHUDs = new List<BTLUI_PokemonHUD>();

    // WIP
    [Header("Party HUD")]
    public GameObject partyHUDPrefab;
    public GameObject partyHUDNearRoot,
        partyHUDFarRoot;
    public Transform
        partyHUDSpawnNear,
        partyHUDSpawnFar,
        partyHUDSpawnNearDouble,
        partyHUDSpawnFarDouble,
        partyHUDSpawnNearTriple,
        partyHUDSpawnFarTriple;
    [HideInInspector] public List<GameObject> partyHUDs = new List<GameObject>();

    private void Awake()
    {
        ClearSelf();
    }

    public override void ClearSelf()
    {
        base.ClearSelf();
        pokemonHUDs = new List<BTLUI_PokemonHUD>();
        partyHUDs = new List<GameObject>();
    }

    // HUD Activity
    public void SetPokemonHUDActive(Pokemon pokemon, bool active)
    {
        BTLUI_PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
        if (pokemonHUD != null)
        {
            pokemonHUD.gameObject.SetActive(active);
        }
    }
    public void SetPokemonHUDsActive(bool active)
    {
        for (int i = 0; i < pokemonHUDs.Count; i++)
        {
            pokemonHUDs[i].gameObject.SetActive(active);
        }
    }

    // Setting HUD Parameters
    public Transform GetPokemonHUDSpawnPosition(Pokemon pokemon, Battle battleModel, bool isNear)
    {
        // get spawn position
        Transform spawnPos = null;
        BattleTeam team = battleModel.GetTeam(pokemon);
        switch (team.teamMode)
        {
            case BattleTeam.TeamMode.Single:
                spawnPos = isNear ? pokemonHUDSpawnNearSingle : pokemonHUDSpawnFarSingle;
                break;

            case BattleTeam.TeamMode.Double:
                spawnPos = (pokemon.battlePos == 0) ? (isNear ? pokemonHUDSpawnNearDouble0 : pokemonHUDSpawnFarDouble0)
                    : isNear ? pokemonHUDSpawnNearDouble1 : pokemonHUDSpawnFarDouble1;
                break;

            case BattleTeam.TeamMode.Triple:
                spawnPos = (pokemon.battlePos == 0) ? (isNear ? pokemonHUDSpawnNearTriple0 : pokemonHUDSpawnFarTriple0)
                    : (pokemon.battlePos == 1) ? (isNear ? pokemonHUDSpawnNearTriple1 : pokemonHUDSpawnFarTriple1)
                    : isNear ? pokemonHUDSpawnNearTriple2 : pokemonHUDSpawnFarTriple2;
                break;
        }
        return spawnPos;
    }
    public BTLUI_PokemonHUD GetPokemonHUD(Pokemon pokemon)
    {
        for (int i = 0; i < pokemonHUDs.Count; i++)
        {
            if (pokemonHUDs[i].pokemonUniqueID == pokemon.uniqueID)
            {
                return pokemonHUDs[i];
            }
        }
        return null;
    }
    public void UpdatePokemonHUD(Pokemon pokemon, Battle battle)
    {
        BTLUI_PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
        if (pokemonHUD != null)
        {
            pokemonHUD.nameTxt.text = pokemon.nickname;
            PokemonGender gender = pokemon.gender;
            if (gender != PokemonGender.Genderless)
            {
                pokemonHUD.nameTxt.text += (gender == PokemonGender.Male) ? " <color=#8080FF>♂</color>"
                    : " <color=#FF8080>♀</color>";
            }

            pokemonHUD.lvlTxt.text = "<color=yellow>Lv</color>" + pokemon.level;
            pokemonHUD.statusTxt.text = "";
            StatusCondition condition = pokemon.nonVolatileStatus;
            if (condition != null)
            {
                if (condition.statusID != "healthy")
                {
                    pokemonHUD.statusTxt.text = condition.data.shortName;
                }
            }

            pokemonHUD.hpTxt.text = pokemon.currentHP + " / " + pokemon.maxHP;

            float hpPercent = battle.GetPokemonHPAsPercentage(pokemon);
            pokemonHUD.hpBar.fillAmount = hpPercent;

            pokemonHUD.hpBar.color = (hpPercent > 0.5f) ? pokemonHUD.hpHigh
                : (hpPercent > 0.25f) ? pokemonHUD.hpMed
                : pokemonHUD.hpLow;
        }
    }

    // Drawing HUD
    public BTLUI_PokemonHUD DrawPokemonHUD(Pokemon pokemon, Battle battleModel, bool isNear)
    {
        // get spawn position
        Transform spawnPos = GetPokemonHUDSpawnPosition(pokemon, battleModel, isNear);
        BattleTeam team = battleModel.GetTeam(pokemon);
        if (spawnPos == null)
        {
            Debug.LogWarning("Could not find HUD Spawn Position for " + pokemon.nickname);
            return null;
        }
        else
        {
            // draw pokemon HUD
            BTLUI_PokemonHUD pokemonHUD = Instantiate(pokemonHUDPrefab, spawnPos.position, Quaternion.identity, spawnPos);
            pokemonHUD.pokemonUniqueID = pokemon.uniqueID;
            pokemonHUD.hpObj.gameObject.SetActive(isNear
                && (team.teamMode == BattleTeam.TeamMode.Single
                    || team.teamMode == BattleTeam.TeamMode.Double));
            // set EXP bar
            pokemonHUD.expObj.SetActive(isNear
                && (team.teamMode == BattleTeam.TeamMode.Single
                    || team.teamMode == BattleTeam.TeamMode.Double));
            pokemonHUDs.Add(pokemonHUD);

            UpdatePokemonHUD(pokemon, battleModel);
            return pokemonHUD;
        }
    }
    public bool UndrawPokemonHUD(Pokemon pokemon)
    {
        BTLUI_PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
        if (pokemonHUD != null)
        {
            pokemonHUDs.Remove(pokemonHUD);
            Destroy(pokemonHUD.gameObject);
            return true;
        }
        return false;
    }

    // Animation
    public IEnumerator AnimatePokemonHUDHPChange(
        Pokemon pokemon, 
        Battle battleModel, 
        int preHP, int postHP, 
        float timeSpan = 1f)
    {
        BTLUI_PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
        if (pokemonHUD != null)
        {
            int maxHP = pokemon.maxHP;
            float preValue = pokemonHUD.hpBar.fillAmount;
            float postValue = battleModel.GetPokemonHPPercentGivenHP(pokemon, postHP);

            float difference = postValue - preValue;
            float increment = (timeSpan == 0) ? 1f : 0f;
            while (increment < 1)
            {
                increment += (1 / timeSpan) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                float curFillAmount = preValue + difference * increment;
                int displayHP = Mathf.FloorToInt(curFillAmount * maxHP);
                if (displayHP == 0 && curFillAmount > 0)
                {
                    displayHP = 1;
                }

                // display changes
                pokemonHUD.hpTxt.text = displayHP + " / " + maxHP;
                pokemonHUD.hpBar.fillAmount = curFillAmount;
                pokemonHUD.hpBar.color = (curFillAmount > 0.5f) ? pokemonHUD.hpHigh
                    : (curFillAmount > 0.25f) ? pokemonHUD.hpMed
                    : pokemonHUD.hpLow;

                yield return null;
            }

            pokemonHUD.hpTxt.text = postHP + " / " + maxHP;
            pokemonHUD.hpBar.fillAmount = postValue;
            pokemonHUD.hpBar.color = (postValue > 0.5f) ? pokemonHUD.hpHigh
                : (postValue > 0.25f) ? pokemonHUD.hpMed
                : pokemonHUD.hpLow;
            yield return null;
        }
    }

}

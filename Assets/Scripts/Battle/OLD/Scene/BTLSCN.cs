using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTLSCN : MonoBehaviour
{
    // General
    [Header("Camera")]
    public BTLCamera mainCamera;

    [Header("Prefabs")]
    public BTLSCN_PokemonBW pokemonPrefab;

    [Header("Team Spawns")]
    public Transform spawnTeamNear;
    public Transform spawnTeamFar;

    [Header("Pokemon Spawns")]
    public Transform spawnNearSingle;
    public Transform spawnFarSingle,

        spawnNearDouble0,
        spawnNearDouble1,
        spawnFarDouble0,
        spawnFarDouble1,

        spawnNearTriple0,
        spawnNearTriple1,
        spawnNearTriple2,
        spawnFarTriple0,
        spawnFarTriple1,
        spawnFarTriple2;

    // Scene Objects
    [HideInInspector] public List<BTLSCN_PokemonBW> scnPokemon = new List<BTLSCN_PokemonBW>();
    public Battle battleModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Model
    public void UpdateModel(Battle battle)
    {
        battleModel = battle;
    }

    // Scene Team

    // Scene Pokemon
    public BTLSCN_PokemonBW DrawPokemon(Pokemon pokemon, Battle battle, bool isNear)
    {
        // get spawn position
        Transform spawnPos = this.transform;
        BattleTeam team = battle.GetTeam(pokemon);
        switch (team.teamMode)
        {
            case BattleTeam.TeamMode.Single:
                spawnPos = (isNear) ? spawnNearSingle : spawnFarSingle;
                break;

            case BattleTeam.TeamMode.Double:
                spawnPos = (pokemon.battlePos == 0) ? (isNear ? spawnNearDouble0 : spawnFarDouble0)
                    : isNear ? spawnNearDouble1 : spawnFarDouble1;
                break;

            case BattleTeam.TeamMode.Triple:
                spawnPos = (pokemon.battlePos == 0) ? (isNear ? spawnNearTriple0 : spawnFarTriple0)
                    : (pokemon.battlePos == 1) ? (isNear ? spawnNearTriple1 : spawnFarTriple1)
                    : isNear ? spawnNearTriple2 : spawnFarTriple2;
                break;
        }

        // draw pokemon
        string drawPath = "pokemonSprites/" + (isNear ? "back/" : "front/") + pokemon.data.displayID;

        BTLSCN_PokemonBW newScnPokemon = Instantiate(pokemonPrefab, spawnPos.position, Quaternion.identity, spawnPos);
        newScnPokemon.spriteRenderer.sprite = BattleAssetLoader.instance.nullSprite;
        newScnPokemon.shadowRenderer.sprite = BattleAssetLoader.instance.nullSprite;
        newScnPokemon.pokemonUniqueID = pokemon.uniqueID;

        // positioning
        float xOffset = isNear ? pokemon.data.xOffset2DNear : pokemon.data.xOffset2DFar;
        float yOffset = isNear ? pokemon.data.yOffset2DNear : pokemon.data.yOffset2DFar;
        newScnPokemon.transform.localPosition = new Vector3(xOffset, yOffset);

        // load model
        newScnPokemon.pokemonID = pokemon.pokemonID;
        StartCoroutine(BattleAssetLoader.instance.LegacyLoadPokemon(
            pokemon: pokemon,
            useFront: !isNear,
            useBack: isNear,
            scnPokemonBW: newScnPokemon
            ));

        scnPokemon.Add(newScnPokemon);
        return newScnPokemon;
    }
    public bool UndrawPokemon(Pokemon pokemon)
    {
        BTLSCN_PokemonBW oldScnPokemon = GetSCNPokemon(pokemon);
        if (oldScnPokemon != null)
        {
            scnPokemon.Remove(oldScnPokemon);
            Destroy(oldScnPokemon.gameObject);
            return true;
        }
        return false;
    }
    public BTLSCN_PokemonBW GetSCNPokemon(Pokemon pokemon)
    {
        for (int i = 0; i < scnPokemon.Count; i++)
        {
            if (scnPokemon[i].pokemonUniqueID == pokemon.uniqueID)
            {
                return scnPokemon[i];
            }
        }
        return null;
    }
}

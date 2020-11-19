using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Scene
{
    public class Canvas : MonoBehaviour
    {
        // General
        [Header("Camera")]
        public Scene.BattleCamera mainCamera;

        [Header("Prefabs")]
        public Entities.Pokemon pokemonPrefab;

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
        [HideInInspector] public List<Entities.Pokemon> scnPokemon = new List<Entities.Pokemon>();

        // Scene Pokemon
        public Entities.Pokemon DrawPokemon(
            Battle.View.Compact.Pokemon pokemon,
            TeamMode teamMode,
            bool isNear)
        {
            // get spawn position
            Transform spawnPos = this.transform;
            switch (teamMode)
            {
                case TeamMode.Single:
                    spawnPos = (isNear) ? spawnNearSingle : spawnFarSingle;
                    break;

                case TeamMode.Double:
                    spawnPos = (pokemon.battlePos == 0) ? (isNear ? spawnNearDouble0 : spawnFarDouble0)
                        : isNear ? spawnNearDouble1 : spawnFarDouble1;
                    break;

                case TeamMode.Triple:
                    spawnPos = (pokemon.battlePos == 0) ? (isNear ? spawnNearTriple0 : spawnFarTriple0)
                        : (pokemon.battlePos == 1) ? (isNear ? spawnNearTriple1 : spawnFarTriple1)
                        : isNear ? spawnNearTriple2 : spawnFarTriple2;
                    break;
            }

            // draw pokemon
            PokemonData pokemonData = PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID);
            string drawPath = "pokemonSprites/" + (isNear ? "back/" : "front/") + pokemonData.displayID;

            Entities.Pokemon newScnPokemon = Instantiate(pokemonPrefab, spawnPos.position, Quaternion.identity, spawnPos);
            newScnPokemon.spriteRenderer.sprite = BattleAssetLoader.instance.nullSprite;
            newScnPokemon.shadowRenderer.sprite = BattleAssetLoader.instance.nullSprite;
            newScnPokemon.pokemonUniqueID = pokemon.uniqueID;

            // positioning
            float xOffset = isNear ? pokemonData.xOffset2DNear : pokemonData.xOffset2DFar;
            float yOffset = isNear ? pokemonData.yOffset2DNear : pokemonData.yOffset2DFar;
            newScnPokemon.transform.localPosition = new Vector3(xOffset, yOffset);

            // load model
            newScnPokemon.pokemonID = pokemon.pokemonID;
            StartCoroutine(BattleAssetLoader.instance.LoadPokemon(
                pokemon: pokemon,
                useFront: !isNear,
                useBack: isNear,
                scnPokemonNew: newScnPokemon
                ));

            scnPokemon.Add(newScnPokemon);
            return newScnPokemon;
        }
        public bool UndrawPokemon(string pokemonUniqueID)
        {
            Entities.Pokemon oldScnPokemon = GetSCNPokemon(pokemonUniqueID);
            if (oldScnPokemon != null)
            {
                scnPokemon.Remove(oldScnPokemon);
                Destroy(oldScnPokemon.gameObject);
                return true;
            }
            return false;
        }
        public Entities.Pokemon GetSCNPokemon(string pokemonUniqueID)
        {
            for (int i = 0; i < scnPokemon.Count; i++)
            {
                if (scnPokemon[i].pokemonUniqueID == pokemonUniqueID)
                {
                    return scnPokemon[i];
                }
            }
            return null;
        }
    }
}

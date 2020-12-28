using PBS.Data;
using PBS.Databases;
using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.Scene
{
    /// <summary>
    /// This canvas handles orchestrating the battle scene and its components. This includes drawing the background,
    /// pokemon, trainers, and other on-field conponents.
    /// </summary>
    public class Canvas : MonoBehaviour
    {
        #region Attributes
        /// <summary>
        /// The main camera displaying the scene canvas.
        /// </summary>
        [Header("Camera")]
        public Scene.BattleCamera mainCamera;

        /// <summary>
        /// The base pokemon object used to spawn into view when a pokemon joins the battle.
        /// </summary>
        [Header("Prefabs")]
        public Entities.Pokemon pokemonPrefab;

        /// <summary>
        /// The root location of the ally team.
        /// </summary>
        [Header("Team Spawns")]
        public Transform spawnTeamNear;
        /// <summary>
        /// The root location of the opposing team.
        /// </summary>
        public Transform spawnTeamFar;

        /// <summary>
        /// The root location of a pokemon depending on the perspective of the player, and the battle type.
        /// </summary>
        [Header("Pokemon Spawns")]
        public Transform spawnNearSingle;
        /// <summary>
        /// The root location of a pokemon depending on the perspective of the player, and the battle type.
        /// </summary>
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
        /// <summary>
        /// A list of all pokemon currently on the canvas.
        /// </summary>
        [HideInInspector] 
        public List<Entities.Pokemon> scnPokemon = new List<Entities.Pokemon>();
        #endregion

        #region Pokemon
        /// <summary>
        /// Draws the given pokemon on the canvas.
        /// </summary>
        /// <param name="pokemon">The pokemon to be drawn.</param>
        /// <param name="teamMode">The pokemon's team's battle type for reference.</param>
        /// <param name="isNear">If true, this pokemon is drawn as being part of the ally team.</param>
        /// <returns></returns>
        public Entities.Pokemon DrawPokemon(
            Battle.View.WifiFriendly.Pokemon pokemon,
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
            PBS.Data.Pokemon pokemonData = PBS.Databases.Pokemon.instance.GetPokemonData(pokemon.pokemonID);
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
        /// <summary>
        /// Removes the given pokemon from the canvas.
        /// </summary>
        /// <param name="pokemonUniqueID">The ID of the pokemon to remove.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns the scene object reference for the given pokemon.
        /// </summary>
        /// <param name="pokemonUniqueID">The ID of the pokemon to get the scene object for.</param>
        /// <returns></returns>
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
        #endregion
    }
}

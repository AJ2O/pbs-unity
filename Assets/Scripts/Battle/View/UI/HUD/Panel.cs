using PBS.Enums.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.UI.HUD
{
    /// <summary>
    /// This component handles HUD elements on the battle UI.
    /// </summary>
    public class Panel : MonoBehaviour
    {
        #region Attributes
        [Header("Pokemon HUD")]
        [Tooltip("The preset for Pokemon Health HUD elements.")]
        public PokemonHUD pokemonHUDPrefab;
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
        [HideInInspector] public List<PokemonHUD> pokemonHUDs = new List<PokemonHUD>();

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
        #endregion

        private void Awake()
        {
            ClearSelf();
        }

        /// <summary>
        /// Clears Pokemon and Party HUD elements.
        /// </summary>
        public void ClearSelf()
        {
            pokemonHUDs = new List<PokemonHUD>();
            partyHUDs = new List<GameObject>();
        }

        #region Pokemon HUD
        /// <summary>
        /// Displays or hides the HUD box associated with the given Pokemon.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <param name="active"></param>
        public void SetPokemonHUDActive(WifiFriendly.Pokemon pokemon, bool active)
        {
            PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
            if (pokemonHUD != null)
            {
                pokemonHUD.gameObject.SetActive(active);
            }
        }
        /// <summary>
        /// Displays or hides all Pokemon HUD boxes.
        /// </summary>
        /// <param name="active"></param>
        public void SetPokemonHUDsActive(bool active)
        {
            for (int i = 0; i < pokemonHUDs.Count; i++)
            {
                pokemonHUDs[i].gameObject.SetActive(active);
            }
        }

        /// <summary>
        /// Returns the root position of the given Pokemon.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <param name="teamMode"></param>
        /// <param name="isNear"></param>
        /// <returns></returns>
        public Transform GetPokemonHUDSpawnPosition(
            WifiFriendly.Pokemon pokemon,
            TeamMode teamMode,
            bool isNear)
        {
            // get spawn position
            Transform spawnPos = null;
            switch (teamMode)
            {
                case TeamMode.Single:
                    spawnPos = isNear ? pokemonHUDSpawnNearSingle : pokemonHUDSpawnFarSingle;
                    break;

                case TeamMode.Double:
                    spawnPos = (pokemon.battlePos == 0) ? (isNear ? pokemonHUDSpawnNearDouble0 : pokemonHUDSpawnFarDouble0)
                        : isNear ? pokemonHUDSpawnNearDouble1 : pokemonHUDSpawnFarDouble1;
                    break;

                case TeamMode.Triple:
                    spawnPos = (pokemon.battlePos == 0) ? (isNear ? pokemonHUDSpawnNearTriple0 : pokemonHUDSpawnFarTriple0)
                        : (pokemon.battlePos == 1) ? (isNear ? pokemonHUDSpawnNearTriple1 : pokemonHUDSpawnFarTriple1)
                        : isNear ? pokemonHUDSpawnNearTriple2 : pokemonHUDSpawnFarTriple2;
                    break;
            }
            return spawnPos;
        }
        /// <summary>
        /// Returns the HUD box associated with the given Pokemon.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public PokemonHUD GetPokemonHUD(WifiFriendly.Pokemon pokemon)
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
        /// <summary>
        /// Updates the HUD box associated with the given Pokemon with the given statistics.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <param name="nickname"></param>
        /// <param name="gender"></param>
        /// <param name="level"></param>
        /// <param name="nonVolatileStatusID"></param>
        /// <param name="currentHP"></param>
        /// <param name="maxHP"></param>
        public void UpdatePokemonHUD(
            WifiFriendly.Pokemon pokemon,
            string nickname = "",
            PokemonGender gender = PokemonGender.Genderless,
            int level = 1,
            string nonVolatileStatusID = null,
            int currentHP = 1,
            int maxHP = 1)
        {
            PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
            if (pokemonHUD != null)
            {
                pokemonHUD.nameTxt.text = nickname;
                if (gender != PokemonGender.Genderless)
                {
                    pokemonHUD.nameTxt.text += (gender == PokemonGender.Male) ? " <color=#8080FF>♂</color>"
                        : " <color=#FF8080>♀</color>";
                }

                pokemonHUD.lvlTxt.text = "<color=yellow>Lv</color>" + level;
                pokemonHUD.statusTxt.text = "";
                if (!string.IsNullOrEmpty(nonVolatileStatusID))
                {
                    StatusPKData statusData = StatusPKDatabase.instance.GetStatusData(nonVolatileStatusID);
                    pokemonHUD.statusTxt.text = statusData.shortName;
                }

                pokemonHUD.hpTxt.text = currentHP + " / " + maxHP;

                float hpPercent = ((float)currentHP) / maxHP; 
                pokemonHUD.hpBar.fillAmount = hpPercent;

                pokemonHUD.hpBar.color = (hpPercent > 0.5f) ? pokemonHUD.hpHigh
                    : (hpPercent > 0.25f) ? pokemonHUD.hpMed
                    : pokemonHUD.hpLow;
            }
        }

        /// <summary>
        /// Spawns a HUD box associated with the given Pokemon.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <param name="teamMode"></param>
        /// <param name="isNear"></param>
        /// <returns></returns>
        public PokemonHUD DrawPokemonHUD(
            WifiFriendly.Pokemon pokemon,
            TeamMode teamMode,
            bool isNear)
        {
            // get spawn position
            Transform spawnPos = GetPokemonHUDSpawnPosition(pokemon, teamMode, isNear);
            if (spawnPos == null)
            {
                Debug.LogWarning("Could not find HUD Spawn Position for " + pokemon.nickname);
                return null;
            }
            else
            {
                // draw pokemon HUD
                PokemonHUD pokemonHUD = Instantiate(pokemonHUDPrefab, spawnPos.position, Quaternion.identity, spawnPos);
                pokemonHUD.pokemonUniqueID = pokemon.uniqueID;
                pokemonHUD.hpObj.gameObject.SetActive(isNear
                    && (teamMode == TeamMode.Single
                        || teamMode == TeamMode.Double));
                // set EXP bar
                pokemonHUD.expObj.SetActive(isNear
                    && (teamMode == TeamMode.Single
                        || teamMode == TeamMode.Double));
                pokemonHUDs.Add(pokemonHUD);

                UpdatePokemonHUD(
                    pokemon: pokemon,
                    nickname: pokemon.nickname,
                    gender: pokemon.gender,
                    level: pokemon.level,
                    nonVolatileStatusID: pokemon.nonVolatileStatus,
                    currentHP: pokemon.currentHP,
                    maxHP: pokemon.maxHP);
                return pokemonHUD;
            }
        }
        /// <summary>
        /// Hides the HUD box associated with the given Pokemon.
        /// </summary>
        /// <param name="pokemon"></param>
        /// <returns></returns>
        public bool UndrawPokemonHUD(WifiFriendly.Pokemon pokemon)
        {
            PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
            if (pokemonHUD != null)
            {
                pokemonHUDs.Remove(pokemonHUD);
                Destroy(pokemonHUD.gameObject);
                return true;
            }
            return false;
        }
        #endregion

        #region Animation
        public IEnumerator AnimatePokemonHUDHPChange(
            WifiFriendly.Pokemon pokemon,
            int preHP, 
            int postHP, 
            int maxHP,
            float timeSpan = 1f)
        {
            PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
            if (pokemonHUD != null)
            {
                float preValue = pokemonHUD.hpBar.fillAmount;
                float postValue = ((float)postHP / maxHP);

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
        #endregion
    }
}


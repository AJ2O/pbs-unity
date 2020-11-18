using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBS.Battle.View.UI.HUD
{
    public class Panel : MonoBehaviour
    {
        [Header("Pokemon HUD")]
        public HUD.PokemonHUD pokemonHUDPrefab;
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
        [HideInInspector] public List<HUD.PokemonHUD> pokemonHUDs = new List<HUD.PokemonHUD>();

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

        public void ClearSelf()
        {
            pokemonHUDs = new List<HUD.PokemonHUD>();
            partyHUDs = new List<GameObject>();
        }

        // HUD Activity
        public void SetPokemonHUDActive(PBS.Battle.View.Compact.Pokemon pokemon, bool active)
        {
            HUD.PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
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
        public Transform GetPokemonHUDSpawnPosition(
            PBS.Battle.View.Compact.Pokemon pokemon, 
            PBS.Battle.Enums.TeamMode teamMode,
            bool isNear)
        {
            // get spawn position
            Transform spawnPos = null;
            switch (teamMode)
            {
                case Battle.Enums.TeamMode.Single:
                    spawnPos = isNear ? pokemonHUDSpawnNearSingle : pokemonHUDSpawnFarSingle;
                    break;

                case Battle.Enums.TeamMode.Double:
                    spawnPos = (pokemon.battlePos == 0) ? (isNear ? pokemonHUDSpawnNearDouble0 : pokemonHUDSpawnFarDouble0)
                        : isNear ? pokemonHUDSpawnNearDouble1 : pokemonHUDSpawnFarDouble1;
                    break;

                case Battle.Enums.TeamMode.Triple:
                    spawnPos = (pokemon.battlePos == 0) ? (isNear ? pokemonHUDSpawnNearTriple0 : pokemonHUDSpawnFarTriple0)
                        : (pokemon.battlePos == 1) ? (isNear ? pokemonHUDSpawnNearTriple1 : pokemonHUDSpawnFarTriple1)
                        : isNear ? pokemonHUDSpawnNearTriple2 : pokemonHUDSpawnFarTriple2;
                    break;
            }
            return spawnPos;
        }
        public HUD.PokemonHUD GetPokemonHUD(PBS.Battle.View.Compact.Pokemon pokemon)
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
        public void UpdatePokemonHUD(
            PBS.Battle.View.Compact.Pokemon pokemon,
            string nickname = "",
            PokemonGender gender = PokemonGender.Genderless,
            int level = 1,
            string nonVolatileStatusID = null,
            int currentHP = 1,
            int maxHP = 1)
        {
            HUD.PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
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

        // Drawing HUD
        public HUD.PokemonHUD DrawPokemonHUD(
            PBS.Battle.View.Compact.Pokemon pokemon,
            PBS.Battle.Enums.TeamMode teamMode,
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
                HUD.PokemonHUD pokemonHUD = Instantiate(pokemonHUDPrefab, spawnPos.position, Quaternion.identity, spawnPos);
                pokemonHUD.pokemonUniqueID = pokemon.uniqueID;
                pokemonHUD.hpObj.gameObject.SetActive(isNear
                    && (teamMode == Battle.Enums.TeamMode.Single
                        || teamMode == Battle.Enums.TeamMode.Double));
                // set EXP bar
                pokemonHUD.expObj.SetActive(isNear
                    && (teamMode == Battle.Enums.TeamMode.Single
                        || teamMode == Battle.Enums.TeamMode.Double));
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
        public bool UndrawPokemonHUD(PBS.Battle.View.Compact.Pokemon pokemon)
        {
            HUD.PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
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
            PBS.Battle.View.Compact.Pokemon pokemon,
            int preHP, 
            int postHP, 
            int maxHP,
            float timeSpan = 1f)
        {
            HUD.PokemonHUD pokemonHUD = GetPokemonHUD(pokemon);
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
    }
}


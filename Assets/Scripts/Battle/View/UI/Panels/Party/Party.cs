using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PBS.Battle.View.UI.Panels
{
    public class Party : BasePanel
    {
        [Header("Panels")]
        public GameObject commandPanel;

        [Header("Buttons")]
        public PartyButton party1Btn;
        public PartyButton party2Btn,
            party3Btn,
            party4Btn,
            party5Btn,
            party6Btn;
        public PartyButton backBtn;
        public BTLUI_Button summaryBtn,
            switchBtn,
            movesBtn,
            cancelBtn;

        [Header("Text")]
        public Text promptText;

        private void Awake()
        {
            backBtn.pokemonUniqueID = "";
        }

        // Party Screen
        public void SetParty(List<WifiFriendly.Pokemon> party)
        {
            for (int i = 0; i < party.Count; i++)
            {
                WifiFriendly.Pokemon pokemon = party[i];
                PartyButton curBtn = (i == 0) ? party1Btn
                    : (i == 1) ? party2Btn
                    : (i == 2) ? party3Btn
                    : (i == 3) ? party4Btn
                    : (i == 4) ? party5Btn
                    : (i == 5) ? party6Btn
                    : null;
                if (curBtn != null)
                {
                    SetPartyButton(pokemon, curBtn);
                }
            }
            if (party.Count < 6) party6Btn.gameObject.SetActive(false);
            if (party.Count < 5) party5Btn.gameObject.SetActive(false);
            if (party.Count < 4) party4Btn.gameObject.SetActive(false);
            if (party.Count < 3) party3Btn.gameObject.SetActive(false);
            if (party.Count < 2) party2Btn.gameObject.SetActive(false);
            if (party.Count < 1) party1Btn.gameObject.SetActive(false);
        }
        public void SetPartyButton(
            WifiFriendly.Pokemon pokemon, 

            PartyButton button)
        {
            button.nameTxt.text = pokemon.nickname;

            PokemonGender gender = pokemon.gender;
            if (gender != PokemonGender.Genderless)
            {
                button.nameTxt.text += (gender == PokemonGender.Male) ? " <color=#8080FF>♂</color>"
                    : (gender == PokemonGender.Female)? " <color=#FF8080>♀</color>"
                    : "";
            }

            button.lvlTxt.text = "Lv." + pokemon.level;
            button.hpTxt.text = pokemon.currentHP + "/" + pokemon.maxHP;
            button.statusTxt.text = (string.IsNullOrEmpty(pokemon.nonVolatileStatus)) ? ""
                : StatusPKDatabase.instance.GetStatusData(pokemon.nonVolatileStatus).shortName;

            // draw icon
            string drawPath = "pokemonSprites/icon/" + PokemonDatabase.instance.GetPokemonData(pokemon.pokemonID).displayID;
            button.icon.sprite = BattleAssetLoader.instance.nullPokemonIconSprite;
            if (BattleAssetLoader.instance.loadedPokemonSprites.ContainsKey(drawPath))
            {
                button.icon.sprite = BattleAssetLoader.instance.loadedPokemonSprites[drawPath];
            }
            else
            {
                StartCoroutine(BattleAssetLoader.instance.LoadPokemon(
                    pokemon: pokemon,
                    useicon: true,
                    imagePokemon: button.icon
                    ));
            }

            // HP Bar
            float hpPercent = ((float)pokemon.currentHP) / pokemon.maxHP;
            button.hpBar.fillAmount = hpPercent;
            button.hpBar.color = (hpPercent > 0.5f) ? button.hpHigh
                : (hpPercent > 0.25f) ? button.hpMed
                : button.hpLow;

            button.pokemonUniqueID = pokemon.uniqueID;
            button.UnselectSelf();
        }

        public void HighlightPokemon(string pokemonUniqueID)
        {
            PartyButton selectedBtn = null;

            if (party1Btn.pokemonUniqueID == pokemonUniqueID)
            {
                selectedBtn = party1Btn;
            }
            else
            {
                party1Btn.UnselectSelf();
            }

            if (party2Btn.pokemonUniqueID == pokemonUniqueID)
            {
                selectedBtn = party2Btn;
            }
            else
            {
                party2Btn.UnselectSelf();
            }

            if (party3Btn.pokemonUniqueID == pokemonUniqueID)
            {
                selectedBtn = party3Btn;
            }
            else
            {
                party3Btn.UnselectSelf();
            }

            if (party4Btn.pokemonUniqueID == pokemonUniqueID)
            {
                selectedBtn = party4Btn;
            }
            else
            {
                party4Btn.UnselectSelf();
            }

            if (party5Btn.pokemonUniqueID == pokemonUniqueID)
            {
                selectedBtn = party5Btn;
            }
            else
            {
                party5Btn.UnselectSelf();
            }

            if (party6Btn.pokemonUniqueID == pokemonUniqueID)
            {
                selectedBtn = party6Btn;
            }
            else
            {
                party6Btn.UnselectSelf();
            }

            if (selectedBtn != null)
            {
                selectedBtn.SelectSelf();
                backBtn.UnselectSelf();
                promptText.text = "Choose a Pokémon.";
            }
        }
        public void HighlightBackButton()
        {
            party1Btn.UnselectSelf();
            party2Btn.UnselectSelf();
            party3Btn.UnselectSelf();
            party4Btn.UnselectSelf();
            party5Btn.UnselectSelf();
            party6Btn.UnselectSelf();

            backBtn.SelectSelf();
            promptText.text = "Go back to commands.";
        }

        // Party Commands
        public void SetCommands(List<BattleExtraCommand> commandList)
        {
            HashSet<BattleExtraCommand> commandSet = new HashSet<BattleExtraCommand>(commandList);
            summaryBtn.gameObject.SetActive(commandSet.Contains(BattleExtraCommand.Summary));
            switchBtn.gameObject.SetActive(commandSet.Contains(BattleExtraCommand.Switch));
            movesBtn.gameObject.SetActive(commandSet.Contains(BattleExtraCommand.Moves));
            cancelBtn.gameObject.SetActive(commandSet.Contains(BattleExtraCommand.Cancel));
        }

        public void HighlightCommand(BattleExtraCommand commandType)
        {
            UnselectAllCommandButtons();
            switch (commandType)
            {
                case BattleExtraCommand.Summary:
                    summaryBtn.SelectSelf();
                    promptText.text = "View this Pokémon's stats.";
                    break;

                case BattleExtraCommand.Switch:
                    switchBtn.SelectSelf();
                    promptText.text = "Switch into this Pokémon.";
                    break;

                case BattleExtraCommand.Moves:
                    movesBtn.SelectSelf();
                    promptText.text = "View this Pokémon's moves.";
                    break;

                case BattleExtraCommand.Cancel:
                    cancelBtn.SelectSelf();
                    promptText.text = "Go back to party.";
                    break;

                default:
                    break;
            }
        }
        public void UnselectAllCommandButtons()
        {
            summaryBtn.UnselectSelf();
            switchBtn.UnselectSelf();
            movesBtn.UnselectSelf();
            cancelBtn.UnselectSelf();
        }
    }
}

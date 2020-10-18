using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BTLUIPanelParty : BTLUIPanel
{
    [Header("Panels")]
    public GameObject commandPanel;

    [Header("Buttons")]
    public BTLUI_ButtonParty party1Btn;
    public BTLUI_ButtonParty party2Btn,
        party3Btn,
        party4Btn,
        party5Btn,
        party6Btn;
    public BTLUI_Button backBtn;
    public BTLUI_Button summaryBtn,
        switchBtn,
        movesBtn,
        cancelBtn;

    [Header("Text")]
    public Text promptText;

    // Party Screen
    public void SetParty(List<Pokemon> party, Item item = null)
    {
        for (int i = 0; i < party.Count; i++)
        {
            Pokemon pokemon = party[i];
            BTLUI_ButtonParty curBtn = (i == 0) ? party1Btn
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
    public void SetPartyButton(Pokemon pokemon, BTLUI_ButtonParty button)
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
        button.statusTxt.text = (pokemon.nonVolatileStatus == null) ? ""
            : pokemon.nonVolatileStatus.data.shortName;

        // draw icon
        string drawPath = "pokemonSprites/icon/" + pokemon.data.displayID;
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
        float hpPercent = pokemon.HPPercent;
        button.hpBar.fillAmount = hpPercent;
        button.hpBar.color = (hpPercent > 0.5f) ? button.hpHigh
            : (hpPercent > 0.25f) ? button.hpMed
            : button.hpLow;

        button.pokemon = pokemon;
        button.UnselectSelf();
    }

    public void HighlightPokemon(Pokemon pokemon)
    {
        BTLUI_ButtonParty selectedBtn = null;

        if (party1Btn.pokemon != null)
        {
            if (party1Btn.pokemon == pokemon)
            {
                selectedBtn = party1Btn;
            }
            else
            {
                party1Btn.UnselectSelf();
            }
        }
        if (party2Btn.pokemon != null)
        {
            if (party2Btn.pokemon == pokemon)
            {
                selectedBtn = party2Btn;
            }
            else
            {
                party2Btn.UnselectSelf();
            }
        }
        if (party3Btn.pokemon != null)
        {
            if (party3Btn.pokemon == pokemon)
            {
                selectedBtn = party3Btn;
            }
            else
            {
                party3Btn.UnselectSelf();
            }
        }
        if (party4Btn.pokemon != null)
        {
            if (party4Btn.pokemon == pokemon)
            {
                selectedBtn = party4Btn;
            }
            else
            {
                party4Btn.UnselectSelf();
            }
        }
        if (party5Btn.pokemon != null)
        {
            if (party5Btn.pokemon == pokemon)
            {
                selectedBtn = party5Btn;
            }
            else
            {
                party5Btn.UnselectSelf();
            }
        }
        if (party6Btn.pokemon != null)
        {
            if (party6Btn.pokemon == pokemon)
            {
                selectedBtn = party6Btn;
            }
            else
            {
                party6Btn.UnselectSelf();
            }
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
        HighlightPokemon(null);
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

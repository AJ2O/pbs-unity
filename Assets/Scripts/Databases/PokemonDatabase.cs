using System.Collections.Generic;
using UnityEngine;

public class PokemonDatabase
{
    //create an object of SingleObject
    private static PokemonDatabase singleton = new PokemonDatabase();

    //make the constructor private so that this class cannot be
    //instantiated
    private PokemonDatabase() { }

    //Get the only object available
    public static PokemonDatabase instance
    {
        get
        {
            return singleton;
        }
        private set
        {
            singleton = value;
        }
    }

    // Database
    private Dictionary<string, PokemonData> database = new Dictionary<string, PokemonData> 
    {
        // Null / Missingno / Placeholder
        {"",
            new PokemonData(
                ID: ""
                ) },



        // --- GENERATION 1: KANTO ---

        {"bulbasaur", 
            new PokemonData(
                ID: "bulbasaur",
                speciesName: "Bulbasaur",
                pokedexNo: 1,
                height: 0.5f, weight: 5f,
                types: new List<string> { "grass", "poison" },
                abilities: new List<string> { "overgrow" },
                hiddenAbilities: new List<string> { "chlorophyll" },
                baseHP: 45, baseATK: 49, baseDEF: 49, baseSPA: 65, baseSPD: 65, baseSPE: 45
                ) },

        {"charmander",
            new PokemonData(
                ID: "charmander",
                speciesName: "Charmander",
                pokedexNo: 4,
                height: 0.5f, weight: 5f,
                types: new List<string> { "fire" },
                abilities: new List<string> { "blaze" },
                hiddenAbilities: new List<string> { "solarpower" },
                baseHP: 39, baseATK: 52, baseDEF: 43, baseSPA: 60, baseSPD: 50, baseSPE: 65
                ) },

        {"squirtle",
            new PokemonData(
                ID: "squirtle",
                speciesName: "Squirtle",
                pokedexNo: 7,
                height: 0.5f, weight: 5f,
                types: new List<string> { "water", },
                abilities: new List<string> { "torrent" },
                hiddenAbilities: new List<string> { "raindish" },
                baseHP: 44, baseATK: 48, baseDEF: 65, baseSPA: 50, baseSPD: 64, baseSPE: 43
                ) },

        {"blastoise",
            new PokemonData(
                ID: "blastoise",
                speciesName: "Blastoise",
                pokedexNo: 9, pokedexCategory: "Shellfish",
                height: 1.6f, weight: 85.5f,
                types: new List<string> { "water", },
                abilities: new List<string> { "torrent" },
                hiddenAbilities: new List<string> { "raindish" },
                baseHP: 79, baseATK: 83, baseDEF: 100, baseSPA: 85, baseSPD: 105, baseSPE: 78,
                tags: new PokemonTag[]
                {
                    PokemonTag.Starter,
                }
                ) },
        {"blastoise-mega",
            new PokemonData(
                ID: "blastoise-mega",
                baseID: "blastoise",
                formName: "Mega Blastoise",
                useBaseMaleRatio: true,
                height: 1.6f, weight: 101.1f,
                types: new List<string> { "water", },
                abilities: new List<string> { "megalauncher" },
                hiddenAbilities: new List<string> { "megalauncher" },
                baseHP: 79, baseATK: 103, baseDEF: 120, baseSPA: 135, baseSPD: 115, baseSPE: 78,
                combineBaseTags: true,
                tags: new PokemonTag[]
                {
                    PokemonTag.IsMegaPokemon,
                }
                ) },
        {"blastoise-gmax",
            PokemonData.AestheticVariant(
                ID: "blastoise-gmax",
                baseID: "blastoise",
                formName: "Gigantamax Blastoise",
                height: 25.0f, weight: 9999f,
                tags: new PokemonTag[]
                {
                    PokemonTag.IsGigantamaxPokemon,
                }
                ) },

        {"pikachu",
            new PokemonData(
                ID: "pikachu",
                speciesName: "Pikachu",
                pokedexNo: 25,
                height: 0.5f, weight: 5f,
                types: new List<string> { "electric", },
                abilities: new List<string> { "static" },
                hiddenAbilities: new List<string> { "lightningrod" },
                baseHP: 35, baseATK: 55, baseDEF: 40, baseSPA: 50, baseSPD: 50, baseSPE: 90
                ) },


        // --- GENERATION 2: JOHTO ---





        // --- GENERATION 3: HOENN ---





        // --- GENERATION 4: SINNOH ---

        {"cresselia",
            new PokemonData(
                ID: "cresselia",
                speciesName: "Cresselia",
                pokedexNo: 488, pokedexCategory: "Lunar",
                maleRatio: 0,
                height: 1.5f, weight: 85.6f,
                yOffset2DNear: 0.25f, yOffset2DFar: 0.25f,
                types: new List<string> { "psychic" },
                abilities: new List<string> { "levitate" },
                baseHP: 120, baseATK: 70, baseDEF: 120, baseSPA: 75, baseSPD: 130, baseSPE: 85,
                tags: new PokemonTag[]
                {
                    PokemonTag.LunarDuo,
                    PokemonTag.Legendary,
                }
                ) },

        {"darkrai",
            new PokemonData(
                ID: "darkrai",
                speciesName: "Darkrai",
                pokedexNo: 491, pokedexCategory: "Pitch-Black",
                maleRatio: -1,
                height: 1.5f, weight: 50.5f,
                yOffset2DNear: 0.25f, yOffset2DFar: 0.25f,
                types: new List<string> { "dark" },
                abilities: new List<string> { "baddreams" },
                baseHP: 70, baseATK: 90, baseDEF: 90, baseSPA: 135, baseSPD: 90, baseSPE: 125,
                tags: new PokemonTag[]
                {
                    PokemonTag.LunarDuo,
                    PokemonTag.Mythical,
                }
                ) },

        // Arceus
        {"arceus",
            new PokemonData(
                ID: "arceus",
                speciesName: "Arceus",
                pokedexNo: 493, pokedexCategory: "Alpha",
                formName: "Normal-Type",
                maleRatio: -1,
                height: 3.2f, weight: 320f,
                types: new List<string> { "normal" },
                abilities: new List<string> { "multitype" },
                baseHP: 120, baseATK: 120, baseDEF: 120, baseSPA: 120, baseSPD: 120, baseSPE: 120,
                tags: new PokemonTag[]
                {
                    PokemonTag.Mythical,
                }
                ) },
        {"arceus-bug",
            new PokemonData(
                ID: "arceus-bug",
                baseID: "arceus",
                formName: "Bug-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "bug" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-dark",
            new PokemonData(
                ID: "arceus-dark",
                baseID: "arceus",
                formName: "Dark-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "dark" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-dragon",
            new PokemonData(
                ID: "arceus-dragon",
                baseID: "arceus",
                formName: "Dragon-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "dragon" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-electric",
            new PokemonData(
                ID: "arceus-electric",
                baseID: "arceus",
                formName: "Electric-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "electric" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-fairy",
            new PokemonData(
                ID: "arceus-fairy",
                baseID: "arceus",
                formName: "Fairy-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "fairy" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-fighting",
            new PokemonData(
                ID: "arceus-fighting",
                baseID: "arceus",
                formName: "Fighting-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "fighting" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-fire",
            new PokemonData(
                ID: "arceus-fire",
                baseID: "arceus",
                formName: "Fire-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "fire" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-flying",
            new PokemonData(
                ID: "arceus-flying",
                baseID: "arceus",
                formName: "Flying-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "flying" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-ghost",
            new PokemonData(
                ID: "arceus-ghost",
                baseID: "arceus",
                formName: "Ghost-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "ghost" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-grass",
            new PokemonData(
                ID: "arceus-grass",
                baseID: "arceus",
                formName: "Grass-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "grass" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-ground",
            new PokemonData(
                ID: "arceus-ground",
                baseID: "arceus",
                formName: "Ground-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "ground" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-ice",
            new PokemonData(
                ID: "arceus-ice",
                baseID: "arceus",
                formName: "Ice-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "ice" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-poison",
            new PokemonData(
                ID: "arceus-poison",
                baseID: "arceus",
                formName: "Poison-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "poison" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-psychic",
            new PokemonData(
                ID: "arceus-psychic",
                baseID: "arceus",
                formName: "Psychic-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "psychic" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-rock",
            new PokemonData(
                ID: "arceus-rock",
                baseID: "arceus",
                formName: "Rock-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "rock" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-steel",
            new PokemonData(
                ID: "arceus-steel",
                baseID: "arceus",
                formName: "Steel-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "steel" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },
        {"arceus-water",
            new PokemonData(
                ID: "arceus-water",
                baseID: "arceus",
                formName: "Water-Type",
                useBaseMaleRatio: true,
                types: new List<string> { "water" },
                useBaseAbilities: true, useBaseHiddenAbilities: true, useBaseBaseStats: true,
                combineBaseTags: true
                ) },



        // --- GENERATION 5: UNOVA ---

        {"meloetta-aria",
            new PokemonData(
                ID: "meloetta-aria",
                speciesName: "Meleotta",
                pokedexNo: 648,
                formName: "Aria Forme",
                maleRatio: -1,
                height: 0.6f, weight: 6.5f,
                yOffset2DNear: 0.5f, yOffset2DFar: 0.5f,
                types: new List<string> { "normal", "psychic" },
                abilities: new List<string> { "serenegrace" },
                baseHP: 100, baseATK: 77, baseDEF: 77, baseSPA: 128, baseSPD: 128, baseSPE: 90,
                tags: new PokemonTag[]
                {
                    PokemonTag.Mythical,
                }
                ) },
        {"meloetta-pirouette",
            new PokemonData(
                ID: "meloetta-pirouette",
                baseID: "meloetta-aria",
                formName: "Pirouette Forme",
                useBaseMaleRatio: true,
                types: new List<string> { "normal", "fighting" },
                useBaseAbilities: true, useBaseHiddenAbilities: true,
                baseHP: 100, baseATK: 128, baseDEF: 90, baseSPA: 77, baseSPD: 77, baseSPE: 128,
                combineBaseTags: true,
                tags: new PokemonTag[]
                {
                    PokemonTag.RevertOnBattleEnd
                }
                ) },


        // --- GENERATION 6: KALOS ---

        {"greninja",
            new PokemonData(
                ID: "greninja",
                speciesName: "Greninja",
                pokedexNo: 658, pokedexCategory: "Ninja",
                height: 1.5f, weight: 40.0f,
                types: new List<string> { "water", "dark" },
                abilities: new List<string> { "torrent" },
                hiddenAbilities: new List<string> { "protean" },
                baseHP: 72, baseATK: 95, baseDEF: 67, baseSPA: 103, baseSPD: 71, baseSPE: 122,
                tags: new PokemonTag[]
                {
                    PokemonTag.Starter,
                }
                ) },
        {"greninja-battlebond",
            new PokemonData(
                ID: "greninja-battlebond",
                baseID: "greninja",
                abilities: new List<string> { "battlebond" },
                useBaseAesthetic: true, useBaseTypes: true, useBaseBaseStats: true, 
                combineBaseTags: true
                ) },
        {"greninja-ash",
            new PokemonData(
                ID: "greninja-ash",
                baseID: "greninja-battlebond",
                formName: "Ash-Greninja",
                baseHP: 72, baseATK: 145, baseDEF: 67, baseSPA: 153, baseSPD: 71, baseSPE: 132,
                useBaseAbilities: true, useBaseTypes: true,
                combineBaseTags: true
                ) },

        {"hoopa-confined",
            new PokemonData(
                ID: "hoopa-confined",
                speciesName: "Hoopa",
                pokedexNo: 720,
                formName: "Hoopa Confined",
                maleRatio: -1,
                height: 0.5f, weight: 9f,
                yOffset2DNear: 0.5f, yOffset2DFar: 0.5f,
                types: new List<string> { "psychic", "ghost" },
                abilities: new List<string> { "intimidate" },
                baseHP: 80, baseATK: 110, baseDEF: 60, baseSPA: 150, baseSPD: 130, baseSPE: 70,
                tags: new PokemonTag[]
                {
                    PokemonTag.Mythical,
                }
                ) },
        {"hoopa-unbound",
            new PokemonData(
                ID: "hoopa-unbound",
                baseID: "hoopa-confined",
                formName: "Hoopa Unbound",
                useBaseMaleRatio: true,
                height: 6.5f, weight: 490f,
                yOffset2DNear: 0.5f, yOffset2DFar: 0.5f,
                types: new List<string> { "psychic", "dark" },
                useBaseAbilities: true,
                useBaseHiddenAbilities: true,
                baseHP: 80, baseATK: 160, baseDEF: 60, baseSPA: 170, baseSPD: 130, baseSPE: 80,
                combineBaseTags: true
                ) },


        // --- GENERATION 7: ALOLA ---

        {"mimikyu",
            new PokemonData(
                ID: "mimikyu",
                speciesName: "Mimikyu",
                pokedexNo: 778, pokedexCategory: "Disguise",
                formName: "Disguised Form",
                height: 0.2f, weight: 0.7f,
                types: new List<string> { "ghost", "fairy" },
                abilities: new List<string> { "disguise" },
                baseHP: 55, baseATK: 90, baseDEF: 80, baseSPA: 50, baseSPD: 105, baseSPE: 96
                ) },
        {"mimikyu-busted",
            PokemonData.AestheticVariant(
                ID: "mimikyu-busted",
                baseID: "mimikyu",
                formName: "Busted Form",
                tags: new PokemonTag[]
                {
                    PokemonTag.RevertOnBattleEnd
                }
                ) },

        // Guardian Deities
        {"tapukoko",
            new PokemonData(
                ID: "tapukoko",
                speciesName: "Tapu Koko",
                pokedexNo: 785, pokedexCategory: "Land Spirit",
                maleRatio: -1,
                height: 1.8f, weight: 20.5f,
                yOffset2DNear: 0.25f, yOffset2DFar: 0.25f,
                types: new List<string> { "electric", "fairy" },
                abilities: new List<string> { "deltastream" },
                baseHP: 70, baseATK: 115, baseDEF: 85, baseSPA: 95, baseSPD: 75, baseSPE: 130,
                tags: new PokemonTag[]
                {
                    PokemonTag.GuardianDeity,
                    PokemonTag.Legendary,
                }
                ) },
        {"tapulele",
            new PokemonData(
                ID: "tapulele",
                speciesName: "Tapu Lele",
                pokedexNo: 786, pokedexCategory: "Land Spirit",
                maleRatio: -1,
                height: 1.2f, weight: 18.6f,
                yOffset2DNear: 0.25f, yOffset2DFar: 0.25f,
                types: new List<string> { "psychic", "fairy" },
                abilities: new List<string> { "psychicsurge" },
                baseHP: 70, baseATK: 85, baseDEF: 75, baseSPA: 130, baseSPD: 115, baseSPE: 95,
                tags: new PokemonTag[]
                {
                    PokemonTag.GuardianDeity,
                    PokemonTag.Legendary,
                }
                ) },
        {"tapubulu",
            new PokemonData(
                ID: "tapubulu",
                speciesName: "Tapu Bulu",
                pokedexNo: 787, pokedexCategory: "Land Spirit",
                maleRatio: -1,
                height: 1.9f, weight: 45.5f,
                yOffset2DNear: 0.25f, yOffset2DFar: 0.25f,
                types: new List<string> { "grass", "fairy" },
                abilities: new List<string> { "grassysurge" },
                baseHP: 70, baseATK: 130, baseDEF: 115, baseSPA: 85, baseSPD: 95, baseSPE: 75,
                tags: new PokemonTag[]
                {
                    PokemonTag.GuardianDeity,
                    PokemonTag.Legendary,
                }
                ) },
        {"tapufini",
            new PokemonData(
                ID: "tapufini",
                speciesName: "Tapu Fini",
                pokedexNo: 788, pokedexCategory: "Land Spirit",
                maleRatio: -1,
                height: 1.3f, weight: 21.2f,
                yOffset2DNear: 0.25f, yOffset2DFar: 0.25f,
                types: new List<string> { "water", "fairy" },
                abilities: new List<string> { "mistysurge" },
                baseHP: 70, baseATK: 75, baseDEF: 115, baseSPA: 95, baseSPD: 130, baseSPE: 85,
                tags: new PokemonTag[]
                {
                    PokemonTag.GuardianDeity,
                    PokemonTag.Legendary,
                }
                ) },




        // --- GENERATION 8: GALAR ---

        {"cramorant",
            new PokemonData(
                ID: "cramorant",
                speciesName: "Cramorant",
                pokedexNo: 845, pokedexCategory: "Gulp",
                height: 0.8f, weight: 18f,
                types: new List<string> { "flying", "water" },
                abilities: new List<string> { "gulpmissile" },
                baseHP: 70, baseATK: 85, baseDEF: 55, baseSPA: 85, baseSPD: 95, baseSPE: 85
                ) },
        {"cramorant-gulping",
            PokemonData.AestheticVariant(
                ID: "cramorant-gulping",
                baseID: "cramorant",
                formName: "Gulping Form",
                tags: new PokemonTag[]
                {
                    PokemonTag.RevertOnBattleEnd,
                    PokemonTag.RevertOnFaint,
                    PokemonTag.RevertOnSwitchOut,
                }
                ) },
        {"cramorant-gorging",
            PokemonData.AestheticVariant(
                ID: "cramorant-gorging",
                baseID: "cramorant",
                formName: "Gorging Form",
                tags: new PokemonTag[]
                {
                    PokemonTag.RevertOnBattleEnd,
                    PokemonTag.RevertOnFaint,
                    PokemonTag.RevertOnSwitchOut,
                }
                ) },

        {"eiscue",
            new PokemonData(
                ID: "eiscue",
                speciesName: "Eiscue",
                pokedexNo: 875, pokedexCategory: "Penguin",
                formName: "Ice Face",
                height: 1.4f, weight: 89f,
                types: new List<string> { "ice" },
                abilities: new List<string> { "iceface" },
                baseHP: 75, baseATK: 80, baseDEF: 110, baseSPA: 65, baseSPD: 90, baseSPE: 50
                ) },
        {"eiscue-noice",
            new PokemonData(
                ID: "eiscue-noice",
                baseID: "eiscue",
                formName: "Noice Face",
                baseHP: 75, baseATK: 80, baseDEF: 70, baseSPA: 65, baseSPD: 50, baseSPE: 130,
                useBaseAbilities: true, useBaseTypes: true,
                combineBaseTags: true,
                tags: new PokemonTag[]
                {
                    PokemonTag.RevertOnBattleEnd,
                }
                ) },

        {"morpeko",
            new PokemonData(
                ID: "morpeko",
                speciesName: "Morpeko",
                pokedexNo: 877,
                formName: "Full Belly Mode",
                maleRatio: 0.5f,
                height: 0.3f, weight: 3f,
                types: new List<string> { "electric", "dark" },
                abilities: new List<string> { "hungerswitch" },
                baseHP: 58, baseATK: 95, baseDEF: 58, baseSPA: 78, baseSPD: 58, baseSPE: 97
                ) },
        {"morpeko-hangry",
            PokemonData.AestheticVariant(
                ID: "morpeko-hangry",
                baseID: "morpeko",
                formName: "Hangry Mode",
                tags: new PokemonTag[]
                {
                    PokemonTag.RevertOnBattleEnd,
                    PokemonTag.RevertOnFaint
                }
                ) },

    }; 

    // Methods
    public PokemonData GetPokemonData(string ID)
    {
        if (database.ContainsKey(ID))
        {
            return database[ID];
        }
        Debug.LogWarning("Could not find Pokemon with ID: " + ID);
        return database[""];
    }
    public PokemonData GetPokemonIllusionData(Pokemon.BattleProperties.Illusion illusion)
    {
        if (database.ContainsKey(illusion.pokemonID))
        {
            return database[illusion.pokemonID];
        }
        Debug.LogWarning("Could not find Illusion appearance with Pokemon ID: " + illusion.pokemonID);
        return database[""];
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings
{
    public static GameLanguages language
    {
        get
        {
            return GameLanguages.English;
        }
        private set { }
    }

    // Pokemon
    public const int pkmnMaxEVValue = 252;
    public const int pkmnMaxEVTotal = 510;
    public const int pkmnMaxIVValue = 31;
    public const int pkmnMaxLevel = 100;
    public const int pkmnMaxMoveCount = 4;
    public const int pkmnMaxPPUps = 3;
    public const float pkmnDynamaxHPLevelBoost = 0.05f;
    public const int pkmnDynamaxTurns = 2;

    // Trainer
    public const int trnMaxPartyCount = 6;

    // Battle
    public const float btlSTABMultiplier = 1.5f;
    public const float btlCriticalHitMultiplier = 1.5f;
    public const float btlSuperEffectivenessMultiplier = 2f;
    public const float btlNotVeryEffectivenessMultiplier = 0.5f;
    public const float btlImmuneEffectivenessMultiplier = 0f;

    public const int btlStatStageMin = -6;
    public const int btlStatStageMax = 6;
    public static int btlMaxStatBoost 
    { 
        get
        {
            return btlStatStageMax - btlStatStageMin;
        } 
    }
    public static int btlMinStatBoost
    {
        get
        {
            return btlStatStageMin - btlStatStageMax;
        }
    }
    public static List<PokemonStats> btlPkmnStats 
    { 
        get 
        {
            return new List<PokemonStats>
            {
                PokemonStats.Attack,
                PokemonStats.Defense,
                PokemonStats.SpecialAttack,
                PokemonStats.SpecialDefense,
                PokemonStats.Speed,
                PokemonStats.Accuracy,
                PokemonStats.Evasion
            };
        } 
    }

    public static int GetMaxStatBoost()
    {
        return btlStatStageMax - btlStatStageMin;
    }
    public static int GetMinStatBoost()
    {
        return btlStatStageMin - btlStatStageMax;
    }
}

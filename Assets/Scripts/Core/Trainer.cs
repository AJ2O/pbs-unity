﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer
{
    // General
    public string name { get; set; }
    public int teamPos { get; set; }
    public bool isWildPokemon { get; set; }
    public Item megaRing { get; set; }
    public Item ZRing { get; set; }
    public Item dynamaxBand { get; set; }

    // Team
    public List<Pokemon> party { get; set; }
    public List<Item> items { get; set; }
    public int[] controlPos { get; set; }

    // Network
    public int playerID = -1;

    // Battle-only Properties
    public TrainerBattleProperties bProps { get; set; }

    // Constructor
    public Trainer(
        string name = "",
        int playerID = -1,
        bool isWildPokemon = false,
        IEnumerable<Pokemon> party=null,
        IEnumerable<Item> items=null,
        
        Item megaRing = null,
        Item ZRing = null,
        Item dynamaxBand = null)
    {
        this.name = name;
        this.playerID = playerID;

        this.party = new List<Pokemon>();
        if (party != null)
        {
            this.party.AddRange(party);
        }

        this.items = new List<Item>();
        if (items != null)
        {
            this.items.AddRange(items);
        }

        this.megaRing = (megaRing == null) ? null : megaRing.Clone();
        this.ZRing = (ZRing == null) ? null : ZRing.Clone();
        this.dynamaxBand = (dynamaxBand == null) ? null : dynamaxBand.Clone();

        // battle
        bProps = new TrainerBattleProperties(this);
    }

    // Clone
    public Trainer Clone()
    {
        List<Pokemon> partyPokemon = new List<Pokemon>();
        for (int i = 0; i < party.Count; i++)
        {
            partyPokemon.Add(Pokemon.Clone(party[i]));
        }

        List<Item> trainerItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            trainerItems.Add(items[i].Clone());
        }

        Trainer cloneTrainer = new Trainer(
            name: name,
            playerID: playerID,
            isWildPokemon: isWildPokemon,
            party: partyPokemon,
            items: trainerItems,

            megaRing: megaRing,
            ZRing: ZRing,
            dynamaxBand: dynamaxBand
            );

        // battle
        cloneTrainer.teamPos = teamPos;
        cloneTrainer.controlPos = controlPos;
        cloneTrainer.bProps = bProps.Clone(cloneTrainer);
        return cloneTrainer;
    }

    // General Methods
    public bool IsTheSameAs(Trainer trainer)
    {
        return playerID == trainer.playerID;
    }
    public bool IsAIControlled()
    {
        return playerID < 0;
    }

    // Party Methods
    /// <summary>
    /// Returns true if the given pokemon is in this trainer's party.
    /// </summary>
    /// <param name="pokemon"></param>
    /// <returns></returns>
    public bool HasPokemon(Pokemon pokemon)
    {
        for (int i = 0; i < party.Count; i++)
        {
            if (party[i].IsTheSameAs(pokemon))
            {
                return true;
            }
        }
        return false;
    }
    public int GetPartyIndexOfPokemon(Pokemon pokemon)
    {
        for (int i = 0; i < this.party.Count; i++)
        {
            if (this.party[i].IsTheSameAs(pokemon))
            {
                return i;
            }
        }
        Debug.LogWarning("Cannot find " + pokemon.nickname + " in the party");
        return -1;
    }

    public bool SwapPartyPokemon(Pokemon fromPokemon, Pokemon toPokemon)
    {
        int fromIndex = GetPartyIndexOfPokemon(fromPokemon);
        int toIndex = GetPartyIndexOfPokemon(toPokemon);

        if (fromIndex == -1 || toIndex == -1)
        {
            return false;
        }
        this.party[fromIndex] = toPokemon;
        this.party[toIndex] = fromPokemon;
        return true;
    }

    // Item Methods
    public bool HasItem(string itemID)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemID == itemID)
            {
                return true;
            }
        }
        return false;
    }
    public int GetItemCount(string itemID)
    {
        int count = 0;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemID == itemID)
            {
                count++;
            }
        }
        return count;
    }
    public void RemoveItem(string itemID, int removeCount = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemID == itemID)
            {
                items.RemoveAt(i);
                if (removeCount > 0)
                {
                    removeCount -= 1;
                    if (removeCount == 0)
                    {
                        return;
                    }
                }
                i--;
            }
        }
    }
    public List<Item> GetItemsByPocket(ItemPocket pocket)
    {
        List<Item> pocketItems = new List<Item>();
        HashSet<string> accountedItems = new HashSet<string>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].data.pocket == pocket
                && !accountedItems.Contains(items[i].itemID))
            {
                pocketItems.Add(items[i]);
            }
        }
        return pocketItems;
    }
    public List<Item> GetItemsByBattlePocket(ItemBattlePocket pocket)
    {
        List<Item> pocketItems = new List<Item>();
        HashSet<string> accountedItems = new HashSet<string>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].data.battlePocket == pocket
                && !accountedItems.Contains(items[i].itemID))
            {
                pocketItems.Add(items[i]);
                accountedItems.Add(items[i].itemID);
            }
        }
        return pocketItems;
    }

}

public class TrainerBattleProperties
{
    public bool usedMegaEvolution { get; set; }
    public bool usedZMove { get; set; }
    public bool usedDynamax { get; set; }

    public bool usedBallFetch { get; set; }

    public Item failedPokeball { get; set; }
    public int payDayMoney { get; set; }

    // Constructor
    public TrainerBattleProperties(Trainer trainer)
    {
        Reset(trainer);
    }

    // Clone
    public TrainerBattleProperties Clone(Trainer original)
    {
        TrainerBattleProperties clone = new TrainerBattleProperties(original);
        clone.usedMegaEvolution = usedMegaEvolution;
        clone.usedZMove = usedZMove;
        clone.usedDynamax = usedDynamax;

        clone.usedBallFetch = usedBallFetch;

        clone.failedPokeball = (failedPokeball == null) ? null : failedPokeball.Clone();
        clone.payDayMoney = payDayMoney;

        return clone;
    }

    public void Reset(Trainer trainer)
    {
        usedMegaEvolution = false;
        usedZMove = false;
        usedDynamax = false;

        usedBallFetch = false;

        failedPokeball = null;
        payDayMoney = 0;
    }
}

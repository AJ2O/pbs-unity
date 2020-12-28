using PBS.Main.Pokemon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSave
{
    //create an object of SingleObject
    private static PlayerSave playerSave = new PlayerSave();

    //make the constructor private so that this class cannot be
    //instantiated
    private PlayerSave() { }

    //Get the only object available
    public static PlayerSave instance { 
        get { 
            if (!playerSave.isInitialized)
            {
                CreateDefaultSave();
            }
            return playerSave; 
        }
        private set { 
            playerSave = value; 
        } 
    }

    // Properties
    public bool isInitialized { get; private set; }
    public string name { get; set; }
    public List<Pokemon> party { get; set; }
    public List<Item> items { get; set; }
    private static void CreateDefaultSave()
    {
        PlayerSave playerSave = new PlayerSave();
        playerSave.isInitialized = true;
        playerSave.name = "44DS";

        // Party
        playerSave.party = new List<Pokemon>();
        
        Pokemon pokemon1 = new Pokemon(
            pokemonID: "bulbasaur",
            level: 15,
            natureID: "hardy",
            moveslots: new Moveslot[]
            {
                new Moveslot("tackle"),
                new Moveslot("leechseed"),
                new Moveslot("razorleaf"),
                new Moveslot("growl")
            }
            );

        Pokemon pokemon2 = new Pokemon(
            pokemonID: "charmander",
            level: 10,
            currentHP: 14,
            natureID: "adamant",
            moveslots: new Moveslot[]
            {
                new Moveslot("ember")
            }
            );

        Pokemon pokemon3 = new Pokemon(
            pokemonID: "squirtle",
            level: 19,
            currentHP: 10,
            natureID: "sassy",
            moveslots: new Moveslot[]
            {
                new Moveslot("watergun"),
                new Moveslot("withdraw"),
                new Moveslot("watergun")
            }
            );

        playerSave.party.AddRange(new List<Pokemon> { pokemon1, pokemon2, pokemon3 });

        // Items
        playerSave.items = new List<Item>();

        Item item1 = new Item("potion");
        Item item2 = new Item("potion");
        Item item3 = new Item("potion");
        Item item4 = new Item("antidote");
        Item item5 = new Item("oranberry");
        Item item6 = new Item("xattack");

        playerSave.items.AddRange(new List<Item> { item1, item2, item3, item4, item5, item6 });

        instance = playerSave;
        Debug.Log("Default Save Created");
    } 

    public Trainer GetTrainer()
    {
        Trainer trainer = new Trainer(
            name: name,
            party: party,
            items: items
            );
        return trainer;
    }
}
